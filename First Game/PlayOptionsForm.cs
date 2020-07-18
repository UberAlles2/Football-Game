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
    private Game ParentGame;
    public OffenderWideReceiver.PatternEnum selectedPatternTop;
    public OffenderWideReceiver.PatternEnum selectedPatternBottom;

    public PlayOptionsForm(Game parentGame)
    {
      InitializeComponent();
      ParentGame = parentGame;
      // We only have 3 pictures, the top ones are the bottom ones flipped.
      picButtonHookPatternTop.Image.RotateFlip(RotateFlipType.RotateNoneFlipY); 
      picFlyPatternTop.Image.RotateFlip(RotateFlipType.RotateNoneFlipY);
      picPostPatternTop.Image.RotateFlip(RotateFlipType.RotateNoneFlipY);
      picSlantPatternTop.Image.RotateFlip(RotateFlipType.RotateNoneFlipY);
      picQuickOutPatternTop.Image.RotateFlip(RotateFlipType.RotateNoneFlipY);
      this.Icon = Game.ParentForm.Icon;
    }

    private void PlayOptionsForm_Load(object sender, EventArgs e)
    {
      DisplayStats();
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
    private void DisplayStats()
    {
      if (Game.PlayOptionsFormStats.YardsGained > 0)
      {
        lblYardGainedValue.ForeColor = Color.DarkGreen;
        lblYardGainedValue.Text = Game.PlayOptionsFormStats.YardsGained.ToString("0.0") + " yards gained.";
      }
      else if (Game.PlayOptionsFormStats.YardsGained < 0)
      {
        lblYardGainedValue.ForeColor = Color.DarkRed;
        lblYardGainedValue.Text = Math.Abs(Game.PlayOptionsFormStats.YardsGained).ToString("0.0") + " yards lost.";
      }
      else
      {
        lblYardGainedValue.ForeColor = Color.DarkBlue;
        lblYardGainedValue.Text = "No gain.";
      }

      if (Game.PlayOptionsFormStats.TackledBy == null)
        lblTackledByValue.Text = "";
      else
        lblTackledByValue.Text = Game.PlayOptionsFormStats.TackledBy.GetType().Name.Replace("Defender", "").Replace("Offender", "");

      lblResultsOfLastPlay.Text = Game.PlayOptionsFormStats.ResultsOfLastPlay;
      if(Game.PlayOptionsFormStats.BallOnYard100 > 50)
        lblBallOnValue.Text = Game.PlayOptionsFormStats.BallOnYard.ToString("0.0") + "  " + '►';
      else
        lblBallOnValue.Text = '◀' + "  " + Game.PlayOptionsFormStats.BallOnYard.ToString("0.0");

      lblDownValue.Text = Game.PlayOptionsFormStats.Down.ToString();
      lblYardsToGoValue.Text = Game.PlayOptionsFormStats.YardsToGo.ToString("0.0");
    }

    private void SetRandomWRPatterns()
    {
      int randomPattern = Game.Random.Next(0, 5);
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
          picReceiverTop.Image = picSlantPatternTop.Image;
          selectedPatternTop = OffenderWideReceiver.PatternEnum.SlantPattern;
          break;
        case 4:
          picReceiverTop.Image = picQuickOutPatternTop.Image;
          selectedPatternTop = OffenderWideReceiver.PatternEnum.QuickOutPattern;
          break;
      }

      randomPattern = Game.Random.Next(0, 5);
      switch (randomPattern)
      {
        case 0:
          picReceiverBottom.Image = picButtonHookPattern.Image;
          selectedPatternBottom = OffenderWideReceiver.PatternEnum.ButtonHookPattern;
          break;
        case 1:
          picReceiverBottom.Image = picFlyPattern.Image;
          selectedPatternBottom = OffenderWideReceiver.PatternEnum.FlyPattern;
          break;
        case 2:
          picReceiverBottom.Image = picPostPattern.Image;
          selectedPatternBottom = OffenderWideReceiver.PatternEnum.PostPattern;
          break;
        case 3:
          picReceiverBottom.Image = picSlantPattern.Image;
          selectedPatternBottom = OffenderWideReceiver.PatternEnum.SlantPattern;
          break;
        case 4:
          picReceiverBottom.Image = picQuickOutPattern.Image;
          selectedPatternBottom = OffenderWideReceiver.PatternEnum.QuickOutPattern;
          break;
      }
    }
  }
}
