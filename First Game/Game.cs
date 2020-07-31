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
 * Add more WR patterns
 * 
 * Draw end zones 
 * Timeouts
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
    Punted,
    FieldGoal,
    FieldGoalMiss,
    Touchdown,
    Safety
  }

  public class Game
  {
    private static bool _running = true;
    private static bool _playEnded = false;
    private static System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer(); // For keyboard arrows
    private static PlayOptionsForm _playOptionsForm;

    public static Form1 ParentForm;
    public static Random Random = new Random();
    public static CurrentGameState CurrentGameState = new CurrentGameState();

    // Player instances
    public static BallAsPlayer                 ballAsPlayer = new BallAsPlayer();
    public static OffenderQuarterback          offenderQuarterback = new OffenderQuarterback();
    public static OffenderOutsideLinemanTop    offenderOutsideLinemanTop = new OffenderOutsideLinemanTop();
    public static OffenderLinemanUpper         offenderLinemanUpper = new OffenderLinemanUpper();
    public static OffenderLinemanCenter        offenderLinemanCenter = new OffenderLinemanCenter();
    public static OffenderLinemanLower         offenderLinemanLower = new OffenderLinemanLower();
    public static OffenderOutsideLinemanBottom offenderOutsideLinemanBottom = new OffenderOutsideLinemanBottom();
    public static OffenderWideReceiverTop      offenderWideReceiverTop = new OffenderWideReceiverTop();
    public static OffenderWideReceiverBottom   offenderWideReceiverBottom = new OffenderWideReceiverBottom();
    public static DefenderCornerbackTop        defenderCornerbackTop = new DefenderCornerbackTop();
    public static DefenderOutsideLinemanTop    defenderOutsideLinemanTop = new DefenderOutsideLinemanTop();
    public static DefenderLinemanUpper         defenderLinemanUpper = new DefenderLinemanUpper();
    public static DefenderMiddleLinebacker     defenderMiddleLinebacker = new DefenderMiddleLinebacker();
    public static DefenderSafety               defenderSafety = new DefenderSafety();
    public static DefenderLinemanLower         defenderLinemanLower = new DefenderLinemanLower();
    public static DefenderOutsideLinemanBottom defenderOutsideLinemanBottom = new DefenderOutsideLinemanBottom();
    public static DefenderCornerbackBottom     defenderCornerbackBottom = new DefenderCornerbackBottom();

    public Game(Form1 form1)
    {
      // Set Parents
      ParentForm = form1;
      Player.ParentForm = form1;
      Player.ParentGame = this;
      Scoreboard.ParentForm = form1;
      PlayingField.ParentForm = form1;

      // Set initial values and Display them.
      CurrentGameState.Down = 4;
      CurrentGameState.YardsToGo = 10;
      CurrentGameState.BallOnYard100 = 8.2F; // 1 - 100

      // Draw the scoreboard and field.
      Scoreboard.InitializeDrawing(); // Draw the starting scoreboard
      PlayingField.InitializeDrawing(CurrentGameState.BallOnYard100); // Draw the starting sideline and other playing field objects
      Player.FieldBounds = PlayingField.FieldBounds;

      AddPlayers();

      _playEnded = true; // Causes PlayOptions form to be displayed from main loop

      // Getting keyboard input
      _timer.Tick += new System.EventHandler(KeyDown);
      _timer.Interval = 50;
    }

    public void AddPlayers()
    {
      // Ball as a Player
      Player.AddPlayer(ballAsPlayer, -999, -999, ParentForm.picFootball);

      //--------------------- Offensive Players
      int initlineX = PlayingField.LineOfScrimagePixel - 25; // All offensive X values

      Player.AddPlayer(offenderQuarterback, PlayingField.LineOfScrimagePixel - 200, PlayingField.FieldCenterY, ParentForm.Player1);
      Player.ControllablePlayer = offenderQuarterback;

      Player.AddPlayer(offenderWideReceiverTop, initlineX, PlayingField.FieldCenterY - 220, ParentForm.Player1, VerticalPosition.PositionTop);
      Player.AddPlayer(offenderOutsideLinemanTop, initlineX, PlayingField.FieldCenterY - 96, ParentForm.Player1, VerticalPosition.PositionTop);
      Player.AddPlayer(offenderLinemanUpper, initlineX, PlayingField.FieldCenterY - 52, ParentForm.Player1, VerticalPosition.PositionMiddle);
      Player.AddPlayer(offenderLinemanCenter, initlineX, PlayingField.FieldCenterY, ParentForm.Player1, VerticalPosition.PositionMiddle);
      Player.AddPlayer(offenderLinemanLower, initlineX, PlayingField.FieldCenterY + 56, ParentForm.Player1, VerticalPosition.PositionMiddle);
      Player.AddPlayer(offenderOutsideLinemanBottom, initlineX, PlayingField.FieldCenterY + 96, ParentForm.Player1, VerticalPosition.PositionBottom);
      Player.AddPlayer(offenderWideReceiverBottom, initlineX, PlayingField.FieldCenterY + 220, ParentForm.Player1, VerticalPosition.PositionBottom);

      //--------------------- Defensive Players
      initlineX = PlayingField.LineOfScrimagePixel + 25; // All defensive X values

      Player.AddPlayer(defenderCornerbackTop, offenderWideReceiverTop.InitialLeft + 160, offenderWideReceiverTop.InitialTop + 30, ParentForm.Player2, VerticalPosition.PositionTop);
      Player.AddPlayer(defenderOutsideLinemanTop, initlineX, PlayingField.FieldCenterY - 152, ParentForm.Player2, VerticalPosition.PositionTop, initialOffsetY: -235);
      defenderOutsideLinemanTop.PicBox.BackColor = Color.LightGreen; // TODO take out
      Player.AddPlayer(defenderLinemanUpper, initlineX, PlayingField.FieldCenterY - 50, ParentForm.Player2, VerticalPosition.PositionTop, initialOffsetY: -85);
      defenderLinemanUpper.PicBox.BackColor = Color.LightBlue; // TODO take out
        // Middle Linebacker
        Player.AddPlayer(defenderMiddleLinebacker, PlayingField.LineOfScrimagePixel + 120, PlayingField.FieldCenterY, ParentForm.Player2, VerticalPosition.PositionMiddle);
        defenderMiddleLinebacker.PicBox.BackColor = Color.DarkGreen; // TODO take out
        // Safety
        Player.AddPlayer(defenderSafety, PlayingField.LineOfScrimagePixel + 420, PlayingField.FieldCenterY, ParentForm.Player2, VerticalPosition.PositionMiddle);
        defenderSafety.PicBox.BackColor = Color.HotPink; // TODO take out
      Player.AddPlayer(defenderLinemanLower, initlineX, PlayingField.FieldCenterY + 47, ParentForm.Player2, VerticalPosition.PositionBottom, initialOffsetY: 78);
      defenderLinemanLower.PicBox.BackColor = Color.DarkBlue; // TODO take out
      Player.AddPlayer(defenderOutsideLinemanBottom, initlineX, PlayingField.FieldCenterY + 152, ParentForm.Player2, VerticalPosition.PositionBottom, initialOffsetY: 235);
      defenderOutsideLinemanBottom.PicBox.BackColor = Color.LightGreen; // TODO take out
      Player.AddPlayer(defenderCornerbackBottom, offenderWideReceiverBottom.InitialLeft + 160, offenderWideReceiverBottom.InitialTop - 30, ParentForm.Player2, VerticalPosition.PositionBottom);

      // Setup Initial TargetPlayers
      defenderCornerbackTop.InitialTargetPlayer        = offenderWideReceiverTop;
      defenderCornerbackBottom.InitialTargetPlayer     = offenderWideReceiverBottom;

      offenderOutsideLinemanTop.InitialTargetPlayer    = defenderOutsideLinemanTop;
      offenderLinemanUpper.InitialTargetPlayer         = defenderLinemanUpper;
      offenderLinemanLower.InitialTargetPlayer         = defenderLinemanLower;
      offenderOutsideLinemanBottom.InitialTargetPlayer = defenderOutsideLinemanBottom;

      InitializePlayers(); // Initial draw
    }

    public void InitializePlayers()
    {
      Player.Players.ForEach(p => p.Initialize());
    }

    public void Run()
    {
      _timer.Start();
      while (_running)
      {
        if(_playEnded)
        {
           ChoosePlay();
          InitializePlayers();
          _playEnded = false;
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
      _playOptionsForm = new PlayOptionsForm(this);
      _playOptionsForm.Location = new Point(ParentForm.Left + 660, ParentForm.Top + 160);
      _playOptionsForm.ShowDialog();

      switch (PlayOptionsForm.PlayOption)
      {
        case PlayOptionsForm.PlayOptionType.NormalPlay:
          Player.ThrowingType = Player.ThrowType.Throw;
          break;
        case PlayOptionsForm.PlayOptionType.Punt:
          Player.ThrowingType = Player.ThrowType.Punt; // Not used for anything yet.
          _playEnded = false;
          EndPlay(EndPlayType.Punted, null, ""); // takes yardage away and possession goes back to offense
          ChoosePlay();
          break;
        case PlayOptionsForm.PlayOptionType.FieldGoal:
          Player.ThrowingType = Player.ThrowType.FieldGoal;
          break;
      }

      Scoreboard.CountDownTimer.Start();
    }

    public void EndPlay(EndPlayType endPlayType, Player tackledBy, string message)
    {
      if (_playEnded) // Play ended by another player
        return;
      else 
        _playEnded = true;

      // Stopping the clock   
      if (endPlayType != EndPlayType.Tackled)
        Scoreboard.CountDownTimer.Pause();
         

      CurrentGameState.YardsGained = 0;
      if (endPlayType == EndPlayType.Tackled || endPlayType == EndPlayType.OutOfBounds)
      {
        CurrentGameState.YardsGained = (float)(Player.ControllablePlayer.Left + Player.ControllablePlayer.PicBox.Width - PlayingField.LineOfScrimagePixel) / PlayingField.PixalsInYard;
        CurrentGameState.TackledBy = tackledBy;
        CurrentGameState.BallOnYard100 += CurrentGameState.YardsGained;
        CurrentGameState.YardsToGo -= CurrentGameState.YardsGained;
      }
      else if (endPlayType == EndPlayType.Punted)
      {
        if(CurrentGameState.BallOnYard100 < 20)
          CurrentGameState.YardsGained -= (13 + Random.Next(0, 5));
        else
          CurrentGameState.YardsGained -= (18 + Random.Next(0, 5));

        CurrentGameState.TackledBy = null;
        CurrentGameState.BallOnYard100 += (CurrentGameState.YardsGained);
        CurrentGameState.YardsToGo = 10;
        CurrentGameState.Down = 0;
        message = "Punted, a loss of " + Math.Abs(CurrentGameState.YardsGained).ToString("00") + " yards on change of possesion."; 
      }
      if (endPlayType == EndPlayType.FieldGoal || endPlayType == EndPlayType.FieldGoalMiss)
      {
        if (endPlayType == EndPlayType.FieldGoal)
          CurrentGameState.HomeScore += 3;

        CurrentGameState.YardsGained = 0;
        CurrentGameState.TackledBy = null;
        CurrentGameState.BallOnYard100 = 20;
        CurrentGameState.YardsToGo = 10;
        CurrentGameState.Down = 0;
        Scoreboard.DisplayBearsScore(CurrentGameState.HomeScore.ToString(" 0"));
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
        Scoreboard.ScrollMessage("First Down!");
      }

      Scoreboard.DisplayBallOn(CurrentGameState.BallOnYard.ToString("00"));
      Scoreboard.DisplayToGo(CurrentGameState.YardsToGo.ToString("00"));
      Scoreboard.DisplayDown(CurrentGameState.Down.ToString("0"));

      PlayingField.DrawField(CurrentGameState.BallOnYard100);

      ParentForm.Invalidate();
    }

    public void Stop()
    {
      _timer.Stop();
      _running = false;
    }

    //----------------------------------------------------------------------------------------------------------
    //                                     Keyboard and Mouse Input
    public void KeyDown(object sender, EventArgs e)
    {
      bool keypressed = false;

      if (_playEnded) // Stop the player from moving after play ends.
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

      // Lessen the speed if near maxing x and Y movement.
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

    public void MouseClick(object sender, MouseEventArgs e, Player player)
    {
      if (e.Button == MouseButtons.Left)
      {
        // Pass the ball. Ball is really another Player object.
        if(!Player.IsThrowingOrKicking)
          ballAsPlayer.ThrowBall(Player.ControllablePlayer.Left + 16, Player.ControllablePlayer.Top + 16, e.Location.Y, e.Location.X);   
      }
    }
  }
}
