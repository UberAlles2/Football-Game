using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  class OffenderWideReceiverTop : OffenderWideReceiver { }
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
        base.MoveTowardsTarget(target.Top, target.Left);
      }

      base.Move();
    }

    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      if(collidedWithPlayer is BallAsPlayer)
      {
        IsThrowing = false;
        if (Random.Next(0,10) > 7)
          ParentGame.EndPlay("Dropped");
        else
        {
          collidedWithPlayer.Left = -999;
          Game.ControllablePlayer.HasBall = false;
          HasBall = true;
          PicBox.Image = ParentForm.picBearsBall.Image;
          runningPattern = false;
          Game.ControllablePlayer = this;
        }
      }
      base.CollisionMove(collidedWithPlayer, collisionOrientation);
    }

    public void ButtonHookPattern()
    {
      receiverPatterns.Clear();

      ReceiverPattern receiverPattern = new ReceiverPattern() { Name = "ButtonHook", TargetX = 500, TargetY = this.InitialTop};
      receiverPatterns.Add(receiverPattern);
      receiverPattern = new ReceiverPattern() { Name = "ButtonHook", TargetX = 150, TargetY = this.InitialTop};
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
