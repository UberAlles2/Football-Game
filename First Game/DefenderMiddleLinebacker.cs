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

        int calculatedTargetY = TargetPlayer.Top; 
        int calculatedTargetX = 0;
        bool closeToTackle = false;

        int diffX = 220;
        if (TargetPlayer.Left > 120 && Math.Abs(TargetPlayer.Top - this.Top) < 100)
          diffX = 40;

        if (Math.Abs(this.CenterX - TargetPlayer.CenterX) < TargetPlayer.PlayerWidth + 60 && Math.Abs(this.CenterY - TargetPlayer.CenterY) < TargetPlayer.PlayerHeight + 60)
          closeToTackle = true;

        // Go right towards target if close to target or are blitzing
        if (closeToTackle || this.DefensiveMode == DefensiveMode.Blitz)
        {
          calculatedTargetX = TargetPlayer.Left;
        }
        else if (this.DefensiveMode == DefensiveMode.Normal)
        {
          calculatedTargetX = TargetPlayer.Left + (diffX/2);
        }
        else if (this.DefensiveMode == DefensiveMode.Soft)
        {
          calculatedTargetX = TargetPlayer.Left + diffX;
        }

        base.MoveTowardsTarget(calculatedTargetY, calculatedTargetX);
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
