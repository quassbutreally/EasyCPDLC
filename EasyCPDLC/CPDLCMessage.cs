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
        private string storedText;

        public CPDLCResponse header;

        public CPDLCMessage()
        {

        }
        public CPDLCMessage(string _type, string _recipient, string _message, bool _outbound = false, CPDLCResponse _header = null)
        {
            type = _type;
            recipient = _recipient;
            message = _message;
            outbound = _outbound;
            header = _header;
            SetStyle(ControlStyles.Selectable, true);
        }

        public void OverrideText(string newText)
        {
            storedText = Text;
            Console.WriteLine(storedText);
            Invoke(new Action(() => Text = newText));
        }

        public void RestoreText()
        {
            Invoke(new Action(() => Text = storedText));
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
        public string DataType { get; set; }
        public int MessageID { get; set; }
        public int ResponseID { get; set; }
        public string Responses { get; set; }

    }

    public class AccessibleLabel : Label
    {
        private readonly Color foreColor;
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
