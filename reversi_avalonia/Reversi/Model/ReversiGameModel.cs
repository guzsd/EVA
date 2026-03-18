using System.Timers;
using Reversi.Presistence;

namespace Reversi.Model
{
    /// <summary>
    /// Játéktábla méretének felsorolási típusa.
    /// </summary>
    public enum BoardSize { Small, Medium, Large }
    /// <summary>
    /// Reversi játék típusa.
    /// </summary>
    public class ReversiGameModel : IDisposable
    {
        #region Size constants
        private const Int32 BoardSizeSmall = 10;
        private const Int32 BoardSizeMedium = 20;
        private const Int32 BoardSizeLarge = 30;
        #endregion

        #region Fields
        private readonly IReversiDataAccess _dataAccess; // adatelérés
        private ReversiGameInfo _data; // adoatok mentéshez és betöltéshez
        private Int32 _whitePlayerPoints; // fehér játékos pontjai
        private Int32 _blackPlayerPoints; // fekete játékos pontjai
        private readonly System.Timers.Timer _timer; // idő
        #endregion

        #region Properties
        /// <summary>
        /// Lehetséges lépések listályának elérése.
        /// </summary>
        public List<Int32> ValidMoves { get; set; }
        /// <summary>
        /// Beállításokman szereplő játéktábla méretének elérése.
        /// </summary>
        public BoardSize TableSizeSetting { get; set; }
        /// <summary>
        /// Jelenlegi játéktábla méretének lekérdezése.
        /// </summary>
        /// 
        public BoardSize ActiveTableSize { get; private set; }

        public BoardSize PreviousTableSize { get; private set; }

        /// <summary>
        /// Jelenlegi játéktábla méretének lekérdezése int-ben.
        /// </summary>
        public Int32 ActiveTableSizeInt
        {
            get
            {
                Int32 size = 10;
                switch (ActiveTableSize)
                {
                    case BoardSize.Small:
                        size = BoardSizeSmall;
                        break;
                    case BoardSize.Medium:
                        size = BoardSizeMedium;
                        break;
                    case BoardSize.Large:
                        size = BoardSizeLarge;
                        break;
                    default:
                        break;
                }
                return size;
            }
        }
        /// <summary>
        /// Soron lévő játékos lekérdezése.
        /// </summary>
        public Boolean IsPlayerWhiteTurnOn { get; private set; }

        /// <summary>
        /// Teszteléshez szükséges lekérdezések:
        /// </summary>
        public Board Board { get; private set; }
        public Boolean IsGameStarted { get; private set; }
        public Boolean IsPassingTurnOn { get; private set; }

        #endregion

        #region Events
        /// <summary>
        /// Gondolkozási idő változásának eseménye.
        /// </summary>
        public event EventHandler<ReversiUpdatePlayerTimeEventArgs>? UpdatePlayerTime;

        /// <summary>
        /// Játéktábla változásának eseménye.
        /// </summary>
        public event EventHandler<ReversiUpdateBoardEventArgs>? UpdateBoard;

        /// <summary>
        /// Játék vége esemény.
        /// </summary>
        public event EventHandler<ReversiSetGameEndedEventArgs>? SetGameEnded;

        #endregion

        #region Constructors

        /// <summary>
        /// Reversi játék példányosítása.
        /// </summary>
        /// <param name="dataAccess">Adatelérés.</param>
        public ReversiGameModel(IReversiDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            TableSizeSetting = BoardSize.Small;
            IsGameStarted = false;

            Board = new Board(10);
            ValidMoves = [];
            _data = new ReversiGameInfo(10, Board, 0, 0, true);

            _timer = new System.Timers.Timer(1000.0);
            _timer.Elapsed += Timer_Elapsed;
            UpdateActiveTableSize();
            _timer.Enabled = false;

        }

        #endregion

        #region Public methods
        /// <summary>
        /// Új reversi játék létrehozása a beállításokban szereplő táblamérettel.
        /// </summary>
        public void NewGame()
        {

            _timer.Enabled = false;
            Int32 size = 10;
            PreviousTableSize = ActiveTableSize;
            switch (TableSizeSetting)
            {
                case BoardSize.Small:
                    size = BoardSizeSmall;
                    ActiveTableSize = BoardSize.Small;
                    break;
                case BoardSize.Medium:
                    size = BoardSizeMedium;
                    ActiveTableSize = BoardSize.Medium;
                    break;
                case BoardSize.Large:
                    size = BoardSizeLarge;
                    ActiveTableSize = BoardSize.Large;
                    break;
                default:
                    break;
            }
            Board = new Board(size);
            ValidMoves.Clear();
            IsPlayerWhiteTurnOn = true;
            _data = new ReversiGameInfo(size, Board, 0, 0, true);
            IsPassingTurnOn = false;
            GetValidMoves(IsPlayerWhiteTurnOn);
            UpdateScore();
            _timer.Enabled = true;
            IsGameStarted = true;
            UpdateView();


        }
        /// <summary>
        /// Játék betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        public async Task LoadGame(String path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("The path cannot be null or empty.", nameof(path));
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("The specified file does not exist.", path);
            }
            await LoadGame(File.OpenRead(path));
        }

        /// <summary>
        /// Játék betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        public async Task LoadGame(Stream stream)
        {
            _timer.Enabled = false;
            if (_dataAccess != null)
            {
                _data = await _dataAccess.LoadAsync(stream);
            }
            Int32 size = 10;

            PreviousTableSize = ActiveTableSize;
            switch (_data.BoardSize)
            {
                case 10:
                    size = BoardSizeSmall;
                    ActiveTableSize = BoardSize.Small;
                    break;
                case 20:
                    size = BoardSizeMedium;
                    ActiveTableSize = BoardSize.Medium;
                    break;
                case 30:
                    size = BoardSizeLarge;
                    ActiveTableSize = BoardSize.Large;
                    break;
                default:
                    break;
            }
            Board = new Board(size);
            Board = _data.Board;
            IsGameStarted = true;
            ValidMoves.Clear();
            IsPlayerWhiteTurnOn = _data.IsPlayerWhiteTurnOn;
            IsPassingTurnOn = false;
            GetValidMoves(IsPlayerWhiteTurnOn);
            if (ValidMoves.Count == 0) { IsPassingTurnOn = true; }
            UpdateScore();
            IsGameStarted = true;
            _timer.Enabled = true;
            UpdateView();
        }

        /// <summary>
        /// Játék mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        public async Task SaveGame(String path)
        {
            await SaveGame(File.OpenWrite(path));
        }

        /// <summary>
        /// Játék mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        public async Task SaveGame(Stream stream)
        {
            _timer.Enabled = false;

            if (_dataAccess != null && IsGameStarted)
            {
                _data.BoardSize = Board.ScreenBoard.GetLength(0);
                _data.Board = Board;
                _data.IsPlayerWhiteTurnOn = IsPlayerWhiteTurnOn;
                await _dataAccess.SaveAsync(stream, _data);
            }
            _timer.Enabled = true;
        }


        /// <summary>
        /// Aktualizálja a vizuális megjelenítést.
        /// </summary>
        public void UpdateView()
        {
            Board.CountColors();
            UpdateScore();
            OnUpdateBoard(new ReversiUpdateBoardEventArgs(Board.ScreenBoard.GetLength(0), Board, _whitePlayerPoints, _blackPlayerPoints, IsPlayerWhiteTurnOn, IsPassingTurnOn));
            OnUpdatePlayerTime(new ReversiUpdatePlayerTimeEventArgs(true, _data.PlayerWhiteTime));
            OnUpdatePlayerTime(new ReversiUpdatePlayerTimeEventArgs(false, _data.PlayerBlackTime));
        }

        /// <summary>
        /// Ellenőrzi, hogy a játékos tudn e lépni.
        /// </summary>
        /// <param name="isPlayerWhiteTurnOn">A soron lévő játékost jelzi.</param>
        /// <returns>Igaz, ha tud rakni, egyébként hamis.</returns>
        public Boolean HasValidMoves(Boolean isPlayerWhiteTurnOn)
        {
            Boolean isValid = false;
            if (Board != null)
            {
                for (int i = 0; i < Board.ScreenBoard.GetLength(0); i++)
                {
                    for (int j = 0; j < Board.ScreenBoard.GetLength(0); j++)
                    {
                        if (Board.IsValidMove(isPlayerWhiteTurnOn, i, j))
                        {
                            isValid = true;
                        }
                    }
                }
            }
            return isValid;
        }

        /// <summary>
        /// Elvégzi a lehelyezést és ellenőrzi, hogy a következő játékos tud e lépni.
        /// </summary>
        /// <param name="isPlayerWhiteTurnOn">A soron lévő játékost jelzi.</param>
        /// <param name="x">A lehelyezés pozíciójának első koordinátája.</param>
        /// <param name="y">A lehelyezés pozíciójának második koordinátája.</param>
        public void PutDown(Boolean isPlayerWhiteTurnOn, Int32 x, Int32 y)
        {
            if (IsGameStarted && _timer.Enabled)
            {
                if (CheckMove(isPlayerWhiteTurnOn, x, y))
                {
                    MakeMove(isPlayerWhiteTurnOn, x, y);
                }
                GetValidMoves(IsPlayerWhiteTurnOn);
                if (ValidMoves.Count == 0)
                {
                    GetValidMoves(!IsPlayerWhiteTurnOn);
                    IsPassingTurnOn = true;
                }
                if (ValidMoves.Count == 0)
                {
                    _timer.Enabled = false;
                    IsGameStarted = false;
                    UpdateScore();
                    OnSetGameEnded(new ReversiSetGameEndedEventArgs(_whitePlayerPoints, _blackPlayerPoints));
                }
            }
            OnUpdateBoard(new ReversiUpdateBoardEventArgs(Board.ScreenBoard.GetLength(0), Board, _whitePlayerPoints, _blackPlayerPoints, IsPlayerWhiteTurnOn, IsPassingTurnOn));

        }

        /// <summary>
        /// Az egyik játékos passzol.
        /// </summary>
        public void Pass()
        {
            if (IsGameStarted && IsPassingTurnOn && _timer.Enabled)
            {
                IsPlayerWhiteTurnOn = !IsPlayerWhiteTurnOn;
                GetValidMoves(IsPlayerWhiteTurnOn);
                OnUpdateBoard(new ReversiUpdateBoardEventArgs(Board.ScreenBoard.GetLength(0), Board, _whitePlayerPoints, _blackPlayerPoints, IsPlayerWhiteTurnOn, IsPassingTurnOn));
            }
            if (IsPassingTurnOn) { IsPassingTurnOn = false; }
        }

        /// <summary>
        /// Megállítja a játékot.
        /// </summary>
        public void Pause()
        {
            if (IsGameStarted) { _timer.Enabled = false; }
        }

        /// <summary>
        /// Újraindítja a játékot.
        /// </summary>
        public void Unpause()
        {
            if (IsGameStarted) { _timer.Enabled = true; }
        }


        /// <summary>
        /// Felszabadítja az erőforrásokat, amelyeket az osztály használ.
        /// </summary>
        public void Dispose()
        {
            _timer?.Dispose();

        }

        public void UpdateActiveTableSize()
        {
            switch (_data.BoardSize)
            {
                case 10:
                    ActiveTableSize = BoardSize.Small;
                    break;
                case 20:
                    ActiveTableSize = BoardSize.Medium;
                    break;
                case 30:
                    ActiveTableSize = BoardSize.Large;
                    break;
                default:
                    break;
            }
        }


        #endregion

        #region Private methods

        /// <summary>
        /// Aktualizálja a játékosok pontszámait.
        /// </summary>
        private void UpdateScore()
        {
            _blackPlayerPoints = Board.CountBlack;
            _whitePlayerPoints = Board.CountWhite;
        }

        /// <summary>
        /// Ellenőrzi, hogy a lépés jó adatokat tartalmaz és megtörténhet.
        /// </summary>
        /// <param name="isPlayerWhiteTurnOn">A soron lévő játékost jelzi.</param>
        /// <param name="row">A lehelyezés pozíciójának első koordinátája.</param>
        /// <param name="col">A lehelyezés pozíciójának második koordinátája.</param>
        /// <returns>Igaz, ha le tudja rakni az adott pozícióra, egyébként hamis.</returns>
        private Boolean CheckMove(Boolean isPlayerWhiteTurnOn, Int32 row, Int32 col)
        {
            Boolean isValid = false;
            if (row >= 0 && row <= Board.ScreenBoard.GetLength(0) && col >= 0 && col <= Board.ScreenBoard.GetLength(0) &&
                Board.IsValidMove(isPlayerWhiteTurnOn, row, col))
            {
                isValid = true;
            }
            return isValid;
        }

        /// <summary>
        /// Felcseréli a soron lévő játékost.
        /// </summary>
        /// <param name="isPlayerWhiteTurnOn">A soron lévő játékost jelzi.</param>
        private void SwitchPlayer(Boolean isPlayerWhiteTurnOn)
        {
            IsPlayerWhiteTurnOn = !isPlayerWhiteTurnOn;
        }

        /// <summary>
        /// Ténylegesen végrehajtja a lépést.
        /// </summary>
        /// <param name="isPlayerWhiteTurnOn">A soron lévő játékost jelzi.</param>
        /// <param name="row">A lehelyezés pozíciójának első koordinátája.</param>
        /// <param name="col">A lehelyezés pozíciójának második koordinátája.</param>
        private void MakeMove(Boolean isPlayerWhiteTurnOn, Int32 row, Int32 col)
        {
            Board.UpdateBoardState(isPlayerWhiteTurnOn, row, col);
            SwitchPlayer(isPlayerWhiteTurnOn);
            Board.CountColors();
            UpdateScore();
        }

        /// <summary>
        /// Frissíti a lehetséges lépések listáját.
        /// </summary>
        /// <param name="isPlayerWhiteTurnOn">A soron lévő játékost jelzi.</param>
        private void GetValidMoves(Boolean isPlayerWhiteTurnOn)
        {
            ValidMoves.Clear();

            for (int i = 0; i < Board.ScreenBoard.GetLength(0); i++)
            {
                for (int j = 0; j < Board.ScreenBoard.GetLength(0); j++)
                {
                    if (Board.IsValidMove(isPlayerWhiteTurnOn, i, j))
                    {
                        ValidMoves.Add(i);
                        ValidMoves.Add(j);
                    }
                }
            }
        }
        #endregion

        #region Private event methods

        /// <summary>
        /// Játék vége eseményének kiváltása.
        /// </summary>
        /// <param name="arg">Játék vége eseménykezelő argumentuma.</param>
        private void OnSetGameEnded(ReversiSetGameEndedEventArgs arg)
        {
            SetGameEnded?.Invoke(this, arg);
        }

        /// <summary>
        /// Játékosidő frissítése eseményének kiváltása.
        /// </summary>
        /// <param name="arg">Játékosidő frissítése eseménykezelő argumentuma.</param>
        public void OnUpdatePlayerTime(ReversiUpdatePlayerTimeEventArgs arg)
        {
            //if (UpdatePlayerTime != null) { UpdatePlayerTime(this, arg); } ugyanaz
            UpdatePlayerTime?.Invoke(this, arg);

        }

        /// <summary>
        /// Játéktábla frissítése eseményének kiváltása.
        /// </summary>
        /// <param name="arg">Játéktábla frissítése eseménykezelő argumentuma.</param>
        public void OnUpdateBoard(ReversiUpdateBoardEventArgs arg)
        {
            UpdateBoard?.Invoke(this, arg);
        }



        /// <summary>
        /// Az időzítő minden egyes ütemnél meghívja ezt a metódust.
        /// </summary>
        /// <param name="sender">Adatokat biztosít a Timer.Elapsed eseményhez.</param>
        /// <param name="e">A hívó objektum.</param>
        private void Timer_Elapsed(Object? sender, ElapsedEventArgs e)
        {
            if (IsPlayerWhiteTurnOn)
            {
                ++_data.PlayerWhiteTime;
                OnUpdatePlayerTime(new ReversiUpdatePlayerTimeEventArgs(IsPlayerWhiteTurnOn, _data.PlayerWhiteTime));
            }
            else
            {
                ++_data.PlayerBlackTime;
                OnUpdatePlayerTime(new ReversiUpdatePlayerTimeEventArgs(IsPlayerWhiteTurnOn, _data.PlayerBlackTime));
            }
        }
        #endregion


    }
}
