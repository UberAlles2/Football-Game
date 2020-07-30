using System.Drawing;
using System.Windows.Forms;

namespace FootballGame
{
  public class PlayingField
  {
    public static Form1 ParentForm;
    public static Image FullSidelineYardage;
    public static Rectangle FieldBounds;
    public static float PixalsInYard = 32;
    public static int FieldHeight;
    public static int FieldCenterY;
    public static int LineOfScrimagePixel = 280;
    public static int FieldGoalPostLeft = 0;
    public static int FieldGoalPostTop = 0;
    public static int FieldGoalPostBottom = 0;

    public static void InitializeDrawing(double lineOfScrimageYard) // 1 - 100
    {
      FullSidelineYardage = ParentForm.picFullSidelineYardage.Image;
      ParentForm.picFullSidelineYardage.Visible = false;
      ParentForm.picScoreboardLetters.Visible = false;

      // Initialize field dimensions
      FieldBounds = new Rectangle(0, ParentForm.pnlScoreboard.Height + 30, ParentForm.Width - ParentForm.Player1.Width, ParentForm.Height - ParentForm.pnlScoreboard.Height - 36);
      FieldCenterY = (FieldBounds.Height / 2) + ParentForm.picSidelineYardage.Height + 16; // Players go out of bounds when their botton goes out.

      DrawField(lineOfScrimageYard);
    }
    public static void DrawField(double lineOfScrimageYard)
    {
      DrawTopSideline(lineOfScrimageYard);
      DrawLeftEndZone(lineOfScrimageYard);
    }

    public static void PaintScrimmageAndFirstDownLines(object sender, PaintEventArgs e)
    {
      // Scrimmage
      Pen pen = new Pen(Color.Blue);
      e.Graphics.DrawLine(pen, PlayingField.LineOfScrimagePixel, 0, PlayingField.LineOfScrimagePixel, ParentForm.Height - 62);
      // First Down
      pen = new Pen(Color.Yellow);
      int firstDownMarker = PlayingField.LineOfScrimagePixel + ((int)Game.CurrentGameState.YardsToGo * (int)PixalsInYard);
      e.Graphics.DrawLine(pen, firstDownMarker, 0, firstDownMarker, ParentForm.Height - 62);
      // Left Goal Line 
      if (Game.CurrentGameState.BallOnYard100 < 9)
      {
        pen = new Pen(Color.White);
        int goalLineMarker = PlayingField.LineOfScrimagePixel - (int)(Game.CurrentGameState.BallOnYard100 * (int)PixalsInYard);
        e.Graphics.DrawLine(pen, goalLineMarker, 0, goalLineMarker, ParentForm.Height - 62);
      }

      // Left Goal Post
      if (Game.CurrentGameState.BallOnYard100 > 45) 
      {
        // Draw post ends
        pen = new Pen(Color.Yellow, 3);
        double leftPosition; 
        double top;
        double bottom;
        if (Game.CurrentGameState.BallOnYard100 < 78) // Outside the 22 yard line
        {
          leftPosition = FieldBounds.Left + FieldBounds.Width - 30; // Far back next to form's right edge
          // Make narrower the farther away
          top = FieldCenterY - (Game.CurrentGameState.BallOnYard100 * .9); 
          bottom = FieldCenterY + (Game.CurrentGameState.BallOnYard100 * .9);
        }
        else
        {
          leftPosition = LineOfScrimagePixel - 5 + (110 - Game.CurrentGameState.BallOnYard100) * PixalsInYard; // 10 yards back of goal line, yard 110
          // Keep at 148 wide
          top = FieldCenterY - 74; 
          bottom = FieldCenterY + 74;
        }

        FieldGoalPostLeft = (int)leftPosition;
        FieldGoalPostTop = (int)top;
        FieldGoalPostBottom = (int)bottom;

        e.Graphics.DrawEllipse(pen, new Rectangle(FieldGoalPostLeft, FieldGoalPostTop, 8, 8));
        e.Graphics.DrawEllipse(pen, new Rectangle(FieldGoalPostLeft, FieldGoalPostBottom, 8, 8));
        e.Graphics.DrawLine(pen, FieldGoalPostLeft + 4, FieldGoalPostTop + 8, FieldGoalPostLeft + 4, FieldGoalPostBottom);
      }
    }

    public static void PaintEndZones(object sender, PaintEventArgs e)
    {
      // Left End Zone Image
      if (Game.CurrentGameState.BallOnYard100 < 6)
      {
        if (ParentForm.picEndZoneLeft.Bounds.IntersectsWith(Player.ControllablePlayer.PicBox.Bounds))
        {
          Player.ControllablePlayer.PicBox.SendToBack();
          Graphics g = e.Graphics;
          g.DrawImage(Player.ControllablePlayer.PicBox.Image, Player.ControllablePlayer.PicBox.Location.X - ParentForm.picEndZoneLeft.Left, Player.ControllablePlayer.PicBox.Location.Y - ParentForm.picEndZoneLeft.Top, Player.ControllablePlayer.PicBox.Width, Player.ControllablePlayer.PicBox.Height);
        }
      }
    }

    public static void DrawTopSideline(double lineOfScrimageYard)
    {
      double leftPosition = (lineOfScrimageYard * PixalsInYard) + (PlayingField.LineOfScrimagePixel - 211);
      ParentForm.picSidelineYardage.Image = GetPartOfBiggerImage(leftPosition);
    }

    public static void DrawLeftEndZone(double lineOfScrimageYard)
    {
      if (Game.CurrentGameState.BallOnYard100 < 6)
      {
        if (ParentForm.picEndZoneLeft.Visible == false)
        {
          ParentForm.picEndZoneLeft.Visible = true;
        }

        double leftPosition = 120 - (lineOfScrimageYard * PixalsInYard);
        ParentForm.picEndZoneLeft.Left = (int)leftPosition;
      }
      else
      {
        if (ParentForm.picEndZoneLeft.Visible == true)
        {
          ParentForm.picEndZoneLeft.Visible = false;
        }
      }
    }

    public static Image GetPartOfBiggerImage(double leftPosition)
    {
      Rectangle cropArea = new Rectangle((int)leftPosition, 0, ParentForm.Width, 32);
      Bitmap bmpImage = new Bitmap(FullSidelineYardage);
      Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);

      return (Image)(bmpCrop);
    }
  }
}