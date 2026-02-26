using System;
using FSUIPC;

namespace EasyCPDLC;

public class FsuipcData
{
    public FsAltitude altitude;
    public int groundspeed;
    public FsLatLonPoint position;
    private FsLatitude latitude;
    private FsLongitude longitude;
    private int pDelta;

    private readonly Offset<int> altOffset = new(0x3324);
    private readonly Offset<ushort> pOffset = new(0x0330);
    private readonly Offset<uint> gsOffset = new(0x02B4);
    private readonly Offset<short> feetOrMeters = new(0x0C18);
    private readonly Offset<long> latOffset = new(0x0560);
    private readonly Offset<long> lonOffset = new(0x0568);

    public static bool OpenConnection()
    {
        try
        {
            FSUIPCConnection.Open();
            if (FSUIPCConnection.IsOpen)
            {
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
            
    }

    public static bool CloseConnection()
    {
        {
            try
            {
                FSUIPCConnection.Close();
                return false;
            }
            catch
            {
                return false;
            }

        }
    }

    public void RefreshData()
    {
        if (FSUIPCConnection.IsOpen)
        {
            FSUIPCConnection.Process();
            latitude = new FsLatitude(latOffset.Value);
            longitude = new FsLongitude(lonOffset.Value);
            position = new FsLatLonPoint(latitude, longitude);
            pDelta = pOffset.Value / 16 - 1013;
            altitude = feetOrMeters.Value > 1 ? FsAltitude.FromMetres(altOffset.Value - (10 * pDelta)) : FsAltitude.FromFeet(altOffset.Value - (30 * pDelta));
            groundspeed = (int)Math.Round(gsOffset.Value / 65536.0 * 1.94384);
                
        }
        else
        {
            throw new FSUIPCException(FSUIPCError.FSUIPC_ERR_NOTOPEN, "Can't Refresh, Connection isn't open");
        }
    }

}