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

namespace FootballGame
{
  public enum CollisionOrientation
  {
    Above,
    Below,
    ToLeft,
    ToRight
  }
  public enum DefensiveMode
  {
    Blitz,
    Normal,
    Soft
  }

  public class Game
  {
    // static
    public static Form1 ParentForm;
    //public static Player PlayerWithBall = new Player();
    public static Player ControllablePlayer = new Player();
    public static BallAsPlayer ballAsPlayer = new BallAsPlayer();
    public static int FieldCenterY;
    public static bool IsThrowing;
    public static Random Random = new Random();

    // instance
    private bool running = true;
    private bool reintialize = false;
    private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
    public List<Player> players = Player.players;
   

    public Game(Form1 form1)
    {
      ParentForm = form1;
      Player.ParentForm = form1;
      Player.ParentGame = this;
      FieldCenterY = ParentForm.Height / 2 - 32;

      AddPlayers();

      timer.Tick += new System.EventHandler(KeyDown);
      timer.Interval = 50;
    }

    public void AddPlayers()
    {
      // Offensive Players
      OffenderQuarterback offenderQuarterback = new OffenderQuarterback();
      offenderQuarterback.InitialTop = FieldCenterY;
      offenderQuarterback.InitialLeft = 10;
      //offenderQuarterback.PicBox = ParentForm.Player1;
      offenderQuarterback.PicBox = AddPlayerPictureBox(ParentForm.Player1);
      offenderQuarterback.Initialize();
      players.Add(offenderQuarterback);
      ControllablePlayer = offenderQuarterback;

      OffenderMiddleLineman offenderMiddleLineman = new OffenderMiddleLineman();
      offenderMiddleLineman.InitialTop = FieldCenterY;
      offenderMiddleLineman.InitialLeft = 150;
      offenderMiddleLineman.PicBox = AddPlayerPictureBox(ParentForm.Player1);
      offenderMiddleLineman.Initialize(); 
      players.Add(offenderMiddleLineman);

      offenderMiddleLineman = offenderMiddleLineman.CloneAndUpcast<OffenderMiddleLineman, OffenderMiddleLineman>();
      offenderMiddleLineman.InitialTop = FieldCenterY - 78;
      offenderMiddleLineman.PicBox = AddPlayerPictureBox(ParentForm.Player1);
      offenderMiddleLineman.Initialize();
      players.Add(offenderMiddleLineman);

      offenderMiddleLineman = offenderMiddleLineman.CloneAndUpcast<OffenderMiddleLineman, OffenderMiddleLineman>();
      offenderMiddleLineman.InitialTop = FieldCenterY + 78;
      offenderMiddleLineman.PicBox = AddPlayerPictureBox(ParentForm.Player1);
      offenderMiddleLineman.Initialize();
      players.Add(offenderMiddleLineman);

      OffenderWideReceiver offenderWideReceiver = new OffenderWideReceiver();
      offenderWideReceiver.InitialTop = FieldCenterY - 240;
      offenderWideReceiver.InitialLeft = 150;
      offenderWideReceiver.PicBox = AddPlayerPictureBox(ParentForm.Player1);
      offenderWideReceiver.ButtonHookPattern(); // TODO randomize
      offenderWideReceiver.Initialize();
      players.Add(offenderWideReceiver);

      // Defensive Players
      DefenderMiddleLineman defenderMiddleLineman = new DefenderMiddleLineman(); 
      defenderMiddleLineman.InitialTop = FieldCenterY;
      defenderMiddleLineman.InitialLeft = 200;
      defenderMiddleLineman.PicBox = AddPlayerPictureBox(ParentForm.Player2);
      defenderMiddleLineman.Initialize();
      players.Add(defenderMiddleLineman);

      DefenderOutsideLineman defenderOutsideLineman = defenderMiddleLineman.CloneAndUpcast<DefenderOutsideLineman, Player>();
      defenderOutsideLineman.Offset = -80;
      defenderOutsideLineman.InitialLeft = 200;
      defenderOutsideLineman.InitialTop = FieldCenterY + defenderOutsideLineman.Offset - 20;
      defenderOutsideLineman.PicBox = AddPlayerPictureBox(ParentForm.Player2);
      defenderOutsideLineman.PicBox.BackColor = Color.LightGreen;
      defenderOutsideLineman.CoDefender = defenderMiddleLineman;
      defenderOutsideLineman.Initialize();
      players.Add(defenderOutsideLineman);

      defenderOutsideLineman = defenderOutsideLineman.CloneAndUpcast<DefenderOutsideLineman, DefenderOutsideLineman>();
      defenderOutsideLineman.Offset = 80;
      defenderOutsideLineman.InitialTop = FieldCenterY + defenderOutsideLineman.Offset + 20;
      defenderOutsideLineman.PicBox = AddPlayerPictureBox(ParentForm.Player2);
      defenderOutsideLineman.PicBox.BackColor = Color.LightGreen;
      defenderOutsideLineman.CoDefender = defenderMiddleLineman;
      defenderOutsideLineman.Initialize();
      players.Add(defenderOutsideLineman);

      DefenderMiddleLinebacker defenderMiddleLinebacker = defenderOutsideLineman.CloneAndUpcast<DefenderMiddleLinebacker, Player>();
      defenderMiddleLinebacker.DefensiveMode = DefensiveMode.Normal;  // TODO randomize between coverage
      defenderMiddleLinebacker.InitialLeft = 400;
      defenderMiddleLinebacker.InitialTop = FieldCenterY;
      defenderMiddleLinebacker.PicBox = AddPlayerPictureBox(ParentForm.Player2);
      defenderMiddleLinebacker.PicBox.BackColor = Color.DarkGreen;
      defenderMiddleLinebacker.Initialize();
      players.Add(defenderMiddleLinebacker);

      DefenderCornerback defenderCornerback = defenderMiddleLineman.CloneAndUpcast<DefenderCornerback, Player>();
      defenderCornerback.DefensiveMode = DefensiveMode.Normal;  // TODO randomize between coverage
      defenderCornerback.InitialLeft = offenderWideReceiver.Left + 200;
      defenderCornerback.InitialTop = offenderWideReceiver.Top + 30;
      defenderCornerback.PicBox = AddPlayerPictureBox(ParentForm.Player2);
      defenderCornerback.InitialTargetPlayer = offenderWideReceiver;
      defenderCornerback.Initialize();
      players.Add(defenderCornerback);

      // Ball as a Player
      ballAsPlayer.InitialLeft = -999;
      ballAsPlayer.InitialTop = -999;
      ballAsPlayer.PicBox = ParentForm.picFootball;
      ballAsPlayer.Initialize();
      players.Add(ballAsPlayer);
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
      players.ForEach(p => p.Initialize());
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
        foreach (Player p in players)
        {
          Application.DoEvents();
          p.Move();
        }
        CheckCollisions(players);

        Thread.Sleep(32);
      }
    }

    public void EndPlay(string message)
    {
      MessageBox.Show(message);
      reintialize = true;
    }

    public void Stop()
    {
      timer.Stop();
      running = false;
    }

    public void KeyDown(object sender, EventArgs e)
    {
      bool keypressed = false;
      
      if (IsKeyDown(Keys.Left))
      {
        if(ControllablePlayer.ChangeX > 30)
          ControllablePlayer.ChangeX -= 16;
        else
          ControllablePlayer.ChangeX -= 8;
        keypressed = true;
      }
      if (IsKeyDown(Keys.Right))
      {
        if (ControllablePlayer.ChangeX < 30)
          ControllablePlayer.ChangeX += 16;
        else
          ControllablePlayer.ChangeX += 8;
        keypressed = true;
      }
      if (IsKeyDown(Keys.Up))
      {
        if (ControllablePlayer.ChangeY > 30)
          ControllablePlayer.ChangeY -= 16;
        else
          ControllablePlayer.ChangeY -= 8;
        keypressed = true;
      }
      if (IsKeyDown(Keys.Down))
      {
        if (ControllablePlayer.ChangeY < 30)
          ControllablePlayer.ChangeY += 16;
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
        if(!Game.IsThrowing)
          ballAsPlayer.ThrowBall(ControllablePlayer.Top + 16, ControllablePlayer.Left + 16, e.Location.Y, e.Location.X);   
      }
    }

    public void CheckCollisions(List<Player> players)
    {
      for (int i = 0; i < players.Count - 1; i++)
      {
        for (int j = i + 1; j < players.Count; j++)
        {
          if (!Game.IsThrowing && players[j].IsBall) 
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
