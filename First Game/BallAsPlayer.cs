using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  public class BallAsPlayer : Player
  {
    private bool endingMove;

    public override void Initialize()
    {
      SpeedCap = 260;
      IsBall = true;
      TargetPlayer = new Player();
      TargetPlayer.Top = -999;  // end position
      TargetPlayer.Left = -999; // end position
      Game.IsThrowing = false;

      base.Initialize();
    }

    public override void Move()
    {
      if (!Game.IsThrowing)
        return;

      // Is the ball close to the ending target
      if (!endingMove && Game.DetectCloseCollision(this, TargetPlayer, 60))
      {
        endingMove = true;
        GetChangeYChangeX();
      }

      // Keep the ball going past the target for a bit. 
      if (endingMove && !Game.DetectCloseCollision(this, TargetPlayer, 60))
      {
        Game.IsThrowing = false;
        ParentGame.EndPlay("Incomplete");
        return;
      }

      // Keep the ball on target by adjusting its path.
      if (!endingMove && TotalMoves % 12 == 0 && Top > -999)
      {
        GetChangeYChangeX();
      }

      base.Move();
    }

    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      base.CollisionMove(collidedWithPlayer, collisionOrientation);
    }

    public void ThrowBall(int Y, int X, int targetY, int targetX)
    {
      TotalMoves = 0;
      Top = Y;  // start position
      Left = X; // start position
      TargetPlayer.Top = targetY - 8;  // end position
      TargetPlayer.Left = targetX; // end position
      GetChangeYChangeX();
      Game.IsThrowing = true;
      endingMove = false;
    }

    private void GetChangeYChangeX()
    {
      float ChgY = TargetPlayer.Top - Top;
      float ChgX = TargetPlayer.Left - Left;
      float totalChange = Math.Abs(ChgY) + Math.Abs(ChgX);
      float slopeY = ChgY / totalChange;
      float slopeX = ChgX / totalChange;
      ChangeY = (int)(slopeY * (float)SpeedCap);
      ChangeX = (int)(slopeX * (float)SpeedCap);
      if (Math.Abs(ChangeY) > 20 && Math.Abs(ChangeY) < 34)
        ChangeY *= 2;
      if (Math.Abs(ChangeX) > 20 && Math.Abs(ChangeX) < 34)
        ChangeX *= 2;
    }
  }
}
