using System;
using System.IO;
using System.Timers;

namespace SimpleHeartbeatService
{
    public class Heartbeat
    {
        private readonly Timer timer;
        public Heartbeat()
        {
            timer = new Timer(1000) { AutoReset = true };
            timer.Elapsed += TimerElapsed;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            string[] lines = new string[] { DateTime.Now.ToString() };
            File.AppendAllLines(@"C:\Heartbeat.txt", lines);
        }

        public void Start ()
        {
            timer.Start();
        }

        public void Stop ()
        {
            timer.Stop();
        }
    }
}
