namespace Reversi.Presistence
{
    /// <summary>
    /// Reversi játéktábla típusa.
    /// </summary>
    public class Board
    {
        #region Sings constants
        private const Int32 Sign_Empty = 0;
        private const Int32 Sign_White = 1;
        private const Int32 Sign_Black = 2;
        #endregion

        #region Fields
        #endregion

        #region Properties
        /// <summary>
        /// Fekete mezők számának elérése.
        /// </summary>
        public Int32 CountBlack { get; set; }

        /// <summary>
        /// Fehér mezők számának elérése.
        /// </summary>
        public Int32 CountWhite { get; set; }
        /// <summary>
        /// Üres mezők számának elérése.
        /// </summary>
        public Int32 CountEmpty { get; set; }
        /// <summary>
        /// A játéktábla elérése int tömbként.
        /// </summary>
        public Int32[,] ScreenBoard { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Reversi játéktábla példányosítása.
        /// </summary>
        /// <param name="size">Játéktábla mérete.</param>
        public Board(Int32 size)
        {
            if (size is not (10 or 20 or 30)) { throw new ArgumentException("Size not correct."); }

            CountWhite = 2;
            CountBlack = 2;
            CountEmpty = (size * size) - CountBlack - CountWhite;
            ScreenBoard = new Int32[size, size];

            for (Int32 i = 0; i < size; i++)
            {
                for (Int32 j = 0; j < size; j++)
                {
                    ScreenBoard[i, j] = Sign_Empty;
                }
            }
            Int32 s = size / 2;
            ScreenBoard[s - 1, s - 1] = Sign_White;
            ScreenBoard[s - 1, s] = Sign_Black;
            ScreenBoard[s, s - 1] = Sign_Black;
            ScreenBoard[s, s] = Sign_White;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Ellenőrzi a lépés lehetséges-e.
        /// </summary>
        /// <param name="isPlayerWhiteTurnOn">Soron lévő játékos.</param>
        /// <param name="row">Sor koordinátája.</param>
        /// <param name="col">Oszlop koordinátája.</param>
        /// <returns>Igaz, ha léphet oda a soron lévő játékos, egyébként hamis.</returns>
        public Boolean IsValidMove(Boolean isPlayerWhiteTurnOn, Int32 row, Int32 col)
        {
            Boolean isValid = false;

            if (ScreenBoard[row, col] != Sign_Empty)
            {
                isValid = false;
            }
            else
            {
                for (Int32 i = -1; i <= 1; i++)
                {
                    for (Int32 j = -1; j <= 1; j++)
                    {
                        if (!((i == 0) && (j == 0)) && HasPossibleToFlip(isPlayerWhiteTurnOn, row, col, i, j))
                        {
                            isValid = true;
                        }
                    }
                }
            }
            return isValid;
        }

        /// <summary>
        /// A lépés után minden irányba frissíti a táblát.
        /// </summary>
        /// <param name="isPlayerWhiteTurnOn">Soron lévő játékos.</param>
        /// <param name="row">Sor koordinátája.</param>
        /// <param name="col">Oszlop koordinátája.</param>
        public void UpdateBoardState(Boolean isPlayerWhiteTurnOn, Int32 row, Int32 col)
        {
            int sign = isPlayerWhiteTurnOn ? Sign_White : Sign_Black;
            ScreenBoard[row, col] = sign;
            for (Int32 i = -1; i <= 1; i++)
            {
                for (Int32 j = -1; j <= 1; j++)
                {
                    if (HasPossibleToFlip(isPlayerWhiteTurnOn, row, col, i, j))
                    {
                        Int32 rowToChange = row + i;
                        Int32 colToChange = col + j;

                        while (ScreenBoard[rowToChange, colToChange] != sign)
                        {
                            ScreenBoard[rowToChange, colToChange] = sign;
                            rowToChange += i;
                            colToChange += j;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Újraszámolja a pontokat.
        /// </summary>
        public void CountColors()
        {
            CountBlack = 0;
            CountEmpty = 0;
            CountWhite = 0;

            for (Int32 i = 0; i < ScreenBoard.GetLength(0); i++)
            {
                for (Int32 j = 0; j < ScreenBoard.GetLength(0); j++)
                {
                    if (ScreenBoard[i, j] == Sign_Black) { CountBlack++; }
                    else if (ScreenBoard[i, j] == Sign_White) { CountWhite++; }
                    else if (ScreenBoard[i, j] == Sign_Empty) { CountEmpty++; }
                }
            }
        }

        /// <summary>
        /// Visszaadja a tábla adott koordinátájú értékét.
        /// </summary>
        /// <param name="i">Sor koordinátája.</param>
        /// <param name="j">Oszlop koordinátája.</param>
        /// <returns>Az adott helyen lévő értéket.</returns>
        public Int32 GetValue(Int32 i, Int32 j)
        {
            return ScreenBoard[i, j];
        }

        /// <summary>
        /// A megadott koordinátára beállítja az értéket.
        /// </summary>
        /// <param name="x">Sor koordinátája.</param>
        /// <param name="y">Oszlop koordinátája.</param>
        /// <param name="value">Az új beállítandó érték.</param>
        public void SetValue(Int32 x, Int32 y, Int32 value)
        {
            ScreenBoard[x, y] = value;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Ellenőrzi hogy a lépésnél az adott irányba történik e forgatás.
        /// </summary>
        /// <param name="isPlayerWhiteTurnOn">Soron lévő játékos.</param>
        /// <param name="row">Sor koordinátája.</param>
        /// <param name="col">Oszlop koordinátája.</param>
        /// <param name="directionRow">Sor elmozdulásánk iránya.</param>
        /// <param name="directionCol">Oszlop elmozdulásának iránya.</param>
        /// <returns>Igaz, ha az adott irányba történik korongátfordítás, egyébként hamis.</returns>
        private Boolean HasPossibleToFlip(Boolean isPlayerWhiteTurnOn, Int32 row, Int32 col, Int32 directionRow, Int32 directionCol)
        {
            Int32 sign;
            Int32 oppsign;
            if (isPlayerWhiteTurnOn) { sign = Sign_White; oppsign = Sign_Black; }
            else { sign = Sign_Black; oppsign = Sign_White; }

            Boolean isPossible = true;
            Int32 rowToCheck = row + directionRow;
            Int32 colToCheck = col + directionCol;

            while (rowToCheck >= 0 && rowToCheck < ScreenBoard.GetLength(0) && colToCheck >= 0 && colToCheck < ScreenBoard.GetLength(0) &&
                (ScreenBoard[rowToCheck, colToCheck] == oppsign))
            {
                rowToCheck += directionRow;
                colToCheck += directionCol;
            }

            if (rowToCheck < 0 || rowToCheck >= ScreenBoard.GetLength(0) || colToCheck < 0 || colToCheck >= ScreenBoard.GetLength(0) ||
                (rowToCheck - directionRow == row && colToCheck - directionCol == col) || ScreenBoard[rowToCheck, colToCheck] != sign)// nem ment arrébb || üres-e a mező
            {
                isPossible = false;
            }

            return isPossible;
        }
        #endregion
    }
}
