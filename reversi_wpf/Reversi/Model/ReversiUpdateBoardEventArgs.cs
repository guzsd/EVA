using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Reversi.Presistence;


namespace Reversi.Model
{
    /// <summary>
    /// Type of the Reversi update table event argument.
    /// </summary>
    public class ReversiUpdateBoardEventArgs : EventArgs
    {
        #region Fields
        private Int32 _boardSize; // játéktábla mérete
        private Board _board; // játéktábla
        private Int32 _playerWhitePoints; // fehér játékos pontjai
        private Int32 _playerBlackPoints; // fekete játékos pontjai
        private Boolean _isPlayerWhiteTurnOn; // jelenleg soron lévő játékos
        private Boolean _isPassingTurnOn; // passszolás aktívitása
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
        /// Fehér pontok lekérdezése.
        /// </summary>
        public Int32 PlayerWhitePoints
        {
            get { return _playerWhitePoints; }
        }
        /// <summary>
        /// Fekete pontok lekérdezése.
        public Int32 PlayerBlackPoints
        {
            get { return _playerBlackPoints; }
        }
        /// <summary>
        /// Soron lévő játékos lekérdezése.
        /// </summary>
        public Boolean IsPlayerWhiteTurnOn
        {
            get { return _isPlayerWhiteTurnOn; }
        }
        /// <summary>
        /// Passzolás állapotának lekérdezése.
        /// </summary>
        public Boolean IsPassingTurnOn
        {
            get { return _isPassingTurnOn; }
        }
        #endregion
        #region Constructor
        /// <summary>
        /// Reversi tábla frissítés eseményargumentum példányosítása.
        /// </summary>
        /// <param name="boardSize">A játéktábla métere.</param>
        /// <param name="board">Játéktábla.</param>
        /// <param name="playerWhitePoints">Fehér játékos pontjai.</param>
        /// <param name="playerBlackPoints">Fekete játékos pontjai.</param>
        /// <param name="isPlayerWhiteTurnOn">Logikai változó, a fehér játékos soron van e.</param>
        /// <param name="isPassingTurnOn">Logikai váltpzó, a passzolás szükségességét jelzi.</param>
        public ReversiUpdateBoardEventArgs(Int32 boardSize, Board board,
            Int32 playerWhitePoints, Int32 playerBlackPoints, Boolean isPlayerWhiteTurnOn, Boolean isPassingTurnOn)
        {
            _boardSize = boardSize;
            _board = board;
            /*
            for (int i = 0; i < _boardSize; i++)
            {
                for (int j = 0; j < _boardSize; j++)
                {
                    _board.SetValue(i, j, board.GetValue(i, j));
                }
            }*/
            _playerWhitePoints = playerWhitePoints;
            _playerBlackPoints = playerBlackPoints;
            _isPlayerWhiteTurnOn = isPlayerWhiteTurnOn;
            _isPassingTurnOn = isPassingTurnOn;
        }
        #endregion
    }
}
