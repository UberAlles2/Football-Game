using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  class DefenderLinemanUpper : DefenderLineman
  {
    public override void Initialize()
    {
      base.Initialize();
    }
    public override void Move()
    {
      ChangeY -= 1; // Tendency to move up
      //if (Top > Game.FieldCenterY - 80) // Top Outside lineman should not leave his zone and stay on top 
      //{
      //  ChangeY -= 4;
      //}
      OffsetY++;
      base.Move();
    }
    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      base.CollisionMove(collidedWithPlayer, collisionOrientation);
    }
  }
  class DefenderLinemanLower : DefenderLineman
  {
    public override void Initialize()
    {
      base.Initialize();
    }
    public override void Move()
    {
      ChangeY += 1; // Tendency to move down
      //if (Top < Game.FieldCenterY + 80) // Bottom lineman should not leave his zone and stay on bottom
      //{
      //  ChangeY += 4;
      //}
      OffsetY--;
      base.Move();
    }
    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      base.CollisionMove(collidedWithPlayer, collisionOrientation);
    }
  }

  class DefenderLineman : Defender
  {
    public override void Initialize()
    {
      SpeedCap = 104;
      Intelligence = 8;
      TargetPlayer = Player.ControllablePlayer;
      base.Initialize();
    }

    public override void Move()
    {
      int calculatedTargetY = TargetPlayer.Top;

      if (TargetPlayer != Player.ControllablePlayer) // if catch is made
        TargetPlayer = Player.ControllablePlayer;

      if (Player.ControllablePlayer.Left > Game.LineOfScrimagePixel + 8)
      {
        Intelligence = 11; // Once the runner get past the line of scrimage, this defender doen't have to worry about the blockers.
      }

      if(Intelligence > Random.Next(0,15) || MovingAroundBlocker > 0)
      {
        if (Game.DetectCloseCollision(this, TargetPlayer, 90))
        {
          calculatedTargetY = TargetPlayer.Top;
        }
        else
        {
          calculatedTargetY = TargetPlayer.Top + OffsetY;
        }
        int calculatedTargetX = AI_BasicMoveTowardsTargetX();
        base.MoveTowardsTarget(calculatedTargetX, calculatedTargetY);
      }

      base.Move();
    }

    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      if (collidedWithPlayer.HasBall && !IsThrowing)
      {
        if (Random.Next(0, 10) > 1) // Allow a missed tackle 10% of time.
          ParentGame.EndPlay(EndPlayType.Tackled, "Tackled by Middle Lineman.");
      }

      if (collidedWithPlayer is Offender)
      {
        if (collisionOrientation == CollisionOrientation.ToLeft || collisionOrientation == CollisionOrientation.ToRight)
          ChangeX = ChangeX/2;
        if (collisionOrientation == CollisionOrientation.Above || collisionOrientation == CollisionOrientation.Below)
          ChangeY = ChangeY/2;

        base.MoveAroundPlayer(collisionOrientation);
        return;
      }

      base.CollisionMove(collidedWithPlayer, collisionOrientation);
    }
  }
}
