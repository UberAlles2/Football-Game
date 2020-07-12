using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Drake.Tools;

/* To Do List
 * Middle linebacker isn't intercepting and coliiding with the ball
 * Add ball on indicator if in opposing territory
 * 
 * 
 * 
 * 
 * 
 * 
 */
 namespace FootballGame
{
  public enum CollisionOrientation
  {
    Above,
    Below,
    ToLeft,
    ToRight
  }

  public enum EndPlayType
  {
    Tackled,
    OutOfBounds,
    Incomplete,
    Intercepted
  }

  public class Game
  {
    private static float PixalsInYard = 32;
    private static float lineOfScrimage = 20;
    private static float yardsGained = 0;
    private static float yardsToGo = 10;
    private static float down = 1;

    private static bool running = true;
    private static System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

    public static Form1 ParentForm;
    public static BallAsPlayer ballAsPlayer = new BallAsPlayer();
    public static Rectangle FieldBounds;
    public static int FieldHeight;
    public static int FieldCenterY;
    public static int LineOfScrimage = 200;
    public static Random Random = new Random();
    public static bool PlayEnded = false;

    public Game(Form1 form1)
    {
      // Set Parents
      ParentForm = form1;
      Player.ParentForm = form1;
      Player.ParentGame = this;
      Scoreboard.ParentForm = form1;
      lineOfScrimage = 20; // 1 - 100;
      yardsToGo = 10;

      ParentForm.pnlPlayOptions.Visible = false;

      Scoreboard.InitializeDrawing(); // Draw the starting scoreboard

      // Initialize field dimensions
      FieldBounds = new Rectangle(0, ParentForm.pnlScoreboard.Height + 30, ParentForm.Width - ParentForm.Player1.Width, ParentForm.Height - ParentForm.pnlScoreboard.Height - 36);
      Player.FieldBounds = FieldBounds;
      FieldCenterY = (FieldBounds.Height / 2) + ParentForm.lblTopSideline.Height + 2; // Players go out of bounds when their botton goes out.

      AddPlayers();

      timer.Tick += new System.EventHandler(KeyDown);
      timer.Interval = 50;
    }

    public void AddPlayers()
    {
      //--------------------- Offensive Players
      OffenderQuarterback offenderQuarterback = new OffenderQuarterback();
      offenderQuarterback.InitialTop = FieldCenterY;
      offenderQuarterback.InitialLeft = 10;
      offenderQuarterback.PicBox = AddPlayerPictureBox(ParentForm.Player1);
      offenderQuarterback.Initialize();
      Player.AddPlayer(offenderQuarterback);
      Player.ControllablePlayer = offenderQuarterback; 

      OffenderLinemanMiddle offenderLinemanMiddle = new OffenderLinemanMiddle();
      offenderLinemanMiddle.VerticalPosition = VerticalPosition.PositionMiddle;
      offenderLinemanMiddle.InitialTop = FieldCenterY;
      offenderLinemanMiddle.InitialLeft = LineOfScrimage - 25;
      offenderLinemanMiddle.PicBox = AddPlayerPictureBox(ParentForm.Player1);
      offenderLinemanMiddle.Initialize();
      Player.AddPlayer(offenderLinemanMiddle);

      OffenderLinemanTop offenderLinemanTop = offenderLinemanMiddle.CloneAndUpcast<OffenderLinemanTop, OffenderLineman>();
      offenderLinemanTop.VerticalPosition = VerticalPosition.PositionTop;
      offenderLinemanTop.InitialTop = FieldCenterY - 85; //? an extra 3 toward top because the top defensive lineman always goes on overtop
      offenderLinemanTop.PicBox = AddPlayerPictureBox(ParentForm.Player1);
      offenderLinemanTop.Initialize();
      Player.AddPlayer(offenderLinemanTop);

      OffenderLinemanBottom offenderLinemanBottom = offenderLinemanTop.CloneAndUpcast<OffenderLinemanBottom, OffenderLineman>();
      offenderLinemanBottom.VerticalPosition = VerticalPosition.PositionBottom;
      offenderLinemanBottom.InitialTop = FieldCenterY + 85;
      offenderLinemanBottom.PicBox = AddPlayerPictureBox(ParentForm.Player1);
      offenderLinemanBottom.Initialize();
      Player.AddPlayer(offenderLinemanBottom);

      OffenderWideReceiverTop offenderWideReceiverTop = new OffenderWideReceiverTop();
      offenderWideReceiverTop.InitialTop = FieldCenterY - 240;
      offenderWideReceiverTop.InitialLeft = LineOfScrimage - 25;
      offenderWideReceiverTop.PicBox = AddPlayerPictureBox(ParentForm.Player1);
      offenderWideReceiverTop.ButtonHookPattern(); // TODO randomize
      offenderWideReceiverTop.Initialize();
      Player.AddPlayer(offenderWideReceiverTop);

      OffenderWideReceiverBottom offenderWideReceiverBottom = new OffenderWideReceiverBottom();
      offenderWideReceiverBottom.InitialTop = FieldCenterY + 240;
      offenderWideReceiverBottom.InitialLeft = LineOfScrimage - 25;
      offenderWideReceiverBottom.PicBox = AddPlayerPictureBox(ParentForm.Player1);
      offenderWideReceiverBottom.TheBomb(); // TODO randomize
      offenderWideReceiverBottom.Initialize();
      Player.AddPlayer(offenderWideReceiverBottom);


      //--------------------- Defensive Players
      DefenderMiddleLineman defenderMiddleLineman = new DefenderMiddleLineman(); 
      defenderMiddleLineman.InitialTop = FieldCenterY;
      defenderMiddleLineman.InitialLeft = LineOfScrimage + 25;
      defenderMiddleLineman.PicBox = AddPlayerPictureBox(ParentForm.Player2);
      defenderMiddleLineman.Initialize();
      Player.AddPlayer(defenderMiddleLineman);

      DefenderOutsideLinemanTop defenderOutsideLinemanTop = defenderMiddleLineman.CloneAndUpcast<DefenderOutsideLinemanTop, Player>();
      defenderOutsideLinemanTop.VerticalPosition = VerticalPosition.PositionTop;
      defenderOutsideLinemanTop.InitialOffset = -240;
      defenderOutsideLinemanTop.InitialLeft = LineOfScrimage + 25;
      defenderOutsideLinemanTop.InitialTop = FieldCenterY -112;
      defenderOutsideLinemanTop.PicBox = AddPlayerPictureBox(ParentForm.Player2);
      defenderOutsideLinemanTop.PicBox.BackColor = Color.LightGreen;
      //defenderOutsideLinemanTop.CoDefender = defenderMiddleLineman;
      defenderOutsideLinemanTop.Initialize();
      Player.AddPlayer(defenderOutsideLinemanTop);

      DefenderOutsideLinemanBottom defenderOutsideLinemanBottom = defenderOutsideLinemanTop.CloneAndUpcast<DefenderOutsideLinemanBottom, DefenderOutsideLineman>();
      defenderOutsideLinemanTop.VerticalPosition = VerticalPosition.PositionBottom;
      defenderOutsideLinemanBottom.InitialOffset = 240;
      defenderOutsideLinemanBottom.InitialTop = FieldCenterY + 112;
      defenderOutsideLinemanBottom.PicBox = AddPlayerPictureBox(ParentForm.Player2);
      defenderOutsideLinemanBottom.PicBox.BackColor = Color.LightGreen;
      //defenderOutsideLinemanBottom.CoDefender = defenderMiddleLineman;
      defenderOutsideLinemanBottom.Initialize();
      Player.AddPlayer(defenderOutsideLinemanBottom);

      DefenderMiddleLinebacker defenderMiddleLinebacker = defenderOutsideLinemanTop.CloneAndUpcast<DefenderMiddleLinebacker, Player>();
      defenderMiddleLinebacker.DefensiveMode = DefensiveMode.Normal;  // TODO randomize between coverage
      defenderMiddleLinebacker.InitialLeft = 420;
      defenderMiddleLinebacker.InitialTop = FieldCenterY;
      defenderMiddleLinebacker.PicBox = AddPlayerPictureBox(ParentForm.Player2);
      defenderMiddleLinebacker.PicBox.BackColor = Color.DarkGreen;
      defenderMiddleLinebacker.Initialize();
      Player.AddPlayer(defenderMiddleLinebacker);

      DefenderCornerbackTop defenderCornerbackTop = new DefenderCornerbackTop();
      defenderCornerbackTop.DefensiveMode = DefensiveMode.Normal;  // TODO randomize between coverage
      defenderCornerbackTop.InitialLeft = offenderWideReceiverTop.Left + 200;
      defenderCornerbackTop.InitialTop = offenderWideReceiverTop.Top + 30;
      defenderCornerbackTop.PicBox = AddPlayerPictureBox(ParentForm.Player2);
      defenderCornerbackTop.Initialize();
      Player.AddPlayer(defenderCornerbackTop);

      DefenderCornerbackBottom defenderCornerbackBottom = new DefenderCornerbackBottom();
      defenderCornerbackBottom.DefensiveMode = DefensiveMode.Normal;  // TODO randomize between coverage
      defenderCornerbackBottom.InitialLeft = offenderWideReceiverBottom.Left + 200;
      defenderCornerbackBottom.InitialTop = offenderWideReceiverBottom.Top -30;
      defenderCornerbackBottom.PicBox = AddPlayerPictureBox(ParentForm.Player2);
      defenderCornerbackBottom.Initialize();
      Player.AddPlayer(defenderCornerbackBottom);

      // Ball as a Player
      ballAsPlayer.InitialLeft = -999;
      ballAsPlayer.InitialTop = -999;
      ballAsPlayer.PicBox = ParentForm.picFootball;
      ballAsPlayer.Initialize();
      Player.AddPlayer(ballAsPlayer);

      // Setup Initial TargetPlayers
      defenderCornerbackTop.InitialTargetPlayer = offenderWideReceiverTop;
      defenderCornerbackBottom.InitialTargetPlayer = offenderWideReceiverBottom;
      offenderLinemanTop.InitialTargetPlayer = defenderOutsideLinemanTop;
       offenderLinemanMiddle.InitialTargetPlayer = defenderMiddleLineman;
      offenderLinemanBottom.InitialTargetPlayer = defenderOutsideLinemanBottom;

      //Player.Get(typeof(OffenderLinemanBottom));
    }

    public PictureBox AddPlayerPictureBox(PictureBox pb)
    {
      PictureBox p = new PictureBox();
      p.Height = pb.Height;
      p.Width = pb.Width;
      p.SizeMode = pb.SizeMode;
      p.Image = pb.Image;
      ParentForm.Controls.Add(p);
      return p;
    }

    public void ReinitializePlayers()
    {
      Player.Players.ForEach(p => p.Initialize());
    }

    public void Run()
    {
      timer.Start();
      while (running)
      {
        if(PlayEnded)
        {
          ReinitializePlayers();
          PlayEnded = false;
        }

        Application.DoEvents();
        foreach (Player p in Player.Players)
        {
          Application.DoEvents();
          p.Move();
        }
        CheckCollisions(Player.Players);

        Thread.Sleep(32);
      }
    }

    public void EndPlay(EndPlayType endPlayType, string message)
    {
      if (PlayEnded) // Play ended by another player
        return;

      PlayEnded = true;
      yardsGained = 0;
      if (endPlayType == EndPlayType.Tackled || endPlayType == EndPlayType.OutOfBounds)
      {
        yardsGained = (float)(Player.ControllablePlayer.Left + Player.ControllablePlayer.PicBox.Width - LineOfScrimage) / PixalsInYard;
        lineOfScrimage += yardsGained;
        yardsToGo -= yardsGained;
      }

      down++;

      if(yardsToGo < 0)
      {
        yardsToGo = 10;
        down = 1;
        message += Environment.NewLine + "First Down!";
      }

      float displayedLineOfScrimage = lineOfScrimage < 50 ? lineOfScrimage : 100 - lineOfScrimage;

      Scoreboard.DisplayBallOn(displayedLineOfScrimage.ToString("00"));
      Scoreboard.DisplayToGo(yardsToGo.ToString("00"));
      Scoreboard.DisplayDown(down.ToString("0"));

      if(yardsGained != 0)
        MessageBox.Show(message + Environment.NewLine + $"{yardsGained,0:#.#} yards gained.");
      else
        MessageBox.Show(message + Environment.NewLine + "No gain");

      ParentForm.Invalidate();
    }

    public void Stop()
    {
      timer.Stop();
      running = false;
    }

    public void KeyDown(object sender, EventArgs e)
    {
      bool keypressed = false;

      if (PlayEnded) // Stop the player from moving after pay ends.
      {
        Player.ControllablePlayer.ChangeX = 0; 
        Player.ControllablePlayer.ChangeY = 0;
        return;
      }

      if (IsKeyDown(Keys.Left))
      {
        if(Player.ControllablePlayer.ChangeX > 30)
          Player.ControllablePlayer.ChangeX -= 14;
        else
          Player.ControllablePlayer.ChangeX -= 6;
        keypressed = true;
      }
      if (IsKeyDown(Keys.Right))
      {
        if (Player.ControllablePlayer.ChangeX < 30)
          Player.ControllablePlayer.ChangeX += 14;
        else
          Player.ControllablePlayer.ChangeX += 6;
        keypressed = true;
      }
      if (IsKeyDown(Keys.Up))
      {
        if (Player.ControllablePlayer.ChangeY > 30)
          Player.ControllablePlayer.ChangeY -= 14;
        else
          Player.ControllablePlayer.ChangeY -= 6;
        keypressed = true;
      }
      if (IsKeyDown(Keys.Down))
      {
        if (Player.ControllablePlayer.ChangeY < 30)
          Player.ControllablePlayer.ChangeY += 14;
        else
          Player.ControllablePlayer.ChangeY += 6;
        keypressed = true;
      }
      if (keypressed == false)
      {
        Player.ControllablePlayer.ChangeX = Player.ControllablePlayer.ChangeX - (8 * Math.Sign(Player.ControllablePlayer.ChangeX));
        Player.ControllablePlayer.ChangeY = Player.ControllablePlayer.ChangeY - (8 * Math.Sign(Player.ControllablePlayer.ChangeY));
      }

      if (Math.Abs(Player.ControllablePlayer.ChangeX) > Player.ControllablePlayer.SpeedCap -32)
      {
        Player.ControllablePlayer.ChangeX = (Player.ControllablePlayer.SpeedCap - 40) * Math.Sign(Player.ControllablePlayer.ChangeX);
      }

      Player.ControllablePlayer.Move();

      return;
    }

    public static bool IsKeyDown(Keys key)
    {
      return (GetKeyState(Convert.ToInt16(key)) & 0X80) == 0X80;
    }
    [DllImport("user32.dll")]
    public extern static Int16 GetKeyState(Int16 nVirtKey);

    public void MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        // Pass the ball. Ball is really another player.
        if(!Player.IsThrowing)
          ballAsPlayer.ThrowBall(Player.ControllablePlayer.Left + 16, Player.ControllablePlayer.Top + 16, e.Location.Y, e.Location.X);   
      }
    }

    public void PaintField(object sender, PaintEventArgs e)
    {
      Pen pen = new Pen(Color.FromArgb(255, 128, 128, 255));
      e.Graphics.DrawLine(pen, Game.LineOfScrimage, 0, Game.LineOfScrimage, 800);
      pen = new Pen(Color.FromArgb(255, 255, 255, 0));
      int firstDownMarker = LineOfScrimage + ((int)yardsToGo * (int)PixalsInYard);
      e.Graphics.DrawLine(pen, firstDownMarker, 0, firstDownMarker, 800);
    }

    public void CheckCollisions(List<Player> players)
    {
      for (int i = 0; i < Player.Players.Count - 1; i++)
      {
        for (int j = i + 1; j < Player.Players.Count; j++)
        {
          if (!Player.IsThrowing && players[j].IsBall) 
            break;

          if (!(players[i] is OffenderWideReceiver) && !(players[i] is DefenderCornerback) && !(players[i] is DefenderMiddleLinebacker) && players[j].IsBall) 
            break;


          //if (players[j].IsBall && (players[i] is DefenderCornerback) && ballAsPlayer.BallIsCatchable) // TODO take out
          //  players[j].IsBall = players[j].IsBall;

          // If player is hitting another player
          if (DetectCollision(players[i], players[j]))
          {
            // Hitting above or below another player
            if (Math.Abs(players[i].Left - players[j].Left) < Math.Abs(players[i].Top - players[j].Top))
            {
              //  | |
              //
              //  | |
              if (players[i].Top < players[j].Top)
              {
                players[j].CollisionMove(players[i], CollisionOrientation.Below);
                players[i].CollisionMove(players[j], CollisionOrientation.Above);
              }
              else
              {
                players[j].CollisionMove(players[i], CollisionOrientation.Above);
                players[i].CollisionMove(players[j], CollisionOrientation.Below);
              }
            }
            else // Hitting to the left or right of another player
            {
              //  | |   | |
              if (players[i].Left < players[j].Left)
              {
                players[j].CollisionMove(players[i], CollisionOrientation.ToRight);
                players[i].CollisionMove(players[j], CollisionOrientation.ToLeft);
              }
              else
              {
                players[j].CollisionMove(players[i], CollisionOrientation.ToLeft);
                players[i].CollisionMove(players[j], CollisionOrientation.ToRight);
              }
            }
            Player.MovePic(players[j]);
            Player.MovePic(players[i]);
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
  }
}
