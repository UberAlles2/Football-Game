﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace FootballGame
{
  public class Scoreboard
  {
    public static Form1 ParentForm;
    public static Image ScoreboardLetters;
    private static List<PictureBox> letterPicBoxes = new List<PictureBox>();
    public static CountDownTimer CountDownTimer = new CountDownTimer(15, 0);

    public static System.Windows.Forms.Timer ScrollTimer;
    public static string MessageToScroll = "";
    public static int ScrollMessageLength = 16;
    public static int ScrollMessagePaddedLength;
    public static int ScrollMessageTicks = 0;

    public static void InitializeDrawing()
    {
      ScoreboardLetters = ParentForm.picScoreboardLetters.Image;
      ParentForm.picScoreboardLetters.Visible = false;
      CountDownTimer.TimeChanged = DisplayClock;
      CountDownTimer.TimeExpired = ClockTimeExired;
      CountDownTimer.SetInterval = 500;
      // Create all the pictureboxes
      for (int i = 0; i < 48; i++)
      {
        letterPicBoxes.Add(CreateLetterPictureBox((i * 25) + 2));
      }

      DisplayDown("1");
      DisplayToGo("10");
      DisplayBallOn(20);
      DisplayQtr(Game.CurrentGameState.Quarter);
      DisplayClock();
      DisplayBearsScore(Game.CurrentGameState.HomeScore);
      DisplayGuestScore(Game.CurrentGameState.GuestScore);
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

    public static void ScrollMessage(string message)
    {
      MessageToScroll = new string(' ', ScrollMessageLength + 1) + message.ToUpper() + new string(' ', 60);
      ScrollMessagePaddedLength = (ScrollMessageLength * 2) + message.Length;

      ScrollTimer = new System.Windows.Forms.Timer();
      ScrollTimer.Interval = 200;
      ScrollTimer.Tick += new EventHandler(TimerScrollMessageEvent);
      ScrollMessageTicks = 0;
      ScrollTimer.Start();
    }

    public static void DisplayScrollMessage(int timerTicks)
    {
      if (timerTicks > ScrollMessagePaddedLength - 1)
      {
        ScrollTimer.Stop();
        ScrollTimer.Dispose();
        return;
      }

      string currentMessagePart = MessageToScroll.Substring(timerTicks, ScrollMessageLength);
      DisplayMessage(currentMessagePart, 31);
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
    public static void DisplayBallOn(float yard)
    {
      DisplayMessage(yard.ToString("0").PadLeft(2), 9);
    }
    public static void DisplayQtr(int quarter)
    {
      DisplayMessage(quarter.ToString("0"), 13);
    }
    public static void DisplayBearsScore(int score)
    {
      DisplayMessage(score.ToString("0").PadLeft(2), 18);
    }
    public static void DisplayTimeMin(string message)
    {
      DisplayMessage(message, 21);
    }
    public static void DisplayTimeSec(string message)
    {
      DisplayMessage(message, 24);
    }
    public static void DisplayGuestScore(int score)
    {
      DisplayMessage(score.ToString("0").PadLeft(2), 27);
    }

    public static void DisplayClock()
    {
      DisplayMessage(CountDownTimer.TimeLeftMinutesString, 21);
      DisplayMessage(CountDownTimer.TimeLeftSecondsString, 24);
    }
    public static void ClockTimeExired()
    {
      CountDownTimer.Stop();
      Game.EndGame();
    }
    public static void TimerScrollMessageEvent(object sender, EventArgs e)
    {
      ScrollMessageTicks++;
      DisplayScrollMessage(ScrollMessageTicks);
    }
  }
}
