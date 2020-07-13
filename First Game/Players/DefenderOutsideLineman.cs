using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  class DefenderOutsideLinemanTop : DefenderOutsideLineman 
  {
    public override void Initialize()
    {
      base.Initialize();
    }
    public override void Move()
    {
      
      Offset++;
      ChangeY -= 1; // Tendency to move up
      base.Move();
    }
    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      base.CollisionMove(collidedWithPlayer, collisionOrientation);
    }
  }

  class DefenderOutsideLinemanBottom : DefenderOutsideLineman 
  {
    public override void Initialize()
    {
      base.Initialize();
    }
    public override void Move()
    {
      Offset--;
      ChangeY += 1; // Tendency to move down
      base.Move(); 
    }
    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      base.CollisionMove(collidedWithPlayer, collisionOrientation);
    }
  }

  class DefenderOutsideLineman : Defender
  {
    //public Defender CoDefender;

    public override void Initialize()
    {
      SpeedCap = 105;
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

      if (Intelligence > Random.Next(0,15) || MovingAroundBlocker > 0)
      {
        if(Random.Next(0, 15) > 8)
        {
          //if (Position == Position.Top && Offset < -43)
          //{
          //  ChangeY -= 4; // Keep on top
          //  if (Top > Game.FieldCenterY - 80) // Top Outside lineman should not leave his zone and stay on top 
          //  {
          //    ChangeY -= 6;
          //  }
          //  Offset++;
          //  Offset++;
          //}
          //if (Position == Position.Bottom && Offset > 43)
          //{
          //  ChangeY += 4; // Keep on bottom
          //  if (Top < Game.FieldCenterY + 80) // Bottom Outside lineman should not leave his zone and stay on bottom 
          //  {
          //    ChangeY += 6;
          //  }
          //  Offset--;
          //}
          //if (Math.Abs(Offset) < 150)
          //  calculatedTargetY = calculatedTargetY; // TODOne


          if (Game.DetectCloseCollision(this, TargetPlayer, 90))
          {
            calculatedTargetY = TargetPlayer.Top;
          }
          else
          {
            calculatedTargetY = TargetPlayer.Top + Offset;
          }
        }
        int calcTargetX = AI_BasicMoveTowardsTargetX();
        base.MoveTowardsTarget(calcTargetX, calculatedTargetY);
      }
      base.Move();
    }

    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      if (collidedWithPlayer.HasBall && !IsThrowing)
      {
        if (Random.Next(0, 10) > 1) // Allow a missed tackle 10% of time.
          ParentGame.EndPlay(EndPlayType.Tackled, "Tackled by Outside Lineman.");
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
