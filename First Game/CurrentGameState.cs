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
    public float DriveYards;
    public int DrivePlays;
    public int DriveFirstDowns;
    public int DriveTime;

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
  }
}
