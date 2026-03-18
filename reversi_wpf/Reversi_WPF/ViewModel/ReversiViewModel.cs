using System;
using Reversi.Model;
using Reversi.Presistence;
using Reversi_WPF.View;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Reversi_WPF.ViewModel
{
    /// <summary>
    /// Reversi nézetmodell típusa.
    /// </summary>
    public class ReversiViewModel : ViewModelBase
    {
        #region Fields
        private ReversiGameModel _model; 
        private Boolean _isPlayerWhiteTurnOn; 
        private Boolean _saveMenuItemEnabled;
        private Boolean _passButtonEnabled;
        private Boolean _pauseButtonEnabled;
        private String _pauseText;
        private Int32 _playerWhiteTime;
        private Int32 _playerBlackTime;
        private Int32 _whitePlayerPoints;
        private Int32 _blackPlayerPoints;
        private Boolean _saved;
        private String _gamePoints;

        #endregion

        #region Properties

        /// <summary>
        /// Új játék kezdése parancs lekérdezése.
        /// </summary>
        public DelegateCommand NewGameCommand { get; private set; }

        /// <summary>
        /// Játék betöltése parancs lekérdezése.
        /// </summary>
        public DelegateCommand LoadGameCommand { get; private set; }

        /// <summary>
        /// Játék mentése parancs lekérdezése.
        /// </summary>
        public DelegateCommand SaveGameCommand { get; private set; }

        /// <summary>
        /// Kilépés parancs lekérdezése.
        /// </summary>
        public DelegateCommand ExitGameCommand { get; private set; }

        /// <summary>
        /// Szabályok parancs lekérdezése.
        /// </summary>
        public DelegateCommand RulesCommand { get; private set; }

        /// <summary>
        /// Passzolás parancs lekérdezése.
        /// </summary>
        public DelegateCommand PassCommand { get; private set; }

        /// <summary>
        /// Szüneteltetés parancs lekérdezése.
        /// </summary>
        public DelegateCommand PauseCommand { get; private set; }

        /// <summary>
        /// Játékmező gyűjtemény lekérdezése.
        /// </summary>
        public ObservableCollection<ReversiField> Fields { get; private set; }
        
        public Boolean SaveMenuItemEnabled
        { get { return _saveMenuItemEnabled; } }
      
        public Boolean PassButtonEnabled { get { return _passButtonEnabled; } set { _passButtonEnabled = value; } }

        public Boolean PauseButtonEnabled { get { return _pauseButtonEnabled; } set { _pauseButtonEnabled = value; } }

        public String PauseText { get { return _pauseText; } }

        public Int32 PlayerWhiteTime { get { return _playerWhiteTime; } }

        public Int32 PlayerBlackTime { get { return _playerBlackTime; } }
        public Int32 WhitePlayerPoints { get { return _whitePlayerPoints; } }
        public Int32 BlackPlayerPoints { get { return _blackPlayerPoints; } }
        public String GamePoints { get { return _gamePoints; } }

        public Boolean Saved
        {
            get { return _saved; }
            set { _saved = value; }
        }
        public Boolean IsGameSmallEnabled { get { return !(IsGameSmall); } }
        public Boolean IsGameMediumEnabled { get { return !(IsGameMedium); } }
        public Boolean IsGameLargeEnabled { get { return !(IsGameLarge); } }

        /// <summary>
        /// Alacsony nehézségi szint állapotának lekérdezése.
        /// </summary>
        public Boolean IsGameSmall
        {
            get { return _model.TableSizeSetting == BoardSize.Small; }
            set
            {
                _model.TableSizeSetting = BoardSize.Small;
                OnPropertyChanged(nameof(IsGameSmall));
                OnPropertyChanged(nameof(IsGameMedium));
                OnPropertyChanged(nameof(IsGameLarge));
                OnPropertyChanged(nameof(IsGameSmallEnabled));
                OnPropertyChanged(nameof(IsGameMediumEnabled));
                OnPropertyChanged(nameof(IsGameLargeEnabled));
            }
        }

        /// <summary>
        /// Közepes nehézségi szint állapotának lekérdezése.
        /// </summary>
        public Boolean IsGameMedium
        {
            get { return _model.TableSizeSetting == BoardSize.Medium; }
            set
            {
                

                _model.TableSizeSetting = BoardSize.Medium;
                OnPropertyChanged(nameof(IsGameSmall));
                OnPropertyChanged(nameof(IsGameMedium));
                OnPropertyChanged(nameof(IsGameLarge));
                OnPropertyChanged(nameof(IsGameSmallEnabled));
                OnPropertyChanged(nameof(IsGameMediumEnabled));
                OnPropertyChanged(nameof(IsGameLargeEnabled));
            }
        }

        /// <summary>
        /// Magas nehézségi szint állapotának lekérdezése.
        /// </summary>
        public Boolean IsGameLarge
        {
            get { return _model.TableSizeSetting == BoardSize.Large; }
            set
            {
               

                _model.TableSizeSetting = BoardSize.Large;
                OnPropertyChanged(nameof(IsGameSmall));
                OnPropertyChanged(nameof(IsGameMedium));
                OnPropertyChanged(nameof(IsGameLarge));
                OnPropertyChanged(nameof(IsGameSmallEnabled));
                OnPropertyChanged(nameof(IsGameMediumEnabled));
                OnPropertyChanged(nameof(IsGameLargeEnabled));
            }
        }


        #endregion

        #region Events

        /// <summary>
        /// Új játék eseménye.
        /// </summary>
        public event EventHandler? NewGame;

        /// <summary>
        /// Játék betöltésének eseménye.
        /// </summary>
        public event EventHandler? LoadGame;

        /// <summary>
        /// Játék mentésének eseménye.
        /// </summary>
        public event EventHandler? SaveGame;

        /// <summary>
        /// Játékból való kilépés eseménye.
        /// </summary>
        public event EventHandler? ExitGame;
        /// <summary>
        /// Játékszabály eseménye.
        /// </summary>
        public event EventHandler? Rules;

        #endregion

        #region Constructors

        /// <summary>
        /// Reversi nézetmodell példányosítása.
        /// </summary>
        /// <param name="model">A modell típusa.</param>
        public ReversiViewModel(ReversiGameModel model)
        {
            _model = model;
            _model.SetGameEnded += new EventHandler<ReversiSetGameEndedEventArgs>(Model_SetGameEnded);
            _model.UpdatePlayerTime += new EventHandler<ReversiUpdatePlayerTimeEventArgs>(Model_UpdatePlayerTime);
            _model.UpdateBoard += new EventHandler<ReversiUpdateBoardEventArgs>(Model_UpdateBoard);

            NewGameCommand = new DelegateCommand(param => { OnNewGame(); });
            LoadGameCommand = new DelegateCommand(param => { OnLoadGame(); });
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitGameCommand = new DelegateCommand(param => OnExitGame());
            RulesCommand = new DelegateCommand(param => OnRules());
            PassCommand = new DelegateCommand(param => OnPass());
            PauseCommand = new DelegateCommand(param => OnPause());

            Fields = new ObservableCollection<ReversiField>();
            _saveMenuItemEnabled = false;
            OnPropertyChanged(nameof(SaveMenuItemEnabled));
            _passButtonEnabled = false;
            OnPropertyChanged(nameof(PassButtonEnabled));
            _pauseButtonEnabled = false;
            OnPropertyChanged(nameof(PauseButtonEnabled));
            _pauseText = "Pause";
            OnPropertyChanged(nameof(PauseText));
            _playerWhiteTime = 0;
            OnPropertyChanged(nameof(PlayerWhiteTime));
            _playerBlackTime = 0;
            OnPropertyChanged(nameof(PlayerBlackTime));
            _whitePlayerPoints = 2;
            OnPropertyChanged(nameof(WhitePlayerPoints));
            _blackPlayerPoints = 2;
            OnPropertyChanged(nameof(BlackPlayerPoints));
            _gamePoints = "_";
            _saved = true;
        }

        #endregion


        #region Game event handlers

        /// <summary>
        /// Játék végének eseménykezelője.
        /// </summary>
        private void Model_SetGameEnded(object? sender, ReversiSetGameEndedEventArgs e)
        {
            _passButtonEnabled = false;
            OnPropertyChanged(nameof(PassButtonEnabled));
            _pauseButtonEnabled = false;
            OnPropertyChanged(nameof(PauseButtonEnabled));

            _saved = true;
            _saveMenuItemEnabled = false;
            OnPropertyChanged(nameof(SaveMenuItemEnabled));

        }
        
        /// <summary>
        /// Játékosok idejének frissítése.
        /// </summary>
        private void Model_UpdatePlayerTime(object? sender, ReversiUpdatePlayerTimeEventArgs e)
        {
            if (e.IsPlayerWhiteTimeNeedUpdate)
            {
                _playerWhiteTime = e.NewTime;
                OnPropertyChanged(nameof(PlayerWhiteTime));
            }
            else
            {
                _playerBlackTime = e.NewTime;
                OnPropertyChanged(nameof(PlayerBlackTime));
            }
        }

        /// <summary>
        /// Játéktábla frissítése.
        /// </summary>
        public void Model_UpdateBoard(object? sender, ReversiUpdateBoardEventArgs e)
        {
            _saved = false;
            _saveMenuItemEnabled = true;
            OnPropertyChanged(nameof(SaveMenuItemEnabled));

            _whitePlayerPoints = e.PlayerWhitePoints;
            _blackPlayerPoints = e.PlayerBlackPoints;
            _gamePoints = "White: " + e.PlayerWhitePoints.ToString() + " -- Black: " + e.PlayerBlackPoints.ToString();
            OnPropertyChanged(nameof(GamePoints));

            _isPlayerWhiteTurnOn = e.IsPlayerWhiteTurnOn;

            _passButtonEnabled = e.IsPassingTurnOn;
            OnPropertyChanged(nameof(PassButtonEnabled));

            if (_model.PreviousTableSize != _model.ActiveTableSize) {
                SetButtonGridUp();
            }

            List<Int32> _validMoves = _model.ValidMoves;
            Board boardd = e.Board;
            Int32 value;
            for (Int32 i = 0; i < e.BoardSize; i++)
            {
                for (Int32 j = 0; j < e.BoardSize; j++)
                {
                    value = boardd.ScreenBoard[i, j];
                    if (IsInValidMoves(i, j, _validMoves))
                    {
                        if (e.IsPassingTurnOn)
                        {
                            if (_isPlayerWhiteTurnOn)
                            {
                                value = 3;
                            }
                            else { value = 4; }
                        }
                        else if (_isPlayerWhiteTurnOn)
                        {
                            value = 5;
                        }
                        else { value = 6; }                        
                    }
                    UpdateField(i, j, value);
                }
            }
        }
        //szükségtelen
        public void Update()
        {
            _model.UpdateView();
        }

        #endregion

        #region Event methods

        /// <summary>
        /// Új játék indításának eseménykiváltása.
        /// </summary>
        private void OnNewGame()
        {
            _model.UpdateActiveTableSize();
            SetButtonGridUp();
            NewGame?.Invoke(this, EventArgs.Empty);

            _saveMenuItemEnabled = true;
            OnPropertyChanged(nameof(SaveMenuItemEnabled));
            _saved = false;
            _pauseButtonEnabled = true;
            OnPropertyChanged(nameof(PauseButtonEnabled));
            _pauseText = "Pause";
            _model.Unpause();
            OnPropertyChanged(nameof(PauseText));

        }

        /// <summary>
        /// Játék betöltése eseménykiváltása.
        /// </summary>
        private void OnLoadGame()
        {
            if (LoadGame != null)
            {
                OnNewGame();
                LoadGame(this, EventArgs.Empty);
                if (_saved == false)
                {
                    _saveMenuItemEnabled = true;
                    OnPropertyChanged(nameof(SaveMenuItemEnabled));
                }
            }

            
        }

        /// <summary>
        /// Játék mentése eseménykiváltása.
        /// </summary>
        private void OnSaveGame()
        {
            if (SaveGame != null)
            {
                SaveGame(this, EventArgs.Empty);
                if (_saved == true)
                {
                    _saveMenuItemEnabled = false;
                    OnPropertyChanged(nameof(SaveMenuItemEnabled));
                }
            }
        }
        /// <summary>
        /// Játékból való kilépés eseménykiváltása.
        /// </summary>
        private void OnExitGame()
        {
            ExitGame?.Invoke(this, EventArgs.Empty);

        }

        /// <summary>
        /// Játékszabály megjelenítésének eseménykiváltása.
        /// </summary>
        private void OnRules()
        {
            Rules?.Invoke(this, EventArgs.Empty);
            
        }

        /// <summary>
        /// Passzolás eseménykiváltása.
        /// </summary>
        private void OnPass()
        {
            _model.Pass();
            _saved = false;
            _saveMenuItemEnabled = true;
            OnPropertyChanged(nameof(SaveMenuItemEnabled));
          
        }
        /// <summary>
        /// Szüneteltetés eseménykiváltása.
        /// </summary>
        private void OnPause()
        {
            if (_pauseText == "Pause")
            {
                _pauseText = "Continue";
                _model.Pause();
                OnPropertyChanged(nameof(PauseText));
            }
            else
            {
                _pauseText = "Pause";
                _model.Unpause();
                OnPropertyChanged(nameof(PauseText));
            }
        }

        #endregion

        #region Private methods
        /// <summary>
        /// Megvizsgálja, hogy a lehetséges lépések közé tartozik.
        /// </summary>
        /// <param name="x">A pozíció első koordinátája.</param>
        /// <param name="y">A oozíció második koordinátája.</param>
        /// <param name="validmoves">A lehetséges lépések listája.</param>
        /// <returns>Igaz, ha a valid lépések eggyike, egyébként hamis.</returns>
        private Boolean IsInValidMoves(Int32 x, Int32 y, List<Int32> validmoves)
        {
            Boolean init = false;
            for (Int32 i = 0; i < validmoves.Count; i = i + 2)
            {
                if (validmoves[i] == x && validmoves[i + 1] == y)
                {
                    init = true;
                }
            }
            return init;
        }



        /// <summary>
        /// Mezők frissítése.
        /// </summary>
        /// <param name="x">Az X koordináta.</param>
        /// <param name="y">Az Y koordináta.</param>
        /// <param name="value">A mező állapotát jelölő érték.</param>
        public void UpdateField(Int32 x, Int32 y, Int32 value)
        {
            Int32 index = (((x * _model.ActiveTableSizeInt) + y));
            if (Fields.Count > index)
            {
                ReversiField Field = Fields[index];
                // gray = 10, white = 20, black = 30
                switch (value)
                {
                    case 0:
                        Field.BackColorInt = 10;
                        Field.Enabled = false;
                        Field.Text = " ";
                        break;
                    case 1:
                        Field.BackColorInt = 20;
                        Field.Enabled = false;
                        Field.Text = " ";
                        break;
                    case 2:
                        Field.BackColorInt = 30;
                        Field.Enabled = false;
                        Field.Text = " ";
                        break;
                    case 3:
                        //ha  a fehérnek passzonia kell és a fekete jön
                        Field.BackColorInt = 10;
                        Field.Text = "X";
                        Field.TextColorInt = 30;
                        break;
                    case 4:
                        //ha  a feketének passzonia kell és a fehér jön
                        Field.BackColorInt = 10;
                        Field.Text = "X";
                        Field.TextColorInt = 20;
                        break;
                    case 5:
                        Field.Text = "X";
                        Field.TextColorInt = 20;
                        Field.BackColorInt = 10;
                        Field.Enabled = true;
                        break;
                    case 6:
                        Field.Text = "X";
                        Field.TextColorInt = 30;
                        Field.BackColorInt = 10;
                        Field.Enabled = true;
                        break;
                    default:
                        throw new Exception("Model gave us a number, that we was not ready for, while updating the table view.");

                }
            }

        }

        /// <summary>
        /// A játék gombjainak generálása, és ezek elmentése.
        /// </summary>
        private void SetButtonGridUp()
        {
            _model.UpdateActiveTableSize();
            if (Fields.Count == 0 || _model.ActiveTableSizeInt * _model.ActiveTableSizeInt != Fields.Count)
            {
                
                Fields.Clear();
                
                for (Int32 x = 0; x < _model.ActiveTableSizeInt; ++x)
                {
                    for (Int32 y = 0; y < _model.ActiveTableSizeInt; ++y)
                    {
                        Fields.Add(new ReversiField(new DelegateCommand(param => FieldClicked(Convert.ToInt32(param))), x, y, ((x * _model.ActiveTableSizeInt) + y)));
                        
                    }
                } 
            }
        }
     


        /// <summary>
        /// Mező frissítése.
        /// </summary>
        /// <param name="index">A mező indexe.</param>

        private void FieldClicked(Int32 index)
        {
            ReversiField button = Fields[index];

            if (button.Enabled == true)
            {
                _model.PutDown(_isPlayerWhiteTurnOn, button.X, button.Y);

            }
            
        }


        #endregion

    }
}


