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
        /// Fájl betöltése.
        /// </summary>
        /// <param name="stream">Adatfolyam.</param>
        /// <returns>A fájlból beolvasott játéktábla.</returns>
        Task<ReversiGameInfo> LoadAsync(Stream stream);


        /// <summary>
        /// Fájl mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <param name="data">A A játék visszaállításához szükséges adatok.</param>
        Task SaveAsync(String path, ReversiGameInfo data);

        /// <summary>
        /// Fájl mentése.
        /// </summary>
        /// <param name="stream">Adatfolyam.</param>
        /// <param name="data">A A játék visszaállításához szükséges adatok.</param>
        Task SaveAsync(Stream stream, ReversiGameInfo data);
    }

}
