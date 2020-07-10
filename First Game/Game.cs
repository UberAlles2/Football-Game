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
 * 
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

  public class Game
  {
    public static Form1 ParentForm;
    public static Player ControllablePlayer = new Player();
    public static BallAsPlayer ballAsPlayer = new BallAsPlayer();
    public static int FieldCenterY;
    public static int LineOfScrimage = 200;
    public static Random Random = new Random();

    private static float PixalsInYard = 32;
    private static float yardsGained;
    private static bool running = true;
    private static bool reintialize = false;
    private static System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

    public Game(Form1 form1)
    {
      ParentForm = form1;
      Player.ParentForm = form1;
      Player.ParentGame = this;
      FieldCenterY = ParentForm.Height / 2 - 32;

      Graphics gdi = ParentForm.CreateGraphics();
      gdi.DrawLine(Pens.Red, new Point(0, 0), new Point(100, 100));

      AddPlayers();

      timer.Tick += new System.EventHandler(KeyDown);
      timer.Interval = 52;
    }

    public void AddPlayers()
    {
      // Offensive Players
      OffenderQuarterback offenderQuarterback = new OffenderQuarterback();
      offenderQuarterback.InitialTop = FieldCenterY;
      offenderQuarterback.InitialLeft = 10;
      offenderQuarterback.PicBox = AddPlayerPictureBox(ParentForm.Player1);
      offenderQuarterback.Initialize();
      Player.AddPlayer(offenderQuarterback);
      ControllablePlayer = offenderQuarterback; 

      OffenderLinemanMiddle offenderLinemanMiddle = new OffenderLinemanMiddle();
      offenderLinemanMiddle.InitialTop = FieldCenterY;
      offenderLinemanMiddle.InitialLeft = LineOfScrimage - 25;
      offenderLinemanMiddle.PicBox = AddPlayerPictureBox(ParentForm.Player1);
      offenderLinemanMiddle.Initialize();
      Player.AddPlayer(offenderLinemanMiddle);

      OffenderLinemanTop offenderLinemanTop = offenderLinemanMiddle.CloneAndUpcast<OffenderLinemanTop, OffenderLineman>();
      offenderLinemanTop.InitialTop = FieldCenterY - 80;
      offenderLinemanTop.PicBox = AddPlayerPictureBox(ParentForm.Player1);
      offenderLinemanTop.Initialize();
      Player.AddPlayer(offenderLinemanTop);

      OffenderLinemanBottom offenderLinemanBottom = offenderLinemanTop.CloneAndUpcast<OffenderLinemanBottom, OffenderLineman>();
      offenderLinemanBottom.InitialTop = FieldCenterY + 80;
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


      // Defensive Players
      DefenderMiddleLineman defenderMiddleLineman = new DefenderMiddleLineman(); 
      defenderMiddleLineman.InitialTop = FieldCenterY;
      defenderMiddleLineman.InitialLeft = LineOfScrimage + 25;
      defenderMiddleLineman.PicBox = AddPlayerPictureBox(ParentForm.Player2);
      defenderMiddleLineman.Initialize();
      Player.AddPlayer(defenderMiddleLineman);

      DefenderOutsideLinemanTop defenderOutsideLinemanTop = defenderMiddleLineman.CloneAndUpcast<DefenderOutsideLinemanTop, Player>();
      defenderOutsideLinemanTop.InitialOffset = -66;
      defenderOutsideLinemanTop.InitialLeft = LineOfScrimage + 25;
      defenderOutsideLinemanTop.InitialTop = FieldCenterY + defenderOutsideLinemanTop.Offset - 20;
      defenderOutsideLinemanTop.PicBox = AddPlayerPictureBox(ParentForm.Player2);
      defenderOutsideLinemanTop.PicBox.BackColor = Color.LightGreen;
      defenderOutsideLinemanTop.CoDefender = defenderMiddleLineman;
      defenderOutsideLinemanTop.Initialize();
      Player.AddPlayer(defenderOutsideLinemanTop);

      DefenderOutsideLinemanBottom defenderOutsideLinemanBottom = defenderOutsideLinemanTop.CloneAndUpcast<DefenderOutsideLinemanBottom, DefenderOutsideLineman>();
      defenderOutsideLinemanBottom.InitialOffset = 66;
      defenderOutsideLinemanBottom.InitialTop = FieldCenterY + defenderOutsideLinemanBottom.Offset + 20;
      defenderOutsideLinemanBottom.PicBox = AddPlayerPictureBox(ParentForm.Player2);
      defenderOutsideLinemanBottom.PicBox.BackColor = Color.LightGreen;
      defenderOutsideLinemanBottom.CoDefender = defenderMiddleLineman;
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
        if(reintialize)
        {
          ReinitializePlayers();
          reintialize = false;
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

    public void EndPlay(string message)
    {
      reintialize = true;
      yardsGained = (float)(ControllablePlayer.CenterX - LineOfScrimage) / PixalsInYard;
      MessageBox.Show(message + Environment.NewLine + $"{yardsGained, 0:#.#} yards gained.");
    }

    public void Stop()
    {
      timer.Stop();
      running = false;
    }

    public void KeyDown(object sender, EventArgs e)
    {
      bool keypressed = false;

      if (reintialize) // Stop the player from moving after pay ends.
      {
        ControllablePlayer.ChangeX = 0; 
        ControllablePlayer.ChangeY = 0;
        return;
      }

      if (IsKeyDown(Keys.Left))
      {
        if(ControllablePlayer.ChangeX > 30)
          ControllablePlayer.ChangeX -= 14;
        else
          ControllablePlayer.ChangeX -= 8;
        keypressed = true;
      }
      if (IsKeyDown(Keys.Right))
      {
        if (ControllablePlayer.ChangeX < 30)
          ControllablePlayer.ChangeX += 14;
        else
          ControllablePlayer.ChangeX += 8;
        keypressed = true;
      }
      if (IsKeyDown(Keys.Up))
      {
        if (ControllablePlayer.ChangeY > 30)
          ControllablePlayer.ChangeY -= 14;
        else
          ControllablePlayer.ChangeY -= 8;
        keypressed = true;
      }
      if (IsKeyDown(Keys.Down))
      {
        if (ControllablePlayer.ChangeY < 30)
          ControllablePlayer.ChangeY += 14;
        else
          ControllablePlayer.ChangeY += 8;
        keypressed = true;
      }
      if (keypressed == false)
      {
        ControllablePlayer.ChangeX = ControllablePlayer.ChangeX - (8 * Math.Sign(ControllablePlayer.ChangeX));
        ControllablePlayer.ChangeY = ControllablePlayer.ChangeY - (8 * Math.Sign(ControllablePlayer.ChangeY));
      }

      ControllablePlayer.Move();

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
          ballAsPlayer.ThrowBall(ControllablePlayer.Top + 16, ControllablePlayer.Left + 16, e.Location.Y, e.Location.X);   
      }
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


          //if (players[j].IsBall)
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
      return Math.Abs(player1.Left - player2.Left) < player1.PlayerWidth && Math.Abs(player1.Top - player2.Top) < player1.PlayerHeight;
    }
    public static bool DetectCloseCollision(Player player1, Player player2, int howClose)
    {
      return Math.Abs(player1.Left - player2.Left) < howClose && Math.Abs(player1.Top - player2.Top) < howClose;
    }
  }
}
