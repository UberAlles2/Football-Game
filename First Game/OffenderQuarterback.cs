using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  class OffenderQuarterback : Player
  {
    public override void Move()
    {
      base.Move();
    }

    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      if (this.HasBall && collidedWithPlayer is Defender)
      {
        MessageBox.Show("collision");
        return;
      }
      base.CollisionMove(collidedWithPlayer, collisionOrientation);
    }
  }
}
