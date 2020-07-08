using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  class DefenderOutsideLineman : Defender
  {
    public Defender CoDefender; 
    public int Offset = 0;

    public override void Initialize()
    {
      this.SpeedCap = 90;
      this.Intelligence = 5;
      this.TargetPlayer = Game.PlayerWithBall;
      base.Initialize();
    }

    public override void Move()
    {
      Random random = new Random();
      if(this.Intelligence > random.Next(0,15))
      {
        int diffX = 1;
        if (TargetPlayer.Left - 100 < this.Left)
          diffX = (TargetPlayer.Left - CoDefender.Left) / -64;

        if(random.Next(0, 15) > 13)
        {
          if (this.Offset > 40)
            this.Offset--;
          if (this.Offset < -40)
            this.Offset++;
        }

        int calculatedTargetY = 0;
        int calculatedTargetX = 0;
        if (Math.Abs(this.CenterX - TargetPlayer.CenterX) < TargetPlayer.PlayerWidth + 90 && Math.Abs(this.CenterY - TargetPlayer.CenterY) < TargetPlayer.PlayerHeight + 90)
        {
          calculatedTargetY = TargetPlayer.Top;
          calculatedTargetX = TargetPlayer.Left;
        }
        else
        {
          calculatedTargetY = (TargetPlayer.Top + CoDefender.Top) / 2 + this.Offset * diffX;
          calculatedTargetX = (TargetPlayer.Left + CoDefender.Left) / 2;
        }

        base.MoveTowardsTarget(calculatedTargetY, calculatedTargetX);
      }
      base.Move();
    }
    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      if (collidedWithPlayer.HasBall)
        ParentGame.EndPlay("Tackled");

      if (collidedWithPlayer is Offender)
      {
        base.MoveAroundPlayer(collisionOrientation);
        return;
      }

      base.CollisionMove(collidedWithPlayer, collisionOrientation);
    }

  }
}
