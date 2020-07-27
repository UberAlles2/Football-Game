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
    }

    public static void PaintEndZones(object sender, PaintEventArgs e)
    {
      Graphics g = e.Graphics;

      // Left End Zone Image
      if (Game.CurrentGameState.BallOnYard100 < 6)
      {
        if (ParentForm.picEndZoneLeft.Bounds.IntersectsWith(Player.ControllablePlayer.PicBox.Bounds))
        {
          Player.ControllablePlayer.PicBox.SendToBack();
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