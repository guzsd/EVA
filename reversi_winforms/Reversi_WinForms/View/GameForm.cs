using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Reversi.Model;
using Reversi.Presistence;

namespace Reversi_WinForms.View
{
    /// <summary>
    /// Játékablak típusa.
    /// </summary>
    public partial class GameForm : Form
    {
        #region Fields

        private ReversiGameModel _model = null!; //játékmodell
        private Boolean _isPlayerWhiteTurnOn; // soron lévõ játékos
        private Button[,] _buttonGrid = null!; //gombrács
        private Boolean _saved; // mentés állapota

        #endregion

        #region Constructors
        /// <summary>
        /// Játékablak példányosítása.
        /// </summary>
        public GameForm()
        {
            InitializeComponent();
            // adatelérés példányosítása
            IReversiDataAccess _dataAccess = new ReversiFileDataAccess();
            // modell létrehozása és az eseménykezelõk társítása
            _model = new ReversiGameModel(_dataAccess);
            _model.SetGameEnded += new EventHandler<ReversiSetGameEndedEventArgs>(Model_SetGameEnded);
            _model.UpdatePlayerTime += new EventHandler<ReversiUpdatePlayerTimeEventArgs>(Model_UpdatePlayerTime);
            _model.UpdateBoard += new EventHandler<ReversiUpdateBoardEventArgs>(Model_UpdateTable);

            _saved = true;
        }

        #endregion

        #region Menu event handlers


        /// <summary>
        /// Kilépés eseménykezelõje.
        /// </summary>
        private void ExitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Kilépés eseménykezelõje.
        /// </summary>

        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _model.Pause();

            if (_saved == false)
            {
                if (MessageBox.Show("Are you sure you want to exit?", "Reversi game", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                    _model.Unpause();
                }
            }
        }

        /// <summary>
        /// Játék betöltésének eseménykezelõje.
        /// </summary>
        private async void LoadToolStripMenuItem1_ClickAsync(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await _model.LoadGame(openFileDialog.FileName);
                    SetButtonGridUp();
                    _model.UpdateView();
                    _saved = true;
                    saveToolStripMenuItem1.Enabled = true;
                    buttonPause.Enabled = true;
                    buttonPause.Text = "Pause";
                }
                catch (ReversiDataException)
                {
                    MessageBox.Show("Játék betöltése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a fájlformátum.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Játék mentésének eseménykezelõje.
        /// </summary>
        private async void SaveToolStripMenuItem1_ClickAsync(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await _model.SaveGame(saveFileDialog.FileName);
                    _saved = true;
                    buttonPause.Enabled = true;
                    buttonPause.Text = "Pause";
                }
                catch (ReversiDataException)
                {
                    MessageBox.Show("Játék mentése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Új játék betöltésének eseménykezelõje.
        /// </summary>
        private void NewToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _model.NewGame();
            saveToolStripMenuItem1.Enabled = true;
            _saved = false;
            buttonPause.Enabled = true;
            buttonPause.Text = "Pause";
            SetButtonGridUp();
            _model.UpdateView();
        }

        /// <summary>
        /// Játékszabály betöltésének eseménykezelõje.
        /// </summary>
        private void ToolStripMenuItemRules_Click(object sender, EventArgs e)
        {
            _model.Pause();

            MessageBox.Show(this, "Each player has a color, and the aim of the game is to get more of your pieces on the board than the opponent's pieces. " + Environment.NewLine +
                "At the start of the game there are four pieces on the board, two white and two black. You must try to capture opponent pieces and flip them" +
                "over so they turn into your color. You do this by making a horizontal, vertical or diagonal line of pieces, where your pieces surround the other player's pieces. " +
                "The surrounded opponent pieces are then captured and will be flipped over to your color, increasing the number of your pieces on the board." + Environment.NewLine +
                "Every move you make must capture some opponent pieces. This means that every time it's your turn you must place one of your pieces next to a line of one or more opponent pieces," +
                " where there's another one of your pieces at the other end of the line. " + Environment.NewLine + "If there is no available move on the board that captures pieces then you must say Pass " +
                "and your opponent gets to play again. If both players say pass in a row then there are no more moves on the board and the game ends. ");

            _model.Unpause();
        }


        private void LargeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _model.TableSizeSetting = BoardSize.Large;
            smallToolStripMenuItem.Enabled = true;
            smallToolStripMenuItem.Checked = false;

            mediumToolStripMenuItem.Enabled = true;
            mediumToolStripMenuItem.Checked = false;

            largeToolStripMenuItem.Enabled = false;
            largeToolStripMenuItem.Checked = true;
        }

        private void MediumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _model.TableSizeSetting = BoardSize.Medium;
            smallToolStripMenuItem.Enabled = true;
            smallToolStripMenuItem.Checked = false;

            mediumToolStripMenuItem.Enabled = false;
            mediumToolStripMenuItem.Checked = true;

            largeToolStripMenuItem.Enabled = true;
            largeToolStripMenuItem.Checked = false;
        }
        private void SmallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _model.TableSizeSetting = BoardSize.Small;
            smallToolStripMenuItem.Enabled = false;
            smallToolStripMenuItem.Checked = true;

            mediumToolStripMenuItem.Enabled = true;
            mediumToolStripMenuItem.Checked = false;

            largeToolStripMenuItem.Enabled = true;
            largeToolStripMenuItem.Checked = false;
        }

        #endregion


        #region Button clicks

        /// <summary>
        /// Pause gomb eseménykezelõje.
        /// </summary>
        private void buttonPause_Click(object sender, EventArgs e)
        {
            if (buttonPause.Text == "Pause")
            {
                buttonPause.Text = "Continue";
                _model.Pause();
            }
            else if (buttonPause.Text == "Continue")
            {
                buttonPause.Text = "Pause";
                _model.Unpause();
            }
        }

        /// <summary>
        /// Pass gomb eseménykezelõje.
        /// </summary>
        private void buttonPass_Click(object sender, EventArgs e)
        {
            _saved = false;
            buttonPass.Enabled = false;

            _model.Pass();
            _model.UpdateView();
        }
        #endregion


        #region Model event handlers

        /// <summary>
        /// Játék vége eseménykezelõje.
        /// </summary>
        private void Model_SetGameEnded(Object? sender, ReversiSetGameEndedEventArgs e)
        {
            buttonPause.Enabled = false;
            buttonPass.Enabled = false;
            _saved = true;
            saveToolStripMenuItem1.Enabled = false;
            _model.UpdateView();
            if (e.PlayerWhitePoints > e.PlayerBlackPoints)
            {
                MessageBox.Show("Player White won." + Environment.NewLine + "Player White points: " + e.PlayerWhitePoints.ToString() + Environment.NewLine + "Player Black points: " + e.PlayerBlackPoints.ToString() + Environment.NewLine + "Game Ended!");
            }
            else if (e.PlayerWhitePoints < e.PlayerBlackPoints)
            {
                MessageBox.Show("Player Black won." + Environment.NewLine + "Player White points: " + e.PlayerWhitePoints.ToString() + Environment.NewLine + "Player Black points: " + e.PlayerBlackPoints.ToString() + Environment.NewLine + "Game Ended!");
            }
            else
            {
                MessageBox.Show("It is a tie." + Environment.NewLine + "Player White points: " + e.PlayerWhitePoints.ToString() + Environment.NewLine + "Player Black points: " + e.PlayerBlackPoints.ToString() + Environment.NewLine + "Game Ended!");
            }



        }

        //// <summary>
        /// Játékosidõ frissítésének eseménykezelõje.
        /// </summary>
        private void Model_UpdatePlayerTime(Object? sender, ReversiUpdatePlayerTimeEventArgs e)
        {
            if (e.IsPlayerWhiteTimeNeedUpdate)
            {
                labelWhiteTime.Invoke((MethodInvoker)(() => labelWhiteTime.Text = e.NewTime.ToString()));
            }
            else
            {
                labelBlackTime.Invoke((MethodInvoker)(() => labelBlackTime.Text = e.NewTime.ToString()));
            }
        }

        /// <summary>
        /// Játéktábla aktualizálásának eseménykezelõje.
        /// </summary>
        private void Model_UpdateTable(Object? sender, ReversiUpdateBoardEventArgs e)
        {
            toolStripStatusLabelWhitePoint.Text = e.PlayerWhitePoints.ToString();
            toolStripStatusLabelBlackPoint.Text = e.PlayerBlackPoints.ToString();
            _isPlayerWhiteTurnOn = e.IsPlayerWhiteTurnOn;
            buttonPass.Enabled = e.IsPassingTurnOn;
            List<Int32> _validMoves = _model.ValidMoves;
            Board boardd = e.Board;
            Int32 value;

            for (Int32 i = 0; i < e.BoardSize; i++)
            {
                for (Int32 j = 0; j < e.BoardSize; j++)
                {
                    value = boardd.ScreenBoard[i, j];
                    if (IsInValidMoves(i, j, _validMoves))
                    {
                        value = 5;
                        if (e.IsPassingTurnOn)
                        {
                            if (_isPlayerWhiteTurnOn)
                            {
                                value = 3;
                            }
                            else { value = 4; }
                        }
                    }
                    UpdateButtonGrid(i, j, value);
                }
            }
        }

        #endregion

        #region Private method

        /// <summary>
        /// Megvizsgálja, hogy a lehetséges lépések közé tartozik.
        /// </summary>
        /// <param name="x">A pozíció elsõ koordinátája.</param>
        /// <param name="y">A oozíció második koordinátája.</param>
        /// <param name="validmoves">A lehetséges lépések listája.</param>
        /// <returns>Igaz, ha a valid lépések eggyike, egyébként hamis.</returns>
        private Boolean IsInValidMoves(Int32 x, Int32 y, List<Int32> validmoves)
        {
            Boolean init = false;
            for (Int32 i = 0; i < validmoves.Count; i = i + 2)
            {
                if (validmoves[i] == x && validmoves[i + 1] == y)
                {
                    init = true;
                }
            }
            return init;
        }

        /// <summary>
        /// Az összes játék gomb kattintási eseménykezelõje.
        /// </summary>
        /// <param name="sender">Az egyik játék gomb.</param>
        /// <param name="e"></param>
        private void GameButton_Clicked(object? sender, EventArgs e)
        {
            if (sender is Button button)
            {
                _saved = false;
                Int32 x = (button.TabIndex - 1000) / _model.ActiveTableSizeInt;
                Int32 y = (button.TabIndex - 1000) % _model.ActiveTableSizeInt;

                _model.PutDown(_model.IsPlayerWhiteTurnOn, x, y);
            }
        }

        /// <summary>
        /// A játék gombjainak generálása, és ezek elmentése.
        /// </summary>
        private void SetButtonGridUp()
        {
            if (_buttonGrid == null || _model.ActiveTableSizeInt != _buttonGrid.GetLength(0))
            {
                Int32 usedGameButtonSize = 30;
                if (_model.ActiveTableSize == BoardSize.Large) { usedGameButtonSize = 25; }
                else if (_model.ActiveTableSize == BoardSize.Small) { usedGameButtonSize = 45; }

                panel1.Controls.Clear();
                _buttonGrid = new Button[_model.ActiveTableSizeInt, _model.ActiveTableSizeInt];

                for (Int32 x = 0; x < _model.ActiveTableSizeInt; ++x)
                {
                    for (Int32 y = 0; y < _model.ActiveTableSizeInt; ++y)
                    {
                        _buttonGrid[x, y] = new Button();
                        _buttonGrid[x, y].Location = new Point(x + x * (usedGameButtonSize - 2), y + y * (usedGameButtonSize - 2));
                        _buttonGrid[x, y].Size = new Size(usedGameButtonSize, usedGameButtonSize);
                        _buttonGrid[x, y].Font = new Font(FontFamily.GenericSansSerif, 7, FontStyle.Bold);
                        _buttonGrid[x, y].TabIndex = 1000 + (x * _model.ActiveTableSizeInt) + y;
                        _buttonGrid[x, y].FlatStyle = FlatStyle.Popup;
                        _buttonGrid[x, y].BackColor = Color.Gray;
                        _buttonGrid[x, y].MouseClick += new MouseEventHandler(GameButton_Clicked);
                        _buttonGrid[x, y].CausesValidation = false;
                        _buttonGrid[x, y].TabStop = false;

                        panel1.Controls.Add(_buttonGrid[x, y]);
                    }
                }
                panel1.Location = new Point((this.ClientSize.Width - panel1.Width) / 2, 150);
                ClientSize = new Size(this.ClientSize.Width, panel1.Height + 200);
            }
        }

        /// <summary>
        /// Frissítjük a gombot, amelyet az X és Y koordináták jelölnek, az érték segítségével.
        /// </summary>
        /// <param name="x">A gomb elsõ koordinátája.</param>
        /// <param name="y">A gomb második koordinátája.</param>
        /// <param name="value">A modell által küldött érték.</param>
        private void UpdateButtonGrid(Int32 x, Int32 y, Int32 value)
        {
            switch (value)
            {
                case 0:
                    _buttonGrid[x, y].BackColor = Color.Gray;
                    _buttonGrid[x, y].FlatAppearance.BorderColor = Color.Gray;
                    _buttonGrid[x, y].Enabled = false;
                    _buttonGrid[x, y].Text = " ";
                    break;
                case 1:
                    _buttonGrid[x, y].BackColor = Color.White;
                    _buttonGrid[x, y].Enabled = false;
                    _buttonGrid[x, y].Text = " ";
                    break;
                case 2:
                    _buttonGrid[x, y].BackColor = Color.Black;
                    _buttonGrid[x, y].Enabled = false;
                    _buttonGrid[x, y].Text = " ";
                    break;
                case 3:
                    //ha  a fehérnek passzonia kell és a fekete jön
                    _buttonGrid[x, y].BackColor = Color.Gray;
                    _buttonGrid[x, y].FlatAppearance.BorderColor = Color.Gray;
                    _buttonGrid[x, y].Text = "X";
                    _buttonGrid[x, y].ForeColor = Color.Black;
                    break;
                case 4:
                    //ha  a feketének passzonia kell és a fehér jön
                    _buttonGrid[x, y].BackColor = Color.Gray;
                    _buttonGrid[x, y].FlatAppearance.BorderColor = Color.Gray;
                    _buttonGrid[x, y].Text = "X";
                    _buttonGrid[x, y].ForeColor = Color.White;
                    break;
                case 5:
                    _buttonGrid[x, y].Text = "X";
                    if (_isPlayerWhiteTurnOn)
                    {
                        _buttonGrid[x, y].ForeColor = Color.White;
                    }
                    else
                    {
                        _buttonGrid[x, y].ForeColor = Color.Black;
                    }
                    _buttonGrid[x, y].BackColor = Color.Gray;
                    _buttonGrid[x, y].Enabled = true;
                    break;
                default:
                    throw new Exception("Model gave us a number, that we was not ready for, while updating the table view.");
            }
        }

        #endregion

    }
}
