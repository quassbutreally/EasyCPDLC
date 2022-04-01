using System;
using System.Drawing;
using System.Windows.Forms;

namespace EasyCPDLC
{
    public class CPDLCMessage : Label
    {

        public string type;
        public bool acknowledged = false;
        public string recipient;
        public string message;
        public bool outbound;
        public CPDLCResponse header;
        public CPDLCMessage(string _type, string _recipient, string _message, bool _outbound = false, CPDLCResponse _header = null)
        {
            type = _type;
            recipient = _recipient;
            message = _message;
            outbound = _outbound;
            header = _header;
            SetStyle(ControlStyles.Selectable, true);
        }

        protected override void OnEnter(EventArgs e)
        {
            ForeColor = Color.Orange;
        }

        protected override void OnLeave(EventArgs e)
        {
            ForeColor = SystemColors.ControlDark;
        }
    }

    public class CPDLCResponse
    {
        public string dataType { get; set; }
        public int messageID { get; set; }
        public int responseID { get; set; }
        public string responses { get; set; }
    }

    public class AccessibleLabel : Label
    {
        private Color foreColor;
        public AccessibleLabel(Color _foreColor)
        {
            SetStyle(ControlStyles.Selectable, true);
            foreColor = _foreColor;
        }

        protected override void OnEnter(EventArgs e)
        {
            ForeColor = Color.Orange;
        }

        protected override void OnLeave(EventArgs e)
        {
            ForeColor = foreColor;
        }
    }
}
