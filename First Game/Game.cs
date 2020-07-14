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
    private static float lineOfScrimageYard = 20;
    private static float yardsGained = 0;
    private static float yardsToGo = 10;
    private static float down = 1;

    private static bool running = true;
    private static System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

    public static float PixalsInYard = 32;
    public static Form1 ParentForm;
    public static BallAsPlayer ballAsPlayer = new BallAsPlayer();
    public static Rectangle FieldBounds;
    public static int FieldHeight;
    public static int FieldCenterY;
    public static int LineOfScrimagePixel = 280;
    public static Random Random = new Random();
    public static bool PlayEnded = false;

    public Game(Form1 form1)
    {
      // Set Parents
      ParentForm = form1;
      Player.ParentForm = form1;
      Player.ParentGame = this;
      Scoreboard.ParentForm = form1;
      Sideline.ParentForm = form1;
      lineOfScrimageYard = 20; // 1 - 100;
      yardsToGo = 10;

      ParentForm.pnlPlayOptions.Visible = false;

      Scoreboard.InitializeDrawing(); // Draw the starting scoreboard
      Sideline.InitializeDrawing();   // Draw the starting sideline

      // Initialize field dimensions
      FieldBounds = new Rectangle(0, ParentForm.pnlScoreboard.Height + 30, ParentForm.Width - ParentForm.Player1.Width, ParentForm.Height - ParentForm.pnlScoreboard.Height - 36);
      Player.FieldBounds = FieldBounds;
      FieldCenterY = (FieldBounds.Height / 2) + ParentForm.picSidelineYardage.Height + 2; // Players go out of bounds when their botton goes out.

      AddPlayers();

      // Getting keyboard input
      timer.Tick += new System.EventHandler(KeyDown);
      timer.Interval = 50;
    }

    public void AddPlayers()
    {
      //--------------------- Offensive Players
      OffenderQuarterback offenderQuarterback = new OffenderQuarterback();
      offenderQuarterback.InitialLeft = LineOfScrimagePixel - 200;
      offenderQuarterback.InitialTop = FieldCenterY;
      offenderQuarterback.PicBox = AddPlayerPictureBox(ParentForm.Player1);
      offenderQuarterback.Initialize();
      Player.AddPlayer(offenderQuarterback);
      Player.ControllablePlayer = offenderQuarterback;

      OffenderWideReceiverTop offenderWideReceiverTop = new OffenderWideReceiverTop();
      offenderWideReceiverTop.InitialTop = FieldCenterY - 240;
      offenderWideReceiverTop.InitialLeft = LineOfScrimagePixel - 25;
      offenderWideReceiverTop.PicBox = AddPlayerPictureBox(ParentForm.Player1);
      offenderWideReceiverTop.ButtonHookPattern(); // TODO randomize
      offenderWideReceiverTop.Initialize();
      Player.AddPlayer(offenderWideReceiverTop);

      OffenderOutsideLinemanTop offenderOutsideLinemanTop = new OffenderOutsideLinemanTop();
      offenderOutsideLinemanTop.VerticalPosition = VerticalPosition.PositionTop;
      offenderOutsideLinemanTop.InitialLeft = LineOfScrimagePixel - 25;
      offenderOutsideLinemanTop.InitialTop = FieldCenterY - 90;
      offenderOutsideLinemanTop.PicBox = AddPlayerPictureBox(ParentForm.Player1);
      offenderOutsideLinemanTop.Initialize();
      Player.AddPlayer(offenderOutsideLinemanTop);

      OffenderLinemanUpper offenderLinemanUpper = new OffenderLinemanUpper();
      offenderLinemanUpper.VerticalPosition = VerticalPosition.PositionMiddle;
      offenderLinemanUpper.InitialLeft = LineOfScrimagePixel - 25;
      offenderLinemanUpper.InitialTop = FieldCenterY - 60;
      offenderLinemanUpper.PicBox = AddPlayerPictureBox(ParentForm.Player1);
      offenderLinemanUpper.Initialize();
      Player.AddPlayer(offenderLinemanUpper);

      OffenderLinemanCenter offenderLinemanCenter = new OffenderLinemanCenter();
      offenderLinemanCenter.VerticalPosition = VerticalPosition.PositionMiddle;
      offenderLinemanCenter.InitialLeft = LineOfScrimagePixel - 25;
      offenderLinemanCenter.InitialTop = FieldCenterY;
      offenderLinemanCenter.PicBox = AddPlayerPictureBox(ParentForm.Player1);
      offenderLinemanCenter.Initialize();
      Player.AddPlayer(offenderLinemanCenter);

      OffenderLinemanLower offenderLinemanLower = new OffenderLinemanLower();
      offenderLinemanLower.VerticalPosition = VerticalPosition.PositionMiddle;
      offenderLinemanLower.InitialLeft = LineOfScrimagePixel - 25;
      offenderLinemanLower.InitialTop = FieldCenterY + 60;
      offenderLinemanLower.PicBox = AddPlayerPictureBox(ParentForm.Player1);
      offenderLinemanLower.Initialize();
      Player.AddPlayer(offenderLinemanLower);

      OffenderOutsideLinemanBottom offenderOutsideLinemanBottom = offenderOutsideLinemanTop.CloneAndUpcast<OffenderOutsideLinemanBottom, OffenderLineman>();
      offenderOutsideLinemanBottom.VerticalPosition = VerticalPosition.PositionBottom;
      offenderOutsideLinemanBottom.InitialTop = FieldCenterY + 90;
      offenderOutsideLinemanBottom.PicBox = AddPlayerPictureBox(ParentForm.Player1);
      offenderOutsideLinemanBottom.Initialize();
      Player.AddPlayer(offenderOutsideLinemanBottom);

      OffenderWideReceiverBottom offenderWideReceiverBottom = new OffenderWideReceiverBottom();
      offenderWideReceiverBottom.InitialTop = FieldCenterY + 240;
      offenderWideReceiverBottom.InitialLeft = LineOfScrimagePixel - 25;
      offenderWideReceiverBottom.PicBox = AddPlayerPictureBox(ParentForm.Player1);
      offenderWideReceiverBottom.TheBomb(); // TODO randomize
      offenderWideReceiverBottom.Initialize();
      Player.AddPlayer(offenderWideReceiverBottom);


      //--------------------- Defensive Players
      DefenderCornerbackTop defenderCornerbackTop = new DefenderCornerbackTop();
      defenderCornerbackTop.DefensiveMode = DefensiveMode.Normal;  // TODO randomize between coverage
      defenderCornerbackTop.InitialLeft = offenderWideReceiverTop.Left + 200;
      defenderCornerbackTop.InitialTop = offenderWideReceiverTop.Top + 30;
      defenderCornerbackTop.PicBox = AddPlayerPictureBox(ParentForm.Player2);
      defenderCornerbackTop.Initialize();
      Player.AddPlayer(defenderCornerbackTop);

      DefenderOutsideLinemanTop defenderOutsideLinemanTop = new DefenderOutsideLinemanTop();
      defenderOutsideLinemanTop.VerticalPosition = VerticalPosition.PositionTop;
      defenderOutsideLinemanTop.InitialLeft = LineOfScrimagePixel + 25;
      defenderOutsideLinemanTop.InitialTop = FieldCenterY -160;
      defenderOutsideLinemanTop.InitialOffsetY = -260;
      defenderOutsideLinemanTop.PicBox = AddPlayerPictureBox(ParentForm.Player2);
      defenderOutsideLinemanTop.PicBox.BackColor = Color.LightGreen;
      //defenderOutsideLinemanTop.CoDefender = defenderMiddleLineman;
      defenderOutsideLinemanTop.Initialize();
      Player.AddPlayer(defenderOutsideLinemanTop);
      
      DefenderLinemanUpper defenderLinemanUpper = new DefenderLinemanUpper();
      defenderLinemanUpper.InitialLeft = LineOfScrimagePixel + 25;
      defenderLinemanUpper.InitialTop = FieldCenterY - 60;
      defenderLinemanUpper.InitialOffsetY = -110;
      defenderLinemanUpper.PicBox = AddPlayerPictureBox(ParentForm.Player2);
      defenderLinemanUpper.Initialize();
      Player.AddPlayer(defenderLinemanUpper);

        // Middle Linebacker
        DefenderMiddleLinebacker defenderMiddleLinebacker = defenderOutsideLinemanTop.CloneAndUpcast<DefenderMiddleLinebacker, Player>();
        defenderMiddleLinebacker.DefensiveMode = DefensiveMode.Normal;  // TODO randomize between coverage
        defenderMiddleLinebacker.InitialLeft = LineOfScrimagePixel + 120; 
        defenderMiddleLinebacker.InitialTop = FieldCenterY;
        defenderMiddleLinebacker.PicBox = AddPlayerPictureBox(ParentForm.Player2);
        defenderMiddleLinebacker.PicBox.BackColor = Color.DarkGreen;
        defenderMiddleLinebacker.Initialize();
        Player.AddPlayer(defenderMiddleLinebacker);

      DefenderLinemanLower defenderLinemanLower = new DefenderLinemanLower();
      defenderLinemanLower.InitialLeft = LineOfScrimagePixel + 25;
      defenderLinemanLower.InitialTop = FieldCenterY + 60;
      defenderLinemanLower.InitialOffsetY = 110;
      defenderLinemanLower.PicBox = AddPlayerPictureBox(ParentForm.Player2);
      defenderLinemanLower.Initialize();
      Player.AddPlayer(defenderLinemanLower);

      DefenderOutsideLinemanBottom defenderOutsideLinemanBottom = defenderOutsideLinemanTop.CloneAndUpcast<DefenderOutsideLinemanBottom, DefenderOutsideLineman>();
      defenderOutsideLinemanTop.VerticalPosition = VerticalPosition.PositionBottom;
      defenderOutsideLinemanBottom.InitialLeft = LineOfScrimagePixel + 25;
      defenderOutsideLinemanBottom.InitialTop = FieldCenterY + 160;
      defenderOutsideLinemanBottom.InitialOffsetY = 260;
      defenderOutsideLinemanBottom.PicBox = AddPlayerPictureBox(ParentForm.Player2);
      defenderOutsideLinemanBottom.PicBox.BackColor = Color.LightGreen;
      defenderOutsideLinemanBottom.Initialize();
      Player.AddPlayer(defenderOutsideLinemanBottom);

      DefenderCornerbackBottom defenderCornerbackBottom = new DefenderCornerbackBottom();
      defenderCornerbackBottom.DefensiveMode = DefensiveMode.Normal;  // TODO randomize between coverage
      defenderCornerbackBottom.InitialLeft = offenderWideReceiverBottom.Left + 200;
      defenderCornerbackBottom.InitialTop = offenderWideReceiverBottom.Top - 30;
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

      offenderOutsideLinemanTop.InitialTargetPlayer = defenderOutsideLinemanTop;
      offenderLinemanUpper.InitialTargetPlayer = defenderLinemanUpper;
      if(Random.Next(-10, 10) < 0)
        offenderLinemanCenter.InitialTargetPlayer = defenderLinemanUpper;
      else
        offenderLinemanCenter.InitialTargetPlayer = defenderLinemanLower;
      offenderLinemanLower.InitialTargetPlayer = defenderLinemanLower;
      offenderOutsideLinemanBottom.InitialTargetPlayer = defenderOutsideLinemanBottom;
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
        yardsGained = (float)(Player.ControllablePlayer.Left + Player.ControllablePlayer.PicBox.Width - LineOfScrimagePixel) / PixalsInYard;
        lineOfScrimageYard += yardsGained;
        yardsToGo -= yardsGained;
      }

      if (lineOfScrimageYard < 0)
      {
        lineOfScrimageYard = -1; // Safety
      }
      if (lineOfScrimageYard > 100)
      {
        lineOfScrimageYard = 101; // Touchdown
      }

      if (down < 4)
        down++;
      

      if(yardsToGo < 0)
      {
        yardsToGo = 10;
        down = 1;
        message += Environment.NewLine + "First Down!";
      }

      float displayedLineOfScrimage = lineOfScrimageYard < 50 ? lineOfScrimageYard : 100 - lineOfScrimageYard;

      Scoreboard.DisplayBallOn(displayedLineOfScrimage.ToString("00"));
      Scoreboard.DisplayToGo(yardsToGo.ToString("00"));
      Scoreboard.DisplayDown(down.ToString("0"));

      if(yardsGained != 0)
        MessageBox.Show(message + Environment.NewLine + $"{yardsGained,0:#.#} yards gained.");
      else
        MessageBox.Show(message + Environment.NewLine + "No gain");

      Sideline.DisplaySideline(lineOfScrimageYard);
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

    public void PaintScrimmageAndFirstDownLines(object sender, PaintEventArgs e)
    {
      // Scrimmage
      Pen pen = new Pen(Color.FromArgb(255, 128, 128, 255));
      e.Graphics.DrawLine(pen, Game.LineOfScrimagePixel, 0, Game.LineOfScrimagePixel, ParentForm.Height - 62);
      // First Down
      pen = new Pen(Color.FromArgb(255, 255, 255, 0));
      int firstDownMarker = LineOfScrimagePixel + ((int)yardsToGo * (int)PixalsInYard);
      e.Graphics.DrawLine(pen, firstDownMarker, 0, firstDownMarker, ParentForm.Height -62);
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
