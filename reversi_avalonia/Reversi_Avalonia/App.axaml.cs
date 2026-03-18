using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Reversi_Avalonia.ViewModels;
using Reversi_Avalonia.Views;
using System;
using Reversi.Presistence;
using Reversi.Model;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System.IO;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Platform;

namespace Reversi_Avalonia
{
    public partial class App : Application, IDisposable
    {
        #region Fields

        private ReversiGameModel _model = null!;
        private ReversiViewModel _viewModel = null!;
        //private MainWindow _view = null!;

        #endregion

        #region Properites

        private TopLevel? TopLevel => ApplicationLifetime switch
        {
            IClassicDesktopStyleApplicationLifetime desktop => TopLevel.GetTopLevel(desktop.MainWindow),
            ISingleViewApplicationLifetime singleViewPlatform => TopLevel.GetTopLevel(singleViewPlatform.MainView),
            _ => null
        };

        #endregion


        #region Application methods

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            BindingPlugins.DataValidators.RemoveAt(0);

            IReversiDataAccess dataAccess = new ReversiFileDataAccess();
            _model = new ReversiGameModel(dataAccess);
            _model.SetGameEnded += new EventHandler<ReversiSetGameEndedEventArgs>(Model_SetGameEnded);
            _model.NewGame();

            _viewModel = new ReversiViewModel(_model);
            _viewModel.NewGame += new EventHandler(ViewModel_NewGame);
            _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
            _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);
            _viewModel.Rules += new EventHandler(ViewModel_ReadRules);

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // asztali környezethez
                desktop.MainWindow = new MainWindow
                {
                    DataContext = _viewModel
                };

                desktop.Startup += async (s, e) =>
                {
                    _model.NewGame(); // indításkor új játékot kezdünk

                    // betöltjük a felfüggesztett játékot, amennyiben van
                    try
                    {
                        await _model.LoadGame(
                            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ReversiSuspendedGame"));
                    }
                    catch { }
                };

                desktop.Exit += async (s, e) =>
                {

                    // elmentjük a jelenleg folyó játékot
                    try
                    {
                        await _model.SaveGame(
                            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ReversiSuspendedGame"));
                        // mentés a felhasználó Documents könyvtárába, oda minden bizonnyal van jogunk írni
                    }
                    catch { }
                };
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                // mobil környezethez
                singleViewPlatform.MainView = new MainView
                {
                    DataContext = _viewModel
                };
                if (Application.Current?.TryGetFeature<IActivatableLifetime>() is { } activatableLifetime)
                {
                    activatableLifetime.Activated += async (sender, args) =>
                    {
                        if (args.Kind == ActivationKind.Background)
                        {
                            // betöltjük a felfüggesztett játékot, amennyiben van
                            try
                            {
                                await _model.LoadGame(
                                    Path.Combine(AppContext.BaseDirectory, "SuspendedGame"));
                            }
                            catch
                            {
                            }
                        }
                    };
                    activatableLifetime.Deactivated += async (sender, args) =>
                    {
                        if (args.Kind == ActivationKind.Background)
                        {

                            // elmentjük a jelenleg folyó játékot
                            try
                            {
                                await _model.SaveGame(
                                    Path.Combine(AppContext.BaseDirectory, "SuspendedGame"));
                                // Androidon az AppContext.BaseDirectory az alkalmazás adat könyvtára, ahova
                                // akár külön jogosultság nélkül is lehetne írni
                            }
                            catch
                            {
                            }
                        }
                    };
                }
            }
            _model.UpdateView();
            base.OnFrameworkInitializationCompleted();
        }

        #endregion


        #region ViewModel event handlers

        private void ViewModel_NewGame(object? sender, EventArgs e)
        {
            _model.Pause();
            _model.NewGame();
            _model.Unpause();
            _viewModel.Update();
        }

        private async void ViewModel_LoadGame(object? sender, EventArgs e)
        {
            if (TopLevel == null)
            {
                _ = await MessageBoxManager.GetMessageBoxStandard(
                        "Reversi game",
                        "File management is not supported!",
                        ButtonEnum.Ok, Icon.Error)
                    .ShowAsync();
                return;
            }

            _model.Pause();

            try
            {
                System.Collections.Generic.IReadOnlyList<IStorageFile> files = await TopLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    Title = "Load Reversi Game",
                    AllowMultiple = false,
                    FileTypeFilter = [new FilePickerFileType("Reversi game") { Patterns = ["*.reversi"] }]
                });

                if (files != null && files.Count > 0)
                {
                    using Stream stream = await files[0].OpenReadAsync();
                    await _model.LoadGame(stream);
                    _viewModel.Saved = false;
                }
            }
            catch (ReversiDataException)
            {
                _ = await MessageBoxManager.GetMessageBoxStandard("Reversi", "The file loading failed!", ButtonEnum.Ok, Icon.Error).ShowAsync();
            }

            _viewModel.Update();
            _model.Unpause();
        }
        private async void ViewModel_SaveGame(object? sender, EventArgs e)
        {
            if (TopLevel == null)
            {
                _ = await MessageBoxManager.GetMessageBoxStandard(
                        "Reversi gamne",
                        "File management is not supported!",
                        ButtonEnum.Ok, Icon.Error)
                    .ShowAsync();
                return;
            }

            _model.Pause();

            try
            {
                IStorageFile? file = await TopLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
                {
                    Title = "Save Reversi Game",
                    FileTypeChoices = [new FilePickerFileType("Reversi game") { Patterns = ["*.reversi"] }]
                });

                if (file != null)
                {
                    using Stream stream = await file.OpenWriteAsync();
                    await _model.SaveGame(stream);
                    _viewModel.Saved = true;
                }
            }
            catch (ReversiDataException)
            {
                _ = await MessageBoxManager.GetMessageBoxStandard("Reversi", "Failed to save the game!", ButtonEnum.Ok, Icon.Error).ShowAsync();
            }

            _model.Unpause();
        }


        private void ViewModel_ReadRules(object? sender, EventArgs e)
        {
            _model.Pause();

            _ = MessageBoxManager.GetMessageBoxStandard("Reversi Rules", "Each player has a color, and the aim of the game is to get more of your pieces on the board than the opponent's pieces. \n" +
                "At the start of the game there are four pieces on the board, two white and two black. You must try to capture opponent pieces and flip them " +
                "over so they turn into your color. You do this by making a horizontal, vertical or diagonal line of pieces, where your pieces surround the other player's pieces. " +
                "The surrounded opponent pieces are then captured and will be flipped over to your color, increasing the number of your pieces on the board. \n" +
                "Every move you make must capture some opponent pieces. This means that every time it's your turn you must place one of your pieces next to a line of one or more opponent pieces, " +
                "where there's another one of your pieces at the other end of the line. \n If there is no available move on the board that captures pieces then you must say Pass " +
                "and your opponent gets to play again. If both players say pass in a row then there are no more moves on the board and the game ends.", ButtonEnum.Ok, Icon.Info).ShowAsync();

            _model.Unpause();
        }

        #endregion

        #region Model event handlers

        private async void Model_SetGameEnded(object? sender, ReversiSetGameEndedEventArgs e)
        {
            _viewModel.PassButtonEnabled = false;
            _viewModel.PauseButtonEnabled = false;
            _viewModel.Saved = true;
            _model.UpdateView();

            string resultMessage = e.PlayerWhitePoints > e.PlayerBlackPoints
                ? $"Player White won.\nWhite points: {e.PlayerWhitePoints}\nBlack points: {e.PlayerBlackPoints}"
                : e.PlayerWhitePoints < e.PlayerBlackPoints
                    ? $"Player Black won.\nWhite points: {e.PlayerWhitePoints}\nBlack points: {e.PlayerBlackPoints}"
                    : $"It is a tie.\nWhite points: {e.PlayerWhitePoints}\nBlack points: {e.PlayerBlackPoints}";

            _ = await MessageBoxManager.GetMessageBoxStandard("Reversi", resultMessage, ButtonEnum.Ok, Icon.Info).ShowAsync();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
