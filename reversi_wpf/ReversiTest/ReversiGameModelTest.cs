using System;
using System.Threading.Tasks;
using Reversi.Model;
using Reversi.Presistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Windows;

namespace ReversiTest
{
    [TestClass]
    public class ReversiGameModelTest : IDisposable
    {
        private ReversiGameModel _model = null!; // A tesztelendõ modell
        private Board _mockedBoard = null!; // Mockolt játéktábla
        private Mock<IReversiDataAccess> _mock = null!; // Adatelérés mockja
        private System.Timers.Timer? _timer; // idõ
        private Boolean eventTriggered = false;

       
        [TestInitialize]
        public void Initialize()
        {
            // A mock játéktábla inicializálása tesztadattal
            _mockedBoard = new Board(10); // Kezdésként egy 10x10-es táblával
            
            // Az adatelérés mockolása
            _mock = new Mock<IReversiDataAccess>();
            _mock.Setup(mock => mock.LoadAsync(It.IsAny<String>()))
                .Returns(() => Task.FromResult(new ReversiGameInfo(10, _mockedBoard, 0, 0, true)));

            // A modell példányosítása a mock adateléréssel
            _model = new ReversiGameModel(_mock.Object);

            _timer = new System.Timers.Timer(1000.0);

            _model.UpdateBoard += (sender, args) => { eventTriggered = true; };
            
        }

        [TestMethod]
        public void ReversiGameModelNewGameSmallTest()
        {
            _model.TableSizeSetting = BoardSize.Small;
            _model.NewGame();

            Assert.AreEqual(BoardSize.Small, _model.ActiveTableSize); // ellenõrzi a tábla méretét
            Assert.IsTrue(_model.IsPlayerWhiteTurnOn); // az elsõ játékos a fehér
            Assert.AreEqual(4, _model.ValidMoves.Count / 2); // kezdéskon mindíg 4 lépési lehetõség van = 8, mert a koordináták külön vannak eltárolva
            Assert.AreEqual(2, _model.Board.CountWhite); // ellenõrzi a kezdeti állást
            Assert.AreEqual(2, _model.Board.CountBlack); // ellenõrzi a kezdeti állást
            Assert.AreEqual(96, _model.Board.CountEmpty);  // ellenõrzi a kezdeti állást

        }

        [TestMethod]
        public void ReversiGameModelNewGameMediumTest()
        {
            _model.TableSizeSetting = BoardSize.Medium;
            _model.NewGame();

            Assert.AreEqual(BoardSize.Medium, _model.ActiveTableSize); // ellenõrzi a tábla méretét
            Assert.IsTrue(_model.IsPlayerWhiteTurnOn); // az elsõ játékos a fehér
            Assert.AreEqual(4, _model.ValidMoves.Count / 2); // kezdéskon mindíg 4 lépési lehetõség van
            Assert.AreEqual(2, _model.Board.CountWhite); // ellenõrzi a kezdeti állást
            Assert.AreEqual(2, _model.Board.CountBlack); // ellenõrzi a kezdeti állást
            Assert.AreEqual(396, _model.Board.CountEmpty);  // ellenõrzi a kezdeti állást
        }

        [TestMethod]
        public void ReversiGameModelNewGameLargeTest()
        {
            _model.TableSizeSetting = BoardSize.Large;
            _model.NewGame();

            Assert.AreEqual(BoardSize.Large, _model.ActiveTableSize); // ellenõrzi a tábla méretét
            Assert.IsTrue(_model.IsPlayerWhiteTurnOn); // az elsõ játékos a fehér
            Assert.AreEqual(4, _model.ValidMoves.Count / 2); // kezdéskon mindíg 4 lépési lehetõség van
            Assert.AreEqual(2, _model.Board.CountWhite); // ellenõrzi a kezdeti állást
            Assert.AreEqual(2, _model.Board.CountBlack); // ellenõrzi a kezdeti állást
            Assert.AreEqual(896, _model.Board.CountEmpty);  // ellenõrzi a kezdeti állást
        }

        [TestMethod]
        public void ReversiGameModelStepTest()
        {
            // Kezdeti ellenõrzés, hogy még nem indult el a játék
            Assert.IsFalse(_model.IsGameStarted);

            // Játék elindítása
            _model.TableSizeSetting = BoardSize.Small;
            _model.NewGame();

            // Ellenõrizzük, hogy az elsõ játékos fehér
            Assert.IsTrue(_model.IsPlayerWhiteTurnOn);
            Assert.AreEqual(BoardSize.Small, _model.ActiveTableSize); // Alapértelmezett tábla méret

            // Választunk egy véletlenszerû lépést a lehetséges lépések közül
            Random random = new Random();
            Int32 x, y, index;

            // Lépés végrehajtása többször, hogy ellenõrizzük a helyes mûködést
            for (Int32 i = 0; i < 100; i++) // Szimulálunk több lépést
            {


                // Ellenõrizzük, hogy a játékosok felváltva lépnek
                Assert.AreEqual(i % 2 == 0, _model.IsPlayerWhiteTurnOn);

                if (_model.IsPassingTurnOn) { _model.Pass(); }
                else if (_model.ValidMoves.Count > 2)
                {

                    index = random.Next(0, _model.ValidMoves.Count / 2) * 2;
                    x = _model.ValidMoves[index];
                    y = _model.ValidMoves[index + 1];

                    _model.PutDown(_model.IsPlayerWhiteTurnOn, x, y);

                    if (!_model.IsPlayerWhiteTurnOn)
                    {
                        Assert.AreEqual(1, _model.Board.GetValue(x, y));
                    }
                    else { Assert.AreEqual(2, _model.Board.GetValue(x, y)); }
                }
                else { i = 100; }
                //Console.WriteLine("Iteráció: " + i);
                //Console.WriteLine("Érvényes lépések száma lépés után: " + _model.ValidMoves.Count);
                Assert.IsTrue(eventTriggered);
            }
        }

        [TestMethod]
        public async Task ReversiGameModelLoadTest()
        {
            _model.TableSizeSetting = BoardSize.Small;
            _model.NewGame();
            _model.PutDown(true, 3, 5);
            _model.PutDown(false, 3, 6);
            _model.PutDown(true, 6, 4);
            await _model.LoadGame(String.Empty);

            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {
                    Assert.AreEqual(_mockedBoard.GetValue(i, j), _model.Board.GetValue(i, j));
                }
        }

        [TestMethod]
        public void Timer_ShouldTriggerElapsedEvent()
        { 
            bool eventTriggered = false;
            _model.UpdatePlayerTime += (sender, args) => { eventTriggered = true; };
            _model.NewGame(); 
            Thread.Sleep(1100); 
            Assert.IsTrue(eventTriggered);

        }

        public void Dispose()
        {
            _timer?.Dispose();

        }

        
    }
}