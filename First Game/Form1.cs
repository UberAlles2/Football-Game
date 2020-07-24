using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
    private void picSidelineYardage_MouseClick(object sender, MouseEventArgs e)
    {
      MouseEventArgs mouseEventArgs = new MouseEventArgs(MouseButtons.Left, 1, e.Location.X, e.Location.Y + 64, 0);
      Form1_MouseClick(sender, mouseEventArgs);
    }
    private void lblBottomSideline_MouseClick(object sender, MouseEventArgs e)
    {
      MouseEventArgs mouseEventArgs = new MouseEventArgs(MouseButtons.Left, 1, e.Location.X, e.Location.Y + Height - 56, 0);
      Form1_MouseClick(sender, mouseEventArgs);
    }

    private void Form1_Paint(object sender, PaintEventArgs e)
    {
      DrawPlayingField.PaintScrimmageAndFirstDownLines(sender, e);       
    }

    private void picEndZoneLeft_Paint(object sender, PaintEventArgs e)
    {
      Graphics g = e.Graphics;

      Player.ControllablePlayer.PicBox.SendToBack();
      if(picEndZoneLeft.Bounds.IntersectsWith(Player.ControllablePlayer.PicBox.Bounds))
        g.DrawImage(Player.ControllablePlayer.PicBox.Image, Player.ControllablePlayer.PicBox.Location.X - picEndZoneLeft.Left, Player.ControllablePlayer.PicBox.Location.Y - picEndZoneLeft.Top, Player.ControllablePlayer.PicBox.Width, Player.ControllablePlayer.PicBox.Height);
    }
  }
}
