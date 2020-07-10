using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  class DefenderCornerbackTop : DefenderCornerback { }
  class DefenderCornerbackBottom : DefenderCornerback { }
  class DefenderCornerback : Defender
  {
    private bool InCoverage = true;
    

    public override void Initialize()
    {
      SpeedCap = 132;
      Intelligence = 9;
      InCoverage = true;
      base.Initialize();
    }

    public override void Move()
    {
      if (Player.ControllablePlayer.Left > Game.LineOfScrimage && InCoverage == true)
      {
        TargetPlayer = Player.ControllablePlayer;
        ChangeX += 20;
        base.MoveTowardsTarget(TargetPlayer.Top, TargetPlayer.Left + 160);
        InCoverage = false;
      }

      if (this.Intelligence > Random.Next(0,15) || MovingAroundBlocker > 0)
      {
        if (InCoverage)
        {
          if (TargetPlayer.Top < Game.FieldCenterY)
            base.MoveTowardsTarget(TargetPlayer.Top + 40, TargetPlayer.Left + Random.Next(-200, 200) + (TargetPlayer.ChangeX / 3));
          else
            base.MoveTowardsTarget(TargetPlayer.Top - 40, TargetPlayer.Left + Random.Next(-200, 200) + (TargetPlayer.ChangeX / 3));
        }
        else
        {
          int diffY = Math.Abs(TargetPlayer.Top - Top);
          base.MoveTowardsTarget(TargetPlayer.Top, TargetPlayer.Left + diffY + (TargetPlayer.ChangeX / 2));
        }
      }

      base.Move();
    }

    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      if (collidedWithPlayer is BallAsPlayer)
      {
        if (((BallAsPlayer)collidedWithPlayer).BallIsCatchable == false)
          return;
        
        IsThrowing = false;
        if (Random.Next(0, 10) > 4)
          ParentGame.EndPlay("Cornerback dropped");
        else
        {
          HasBall = true;
          PicBox.BackColor = System.Drawing.Color.Yellow;
          ParentGame.EndPlay("Intercepted");
        }
      }

      if (collidedWithPlayer.HasBall && !IsThrowing)
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
