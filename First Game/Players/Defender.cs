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
    public override void MoveTowardsTarget(int X, int Y)
    {
      base.MoveTowardsTarget(X, Y);
    }

    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      if (MovingAroundBlocker > 0)
      {
        MovingAroundBlocker--;
        return;
      }

      base.CollisionMove(collidedWithPlayer, collisionOrientation);
    }

    public override void MoveAroundPlayer(CollisionOrientation collisionOrientation)
    {
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

      int r = Random.Next(-10, 10);
      if (MovingAroundBlocker == 0)
      {
        switch (collisionOrientation)
        {
          case CollisionOrientation.Above:
          case CollisionOrientation.Below:
            if (TargetPlayer.Left < Left - 90 && ChangeX < -40) // The target is way to the left and this is moving left, keep moving left
              ChangeX -= 8;
            else if (TargetPlayer.Left > Left + 90 && ChangeX > 40) // The target is way to the right and this is moving right, keep moving right
              ChangeX += 8;
            else // Else, randomly go left or right.
              ChangeX = 8 * r;
            break;
          case CollisionOrientation.ToLeft:
          case CollisionOrientation.ToRight:
            if(Math.Abs(Offset) > 40) // For Outside defensive linemen
              ChangeY = 8 * r;
            else if (TargetPlayer.Top < Top - 90 && ChangeY < -40)
              ChangeY -= 8;
            else if (TargetPlayer.Top > Top + 90 && ChangeY > 40)
              ChangeY += 8;
            else
              ChangeY = 8 * r;
            break;
        }
        MovingAroundBlocker = 20;
      }
    }
  
    public int AI_BasicMoveTowardsTargetX()
    {
      int targetX = 0;
      bool closeToTackle = false;

      // If player is way above or way below his target, you must lead move at a diagonal in front of the target, not right at the target
      int diffY = Math.Abs(TargetPlayer.Top - this.Top);

      if (diffY < 60)
        if (Game.DetectCloseCollision(this, TargetPlayer, 60))
          closeToTackle = true; // Move right towards player

      // Go right towards target if close to target or are blitzing
      if (closeToTackle || this.DefensiveMode == DefensiveMode.Blitz)
      {
        targetX = TargetPlayer.Left + (TargetPlayer.ChangeX / 2);
      }
      else if (this.DefensiveMode == DefensiveMode.Normal)
      {
        if (TargetPlayer.Left < Game.LineOfScrimagePixel)
          targetX = 280;
        else
          targetX = TargetPlayer.Left + (TargetPlayer.ChangeX / 2) + (diffY / 3);
      }
      else if (this.DefensiveMode == DefensiveMode.Soft)
      {
        if (TargetPlayer.Left < Game.LineOfScrimagePixel)
          targetX = 400;
        else
          targetX = TargetPlayer.Left + (TargetPlayer.ChangeX / 2) + (diffY / 2);
      }

      return targetX;
    }
  }
}
