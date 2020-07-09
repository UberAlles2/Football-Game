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

namespace FootballGame
{
  public partial class Form1 : Form
  {
    Game game;

    public Form1()
    {
      InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      game = new Game(this);
    }

    private void Form1_Shown(object sender, EventArgs e)
    {
      game.Run();
    }

    private void Form1_FormClosed(object sender, FormClosedEventArgs e)
    {
      game.Stop();
    }

    private void Form1_MouseClick(object sender, MouseEventArgs e)
    {
      game.MouseClick(sender, e);
    }

    private void Form1_Paint(object sender, PaintEventArgs e)
    {
      Pen pen = new Pen(Color.FromArgb(255, 128, 128, 255));
      e.Graphics.DrawLine(pen, Game.LineOfScrimage, 0, Game.LineOfScrimage, 800);
    }
  }
}
