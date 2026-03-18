using Reversi.Presistence;


namespace Reversi.Model
{
    /// <summary>
    /// Type of the Reversi update table event argument.
    /// </summary>
    /// <remarks>
    /// Reversi tábla frissítés eseményargumentum példányosítása.
    /// </remarks>
    /// <param name="boardSize">A játéktábla métere.</param>
    /// <param name="board">Játéktábla.</param>
    /// <param name="playerWhitePoints">Fehér játékos pontjai.</param>
    /// <param name="playerBlackPoints">Fekete játékos pontjai.</param>
    /// <param name="isPlayerWhiteTurnOn">Logikai változó, a fehér játékos soron van e.</param>
    /// <param name="isPassingTurnOn">Logikai váltpzó, a passzolás szükségességét jelzi.</param>
    public class ReversiUpdateBoardEventArgs(Int32 boardSize, Board board,
        Int32 playerWhitePoints, Int32 playerBlackPoints, Boolean isPlayerWhiteTurnOn, Boolean isPassingTurnOn) : EventArgs
    {
        #region Fields
        #endregion

        #region Properties
        /// <summary>
        /// Játéktábla méretének elérése.
        /// </summary>
        public Int32 BoardSize { get; set; } = boardSize;
        /// <summary>
        /// Játéktábla elérése.
        /// </summary>
        public Board Board { get; set; } = board;
        /// <summary>
        /// Fehér pontok lekérdezése.
        /// </summary>
        public Int32 PlayerWhitePoints { get; } = playerWhitePoints;
        /// <summary>
        /// Fekete pontok lekérdezése.
        public Int32 PlayerBlackPoints { get; } = playerBlackPoints;
        /// <summary>
        /// Soron lévő játékos lekérdezése.
        /// </summary>
        public Boolean IsPlayerWhiteTurnOn { get; } = isPlayerWhiteTurnOn;
        /// <summary>
        /// Passzolás állapotának lekérdezése.
        /// </summary>
        public Boolean IsPassingTurnOn { get; } = isPassingTurnOn;

        #endregion
    }
}
