/*  EASYCPDLC: CPDLC Client for the VATSIM Network
    Copyright (C) 2021 Joshua Seagrave joshseagrave@googlemail.com

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.Timers;

namespace EasyCPDLC
{
    class Contract
    {
        public string sender;
        public string contractLength;
        private Timer timer;
        readonly private MainForm parent;

        public Contract(MainForm _parent, string _sender, string _timeSpan)
        {
            parent = _parent;
            sender = _sender;
            contractLength = _timeSpan;
        }

        public void StartContract()
        {
            timer = new Timer(Convert.ToInt32(contractLength) * 1000)
            {
                AutoReset = true
            };
            timer.Elapsed += new ElapsedEventHandler(SendReply);
            timer.Start();
        }

        private async void SendReply(Object source, ElapsedEventArgs e)
        {
            try
            {
                if (MainForm.UseFSUIPC)
                {
                    string message = String.Format("REPORT {0} {1} {2} {3} {4}",
                    parent.callsign,
                    DateTime.UtcNow.ToString("ddHHmm"),
                    Math.Round(parent.fsuipc.position.Latitude.DecimalDegrees, 5),
                    Math.Round(parent.fsuipc.position.Longitude.DecimalDegrees, 5),
                    Math.Round(parent.fsuipc.altitude.Feet / 100));
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
