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
      int r = Player.Random.Next(-10, 10);

      switch (collisionOrientation)
      {
        case CollisionOrientation.Above:
          ChangeY = -8;
          break;
        case CollisionOrientation.Below:
          ChangeY = 8;
          break;
        case CollisionOrientation.ToLeft:
          ChangeX = -8;
      break;
        case CollisionOrientation.ToRight:
          ChangeX = 8;
          break;
      }

      switch (collisionOrientation)
      {
        case CollisionOrientation.Above:
        case CollisionOrientation.Below:
          if (TargetPlayer.Left < Left - 60 && ChangeX < -40) // The target is way to the left and this is moving left, keep moving left
            ChangeX -= 8;
          else if (TargetPlayer.Left > Left + 60 && ChangeX > 40) // The target is way to the right and this is moving right, keep moving right
            ChangeX += 8;
          else // Else, randomly go left or right.
            ChangeX = 8 * r;
          break;
        case CollisionOrientation.ToLeft:
        case CollisionOrientation.ToRight:
          if (TargetPlayer.Top < Top - 60 && ChangeY < -40)
            ChangeY -= 8;
          else if (TargetPlayer.Top > Top + 60 && ChangeY > 40)
            ChangeY += 8;
          else
            ChangeY = 8 * r;
          break;
      }
      MovingAroundBlocker = 20;
    }
  
    public void AIBasicMoveTowardsTarget(Player target, out int targetX, out int targetY, DefensiveMode defensiveMode)
    {
      targetX = 1;
      targetY = 1;
    }
  }
}
