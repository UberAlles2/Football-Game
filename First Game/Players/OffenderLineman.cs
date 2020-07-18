using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  class OffenderOutsideLinemanTop : OffenderLineman 
  {
    public override void Initialize()
    {
      base.Initialize();
      ChangeX = -80;
    }
    public override void Move()
    {
      base.Move();
    }
  }
  class OffenderLinemanUpper : OffenderLineman
  {
    public override void Initialize()
    {
      base.Initialize();
    }
    public override void Move()
    {
      base.Move();
    }
  }
  class OffenderLinemanCenter : OffenderLineman
  {
    public Player DefenseLinemanUpper;
    public Player DefenseLinemanLower;

    public override void Initialize()
    {
      if (Random.Next(0, 20) < 10)
        TargetPlayer = DefenseLinemanUpper;
      else
        TargetPlayer = DefenseLinemanLower;
      base.Initialize();
    }
    public override void Move()
    {
      base.Move();
    }
  }
  class OffenderLinemanLower : OffenderLineman
  {
    public override void Initialize()
    {
      base.Initialize();
    }
    public override void Move()
    {
      base.Move();
    }
  }
  class OffenderOutsideLinemanBottom : OffenderLineman 
  {
    public override void Initialize()
    {
      base.Initialize();
      ChangeX = -80;
    }
    public override void Move()
    {
      base.Move();
    }
  }

  class OffenderLineman : Offender
  {
    public override void Initialize()
    {
      SpeedCap = 90;

      base.Initialize();
    }

    public override void Move()
    {
      if (ControllablePlayer.Left > Game.LineOfScrimagePixel && TargetPlayer != ControllablePlayer)
      {
        TargetPlayer = ControllablePlayer;
        ChangeX += 20;
        base.MoveTowardsTarget(TargetPlayer.Left + 160 + (TargetPlayer.ChangeX / 2), TargetPlayer.Top);
      }

      if (VerticalPosition == VerticalPosition.PositionTop && ControllablePlayer.Top > Top + 16 && TargetPlayer.Top > Top)
        ChangeY += 1;
      if (VerticalPosition == VerticalPosition.PositionBottom && ControllablePlayer.Top < Top - 16 && TargetPlayer.Top < Top)
        ChangeY += -1;

      // Horizontal move
      if (TargetPlayer.Left < Left + 20)
      {
        ChangeX -= 2;
      }
      if (TargetPlayer.Left < Left + 30)
      {
        TargetPlayer.OffsetY = TargetPlayer.OffsetY - (Math.Sign(TargetPlayer.OffsetY) * 2);
        ChangeX -= 3;
      }
      else if (TargetPlayer.Left > Left - 8)
        ChangeX += 3;
      else
        ChangeX = ChangeX / 2;

      // Vertical move
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
