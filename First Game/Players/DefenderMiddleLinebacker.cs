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
        Left = 200;
      }
      else if (Random.Next(0, 10) < 8)
        DefensiveMode = DefensiveMode.Normal;
      else
        DefensiveMode = DefensiveMode.Soft;
    }

    public override void Move()
    {
      if (IsThrowing)
        this.ChangeX += 20;
 
      if (TargetPlayer != Player.ControllablePlayer)
        TargetPlayer = Player.ControllablePlayer;

      Random random = new Random();
      if(this.Intelligence > random.Next(0,15) || MovingAroundBlocker > 0)
      {

        int calculatedTargetX = AI_BasicMoveTowardsTarget();

        ChangeX += 8; // keeps player moving down field a little
        base.MoveTowardsTarget(calculatedTargetX, TargetPlayer.Top);
      }

      base.Move();
    }

    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      if (collidedWithPlayer.HasBall && !IsThrowing)
      {
        ParentGame.EndPlay("Tackled");
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
