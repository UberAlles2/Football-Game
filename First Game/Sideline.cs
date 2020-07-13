using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

      DisplaySideline(1);
    }

    public static void DisplaySideline(double BallOnYard)
    {
        ParentForm.picSidelineYardage.Image = GetLetterImage(BallOnYard);
    }

    public static Image GetLetterImage(double BallOnYard)
    {
      double leftPosition = (BallOnYard * Game.PixalsInYard) + 149;

      Rectangle cropArea = new Rectangle((int)leftPosition, 0, ParentForm.Width, 32);
      Bitmap bmpImage = new Bitmap(FullSidelineYardage);
      Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);

      return (Image)(bmpCrop);
    }

  }
}