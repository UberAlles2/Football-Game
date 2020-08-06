using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  public class CountDownTimer : IDisposable
  {
    public Stopwatch _stpWatch;

    public Action TimeChanged; // Set this delegate to an Action method that gets called when the time ticks. (Every one second default.)
    public Action TimeExpired; // Set this delegate to an Action method that gets called when the time expires.

    public bool IsRunning => timer.Enabled;

    public int SetInterval
    {
      get => timer.Interval;
      set => timer.Interval = value;
    }

    private Timer timer = new Timer();
    private TimeSpan _max = TimeSpan.FromMilliseconds(600000);
    public TimeSpan TimeLeft
    {
      get 
      {
        if (_stpWatch == null)
          _stpWatch = new Stopwatch();
        return (_max.TotalMilliseconds - _stpWatch.ElapsedMilliseconds) > 0 ? TimeSpan.FromMilliseconds(_max.TotalMilliseconds - _stpWatch.ElapsedMilliseconds) : TimeSpan.FromMilliseconds(0);
      }
    }
    
    private bool _mustStop => (_max.TotalMilliseconds - _stpWatch.ElapsedMilliseconds) < 0;
    public string TimeLeftString => TimeLeft.ToString(@"mm\:ss");
    public string TimeLeftMinutesString => TimeLeft.ToString(@"mm");
    public string TimeLeftSecondsString => TimeLeft.ToString(@"ss");
    public string TimeLeftMillisecondsString => TimeLeft.ToString(@"mm\:ss\.fff");

    private void TimerTick(object sender, EventArgs e)
    {
      TimeChanged?.Invoke();

      if (_mustStop)
      {
        TimeExpired?.Invoke();
        _stpWatch.Stop();
        timer.Enabled = false;
      }
    }

    public CountDownTimer(int min, int sec)
    {
      SetTime(min, sec);
      Init();
    }
    public CountDownTimer(TimeSpan ts)
    {
      SetTime(ts);
      Init();
    }
    public CountDownTimer()
    {
      Init();
    }
    private void Init()
    {
      SetInterval = 1000; // One second default
      _stpWatch = new Stopwatch();
      _stpWatch.Reset();
      timer.Tick += new EventHandler(TimerTick);
    }
    public void SetTime(TimeSpan ts)
    {
      _max = ts;
      TimeChanged?.Invoke();
    }
    public void SetTime(int min, int sec = 0) => SetTime(TimeSpan.FromSeconds(min * 60 + sec));
    //public void SubtractTime(int min, int sec = 0) => _stpWatch.Elapsed.Add(TimeSpan.FromSeconds(min * 60 + sec));
    public void SubtractTime(int min, int sec = 0)
    {
      SetTime(_max - TimeSpan.FromSeconds(min * 60 + sec));
    }
    

    public void Start()
    {
      timer.Start();
      _stpWatch.Start();
    }
    public void Pause()
    {
      timer.Stop();
      _stpWatch.Stop();
    }
    public void Stop()
    {
      Reset();
      Pause();
    }

    public void Reset()
    {
      _stpWatch.Reset();
    }

    public void Restart()
    {
      _stpWatch.Reset();
      timer.Start();
    }

    public void Dispose()
    {
      _stpWatch.Reset();
      _stpWatch = null;
      timer.Dispose();
    }

  }
}
