using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  class DefenderMiddleLineman : Defender
  {
    public override void Initialize()
    {
      this.SpeedCap = 95;
      this.Intelligence = 9;
      this.TargetPlayer = Game.ControllablePlayer;
      base.Initialize();
    }

    public override void Move()
    {
      if (MovingAroundBlocker > 0)
      {
        MovingAroundBlocker--;
        base.Move(); 
        return;
      }
      
      if(this.Intelligence > Game.Random.Next(0,15))
      {
        base.MoveTowardsTarget(TargetPlayer.Top, TargetPlayer.Left);
      }
      base.Move();
    }

    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      if (collidedWithPlayer.HasBall && !Game.IsThrowing)
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
