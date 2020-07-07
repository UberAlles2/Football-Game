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

  class Game
  {
    public static Form1 ParentForm;
    public List<Player> players = Player.players;
    Player playerWithBall = new Player();
    bool running = true;
    System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

    public Game(Form1 form1)
    {
      ParentForm = form1;
      Player.ParentForm = form1;
      AddPlayers();
      timer.Tick += new System.EventHandler(KeyDown);
      timer.Interval = 50;
    }

    public void AddPlayers()
    {
      OffenderQuarterback offenderQuarterback = new OffenderQuarterback();
      offenderQuarterback.Top = ParentForm.Height/2 + 16;
      offenderQuarterback.Left = 10;
      offenderQuarterback.Team = 1;
      offenderQuarterback.PicBox = ParentForm.Player1;
      offenderQuarterback.ChangeX = 1;
      offenderQuarterback.Cap = 90;
      offenderQuarterback.ChangeY = 1;
      offenderQuarterback.HasBall = true;
      players.Add(offenderQuarterback);
      playerWithBall = offenderQuarterback;

      // Clonable base
      Player clonable = new Player();
      clonable.Team = 1;
      clonable.Top = playerWithBall.Top;
      clonable.Left = 140;
      clonable.PicBox = ParentForm.Player1;
      clonable.ChangeX = -3;
      clonable.ChangeY = -3;
      clonable.Cap = 90;

      OffenderMiddleLineman offenderMiddleLineman = clonable.CloneAndUpcast<OffenderMiddleLineman, Player>();
      offenderMiddleLineman.PicBox = AddPlayerPictureBox(ParentForm.Player1);
      players.Add(offenderMiddleLineman);

      offenderMiddleLineman = clonable.CloneAndUpcast<OffenderMiddleLineman, Player>();
      offenderMiddleLineman.PicBox = AddPlayerPictureBox(ParentForm.Player1);
      offenderMiddleLineman.Top = playerWithBall.Top - 70;
      players.Add(offenderMiddleLineman);

      offenderMiddleLineman = clonable.CloneAndUpcast<OffenderMiddleLineman, Player>();
      offenderMiddleLineman.PicBox = AddPlayerPictureBox(ParentForm.Player1);
      offenderMiddleLineman.Top = playerWithBall.Top + 70;
      players.Add(offenderMiddleLineman);

      // Defensive Players
      clonable.Team = 2;
      clonable.Top = playerWithBall.Top;
      clonable.Left = 200;
      clonable.PicBox = ParentForm.Player2;
      clonable.ChangeX = -3;
      clonable.ChangeY = -3;
      clonable.Cap = 100;

      DefenderMiddleLineman defenderMiddleLineman = clonable.CloneAndUpcast<DefenderMiddleLineman, Player>();
      defenderMiddleLineman.PicBox = AddPlayerPictureBox(ParentForm.Player2);
      defenderMiddleLineman.Intelligence = 10;
      defenderMiddleLineman.TargetPlayer = playerWithBall;
      players.Add(defenderMiddleLineman);

      DefenderOutsideLineman defenderOutsideLineman = clonable.CloneAndUpcast<DefenderOutsideLineman, Player>();
      defenderOutsideLineman.PicBox = AddPlayerPictureBox(ParentForm.Player2);
      //defenderOutsideLineman.PicBox.Image.RotateFlip(RotateFlipType.Rotate180FlipY);
      defenderOutsideLineman.PicBox.BackColor = Color.LightGreen; 
      defenderOutsideLineman.Offset = -80;
      defenderOutsideLineman.Top = clonable.Top + defenderOutsideLineman.Offset - 20;
      //defenderOutsideLineman.Cap = 100;
      defenderOutsideLineman.Intelligence = 4;
      defenderOutsideLineman.TargetPlayer = playerWithBall;
      defenderOutsideLineman.CoDefender = defenderMiddleLineman;
      players.Add(defenderOutsideLineman);

      defenderOutsideLineman = clonable.CloneAndUpcast<DefenderOutsideLineman, Player>();
      defenderOutsideLineman.PicBox = AddPlayerPictureBox(ParentForm.Player2);
      //defenderOutsideLineman.PicBox.Image.RotateFlip(RotateFlipType.Rotate180FlipY);
      defenderOutsideLineman.PicBox.BackColor = Color.LightGreen;
      defenderOutsideLineman.Offset = 80;
      defenderOutsideLineman.Top = clonable.Top + defenderOutsideLineman.Offset + 20;
      //defenderOutsideLineman.Cap = 100;
      defenderOutsideLineman.Intelligence = 4;
      defenderOutsideLineman.TargetPlayer = playerWithBall;
      defenderOutsideLineman.CoDefender = defenderMiddleLineman;
      players.Add(defenderOutsideLineman);

      ParentForm.Player2.Visible = false;
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
 
    public void Run()
    {
      timer.Start();
      while (running)
      {
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
        if(playerWithBall.ChangeX > -32 && playerWithBall.ChangeX < 32)
          playerWithBall.ChangeX -= 32;
        else
          playerWithBall.ChangeX -= 12;
        keypressed = true;
      }
      if (IsKeyDown(Keys.Right))
      {
        if (playerWithBall.ChangeX < 32 && playerWithBall.ChangeX > -32)
          playerWithBall.ChangeX += 32;
        else
          playerWithBall.ChangeX += 12;
        keypressed = true;
      }
      if (IsKeyDown(Keys.Up))
      {
        if (playerWithBall.ChangeY > -32 && playerWithBall.ChangeY < 32)
          playerWithBall.ChangeY -= 32;
        else
          playerWithBall.ChangeY -= 12;
        keypressed = true;
      }
      if (IsKeyDown(Keys.Down))
      {
        if (playerWithBall.ChangeY < 32 && playerWithBall.ChangeY > -32)
          playerWithBall.ChangeY += 26;
        else
          playerWithBall.ChangeY += 12;
        keypressed = true;
      }
      if (keypressed == false)
      {
        playerWithBall.ChangeX = playerWithBall.ChangeX - (8 * Math.Sign(playerWithBall.ChangeX));
        playerWithBall.ChangeY = playerWithBall.ChangeY - (8 * Math.Sign(playerWithBall.ChangeY));
      }

      playerWithBall.Move();

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
                players[i].CollisionMove(players[j], CollisionOrientation.Above);
                players[j].CollisionMove(players[i], CollisionOrientation.Below);
              }
              else
              {
                players[i].CollisionMove(players[i], CollisionOrientation.Below);
                players[j].CollisionMove(players[j], CollisionOrientation.Above);
              }
            }
            else // Hitting to the left or right of another player
            {
              //  | |   | |
              if (players[i].Left < players[j].Left)
              {
                players[i].CollisionMove(players[j], CollisionOrientation.ToLeft);
                players[j].CollisionMove(players[i], CollisionOrientation.ToRight);
              }
              else
              {
                players[i].CollisionMove(players[j], CollisionOrientation.ToRight);
                players[j].CollisionMove(players[i], CollisionOrientation.ToLeft);
              }
            }
            Player.MovePic(players[i]);
            Player.MovePic(players[j]);
          }
        }
      }
    }
  }
}
