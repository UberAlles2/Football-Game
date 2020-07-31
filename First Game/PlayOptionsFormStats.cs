using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballGame
{
  public class CurrentGameState
  {
    public int HomeScore = 0;
    public int GuestScore = 0;
    public string ResultsOfLastPlay = "";
    public float YardsGained;
    public Player TackledBy = null;
    public float BallOnYard;
    public int Down;
    public float YardsToGo;

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
