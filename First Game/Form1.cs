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
  }
}
