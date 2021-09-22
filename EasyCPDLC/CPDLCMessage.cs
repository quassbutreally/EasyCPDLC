namespace EasyCPDLC
{
    class CPDLCMessage : System.Windows.Forms.Label
    {

        public string type;
        public bool acknowledged = false;
        public string recipient;
        public bool outbound;
        public CPDLCMessage(string _type, string _recipient, bool _outbound = false)
        {
            type = _type;
            recipient = _recipient;
            outbound = _outbound;
        }
    }
}
