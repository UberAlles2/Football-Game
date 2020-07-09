using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  class OffenderMiddleLineman : Offender
  {
    public override void Initialize()
    {
      SpeedCap = 90;

      base.Initialize();
    }

    public override void Move()
    {
      ChangeX = Game.ControllablePlayer.ChangeX / 2;
      
      base.Move();
    }
  }
}
