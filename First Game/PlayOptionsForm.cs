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
    public enum PlayOptionType
    {
      NormalPlay,
      Punt,
      FieldGoal
    }
    
    public static CountDownTimer CountDownTimer;
    public static PlayOptionType PlayOption = PlayOptionType.NormalPlay;

    public PlayOptionsForm(Game parentGame)
    {
      InitializeComponent();
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

      btnPunt.Enabled = btnFieldGoal.Enabled = false;

      if (Game.CurrentGameState.Down == 4 || Scoreboard.CountDownTimer.TimeLeft.TotalSeconds < 60)
      {
        btnPunt.Enabled = true;
        if(Game.CurrentGameState.BallOnYard100 > 55)
        {
          btnFieldGoal.Enabled = true;
        }
      }

      CountDownTimer = new CountDownTimer(0, 20); // 20 seconds to choose a play.
      CountDownTimer.TimeChanged = TimeChanged;
      CountDownTimer.TimeExpired = TimeExpired;
      CountDownTimer.Start();
    }

    public void TimeChanged()
    {
      lblTimeLeft.Text = CountDownTimer.TimeLeftSecondsString;
    }
    public void TimeExpired()
    {
      btnStartPlay_Click(this, null);
    }
    private void btnStartPlay_Click(object sender, EventArgs e)
    {
      PlayOption = PlayOptionType.NormalPlay;
      this.Close();
    }
    private void btnPunt_Click(object sender, EventArgs e)
    {
      PlayOption = PlayOptionType.Punt;
      this.Close();
    }
    private void btnFieldGoal_Click(object sender, EventArgs e)
    {
      PlayOption = PlayOptionType.FieldGoal;
      Game.offenderWideReceiverTop.FieldGoalTop();
      Game.offenderWideReceiverBottom.FieldGoalBottom();
      this.Close();
    }

    private void btnChangePatterns_Click(object sender, EventArgs e)
    {
      SetRandomWRPatterns();
    }
    private void DisplayStats()
    {
      if (Game.CurrentGameState.YardsGained > 0)
      {
        lblYardGainedValue.ForeColor = Color.DarkGreen;
        lblYardGainedValue.Text = Game.CurrentGameState.YardsGained.ToString("0.0") + " yards gained.";
      }
      else if (Game.CurrentGameState.YardsGained < 0)
      {
        lblYardGainedValue.ForeColor = Color.DarkRed;
        lblYardGainedValue.Text = Math.Abs(Game.CurrentGameState.YardsGained).ToString("0.0") + " yards lost.";
      }
      else
      {
        lblYardGainedValue.ForeColor = Color.DarkBlue;
        lblYardGainedValue.Text = "No gain.";
      }

      if (Game.CurrentGameState.TackledBy == null)
        lblTackledByValue.Text = "";
      else
        lblTackledByValue.Text = Game.CurrentGameState.TackledBy.GetType().Name.Replace("Defender", "").Replace("Offender", "");

      lblResultsOfLastPlay.Text = Game.CurrentGameState.ResultsOfLastPlay;
      if(Game.CurrentGameState.BallOnYard100 > 50)
        lblBallOnValue.Text = Game.CurrentGameState.BallOnYard.ToString("0.0") + "  " + '►';
      else
        lblBallOnValue.Text = '◀' + "  " + Game.CurrentGameState.BallOnYard.ToString("0.0");

      lblDownValue.Text = Game.CurrentGameState.Down.ToString();
      lblYardsToGoValue.Text = Game.CurrentGameState.YardsToGo.ToString("0.0");
    }

    private void SetRandomWRPatterns()
    {
      int randomPattern = Game.Random.Next(0, 5);
      switch (randomPattern)
      {
        case 0:
          picReceiverTop.Image = picButtonHookPatternTop.Image;
          Game.offenderWideReceiverTop.ButtonHookPattern();
          break;
        case 1:
          picReceiverTop.Image = picFlyPatternTop.Image;
          Game.offenderWideReceiverTop.FlyPattern();
          break;
        case 2:
          picReceiverTop.Image = picPostPatternTop.Image;
          Game.offenderWideReceiverTop.PostPatternTop();
          break;
        case 3:
          picReceiverTop.Image = picSlantPatternTop.Image;
          Game.offenderWideReceiverTop.SlantPatternTop();
          break;
        case 4:
          picReceiverTop.Image = picQuickOutPatternTop.Image;
          Game.offenderWideReceiverTop.QuickOutPatternTop();
          break;
      }

      randomPattern = Game.Random.Next(0, 5);
      switch (randomPattern)
      {
        case 0:
          picReceiverBottom.Image = picButtonHookPattern.Image;
          Game.offenderWideReceiverBottom.ButtonHookPattern();
          break;
        case 1:
          picReceiverBottom.Image = picFlyPattern.Image;
          Game.offenderWideReceiverBottom.FlyPattern();
          break;
        case 2:
          picReceiverBottom.Image = picPostPattern.Image;
          Game.offenderWideReceiverBottom.PostPatternBottom();
          break;
        case 3:
          picReceiverBottom.Image = picSlantPattern.Image;
          Game.offenderWideReceiverBottom.SlantPatternBottom();
          break;
        case 4:
          picReceiverBottom.Image = picQuickOutPattern.Image;
          Game.offenderWideReceiverBottom.QuickOutPatternBottom();
          break;
      }
    }
  }
}
