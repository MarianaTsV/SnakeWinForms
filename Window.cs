using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Window : Form
    {

        private const int UpperMargin = 30;
        private Bitmap gameArea;
        private Graphics gameGraphics;
        private Game game;

        public Window()
        {
            InitializeComponent();
            gameArea = new Bitmap(ClientSize.Width, ClientSize.Height - UpperMargin);
            gameGraphics = Graphics.FromImage(gameArea);
            game = new Game(50, 50, 10, 30);
            RestartButton.Enabled = false;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            game.Draw(gameGraphics);
            e.Graphics.DrawImage(gameArea, 0, UpperMargin);
        }

        private void OnTick(object sender, EventArgs e)
        {
            game.Update();
            Thread.Sleep(game.SetSleepTime());
            Invalidate();
            ScoreLabel.Text = "Score: " + game.GetScore();
            if(game.GameOver)
            {
                RestartButton.Enabled = true;
            }
        }

        private void OnRestart(object sender, EventArgs e)
        {
            game.Initialize();
            RestartButton.Enabled = false;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            game.OnKeyPress(e.KeyCode);
        }
    }
}
