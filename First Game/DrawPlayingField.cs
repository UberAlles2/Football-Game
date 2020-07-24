using System.Drawing;
using System.Windows.Forms;

namespace FootballGame
{
  public class DrawPlayingField
  {
    public static Game ParentGame;
    public static Form1 ParentForm;
    public static Image FullSidelineYardage;
    public static void InitializeDrawing(double lineOfScrimageYard) // 1 - 100
    {
      FullSidelineYardage = ParentForm.picFullSidelineYardage.Image;
      ParentForm.picFullSidelineYardage.Visible = false;
      ParentForm.picScoreboardLetters.Visible = false;

      DrawField(lineOfScrimageYard);
    }
    public static void DrawField(double lineOfScrimageYard)
    {
      DrawTopSideline(lineOfScrimageYard);
      DrawEndZone(lineOfScrimageYard);
    }

    public static void PaintScrimmageAndFirstDownLines(object sender, PaintEventArgs e) 
    {
      // Scrimmage
      Pen pen = new Pen(Color.FromArgb(255, 128, 128, 255));
      e.Graphics.DrawLine(pen, Game.LineOfScrimagePixel, 0, Game.LineOfScrimagePixel, ParentForm.Height - 62);
      // First Down
      pen = new Pen(Color.FromArgb(255, 255, 255, 0));
      int firstDownMarker = Game.LineOfScrimagePixel + ((int)Game.PlayOptionsFormStats.YardsToGo * (int)Game.PixalsInYard);
      e.Graphics.DrawLine(pen, firstDownMarker, 0, firstDownMarker, ParentForm.Height - 62);
    }

    public static void DrawTopSideline(double lineOfScrimageYard)
    {
      double leftPosition = (lineOfScrimageYard * Game.PixalsInYard) + (Game.LineOfScrimagePixel - 211);
      ParentForm.picSidelineYardage.Image = GetPartOfBiggerImage(leftPosition);
    }

    public static void DrawEndZone(double lineOfScrimageYard)
    {
      double leftPosition = (lineOfScrimageYard * Game.PixalsInYard) + Game.LineOfScrimagePixel;
      ParentForm.picEndZoneLeft.Left = (int)leftPosition - 290;
      ParentForm.lblLeftSideLine.Left = (int)leftPosition - 129;
      ParentForm.picEndZoneLeft.SendToBack();
      ParentForm.lblLeftSideLine.SendToBack();
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