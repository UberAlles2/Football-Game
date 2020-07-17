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
        lblYardGainedValue.ForeColor = Color.Green;
        lblYardGainedValue.Text = $"{Game.PlayOptionsFormStats.YardsGained,0:#.#} yards gained.";
      }
      else if (Game.PlayOptionsFormStats.YardsGained < 0)
      {
        lblYardGainedValue.ForeColor = Color.Red;
        lblYardGainedValue.Text = $"{Math.Abs(Game.PlayOptionsFormStats.YardsGained),0:#.#} yards lost.";
      }
      else
      {
        lblYardGainedValue.ForeColor = Color.DarkBlue;
        lblYardGainedValue.Text = "No gain.";
      }

      if (Game.PlayOptionsFormStats.TackledBy == null)
        lblTackledByValue.Text = "";
      else
        lblTackledByValue.Text = Game.PlayOptionsFormStats.TackledBy.GetType().Name;

      lblResultsOfLastPlay.Text = Game.PlayOptionsFormStats.ResultsOfLastPlay;
      lblBallOnValue.Text = Game.PlayOptionsFormStats.BallOnYard.ToString();
      lblDownValue.Text = Game.PlayOptionsFormStats.Down.ToString();
      lblYardsToGoValue.Text = Game.PlayOptionsFormStats.YardsToGo.ToString();
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
