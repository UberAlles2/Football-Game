using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  public class DefenderOutsideLinemanTop : DefenderOutsideLineman 
  {
    public override void Initialize()
    {
      base.Initialize();
    }
    public override void Move()
    {
      ChangeY -= 2; // Tendency to move up
      if (Top > PlayingField.FieldCenterY - 100) // Top Outside lineman should not leave his zone and stay on top 
      {
        ChangeY -= 4;
      }
      if (Top + 32 > TargetPlayer.Top) // Top Outside need to stay above than the ball runner
      {
        ChangeY -= 4;
      }
      if (OffsetY < -8)
        OffsetY++;
      base.Move();
    }
    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      base.CollisionMove(collidedWithPlayer, collisionOrientation);
    }
  }

  public class DefenderOutsideLinemanBottom : DefenderOutsideLineman 
  {
    public override void Initialize()
    {
      base.Initialize();
    }
    public override void Move()
    {
      ChangeY += 1; // Tendency to move down
      if (Top < PlayingField.FieldCenterY + 100) // Bottom Outside lineman should not leave his zone and stay on bottom
      {
        ChangeY += 4;
      }
      if (Top - 32 < TargetPlayer.Top) // Bottom Outside need to stay ;ower than the ball runner
      {
        ChangeY += 4;
      }
      if(OffsetY > 8)
        OffsetY--;
      base.Move();
      //Debug.WriteLine(this.ChangeX);
      //Debug.WriteLine(this.ChangeY);
      //Debug.WriteLine("--------------");
    }
    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      base.CollisionMove(collidedWithPlayer, collisionOrientation);
    }
  }

  public class DefenderOutsideLineman : Defender
  {
    public override void Initialize()
    {
      SpeedCap = 106;
      Intelligence = 8;
      TargetPlayer = Player.ControllablePlayer;
      DefensiveMode = DefensiveMode.Blitz;
      base.Initialize();
    }

    public override void Move()
    {
      int calculatedTargetY = TargetPlayer.Top;

      if (TargetPlayer != Player.ControllablePlayer)
        TargetPlayer = Player.ControllablePlayer;

      if (Player.ControllablePlayer.Left > PlayingField.LineOfScrimagePixel + 8)
      {
        Intelligence = 11; // Once the runner get past the line of scrimage, this defender doen't have to worry about the blockers.
      }

      if (Intelligence > Random.Next(0,15) || MovingAroundBlocker > 0)
      {
        if(Random.Next(0, 15) > 8)
        {
          if (Player.DetectCloseCollision(this, TargetPlayer, 90))
          {
            calculatedTargetY = TargetPlayer.Top;
          }
          else
          {
            calculatedTargetY = TargetPlayer.Top + OffsetY;
          }
        }
        int calcTargetX = AI_BasicMoveTowardsTargetX();
        base.MoveTowardsTarget(calcTargetX, calculatedTargetY);
      }
      base.Move();
    }

    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      if (collidedWithPlayer.HasBall && !IsThrowingOrKicking)
      {
        if (Random.Next(0, 10) > 1) // Allow a missed tackle 10% of time.
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
