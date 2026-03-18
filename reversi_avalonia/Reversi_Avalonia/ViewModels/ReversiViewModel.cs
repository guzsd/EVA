using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using Reversi.Model;
using Reversi.Presistence;
using CommunityToolkit.Mvvm.Input;

namespace Reversi_Avalonia.ViewModels
{


    /// <summary>
    /// Reversi nézetmodell típusa.
    /// </summary>
    public class ReversiViewModel : ViewModelBase
    {
        #region Fields
        private readonly ReversiGameModel _model;
        private Boolean _isPlayerWhiteTurnOn;


        #endregion





        #region Properties

        /// <summary>
        /// Új játék kezdése parancs lekérdezése.
        /// </summary>
        public RelayCommand NewGameCommand { get; private set; }

        /// <summary>
        /// Játék betöltése parancs lekérdezése.
        /// </summary>
        public RelayCommand LoadGameCommand { get; private set; }

        /// <summary>
        /// Játék mentése parancs lekérdezése.
        /// </summary>
        public RelayCommand SaveGameCommand { get; private set; }

        /// <summary>
        /// Kilépés parancs lekérdezése.
        /// </summary>
        public RelayCommand ExitGameCommand { get; private set; }

        /// <summary>
        /// Szabályok parancs lekérdezése.
        /// </summary>
        public RelayCommand RulesCommand { get; private set; }

        /// <summary>
        /// Passzolás parancs lekérdezése.
        /// </summary>
        public RelayCommand PassCommand { get; private set; }

        /// <summary>
        /// Szüneteltetés parancs lekérdezése.
        /// </summary>
        public RelayCommand PauseCommand { get; private set; }

        /// <summary>
        /// Játékmező gyűjtemény lekérdezése.
        /// </summary>
        public ObservableCollection<ReversiField> Fields { get; private set; }

        public Boolean SaveMenuItemEnabled { get; private set; }

        public Boolean PassButtonEnabled { get; set; }

        public Boolean PauseButtonEnabled { get; set; }

        public String PauseText { get; private set; }

        public Int32 PlayerWhiteTime { get; private set; }

        public Int32 PlayerBlackTime { get; private set; }
        public Int32 WhitePlayerPoints { get; private set; }
        public Int32 BlackPlayerPoints { get; private set; }
        public String GamePoints { get; private set; }

        public Boolean Saved { get; set; }
        public Boolean IsGameSmallEnabled => !IsGameSmall;
        public Boolean IsGameMediumEnabled => !IsGameMedium;
        public Boolean IsGameLargeEnabled => !IsGameLarge;


        /// <summary>
        /// Alacsony nehézségi szint állapotának lekérdezése.
        /// </summary>
        public Boolean IsGameSmall
        {
            get => _model.TableSizeSetting == BoardSize.Small;
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
            get => _model.TableSizeSetting == BoardSize.Medium;
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
            get => _model.TableSizeSetting == BoardSize.Large;
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

            NewGameCommand = new RelayCommand(OnNewGame);
            LoadGameCommand = new RelayCommand(OnLoadGame);
            SaveGameCommand = new RelayCommand(OnSaveGame);
            ExitGameCommand = new RelayCommand(OnExitGame);
            RulesCommand = new RelayCommand(OnRules);
            PassCommand = new RelayCommand(OnPass);
            PauseCommand = new RelayCommand(OnPause);

            Fields = [];
            SaveMenuItemEnabled = false;
            OnPropertyChanged(nameof(SaveMenuItemEnabled));
            PassButtonEnabled = false;
            OnPropertyChanged(nameof(PassButtonEnabled));
            PauseButtonEnabled = false;
            OnPropertyChanged(nameof(PauseButtonEnabled));
            PauseText = "Pause";
            OnPropertyChanged(nameof(PauseText));
            PlayerWhiteTime = 0;
            OnPropertyChanged(nameof(PlayerWhiteTime));
            PlayerBlackTime = 0;
            OnPropertyChanged(nameof(PlayerBlackTime));
            WhitePlayerPoints = 0;
            OnPropertyChanged(nameof(WhitePlayerPoints));
            BlackPlayerPoints = 0;
            OnPropertyChanged(nameof(BlackPlayerPoints));
            GamePoints = "_";
            Saved = true;

            OnNewGame();
            Update();
        }

        #endregion


        #region Game event handlers

        /// <summary>
        /// Játék végének eseménykezelője.
        /// </summary>
        private void Model_SetGameEnded(object? sender, ReversiSetGameEndedEventArgs e)
        {
            PassButtonEnabled = false;
            OnPropertyChanged(nameof(PassButtonEnabled));
            PauseButtonEnabled = false;
            OnPropertyChanged(nameof(PauseButtonEnabled));

            Saved = true;
            SaveMenuItemEnabled = false;
            OnPropertyChanged(nameof(SaveMenuItemEnabled));

        }

        /// <summary>
        /// Játékosok idejének frissítése.
        /// </summary>
        private void Model_UpdatePlayerTime(object? sender, ReversiUpdatePlayerTimeEventArgs e)
        {
            if (e.IsPlayerWhiteTimeNeedUpdate)
            {
                PlayerWhiteTime = e.NewTime;
                OnPropertyChanged(nameof(PlayerWhiteTime));
            }
            else
            {
                PlayerBlackTime = e.NewTime;
                OnPropertyChanged(nameof(PlayerBlackTime));
            }
        }

        /// <summary>
        /// Játéktábla frissítése.
        /// </summary>
        private void Model_UpdateBoard(object? sender, ReversiUpdateBoardEventArgs e)
        {
            Saved = false;
            SaveMenuItemEnabled = true;
            OnPropertyChanged(nameof(SaveMenuItemEnabled));

            WhitePlayerPoints = e.PlayerWhitePoints;
            BlackPlayerPoints = e.PlayerBlackPoints;
            GamePoints = "White: " + e.PlayerWhitePoints.ToString() + " -- Black: " + e.PlayerBlackPoints.ToString();
            OnPropertyChanged(nameof(GamePoints));

            _isPlayerWhiteTurnOn = e.IsPlayerWhiteTurnOn;

            PassButtonEnabled = e.IsPassingTurnOn;
            OnPropertyChanged(nameof(PassButtonEnabled));

            if (_model.PreviousTableSize != _model.ActiveTableSize)
            {
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
                        value = e.IsPassingTurnOn ? _isPlayerWhiteTurnOn ? 3 : 4 : _isPlayerWhiteTurnOn ? 5 : 6;
                    }
                    UpdateField(i, j, value);
                }
            }
        }
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

            SaveMenuItemEnabled = true;
            OnPropertyChanged(nameof(SaveMenuItemEnabled));
            Saved = false;
            PauseButtonEnabled = true;
            OnPropertyChanged(nameof(PauseButtonEnabled));
            PauseText = "Pause";
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
                if (!Saved)
                {
                    SaveMenuItemEnabled = true;
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
                if (Saved)
                {
                    SaveMenuItemEnabled = false;
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
            Saved = false;
            SaveMenuItemEnabled = true;
            OnPropertyChanged(nameof(SaveMenuItemEnabled));

        }
        /// <summary>
        /// Szüneteltetés eseménykiváltása.
        /// </summary>
        private void OnPause()
        {
            if (PauseText == "Pause")
            {
                PauseText = "Continue";
                _model.Pause();
                OnPropertyChanged(nameof(PauseText));
            }
            else
            {
                PauseText = "Pause";
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
            for (Int32 i = 0; i < validmoves.Count; i += 2)
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
            Int32 index = (x * _model.ActiveTableSizeInt) + y;
            if (Fields.Count > index)
            {
                ReversiField Field = Fields[index];
                // gray = 10, white = 20, black = 30
                switch (value)
                {
                    case 0:
                        Field.BackColorInt = 10;
                        Field.Text = " ";
                        break;
                    case 1:
                        Field.BackColorInt = 20;
                        Field.Text = " ";
                        break;
                    case 2:
                        Field.BackColorInt = 30;
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
                        break;
                    case 6:
                        Field.Text = "X";
                        Field.TextColorInt = 30;
                        Field.BackColorInt = 10;
                        break;
                    default:
                        throw new Exception("Model gave us a number, that we was not ready for, while updating the table view.");

                }
            }

        }



        private void SetButtonGridUp()
        {
            _model.UpdateActiveTableSize();
            int gridSize = _model.ActiveTableSizeInt; // Táblaméret 
            int requiredFieldCount = gridSize * gridSize;
            // Ha az aktuális mezők száma nem egyezik meg az új mérettel, újragenerálás
            if (Fields.Count != requiredFieldCount)
            {
                Fields.Clear();

                for (int x = 0; x < gridSize; ++x)
                {
                    for (int y = 0; y < gridSize; ++y)
                    {
                        Fields.Add(new ReversiField(
                            new RelayCommand<int>(param => FieldClicked(Convert.ToInt32(param))),
                            x,
                            y,
                            (x * gridSize) + y))
                        ;
                    }
                }
            }
            else
            {
                // Csak frissítés, ha a mezők száma nem változott
                foreach (ReversiField field in Fields)
                {
                    field.Text = "";
                    field.BackColorInt = 10;
                    field.TextColorInt = 10;
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
            _model.PutDown(_isPlayerWhiteTurnOn, button.X, button.Y);

        }


        #endregion




    }
}
