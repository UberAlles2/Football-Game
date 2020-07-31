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

    public override void Initialize()
    {
      SpeedCap = 400;
      IsBall = true;
      TargetPlayer = new Player();
      TargetPlayer.Top = -999;  // end position
      TargetPlayer.Left = -999; // end position
      IsThrowingOrKicking = false;
      BallIsCatchable = false;
      keepGoing = 0;

      base.Initialize();
    }

    public override void Move()
    {
      if (!IsThrowingOrKicking)
        return;

      // Is the ball close to the ending target.  Ball is catchable while this is going on. 
      if (!BallIsCatchable && keepGoing == 0 && Player.DetectCloseCollision(this, TargetPlayer, 90)) 
      {
        keepGoing = 1;
        BallIsCatchable = true;

        if (Math.Abs(ChangeX) > (Math.Abs(ChangeY) + 10) && (Math.Abs(this.Top) - Math.Abs(TargetPlayer.Top)) > 30) // Going horizontal, Y difference has to be tighter.
          return;
        if (Math.Abs(ChangeY) > (Math.Abs(ChangeX) + 10) && (Math.Abs(this.Left) - Math.Abs(TargetPlayer.Left)) > 30) // Going vertical, Y difference has to be tighter.
          return;

        GetChangeYChangeX();
      }

      if (keepGoing > 0)
        keepGoing++;

      // Field Goal 
      if (ThrowingType == ThrowType.FieldGoal && keepGoing == 5)
      {
        EvaluateFieldGoalTry(); // Missed or Good. EndPlay()
        return;
      }

      // Keep the ball going past the target for a bit.
      if (keepGoing > 14) // Go past target for 14 moves
      {
        // stopping movement
        IsThrowingOrKicking = false;
        keepGoing = 0;

        if (BallIsCatchable)
        {
          BallIsCatchable = false;
          ParentGame.EndPlay(EndPlayType.Incomplete, null, "Incomplete.");
        }
        else
          ParentGame.EndPlay(EndPlayType.Incomplete, null, "Pass Broken Up.");

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
      int randomX = 0;
      int randomY = 0;
      if (ThrowingType == ThrowType.Throw)
      {
        randomX = Random.Next(0, 8) - 4;
        randomY = Random.Next(0, 8) - 4;
      }
      else if (ThrowingType == ThrowType.FieldGoal)
      {
        randomX = Random.Next(0, 5) - 2;  // More accurate
        randomY = Random.Next(0, 15) - 10; // Less accurate

        targetX = PlayingField.FieldGoalPostLeft + 20;

        if (Game.CurrentGameState.BallOnYard100 < 65) // 52 yards or greater
        {
          int diff = (((int)Game.CurrentGameState.BallOnYard100 - 65) * 2) + 4;
          targetX += diff; // Could be short, longer it is the more possible it is short
        }
        else // 49-
          targetX += 50; 
      }

      Top = startY;  // start position
      Left = startX; // start position
      TargetPlayer.Left = targetX + 12 + ((int)randomX * (targetX / 100));  // end position with randomness
      TargetPlayer.Top  = targetY + 12 + ((int)randomY * (targetX / 100));  // end position with randomness
      Player.ControllablePlayer.PicBox.Image = ParentForm.Player1.Image;

      GetChangeYChangeX();
      IsThrowingOrKicking = true;
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

    private void EvaluateFieldGoalTry()
    {
      if (this.Left < PlayingField.FieldGoalPostLeft)
      {
        ParentGame.EndPlay(EndPlayType.FieldGoalMiss, null, "Missed Field Goal Short.");
      }

      this.Left = PlayingField.FieldGoalPostLeft + 4;

      if (DetectCloseCollision(this, PlayingField.FieldGoalPostLeft, PlayingField.FieldGoalPostTop - 12, 12) // Hit goal Post?
       || DetectCloseCollision(this, PlayingField.FieldGoalPostLeft, PlayingField.FieldGoalPostBottom - 8, 12))
      {
        SpinDefectedBall();
        if (Random.Next(0, 20) > 9)
          ParentGame.EndPlay(EndPlayType.FieldGoal, null, "Field Goal! Went through.");
        else
          ParentGame.EndPlay(EndPlayType.FieldGoalMiss, null, "Missed Field Goal. Hit Post.");
      }
      else if (this.Top > PlayingField.FieldGoalPostTop && this.Top < PlayingField.FieldGoalPostBottom) // Most common result
      {
        ParentGame.EndPlay(EndPlayType.FieldGoal, null, "Field Goal!");
      }
      else
      {
        ParentGame.EndPlay(EndPlayType.FieldGoalMiss, null, "Missed Field Goal Wide.");
      }
    }

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
  }
}
