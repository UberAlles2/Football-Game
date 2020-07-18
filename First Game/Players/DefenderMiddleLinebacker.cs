using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  class DefenderMiddleLinebacker : Defender
  {
    public override void Initialize()
    {
      int shortYardage = 0;
      
      SpeedCap = 116;
      Intelligence = 10;
      TargetPlayer = ControllablePlayer;

      base.Initialize();

      if(Game.PlayOptionsFormStats.YardsToGo < 2) // Short yardage play
      {
        Left = InitialLeft - 80;
        ChangeX = -12;
        shortYardage = 3; // More likely to blitz
      }

      if (Random.Next(0, 12) < 3 + shortYardage)
      {
        Intelligence = 9; // Mixed in with blocker less intelligence
        DefensiveMode = DefensiveMode.Blitz;
        Top = Game.FieldCenterY - 40;
      }
      else if (Random.Next(0, 10) < 8 + shortYardage)
        DefensiveMode = DefensiveMode.Normal;
      else
        DefensiveMode = DefensiveMode.Soft;
    }

    public override void Move()
    {
      if (TargetPlayer != Player.ControllablePlayer) // If a catch is made by offensive player
        TargetPlayer = Player.ControllablePlayer;    // Change your target

      if (IsThrowing)
      {
        DefensiveMode = DefensiveMode.Blitz; // Switch to tight coverage.
        ChangeX += 10; // If the ball is thrown go out where the receivers are.

        if (ControllablePlayer.Top < Top)
          ChangeY += 4;
        else
          ChangeY -= 4;
      }
      else if(Intelligence > Random.Next(0,15) || MovingAroundBlocker > 0)
      {
        int calculatedTargetX = AI_BasicMoveTowardsTargetX();
        switch(DefensiveMode)
        {
          case DefensiveMode.Normal:
            if (TargetPlayer.Left < Game.LineOfScrimagePixel - 30)
              calculatedTargetX += (Game.LineOfScrimagePixel - 90);
            break;
          case DefensiveMode.Soft:
            if (TargetPlayer.Left < Game.LineOfScrimagePixel - 30)
              calculatedTargetX += (Game.LineOfScrimagePixel - 10);
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
