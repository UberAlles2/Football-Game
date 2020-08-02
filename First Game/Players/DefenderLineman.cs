using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  public class DefenderLinemanUpper : DefenderLineman
  {
    public override void Initialize()
    {
      base.Initialize();
    }
    public override void Move()
    {
      ChangeY -= 1; // Tendency to move up
      if (Top < PlayingField.FieldCenterY - 280) // Top Outside lineman should not leave his zone and stay on top 
      {
        OffsetY++;
      }
      base.Move();
    }
    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      base.CollisionMove(collidedWithPlayer, collisionOrientation);
    }
  }
  public class DefenderLinemanLower : DefenderLineman
  {
    public override void Initialize()
    {
      base.Initialize();
    }
    public override void Move()
    {
      ChangeY += 1; // Tendency to move down
      if (Top > PlayingField.FieldCenterY + 280) // Bottom lineman should not leave his zone and stay on bottom
      {
        OffsetY--;
      }
      base.Move();
    }
    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      base.CollisionMove(collidedWithPlayer, collisionOrientation);
    }
  }

  public class DefenderLineman : Defender
  {
    public override void Initialize()
    {
      SpeedCap = 102;
      Intelligence = 8;
      TargetPlayer = Player.ControllablePlayer;
      base.Initialize();
    }

    public override void Move()
    {
      int calculatedTargetY = TargetPlayer.Top;

      if (TargetPlayer != Player.ControllablePlayer) // if catch is made
        TargetPlayer = Player.ControllablePlayer;

      if (Player.ControllablePlayer.Left > PlayingField.LineOfScrimagePixel + 8)
      {
        Intelligence = 10; // Once the runner get past the line of scrimage, this defender doen't have to worry about the blockers.
        SpeedCap = 106;
      }

      if(Intelligence > Random.Next(0,15) || MovingAroundBlocker > 0)
      {
        if (DetectCloseCollision(this, TargetPlayer, 90))
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
      if (collidedWithPlayer.HasBall && !IsThrowingOrKicking)
      {
        if (Random.Next(0, 100) > 10) // Allow a missed tackle 10% of time.
        {
          ParentGame.EndPlay(EndPlayType.Tackled, this, "Tackled.");
          return;
        }
      }

      if (collidedWithPlayer is Offender)
      {
        if (collisionOrientation == CollisionOrientation.ToLeft || collisionOrientation == CollisionOrientation.ToRight)
          ChangeX = ChangeX / 2;
        if (collisionOrientation == CollisionOrientation.Above || collisionOrientation == CollisionOrientation.Below)
          ChangeY = ChangeY / 2;

        base.MoveAroundPlayer(collisionOrientation);
        return;
      }

      base.CollisionMove(collidedWithPlayer, collisionOrientation);
    }
  }
}
