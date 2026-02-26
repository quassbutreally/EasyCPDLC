using System;
using System.Drawing;
using System.Windows.Forms;

namespace EasyCPDLC
{
    public class CpdlcMessage : Label
    {

        public readonly string type;
        public bool acknowledged = false;
        public readonly string recipient;
        public readonly string contents;
        public readonly bool outbound;
        private string storedText;

        public readonly CpdlcResponse header;
        
        public CpdlcMessage(string msgType, string msgRecipient, string msgContents, bool msgOutbound = false, CpdlcResponse msgHeader = null)
        {
            type = msgType;
            recipient = msgRecipient;
            contents = msgContents;
            outbound = msgOutbound;
            header = msgHeader;
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

    public class CpdlcResponse
    {
        public int MessageId { get; init; }
        public int ResponseId { get; set; }
        public string Responses { get; init; }

    }

    public class AccessibleLabel : Label
    {
        private readonly Color foreColor;
        public AccessibleLabel(Color labelForeColor)
        {
            SetStyle(ControlStyles.Selectable, true);
            foreColor = labelForeColor;
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
