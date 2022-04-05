using System;

namespace EasyCPDLC
{

    public class VATSIMRootobject
    {
        public VATSIMGeneral General { get; set; }
        public Pilot[] Pilots { get; set; }
        public Controller[] Controllers { get; set; }
        public Atis[] Atis { get; set; }
        public Server[] Servers { get; set; }
        public Prefile[] Prefiles { get; set; }
        public Facility[] Facilities { get; set; }
        public Rating[] Ratings { get; set; }
        public Pilot_Ratings[] PilotRatings { get; set; }
    }

    public class VATSIMGeneral
    {
        public int Version { get; set; }
        public int Reload { get; set; }
        public string Update { get; set; }
        public DateTime UpdateTimestamp { get; set; }
        public int ConnectedClients { get; set; }
        public int UniqueUsers { get; set; }
    }

    public class Pilot
    {
        public int Cid { get; set; }
        public string Name { get; set; }
        public string Callsign { get; set; }
        public string Server { get; set; }
        public int PilotRating { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int Altitude { get; set; }
        public int Groundspeed { get; set; }
        public string Transponder { get; set; }
        public int Heading { get; set; }
        public float QnhIHg { get; set; }
        public int QnhMb { get; set; }
        public Flight_Plan FlightPlan { get; set; }
        public DateTime LogonTime { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class Flight_Plan
    {
        public string FlightRules { get; set; }
        public string Aircraft { get; set; }
        public string AircraftFaa { get; set; }
        public string AircraftShort { get; set; }
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public string Alternate { get; set; }
        public string CruiseTas { get; set; }
        public string Altitude { get; set; }
        public string Deptime { get; set; }
        public string EnrouteTime { get; set; }
        public string FuelTime { get; set; }
        public string Remarks { get; set; }
        public string Route { get; set; }
        public int RevisionId { get; set; }
        public string AssignedTransponder { get; set; }
    }

    public class Controller
    {
        public int Cid { get; set; }
        public string Name { get; set; }
        public string Callsign { get; set; }
        public string Frequency { get; set; }
        public int Facility { get; set; }
        public int Rating { get; set; }
        public string Server { get; set; }
        public int VisualRange { get; set; }
        public string[] TextAtis { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime LogonTime { get; set; }
    }

    public class Atis
    {
        public int Cid { get; set; }
        public string Name { get; set; }
        public string Callsign { get; set; }
        public string Frequency { get; set; }
        public int Facility { get; set; }
        public int Rating { get; set; }
        public string Server { get; set; }
        public int VisualRange { get; set; }
        public string AtisCode { get; set; }
        public string[] TextAtis { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime LogonTime { get; set; }
    }

    public class Server
    {
        public string Ident { get; set; }
        public string HostnameOrIp { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public int ClientsConnectionAllowed { get; set; }
        public bool ClientConnectionsAllowed { get; set; }
        public bool IsSweatbox { get; set; }
    }

    public class Prefile
    {
        public int Cid { get; set; }
        public string Name { get; set; }
        public string Callsign { get; set; }
        public VATSIMFlight_Plan FlightPlan { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class VATSIMFlight_Plan
    {
        public string FlightRules { get; set; }
        public string Aircraft { get; set; }
        public string AircraftFaa { get; set; }
        public string AircraftShort { get; set; }
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public string Alternate { get; set; }
        public string Cruise_tas { get; set; }
        public string Altitude { get; set; }
        public string Deptime { get; set; }
        public string EnrouteTime { get; set; }
        public string FuelTime { get; set; }
        public string Remarks { get; set; }
        public string Route { get; set; }
        public int RevisionId { get; set; }
        public string Assigned_transponder { get; set; }
    }

    public class Facility
    {
        public int Id { get; set; }
        public string Short { get; set; }
        public string Long { get; set; }
    }

    public class Rating
    {
        public int Id { get; set; }
        public string Short { get; set; }
        public string Long { get; set; }
    }

    public class Pilot_Ratings
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
    }

}
