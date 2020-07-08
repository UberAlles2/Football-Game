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
    public static Form1 ParentForm;
    public static Player PlayerWithBall = new Player();
    public int FieldCenterY;
    public List<Player> players = Player.players;

    private bool running = true;
    private bool reintialize = false;
    private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

    public Game(Form1 form1)
    {
      ParentForm = form1;
      Player.ParentForm = form1;
      Player.ParentGame = this;
      FieldCenterY = ParentForm.Height / 2 + 16;

      AddPlayers();

      timer.Tick += new System.EventHandler(KeyDown);
      timer.Interval = 50;
    }

    public void AddPlayers()
    {
      OffenderQuarterback offenderQuarterback = new OffenderQuarterback();
      offenderQuarterback.InitialTop = FieldCenterY;
      offenderQuarterback.InitialLeft = 10;
      offenderQuarterback.Initialize();
      offenderQuarterback.PicBox = ParentForm.Player1;
      players.Add(offenderQuarterback);
      PlayerWithBall = offenderQuarterback;

      OffenderMiddleLineman offenderMiddleLineman = new OffenderMiddleLineman();
      offenderMiddleLineman.InitialTop = FieldCenterY;
      offenderMiddleLineman.InitialLeft = 150;
      offenderMiddleLineman.Initialize(); 
      offenderMiddleLineman.PicBox = AddPlayerPictureBox(ParentForm.Player1);
      players.Add(offenderMiddleLineman);

      offenderMiddleLineman = offenderMiddleLineman.CloneAndUpcast<OffenderMiddleLineman, OffenderMiddleLineman>();
      offenderMiddleLineman.InitialTop = FieldCenterY - 70;
      offenderMiddleLineman.Initialize();
      offenderMiddleLineman.PicBox = AddPlayerPictureBox(ParentForm.Player1);
      players.Add(offenderMiddleLineman);

      offenderMiddleLineman = offenderMiddleLineman.CloneAndUpcast<OffenderMiddleLineman, OffenderMiddleLineman>();
      offenderMiddleLineman.InitialTop = FieldCenterY + 70;
      offenderMiddleLineman.Initialize();
      offenderMiddleLineman.PicBox = AddPlayerPictureBox(ParentForm.Player1);
      players.Add(offenderMiddleLineman);

      // Defensive Players
      // Clonable base
      //Player clonable = new Player();
      //clonable.Team = 2;
      //clonable.Top = PlayerWithBall.Top;
      //clonable.Left = 200;
      //clonable.PicBox = ParentForm.Player2;
      //clonable.ChangeX = -3;
      //clonable.ChangeY = -3;
      //clonable.SpeedCap = 100;

      DefenderMiddleLineman defenderMiddleLineman = new DefenderMiddleLineman(); //clonable.CloneAndUpcast<DefenderMiddleLineman, Player>();
      defenderMiddleLineman.InitialTop = FieldCenterY;
      defenderMiddleLineman.InitialLeft = 200;
      defenderMiddleLineman.Initialize();
      defenderMiddleLineman.PicBox = ParentForm.Player2;
      players.Add(defenderMiddleLineman);

      DefenderOutsideLineman defenderOutsideLineman = defenderMiddleLineman.CloneAndUpcast<DefenderOutsideLineman, Player>();
      defenderOutsideLineman.Offset = -80;
      defenderOutsideLineman.InitialLeft = 200;
      defenderOutsideLineman.InitialTop = FieldCenterY + defenderOutsideLineman.Offset - 20;
      defenderOutsideLineman.Initialize();
      defenderOutsideLineman.PicBox = AddPlayerPictureBox(ParentForm.Player2);
      defenderOutsideLineman.PicBox.BackColor = Color.LightGreen; 
      defenderOutsideLineman.CoDefender = defenderMiddleLineman;
      players.Add(defenderOutsideLineman);


      defenderOutsideLineman = defenderOutsideLineman.CloneAndUpcast<DefenderOutsideLineman, DefenderOutsideLineman>();
      defenderOutsideLineman.Offset = 80;
      defenderOutsideLineman.InitialTop = FieldCenterY + defenderOutsideLineman.Offset + 20;
      defenderOutsideLineman.Initialize();
      defenderOutsideLineman.PicBox = AddPlayerPictureBox(ParentForm.Player2);
      defenderOutsideLineman.PicBox.BackColor = Color.LightGreen;
      defenderOutsideLineman.CoDefender = defenderMiddleLineman;
      players.Add(defenderOutsideLineman);

      DefenderMiddleLinebacker defenderMiddleLinebacker = defenderOutsideLineman.CloneAndUpcast<DefenderMiddleLinebacker, Player>();
      defenderMiddleLinebacker.DefensiveMode = DefensiveMode.Normal;
      defenderMiddleLinebacker.InitialLeft = 400;
      defenderMiddleLinebacker.InitialTop = FieldCenterY;
      defenderMiddleLinebacker.Initialize();
      defenderMiddleLinebacker.PicBox = AddPlayerPictureBox(ParentForm.Player2);
      defenderMiddleLinebacker.PicBox.BackColor = Color.DarkGreen;
      players.Add(defenderMiddleLinebacker);

//      ParentForm.Player2.Visible = false;
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
        if(Math.Abs(PlayerWithBall.ChangeX) > 30)
          PlayerWithBall.ChangeX -= 20;
        else
          PlayerWithBall.ChangeX -= 10;
        keypressed = true;
      }
      if (IsKeyDown(Keys.Right))
      {
        if (Math.Abs(PlayerWithBall.ChangeX) > 30)
          PlayerWithBall.ChangeX += 20;
        else
          PlayerWithBall.ChangeX += 10;
        keypressed = true;
      }
      if (IsKeyDown(Keys.Up))
      {
        if (PlayerWithBall.ChangeY > -30 && PlayerWithBall.ChangeY < 30)
          PlayerWithBall.ChangeY -= 20;
        else
          PlayerWithBall.ChangeY -= 10;
        keypressed = true;
      }
      if (IsKeyDown(Keys.Down))
      {
        if (PlayerWithBall.ChangeY < 30 && PlayerWithBall.ChangeY > -30)
          PlayerWithBall.ChangeY += 20;
        else
          PlayerWithBall.ChangeY += 10;
        keypressed = true;
      }
      if (keypressed == false)
      {
        PlayerWithBall.ChangeX = PlayerWithBall.ChangeX - (8 * Math.Sign(PlayerWithBall.ChangeX));
        PlayerWithBall.ChangeY = PlayerWithBall.ChangeY - (8 * Math.Sign(PlayerWithBall.ChangeY));
      }

      PlayerWithBall.Move();

      return;
    }

    public static bool IsKeyDown(Keys key)
    {
      return (GetKeyState(Convert.ToInt16(key)) & 0X80) == 0X80;
    }
    [DllImport("user32.dll")]
    public extern static Int16 GetKeyState(Int16 nVirtKey);

    public void CheckCollisions(List<Player> players)
    {
      for (int i = 0; i < players.Count - 1; i++)
      {
        for (int j = i + 1; j < players.Count; j++)
        {
          // If player is hitting another player
          if (Math.Abs(players[i].CenterX - players[j].CenterX) < players[i].PlayerWidth && Math.Abs(players[i].CenterY - players[j].CenterY) < players[i].PlayerHeight)
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
  }
}
