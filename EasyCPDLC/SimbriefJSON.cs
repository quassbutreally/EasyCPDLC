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

namespace EasyCPDLC
{

    public class Navlog
    {
        public Fix[] fix { get; set; }
    }

    public class Fix
    {
        public string ident { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public object frequency { get; set; }
        public string pos_lat { get; set; }
        public string pos_long { get; set; }
        public string stage { get; set; }
        public string via_airway { get; set; }
        public string is_sid_star { get; set; }
        public string distance { get; set; }
        public string track_true { get; set; }
        public string track_mag { get; set; }
        public string heading_true { get; set; }
        public string heading_mag { get; set; }
        public string altitude_feet { get; set; }
        public string ind_airspeed { get; set; }
        public string true_airspeed { get; set; }
        public string mach { get; set; }
        public string mach_thousandths { get; set; }
        public string wind_component { get; set; }
        public string groundspeed { get; set; }
        public string time_leg { get; set; }
        public string time_total { get; set; }
        public string fuel_flow { get; set; }
        public string fuel_leg { get; set; }
        public string fuel_totalused { get; set; }
        public string fuel_min_onboard { get; set; }
        public string fuel_plan_onboard { get; set; }
        public string oat { get; set; }
        public string oat_isa_dev { get; set; }
        public string wind_dir { get; set; }
        public string wind_spd { get; set; }
        public string shear { get; set; }
        public string tropopause_feet { get; set; }
        public string ground_height { get; set; }
        public string mora { get; set; }
        public string fir { get; set; }
        public string fir_units { get; set; }
        public string fir_valid_levels { get; set; }
        public object wind_data { get; set; }
        public object fir_crossing { get; set; }
    }
}