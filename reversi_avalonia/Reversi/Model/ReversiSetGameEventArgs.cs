namespace Reversi.Model
{
    /// <summary>
    /// Reversi játék vége eseményargumentum típusa.
    /// </summary>
    /// <remarks>
    /// Reversi játék vége eseményargumentum példányosítása.
    /// </remarks>
    /// <param name="playerWhitePoints">Fehér játékos pontja.</param>
    /// <param name="playerBlackPoints">Fekete játékos pontja.</param>
    public class ReversiSetGameEndedEventArgs(Int32 playerWhitePoints, Int32 playerBlackPoints) : EventArgs
    {

        /// <summary>
        /// A fehér pontjának lekérdezése.
        /// </summary>
        public Int32 PlayerWhitePoints { get; } = playerWhitePoints;

        /// <summary>
        /// A fekete pontjának lekérdezése.
        /// </summary>
        public Int32 PlayerBlackPoints { get; } = playerBlackPoints;
    }
}

