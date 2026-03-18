using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Reversi.Model
{
    /// <summary>
    /// Reversi játék vége eseményargumentum típusa.
    /// </summary>
    public class ReversiSetGameEndedEventArgs : EventArgs
    {
        private Int32 _playerWhitePoints;
        private Int32 _playerBlackPoints;

        /// <summary>
        /// A fehér pontjának lekérdezése.
        /// </summary>
        public Int32 PlayerWhitePoints
        {
            get { return _playerWhitePoints; }
        }

        /// <summary>
        /// A fekete pontjának lekérdezése.
        /// </summary>
        public Int32 PlayerBlackPoints
        {
            get { return _playerBlackPoints; }
        }

        /// <summary>
        /// Reversi játék vége eseményargumentum példányosítása.
        /// </summary>
        /// <param name="playerWhitePoints">Fehér játékos pontja.</param>
        /// <param name="playerBlackPoints">Fekete játékos pontja.</param>
        public ReversiSetGameEndedEventArgs(Int32 playerWhitePoints, Int32 playerBlackPoints)
        {
            _playerWhitePoints = playerWhitePoints;
            _playerBlackPoints = playerBlackPoints;
        }
    }
}

