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
    public int Offset = 0;

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

      Random random = new Random();
      if(this.Intelligence > random.Next(0,15))
      {
        int diffX = 1;
        if (TargetPlayer.Left - 100 < this.Left)
          diffX = (TargetPlayer.Left - CoDefender.Left) / -64;

        if(random.Next(0, 15) > 6)
        {
          if (this.Offset > 42)
            this.Offset--;
          if (this.Offset < -42)
            this.Offset++;
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
        ParentGame.EndPlay("Tackled");
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
