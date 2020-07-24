using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FootballGame
{
  public class Scoreboard
  {
    public static Form1 ParentForm;
    public static Image ScoreboardLetters;
    private static List<PictureBox> letterPicBoxes = new List<PictureBox>();
    public static CountDownTimer CountDownTimer = new CountDownTimer(15, 0);

    public static void InitializeDrawing()
    {
      ScoreboardLetters = ParentForm.picScoreboardLetters.Image;
      ParentForm.picScoreboardLetters.Visible = false;
      CountDownTimer.TimeChanged = DisplayClock; 

      // Create all the pictureboxes
      for (int i = 0; i < 48; i++)
      {
        letterPicBoxes.Add(CreateLetterPictureBox((i * 25) + 2));
      }

      DisplayDown("1");
      DisplayToGo("10");
      DisplayBallOn("20");
      DisplayQtr("1");
      DisplayBearsScore(" 0");
      DisplayTimeMin("15");
      DisplayTimeSec("00");
      DisplayGuestScore(" 0");
    }

    public static PictureBox CreateLetterPictureBox(int left)
    {
      PictureBox p = new PictureBox();
      p.Top = 30;
      p.Left = left;
      p.Width = 26;
      p.Height = 34;
      ParentForm.pnlScoreboard.Controls.Add(p);
      return p;
    }

    public static void DisplayMessage(string message, int startPosition)
    {
      char[] charArray = message.ToCharArray();
      for (int i = 0; i < charArray.Length; i++)
      {
        letterPicBoxes[i + startPosition].Image = GetLetterImage(charArray[i].ToString());
      }
    }

    public static Image GetLetterImage(string letter)
    {
      int ascii = (int)Encoding.ASCII.GetBytes(letter).First();
      double leftPosition = 32;
      if (ascii > 64 && ascii < 91)
        leftPosition = ((ascii - 65) * 27.4) + 2;
      else if (ascii > 48 && ascii < 58) // 1 - 9
        leftPosition = ((ascii - 48) * 27.4) + 689;
      else if (letter == "0")
        leftPosition = 963;
      else if (letter == "!")
        leftPosition = 990;
      else if (letter == "?")
        leftPosition = 1018;
      else if (letter == " ")
        leftPosition = 1045;
      else
        leftPosition = 1045;

      Rectangle cropArea = new Rectangle((int)leftPosition, 2, 26, 34);
      Bitmap bmpImage = new Bitmap(ScoreboardLetters);
      Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);

      return (Image)(bmpCrop);
    }

    public static void DisplayDown(string message)
    {
      DisplayMessage(message, 2);
    }
    public static void DisplayToGo(string message)
    {
      DisplayMessage(message, 5);
    }
    public static void DisplayBallOn(string message)
    {
      DisplayMessage(message, 9);
    }
    public static void DisplayQtr(string message)
    {
      DisplayMessage(message, 13);
    }
    public static void DisplayBearsScore(string message)
    {
      DisplayMessage(message, 18);
    }
    public static void DisplayTimeMin(string message)
    {
      DisplayMessage(message, 21);
    }
    public static void DisplayTimeSec(string message)
    {
      DisplayMessage(message, 24);
    }
    public static void DisplayGuestScore(string message)
    {
      DisplayMessage(message, 27);
    }
    public static void DisplayClock()
    {
      DisplayMessage(CountDownTimer.TimeLeftMinutesString, 21);
      DisplayMessage(CountDownTimer.TimeLeftSecondsString, 24);
    }
  }
}
