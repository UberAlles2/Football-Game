using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  class OffenderQuarterback : Offender
  {
    public override void Initialize()
    {
      SpeedCap = 96;
      HasBall = true;
      Player.ControllablePlayer = this;
      PicBox.Image = ParentForm.picBearsBall.Image;

      base.Initialize();
    }

    public override void Move()
    {
      base.Move();
    }

    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      base.CollisionMove(collidedWithPlayer, collisionOrientation);
    }
  }
}
