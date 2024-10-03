using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace DwarfMaze.Custom_Timer
{
    //Custom timer that should be thread-safe using Stopwatch
    public class CustomTimer
    {
        #region Declarations

        private Stopwatch stopwatch;

        private int millisecondsToPass;

        private bool autoReset = false;

        
        public ElapsedEventHandler ElapsedEvent { get; set; }

        public int MillisecondsToPass { get => millisecondsToPass; }

        //AutoReset = false is the only way to stop the timer for now
        public bool AutoReset { get => autoReset; set => autoReset = value; }

        public bool IsRunning { get => stopwatch.IsRunning; }

        #endregion

        #region Class Methods

        public CustomTimer(int milliseconds)
        {
            millisecondsToPass = milliseconds;
            stopwatch = new Stopwatch();
        }

        #endregion

        #region Timer Methods

        public void Start()
        {
            if (stopwatch.IsRunning) return;

            stopwatch.Reset();
            stopwatch.Start();
            StartWaiting();
        }

        #endregion

        #region Internal Clock

        //async/await method that handles stopwatch time, then stops the stopwatch, invokes events, and if AutoReset is true, continues again from 0
        private async void StartWaiting()
        {
            while (stopwatch.IsRunning)
            {
                if (stopwatch.ElapsedMilliseconds >= millisecondsToPass)
                {
                    stopwatch.Reset();
                    ElapsedEvent.Invoke(this, null);

                    if (!autoReset)
                    {
                        return;
                    }
                    
                    stopwatch.Start();
                }
                await Task.Yield();
            }
        }

        #endregion
    }
}
