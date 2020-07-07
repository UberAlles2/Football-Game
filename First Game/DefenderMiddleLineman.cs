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
    public override void Move()
    {
      if (MovingAroundBlocker > 0)
      {
        MovingAroundBlocker--;
        base.Move(); 
        return;
      }

      Random random = new Random();
      if(this.Intelligence > random.Next(0,15))
      {
        base.MoveTowardsTarget(TargetPlayer.Top, TargetPlayer.Left);
      }
      base.Move();
    }

    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      if(collidedWithPlayer is Offender)
      {
        if (collidedWithPlayer.HasBall)
          MessageBox.Show("Tackled");
        base.MoveAroundPlayer(collisionOrientation);
        return;
      }

      base.CollisionMove(collidedWithPlayer, collisionOrientation);
    }
  }
}
