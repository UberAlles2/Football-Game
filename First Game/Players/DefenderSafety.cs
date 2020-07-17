using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  class DefenderSafety : Defender
  {
    public Player OffenseWideReceiverTop;
    public Player OffenseWideReceiverBottom;
    public Player DefenderMiddleLinebacker;
    public Player CoveredPlayer;
    public int CoverAfterMove;
    private bool InCoverage = true;

    public override void Initialize()
    {
      SpeedCap = 128;
      Intelligence = 11;
      InCoverage = true;

      base.Initialize();

      // Cover one of the wide receivers
      if (Random.Next(0, 16) < 8)
      {
        CoveredPlayer = OffenseWideReceiverTop;
      }
      else
      {
        CoveredPlayer = OffenseWideReceiverBottom;
      }
      CoverAfterMove = Random.Next(10, 350); // Initially don't cover anyone but switch later on.

      TargetPlayer = DefenderMiddleLinebacker; // Inital zone coverage, switches later
      if (Random.Next(0, 15) < 2) // Blitz
      {
        DefensiveMode = DefensiveMode.Blitz;
        Intelligence = 9; // Mixed in with blocker less intelligence
        TargetPlayer = ControllablePlayer;
        CoverAfterMove = 999999; // Never switch;
        if (Random.Next(0, 15) < 7)
          Top = Game.FieldCenterY - 200;
        else
          Top = Game.FieldCenterY + 200;

        Left = Game.LineOfScrimagePixel + 30;
      }
      else if (Random.Next(0, 10) < 10)
      {
        DefensiveMode = DefensiveMode.Normal;
        Left = Game.LineOfScrimagePixel + 300;
        Top += Random.Next(0, 200) - 100;
      }
      else
      {
        DefensiveMode = DefensiveMode.Soft;
        Left = Game.LineOfScrimagePixel + 500;
        Top += Random.Next(0, 200) - 100;
      }
    }

    public override void Move()
    {
      // Either quarterback is running or ball was caught 
      if (ControllablePlayer.Left > Game.LineOfScrimagePixel - 30 && InCoverage == true)
      {
        TargetPlayer = ControllablePlayer;
        DefensiveMode = DefensiveMode.Soft;
        ChangeX += 10;
        base.MoveTowardsTarget(TargetPlayer.Left + 320, TargetPlayer.Top);
        InCoverage = false;
      }
      if(InCoverage && TotalMoves > CoverAfterMove) // Swicth to covering wide receivers
      {
        TargetPlayer = CoveredPlayer;
        CoverAfterMove = 99999999; // Never switch again
      }
      if (IsThrowing)
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
        if(TargetPlayer == DefenderMiddleLinebacker)
        {
          calculatedTargetX += 50;
          if (TargetPlayer.Top < Game.FieldCenterY)
            calculatedTargeyY = Game.FieldCenterY + 10;
          else
            calculatedTargeyY = Game.FieldCenterY - 10;
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

        int random = Random.Next(0, 10);
        if (random < 9)
        {
          BallAsPlayer.BallIsCatchable = false; // Tipped ball, ball is uncatchable
          BallAsPlayer.SpinDefectedBall();
        }
        else
        {
          ParentGame.EndPlay(EndPlayType.Intercepted, this, "Intercepted.");
          return;
        }
      }

      if (collidedWithPlayer.HasBall && !IsThrowing)
      {
        ParentGame.EndPlay(EndPlayType.Tackled, this, "Tackled.");
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
