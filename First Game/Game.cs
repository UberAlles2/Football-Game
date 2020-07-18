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
 * Middle linebacker isn't intercepting and colliding with the ball
 * Add ball on indicator if in opposing territory
 * Add more WR patterns
 * Only allow so much time, 20 seconds to choose a play.
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
    Dropped,
    Intercepted
  }

  public class Game
  {
    private static float LineOfScrimageYard = 20;

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
    public static PlayOptionsForm PlayOptionsForm;
    public static PlayOptionsFormStats PlayOptionsFormStats = new PlayOptionsFormStats();

    public static OffenderWideReceiverTop offenderWideReceiverTop = new OffenderWideReceiverTop();
    public static OffenderWideReceiverBottom offenderWideReceiverBottom = new OffenderWideReceiverBottom();

    public Game(Form1 form1)
    {
      // Set Parents
      ParentForm = form1;
      Player.ParentForm = form1;
      Player.ParentGame = this;
      Scoreboard.ParentForm = form1;
      Sideline.ParentForm = form1;
      PlayOptionsForm = new PlayOptionsForm(this);

      // Set initial values and Display them.
      LineOfScrimageYard = 20; // 1 - 100; 
      PlayOptionsFormStats.Down = 1;
      PlayOptionsFormStats.YardsToGo = 10;
      PlayOptionsFormStats.BallOnYard = 20;
      Scoreboard.InitializeDrawing(); // Draw the starting scoreboard
      Sideline.InitializeDrawing();   // Draw the starting sideline

      // Initialize field dimensions
      FieldBounds = new Rectangle(0, ParentForm.pnlScoreboard.Height + 30, ParentForm.Width - ParentForm.Player1.Width, ParentForm.Height - ParentForm.pnlScoreboard.Height - 36);
      Player.FieldBounds = FieldBounds;
      FieldCenterY = (FieldBounds.Height / 2) + ParentForm.picSidelineYardage.Height + 16; // Players go out of bounds when their botton goes out.

      AddPlayers();

      PlayEnded = true; // Causes PlayOptions form to be displayed

      // Getting keyboard input
      timer.Tick += new System.EventHandler(KeyDown);
      timer.Interval = 50;
    }

    public void AddPlayers()
    {
      int initlineX = LineOfScrimagePixel - 25;

      //--------------------- Offensive Players
      OffenderQuarterback offenderQuarterback = new OffenderQuarterback();
      Player.AddPlayer(offenderQuarterback, LineOfScrimagePixel - 200, FieldCenterY, ParentForm.Player1);
      Player.ControllablePlayer = offenderQuarterback;

      Player.AddPlayer(offenderWideReceiverTop, initlineX, FieldCenterY - 220, ParentForm.Player1);

      OffenderOutsideLinemanTop offenderOutsideLinemanTop = new OffenderOutsideLinemanTop();
      offenderOutsideLinemanTop.VerticalPosition = VerticalPosition.PositionTop;
      Player.AddPlayer(offenderOutsideLinemanTop, initlineX, FieldCenterY - 96, ParentForm.Player1);

      OffenderLinemanUpper offenderLinemanUpper = new OffenderLinemanUpper();
      offenderLinemanUpper.VerticalPosition = VerticalPosition.PositionMiddle;
      Player.AddPlayer(offenderLinemanUpper, initlineX, FieldCenterY - 52, ParentForm.Player1);

      OffenderLinemanCenter offenderLinemanCenter = new OffenderLinemanCenter();
      offenderLinemanCenter.VerticalPosition = VerticalPosition.PositionMiddle;
      Player.AddPlayer(offenderLinemanCenter, initlineX, FieldCenterY, ParentForm.Player1);

      OffenderLinemanLower offenderLinemanLower = new OffenderLinemanLower();
      offenderLinemanLower.VerticalPosition = VerticalPosition.PositionMiddle;
      Player.AddPlayer(offenderLinemanLower, initlineX, FieldCenterY + 52, ParentForm.Player1);

      OffenderOutsideLinemanBottom offenderOutsideLinemanBottom = new OffenderOutsideLinemanBottom();
      offenderOutsideLinemanBottom.VerticalPosition = VerticalPosition.PositionBottom;
      Player.AddPlayer(offenderOutsideLinemanBottom, initlineX, FieldCenterY + 96, ParentForm.Player1);

      Player.AddPlayer(offenderWideReceiverBottom, initlineX, FieldCenterY + 220, ParentForm.Player1);

      initlineX = LineOfScrimagePixel + 25;
      
      //--------------------- Defensive Players
      DefenderCornerbackTop defenderCornerbackTop = new DefenderCornerbackTop();
      Player.AddPlayer(defenderCornerbackTop, offenderWideReceiverTop.InitialLeft + 160, offenderWideReceiverTop.InitialTop + 30, ParentForm.Player2);

      DefenderOutsideLinemanTop defenderOutsideLinemanTop = new DefenderOutsideLinemanTop();
      defenderOutsideLinemanTop.VerticalPosition = VerticalPosition.PositionTop;
      Player.AddPlayer(defenderOutsideLinemanTop, initlineX, FieldCenterY - 152, ParentForm.Player2, initialOffsetY: -250);
      defenderOutsideLinemanTop.PicBox.BackColor = Color.LightGreen; // TODO take out

      DefenderLinemanUpper defenderLinemanUpper = new DefenderLinemanUpper();
      Player.AddPlayer(defenderLinemanUpper, initlineX, FieldCenterY - 50, ParentForm.Player2, initialOffsetY: -87);
      defenderLinemanUpper.PicBox.BackColor = Color.LightBlue; // TODO take out

        // Middle Linebacker
        DefenderMiddleLinebacker defenderMiddleLinebacker = new DefenderMiddleLinebacker();
        Player.AddPlayer(defenderMiddleLinebacker, LineOfScrimagePixel + 120, FieldCenterY, ParentForm.Player2);
        defenderMiddleLinebacker.PicBox.BackColor = Color.DarkGreen; // TODO take out
      
        // Safety
        DefenderSafety defenderSafety = new DefenderSafety();
        Player.AddPlayer(defenderSafety, LineOfScrimagePixel + 420, FieldCenterY, ParentForm.Player2);
        defenderSafety.PicBox.BackColor = Color.HotPink; // TODO take out

      DefenderLinemanLower defenderLinemanLower = new DefenderLinemanLower();
      Player.AddPlayer(defenderLinemanLower, initlineX, FieldCenterY + 48, ParentForm.Player2, initialOffsetY: 87);
      defenderLinemanLower.PicBox.BackColor = Color.DarkBlue; // TODO take out

      DefenderOutsideLinemanBottom defenderOutsideLinemanBottom = new DefenderOutsideLinemanBottom();
      defenderOutsideLinemanTop.VerticalPosition = VerticalPosition.PositionBottom;
      Player.AddPlayer(defenderOutsideLinemanBottom, initlineX, FieldCenterY + 152, ParentForm.Player2, initialOffsetY: 250);
      defenderOutsideLinemanBottom.PicBox.BackColor = Color.LightGreen; // TODO take out

      DefenderCornerbackBottom defenderCornerbackBottom = new DefenderCornerbackBottom();
      defenderCornerbackBottom.DefensiveMode = DefensiveMode.Normal;  // TODO randomize between coverage
      Player.AddPlayer(defenderCornerbackBottom, offenderWideReceiverBottom.InitialLeft + 160, offenderWideReceiverBottom.InitialTop - 30, ParentForm.Player2);

      // Ball as a Player
      ballAsPlayer.InitialLeft = -999;
      ballAsPlayer.InitialTop = -999;
      ballAsPlayer.PicBox = ParentForm.picFootball;
      Player.AddPlayer(ballAsPlayer);

      // Setup Initial TargetPlayers
      defenderCornerbackTop.InitialTargetPlayer = offenderWideReceiverTop;
      defenderCornerbackBottom.InitialTargetPlayer = offenderWideReceiverBottom;
      defenderSafety.OffenseWideReceiverTop = offenderWideReceiverTop; // Safty can cover 3 different player ot the ball if thrown.
      defenderSafety.OffenseWideReceiverBottom = offenderWideReceiverBottom;
      defenderSafety.DefenderMiddleLinebacker = defenderMiddleLinebacker;

      offenderOutsideLinemanTop.InitialTargetPlayer = defenderOutsideLinemanTop;
      offenderLinemanUpper.InitialTargetPlayer = defenderLinemanUpper;
      offenderLinemanCenter.DefenseLinemanUpper = defenderLinemanUpper;
      offenderLinemanCenter.DefenseLinemanLower = defenderLinemanLower;
      offenderLinemanLower.InitialTargetPlayer = defenderLinemanLower;
      offenderOutsideLinemanBottom.InitialTargetPlayer = defenderOutsideLinemanBottom;

      InitializePlayers(); // Initial draw
    }

    public void InitializePlayers()
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
          ChoosePlay();
          InitializePlayers();
          PlayEnded = false;
        }

        Application.DoEvents();
        foreach (Player p in Player.Players)
        {
          Application.DoEvents();
          p.Move();
        }
        CheckCollisions(Player.Players);

        Thread.Sleep(30);  // Speed of the game, increase for easy mode
      }
    }

    private void ChoosePlay()
    {
      PlayOptionsForm.Location = new Point(ParentForm.Left + 660, ParentForm.Top + 160);
      PlayOptionsForm.ShowDialog();
      Scoreboard.CountDownTimer.Start();
    }

    public void EndPlay(EndPlayType endPlayType, Player tackledBy, string message)
    {
      if (PlayEnded) // Play ended by another player
        return;

      if (endPlayType != EndPlayType.Tackled)
        Scoreboard.CountDownTimer.Stop();

      PlayEnded = true;
      PlayOptionsFormStats.YardsGained = 0;
      if (endPlayType == EndPlayType.Tackled || endPlayType == EndPlayType.OutOfBounds)
      {
        PlayOptionsFormStats.YardsGained = (float)(Player.ControllablePlayer.Left + Player.ControllablePlayer.PicBox.Width - LineOfScrimagePixel) / PixalsInYard;
        PlayOptionsFormStats.TackledBy = tackledBy;
        LineOfScrimageYard += PlayOptionsFormStats.YardsGained;
        PlayOptionsFormStats.YardsToGo -= PlayOptionsFormStats.YardsGained;
      }
      else
        PlayOptionsFormStats.TackledBy = null;

      PlayOptionsFormStats.ResultsOfLastPlay = message;

      if (LineOfScrimageYard < 0)
      {
        LineOfScrimageYard = -1; // Safety
      }
      if (LineOfScrimageYard > 100)
      {
        LineOfScrimageYard = 101; // Touchdown
      }

      if (PlayOptionsFormStats.Down < 4)
        PlayOptionsFormStats.Down++;
      

      if(PlayOptionsFormStats.YardsToGo < 0.05)
      {
        PlayOptionsFormStats.YardsToGo = 10;
        PlayOptionsFormStats.Down = 1;
        PlayOptionsFormStats.ResultsOfLastPlay += "  First Down!";
      }
      PlayOptionsFormStats.BallOnYard100 = LineOfScrimageYard;
      PlayOptionsFormStats.BallOnYard = LineOfScrimageYard < 50 ? LineOfScrimageYard : 100 - LineOfScrimageYard;

      Scoreboard.DisplayBallOn(PlayOptionsFormStats.BallOnYard.ToString("00"));
      Scoreboard.DisplayToGo(PlayOptionsFormStats.YardsToGo.ToString("00"));
      Scoreboard.DisplayDown(PlayOptionsFormStats.Down.ToString("0"));

      //if(PlayOptionsFormStats.YardsGained != 0)
      //  MessageBox.Show(message + Environment.NewLine + $"{PlayOptionsFormStats.YardsGained,0:#.#} yards gained.");
      //else
      //  MessageBox.Show(message + Environment.NewLine + "No gain");

      Sideline.DisplaySideline(LineOfScrimageYard);
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

      if (IsKeyDown(Keys.Left)) // <<<-------
      {
        if(Player.ControllablePlayer.ChangeX > 30) // Moving Right -->>
          Player.ControllablePlayer.ChangeX -= 16;
        else if (Player.ControllablePlayer.ChangeX < 40) // Moving Left <<--
          Player.ControllablePlayer.ChangeX -= 4;
        else
          Player.ControllablePlayer.ChangeX -= 8;

        keypressed = true;
      }
      if (IsKeyDown(Keys.Right)) // ------->>>
      {
        if (Player.ControllablePlayer.ChangeX < 30) // Moving Left <<--
          Player.ControllablePlayer.ChangeX += 16;
        else if (Player.ControllablePlayer.ChangeX > 40) // Moving Right -->>
          Player.ControllablePlayer.ChangeX += 4;
        else
          Player.ControllablePlayer.ChangeX += 8;

        keypressed = true;
      }
      if (IsKeyDown(Keys.Up)) //^^^^^
      {
        if (Player.ControllablePlayer.ChangeY > 30) // Moving Down vvvv
          Player.ControllablePlayer.ChangeY -= 16;
        else if (Player.ControllablePlayer.ChangeY < 40) // Moving Up ^^^^
          Player.ControllablePlayer.ChangeY -= 4;
        else
          Player.ControllablePlayer.ChangeY -= 8;

        keypressed = true;
      }
      if (IsKeyDown(Keys.Down)) //vvvvv
      {
        if (Player.ControllablePlayer.ChangeY < 30) // Moving Up ^^^^
          Player.ControllablePlayer.ChangeY += 16;
        else if (Player.ControllablePlayer.ChangeY > 40) // Moving Down ^^^^
          Player.ControllablePlayer.ChangeY += 4;
        else
          Player.ControllablePlayer.ChangeY += 8;
        keypressed = true;
      }
      if (keypressed == false)
      {
        Player.ControllablePlayer.ChangeX = Player.ControllablePlayer.ChangeX - (8 * Math.Sign(Player.ControllablePlayer.ChangeX));
        Player.ControllablePlayer.ChangeY = Player.ControllablePlayer.ChangeY - (8 * Math.Sign(Player.ControllablePlayer.ChangeY));
      }

      if (Math.Abs(Player.ControllablePlayer.ChangeX) + Math.Abs(Player.ControllablePlayer.ChangeY) > Player.ControllablePlayer.SpeedCap * 2 - 60)
      {
        Player.ControllablePlayer.ChangeX = (Player.ControllablePlayer.SpeedCap - 36) * Math.Sign(Player.ControllablePlayer.ChangeX);
        Player.ControllablePlayer.ChangeY = (Player.ControllablePlayer.SpeedCap - 36) * Math.Sign(Player.ControllablePlayer.ChangeY);
      }

      //if (Math.Abs(Player.ControllablePlayer.ChangeX) > Player.ControllablePlayer.SpeedCap - 30)
      //{
      //  Player.ControllablePlayer.ChangeX = (Player.ControllablePlayer.SpeedCap - 36) * Math.Sign(Player.ControllablePlayer.ChangeX);
      //}
      //if (Math.Abs(Player.ControllablePlayer.ChangeY) > Player.ControllablePlayer.SpeedCap - 30)
      //{
      //  Player.ControllablePlayer.ChangeY = (Player.ControllablePlayer.SpeedCap - 36) * Math.Sign(Player.ControllablePlayer.ChangeY);
      //}

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

    public void PaintScrimmageAndFirstDownLines(object sender, PaintEventArgs e) //TODO move to PlayingFieldDrawing class
    {
      // Scrimmage
      Pen pen = new Pen(Color.FromArgb(255, 128, 128, 255));
      e.Graphics.DrawLine(pen, Game.LineOfScrimagePixel, 0, Game.LineOfScrimagePixel, ParentForm.Height - 62);
      // First Down
      pen = new Pen(Color.FromArgb(255, 255, 255, 0));
      int firstDownMarker = LineOfScrimagePixel + ((int)PlayOptionsFormStats.YardsToGo * (int)PixalsInYard);
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

          // If check for ball collision, the below positions are the only one who can catch the ball
          // any other positions willl not be checked and thus the break;
          if (!(players[i] is OffenderWideReceiver) 
           && !(players[i] is DefenderCornerback) 
           && !(players[i] is DefenderMiddleLinebacker) 
           && !(players[i] is DefenderSafety) 
           && players[j].IsBall) 
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
