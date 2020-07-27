using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  public class OffenderWideReceiverTop : OffenderWideReceiver
  {
    public override void Initialize()
    {
      PicBox.MouseClick += new System.Windows.Forms.MouseEventHandler(MouseClick); //TODO put so it is initialized one time
      base.Initialize();
    }
    private void MouseClick(object sender, MouseEventArgs e)
    {
      MouseEventArgs mouseEventArgs = new MouseEventArgs(MouseButtons.Left, 1, Left + 16, Top + 16, 0);
      ParentGame.MouseClick(sender, mouseEventArgs);
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
  public class OffenderWideReceiverBottom : OffenderWideReceiver
  {
    public override void Initialize()
    {
      PicBox.MouseClick += new System.Windows.Forms.MouseEventHandler(MouseClick); //TODO put so it is initialized one time
      base.Initialize();
    }
    private void MouseClick(object sender, MouseEventArgs e)
    {
      MouseEventArgs mouseEventArgs = new MouseEventArgs(MouseButtons.Left, 1, Left + 16, Top + 16, 0);
      ParentGame.MouseClick(sender, mouseEventArgs);
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


  public class OffenderWideReceiver : Offender
  {
    private List<ReceiverPattern> receiverPatterns = new List<ReceiverPattern>();
    private int receiverPatternIndex;
    private bool runningPattern;
    private Player target = new Player();

    //public enum PatternEnum
    //{
    //  ButtonHookPattern,
    //  FlyPattern,
    //  PostPattern,
    //  SlantPattern,
    //  QuickOutPattern
    //}

    public override void Initialize()
    {
      SpeedCap = 108;
      HasBall = false;
      PicBox.Image = ParentForm.Player1.Image;
      receiverPatternIndex = 0;
      TargetPlayer = target;
      if(receiverPatterns.Count > 0)
      {
        target.Top = receiverPatterns[0].TargetY;
        target.Left = receiverPatterns[0].TargetX;
      }
      runningPattern = true;

      base.Initialize();
    }

    public override void Move()
    {
      
      if(runningPattern == true)
      {
        if (Player.DetectCloseCollision(this, target, 40))
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
            ParentGame.EndPlay(EndPlayType.Incomplete, null, "Dropped.");
          }
          else
          { // Caught, run with ball.
            SpeedCap = 102; // run a slight bit slower with ball.
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

      ReceiverPattern receiverPattern = new ReceiverPattern() { Name = "ButtonHook", TargetX = 420, TargetY = InitialTop};
      receiverPatterns.Add(receiverPattern);
      receiverPattern = new ReceiverPattern() { Name = "ButtonHook", TargetX = 250, TargetY = InitialTop};
      receiverPatterns.Add(receiverPattern);
      receiverPattern = new ReceiverPattern() { Name = "ButtonHook", TargetX = 250, TargetY = InitialTop + 200};
      receiverPatterns.Add(receiverPattern);
    }

    public void FlyPattern()
    {
      receiverPatterns.Clear();

      ReceiverPattern receiverPattern = new ReceiverPattern() { Name = "Fly", TargetX = 900, TargetY = this.InitialTop };
      receiverPatterns.Add(receiverPattern);
      receiverPattern = new ReceiverPattern() { Name = "Fly", TargetX = 900, TargetY = 500 };
      receiverPatterns.Add(receiverPattern);
      receiverPattern = new ReceiverPattern() { Name = "Fly", TargetX = 200, TargetY = 500 };
      receiverPatterns.Add(receiverPattern);
    }

    public void PostPatternTop()
    {
      receiverPatterns.Clear();

      ReceiverPattern receiverPattern = new ReceiverPattern() { Name = "Post", TargetX = 400, TargetY = this.InitialTop };
      receiverPatterns.Add(receiverPattern);
      receiverPattern = new ReceiverPattern() { Name = "Post", TargetX = 1000, TargetY = InitialTop + 600 };
      receiverPatterns.Add(receiverPattern);
      receiverPattern = new ReceiverPattern() { Name = "Post", TargetX = 200, TargetY = InitialTop + 600 };
      receiverPatterns.Add(receiverPattern);
    }
    public void PostPatternBottom()
    {
      receiverPatterns.Clear();

      ReceiverPattern receiverPattern = new ReceiverPattern() { Name = "Post", TargetX = 400, TargetY = this.InitialTop };
      receiverPatterns.Add(receiverPattern);
      receiverPattern = new ReceiverPattern() { Name = "Post", TargetX = 1000, TargetY = InitialTop - 600 };
      receiverPatterns.Add(receiverPattern);
      receiverPattern = new ReceiverPattern() { Name = "Post", TargetX = 200, TargetY = InitialTop - 600 };
      receiverPatterns.Add(receiverPattern);
    }

    public void SlantPatternTop()
    {
      receiverPatterns.Clear();

      ReceiverPattern receiverPattern = new ReceiverPattern() { Name = "Slant", TargetX = PlayingField.LineOfScrimagePixel + 50, TargetY = this.InitialTop };
      receiverPatterns.Add(receiverPattern);
      receiverPattern = new ReceiverPattern() { Name = "Slant", TargetX = 600, TargetY = InitialTop + 700 };
      receiverPatterns.Add(receiverPattern);
      receiverPattern = new ReceiverPattern() { Name = "Slant", TargetX = 200, TargetY = InitialTop + 700 };
      receiverPatterns.Add(receiverPattern);
    }
    public void SlantPatternBottom()
    {
      receiverPatterns.Clear();

      ReceiverPattern receiverPattern = new ReceiverPattern() { Name = "Slant", TargetX = PlayingField.LineOfScrimagePixel + 50, TargetY = this.InitialTop };
      receiverPatterns.Add(receiverPattern);
      receiverPattern = new ReceiverPattern() { Name = "Slant", TargetX = 600, TargetY = InitialTop - 700 };
      receiverPatterns.Add(receiverPattern);
      receiverPattern = new ReceiverPattern() { Name = "Slant", TargetX = 200, TargetY = InitialTop - 700 };
      receiverPatterns.Add(receiverPattern);
    }

    public void QuickOutPatternTop()
    {
      receiverPatterns.Clear();

      ReceiverPattern receiverPattern = new ReceiverPattern() { Name = "QuickOut", TargetX = PlayingField.LineOfScrimagePixel + 40, TargetY = this.InitialTop + 20 };
      receiverPatterns.Add(receiverPattern);
      receiverPattern = new ReceiverPattern() { Name = "QuickOut", TargetX = PlayingField.LineOfScrimagePixel + 80, TargetY = InitialTop - 100 };
      receiverPatterns.Add(receiverPattern);
      receiverPattern = new ReceiverPattern() { Name = "QuickOut", TargetX = PlayingField.LineOfScrimagePixel + 80, TargetY = InitialTop - 40};
      receiverPatterns.Add(receiverPattern);
    }
    public void QuickOutPatternBottom()
    {
      receiverPatterns.Clear();

      ReceiverPattern receiverPattern = new ReceiverPattern() { Name = "QuickOut", TargetX = PlayingField.LineOfScrimagePixel + 40, TargetY = this.InitialTop - 20 };
      receiverPatterns.Add(receiverPattern);
      receiverPattern = new ReceiverPattern() { Name = "QuickOut", TargetX = PlayingField.LineOfScrimagePixel + 80, TargetY = InitialTop + 100 };
      receiverPatterns.Add(receiverPattern);
      receiverPattern = new ReceiverPattern() { Name = "QuickOut", TargetX = PlayingField.LineOfScrimagePixel + 80, TargetY = InitialTop + 40 };
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
