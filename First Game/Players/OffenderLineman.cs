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
      Top += (2 -PlayOptionsForm.RunPassTendency); // 0-10, 0 better running, 10 better passing
      Left -= (2 - PlayOptionsForm.RunPassTendency); // 0-10, 0 better running, 10 better passing
      ChangeX -= 70 + (PlayOptionsForm.RunPassTendency); // 0-10, 0 better running, 10 better passing;
      ChangeY += (PlayOptionsForm.RunPassTendency); // 0-10, 0 better running, 10 better passing;
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
      Top -= (2 - PlayOptionsForm.RunPassTendency); // 0-10, 0 better running, 10 better passing
      Left -= (2 - PlayOptionsForm.RunPassTendency); // 0-10, 0 better running, 10 better passing
      ChangeX -= 70 + (PlayOptionsForm.RunPassTendency); // 0-10, 0 better running, 10 better passing;
      ChangeY -= (PlayOptionsForm.RunPassTendency); // 0-10, 0 better running, 10 better passing;
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
      // QB passed line of scrimmage
      if (ControllablePlayer.Left > PlayingField.LineOfScrimagePixel && TargetPlayer != ControllablePlayer)
      {
        TargetPlayer = ControllablePlayer;
        ChangeX += 20;
        base.MoveTowardsTarget(TargetPlayer.Left + 160 + (TargetPlayer.ChangeX / 2), TargetPlayer.Top);
      }

      if (VerticalPosition == VerticalPosition.PositionTop && ControllablePlayer.Top > Top + 16 && TargetPlayer.Top > Top)
        ChangeY += 2;
      if (VerticalPosition == VerticalPosition.PositionBottom && ControllablePlayer.Top < Top - 16 && TargetPlayer.Top < Top)
        ChangeY -= 2;

      //-------------------------------- Horizontal move

      // Passing tendacy, fall back
      if (PlayOptionsForm.RunPassTendency > 6 && Left > 100) // 0-10, 0 better running, 10 better passing;
        ChangeX -= 1;

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

      // stay off left edge of field
      if(Left < 100)
        ChangeX += 5; 

      //-------------------------------------------- Vertical move

      // Passing tendacy, move towards QB
      if (PlayOptionsForm.RunPassTendency > 6) // 0-10, 0 better running, 10 better passing;
      {
        if(ControllablePlayer.Top < Top)
          ChangeY -= 2;
        else
          ChangeY += 2;
      }

      if (TargetPlayer.Top < Top + 6)
        ChangeY -= 2;
      else if (TargetPlayer.Top > Top - 6)
        ChangeY += 2;
      else
        ChangeY = ChangeY / 2;

      base.Move();
    }
  }
}
