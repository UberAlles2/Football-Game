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
      SpeedCap = 116;
      Intelligence = 10;
      TargetPlayer = ControllablePlayer;

      base.Initialize();

      if (Random.Next(0, 10) < 3)
      {
        Intelligence = 9; // Mixed in with blocker less intelligence
        DefensiveMode = DefensiveMode.Blitz;
        Top = Game.FieldCenterY - 40;
        Left = InitialLeft;
      }
      else if (Random.Next(0, 10) < 8)
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
        if(DefensiveMode == DefensiveMode.Blitz)
           ChangeX += 10; // If the ball is thrown go out where the receivers are.
        else
          ChangeX += 2; 

        DefensiveMode = DefensiveMode.Blitz; // Switch to tight coverage.
        if (Player.ControllablePlayer.Top < Top)
          ChangeY += 4;
        else
          ChangeY -= 4;
      }
      else if(Intelligence > Random.Next(0,15) || MovingAroundBlocker > 0)
      {
        base.MoveTowardsTarget(AI_BasicMoveTowardsTargetX(), TargetPlayer.Top);
      }

      base.Move();
    }

    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      if (collidedWithPlayer.HasBall && !IsThrowing)
      {
        ParentGame.EndPlay(EndPlayType.Tackled, "Tackled");
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
