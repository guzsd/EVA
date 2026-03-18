using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;

namespace Reversi_Avalonia.ViewModels
{
    /// <summary>
    /// Reversi mező típusa.
    /// </summary>
    /// <remarks>
    /// Reversi mező példányosítása.
    /// </remarks>
    /// <param name="putDown">A kattintási parancs.</param>
    /// <param name="x">X koordináta.</param>
    /// <param name="y">Y koordináta.</param>
    /// <param name="index">A mező indexe.</param>
    /// <param name="text">Megjelenített szöveg.</param>
    /// <param name="textColorInt">Szöveg színe (integer).</param>
    /// <param name="backColorInt">Háttérszín (integer).</param>
    public class ReversiField(RelayCommand<int> putDown, int x, int y, int index, string text = " ", int textColorInt = 10, int backColorInt = 10) : INotifyPropertyChanged
    {
        #region Private Fields

        private string _text = text;            // A megjelenített szöveg
        private int _textColorInt = textColorInt;       // Szöveg színe (integer reprezentáció)
        private int _backColorInt = backColorInt;       // Háttérszín (integer reprezentáció)
        private RelayCommand<int> _putDownCommand = putDown; // Gombhoz kapcsolódó parancs

        #endregion

        #region Public Properties
        /// <summary>
        /// A mezőn megjelenített szöveg.
        /// </summary>
        public string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged(nameof(Text));
                }
            }
        }

        /// <summary>
        /// A szöveg színének integer értéke.
        /// Ha változik, frissíti a Foreground tulajdonságot.
        /// </summary>
        public int TextColorInt
        {
            get => _textColorInt;
            set
            {
                if (_textColorInt != value)
                {
                    _textColorInt = value;
                    OnPropertyChanged(nameof(TextColorInt));
                    OnPropertyChanged(nameof(Foreground));
                }
            }
        }
        /// <summary>
        /// A háttérszín integer értéke.
        /// Ha változik, frissíti a Background tulajdonságot.
        /// </summary>
        public int BackColorInt
        {
            get => _backColorInt;
            set
            {
                if (_backColorInt != value)
                {
                    _backColorInt = value;
                    OnPropertyChanged(nameof(BackColorInt));
                    OnPropertyChanged(nameof(Background));
                }
            }
        }
        /// <summary>
        /// A háttérszín szöveges reprezentációja a BackColorInt alapján.
        /// </summary>
        public string Background => BackColorInt switch
        {
            10 => "LightGray",
            20 => "White",
            30 => "Black",
            _ => "Transparent"
        };
        /// <summary>
        /// A szöveg színének szöveges reprezentációja a TextColorInt alapján.
        /// </summary>
        public string Foreground => TextColorInt switch
        {
            10 => "LightGray",
            20 => "White",
            30 => "Black",
            _ => "Transparent"
        };
        /// A mező X koordinátája.
        public int X { get; } = x;
        /// A mező Y koordinátája.
        public int Y { get; } = y;
        /// A mező indexe a listában.
        public int Index { get; } = index;

        /// A mezőhöz kapcsolódó parancs, amely kattintáskor fut le.
        public RelayCommand<int> PutDownCommand
        {
            get => _putDownCommand;
            set
            {
                if (_putDownCommand != value)
                {
                    _putDownCommand = value;
                    OnPropertyChanged(nameof(PutDownCommand));
                }
            }
        }

        #endregion
        #region Constructors

        #endregion

        #region INotifyPropertyChanged Implementation
        /// <summary>
        /// Akkor hívódik meg, amikor egy tulajdonság értéke megváltozik.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Kiváltja a "PropertyChanged" eseményt az adott tulajdonság nevével.
        /// </summary>

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
