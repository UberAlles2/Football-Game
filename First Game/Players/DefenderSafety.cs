using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  public class DefenderSafety : Defender
  {
    public Player CoveredPlayer;
    public int CoverAfterMove;
    private bool InCoverage = true;

    public override void Initialize()
    {
      SpeedCap = 134;
      Intelligence = 11;
      InCoverage = true;
      CanStillIntercept = true;

      base.Initialize();

      // Cover one of the wide receivers
      if (Random.Next(0, 100) < 50)
      {
        CoveredPlayer = Game.offenderWideReceiverTop;
        Game.defenderCornerbackTop.DefensiveMode = DefensiveMode.CoverageInfront; // double coverage, one in front, one behind
      }
      else
      {
        CoveredPlayer = Game.offenderWideReceiverBottom;
        Game.defenderCornerbackBottom.DefensiveMode = DefensiveMode.CoverageInfront; // double coverage, one in front, one behind
      }
      CoverAfterMove = Random.Next(6, 24); // Initially don't cover anyone but switch later on.

      TargetPlayer = Game.defenderMiddleLinebacker; // Inital zone coverage, switches later
      if (Random.Next(0, 100) < 15 || Player.ThrowingType ==  ThrowType.Punt || Player.ThrowingType == ThrowType.FieldGoal) // Blitz
      { // Blitz
        DefensiveMode = DefensiveMode.Blitz;
        Intelligence = 9; // Mixed in with blocker less intelligence
        TargetPlayer = ControllablePlayer;
        CoverAfterMove = 999999; // Never switch;
        SpeedCap -= 16; // In blockers, less speed
        if (Random.Next(0, 100) < 50)
          Top = PlayingField.FieldCenterY - 200;
        else
          Top = PlayingField.FieldCenterY + 200;

        Left = PlayingField.LineOfScrimagePixel + 30;
      }
      else if (Random.Next(0, 100) < 40)
      {
        DefensiveMode = DefensiveMode.Normal;
        Left = PlayingField.LineOfScrimagePixel + 300;
        if (CoveredPlayer == Game.offenderWideReceiverTop)
          Top -= (Random.Next(0, 200) - 10);
        else
          Top += (Random.Next(0, 200) - 10);
      }
      else
      {
        DefensiveMode = DefensiveMode.Soft;
        Left = PlayingField.LineOfScrimagePixel + 500;
        Top += Random.Next(0, 200) - 100;
      }
    }

    public override void Move()
    {
      // Either quarterback is running or ball was caught 
      if (ControllablePlayer.Left > PlayingField.LineOfScrimagePixel - 30 && InCoverage == true)
      {
        // One time deal when switching
        SpeedCap += 6;
        Intelligence += 1;
        MovingAroundBlocker = 0;
        TargetPlayer = ControllablePlayer;
        DefensiveMode = DefensiveMode.Blitz; // tight coverage
        ChangeX += 12;
        base.MoveTowardsTarget(TargetPlayer.Left + 200, TargetPlayer.Top);
        InCoverage = false; // No longer in coverage.
      }
      if(InCoverage && TotalMoves > CoverAfterMove) // Switch to covering wide receivers
      {
        Intelligence = 6; // while in coverage, you don't always keep up with player
        TargetPlayer = CoveredPlayer;
        DefensiveMode = DefensiveMode.Normal;
        CoverAfterMove = 99999999; // Never switch again
      }
      if (IsThrowingOrKicking)
      {
        if(TargetPlayer != Game.ballAsPlayer.TargetPlayer)
        {
          TargetPlayer = Game.ballAsPlayer.TargetPlayer; // Go for the ball.
          DefensiveMode = DefensiveMode.Blitz; // Switch to tight coverage.
        }
      }
      if(Intelligence > Random.Next(0,15) || MovingAroundBlocker > 0)
      {
        int calculatedTargetX = AI_BasicMoveTowardsTargetX();
        int calculatedTargeyY = TargetPlayer.Top;
        if(TargetPlayer == Game.defenderMiddleLinebacker)
        {
          calculatedTargetX += 50;
          if (TargetPlayer.Top < PlayingField.FieldCenterY)
            calculatedTargeyY = PlayingField.FieldCenterY + 10;
          else
            calculatedTargeyY = PlayingField.FieldCenterY - 10;
        }

        switch (DefensiveMode)
        {
          case DefensiveMode.Normal:
              calculatedTargetX += 50;
            break;
          case DefensiveMode.Soft:
              calculatedTargetX += 100;
            break;
        }
        base.MoveTowardsTarget(calculatedTargetX, calculatedTargeyY);
      }

      base.Move();
    }

    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      if (collidedWithPlayer is BallAsPlayer)
      {
        if (BallAsPlayer.BallIsCatchable == false)
          return;

        if (IsInterception(this))
        {
          ParentGame.EndPlay(EndPlayType.Intercepted, this, "Intercepted.");
          return;
        }
        
        return; // No collision moves if it's the ball
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
  }
}
