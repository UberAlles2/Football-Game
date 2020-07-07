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
    public Player TargetPlayer;
    public int Intelligence = 10;

    public override void Move()
    {
        base.Move();
    }
    public override void MoveTowardsTarget(int Y, int X)
    {

      // Vertical move
      if (Math.Abs(this.Left - X) < Math.Abs(this.Top - Y))
      {
        if (Y < this.Top)
        {
          this.ChangeY += -16;
        }
        if (Y > this.Top)
        {
          this.ChangeY += 16;
        }
        if (X < this.Left)
        {
          this.ChangeX += -4;
        }
        if (X > this.Left)
        {
          this.ChangeX += 4;
        }

      }
      else // Horizontal Move
      {
        if (Y < this.Top)
        {
          this.ChangeY += -4;
        }
        if (Y > this.Top)
        {
          this.ChangeY += 4;
        }
        if (X < this.Left)
        {
          this.ChangeX += -16;
        }
        if (X > this.Left)
        {
          this.ChangeX += 16;
        }
      }
    }

    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      if (this.MovingAroundBlocker > 0)
      {
        return;
      }
      base.CollisionMove(collidedWithPlayer, collisionOrientation);
    }

    public override void MoveAroundPlayer(CollisionOrientation collisionOrientation)
    {
      int r = new Random().Next(-2, 3);

      switch (collisionOrientation)
      {
        case CollisionOrientation.Above:
          this.ChangeY = 5;
          this.ChangeX = 40 * r;
          break;
        case CollisionOrientation.Below:
          this.ChangeY = -5;
          this.ChangeX = 40 * r;
          break;
        case CollisionOrientation.ToLeft:
          this.ChangeX = -5;
          this.ChangeY = 40 * r;
          break;
        case CollisionOrientation.ToRight:
          this.ChangeX = 5;
          this.ChangeY = 40 * r;
          break;
      }
      this.MovingAroundBlocker = 20;
    }
  }
}
