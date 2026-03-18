using Microsoft.Win32;
using Reversi.Model;
using Reversi.Presistence;
using Reversi_WPF.View;
using Reversi_WPF.ViewModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Reversi_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IDisposable
    {

        #region Fields

        private ReversiGameModel _model = null!;
        private ReversiViewModel _viewModel = null!;
        private MainWindow _view = null!;

        #endregion

        #region Constructors

        /// <summary>
        /// Alkalmazás példányosítása.
        /// </summary>
        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        #endregion

        #region Application event handlers

        /// <summary>
        /// Az alkalmazás inicializálása.
        /// </summary>
        private void App_Startup(object? sender, StartupEventArgs e)
        {
            IReversiDataAccess _dataAccess = new ReversiFileDataAccess();
            _model = new ReversiGameModel(_dataAccess);
            _model.SetGameEnded += new EventHandler<ReversiSetGameEndedEventArgs>(Model_SetGameEnded);
            
            _viewModel = new ReversiViewModel(_model);
            _viewModel.NewGame += new EventHandler(ViewModel_NewGame);
            _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
            _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);
            _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);
            _viewModel.Rules += new EventHandler(ViewModel_ReadRules);

            _view = new MainWindow();
            _view.DataContext = _viewModel;
            _view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing); 
            _view.Show();
        }

        #endregion

        #region View event handlers

        /// <summary>
        /// Nézet bezárásának eseménykezelője.
        /// </summary>
        private void View_Closing(object? sender, CancelEventArgs e)
        {
            _model.Pause();
            if (MessageBox.Show(_view, "Are you sure, you want to exit without save?", "Reversi", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true; 
                _model.Unpause();
            }
            
        }

        #endregion

        #region ViewModel event handlers
        /// <summary>
        /// Új játék indításának eseménykezelője.
        /// </summary>
        private void ViewModel_NewGame(object? sender, EventArgs e)
        {
            _model.Pause();
            _model.NewGame();
            _model.Unpause();
            _viewModel.Update();
        }
        /// <summary>
        /// Játék betöltésének eseménykezelője.
        /// </summary>
        private async void ViewModel_LoadGame(object? sender, EventArgs e)
        {
            _model.Pause();

            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Loading Reversi game";
                openFileDialog.Filter = "Reversi game|*.reversi";
                if (openFileDialog.ShowDialog() == true)
                {
                    await _model.LoadGame(openFileDialog.FileName);
                    _viewModel.Saved = false;
                }
            }
            catch (ReversiDataException)
            {
                MessageBox.Show("The file loading failed!", "Reversi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            _viewModel.Update();
            _model.Unpause();
        }
        /// <summary>
        /// Játék mentésének eseménykezelője.
        /// </summary>
        private async void ViewModel_SaveGame(object? sender, EventArgs e)
        {
            _model.Pause();

            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Saving Reversi game";
                saveFileDialog.Filter = "Reversi game|*.reversi";
                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        await _model.SaveGame(saveFileDialog.FileName);
                        _viewModel.Saved = true;
                    }
                    catch (ReversiDataException)
                    {
                        MessageBox.Show("Game saving failed!" + Environment.NewLine + "The path is invalid, or the directory is not writable.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Failed to save the file!", "Reversi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            _model.Unpause();
        }
        /// <summary>
        /// Játékból való kilépés eseménykezelője.
        /// </summary>
        private void ViewModel_ExitGame(object? sender, EventArgs e)
        {
            _view.Close();
        }
        /// <summary>
        /// A Játékszabály megjelenítésének eseménykezelője.
        /// </summary>
        private void ViewModel_ReadRules(object? sender, EventArgs e)
        {
            _model.Pause();

            MessageBox.Show(_view, "Each player has a color, and the aim of the game is to get more of your pieces on the board than the opponent's pieces. " + Environment.NewLine +
                "At the start of the game there are four pieces on the board, two white and two black. You must try to capture opponent pieces and flip them" +
                "over so they turn into your color. You do this by making a horizontal, vertical or diagonal line of pieces, where your pieces surround the other player's pieces. " +
                "The surrounded opponent pieces are then captured and will be flipped over to your color, increasing the number of your pieces on the board." + Environment.NewLine +
                "Every move you make must capture some opponent pieces. This means that every time it's your turn you must place one of your pieces next to a line of one or more opponent pieces," +
                " where there's another one of your pieces at the other end of the line. " + Environment.NewLine + "If there is no available move on the board that captures pieces then you must say Pass " +
                "and your opponent gets to play again. If both players say pass in a row then there are no more moves on the board and the game ends. ");
            _model.Unpause();
        }

        #endregion

        #region Model event handlers

        /// <summary>
        /// Játék végének eseménykezelője.
        /// </summary>
        private void Model_SetGameEnded(object? sender, ReversiSetGameEndedEventArgs e)
        {
            _viewModel.PassButtonEnabled = false;
            _viewModel.PauseButtonEnabled = false;
            _viewModel.Saved = true;
            _model.UpdateView();
            if (e.PlayerWhitePoints > e.PlayerBlackPoints)
            {
                MessageBox.Show("Player White won." + Environment.NewLine + "Player White points: " + e.PlayerWhitePoints.ToString() + Environment.NewLine + "Player Black points: " + e.PlayerBlackPoints.ToString() + Environment.NewLine + "Game Ended!");
            }
            else if (e.PlayerWhitePoints < e.PlayerBlackPoints)
            {
                MessageBox.Show("Player Black won." + Environment.NewLine + "Player White points: " + e.PlayerWhitePoints.ToString() + Environment.NewLine + "Player Black points: " + e.PlayerBlackPoints.ToString() + Environment.NewLine + "Game Ended!");
            }
            else
            {
                MessageBox.Show("It is a tie." + Environment.NewLine + "Player White points: " + e.PlayerWhitePoints.ToString() + Environment.NewLine + "Player Black points: " + e.PlayerBlackPoints.ToString() + Environment.NewLine + "Game Ended!");
            }

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
