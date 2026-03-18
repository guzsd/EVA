using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi_WPF.ViewModel
{
    /// <summary>
    /// Reversi játékmező típusa.
    /// </summary>
    public class ReversiField : ViewModelBase
    {
        private Boolean _enabled; //nem történik semmi kattintáskor
        private String _text;// = String.Empty; //a lehetséges lerakások jelzéséhez

        private Int32 _textColorInt; //megjelenítendő szöveg színe
        private Int32 _backColorInt; //megjelenítendő táttér színe
        private Int32 _x; // x koordináta
        private Int32 _y; //y koordináta
        private Int32 _index; //a mező indexe


        private DelegateCommand _putDownCommand;


        #region Properties

        public Boolean Enabled
        {
            get { return _enabled; }
            set {
                if (_enabled != value) 
                {
                    _enabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public String Text
        {
            get { return _text; }
            set
            {
                if (_text != value) 
                {
                    _text = value;
                    OnPropertyChanged();
                }
            }
        }

        public Int32 TextColorInt
        {
            get { return _textColorInt; }

            set
            {
                if (_textColorInt != value) 
                {
                    _textColorInt = value;
                    OnPropertyChanged();
                }
            }
        }

        public Int32 BackColorInt
        {
            get { return _backColorInt; }

            set
            {
                if (_backColorInt != value) 
                {
                    _backColorInt = value;
                    OnPropertyChanged();
                }
            }
        }

        public Int32 X
        {
            get { return _x; }
        }

        public Int32 Y
        {
            get { return _y; }
        }
        public Int32 Index
        {
            get { return _index; }
        }

        public DelegateCommand PutDownCommand
        {
            get { return _putDownCommand; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Reversi mező példányosítása.
        /// </summary>
        /// /// <param name="putDown">A delegált, amelyet kattintáskor végrehajtunk.</param>
        /// <param name="x">A cella X koordinátája a rácsban.</param>
        /// <param name="y">A cella Y koordinátája a rácsban.</param>
        /// <param name="index">A cella indexe a rácsban.</param>
        /// <param name="enabled">Ha hamis, a gombra kattintani lehet, de a modell nem végez műveletet.</param>
        /// <param name="text">Azt mutatja, hogy lehetséges-e lerakás.</param>
        /// <param name="textColorInt">Megjelenő szöveg színe.</param>
        /// <param name="backColorInt">A gomb háttérszíne.</param>
        public ReversiField(DelegateCommand putDown, Int32 x, Int32 y, Int32 index, Boolean enabled = false, String text = "", Int32 textColorInt = 10, Int32 backColorInt = 10)
        {
            _putDownCommand = putDown;
            _x = x;
            _y = y;
            _index = index;
            _enabled = enabled;
            _text = text;
            _textColorInt = textColorInt;
            _backColorInt = backColorInt;
        }

        #endregion



    }
}
