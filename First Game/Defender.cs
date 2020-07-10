using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  class Defender : Player
  {
    public int Intelligence = 10;
    public DefensiveMode DefensiveMode = DefensiveMode.Normal;
    public override void Initialize()
    {
      Team = 2;
      base.Initialize();
    }

    public override void Move()
    {
        base.Move();
    }
    public override void MoveTowardsTarget(int Y, int X)
    {
      base.MoveTowardsTarget(Y, X);
    }

    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      //if (collidedWithPlayer.HasBall)
      //{
      //  ParentGame.EndPlay("Tackled");
      //  return;
      //}

      if (MovingAroundBlocker > 0)
      {
        MovingAroundBlocker--;
        return;
      }

      base.CollisionMove(collidedWithPlayer, collisionOrientation);
    }

    public override void MoveAroundPlayer(CollisionOrientation collisionOrientation)
    {
      int r = new Random().Next(-1, 2);

      switch (collisionOrientation)
      {
        case CollisionOrientation.Above:
          ChangeY = -8;
          if (TargetPlayer.Left < Left - 60 && ChangeX < -60)
            ChangeX -= 10;
          else if (TargetPlayer.Left > Left + 60 && ChangeX > 60)
            ChangeX += 10;
          else
            ChangeX = 40 * r;
          break;
        case CollisionOrientation.Below:
          ChangeY = 8;
          if (TargetPlayer.Left < Left - 60 && ChangeX < -60)
            ChangeX -= 10;
          else if (TargetPlayer.Left > Left + 60 && ChangeX > 60)
            ChangeX += 10;
          else
          {
            if(ChangeX > 60)
              ChangeX = ChangeX - 40;
            else if (ChangeX < -60)
              ChangeX = ChangeX + 40;
            else
              ChangeX = 40 * r;
          }

          break;
        case CollisionOrientation.ToLeft:
          ChangeX = -5;
          if (TargetPlayer.Top < Top -60 && ChangeY < -60)
            ChangeY -= 10;
          else if (TargetPlayer.Top > Top + 60 && ChangeY > 60)
            ChangeY += 10;
          else
            ChangeY = 40 * r;
          break;
        case CollisionOrientation.ToRight:
          if(this is DefenderCornerbackTop) //TODO debug
            ChangeX = ChangeX;

          ChangeX = 5;
          if (TargetPlayer.Left > Left)
            ChangeX += 10;
          else if (TargetPlayer.Top < Top - 60 && ChangeY < -60)
            ChangeY -= 10;
          else if (TargetPlayer.Top > Top + 60 && ChangeY > 60)
            ChangeY += 10;
          else
            ChangeY = 40 * r;
          break;
      }
      MovingAroundBlocker = 20;
    }
  }
}
