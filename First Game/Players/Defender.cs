using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  public class Defender : Player
  {
    public int Intelligence = 10;
    public DefensiveMode DefensiveMode = DefensiveMode.Normal;
    public bool CanStillIntercept = false;

    public override void Initialize()
    {
      Team = 2; // TODO, needed?
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

      int r = Random.Next(0, 20) - 10;
      if(Math.Abs(r) < 5) // Try to move around more. Give it a second try to be greater than -4 or 4
        r = Random.Next(0, 20) - 10;
      if (MovingAroundBlocker == 0)
      {
        switch (collisionOrientation)
        {
          case CollisionOrientation.Above:
          case CollisionOrientation.Below:
            if (TargetPlayer.Left < Left - 90 && ChangeX < -40) // The target is way to the left and this is moving left, keep moving left
              ChangeX -= 8;
            else if (TargetPlayer.Left > Left - 10 && ChangeX > 20) // The target is to the right and this is moving right, keep moving right
              ChangeX += 8;
            else // Else, randomly go left or right.
              ChangeX = 8 * r;
            break;
          case CollisionOrientation.ToLeft:
          case CollisionOrientation.ToRight:
            if(Math.Abs(OffsetY) > 30) // For defensive linemen
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
        if (Player.DetectCloseCollision(this, TargetPlayer, 60))
          closeToTackle = true; // Move right towards player

      // Go right towards target if close to target or are blitzing
      if (this.DefensiveMode == DefensiveMode.Blitz || closeToTackle)
      {
        targetX = TargetPlayer.Left + (TargetPlayer.ChangeX / 2) + (diffY / 2);
      }
      else if (this.DefensiveMode == DefensiveMode.Normal)
      {
        if (TargetPlayer.Left < PlayingField.LineOfScrimagePixel)
            targetX = TargetPlayer.Left + 30 + (diffY / 2); 
        else
          targetX = TargetPlayer.Left + (TargetPlayer.ChangeX / 2) + (diffY / 2);
      }
      else if (this.DefensiveMode == DefensiveMode.Soft)
      {
        if (TargetPlayer.Left < PlayingField.LineOfScrimagePixel)
            targetX = TargetPlayer.Left + 60;
        else
          targetX = TargetPlayer.Left + (TargetPlayer.ChangeX / 2) + (diffY / 2);
      }

      return targetX;
    }

    protected bool IsInterception(Defender player)
    {
      int normalDeflectionOdds = 60;
      int normalInterceptionOdds = 10;

      if (player.CanStillIntercept)
        player.CanStillIntercept = false;
      else 
        return false; 

      if (player is DefenderMiddleLinebacker)
      {
        normalDeflectionOdds = 50;
        normalInterceptionOdds = 15;
      }
      if (player is DefenderSafety)
      {
        normalDeflectionOdds = 45;
        normalInterceptionOdds = 20;
      }

      int random = Random.Next(0, 100);
      if (random > 100 - normalInterceptionOdds) // 10% - 25% for Safety. Percent of the time the defender intercepts the ball 
      {
        return true;
      }
      else if (random > 100 - normalDeflectionOdds) // 60% - 40% for Safety. Percent of the time the defender deflects the ball 
      {
        BallAsPlayer.BallIsCatchable = false; // Tipped ball, ball is uncatchable
        BallAsPlayer.SpinDefectedBall();
      }
      // 30% ball goes by defender
      return false;
    }
  }
}
