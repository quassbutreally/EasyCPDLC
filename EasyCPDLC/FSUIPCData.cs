using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSUIPC;

namespace EasyCPDLC
{
    public class FSUIPCData
    {
        public FsAltitude altitude;
        public int groundspeed;
        public FsLatLonPoint position;
        FsLatitude latitude;
        FsLongitude longitude;
        int pDelta;

        readonly Offset<int> altOffset = new Offset<int>(0x3324);
        readonly Offset<ushort> pOffset = new Offset<ushort>(0x0330);
        readonly Offset<uint> gsOffset = new Offset<uint>(0x02B4);
        readonly Offset<short> feetOrMeters = new Offset<short>(0x0C18);
        readonly Offset<long> latOffset = new Offset<long>(0x0560);
        readonly Offset<long> lonOffset = new Offset<long>(0x0568);

        public bool OpenConnection()
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

        public bool CloseConnection()
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

        public Task RefreshData()
        {
            if (FSUIPCConnection.IsOpen)
            {
                FSUIPCConnection.Process();
                latitude = new FsLatitude(latOffset.Value);
                longitude = new FsLongitude(lonOffset.Value);
                position = new FsLatLonPoint(latitude, longitude);
                pDelta = pOffset.Value / 16 - 1013;
                altitude = feetOrMeters.Value > 1 ? FsAltitude.FromMetres(altOffset.Value - (10 * pDelta)) : FsAltitude.FromFeet(altOffset.Value - (30 * pDelta));
                groundspeed = (int)Math.Round(gsOffset.Value / 65536 * 1.94384);
                
            }
            else
            {
                throw new FSUIPCException(FSUIPCError.FSUIPC_ERR_NOTOPEN, "Can't Refresh, Connection isn't open");
            }

            return Task.CompletedTask;
        }

    }
}
