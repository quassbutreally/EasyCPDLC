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
        public Fix[] Fix { get; set; }
    }

    public class Fix
    {
        public Fix(string mora)
        {
            this.Mora = mora;
        }

        public string Ident { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public object Frequency { get; set; }
        public string Pos_lat { get; set; }
        public string Pos_long { get; set; }
        public string Stage { get; set; }
        public string ViaAirway { get; set; }
        public string IsSidStar { get; set; }
        public string Distance { get; set; }
        public string TrackTrue { get; set; }
        public string TrackMag { get; set; }
        public string HeadingTrue { get; set; }
        public string HeadingMag { get; set; }
        public string AltitudeFeet { get; set; }
        public string IndAirspeed { get; set; }
        public string TrueAirspeed { get; set; }
        public string Mach { get; set; }
        public string MachThousandths { get; set; }
        public string WindComponent { get; set; }
        public string Groundspeed { get; set; }
        public string TimeLeg { get; set; }
        public string TimeTotal { get; set; }
        public string FuelFlow { get; set; }
        public string FuelLeg { get; set; }
        public string FuelTotalused { get; set; }
        public string FuelMinOnboard { get; set; }
        public string FuelPlanOnboard { get; set; }
        public string Oat { get; set; }
        public string OatIsaDev { get; set; }
        public string WindDir { get; set; }
        public string WindSpd { get; set; }
        public string Shear { get; set; }
        public string TropopauseFeet { get; set; }
        public string GroundHeight { get; set; }
        public string Mora { get; set; }
        public string Fir { get; set; }
        public string FirUnits { get; set; }
        public string FirValidLevels { get; set; }
        public object WindData { get; set; }
        public object FirCrossing { get; set; }
    }
}