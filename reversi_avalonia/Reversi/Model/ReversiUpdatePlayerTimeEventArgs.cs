namespace Reversi.Model
{
    /// <summary>
    /// Reversi játékos idejét frissítő eseményargumentum típusa.
    /// </summary>
    /// <remarks>
    /// Reversi játékos idejét frissítő eseményargumentum példányosítása.
    /// </remarks>
    /// <param name="isPlayerWhiteTimeNeedUpdate">Annak jelzése, hogy melyik játékos idejét kell frissíteni.</param>
    /// <param name="newTime">A játékos új ideje.</param>
    public class ReversiUpdatePlayerTimeEventArgs(Boolean isPlayerWhiteTimeNeedUpdate, Int32 newTime) : EventArgs
    {

        /// <summary>
        /// A '_isPlayerWhiteTimeNeedUpdate' mező értékének lekérdezése.
        /// Ha igaz, a fehér játékos idejét kell frissíteni. Ha hamis, a fekete játékosét.
        /// </summary>

        public Boolean IsPlayerWhiteTimeNeedUpdate { get; } = isPlayerWhiteTimeNeedUpdate;

        /// <summary>
        /// A '_newTime' mező értékének lekérdezése. Az aktív játékos új időértéke.
        /// </summary>

        public Int32 NewTime { get; } = newTime;
    }
}
