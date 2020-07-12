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
      if (ControllablePlayer.Left > Game.LineOfScrimage && TargetPlayer != ControllablePlayer)
      {
        TargetPlayer = ControllablePlayer;
        ChangeX += 20;
        base.MoveTowardsTarget(TargetPlayer.Left + 160 + (TargetPlayer.ChangeX / 2), TargetPlayer.Top);
      }

      if (VerticalPosition == VerticalPosition.PositionTop && ControllablePlayer.Top > Top + 16 && TargetPlayer.Top > Top)
        ChangeY += 1;
      if (VerticalPosition == VerticalPosition.PositionBottom && ControllablePlayer.Top < Top - 16 && TargetPlayer.Top < Top)
        ChangeY += -1;

      if (TargetPlayer.Left < Left + 20)
      {
        ChangeX -= 2;
      }
      if (TargetPlayer.Left < Left + 30)
      {
        TargetPlayer.Offset = TargetPlayer.Offset - (Math.Sign(TargetPlayer.Offset) * 2);
        ChangeX -= 3;
      }
      else if (TargetPlayer.Left > Left - 4)
        ChangeX += 2;
      else
        ChangeX = ChangeX / 2;

      if (TargetPlayer.Top < Top + 4)
        ChangeY -= 2;
      else if (TargetPlayer.Top > Top - 4)
        ChangeY += 2;
      else
        ChangeY = ChangeY / 2;

      base.Move();
    }
  }
}
