using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  public class BallAsPlayer : Player
  {
    public static bool BallIsCatchable;

    private int keepGoing;

    public static void SpinDefectedBall()
    {
      Game.ballAsPlayer.PicBox.Image.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
      Game.ballAsPlayer.PicBox.Invalidate();
      Thread.Sleep(100);
      Game.ballAsPlayer.PicBox.Image.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
      Game.ballAsPlayer.PicBox.Invalidate();
      Application.DoEvents();
      Thread.Sleep(100);
      Game.ballAsPlayer.PicBox.Image.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
      Game.ballAsPlayer.PicBox.Invalidate();
      Application.DoEvents();
      Thread.Sleep(100);
      Game.ballAsPlayer.PicBox.Image.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
      Game.ballAsPlayer.PicBox.Invalidate();
      Application.DoEvents();
      Thread.Sleep(100);
    }

    public override void Initialize()
    {
      SpeedCap = 400;
      IsBall = true;
      TargetPlayer = new Player();
      TargetPlayer.Top = -999;  // end position
      TargetPlayer.Left = -999; // end position
      IsThrowing = false;
      BallIsCatchable = false;
      keepGoing = 0;

      base.Initialize();
    }

    public override void Move()
    {
      if (!IsThrowing)
        return;

      // Is the ball close to the ending target.  Ball is catchable while this is going on. 
      if (!BallIsCatchable && keepGoing == 0 && Game.DetectCloseCollision(this, TargetPlayer, 90)) 
      {
        keepGoing = 1;
        if (Math.Abs(ChangeX) > (Math.Abs(ChangeY) + 10) && (Math.Abs(this.Top) - Math.Abs(TargetPlayer.Top)) > 30) // Going horizontal, Y difference has to be tighter.
          return;
        if (Math.Abs(ChangeY) > (Math.Abs(ChangeX) + 10) && (Math.Abs(this.Left) - Math.Abs(TargetPlayer.Left)) > 30) // Going vertical, Y difference has to be tighter.
          return;

        BallIsCatchable = true;
        GetChangeYChangeX();
      }

      if (keepGoing > 0)
        keepGoing++;

      // Keep the ball going past the target for a bit.
      if (keepGoing > 12)
      {
        BallIsCatchable = false;
        IsThrowing = false;
        ParentGame.EndPlay(EndPlayType.Incomplete, "Incomplete");
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

    public void ThrowBall(int startX, int startY, int targetY, int targetX)
    {
      TotalMoves = 0;
      Top = startY;  // start position
      Left = startX; // start position
      TargetPlayer.Top  = targetY - 8 + (Random.Next(-5, 5) * (targetX / 100));  // end position with randomness
      TargetPlayer.Left = targetX + 8 + (Random.Next(-5, 5) * (targetX / 100));  // end position with randomness
      Player.ControllablePlayer.PicBox.Image = ParentForm.Player1.Image;

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
