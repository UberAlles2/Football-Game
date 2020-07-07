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
    public int Offset = -40;
    
    public override void Move()
    {
      Random random = new Random();
      if(this.Intelligence > random.Next(0,15))
      {
        int diffX = 1;
        if (TargetPlayer.Left - 100 < this.Left)
          diffX = (TargetPlayer.Left - CoDefender.Left) / -64;

        int calculatedTargetY = (TargetPlayer.Top + CoDefender.Top) / 2 + this.Offset * diffX;
        int calculatedTargetX = (TargetPlayer.Left + CoDefender.Left) / 2;
        base.MoveTowardsTarget(calculatedTargetY, calculatedTargetX);
      }
      base.Move();
    }
    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      if (collidedWithPlayer is Offender)
      {
          base.MoveAroundPlayer(collisionOrientation);
          return;
      }

      base.CollisionMove(collidedWithPlayer, collisionOrientation);
    }

  }
}
