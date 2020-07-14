using System.Drawing;

namespace FootballGame
{
  public class Sideline
  {
    public static Form1 ParentForm;
    public static Image FullSidelineYardage;
    public static void InitializeDrawing()
    {
      FullSidelineYardage = ParentForm.picFullSidelineYardage.Image;
      ParentForm.picFullSidelineYardage.Visible = false;
      ParentForm.picScoreboardLetters.Visible = false;

      DisplaySideline(20);
    }

    public static void DisplaySideline(double BallOnYard)
    {
      double leftPosition = (BallOnYard * Game.PixalsInYard) + (Game.LineOfScrimagePixel - 211);
      ParentForm.picSidelineYardage.Image = GetPartOfBiggerImage(leftPosition);
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