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
  public class DefenderCornerbackTop : DefenderCornerback 
  {
    public override void Initialize()
    {
      base.SetRandomcoverage(); // Infront, Normal, Soft
      base.Initialize();
      if (Game.defenderSafety.CoveredPlayer == Game.offenderWideReceiverTop || Game.defenderMiddleLinebacker.TargetPlayer == Game.offenderWideReceiverTop)
        DefensiveMode = DefensiveMode.CoverageInfront;
    }
    public override void Move()
    {
      base.Move();
    }
  }
  public class DefenderCornerbackBottom : DefenderCornerback 
  {
    public override void Initialize()
    {
      base.SetRandomcoverage(); // Infront, Normal, Soft
      base.Initialize();
      if (Game.defenderSafety.CoveredPlayer == Game.offenderWideReceiverBottom || Game.defenderMiddleLinebacker.TargetPlayer == Game.offenderWideReceiverBottom)
        DefensiveMode = DefensiveMode.CoverageInfront;
    }

    public override void Move()
    {
      base.Move();
    }
  }

  public class DefenderCornerback : Defender
  {
    private bool InCoverage = true;

    public override void Initialize()
    {
      SpeedCap = 132;
      Intelligence = 7;
      InCoverage = true;
      CanStillIntercept = true;
      base.Initialize();
    }

    public override void Move()
    {
      // Running back / quarterback has cross the scrimmage line. Change the target to the running back
      if (ControllablePlayer.Left > PlayingField.LineOfScrimagePixel && InCoverage == true)
      {
        TargetPlayer = ControllablePlayer;
        ChangeX += 20;
        base.MoveTowardsTarget(TargetPlayer.Left + 160, TargetPlayer.Top);
        InCoverage = false;
      }

      if (this.Intelligence > Random.Next(0, 15) || MovingAroundBlocker > 0)
      {
        int calcTargetX;
        int calcTargetY;

        //---- Calculate horizontalcoverage
        if (DefensiveMode == DefensiveMode.CoverageInfront)
        {
          calcTargetX = TargetPlayer.Left;
        }
        else
          calcTargetX = AI_BasicMoveTowardsTargetX();

        //---- Calculate vertical coverage
        if (InCoverage)
        {
          if (VerticalPosition == VerticalPosition.PositionTop)
          {
            if (TargetPlayer.Left < Left && Math.Abs(TargetPlayer.Top - Top) < 32 && TargetPlayer.Top < PlayingField.FieldCenterY) // We are trying to move in front, go around if at the same Y.
              calcTargetY = TargetPlayer.Top + 50 - TargetPlayer.ChangeY;
            else if (TargetPlayer.Top < PlayingField.FieldCenterY)
              calcTargetY = TargetPlayer.Top + 30;
            else
              calcTargetY = TargetPlayer.Top - 30;
          }
          else // Bottom CB
          {
            if (TargetPlayer.Left < Left && Math.Abs(TargetPlayer.Top - Top) < 32 && TargetPlayer.Top > PlayingField.FieldCenterY) // We are trying to move in front, go around if at the same Y.
              calcTargetY = TargetPlayer.Top - 50 - TargetPlayer.ChangeY;
            else if (TargetPlayer.Top > PlayingField.FieldCenterY)
              calcTargetY = TargetPlayer.Top - 30;
            else
              calcTargetY = TargetPlayer.Top + 30;
          }

          base.MoveTowardsTarget(calcTargetX + (Random.Next(0, 120) - 50), calcTargetY + (Random.Next(0, 100) - 50));

          if (IsThrowingOrKicking && Random.Next(0, 10) > 9) // Player will move towards thrown ball
          {
            TargetPlayer = Game.ballAsPlayer.TargetPlayer; // Where the ball is being thrown.
          }
        }
        else
        {
          Intelligence = 11;
          base.MoveTowardsTarget(calcTargetX, TargetPlayer.Top);
        }
      }

      base.Move();
    }

    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      if (collidedWithPlayer is BallAsPlayer)
      {
        if (BallAsPlayer.BallIsCatchable == false)
          return;

        if(IsInterception(this))
        {
          ParentGame.EndPlay(EndPlayType.Intercepted, this, "Intercepted.");
          return;
        }
      }

      if (collidedWithPlayer.HasBall && !IsThrowingOrKicking)
      {
        ParentGame.EndPlay(EndPlayType.Tackled, this, "Tackled.");
        return;
      }

      if (collidedWithPlayer is Offender)
      {
        base.MoveAroundPlayer(collisionOrientation);
        return;
      }

      base.CollisionMove(collidedWithPlayer, collisionOrientation);
    }
    protected void SetRandomcoverage()
    {
      Top += Random.Next(0, 80) - 40;

      if (Random.Next(0, 100) < 20 || Player.ThrowingType == ThrowType.Punt || Player.ThrowingType == ThrowType.FieldGoal) // Blitz
      {
        DefensiveMode = DefensiveMode.CoverageInfront;
        Left -= 20;
      }
      else if (Random.Next(0, 100) < 85)
      {
        DefensiveMode = DefensiveMode.Normal;
      }
      else
      {
        DefensiveMode = DefensiveMode.Soft;
        Left += 20;
      }
    }
  }
}
