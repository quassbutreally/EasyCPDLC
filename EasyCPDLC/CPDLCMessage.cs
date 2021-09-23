namespace EasyCPDLC
{
    class CPDLCMessage : System.Windows.Forms.Label
    {

        public string type;
        public bool acknowledged = false;
        public string recipient;
        public bool outbound;
        public CPDLCResponse header;
        public CPDLCMessage(string _type, string _recipient, bool _outbound = false, CPDLCResponse _header = null)
        {
            type = _type;
            recipient = _recipient;
            outbound = _outbound;
            header = _header;
        }
    }

    public class CPDLCResponse
    {
        public string dataType { get; set; }
        public int messageID { get; set; }
        public int responseID { get; set; }
        public string responses { get; set; }
    }
}
