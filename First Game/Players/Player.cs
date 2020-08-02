using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace FootballGame
{
  public enum DefensiveMode
  {
    Blitz,
    Normal,
    Soft
  }

  public enum VerticalPosition
  {
    PositionTop,
    PositionMiddle,
    PositionBottom
  }

  public class Player
  {
    public enum ThrowType
    {
      Throw,
      Punt,
      FieldGoal
    }

    public static Player ControllablePlayer = new Player();
    public static Random Random = new Random();
    public static Form1 ParentForm;
    public static Game ParentGame;
    public static Rectangle FieldBounds;
    //public static List<Player> Players = new List<Player>(new Player[Enum.GetValues(typeof(Position)).Cast<int>().Max() + 1]);
    public static List<Player> Players = new List<Player>();
    public static bool IsThrowingOrKicking;
    public static ThrowType ThrowingType;

    private int changeX;
    private int changeY;
    private int playerWidth;
    private int playerHeight;
    private int cap = 2;
    private int team;
    private int centerX;
    private int centerY;
    private bool hasBall = false;
    private bool isBall = false;
    private int movingAroundBlocker = 0;
    private int initialTop = -1;
    private int initialLeft = -1;
    private int initialOffset = 0;
    private Player initialTargetPlayer;
    private VerticalPosition position;
    private int top;
    private int left;
    private int offset;
    private PictureBox pictureBox;

    public Player TargetPlayer;
    public int TotalMoves;

    public int SpeedCap { get => cap; set => cap = value; }
    public int Team { get => team; set => team = value; }
    public int PlayerWidth { get => playerWidth; set => playerWidth = value; }
    public int PlayerHeight { get => playerHeight; set => playerHeight = value; }
    public int ChangeX { get => changeX; set => changeX = value; }
    public int ChangeY { get => changeY; set => changeY = value; }
    public int CenterX { get => centerX; set => centerX = value; }
    public int CenterY { get => centerY; set => centerY = value; }
    public bool HasBall { get => hasBall; set => hasBall = value; }
    public bool IsBall { get => isBall; set => isBall = value; }
    public int MovingAroundBlocker { get => movingAroundBlocker; set => movingAroundBlocker = value; }
    public int InitialTop { get => initialTop; set => initialTop = value; }
    public int InitialLeft { get => initialLeft; set => initialLeft = value; }
    public int OffsetY { get => offset; set => offset = value; }
    public int InitialOffsetY { get => initialOffset; set { initialOffset = value; offset = value; } }
    public Player InitialTargetPlayer { get => initialTargetPlayer; set { initialTargetPlayer = value; TargetPlayer = value; } }
    public VerticalPosition VerticalPosition { get => position; set => position = value; }



    public int Top
    {
      get => top;
      set
      {
        top = value;
        CenterY = top + (PlayerHeight/2);
      }
    }
    public int Left
    {
      get => left;
      set
      {
        left = value;
        CenterX = left + (PlayerWidth/2);
      }
    }

    public PictureBox PicBox
    {
      get { return pictureBox; }
      set
      {
        pictureBox = value;
        PlayerWidth = pictureBox.Width;
        PlayerHeight = pictureBox.Height;
      }
    }

    public Player()
    {
       
    }

    //-------------- Events
    public virtual void MouseClick(object sender, MouseEventArgs e)
    {
      MouseEventArgs mouseEventArgs = new MouseEventArgs(MouseButtons.Left, 1, Left + 16, Top + 16, 0);
      ParentGame.MouseClick(sender, mouseEventArgs, this);
    }

    public static Player Get(Type type)
    {
      Player player = Players.Where(p => p.GetType() == type).FirstOrDefault();
      return player;
    }

    public static void AddPlayer(Player player)
    {
      Players.Add(player);
    }
    public static void AddPlayer(Player player, int initialLeft, int initialTop, PictureBox pictureBox, VerticalPosition verticalPosition = VerticalPosition.PositionMiddle, int initialOffsetY = 0, DefensiveMode defensiveMode = DefensiveMode.Normal)
    {
      player.InitialLeft = initialLeft;
      player.InitialTop = initialTop;
      player.PicBox = AddPlayerPictureBox(pictureBox);
      player.PicBox.BringToFront();
      player.PicBox.MouseClick += new System.Windows.Forms.MouseEventHandler(player.MouseClick); 

      player.InitialOffsetY = initialOffsetY;
      if(player is Defender) (player as Defender).DefensiveMode = defensiveMode;
      Players.Add(player);
    }

    public static PictureBox AddPlayerPictureBox(PictureBox pb)
    {
      PictureBox p = new PictureBox();
      p.Height = pb.Height;
      p.Width = pb.Width;
      p.SizeMode = pb.SizeMode;
      p.Image = pb.Image;
      ParentForm.Controls.Add(p);
      return p;
    }


    public virtual void Initialize()
    {
      if (InitialTop != -1) Top = InitialTop;
      if (InitialLeft != -1) Left = InitialLeft;
      if (InitialTargetPlayer != null) TargetPlayer = InitialTargetPlayer;
      OffsetY = InitialOffsetY;

      Player.MovePic(this); // Place player at initial position

      this.TotalMoves = 0;
      this.ChangeX = 0;
      this.ChangeY = 0;
    }

    public virtual void Move()
    {
      TotalMoves++;
      CheckFieldBoundries();
      CheckForTouchdownOrSafety();
      MovePic(this);
      Application.DoEvents();
      ParentForm.picEndZoneLeft.Invalidate();
    }
    public static void MovePic(Player player)
    {
      if (Math.Abs(player.ChangeY) + Math.Abs(player.ChangeX) > player.SpeedCap * 1.5)
      {
        player.ChangeY = player.ChangeY - Math.Sign(player.ChangeY) * 10;
        player.ChangeX = player.ChangeX - Math.Sign(player.ChangeX) * 10;
      }

      if (Math.Abs(player.ChangeY) > player.SpeedCap)
      {
        player.ChangeY = (player.SpeedCap - 12) * Math.Sign(player.ChangeY);
      }
      if (Math.Abs(player.ChangeX) > player.SpeedCap)
      {
        player.ChangeX = (player.SpeedCap -12) * Math.Sign(player.ChangeX);
      }

      // Debug TODO
      //if (player.Top < 109)
      //  player.Top = player.Top;

      player.Top = player.Top + player.ChangeY / 32;
      player.Left = player.Left + player.ChangeX / 32;
      player.PicBox.Top = player.Top;
      player.PicBox.Left = player.Left;
    }

    public virtual void MoveTowardsTarget(int X, int Y)
    {
      int closingIn = 16;

      if (this is DefenderLineman || this is DefenderOutsideLineman) // Can't close in on target that fast with blockers
        closingIn = 12;

      // Vertical move, the target player if either way above or below chasing player, Y should change more 
      if (Math.Abs(this.Left - X) < Math.Abs(this.Top - Y))
      {
        if (Y < Top) // Target is above
        {
          if(ChangeY > 30) // Player is moving down, reverse
            ChangeY -= (closingIn + 4); 
          else
            ChangeY -= (closingIn - 4);
        }
        if (Y > Top) // Target is below
        {
          if (ChangeY < -30) // Player is moving up, reverse
            ChangeY += (closingIn + 4); 
          else
            ChangeY += (closingIn - 4); 
        }
        if (X < this.Left)
        {
          if (ChangeX > 30)
            ChangeX -= closingIn;
          else
            ChangeX -= (closingIn - 4); 
        }
        if (X > this.Left)
        {
          if (ChangeX < -30)
            ChangeX += closingIn; 
          else
            ChangeX += (closingIn - 4); 
        }
      }
      else // Horizontal move, the target player if either way right or left of the chasing player, X should change more 
      {
        if (Y < Top)
        {
          if (ChangeY > 30)
            ChangeY -= closingIn;
          else
            ChangeY -= (closingIn - 4);
        }
        if (Y > Top)
        {
          if (ChangeY < -30)
            ChangeY += closingIn;
          else
            ChangeY += (closingIn - 4);
        }
        if (X < this.Left)
        {
          if (ChangeX > 30)
            ChangeX -= (closingIn + 4);
          else
            ChangeX -= (closingIn - 4);
        }
        if (X > this.Left)
        {
          if (ChangeX < -30)
            ChangeX += (closingIn + 4);
          else
            ChangeX += (closingIn - 4);
        }
      }
    }


    public virtual void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      switch (collisionOrientation)
      {
        case CollisionOrientation.Above:
          this.Top -= 3;
          this.ChangeY = -10;
          break;
        case CollisionOrientation.Below:
          this.Top += 3;
          this.ChangeY = 10;
          break;
        case CollisionOrientation.ToLeft:
          this.Left -= 3;
          this.ChangeX = -10;
          break;
        case CollisionOrientation.ToRight:
          this.Left += 3;
          this.ChangeX = 10;
          break;
      }
    }

    public virtual void MoveAroundPlayer(CollisionOrientation collisionOrientation)
    {

    }

    private void CheckFieldBoundries()
    {
      if (IsBall)
        return;

      if (this.Left < FieldBounds.X)
      {
        if (this is Defender)
          this.ChangeX = 30;
        else
          this.ChangeX = 0;

        this.Left = FieldBounds.X + 1;
      }
      if (this.Left > FieldBounds.Width)
      {
        if (this is Defender)
          this.ChangeX = 0;
        else if (this.HasBall)
        {
          this.Left += Random.Next(60, 360);
          ParentGame.EndPlay(EndPlayType.Tackled, null, "Tackled after extra yards.");
          return;
        }
        else
          this.ChangeX = 0;

        this.Left = FieldBounds.Width - 1;
      }
      if (this.Top < FieldBounds.Y)
      {
        if (this is Defender)
          this.ChangeY = 60;
        else if (this.HasBall)
        {
          ParentGame.EndPlay(EndPlayType.OutOfBounds, null, "Out of Bounds.");
          return;
        }
        else
          this.ChangeY = 0;


        this.Top = FieldBounds.Y + 1;
      }
      if (this.Top > FieldBounds.Height)
      {
        if (this is Defender)
          this.ChangeY = -60;
        else if (this.HasBall)
        {
          ParentGame.EndPlay(EndPlayType.OutOfBounds, null, "Out of Bounds.");
          return;
        }
        else
          this.ChangeY = 0;

        this.Top = FieldBounds.Height - 1;
      }
    }

    private void CheckForTouchdownOrSafety()
    {
      if (this != ControllablePlayer)
        return;
      
      if (this.Left + 28 > PlayingField.PixelFromYard(100)) // 28 is tip of ball crossing, not Left
        ParentGame.EndPlay(EndPlayType.Touchdown, this, "Touchdown!");
    }

    public static void CheckCollisions()
    {
      for (int i = 0; i < Player.Players.Count - 1; i++)
      {
        for (int j = i + 1; j < Player.Players.Count; j++)
        {
          if (!Player.IsThrowingOrKicking && Players[j].IsBall)
            break;

          // If check for ball collision, the below positions are the only one who can catch the ball
          // any other positions willl not be checked and thus the break;
          if (!(Players[i] is OffenderWideReceiver)
           && !(Players[i] is DefenderCornerback)
           && !(Players[i] is DefenderMiddleLinebacker)
           && !(Players[i] is DefenderSafety)
           && Players[j].IsBall)
            break;


          //if (players[j].IsBall && (players[i] is DefenderCornerback) && ballAsPlayer.BallIsCatchable) // TODO take out
          //  players[j].IsBall = players[j].IsBall;

          // If player is hitting another player
          if (DetectCollision(Players[i], Players[j]))
          {
            // Hitting above or below another player
            if (Math.Abs(Players[i].Left - Players[j].Left) < Math.Abs(Players[i].Top - Players[j].Top))
            {
              //  | |
              //
              //  | |
              if (Players[i].Top < Players[j].Top)
              {
                Players[j].CollisionMove(Players[i], CollisionOrientation.Below);
                Players[i].CollisionMove(Players[j], CollisionOrientation.Above);
              }
              else
              {
                Players[j].CollisionMove(Players[i], CollisionOrientation.Above);
                Players[i].CollisionMove(Players[j], CollisionOrientation.Below);
              }
            }
            else // Hitting to the left or right of another player
            {
              //  | |   | |
              if (Players[i].Left < Players[j].Left)
              {
                Players[j].CollisionMove(Players[i], CollisionOrientation.ToRight);
                Players[i].CollisionMove(Players[j], CollisionOrientation.ToLeft);
              }
              else
              {
                Players[j].CollisionMove(Players[i], CollisionOrientation.ToLeft);
                Players[i].CollisionMove(Players[j], CollisionOrientation.ToRight);
              }
            }
            Player.MovePic(Players[j]);
            Player.MovePic(Players[i]);
          }
        }
      }
    }

    public static bool DetectCollision(Player player1, Player player2)
    {
      return Math.Abs(player1.Left - player2.Left) < player1.PlayerWidth - 1 && Math.Abs(player1.Top - player2.Top) < player1.PlayerHeight - 1;
    }
    public static bool DetectCloseCollision(Player player1, Player player2, int howClose)
    {
      return Math.Abs(player1.Left - player2.Left - 1) < howClose && Math.Abs(player1.Top - player2.Top - 1) < howClose;
    }
    public static bool DetectCloseCollision(Player player, int X, int Y, int howClose)
    {
      return Math.Abs(player.Left - player.Left - 1) < howClose && Math.Abs(player.Top - Y - 1) < howClose;
    }
  }
}
