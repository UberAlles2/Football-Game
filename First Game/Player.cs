using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

  //public enum Position
  //{
  //  OffenderQuarterback,
  //  OffenderLinemanTop,
  //  OffenderLinemanMiddle,
  //  OffenderLinemanBottom,
  //  OffenderWideReceiver,
  //  DefenderMiddleLineman,
  //  DefenderOutsideLinemanTop,
  //  DefenderOutsideLinemanBottom,
  //  DefenderMiddleLinebacker,
  //  DefenderCornerback,
  //  BallAsPlayer
  //}

  public class Player
  {
    public Player TargetPlayer;
    public static Form1 ParentForm;
    public static Game ParentGame;
    //public static List<Player> Players = new List<Player>(new Player[Enum.GetValues(typeof(Position)).Cast<int>().Max() + 1]);
    public static List<Player> Players = new List<Player>();
    public static Random Random = new Random();

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
    private int top;
    private int left;
    private int offset;
    private PictureBox pictureBox;

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
    public int Offset { get => offset; set => offset = value; }
    public int InitialOffset { get => initialOffset; set { initialOffset = value; offset = value; } }
    public Player InitialTargetPlayer { get => initialTargetPlayer; set { initialTargetPlayer = value; TargetPlayer = value; } }

    public int TotalMoves;

    public int Top
    {
      get => top;
      set
      {
        top = value;
        CenterY = top + PlayerHeight;
      }
    }
    public int Left
    {
      get => left;
      set
      {
        left = value;
        CenterX = left + PlayerWidth;
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

    //public static Player Get(Position position)
    //{
    //  return Players[(int)position];
    //}
    public static Player Get(Type type)
    {
      Player player = Players.Where(p => p.GetType() == type).FirstOrDefault();
      return player;
    }

    public static void AddPlayer(Player player)
    {
      Players.Add(player);
      //string name = player.GetType().Name;
      //Position positionEnum = (Position)Enum.Parse(typeof(Position), name);
      //Players.RemoveAt((int)positionEnum);
      //Players.Insert((int)positionEnum, player);
    }

    public virtual void Initialize()
    {
      if (InitialTop != -1) Top = InitialTop;
      if (InitialLeft != -1) Left = InitialLeft;
      Offset = InitialOffset;
      if (InitialTargetPlayer != null) TargetPlayer = InitialTargetPlayer;

      Player.MovePic(this);

      this.TotalMoves = 0;
      this.ChangeX = 0;
      this.ChangeY = 0;
    }

    public virtual void Move()
    {
      TotalMoves++;
      CheckFormBoundries();
      MovePic(this);
      Application.DoEvents();
    }
    public static void MovePic(Player player)
    {
      if (Math.Abs(player.ChangeY) > player.SpeedCap)
      {
        player.ChangeY = player.SpeedCap * Math.Sign(player.ChangeY);

        if (Math.Abs(player.ChangeX) > player.SpeedCap - 40)
        {
          player.ChangeX -= 16;
        }
      }
      if (Math.Abs(player.ChangeX) > player.SpeedCap)
      {
        player.ChangeX = player.SpeedCap * Math.Sign(player.ChangeX);
        
        if (Math.Abs(player.ChangeY) > player.SpeedCap - 40)
        {
          player.ChangeY -= 16;
        }
      }

      player.Top = player.Top + player.ChangeY / 32;
      player.Left = player.Left + player.ChangeX / 32;
      player.PicBox.Top = player.Top;
      player.PicBox.Left = player.Left;
    }

    public virtual void MoveTowardsTarget(int Y, int X)
    {
      // Vertical move
      if (Math.Abs(this.Left - X) < Math.Abs(this.Top - Y))
      {
        if (Y < Top)
        {
          if(ChangeY > 30)
            ChangeY += -20;
          else
            ChangeY += -16;
        }
        if (Y > Top)
        {
          if (ChangeY < -30)
            ChangeY += 20;
          else
            ChangeY += 16;
        }
        if (X < this.Left)
        {
          this.ChangeX += -4;
        }
        if (X > this.Left)
        {
          this.ChangeX += 4;
        }

      }
      else // Horizontal Move
      {
        if (Y < this.Top)
        {
          this.ChangeY += -4;
        }
        if (Y > this.Top)
        {
          this.ChangeY += 4;
        }
        if (X < this.Left)
        {
          this.ChangeX += -16;
        }
        if (X > this.Left)
        {
          this.ChangeX += 16;
        }
      }
    }


    public virtual void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      switch (collisionOrientation)
      {
        case CollisionOrientation.Above:
          this.Top -= 3;
          this.ChangeY = -12;
          break;
        case CollisionOrientation.Below:
          this.Top += 3;
          this.ChangeY = 12;
          break;
        case CollisionOrientation.ToLeft:
          this.Left -= 3;
          this.ChangeX = 12;
          break;
        case CollisionOrientation.ToRight:
          this.Left += 3;
          this.ChangeX = -12;
          break;
      }
    }

    public virtual void MoveAroundPlayer(CollisionOrientation collisionOrientation)
    {

    }

    private void CheckFormBoundries()
    {
      if (IsBall)
        return;

      if (this.Left < 0)
      {
        if (this is Defender)
          this.ChangeX = 30;
        else
          this.ChangeX = 0;

        this.Left = 1;
      }
      if (this.Left > ParentForm.Width - this.PicBox.Width - 10)
      {
        if (this is Defender)
          this.ChangeX = 0;
        else
          this.ChangeX = 0;

        this.Left = ParentForm.Width - this.PicBox.Width - 10 - 1;
      }
      if (this.Top < 0)
      {
        if (this is Defender)
          this.ChangeY = 60;
        else
          this.ChangeY = 0;

        this.Top = 1;
      }
      if (this.Top > ParentForm.Height - this.PicBox.Height - 35)
      {
        if (this is Defender)
          this.ChangeY = -60;
        else
          this.ChangeY = 0;

        this.Top = ParentForm.Height - this.PicBox.Height - 35 - 1;
      }
    }
  }
}
