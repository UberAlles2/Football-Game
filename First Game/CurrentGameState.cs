using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballGame
{
  public class CurrentGameState
  {
    public int Quarter = 0;
    public int HomeScore = 0;
    public int GuestScore = 0;
    public string ResultsOfLastPlay = "";
    public float YardsGained;
    public Player TackledBy = null;
    public float BallOnYard;
    public int Down;
    public float YardsToGo;
    public bool FirstDown;
    public float DriveStartYard;
    private float DriveYards;
    public int DrivePlays;
    public int DriveFirstDowns;
    public TimeSpan DriveStartTimeSpan = new TimeSpan();
    public TimeSpan DriveElapsedTimeSpan = new TimeSpan();
    public int TimeOutsLeft;

    public float TackledAt100;
    private float ballOnYard100;
    public float BallOnYard100 
    { 
      get => ballOnYard100;
      set
      {
        ballOnYard100 = value;
        BallOnYard = ballOnYard100 < 50 ? ballOnYard100 : 100 - ballOnYard100;
      }
    }

    public void AddDriveYards(float yardsGained)
    {
      DriveYards += yardsGained;
    }
    public void IncrementDrivePlays()
    {
      DrivePlays++;
    }
    public void IncrementFirstDowns()
    {
      DriveFirstDowns++;
    }

    public string GetElapsedString()
    {
      DriveElapsedTimeSpan = Scoreboard.CountDownTimer.TimeLeft.Subtract(Game.CurrentGameState.DriveStartTimeSpan);
      return DriveElapsedTimeSpan.ToString(@"mm\:ss");
    }
    public float GetDriveYards()
    {
      DriveYards = BallOnYard100 - DriveStartYard;
      return DriveYards;
    }

    public void ResetDriveState()
    {
      DriveYards = 0;
      DrivePlays = 0;
      DriveFirstDowns = 0;
      DriveStartTimeSpan = new TimeSpan(Scoreboard.CountDownTimer.TimeLeft.Ticks);
      DriveStartYard = BallOnYard100;
    }
  }
}
