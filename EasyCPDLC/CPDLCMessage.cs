namespace EasyCPDLC
{
    class CPDLCMessage : System.Windows.Forms.Label
    {

        public string type;
        public bool acknowledged = false;
        public string recipient;
        public CPDLCMessage(string _type, string _recipient)
        {
            type = _type;
            recipient = _recipient;
        }
    }
}
