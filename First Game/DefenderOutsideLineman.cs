using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  class DefenderOutsideLinemanTop : DefenderOutsideLineman { }
  class DefenderOutsideLinemanBottom : DefenderOutsideLineman { }

  class DefenderOutsideLineman : Defender
  {
    public Defender CoDefender;

    public override void Initialize()
    {
      SpeedCap = 105;
      Intelligence = 6;
      TargetPlayer = Game.ControllablePlayer;
      base.Initialize();
    }

    public override void Move()
    {
      if (TargetPlayer != Game.ControllablePlayer)
        TargetPlayer = Game.ControllablePlayer;

      if(this.Intelligence > Game.Random.Next(0,15))
      {
        int diffX = 1;
        if (TargetPlayer.Left - 100 < this.Left)
          diffX = (TargetPlayer.Left - CoDefender.Left) / -64;

        if(Game.Random.Next(0, 15) > 6)
        {
          if (this is DefenderOutsideLinemanTop && Offset < -44)
            Offset++;
          if (this is DefenderOutsideLinemanBottom && Offset > 44)
            Offset--;
          //if(Math.Abs(this.Offset) < 45)
          //  this.Offset--;
        }

        int calculatedTargetY = 0;
        int calculatedTargetX = 0;
        //         if (Math.Abs(this.CenterX - TargetPlayer.CenterX) < TargetPlayer.PlayerWidth + 90 && Math.Abs(this.CenterY - TargetPlayer.CenterY) < TargetPlayer.PlayerHeight + 90)
        if (Game.DetectCloseCollision(this, TargetPlayer, 90))
        {
          calculatedTargetY = TargetPlayer.Top;
          calculatedTargetX = TargetPlayer.Left + (TargetPlayer.ChangeX / 2);
        }
        else
        {
          calculatedTargetY = (TargetPlayer.Top + TargetPlayer.Top + CoDefender.Top) / 3 + this.Offset * diffX;
          calculatedTargetX = (TargetPlayer.Left + 20 + CoDefender.Left) / 2 + (TargetPlayer.ChangeX / 2);
        }

        base.MoveTowardsTarget(calculatedTargetY, calculatedTargetX);
      }
      base.Move();
    }
    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      if (collidedWithPlayer.HasBall && !Game.IsThrowing)
      {
        if (Game.Random.Next(0, 10) > 1) // Allow a missed tackle 10% of time.
          ParentGame.EndPlay("Tackled by Outside Lineman.");
      }

      if (collidedWithPlayer is Offender)
      {
        base.MoveAroundPlayer(collisionOrientation);
        return;
      }

      base.CollisionMove(collidedWithPlayer, collisionOrientation);
    }

  }
}
