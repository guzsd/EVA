using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi.Presistence
{
    /// <summary>
    /// Reversi adatelérés kivétel típusa.
    /// </summary>
    public class ReversiDataException : Exception
    {
        /// <summary>
        /// Reversi adatelérés kivétel példányosítása.
        /// </summary>
        public ReversiDataException() { }
    }
}
