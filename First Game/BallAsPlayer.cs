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
    public bool BallIsCatchable;

    public override void Initialize()
    {
      SpeedCap = 400;
      IsBall = true;
      TargetPlayer = new Player();
      TargetPlayer.Top = -999;  // end position
      TargetPlayer.Left = -999; // end position
      IsThrowing = false;
      BallIsCatchable = false;

      base.Initialize();
    }

    public override void Move()
    {
      if (!IsThrowing)
        return;

      // Is the ball close to the ending target.  Ball is catchable while this is going on. 
      if (!BallIsCatchable && Game.DetectCloseCollision(this, TargetPlayer, 30))
      {
        BallIsCatchable = true;
        GetChangeYChangeX();
      }

      // Keep the ball going past the target for a bit.
      if (BallIsCatchable && !Game.DetectCloseCollision(this, TargetPlayer, 30))
      {
        BallIsCatchable = false;
        IsThrowing = false;
        ParentGame.EndPlay("Incomplete");
        return;
      }

      // Keep the ball on target by adjusting its path every dozen moves.
      if (!BallIsCatchable && TotalMoves % 12 == 0 && Top > -999)
      {
        GetChangeYChangeX();
      }

      base.Move();
    }

    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      // The ball doesn't move when colliding.
    }

    public void ThrowBall(int startY, int startX, int targetY, int targetX)
    {
      TotalMoves = 0;
      Top = startY;  // start position
      Left = startX; // start position
      TargetPlayer.Top  = targetY - 8 + (Random.Next(-4, 4) * (targetX / 100));  // end position with randomness
      TargetPlayer.Left = targetX + 8 + (Random.Next(-4, 4) * (targetX / 100));  // end position with randomness
      Game.ControllablePlayer.PicBox.Image = ParentForm.Player1.Image;

      GetChangeYChangeX();
      IsThrowing = true;
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
