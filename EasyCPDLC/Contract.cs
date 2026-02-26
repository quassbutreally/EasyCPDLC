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
using System.Threading.Tasks;
using System.Timers;
using FSUIPC;

namespace EasyCPDLC
{
    internal class Contract(
        string sender,
        string contractLength,
        Func<string, string, string, Task> sendMessage,
        Func<FsuipcData> getFsuipc,
        Func<string> getCallsign)
    {
        public string Sender { get; } = sender;
        public string ContractLength { get; } = contractLength;

        private Timer _timer;

        public void StartContract()
        {
            _timer = new Timer(Convert.ToInt32(ContractLength) * 1000)
            {
                AutoReset = true
            };
            _timer.Elapsed += SendReply;
            _timer.Start();
        }

        private async void SendReply(object source, ElapsedEventArgs e)
        {
            try
            {
                if (!MainForm.UseFsuipc) return;
                var fsuipc  = getFsuipc();
                var message =
                    $"REPORT {getCallsign()} {DateTime.UtcNow:ddHHmm} {Math.Round(fsuipc.position.Latitude.DecimalDegrees, 5)} {Math.Round(fsuipc.position.Longitude.DecimalDegrees, 5)} {Math.Round(fsuipc.altitude.Feet / 100)}";

                await sendMessage(Sender, "ADS-C", message);
            }
            catch
            {
                // ignored
            }
        }

        public void StopContract()
        {
            _timer.Stop();
            _timer.Dispose();
        }
    }
}