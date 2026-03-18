using System;
using System.Drawing;
using System.Windows.Forms;

namespace Reversi_WinForms.View
{
    partial class GameForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            toolStripMenuItemGame = new ToolStripMenuItem();
            newToolStripMenuItem1 = new ToolStripMenuItem();
            saveToolStripMenuItem1 = new ToolStripMenuItem();
            loadToolStripMenuItem1 = new ToolStripMenuItem();
            exitToolStripMenuItem1 = new ToolStripMenuItem();
            toolStripMenuItemBoard = new ToolStripMenuItem();
            sizeToolStripMenuItem = new ToolStripMenuItem();
            smallToolStripMenuItem = new ToolStripMenuItem();
            mediumToolStripMenuItem = new ToolStripMenuItem();
            largeToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItemRules = new ToolStripMenuItem();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            groupBoxWhite = new GroupBox();
            labelWhiteTime = new Label();
            labelWhiteTimeText = new Label();
            groupBoxBlack = new GroupBox();
            labelBlackTime = new Label();
            labelBlackTimeText = new Label();
            buttonPass = new Button();
            buttonPause = new Button();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabelWhiteText = new ToolStripStatusLabel();
            toolStripStatusLabelWhitePoint = new ToolStripStatusLabel();
            toolStripStatusLabelBlackText = new ToolStripStatusLabel();
            toolStripStatusLabelBlackPoint = new ToolStripStatusLabel();
            openFileDialog = new OpenFileDialog();
            saveFileDialog = new SaveFileDialog();
            panel1 = new Panel();
            menuStrip1.SuspendLayout();
            groupBoxWhite.SuspendLayout();
            groupBoxBlack.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItemGame, toolStripMenuItemBoard, toolStripMenuItemRules });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(802, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItemGame
            // 
            toolStripMenuItemGame.DropDownItems.AddRange(new ToolStripItem[] { newToolStripMenuItem1, saveToolStripMenuItem1, loadToolStripMenuItem1, exitToolStripMenuItem1 });
            toolStripMenuItemGame.Name = "toolStripMenuItemGame";
            toolStripMenuItemGame.Size = new Size(62, 24);
            toolStripMenuItemGame.Text = "Game";
            // 
            // newToolStripMenuItem1
            // 
            newToolStripMenuItem1.Name = "newToolStripMenuItem1";
            newToolStripMenuItem1.Size = new Size(125, 26);
            newToolStripMenuItem1.Text = "New";
            newToolStripMenuItem1.Click += NewToolStripMenuItem1_Click;
            // 
            // saveToolStripMenuItem1
            // 
            saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
            saveToolStripMenuItem1.Size = new Size(125, 26);
            saveToolStripMenuItem1.Text = "Save";
            saveToolStripMenuItem1.Click += SaveToolStripMenuItem1_ClickAsync;
            // 
            // loadToolStripMenuItem1
            // 
            loadToolStripMenuItem1.Name = "loadToolStripMenuItem1";
            loadToolStripMenuItem1.Size = new Size(125, 26);
            loadToolStripMenuItem1.Text = "Load";
            loadToolStripMenuItem1.Click += LoadToolStripMenuItem1_ClickAsync;
            // 
            // exitToolStripMenuItem1
            // 
            exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            exitToolStripMenuItem1.Size = new Size(125, 26);
            exitToolStripMenuItem1.Text = "Exit";
            exitToolStripMenuItem1.Click += ExitToolStripMenuItem1_Click;
            // 
            // toolStripMenuItemBoard
            // 
            toolStripMenuItemBoard.DropDownItems.AddRange(new ToolStripItem[] { sizeToolStripMenuItem });
            toolStripMenuItemBoard.Name = "toolStripMenuItemBoard";
            toolStripMenuItemBoard.Size = new Size(63, 24);
            toolStripMenuItemBoard.Text = "Board";
            // 
            // sizeToolStripMenuItem
            // 
            sizeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { smallToolStripMenuItem, mediumToolStripMenuItem, largeToolStripMenuItem });
            sizeToolStripMenuItem.Name = "sizeToolStripMenuItem";
            sizeToolStripMenuItem.Size = new Size(119, 26);
            sizeToolStripMenuItem.Text = "Size";
            // 
            // smallToolStripMenuItem
            // 
            smallToolStripMenuItem.Checked = true;
            smallToolStripMenuItem.CheckState = CheckState.Checked;
            smallToolStripMenuItem.Enabled = false;
            smallToolStripMenuItem.Name = "smallToolStripMenuItem";
            smallToolStripMenuItem.Size = new Size(147, 26);
            smallToolStripMenuItem.Text = "Small";
            smallToolStripMenuItem.Click += SmallToolStripMenuItem_Click;
            // 
            // mediumToolStripMenuItem
            // 
            mediumToolStripMenuItem.Name = "mediumToolStripMenuItem";
            mediumToolStripMenuItem.Size = new Size(147, 26);
            mediumToolStripMenuItem.Text = "Medium";
            mediumToolStripMenuItem.Click += MediumToolStripMenuItem_Click;
            // 
            // largeToolStripMenuItem
            // 
            largeToolStripMenuItem.Name = "largeToolStripMenuItem";
            largeToolStripMenuItem.Size = new Size(147, 26);
            largeToolStripMenuItem.Text = "Large";
            largeToolStripMenuItem.Click += LargeToolStripMenuItem_Click;
            // 
            // toolStripMenuItemRules
            // 
            toolStripMenuItemRules.Name = "toolStripMenuItemRules";
            toolStripMenuItemRules.Size = new Size(58, 24);
            toolStripMenuItemRules.Text = "Rules";
            toolStripMenuItemRules.Click += ToolStripMenuItemRules_Click;
            // 
            // groupBoxWhite
            // 
            groupBoxWhite.Controls.Add(labelWhiteTime);
            groupBoxWhite.Controls.Add(labelWhiteTimeText);
            groupBoxWhite.Location = new Point(12, 31);
            groupBoxWhite.Name = "groupBoxWhite";
            groupBoxWhite.Size = new Size(285, 85);
            groupBoxWhite.TabIndex = 1;
            groupBoxWhite.TabStop = false;
            groupBoxWhite.Text = "Player White";
            // 
            // labelWhiteTime
            // 
            labelWhiteTime.AutoSize = true;
            labelWhiteTime.Location = new Point(129, 29);
            labelWhiteTime.Name = "labelWhiteTime";
            labelWhiteTime.Size = new Size(17, 20);
            labelWhiteTime.TabIndex = 1;
            labelWhiteTime.Text = "0";
            // 
            // labelWhiteTimeText
            // 
            labelWhiteTimeText.AutoSize = true;
            labelWhiteTimeText.Location = new Point(10, 28);
            labelWhiteTimeText.Name = "labelWhiteTimeText";
            labelWhiteTimeText.Size = new Size(102, 20);
            labelWhiteTimeText.TabIndex = 0;
            labelWhiteTimeText.Text = "Thinking time:";
            // 
            // groupBoxBlack
            // 
            groupBoxBlack.Controls.Add(labelBlackTime);
            groupBoxBlack.Controls.Add(labelBlackTimeText);
            groupBoxBlack.Location = new Point(503, 31);
            groupBoxBlack.Name = "groupBoxBlack";
            groupBoxBlack.Size = new Size(285, 85);
            groupBoxBlack.TabIndex = 2;
            groupBoxBlack.TabStop = false;
            groupBoxBlack.Text = "Player Black";
            // 
            // labelBlackTime
            // 
            labelBlackTime.AutoSize = true;
            labelBlackTime.Location = new Point(135, 29);
            labelBlackTime.Name = "labelBlackTime";
            labelBlackTime.Size = new Size(17, 20);
            labelBlackTime.TabIndex = 1;
            labelBlackTime.Text = "0";
            // 
            // labelBlackTimeText
            // 
            labelBlackTimeText.AutoSize = true;
            labelBlackTimeText.Location = new Point(12, 29);
            labelBlackTimeText.Name = "labelBlackTimeText";
            labelBlackTimeText.Size = new Size(102, 20);
            labelBlackTimeText.TabIndex = 0;
            labelBlackTimeText.Text = "Thinking time:";
            // 
            // buttonPass
            // 
            buttonPass.CausesValidation = false;
            buttonPass.Enabled = false;
            buttonPass.Location = new Point(355, 50);
            buttonPass.Name = "buttonPass";
            buttonPass.Size = new Size(94, 29);
            buttonPass.TabIndex = 3;
            buttonPass.Text = "Pass";
            buttonPass.UseVisualStyleBackColor = true;
            buttonPass.Click += buttonPass_Click;
            // 
            // buttonPause
            // 
            buttonPause.CausesValidation = false;
            buttonPause.Enabled = false;
            buttonPause.Location = new Point(355, 85);
            buttonPause.Name = "buttonPause";
            buttonPause.Size = new Size(94, 29);
            buttonPause.TabIndex = 4;
            buttonPause.Text = "Pause";
            buttonPause.UseVisualStyleBackColor = true;
            buttonPause.Click += buttonPause_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabelWhiteText, toolStripStatusLabelWhitePoint, toolStripStatusLabelBlackText, toolStripStatusLabelBlackPoint });
            statusStrip1.Location = new Point(0, 380);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(802, 26);
            statusStrip1.TabIndex = 5;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelWhiteText
            // 
            toolStripStatusLabelWhiteText.Name = "toolStripStatusLabelWhiteText";
            toolStripStatusLabelWhiteText.Size = new Size(51, 20);
            toolStripStatusLabelWhiteText.Text = "White:";
            // 
            // toolStripStatusLabelWhitePoint
            // 
            toolStripStatusLabelWhitePoint.Name = "toolStripStatusLabelWhitePoint";
            toolStripStatusLabelWhitePoint.Size = new Size(17, 20);
            toolStripStatusLabelWhitePoint.Text = "0";
            // 
            // toolStripStatusLabelBlackText
            // 
            toolStripStatusLabelBlackText.Name = "toolStripStatusLabelBlackText";
            toolStripStatusLabelBlackText.Padding = new Padding(100, 0, 0, 0);
            toolStripStatusLabelBlackText.Size = new Size(147, 20);
            toolStripStatusLabelBlackText.Text = "Black:";
            toolStripStatusLabelBlackText.TextAlign = ContentAlignment.MiddleRight;
            // 
            // toolStripStatusLabelBlackPoint
            // 
            toolStripStatusLabelBlackPoint.Name = "toolStripStatusLabelBlackPoint";
            toolStripStatusLabelBlackPoint.Size = new Size(17, 20);
            toolStripStatusLabelBlackPoint.Text = "0";
            // 
            // openFileDialog
            // 
            openFileDialog.Filter = "Reversi game files (*.reversi)|*.reversi|All files (*.*)|*.*";
            openFileDialog.Title = "Loading reversi game.";
            // 
            // saveFileDialog
            // 
            saveFileDialog.Filter = "Reversi game files (*.reversi)|*.reversi";
            saveFileDialog.Title = "Saving reversi game.";
            // 
            // panel1
            // 
            panel1.AutoSize = true;
            panel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel1.Location = new Point(12, 122);
            panel1.Name = "panel1";
            panel1.Size = new Size(0, 0);
            panel1.TabIndex = 6;
            // 
            // GameForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(802, 406);
            Controls.Add(panel1);
            Controls.Add(statusStrip1);
            Controls.Add(buttonPause);
            Controls.Add(buttonPass);
            Controls.Add(groupBoxBlack);
            Controls.Add(groupBoxWhite);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            MinimumSize = new Size(820, 300);
            Name = "GameForm";
            StartPosition = FormStartPosition.Manual;
            Location = new Point(70, 70);
            Text = "Reversi";
            WindowState = FormWindowState.Minimized;
            FormClosing += GameForm_FormClosing;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            groupBoxWhite.ResumeLayout(false);
            groupBoxWhite.PerformLayout();
            groupBoxBlack.ResumeLayout(false);
            groupBoxBlack.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }



        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem toolStripMenuItemGame;
        private ToolStripMenuItem newToolStripMenuItem1;
        private ToolStripMenuItem saveToolStripMenuItem1;
        private ToolStripMenuItem loadToolStripMenuItem1;
        private ToolStripMenuItem exitToolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItemBoard;
        private ToolStripMenuItem sizeToolStripMenuItem;
        private ToolStripMenuItem smallToolStripMenuItem;
        private ToolStripMenuItem mediumToolStripMenuItem;
        private ToolStripMenuItem largeToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private ToolStripMenuItem toolStripMenuItemRules;
        private GroupBox groupBoxWhite;
        private Label labelWhiteTimeText;
        private GroupBox groupBoxBlack;
        private Label labelBlackTimeText;
        private Label labelWhiteTime;
        private Label labelBlackTime;
        private Button buttonPass;
        private Button buttonPause;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabelWhiteText;
        private ToolStripStatusLabel toolStripStatusLabelWhitePoint;
        private ToolStripStatusLabel toolStripStatusLabelBlackText;
        private ToolStripStatusLabel toolStripStatusLabelBlackPoint;
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;
        private Panel panel1;
    }
}