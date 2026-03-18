using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private IReversiDataAccess _dataAccess; // adatelérés
        private Board _board; // játéktábla
        private ReversiGameInfo _data; // adoatok mentéshez és betöltéshez
        private Boolean _isPlayerWhiteTurnOn; //soron lévő játékos
        private Boolean _isPassingTurnOn; // passzolás szükségességét jelzi
        private Int32 _whitePlayerPoints; // fehér játékos pontjai
        private Int32 _blackPlayerPoints; // fekete játékos pontjai
        private Boolean _isGameStarted; // a játték kezdete megtörtént
        private System.Timers.Timer _timer; // idő
        private BoardSize _tableSizeSetting; // beállított játéktábla mérete
        private BoardSize _activeTableSize; // jelenlegi játéktábla mérete
        private List<Int32> _validMoves; // lehetséges lépések listája
        #endregion

        #region Properties
        /// <summary>
        /// Lehetséges lépések listályának elérése.
        /// </summary>
        public List<Int32> ValidMoves
        {
            get { return _validMoves; }
            set { _validMoves = value; }
        }
        /// <summary>
        /// Beállításokman szereplő játéktábla méretének elérése.
        /// </summary>
        public BoardSize TableSizeSetting { get { return _tableSizeSetting; } set { _tableSizeSetting = value; } }
        /// <summary>
        /// Jelenlegi játéktábla méretének lekérdezése.
        /// </summary>
        public BoardSize ActiveTableSize { get { return _activeTableSize; } }
        /// <summary>
        /// Jelenlegi játéktábla méretének lekérdezése int-ben.
        /// </summary>
        public Int32 ActiveTableSizeInt
        {
            get
            {
                Int32 size = 10;
                switch (_activeTableSize)
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
                }
                return size;
            }
        }
        /// <summary>
        /// Soron lévő játékos lekérdezése.
        /// </summary>
        public Boolean IsPlayerWhiteTurnOn { get { return _isPlayerWhiteTurnOn; } }

        /// <summary>
        /// Teszteléshez szükséges lekérdezések:
        /// </summary>
        public Board Board { get { return _board; } }
        public Boolean IsGameStarted { get { return _isGameStarted; } }
        public Boolean IsPassingTurnOn { get { return _isPassingTurnOn; } }
        
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
            _tableSizeSetting = BoardSize.Small;
            _isGameStarted = false;

            _board = new Board(10);
            _validMoves = new List<Int32>();
            _data = new ReversiGameInfo(10, _board, 0, 0, true);

            _timer = new System.Timers.Timer(1000.0);
            _timer.Elapsed += Timer_Elapsed;

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
            switch (_tableSizeSetting)
            {
                case BoardSize.Small:
                    size = BoardSizeSmall;
                    _activeTableSize = BoardSize.Small;
                    break;
                case BoardSize.Medium:
                    size = BoardSizeMedium;
                    _activeTableSize = BoardSize.Medium;
                    break;
                case BoardSize.Large:
                    size = BoardSizeLarge;
                    _activeTableSize = BoardSize.Large;
                    break;
            }
            _board = new Board(size);
            _validMoves.Clear();
            _isPlayerWhiteTurnOn = true;
            _data = new ReversiGameInfo(size, _board, 0, 0, true);
            _isPassingTurnOn = false;
            GetValidMoves(_isPlayerWhiteTurnOn);
            UpdateScore();
            _timer.Enabled = true;
            _isGameStarted = true;

        }
        /// <summary>
        /// Játék betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        public async Task LoadGame(String path)
        {
            _timer.Enabled = false;
            if (_dataAccess != null)
            {
                _data = await _dataAccess.LoadAsync(path);
            }
            Int32 size = 10;
            switch (_data.BoardSize)
            {
                case 10:
                    size = BoardSizeSmall;
                    _activeTableSize = BoardSize.Small;
                    break;
                case 20:
                    size = BoardSizeMedium;
                    _activeTableSize = BoardSize.Medium;
                    break;
                case 30:
                    size = BoardSizeLarge;
                    _activeTableSize = BoardSize.Large;
                    break;
            }
            _board = new Board(size);
            _board = _data.Board;
            _isGameStarted = true;
            _validMoves.Clear();
            _isPlayerWhiteTurnOn = _data.IsPlayerWhiteTurnOn;
            _isPassingTurnOn = false;
            GetValidMoves(_isPlayerWhiteTurnOn);
            if (_validMoves.Count == 0) { _isPassingTurnOn = true; }
            UpdateScore();
            _isGameStarted = true;
            _timer.Enabled = true;
        }

        /// <summary>
        /// Játék mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        public async Task SaveGame(String path)
        {
            _timer.Enabled = false;

            if (_dataAccess != null && _isGameStarted)
            {
                _data.BoardSize = _board.ScreenBoard.GetLength(0);
                _data.Board = _board;
                _data.IsPlayerWhiteTurnOn = _isPlayerWhiteTurnOn;
                await _dataAccess.SaveAsync(path, _data);
            }
            _timer.Enabled = true;
        }

        /// <summary>
        /// Aktualizálja a vizuális megjelenítést.
        /// </summary>
        public void UpdateView()
        {
            _board.CountColors();
            UpdateScore();
            OnUpdateBoard(new ReversiUpdateBoardEventArgs(_board.ScreenBoard.GetLength(0), _board, _whitePlayerPoints, _blackPlayerPoints, _isPlayerWhiteTurnOn, _isPassingTurnOn));
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
            if (_board != null)
            {
                for (int i = 0; i < _board.ScreenBoard.GetLength(0); i++)
                {
                    for (int j = 0; j < _board.ScreenBoard.GetLength(0); j++)
                    {
                        if (_board.IsValidMove(isPlayerWhiteTurnOn, i, j))
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
            if (_isGameStarted && _timer.Enabled)
            {
                if (CheckMove(isPlayerWhiteTurnOn, x, y))
                {
                    MakeMove(isPlayerWhiteTurnOn, x, y);
                }
                GetValidMoves(_isPlayerWhiteTurnOn);
                if (_validMoves.Count == 0)
                {
                    GetValidMoves(!_isPlayerWhiteTurnOn);
                    _isPassingTurnOn = true;
                }
                if (_validMoves.Count == 0)
                {
                    _timer.Enabled = false;
                    _isGameStarted = false;
                    UpdateScore();
                    OnSetGameEnded(new ReversiSetGameEndedEventArgs(_whitePlayerPoints, _blackPlayerPoints));
                }
            }
            OnUpdateBoard(new ReversiUpdateBoardEventArgs(_board.ScreenBoard.GetLength(0), _board, _whitePlayerPoints, _blackPlayerPoints, _isPlayerWhiteTurnOn, _isPassingTurnOn));
        }

        /// <summary>
        /// Az egyik játékos passzol.
        /// </summary>
        public void Pass()
        {
            if (_isGameStarted && _isPassingTurnOn && _timer.Enabled)
            {
                _isPlayerWhiteTurnOn = !_isPlayerWhiteTurnOn;
                GetValidMoves(_isPlayerWhiteTurnOn);
                OnUpdateBoard(new ReversiUpdateBoardEventArgs(_board.ScreenBoard.GetLength(0), _board, _whitePlayerPoints, _blackPlayerPoints, _isPlayerWhiteTurnOn, _isPassingTurnOn));
            }
            if (_isPassingTurnOn) { _isPassingTurnOn = false; }
        }

        /// <summary>
        /// Megállítja a játékot.
        /// </summary>
        public void Pause()
        {
            if (_isGameStarted) { _timer.Enabled = false; }
        }

        /// <summary>
        /// Újraindítja a játékot.
        /// </summary>
        public void Unpause()
        {
            if (_isGameStarted) { _timer.Enabled = true; }
        }


        /// <summary>
        /// Felszabadítja az erőforrásokat, amelyeket az osztály használ.
        /// </summary>
        public void Dispose()
        {
            _timer?.Dispose();

        }

        #endregion

        #region Private methods

        /// <summary>
        /// Aktualizálja a játékosok pontszámait.
        /// </summary>
        private void UpdateScore()
        {
            _blackPlayerPoints = _board.CountBlack;
            _whitePlayerPoints = _board.CountWhite;
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
            if ((row >= 0 && row <= _board.ScreenBoard.GetLength(0) && col >= 0 && col <= _board.ScreenBoard.GetLength(0)) &&
                _board.IsValidMove(isPlayerWhiteTurnOn, row, col))
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
            if (isPlayerWhiteTurnOn) { _isPlayerWhiteTurnOn = false; }
            else { _isPlayerWhiteTurnOn = true; }
        }

        /// <summary>
        /// Ténylegesen végrehajtja a lépést.
        /// </summary>
        /// <param name="isPlayerWhiteTurnOn">A soron lévő játékost jelzi.</param>
        /// <param name="row">A lehelyezés pozíciójának első koordinátája.</param>
        /// <param name="col">A lehelyezés pozíciójának második koordinátája.</param>
        private void MakeMove(Boolean isPlayerWhiteTurnOn, Int32 row, Int32 col)
        {
            _board.UpdateBoardState(isPlayerWhiteTurnOn, row, col);
            SwitchPlayer(isPlayerWhiteTurnOn);
            _board.CountColors();
            UpdateScore();
        }

        /// <summary>
        /// Frissíti a lehetséges lépések listáját.
        /// </summary>
        /// <param name="isPlayerWhiteTurnOn">A soron lévő játékost jelzi.</param>
        private void GetValidMoves(Boolean isPlayerWhiteTurnOn)
        {
            ValidMoves.Clear();

            for (int i = 0; i < _board.ScreenBoard.GetLength(0); i++)
            {
                for (int j = 0; j < _board.ScreenBoard.GetLength(0); j++)
                {
                    if (_board.IsValidMove(isPlayerWhiteTurnOn, i, j))
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
        private void OnUpdatePlayerTime(ReversiUpdatePlayerTimeEventArgs arg)
        {
            //if (UpdatePlayerTime != null) { UpdatePlayerTime(this, arg); } ugyanaz
            UpdatePlayerTime?.Invoke(this, arg);

        }

        /// <summary>
        /// Játéktábla frissítése eseményének kiváltása.
        /// </summary>
        /// <param name="arg">Játéktábla frissítése eseménykezelő argumentuma.</param>
        private void OnUpdateBoard(ReversiUpdateBoardEventArgs arg)
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
            if (_isPlayerWhiteTurnOn)
            {
                ++(_data.PlayerWhiteTime);
                OnUpdatePlayerTime(new ReversiUpdatePlayerTimeEventArgs(_isPlayerWhiteTurnOn, _data.PlayerWhiteTime));
            }
            else
            {
                ++(_data.PlayerBlackTime);
                OnUpdatePlayerTime(new ReversiUpdatePlayerTimeEventArgs(_isPlayerWhiteTurnOn, _data.PlayerBlackTime));
            }
        }
        #endregion


    }
}
