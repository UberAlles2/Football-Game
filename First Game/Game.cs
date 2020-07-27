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
 * 
 * 
 * Add more WR patterns
 * 
 * Draw end zones 
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
    Intercepted,
    Punted
  }

  public class Game
  {
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
    public static CurrentGameState CurrentGameState = new CurrentGameState();

    public static OffenderWideReceiverTop offenderWideReceiverTop = new OffenderWideReceiverTop();
    public static OffenderWideReceiverBottom offenderWideReceiverBottom = new OffenderWideReceiverBottom();

    public Game(Form1 form1)
    {
      // Set Parents
      ParentForm = form1;
      Player.ParentForm = form1;
      Player.ParentGame = this;
      Scoreboard.ParentForm = form1;
      DrawPlayingField.ParentForm = form1;

      // Set initial values and Display them.
      CurrentGameState.Down = 1;
      CurrentGameState.YardsToGo = 10;
      CurrentGameState.BallOnYard = 1; // 1 - 50
      CurrentGameState.BallOnYard100 = 1;
      Scoreboard.InitializeDrawing(); // Draw the starting scoreboard
      DrawPlayingField.InitializeDrawing(CurrentGameState.BallOnYard100);   // Draw the starting sideline

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
          if(CurrentGameState.Down == 4)
          {
            DialogResult result =  MessageBox.Show("Punt?", "Football", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if(result == DialogResult.Yes)
            {
              PlayEnded = false;
              EndPlay(EndPlayType.Punted, null, "");
            }
          }

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
        Player.CheckCollisions();

        Thread.Sleep(30);  // Speed of the game, increase for easy mode
      }
    }

    private void ChoosePlay()
    {
      PlayOptionsForm = new PlayOptionsForm(this);
      PlayOptionsForm.Location = new Point(ParentForm.Left + 660, ParentForm.Top + 160);
      PlayOptionsForm.ShowDialog();
      Scoreboard.CountDownTimer.Start();
    }

    public void EndPlay(EndPlayType endPlayType, Player tackledBy, string message)
    {
      if (PlayEnded) // Play ended by another player
        return;
      else 
        PlayEnded = true;

      // Stopping the clock   
      if (endPlayType != EndPlayType.Tackled || endPlayType != EndPlayType.Intercepted || endPlayType != EndPlayType.Punted || endPlayType == EndPlayType.OutOfBounds)
        Scoreboard.CountDownTimer.Stop();

      CurrentGameState.YardsGained = 0;
      if (endPlayType == EndPlayType.Tackled || endPlayType == EndPlayType.OutOfBounds)
      {
        CurrentGameState.YardsGained = (float)(Player.ControllablePlayer.Left + Player.ControllablePlayer.PicBox.Width - LineOfScrimagePixel) / PixalsInYard;
        CurrentGameState.TackledBy = tackledBy;
        CurrentGameState.BallOnYard100 += CurrentGameState.YardsGained;
        CurrentGameState.YardsToGo -= CurrentGameState.YardsGained;
      }
      else if (endPlayType == EndPlayType.Punted)
      {
        if(CurrentGameState.BallOnYard < 20)
          CurrentGameState.YardsGained -= (13 + Random.Next(0, 5));
        else
          CurrentGameState.YardsGained -= (18 + Random.Next(0, 5));

        CurrentGameState.TackledBy = null;
        CurrentGameState.BallOnYard100 += (CurrentGameState.YardsGained);
        CurrentGameState.YardsToGo = 10;
        CurrentGameState.Down = 0;
        message = "Punted, a loss of " + Math.Abs(CurrentGameState.YardsGained).ToString("00") + " yards on change of possesion."; 
      }
      else
        CurrentGameState.TackledBy = null;

      CurrentGameState.ResultsOfLastPlay = message;

      if (CurrentGameState.BallOnYard100 < 0)
      {
        CurrentGameState.BallOnYard100 = 0; // Safety
      }
      if (CurrentGameState.BallOnYard100 > 100)
      {
        CurrentGameState.BallOnYard100 = 100; // Touchdown
      }

      if (CurrentGameState.Down < 4)
        CurrentGameState.Down++;

      if(CurrentGameState.YardsToGo < 0.05)
      {
        CurrentGameState.YardsToGo = 10;
        CurrentGameState.Down = 1;
        CurrentGameState.ResultsOfLastPlay += "  First Down!";
      }
      CurrentGameState.BallOnYard = CurrentGameState.BallOnYard100 < 50 ? CurrentGameState.BallOnYard100 : 100 - CurrentGameState.BallOnYard100;

      Scoreboard.DisplayBallOn(CurrentGameState.BallOnYard.ToString("00"));
      Scoreboard.DisplayToGo(CurrentGameState.YardsToGo.ToString("00"));
      Scoreboard.DisplayDown(CurrentGameState.Down.ToString("0"));

      DrawPlayingField.DrawField(CurrentGameState.BallOnYard100);

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

      if (PlayEnded) // Stop the player from moving after play ends.
      {
        Player.ControllablePlayer.ChangeX = 0; 
        Player.ControllablePlayer.ChangeY = 0;
        return;
      }

      if (IsKeyDown(Keys.Left)) // <<<-------
      {
        if(Player.ControllablePlayer.ChangeX > 30) // Moving Right -->> // Easier to hit the brakes and change speed.
          Player.ControllablePlayer.ChangeX -= 16;
        else if (Player.ControllablePlayer.ChangeX < 40) // Moving Left <<-- // Harder to gain speed when approching max speed.
          Player.ControllablePlayer.ChangeX -= 4;
        else
          Player.ControllablePlayer.ChangeX -= 8; // Normal speed change

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

      // lessen the speed if near maxing x and Y movement.
      if (Math.Abs(Player.ControllablePlayer.ChangeX) + Math.Abs(Player.ControllablePlayer.ChangeY) > Player.ControllablePlayer.SpeedCap * 2 - 60)
      {
        Player.ControllablePlayer.ChangeX = (Player.ControllablePlayer.SpeedCap - 36) * Math.Sign(Player.ControllablePlayer.ChangeX);
        Player.ControllablePlayer.ChangeY = (Player.ControllablePlayer.SpeedCap - 36) * Math.Sign(Player.ControllablePlayer.ChangeY);
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
  }
}
