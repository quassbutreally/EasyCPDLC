#pragma warning disable IDE1006 // Naming Styles

/*  EASYCPDLC: CPDLC Client for the VATSIM Network
    Copyright (C) 2022 Joshua Seagrave joshseagrave@googlemail.com

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

namespace EasyCPDLC
{

    public class VATSIMRootobject
    {
        public VATSIMGeneral general { get; set; }
        public Pilot[] pilots { get; set; }
        public Controller[] controllers { get; set; }
        public Atis[] atis { get; set; }
        public Server[] servers { get; set; }
        public Prefile[] prefiles { get; set; }
        public Facility[] facilities { get; set; }
        public Rating[] ratings { get; set; }
        public Pilot_Ratings[] pilot_ratings { get; set; }
    }

    public class VATSIMGeneral
    {

        public int version { get; set; }
        public int reload { get; set; }
        public string update { get; set; }
        public DateTime update_timestamp { get; set; }
        public int connected_clients { get; set; }
        public int unique_users { get; set; }
    }

    public class Pilot
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
        public Flight_Plan flight_plan { get; set; }
        public DateTime logon_time { get; set; }
        public DateTime last_updated { get; set; }
    }

    public class Flight_Plan
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

    public class Controller
    {
        public int cid { get; set; }
        public string name { get; set; }
        public string callsign { get; set; }
        public string frequency { get; set; }
        public int facility { get; set; }
        public int rating { get; set; }
        public string server { get; set; }
        public int visual_range { get; set; }
        public string[] text_atis { get; set; }
        public DateTime last_updated { get; set; }
        public DateTime logon_time { get; set; }
    }

    public class Atis
    {
        public int cid { get; set; }
        public string name { get; set; }
        public string callsign { get; set; }
        public string frequency { get; set; }
        public int facility { get; set; }
        public int rating { get; set; }
        public string server { get; set; }
        public int visual_range { get; set; }
        public string atis_code { get; set; }
        public string[] text_atis { get; set; }
        public DateTime last_updated { get; set; }
        public DateTime logon_time { get; set; }
    }

    public class Server
    {
        public string ident { get; set; }
        public string hostname_or_ip { get; set; }
        public string location { get; set; }
        public string name { get; set; }
        public int clients_connection_allowed { get; set; }
        public bool client_connections_allowed { get; set; }
        public bool is_sweatbox { get; set; }
    }

    public class Prefile
    {
        public int cid { get; set; }
        public string name { get; set; }
        public string callsign { get; set; }
        public VATSIMFlight_Plan flight_plan { get; set; }
        public DateTime last_updated { get; set; }
    }

    public class VATSIMFlight_Plan
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

    public class Facility
    {
        public int id { get; set; }
        public string _short { get; set; }
        public string _long { get; set; }
    }

    public class Rating
    {
        public int id { get; set; }
        public string _short { get; set; }
        public string _long { get; set; }
    }

    public class Pilot_Ratings
    {
        public int id { get; set; }
        public string short_name { get; set; }
        public string long_name { get; set; }
    }

}