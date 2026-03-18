using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi.Presistence
{
    /// <summary>
    /// Reversi fájl kezelő felülete.
    /// </summary>
    public interface IReversiDataAccess
    {
        /// <summary>
        /// Fájl betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <returns>A fájlból beolvasott adatok, amik szükségesek a mentett játék visszaállításához.</returns>
        Task<ReversiGameInfo> LoadAsync(String path);

        /// <summary>
        /// Fájl mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <param name="data">A A játék visszaállításához szükséges adatok.</param>
        Task SaveAsync(String path, ReversiGameInfo data);
    }

}
