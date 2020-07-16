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
    public OffenderWideReceiver.PatternEnum selectedPatternTop;
    public OffenderWideReceiver.PatternEnum selectedPatternBottom;

    public PlayOptionsForm()
    {
      InitializeComponent();
      // We only have 3 pictures, the top ones are the bottom ones flipped.
      picButtonHookPatternTop.Image.RotateFlip(RotateFlipType.RotateNoneFlipY); 
      picFlyPatternTop.Image.RotateFlip(RotateFlipType.RotateNoneFlipY);
      picPostPatternTop.Image.RotateFlip(RotateFlipType.RotateNoneFlipY);
    }

    private void PlayOptionsForm_Load(object sender, EventArgs e)
    {
      SetRandomWRPatterns();
    }

    private void btnStartPlay_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void btnChangePatterns_Click(object sender, EventArgs e)
    {
      SetRandomWRPatterns();
    }

    private void SetRandomWRPatterns()
    {
      int randomPattern = Game.Random.Next(0, 3);
      switch (randomPattern)
      {
        case 0:
          picReceiverTop.Image = picButtonHookPatternTop.Image;
          selectedPatternTop = OffenderWideReceiver.PatternEnum.ButtonHookPattern;
          break;
        case 1:
          picReceiverTop.Image = picFlyPatternTop.Image;
          selectedPatternTop = OffenderWideReceiver.PatternEnum.FlyPattern;
          break;
        case 2:
          picReceiverTop.Image = picPostPatternTop.Image;
          selectedPatternTop = OffenderWideReceiver.PatternEnum.PostPattern;
          break;
        case 3:
          picReceiverTop.Image = picPostPatternTop.Image;
          break;
      }

      randomPattern = Game.Random.Next(1, 4);
      switch (randomPattern)
      {
        case 1:
          picReceiverBottom.Image = picButtonHookPattern.Image;
          selectedPatternBottom = OffenderWideReceiver.PatternEnum.ButtonHookPattern;
          break;
        case 2:
          picReceiverBottom.Image = picFlyPattern.Image;
          selectedPatternBottom = OffenderWideReceiver.PatternEnum.FlyPattern;
          break;
        case 3:
          picReceiverBottom.Image = picPostPattern.Image;
          selectedPatternBottom = OffenderWideReceiver.PatternEnum.PostPattern;
          break;
        case 4:
          picReceiverBottom.Image = picPostPattern.Image;
          selectedPatternBottom = OffenderWideReceiver.PatternEnum.PostPattern;
          break;
      }
    }
  }
}
