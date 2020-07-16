using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  public partial class PlayOptionsForm : Form
  {
    public PlayOptionsForm()
    {
      InitializeComponent();
      picButtonHookPatternTop.Image.RotateFlip(RotateFlipType.RotateNoneFlipY);
      picFlyPatternTop.Image.RotateFlip(RotateFlipType.RotateNoneFlipY);
      picPostPatternTop.Image.RotateFlip(RotateFlipType.RotateNoneFlipY);
    }

    private void PlayOptionsForm_Load(object sender, EventArgs e)
    {
    }

    private void btnStartPlay_Click(object sender, EventArgs e)
    {
      Close();
    }
  }
}
