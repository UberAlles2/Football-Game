using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  public class OffenderOutsideLinemanTop : OffenderLineman 
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
  public class OffenderLinemanUpper : OffenderLineman
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
  public class OffenderLinemanCenter : OffenderLineman
  {
    public override void Initialize()
    {
      int r = Random.Next(0, 25);
      if (r < 10)
        TargetPlayer = Game.defenderLinemanUpper;
      else if (r < 20)
        TargetPlayer = Game.defenderLinemanLower;
      else 
        TargetPlayer = Game.defenderMiddleLinebacker;

      base.Initialize();
    }
    public override void Move()
    {
      base.Move();
    }
  }
  public class OffenderLinemanLower : OffenderLineman
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
  public class OffenderOutsideLinemanBottom : OffenderLineman 
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

  public class OffenderLineman : Offender
  {
    public override void Initialize()
    {
      SpeedCap = 96;
      base.Initialize();
    }

    public override void Move()
    {
      if (ControllablePlayer.Left > PlayingField.LineOfScrimagePixel && TargetPlayer != ControllablePlayer)
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
