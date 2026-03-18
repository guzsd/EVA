using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Reversi.Model
{
    /// <summary>
    /// Reversi játékos idejét frissítő eseményargumentum típusa.
    /// </summary>
    public class ReversiUpdatePlayerTimeEventArgs : EventArgs
    {
        private Boolean _isPlayerWhiteTimeNeedUpdate;
        private Int32 _newTime;

        /// <summary>
        /// A '_isPlayerWhiteTimeNeedUpdate' mező értékének lekérdezése.
        /// Ha igaz, a fehér játékos idejét kell frissíteni. Ha hamis, a fekete játékosét.
        /// </summary>

        public Boolean IsPlayerWhiteTimeNeedUpdate
        {
            get { return _isPlayerWhiteTimeNeedUpdate; }
        }

        /// <summary>
        /// A '_newTime' mező értékének lekérdezése. Az aktív játékos új időértéke.
        /// </summary>

        public Int32 NewTime
        {
            get { return _newTime; }
        }

        /// <summary>
        /// Reversi játékos idejét frissítő eseményargumentum példányosítása.
        /// </summary>
        /// <param name="isPlayerWhiteTimeNeedUpdate">Annak jelzése, hogy melyik játékos idejét kell frissíteni.</param>
        /// <param name="newTime">A játékos új ideje.</param>
        public ReversiUpdatePlayerTimeEventArgs(Boolean isPlayerWhiteTimeNeedUpdate, Int32 newTime)
        {
            _isPlayerWhiteTimeNeedUpdate = isPlayerWhiteTimeNeedUpdate;
            _newTime = newTime;
        }
    }
}
