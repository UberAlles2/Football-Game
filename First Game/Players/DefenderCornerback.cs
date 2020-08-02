using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  public class DefenderCornerbackTop : DefenderCornerback 
  {
    public override void Move()
    {
      if (Top > PlayingField.FieldCenterY - 120) // Top man should not leave his zone and stay on top 
      {
        ChangeY -= 6;
      }
      base.Move();
    }
  }
  public class DefenderCornerbackBottom : DefenderCornerback 
  {
    public override void Move()
    {
      if (Top < PlayingField.FieldCenterY + 120) // Bottom man should not leave his zone and stay on bottom 
      {
        ChangeY += 6;
      }
      base.Move();
    }
  }

  public class DefenderCornerback : Defender
  {
    private bool InCoverage = true;
    

    public override void Initialize()
    {
      SpeedCap = 132;
      Intelligence = 6;
      InCoverage = true;
      base.Initialize();
    }

    public override void Move()
    {
      // Running back / quarterback has cross the scrimmage line. Change the target to the running back
      if (ControllablePlayer.Left > PlayingField.LineOfScrimagePixel && InCoverage == true)
      {
        TargetPlayer = ControllablePlayer;
        ChangeX += 20;
        base.MoveTowardsTarget(TargetPlayer.Left + 160, TargetPlayer.Top);
        InCoverage = false;
      }

      if (this.Intelligence > Random.Next(0,15) || MovingAroundBlocker > 0)
      {
        int calcTargetX = AI_BasicMoveTowardsTargetX();

        if (InCoverage)
        {
          if (TargetPlayer.Top < PlayingField.FieldCenterY)
            base.MoveTowardsTarget(calcTargetX + (Random.Next(0, 340) - 140), TargetPlayer.Top + 40);
          else
            base.MoveTowardsTarget(calcTargetX + (Random.Next(0, 340) - 140), TargetPlayer.Top - 40);

          if (IsThrowingOrKicking && Random.Next(0, 10) > 9) // Player will move towards thrown ball
          {
            TargetPlayer = Game.ballAsPlayer.TargetPlayer; // Where the ball is being thrown.
          }
        }
        else
        {
          Intelligence = 11;
          base.MoveTowardsTarget(calcTargetX, TargetPlayer.Top);
        }
      }

      base.Move();
    }

    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      if (collidedWithPlayer is BallAsPlayer)
      {
        if (BallAsPlayer.BallIsCatchable == false)
          return;
        
        int random = Random.Next(0, 10);
        if (random < 9) 
        {
          BallAsPlayer.BallIsCatchable = false; // Tipped ball, ball is uncatchable
          BallAsPlayer.SpinDefectedBall();
        }
        else
        {
          PicBox.BackColor = System.Drawing.Color.Yellow; // TODO
          ParentGame.EndPlay(EndPlayType.Intercepted, this,  "Intercepted.");
          return;
        }
      }

      if (collidedWithPlayer.HasBall && !IsThrowingOrKicking)
      {
        ParentGame.EndPlay(EndPlayType.Tackled, this, "Tackled.");
        return;
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
