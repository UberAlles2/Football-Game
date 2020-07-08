using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  class OffenderWideReceiver : Offender
  {
    public override void Initialize()
    {
      SpeedCap = 140;
      HasBall = false;
      PicBox.MouseClick += new System.Windows.Forms.MouseEventHandler(MouseClick);

      base.Initialize();
    }

    private void MouseClick(object sender, MouseEventArgs e)
    {
      MouseEventArgs mouseEventArgs = new MouseEventArgs(MouseButtons.Left, 1, Left + 16, Top + 16, 0); 
      ParentGame.MouseClick(sender, mouseEventArgs);
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
