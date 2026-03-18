using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
        private Int32[,] _board; //játéktábla
        private Int32 _countEmpty; //üres mezők száma
        private Int32 _countWhite; //fehér mezők száma
        private Int32 _countBlack; //fekete mezők száma
        #endregion

        #region Properties
        /// <summary>
        /// Fekete mezők számának elérése.
        /// </summary>
        public Int32 CountBlack
        {
            get { return _countBlack; }
            set { _countBlack = value; }
        }

        /// <summary>
        /// Fehér mezők számának elérése.
        /// </summary>
        public Int32 CountWhite
        {
            get { return _countWhite; }
            set { _countWhite = value; }
        }
        /// <summary>
        /// Üres mezők számának elérése.
        /// </summary>
        public Int32 CountEmpty
        {
            get { return _countEmpty; }
            set { _countEmpty = value; }
        }
        /// <summary>
        /// A játéktábla elérése int tömbként.
        /// </summary>
        public Int32[,] ScreenBoard
        {
            get { return _board; }
            set { _board = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Reversi játéktábla példányosítása.
        /// </summary>
        /// <param name="size">Játéktábla mérete.</param>
        public Board(Int32 size)
        {
            if (!(size == 10 || size == 20 || size == 30)) { throw new ArgumentException("Size not correct."); }

            _countWhite = 2;
            _countBlack = 2;
            _countEmpty = (size * size) - _countBlack - _countWhite;
            _board = new Int32[size, size];

            for (Int32 i = 0; i < size; i++)
            {
                for (Int32 j = 0; j < size; j++)
                {
                    _board[i, j] = Sign_Empty;
                }
            }
            Int32 s = size / 2;
            _board[(s - 1), (s - 1)] = Sign_White;
            _board[(s - 1), s] = Sign_Black;
            _board[s, (s - 1)] = Sign_Black;
            _board[s, s] = Sign_White;
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

            if (_board[row, col] != Sign_Empty)
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
            Int32 sign;
            if (isPlayerWhiteTurnOn) { sign = Sign_White; }
            else { sign = Sign_Black; }
            _board[row, col] = sign;
            for (Int32 i = -1; i <= 1; i++)
            {
                for (Int32 j = -1; j <= 1; j++)
                {
                    if (HasPossibleToFlip(isPlayerWhiteTurnOn, row, col, i, j))
                    {
                        Int32 rowToChange = row + i;
                        Int32 colToChange = col + j;

                        while (_board[rowToChange, colToChange] != sign)
                        {
                            _board[rowToChange, colToChange] = sign;
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
            _countBlack = 0;
            _countEmpty = 0;
            _countWhite = 0;

            for (Int32 i = 0; i < _board.GetLength(0); i++)
            {
                for (Int32 j = 0; j < _board.GetLength(0); j++)
                {
                    if (_board[i, j] == Sign_Black) { _countBlack++; }
                    else if (_board[i, j] == Sign_White) { _countWhite++; }
                    else if (_board[i, j] == Sign_Empty) { _countEmpty++; }
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
            return _board[i, j];
        }

        /// <summary>
        /// A megadott koordinátára beállítja az értéket.
        /// </summary>
        /// <param name="x">Sor koordinátája.</param>
        /// <param name="y">Oszlop koordinátája.</param>
        /// <param name="value">Az új beállítandó érték.</param>
        public void SetValue(Int32 x, Int32 y, Int32 value)
        {
            _board[x, y] = value;
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

            while (rowToCheck >= 0 && rowToCheck < _board.GetLength(0) && colToCheck >= 0 && colToCheck < _board.GetLength(0) &&
                (_board[rowToCheck, colToCheck] == oppsign))
            {
                rowToCheck += directionRow;
                colToCheck += directionCol;
            }

            if (rowToCheck < 0 || rowToCheck >= _board.GetLength(0) || colToCheck < 0 || colToCheck >= _board.GetLength(0) ||
                (rowToCheck - directionRow == row && colToCheck - directionCol == col) || _board[rowToCheck, colToCheck] != sign)// nem ment arrébb || üres-e a mező
            {
                isPossible = false;
            }

            return isPossible;
        }
        #endregion
    }
}
