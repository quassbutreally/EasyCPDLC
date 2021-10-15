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



using System.Collections.Generic;

namespace EasyCPDLC
{
    public class Pilots
    {
        public IList<PilotData> pilots { get; set; }
    }


    public class PilotData
    {
        public int cid { get; set; }
        public string name { get; set; }
        public string callsign { get; set; }
        public string server { get; set; }
        public int pilot_rating { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public int altitude { get; set; }
        public int groundspeed { get; set; }
        public string transponder { get; set; }
        public int heading { get; set; }
        public float qnh_i_hg { get; set; }
        public int qnh_mb { get; set; }
        public FlightPlan flight_plan { get; set; }
        public string logon_time { get; set; }
        public string last_updated { get; set; }
    }
    public class FlightPlan
    {
        public string flight_rules { get; set; }
        public string aircraft { get; set; }
        public string aircraft_faa { get; set; }
        public string aircraft_short { get; set; }
        public string departure { get; set; }
        public string arrival { get; set; }
        public string alternate { get; set; }
        public string cruise_tas { get; set; }
        public string altitude { get; set; }
        public string deptime { get; set; }
        public string enroute_time { get; set; }
        public string fuel_time { get; set; }
        public string remarks { get; set; }
        public string route { get; set; }
        public int revision_id { get; set; }
        public string assigned_transponder { get; set; }
    }
}
