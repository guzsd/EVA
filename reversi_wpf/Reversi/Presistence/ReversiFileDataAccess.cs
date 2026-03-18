using Reversi.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi.Presistence
{
    /// <summary>
    /// Reversi fájlkezelő típusa.
    /// </summary>
    public class ReversiFileDataAccess : IReversiDataAccess
    {
        #region Public methodes
        /// <summary>
        /// Fájl betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <returns>A fájlból beolvasott adatok, amik szükségesek a mentett játék visszaállításához.</returns>
        public async Task<ReversiGameInfo> LoadAsync(String path)
        {
            try
            {
                using (StreamReader reader = new(path)) // fájl megnyitása
                {
                    // egy sor beolvasása, és széttördelése a szóközök mentén
                    String line = await reader.ReadLineAsync() ?? String.Empty;
                    char[] separatingChars = { ' ' };
                    String[] datas = line.Split(separatingChars, StringSplitOptions.RemoveEmptyEntries);

                    if (datas.Length < 4) throw new Exception("Invalid data format.");
                    Int32 boardSize = Int32.Parse(datas[0]); // beolvassuk a tábla méretét
                    Int32 playerWhiteTime = Int32.Parse(datas[1]); // beolvassuk a fehér játékos idejét
                    Int32 playerBlackTime = Int32.Parse(datas[2]); // beolvassuk a fekete játékos idejét
                    Boolean isPlayerWhiteTurnOn = Boolean.Parse(datas[3]); // beolvassuk a soron lévő játékost
                    Board board = new Board(boardSize); // létrehozzuk a táblát
                    for (Int32 i = 0; i < boardSize; i++)
                    {
                        line = await reader.ReadLineAsync() ?? String.Empty;
                        datas = line.Split(separatingChars, StringSplitOptions.RemoveEmptyEntries);
                        for (Int32 j = 0; j < boardSize; j++)
                        {
                            board.SetValue(j, i, Int32.Parse(datas[j]));
                        }
                    }
                    ReversiGameInfo data = new ReversiGameInfo(boardSize, board, playerWhiteTime, playerBlackTime, isPlayerWhiteTurnOn); // létrehozzuk a visszatérési értéket

                    return data;
                }
            }
            catch
            {
                throw new ReversiDataException();
            }
        }

        /// <summary>
        /// Fájl mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <param name="data">A A játék visszaállításához szükséges adatok.</param>
        public async Task SaveAsync(String path, ReversiGameInfo data)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path)) // fájl megnyitása
                {
                    // kiírjuk az a adtokat: tábla mérete, fehér játékos ideje, fekete játékos ideje, soron lévő játékos
                    writer.Write(data.BoardSize.ToString() + " " + data.PlayerWhiteTime.ToString() + " " + data.PlayerBlackTime.ToString() + " " + data.IsPlayerWhiteTurnOn.ToString());
                    await writer.WriteLineAsync();
                    //kimentjük a tábla értékeit
                    for (Int32 i = 0; i < data.BoardSize; i++)
                    {
                        for (Int32 j = 0; j < data.BoardSize; j++)
                        {
                            await writer.WriteAsync(data.Board.ScreenBoard[j, i].ToString() + " ");
                        }
                        await writer.WriteLineAsync();
                    }
                }
            }
            catch
            {
                throw new ReversiDataException();
            }
        }
        #endregion
    }
}
