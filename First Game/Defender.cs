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
    public DefensiveMode DefensiveMode = DefensiveMode.Normal;
    public override void Initialize()
    {
      this.Team = 2;
      base.Initialize();
    }

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
      //if (collidedWithPlayer.HasBall)
      //{
      //  ParentGame.EndPlay("Tackled");
      //  return;
      //}

      if (this.MovingAroundBlocker > 0)
      {
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
          this.ChangeY = 5;
          if (this.TargetPlayer.Left < this.Left - 60 && this.ChangeX < -60)
            this.ChangeX -= 10;
          else if (this.TargetPlayer.Left > this.Left + 50 && this.ChangeX > 60)
            this.ChangeX += 10;
          else
            this.ChangeX = 40 * r;
          break;
        case CollisionOrientation.Below:
          this.ChangeY = -5;
          if (this.TargetPlayer.Left < this.Left - 60 && this.ChangeX < -60)
            this.ChangeX -= 10;
          else if (this.TargetPlayer.Left > this.Left + 50 && this.ChangeX > 60)
            this.ChangeX += 10;
          else
            this.ChangeX = 40 * r;
          break;
        case CollisionOrientation.ToLeft:
          this.ChangeX = -5;
          if (this.TargetPlayer.Top < this.Top -60 && this.ChangeY < -60)
            this.ChangeY -= 10;
          else if (this.TargetPlayer.Top > this.Top + 50 && this.ChangeY > 60)
            this.ChangeY += 10;
          else
            this.ChangeY = 40 * r;
          break;
        case CollisionOrientation.ToRight:
          this.ChangeX = 5;
          if (this.TargetPlayer.Top < this.Top - 60 && this.ChangeY < -60)
            this.ChangeY -= 10;
          else if (this.TargetPlayer.Top > this.Top + 50 && this.ChangeY > 60)
            this.ChangeY += 10;
          else
            this.ChangeY = 40 * r;
          break;
      }
      this.MovingAroundBlocker = 18;
    }
  }
}
