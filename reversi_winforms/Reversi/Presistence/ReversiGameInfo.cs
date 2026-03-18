using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi.Presistence
{
    public class ReversiGameInfo
    {
        #region Fields
        private Int32 _boardSize; //játéktábla mérete
        private Board _board; //játéktábla
        private Int32 _playerWhiteTime; //fehér játékos eddigi gondolkodási ideje
        private Int32 _playerBlackTime; //fekete játékos eddigi gondolkodási ideje
        private Boolean _isPlayerWhiteTurnOn; //aktuális játékos
        #endregion

        #region Properties
        /// <summary>
        /// Játéktábla méretének elérése.
        /// </summary>
        public Int32 BoardSize
        {
            get { return _boardSize; }
            set { _boardSize = value; }
        }
        /// <summary>
        /// Játéktábla elérése.
        /// </summary>
        public Board Board
        {
            get { return _board; }
            set { _board = value; }
        }
        /// <summary>
        /// Fehér játékos gonolkodási idejének elérése másodpercekben.
        /// </summary>
        public Int32 PlayerWhiteTime
        {
            get { return _playerWhiteTime; }
            set { _playerWhiteTime = value; }
        }
        /// <summary>
        /// Fekete játékos gonolkodási idejének elérése másodpercekben.
        /// </summary>
        public Int32 PlayerBlackTime
        {
            get { return _playerBlackTime; }
            set { _playerBlackTime = value; }
        }

        /// <summary>
        /// Aktuális játékos elérése.
        /// </summary>
        public Boolean IsPlayerWhiteTurnOn
        {
            get { return _isPlayerWhiteTurnOn; }
            set { _isPlayerWhiteTurnOn = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// A játékinformációk példányosítása.
        /// </summary>
        /// <param name="boardSize">A játéktábla métere.</param>
        /// <param name="board">Játéktábla.</param>
        /// <param name="playerWhiteTime">Fehér eddigi gondolkodási ideje.</param>
        /// <param name="playerBlackTime">Fekete eddigi gondolkodási ideje.</param>
        /// <param name="isPlayerWhiteTurnOn">Logikai változó, a fehér játékos soron van e.</param>
        public ReversiGameInfo(Int32 boardSize, Board board, Int32 playerWhiteTime = 0, Int32 playerBlackTime = 0, Boolean isPlayerWhiteTurnOn = true)
        {
            _boardSize = boardSize;
            _board = new Board(_boardSize);
            // Mátrix értékek másolása
            for (int i = 0; i < _boardSize; i++)
            {
                for (int j = 0; j < _boardSize; j++)
                {
                    _board.SetValue(i, j, board.GetValue(i, j));
                }
            }
            _playerWhiteTime = playerWhiteTime;
            _playerBlackTime = playerBlackTime;
            _isPlayerWhiteTurnOn = isPlayerWhiteTurnOn;
        }
        #endregion
    }
}
