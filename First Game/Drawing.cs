using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  public class Drawing
  {
    public static Form1 ParentForm;
    public static Image ScoreboardLetters;
    private static List<PictureBox> letterPicBoxes = new List<PictureBox>();
    private static List<LetterLocation> letterLocations = new List<LetterLocation>();
    //byte[] asciiBytes = Encoding.ASCII.GetBytes(value);
    public static void InitializeDrawing()
    {
      ScoreboardLetters = ParentForm.picScoreboardLetters.Image;
      letterPicBoxes.Add(AddLetterPictureBox());
      letterPicBoxes[0].Image = GetLetterImage("0");

      //for(int i = 0; i < 10; i++) 
      //{
      //  string letter = ((char)(i + 65)).ToString(); // 'A' = 65, 'B' = 66
      //  letterLocations.Add(new LetterLocation() { Letter = letter, X = i * 28 });
      //}



    }
    
    public static PictureBox AddLetterPictureBox()
    {
      PictureBox p = new PictureBox();
      p.Top = 2;
      p.Left = 2;
      p.Width = 26;
      p.Height = 34;
      ParentForm.pnlScoreboard.Controls.Add(p);
      p.Name = "Letter1";
      return p;
    }


    public static Image GetLetterImage(string letter)
    {
      int ascii = (int)Encoding.ASCII.GetBytes(letter).First();
      double leftPosition = 32;
      if(ascii > 64 && ascii < 91)
        leftPosition = ((ascii - 65) * 27.4) + 2;
      else if (ascii > 48 && ascii < 58) // 1 - 9
        leftPosition = ((ascii - 48) * 27.4) + 689;
      else if (ascii == 48) // 0
        leftPosition = 963;

      Rectangle cropArea = new Rectangle((int)leftPosition, 2, 26, 34);
      Bitmap bmpImage = new Bitmap(ScoreboardLetters);
      Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
      
      return (Image)(bmpCrop);
    }
  }

  class LetterLocation
  {
    public string Letter;
    public int X;
  }
}
