﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Drake.Tools;

/*-------------- To Do List ---------------
 * 
 * Play begin dialog, difficulty, decribe game situation.
 * 
 * Bug, End of game field goal didn't count.
 * 
 * Add more WR patterns
 * Penalties?
 * ---------- Discarded Changes -----------
 * Draw end zones, too much time for the graphics
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
    LossOfPossessionOnDowns,
    FieldGoal,
    FieldGoalMiss,
    Touchdown,
    Safety,
    Penalty
  }

  public class Game
  {
    private static bool _running = true;
    private static bool _timeExpired = false;
    private static bool _playEnded = false;
    private static System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer(); // For keyboard arrows
    private static PlayOptionsForm _playOptionsForm;

    public static Form1 ParentForm;
    public static Random Random = new Random();
    public static CurrentGameState CurrentGameState = new CurrentGameState();

    // Player instances
    public static BallAsPlayer ballAsPlayer = new BallAsPlayer();
    public static OffenderQuarterback offenderQuarterback = new OffenderQuarterback();
    public static OffenderOutsideLinemanTop offenderOutsideLinemanTop = new OffenderOutsideLinemanTop();
    public static OffenderLinemanUpper offenderLinemanUpper = new OffenderLinemanUpper();
    public static OffenderLinemanCenter offenderLinemanCenter = new OffenderLinemanCenter();
    public static OffenderLinemanLower offenderLinemanLower = new OffenderLinemanLower();
    public static OffenderOutsideLinemanBottom offenderOutsideLinemanBottom = new OffenderOutsideLinemanBottom();
    public static OffenderWideReceiverTop offenderWideReceiverTop = new OffenderWideReceiverTop();
    public static OffenderWideReceiverBottom offenderWideReceiverBottom = new OffenderWideReceiverBottom();
    public static DefenderCornerbackTop defenderCornerbackTop = new DefenderCornerbackTop();
    public static DefenderOutsideLinemanTop defenderOutsideLinemanTop = new DefenderOutsideLinemanTop();
    public static DefenderLinemanUpper defenderLinemanUpper = new DefenderLinemanUpper();
    public static DefenderMiddleLinebacker defenderMiddleLinebacker = new DefenderMiddleLinebacker();
    public static DefenderSafety defenderSafety = new DefenderSafety();
    public static DefenderLinemanLower defenderLinemanLower = new DefenderLinemanLower();
    public static DefenderOutsideLinemanBottom defenderOutsideLinemanBottom = new DefenderOutsideLinemanBottom();
    public static DefenderCornerbackBottom defenderCornerbackBottom = new DefenderCornerbackBottom();

    public Game(Form1 form1)
    {
      // Set Parents
      ParentForm = form1;
      Player.ParentForm = form1;
      Player.ParentGame = this;
      Scoreboard.ParentForm = form1;
      PlayingField.ParentForm = form1;

      Scoreboard.CountDownTimer.SetTime(10, 0);

      // Set initial values and Display them.
      CurrentGameState.Down = 1;
      CurrentGameState.YardsToGo = 10;
      CurrentGameState.BallOnYard100 = 20.0F; // 1 - 100
      CurrentGameState.GuestScore = 3;
      CurrentGameState.Quarter = 4;
      CurrentGameState.TimeOutsLeft = 3;
      CurrentGameState.ResetDriveState();

      // Draw the scoreboard and field.
      Scoreboard.InitializeDrawing(); // Draw the starting scoreboard
      PlayingField.InitializeDrawing(CurrentGameState.BallOnYard100); // Draw the starting sideline and other playing field objects
      Player.FieldBounds = PlayingField.FieldBounds;

      //////////////////////////
      AddPlayers();
      //////////////////////////

      _playEnded = true; // Causes PlayOptions form to be displayed from main loop

      // Getting keyboard input
      _timer.Tick += new System.EventHandler(KeyDown);
      _timer.Interval = 72;
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
      Player.AddPlayer(offenderOutsideLinemanTop, initlineX - 4, PlayingField.FieldCenterY - 104, ParentForm.Player1, VerticalPosition.PositionTop);
      Player.AddPlayer(offenderLinemanUpper, initlineX, PlayingField.FieldCenterY - 58, ParentForm.Player1, VerticalPosition.PositionMiddle);
      Player.AddPlayer(offenderLinemanCenter, initlineX, PlayingField.FieldCenterY + 2, ParentForm.Player1, VerticalPosition.PositionMiddle);
      Player.AddPlayer(offenderLinemanLower, initlineX, PlayingField.FieldCenterY + 62, ParentForm.Player1, VerticalPosition.PositionMiddle);
      Player.AddPlayer(offenderOutsideLinemanBottom, initlineX - 4, PlayingField.FieldCenterY + 106, ParentForm.Player1, VerticalPosition.PositionBottom);
      Player.AddPlayer(offenderWideReceiverBottom, initlineX, PlayingField.FieldCenterY + 220, ParentForm.Player1, VerticalPosition.PositionBottom);

      //--------------------- Defensive Players
      initlineX = PlayingField.LineOfScrimagePixel + 25; // All defensive X values

      Player.AddPlayer(defenderCornerbackTop, offenderWideReceiverTop.InitialLeft + 120, offenderWideReceiverTop.InitialTop + 30, ParentForm.Player2, VerticalPosition.PositionTop);
      Player.AddPlayer(defenderOutsideLinemanTop, initlineX, PlayingField.FieldCenterY - 152, ParentForm.Player2, VerticalPosition.PositionTop, initialOffsetY: -235);
      defenderOutsideLinemanTop.PicBox.BackColor = Color.LightGreen; // TODO take out
      Player.AddPlayer(defenderLinemanUpper, initlineX, PlayingField.FieldCenterY - 50, ParentForm.Player2, VerticalPosition.PositionTop, initialOffsetY: -85);
      //defenderLinemanUpper.PicBox.BackColor = Color.LightBlue; // TODO take out
      // Middle Linebacker
      Player.AddPlayer(defenderMiddleLinebacker, PlayingField.LineOfScrimagePixel + 120, PlayingField.FieldCenterY, ParentForm.Player2, VerticalPosition.PositionMiddle);
      defenderMiddleLinebacker.PicBox.BackColor = Color.DarkGreen; // TODO take out
      // Safety
      Player.AddPlayer(defenderSafety, PlayingField.LineOfScrimagePixel + 320, PlayingField.FieldCenterY, ParentForm.Player2, VerticalPosition.PositionMiddle);
      defenderSafety.PicBox.BackColor = Color.DarkBlue; // TODO take out
      Player.AddPlayer(defenderLinemanLower, initlineX, PlayingField.FieldCenterY + 47, ParentForm.Player2, VerticalPosition.PositionBottom, initialOffsetY: 78);
      //defenderLinemanLower.PicBox.BackColor = Color.DarkBlue; // TODO take out
      Player.AddPlayer(defenderOutsideLinemanBottom, initlineX, PlayingField.FieldCenterY + 152, ParentForm.Player2, VerticalPosition.PositionBottom, initialOffsetY: 235);
      //defenderOutsideLinemanBottom.PicBox.BackColor = Color.LightGreen; // TODO take out
      Player.AddPlayer(defenderCornerbackBottom, offenderWideReceiverBottom.InitialLeft + 120, offenderWideReceiverBottom.InitialTop - 30, ParentForm.Player2, VerticalPosition.PositionBottom);

      // Setup Initial TargetPlayers
      defenderCornerbackTop.InitialTargetPlayer = offenderWideReceiverTop;
      defenderCornerbackBottom.InitialTargetPlayer = offenderWideReceiverBottom;

      offenderOutsideLinemanTop.InitialTargetPlayer = defenderOutsideLinemanTop;
      offenderLinemanUpper.InitialTargetPlayer = defenderLinemanUpper;
      offenderLinemanLower.InitialTargetPlayer = defenderLinemanLower;
      offenderOutsideLinemanBottom.InitialTargetPlayer = defenderOutsideLinemanBottom;

      InitializePlayers(); // Initial draw
    }

    public void InitializePlayers()
    {
      defenderMiddleLinebacker.TargetPlayer = null; // Can't depend on initialization order so some things need resetting;
      defenderSafety.CoveredPlayer          = null; // Can't depend on initialization order so some things need resetting;
      Player.Players.ForEach(p => p.Initialize());
    }

    public void Run()
    {
      _timer.Start();
      while (_running)
      {
        if (_playEnded)
        {
          if (_timeExpired)
          {
            DisplayEndGameMessage();
            return;
          }
          else
          {
            ChoosePlay();
            while (PlayOptionsForm.PlayClockPenalty)
            {
              Game.EndPlay(EndPlayType.Penalty, null, "Play clock ran down. 5 yard penalty.", -5);
              Thread.Sleep(1000);
              PlayingField.DrawField(CurrentGameState.BallOnYard100);
              ParentForm.Invalidate();
              ChoosePlay();
            }

            PlayingField.DrawField(CurrentGameState.BallOnYard100);
            ParentForm.Invalidate();

            if (_timeExpired)
            {
              DisplayEndGameMessage();
              return;
            }
          }

          if (!_running) // Game ended while chosing a play.
            return;

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

        Thread.Sleep(20);  // Speed of the game, increase for easy mode
      }
    }

    public static void EndGame()
    {
      _timeExpired = true;
      Scoreboard.CountDownTimer.SetTime(0, 0);
      PlayOptionsForm.CountDownTimer.Stop();
      Scoreboard.DisplayClock();
      _playOptionsForm.Close();
      Scoreboard.ScrollMessage("Game Ended");
    }

    public static void DisplayEndGameMessage()
    {
      _running = false;
      _timer.Stop();
      Scoreboard.ScrollTimer.Stop();
      _playOptionsForm.Close();

      string message = "Game Ended.\n";
      if (CurrentGameState.HomeScore > CurrentGameState.GuestScore)
        message += "Bears Won!\n";
      else if (CurrentGameState.HomeScore < CurrentGameState.GuestScore)
        message += "Bears Lost.\n";
      else
        message += "Tie Game.\n";
      MessageBox.Show(message, "Game Ended", MessageBoxButtons.OK);
    }

    private static void ChoosePlay()
    {
      _playOptionsForm = new PlayOptionsForm();
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
          if (!_timeExpired)
            ChoosePlay();
          break;
        case PlayOptionsForm.PlayOptionType.FieldGoal:
          Player.ThrowingType = Player.ThrowType.FieldGoal;
          break;
      }

      if (!_timeExpired)
      {
        Scoreboard.CountDownTimer.Start();
      }
    }

    public static void EndPlay(EndPlayType endPlayType, Player tackledBy, string message, int penaltyYards = 0)
    {
      CurrentGameState.FirstDown = false;
      if (CurrentGameState.DrivePlays == 0)
        CurrentGameState.DriveStartYard = CurrentGameState.BallOnYard100;

      if (_playEnded && penaltyYards == 0) // Play ended by another player
        return;
      else
        _playEnded = true;

      // Stopping the clock   
      if (endPlayType != EndPlayType.Tackled)
        Scoreboard.CountDownTimer.Pause();


      CurrentGameState.YardsGained = 0;
      CurrentGameState.TackledBy = tackledBy;

      if (Player.ControllablePlayer.Left + 30 > PlayingField.PixelFromYard(100)) // TouchDown, (30 is tip of ball)
      {
        endPlayType = EndPlayType.Touchdown;
        CurrentGameState.YardsGained = 100 - CurrentGameState.BallOnYard100;
      }

      if(penaltyYards == 0)
      {
        CurrentGameState.DrivePlays++;
        CurrentGameState.Down++;
      }
      else // Penalty
      {
        Player.ControllablePlayer.Left = PlayingField.PixelFromYard((int)CurrentGameState.BallOnYard100 + penaltyYards) - 30;
      }

    ReevaluateEndPlayCase:
      switch (endPlayType)
      {
        // These 2 cases below are the only cases for yards gained / loss and also a first down made.
        case EndPlayType.Tackled:
        case EndPlayType.OutOfBounds:
        case EndPlayType.Penalty:
          //---- Safty
          if (Player.ControllablePlayer.Left + 30 < PlayingField.PixelFromYard(0)) // Safety, (28 is tip of ball)
          {
            CurrentGameState.GuestScore += 2;
            CurrentGameState.YardsGained = 0;
            CurrentGameState.BallOnYard100 = 20;
            CurrentGameState.YardsToGo = 10;
            CurrentGameState.Down = 1;
            Scoreboard.DisplayGuestScore(CurrentGameState.GuestScore);
            message = "Safety.";
            Scoreboard.ScrollMessage(message);
          }
          else
          {
            //////////////////////
            Update_TackledAt_YardsGained_BallOnYard100_YardsToGo_FirstDown();
            //////////////////////
          }
          break;
        case EndPlayType.Punted:
        case EndPlayType.Intercepted:
        case EndPlayType.LossOfPossessionOnDowns:
          if (endPlayType == EndPlayType.LossOfPossessionOnDowns)
            CurrentGameState.YardsGained -= (Random.Next(0, 26));
          else if (endPlayType == EndPlayType.Intercepted)
            CurrentGameState.YardsGained = PlayingField.YardsFromLineOfScrimage(ballAsPlayer.Left) - Random.Next(20, 46); // includes random punt back
          else if (CurrentGameState.BallOnYard100 < 20)
            CurrentGameState.YardsGained -= (12 + Random.Next(0, 6));
          else
            CurrentGameState.YardsGained -= (16 + Random.Next(0, 6));

          CurrentGameState.BallOnYard100 += CurrentGameState.YardsGained;

          if (CurrentGameState.BallOnYard100 < 12)
          {
            if (CurrentGameState.BallOnYard100 < 3 && Scoreboard.CountDownTimer.TimeLeft.TotalSeconds > 60)
            {
              // Other team scored.
              if (CurrentGameState.BallOnYard100 < 3 && Scoreboard.CountDownTimer.TimeLeft.TotalSeconds > 60)
              {
                message += "\nGuest scored a touchdown.";
                CurrentGameState.GuestScore += 7;
              }
              else
              {
                message += "\nGuest scored a field goal.";
                CurrentGameState.GuestScore += 3;
              }
            }
            CurrentGameState.YardsGained = 0;
            CurrentGameState.BallOnYard100 = 20;
            Scoreboard.ScrollMessage(message);
            Scoreboard.DisplayGuestScore(CurrentGameState.GuestScore);
          }
          else
          {
            // "Intercepted" or "Punted" or "LossOfPossessionOnDowns"  
            if (endPlayType == EndPlayType.LossOfPossessionOnDowns)
              message = "Loss of possession on downs.\nA loss of " + Math.Abs(CurrentGameState.YardsGained).ToString("0.0") + " yards on change of possesion.";
            else
            {
              CurrentGameState.TackledBy = null;
              message = Enum.GetName(typeof(EndPlayType), endPlayType) + ".\nA loss of " + Math.Abs(CurrentGameState.YardsGained).ToString("0.0") + " yards on change of possesion.";
            }
          }
          CurrentGameState.YardsToGo = 10;
          CurrentGameState.Down = 1;

          UpdateClockAndTimeoutsForChangeOfPossession();
          break;
        case EndPlayType.FieldGoal:
        case EndPlayType.FieldGoalMiss:
        case EndPlayType.Touchdown:
          if (endPlayType == EndPlayType.FieldGoal)
            CurrentGameState.HomeScore += 3;
          if (endPlayType == EndPlayType.Touchdown)
            CurrentGameState.HomeScore += 7;

          CurrentGameState.TackledBy = null;
          CurrentGameState.BallOnYard100 = 20;
          CurrentGameState.YardsToGo = 10;
          CurrentGameState.Down = 1;
          Scoreboard.DisplayBearsScore(CurrentGameState.HomeScore);
          Scoreboard.ScrollMessage(message);

          UpdateClockAndTimeoutsForChangeOfPossession();
          break;
      }

      if (CurrentGameState.Down > 4 && !CurrentGameState.FirstDown) // Loss of possession on downs if not first down.
      {
        endPlayType = EndPlayType.LossOfPossessionOnDowns;
        goto ReevaluateEndPlayCase; //GOTO ===^^^^
      }

      CurrentGameState.ResultsOfLastPlay = message;

      if (CurrentGameState.FirstDown)
      {
        if (CurrentGameState.BallOnYard100 > 90)
          CurrentGameState.YardsToGo = 100 - CurrentGameState.BallOnYard100;
        else
          CurrentGameState.YardsToGo = 10;

        CurrentGameState.Down = 1;
        CurrentGameState.ResultsOfLastPlay += "  First Down!";
        CurrentGameState.DriveFirstDowns++;
        Scoreboard.ScrollMessage("First Down!");
      }

      //if(endPlayType == EndPlayType.Penalty)
      //  ChoosePlay();

      Scoreboard.DisplayBallOn(CurrentGameState.BallOnYard);
      Scoreboard.DisplayToGo(CurrentGameState.YardsToGo.ToString("00"));
      Scoreboard.DisplayDown(CurrentGameState.Down.ToString("0"));
    }

    private static void UpdateClockAndTimeoutsForChangeOfPossession()
    {
      if (Scoreboard.CountDownTimer.TimeLeft.TotalSeconds > 240) // 4 minutes left
        Scoreboard.CountDownTimer.SubtractTime(2, 0);
      else if (Scoreboard.CountDownTimer.TimeLeft.TotalSeconds > 160) // 2:40 seconds left, subtract 1:28
        Scoreboard.CountDownTimer.SubtractTime(1, 28); // Subtract 88 second off the clock, 2 minute warning was employed
      else
      {
        if (Scoreboard.CountDownTimer.TimeLeft.TotalSeconds > 120 && PlayOptionsForm.TimeOutsLeft > 0)
        {
          Scoreboard.CountDownTimer.SubtractTime(0, 58); // Subtract 58 seconds off the clock, 2 minute warning was employed, only 1 timeout spent.
          MessageBox.Show("Needed to use a timeout to stop the clock.", "Time Outs Used", MessageBoxButtons.OK);
          PlayOptionsForm.TimeOutsLeft--;
        }
        else if (Scoreboard.CountDownTimer.TimeLeft.TotalSeconds > 80 && PlayOptionsForm.TimeOutsLeft > 1)
        {
          Scoreboard.CountDownTimer.SubtractTime(0, 59); // Subtract 60 seconds off the clock
          MessageBox.Show("Needed to use 2 timeouts to stop the clock.", "Time Outs Used", MessageBoxButtons.OK);
          PlayOptionsForm.TimeOutsLeft -= 2;
        }
        else if (Scoreboard.CountDownTimer.TimeLeft.TotalSeconds > 50 && PlayOptionsForm.TimeOutsLeft > 2)
        {
          Scoreboard.CountDownTimer.SubtractTime(0, 29); // Subtract 60 seconds off the clock
          MessageBox.Show("Needed to use 3 timeouts to stop the clock.", "Time Outs Used", MessageBoxButtons.OK);
          PlayOptionsForm.TimeOutsLeft -= 3;
        }
        else
        {
          EndGame();
        }
        if(Scoreboard.CountDownTimer.TimeLeft.TotalSeconds < 10)
        {
          EndGame();
        }
      }
      CurrentGameState.ResetDriveState();
    }

    private static void Update_TackledAt_YardsGained_BallOnYard100_YardsToGo_FirstDown()
    {
      CurrentGameState.TackledAt100 = GetTackledAt();
      CurrentGameState.YardsGained = CurrentGameState.TackledAt100 - CurrentGameState.BallOnYard100;
      CurrentGameState.BallOnYard100 = CurrentGameState.TackledAt100;
      CurrentGameState.YardsToGo -= CurrentGameState.YardsGained;
      CurrentGameState.FirstDown = CurrentGameState.YardsToGo < .01;
    }

    private static float GetTackledAt()
    {
      return PlayingField.YardFromPixel(Player.ControllablePlayer.Left + 30);
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
        if(Player.ControllablePlayer.ChangeX > 16) // Moving Right -->> // Easier to hit the brakes and change speed.
          Player.ControllablePlayer.ChangeX -= 24;
        else if (Player.ControllablePlayer.ChangeX < 40) // Moving Left <<-- // Harder to gain speed when approching max speed.
          Player.ControllablePlayer.ChangeX -= 4;
        else
          Player.ControllablePlayer.ChangeX -= 12; // Normal speed change

        keypressed = true;
      }
      if (IsKeyDown(Keys.Right)) // ------->>>
      {
        if (Player.ControllablePlayer.ChangeX < -16) // Moving Left <<--
          Player.ControllablePlayer.ChangeX += 24;
        else if (Player.ControllablePlayer.ChangeX > 40) // Moving Right -->>
          Player.ControllablePlayer.ChangeX += 4;
        else
          Player.ControllablePlayer.ChangeX += 12;

        keypressed = true;
      }
      if (IsKeyDown(Keys.Up)) //^^^^^
      {
        if (Player.ControllablePlayer.ChangeY > 16) // Moving Down vvvv
          Player.ControllablePlayer.ChangeY -= 24;
        else if (Player.ControllablePlayer.ChangeY < 40) // Moving Up ^^^^
          Player.ControllablePlayer.ChangeY -= 6;
        else
          Player.ControllablePlayer.ChangeY -= 12;

        keypressed = true;
      }
      if (IsKeyDown(Keys.Down)) //vvvvv
      {
        if (Player.ControllablePlayer.ChangeY < -16) // Moving Up ^^^^
          Player.ControllablePlayer.ChangeY += 24;
        else if (Player.ControllablePlayer.ChangeY > 40) // Moving Down ^^^^
          Player.ControllablePlayer.ChangeY += 6;
        else
          Player.ControllablePlayer.ChangeY += 12;
        keypressed = true;
      }
      if (keypressed == false)
      {
        Player.ControllablePlayer.ChangeX = Player.ControllablePlayer.ChangeX - (8 * Math.Sign(Player.ControllablePlayer.ChangeX));
        Player.ControllablePlayer.ChangeY = Player.ControllablePlayer.ChangeY - (8 * Math.Sign(Player.ControllablePlayer.ChangeY));
      }

      // Lessen the speed if near maxing x and Y movement.
      int totalMovement = Math.Abs(Player.ControllablePlayer.ChangeX) + Math.Abs(Player.ControllablePlayer.ChangeY); // total movement x + y
      if (totalMovement > Player.ControllablePlayer.SpeedCap * 2 - 36)
      {
        if (totalMovement > Player.ControllablePlayer.SpeedCap * 2 - 16)
        {
          Player.ControllablePlayer.ChangeX = (Player.ControllablePlayer.SpeedCap - 10) * Math.Sign(Player.ControllablePlayer.ChangeX);
          Player.ControllablePlayer.ChangeY = (Player.ControllablePlayer.SpeedCap - 10) * Math.Sign(Player.ControllablePlayer.ChangeY);
        }
        else
        {
          Player.ControllablePlayer.ChangeX = (Player.ControllablePlayer.SpeedCap - 16) * Math.Sign(Player.ControllablePlayer.ChangeX);
          Player.ControllablePlayer.ChangeY = (Player.ControllablePlayer.SpeedCap - 16) * Math.Sign(Player.ControllablePlayer.ChangeY);
        }
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
