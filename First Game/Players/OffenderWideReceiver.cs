using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  class OffenderWideReceiverTop : OffenderWideReceiver
  {
    public override void Initialize()
    {
      base.Initialize();
    }
    public override void Move()
    {
      base.Move();
    }
    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      base.CollisionMove(collidedWithPlayer, collisionOrientation);
    }
  }
  class OffenderWideReceiverBottom : OffenderWideReceiver { }

  class OffenderWideReceiver : Offender
  {
    private List<ReceiverPattern> receiverPatterns = new List<ReceiverPattern>();
    private int receiverPatternIndex;
    private bool runningPattern;
    private Player target = new Player();

    public override void Initialize()
    {
      SpeedCap = 108;
      HasBall = false;
      PicBox.Image = ParentForm.Player1.Image;
      PicBox.MouseClick += new System.Windows.Forms.MouseEventHandler(MouseClick);
      receiverPatternIndex = 0;
      TargetPlayer = target;
      target.Top = receiverPatterns[0].TargetY;
      target.Left = receiverPatterns[0].TargetX;
      runningPattern = true;

      base.Initialize();
    }

    private void MouseClick(object sender, MouseEventArgs e)
    {
      MouseEventArgs mouseEventArgs = new MouseEventArgs(MouseButtons.Left, 1, Left + 16, Top + 16, 0); 
      ParentGame.MouseClick(sender, mouseEventArgs);
    }

    public override void Move()
    {
      
      if(runningPattern == true)
      {
        if (Game.DetectCloseCollision(this, target, 40))
        {
          target.Top = receiverPatterns[receiverPatternIndex].TargetY;
          target.Left = receiverPatterns[receiverPatternIndex].TargetX;

          if (receiverPatternIndex < receiverPatterns.Count - 1)
            receiverPatternIndex++;
          else
            receiverPatternIndex = 0; // loop through again
        }
        if (IsThrowing && Random.Next(0, 10) > 7) // Player will move towards thrown ball
        {
          target.Top  = (target.Top + Game.ballAsPlayer.TargetPlayer.Top) / 2;
          target.Left = (target.Left + Game.ballAsPlayer.TargetPlayer.Left) / 2;
        }

        base.MoveTowardsTarget(target.Left, target.Top);
      }

      base.Move();
    }

    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      if(collidedWithPlayer is BallAsPlayer)
      {
        if(BallAsPlayer.BallIsCatchable)
        {
          if (Random.Next(0, 10) > 7)
          {
            BallAsPlayer.SpinDefectedBall();
            ParentGame.EndPlay(EndPlayType.Incomplete, "Dropped");
          }
          else
          { // Caught, run with ball.
            ControllablePlayer.HasBall = false;
            HasBall = true;
            PicBox.Image = ParentForm.picBearsBall.Image;

            collidedWithPlayer.Left = -999;

            IsThrowing = false;
            runningPattern = false;

            ControllablePlayer = this;
            ControllablePlayer.HasBall = true;
          }
        }
      }
      base.CollisionMove(collidedWithPlayer, collisionOrientation);
    }

    public void ButtonHookPattern()
    {
      receiverPatterns.Clear();

      ReceiverPattern receiverPattern = new ReceiverPattern() { Name = "ButtonHook", TargetX = 500, TargetY = InitialTop};
      receiverPatterns.Add(receiverPattern);
      receiverPattern = new ReceiverPattern() { Name = "ButtonHook", TargetX = 250, TargetY = InitialTop};
      receiverPatterns.Add(receiverPattern);
      receiverPattern = new ReceiverPattern() { Name = "ButtonHook", TargetX = 250, TargetY = InitialTop + 200};
      receiverPatterns.Add(receiverPattern);
    }

    public void TheBomb()
    {
      receiverPatterns.Clear();

      ReceiverPattern receiverPattern = new ReceiverPattern() { Name = "TheBomb", TargetX = 900, TargetY = this.InitialTop };
      receiverPatterns.Add(receiverPattern);
      receiverPattern = new ReceiverPattern() { Name = "TheBomb", TargetX = 900, TargetY = 500 };
      receiverPatterns.Add(receiverPattern);
      receiverPattern = new ReceiverPattern() { Name = "TheBomb", TargetX = 200, TargetY = 500 };
      receiverPatterns.Add(receiverPattern);
    }

  }

  class ReceiverPattern
  {
    public string Name;
    public int TargetX;
    public int TargetY;
  }
}
