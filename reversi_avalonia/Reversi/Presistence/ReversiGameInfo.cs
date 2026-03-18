namespace Reversi.Presistence
{
    public class ReversiGameInfo
    {
        #region Fields
        #endregion

        #region Properties
        /// <summary>
        /// Játéktábla méretének elérése.
        /// </summary>
        public Int32 BoardSize { get; set; }
        /// <summary>
        /// Játéktábla elérése.
        /// </summary>
        public Board Board { get; set; }
        /// <summary>
        /// Fehér játékos gonolkodási idejének elérése másodpercekben.
        /// </summary>
        public Int32 PlayerWhiteTime { get; set; }
        /// <summary>
        /// Fekete játékos gonolkodási idejének elérése másodpercekben.
        /// </summary>
        public Int32 PlayerBlackTime { get; set; }

        /// <summary>
        /// Aktuális játékos elérése.
        /// </summary>
        public Boolean IsPlayerWhiteTurnOn { get; set; }
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
            BoardSize = boardSize;
            Board = new Board(BoardSize);
            // Mátrix értékek másolása
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    Board.SetValue(i, j, board.GetValue(i, j));
                }
            }
            PlayerWhiteTime = playerWhiteTime;
            PlayerBlackTime = playerBlackTime;
            IsPlayerWhiteTurnOn = isPlayerWhiteTurnOn;
        }
        #endregion
    }
}
