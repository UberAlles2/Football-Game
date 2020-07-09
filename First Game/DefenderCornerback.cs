using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  class DefenderCornerback : Defender
  {
    public override void Initialize()
    {
      SpeedCap = 120;
      Intelligence = 11;
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
        if(TargetPlayer.Top < Game.FieldCenterY)
          base.MoveTowardsTarget(TargetPlayer.Top + 30, TargetPlayer.Left + (TargetPlayer.ChangeX / 2));
        else
          base.MoveTowardsTarget(TargetPlayer.Top - 30, TargetPlayer.Left + (TargetPlayer.ChangeX / 2));
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
