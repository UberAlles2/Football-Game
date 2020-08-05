using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  public class DefenderMiddleLinebacker : Defender
  {
    private bool InCoverage = true;

    public override void Initialize()
    {
      int shortYardage = 0;
      
      SpeedCap = 114;
      Intelligence = 10;
      TargetPlayer = ControllablePlayer;
      CanStillIntercept = true;

      base.Initialize();

      if(Game.CurrentGameState.YardsToGo < 2) // Short yardage play
      {
        Left = InitialLeft - 80;
        ChangeX = -12;
        shortYardage = 4; // More likely to blitz
      }
      if (Game.CurrentGameState.YardsToGo > 12) // Long yardage play
      {
        Left = InitialLeft + 20;
        ChangeX = +6;
        shortYardage = -4; // More likely to play soft

        // Cover one of the wide receivers
        if (Random.Next(0, 100) < 20)
        {
          DefensiveMode = DefensiveMode.Blitz;
          if (Random.Next(0, 100) < 50)
          {
            TargetPlayer = Game.offenderWideReceiverTop;
            Game.defenderCornerbackTop.DefensiveMode = DefensiveMode.CoverageInfront; // double coverage, one in front, one behind
          }
          else
          {
            TargetPlayer = Game.offenderWideReceiverBottom;
            Game.defenderCornerbackBottom.DefensiveMode = DefensiveMode.CoverageInfront; // double coverage, one in front, one behind
          }
        }
      }

      int r = Random.Next(0, 12);
      if (r < (3 + shortYardage) || Player.ThrowingType == ThrowType.Punt || Player.ThrowingType == ThrowType.FieldGoal)
      {
        Intelligence = 9; // Mixed in with blocker less intelligence
        DefensiveMode = DefensiveMode.Blitz;
        Top = PlayingField.FieldCenterY - 40;
      }
      else if (r < (9 + shortYardage))
        DefensiveMode = DefensiveMode.Normal;
      //else 
      //  DefensiveMode = DefensiveMode.Soft; // never play soft
    }

    public override void Move()
    {
      if (ControllablePlayer.Left > PlayingField.LineOfScrimagePixel - 30 && InCoverage == true)
      {
        // One time deal when switching
        SpeedCap += 4;
        Intelligence += 1;
        MovingAroundBlocker = 0;
        TargetPlayer = ControllablePlayer;
        DefensiveMode = DefensiveMode.Blitz; // tight coverage
        ChangeX += 4;
        base.MoveTowardsTarget(TargetPlayer.Left + 200, TargetPlayer.Top);
        InCoverage = false; // No longer in coverage.
      }

      if (IsThrowingOrKicking)
      {
        DefensiveMode = DefensiveMode.Blitz; // Switch to tight coverage.
        ChangeX += 12; // If the ball is thrown go out where the receivers are.

        if (ControllablePlayer.Top < Top)
          ChangeY += 8;
        else
          ChangeY -= 8;
      }
      else if(Intelligence > Random.Next(0,15) || MovingAroundBlocker > 0)
      {
        int calculatedTargetX = AI_BasicMoveTowardsTargetX();
        switch(DefensiveMode)
        {
          case DefensiveMode.Normal:
            if (TargetPlayer.Left < PlayingField.LineOfScrimagePixel - 30)
              calculatedTargetX += (PlayingField.LineOfScrimagePixel - 90);
            break;
          case DefensiveMode.Soft:
            if (TargetPlayer.Left < PlayingField.LineOfScrimagePixel - 30)
              calculatedTargetX += (PlayingField.LineOfScrimagePixel - 10);
            break;
        }
        base.MoveTowardsTarget(calculatedTargetX, TargetPlayer.Top);
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
          FlashPlayer();
          Game.EndPlay(EndPlayType.Intercepted, this, "Intercepted.");
          return;
        }
      }

      if (collidedWithPlayer.HasBall && !IsThrowingOrKicking)
      {
        Game.EndPlay(EndPlayType.Tackled, this, "Tackled.");
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
