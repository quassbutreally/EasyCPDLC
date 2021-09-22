using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyCPDLC
{
    public partial class RequestForm : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private MainForm parent;
        private PilotData userVATSIMData;
        private Color controlBackColor;
        private Color controlFrontColor;
        private Font controlFont;
        private Font controlFontBold;

        public RequestForm(MainForm parent)
        {
            InitializeComponent();
            this.parent = parent;
            userVATSIMData = parent.userVATSIMData;
            controlBackColor = parent.controlBackColor;
            controlFrontColor = parent.controlFrontColor;
            controlFont = parent.controlFont;
            controlFontBold = new Font("Oxygen", 12.5F, FontStyle.Bold);

        }

        private void pdcButton_Click(object sender, EventArgs e)
        {
            messageFormatPanel.Controls.Clear();
            messageFormatPanel.Controls.Add(createTemplate("RECIPIENT:"));
            messageFormatPanel.Controls.Add(createTextBox(userVATSIMData.flight_plan.departure, 4));
            messageFormatPanel.SetFlowBreak(messageFormatPanel.Controls[messageFormatPanel.Controls.Count - 1], true);
            messageFormatPanel.Controls.Add(createTemplate("REQUEST PREDEP CLEARANCE"));
            messageFormatPanel.Controls.Add(createTextBox(userVATSIMData.callsign, 7));
            messageFormatPanel.Controls.Add(createTextBox(userVATSIMData.flight_plan.aircraft_short, 4));
            messageFormatPanel.SetFlowBreak(messageFormatPanel.Controls[messageFormatPanel.Controls.Count - 1], true);
            messageFormatPanel.Controls.Add(createTemplate("TO"));
            messageFormatPanel.Controls.Add(createTextBox(userVATSIMData.flight_plan.arrival, 4));
            messageFormatPanel.Controls.Add(createTemplate("AT"));
            messageFormatPanel.Controls.Add(createTextBox(userVATSIMData.flight_plan.departure, 4));
            messageFormatPanel.SetFlowBreak(messageFormatPanel.Controls[messageFormatPanel.Controls.Count - 1], true);
            messageFormatPanel.Controls.Add(createTemplate("STAND"));
            messageFormatPanel.Controls.Add(createTextBox("", 4));
            messageFormatPanel.Controls.Add(createTemplate("ATIS"));
            messageFormatPanel.Controls.Add(createTextBox("", 1));
        }

        private Label createTemplate(string _text)
        {
            Label _temp = new Label();
            _temp.BackColor = controlBackColor;
            _temp.ForeColor = controlFrontColor;
            _temp.Font = controlFont;
            _temp.AutoSize = true;
            _temp.Text = _text;
            _temp.Top = 10;
            _temp.Height = 20;
            _temp.TextAlign = ContentAlignment.MiddleLeft;
            _temp.Padding = new Padding(0, 10, 0, 0);
            _temp.Margin = new Padding(0, 0, 0, 0);

            return _temp;
        }

        private UITextBox createTextBox(string _text, int _maxLength)
        {
            UITextBox _temp = new UITextBox(controlFrontColor);

            _temp.BackColor = controlBackColor;
            _temp.ForeColor = controlFrontColor;
            _temp.Font = controlFontBold;
            _temp.MaxLength = _maxLength;
            _temp.BorderStyle = BorderStyle.None;
            _temp.Text = _text;
            _temp.CharacterCasing = CharacterCasing.Upper;
            _temp.Top = 10;
            _temp.Padding = new Padding(3, 0, 3, -10);
            _temp.Margin = new Padding(3, 5, 3, -10);
            _temp.Height = 20;
            _temp.TextAlign = HorizontalAlignment.Center;

            using (Graphics G = _temp.CreateGraphics())
            {
                _temp.Width = (int)(_temp.MaxLength *
                              G.MeasureString("x", _temp.Font).Width);
            }

            return _temp;
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            messageFormatPanel.Controls.Clear();
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            string _formatMessage = "";
            string _recipient = messageFormatPanel.Controls[1].Text;

            for(int i = 2; i < messageFormatPanel.Controls.Count; i++)
            {
                _formatMessage += messageFormatPanel.Controls[i].Text + " ";
            }
            this.parent.SendCPDLCMessage(_recipient, "TELEX", _formatMessage.Trim());
            Console.WriteLine(_formatMessage.Trim());
            this.Close();
        }

        private void WindowDrag(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
