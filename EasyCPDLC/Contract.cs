using System;
using System.Timers;

namespace EasyCPDLC
{
    class Contract
    {
        public string sender;
        public string contractLength;
        private Timer timer;
        private MainForm parent;

        public Contract(MainForm _parent, string _sender, string _timeSpan)
        {
            parent = _parent;
            sender = _sender;
            contractLength = _timeSpan;
        }

        public void StartContract()
        {
            timer = new Timer(Convert.ToInt32(contractLength) * 1000);
            timer.AutoReset = true;
            timer.Elapsed += new ElapsedEventHandler(SendReply);
            timer.Start();
        }

        private async void SendReply(Object source, ElapsedEventArgs e)
        {
            try
            {
                if (parent.useFSUIPC)
                {
                    string message = String.Format("REPORT {0} {1} {2} {3} {4}",
                    parent.callsign,
                    DateTime.UtcNow.ToString("ddHHmm"),
                    Math.Round(parent.fsuipc.position.Latitude.DecimalDegrees, 5),
                    Math.Round(parent.fsuipc.position.Longitude.DecimalDegrees, 5),
                    Math.Round(parent.fsuipc.altitude.Feet / 100));
                    Console.WriteLine(message);
                    await parent.SendCPDLCMessage(sender, "ADS-C", message, true, false);
                }
            }
            catch { }
        }

        public void StopContract()
        {
            timer.Stop();
            timer.Dispose();
        }
    }
}
