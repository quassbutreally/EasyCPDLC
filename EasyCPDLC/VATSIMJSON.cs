/*  EASYCPDLC: CPDLC Client for the VATSIM Network
    Copyright (C) 2026 Joshua Seagrave joshseagrave@googlemail.com

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

// ReSharper disable InconsistentNaming

namespace EasyCPDLC;

public class VatsimRootobject
{
    public Pilot[] pilots { get; init; }
}

public class Pilot
{
    public int cid { get; init; }
    public string callsign { get; init; }
    public FlightPlan flight_plan { get; init; }

}
public class FlightPlan
{
    public string aircraft_short { get; init; }
    public string departure { get; init; }
    public string arrival { get; init; }

    public string altitude { get; init; }
}