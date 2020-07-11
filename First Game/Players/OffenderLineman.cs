using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  class OffenderLinemanTop : OffenderLineman { }
  class OffenderLinemanMiddle : OffenderLineman { }
  class OffenderLinemanBottom : OffenderLineman { }

  class OffenderLineman : Offender
  {
    public override void Initialize()
    {
      SpeedCap = 90;

      base.Initialize();
    }

    public override void Move()
    {
      if (Player.ControllablePlayer.Left > Game.LineOfScrimage && TargetPlayer != Player.ControllablePlayer)
      {
        TargetPlayer = Player.ControllablePlayer;
        ChangeX += 20;
        base.MoveTowardsTarget(TargetPlayer.Top, TargetPlayer.Left + 160 + (TargetPlayer.ChangeX / 2));
      }

      //Player.ControllablePlayer.ChangeX / 2; // TODO put back?

      if (TargetPlayer.Left < Left + 4 && Player.ControllablePlayer.Left < Left - 8)
        ChangeX -= 10;
      else if (TargetPlayer.Left > Left + 4)
        ChangeX += 2;
      else
        ChangeX = ChangeX / 2;

      if (TargetPlayer.Top < Top - 4)
        ChangeY -= 2;
      else if (TargetPlayer.Top > Top)
        ChangeY += 2;
      else
        ChangeY = ChangeY / 2;


      base.Move();
    }
  }
}
