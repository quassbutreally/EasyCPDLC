using System;
using System.Drawing;
using System.Windows.Forms;

namespace EasyCPDLC
{

    public partial class TelexForm : Form
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
        private string recipient;
        public TelexForm(MainForm _parent, string _recipient = null)
        {
            InitializeComponent();
            this.parent = _parent;
            userVATSIMData = parent.userVATSIMData;
            controlBackColor = parent.controlBackColor;
            controlFrontColor = parent.controlFrontColor;
            controlFont = parent.controlFont;
            controlFontBold = new Font("Oxygen", 12.5F, FontStyle.Bold);
            recipient = _recipient is null ? null : _recipient;
        }

        private Label CreateTemplate(string _text)
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

        private UITextBox CreateTextBox(string _text, int _maxLength)
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

        private UITextBox CreateMultiLineBox(string _text)
        {
            UITextBox _temp = new UITextBox(controlFrontColor);
            _temp.BackColor = controlBackColor;
            _temp.ForeColor = controlFrontColor;
            _temp.Font = controlFontBold;
            _temp.BorderStyle = BorderStyle.None;
            _temp.Width = messageFormatPanel.Width - 50;
            _temp.Multiline = true;
            _temp.WordWrap = true;
            _temp.Text = _text;
            _temp.MaxLength = 255;
            _temp.Height = 20;
            _temp.TextChanged += ExpandMultiLineBox;

            _temp.CharacterCasing = CharacterCasing.Upper;
            _temp.Padding = new Padding(3, 0, 3, -10);
            _temp.Margin = new Padding(3, 5, 3, -10);
            _temp.TextAlign = HorizontalAlignment.Left;

            return _temp;
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
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
        private void ReloadPanel(object sender, EventArgs e)
        {
            messageFormatPanel.Controls.Clear();
            messageFormatPanel.Controls.Add(CreateTemplate("RECIPIENT:"));
            messageFormatPanel.Controls.Add(CreateTextBox(recipient is null ? "" : recipient, 7));
            messageFormatPanel.SetFlowBreak(messageFormatPanel.Controls[messageFormatPanel.Controls.Count - 1], true);
            messageFormatPanel.Controls.Add(CreateTemplate("MSG:"));
            messageFormatPanel.SetFlowBreak(messageFormatPanel.Controls[messageFormatPanel.Controls.Count - 1], true);
            messageFormatPanel.Controls.Add(CreateMultiLineBox(""));
        }

        private void ExpandMultiLineBox(object sender, EventArgs e)
        {
            TextBox _sender = (TextBox)sender;
            // amount of padding to add
            const int padding = 3;
            // get number of lines (first line is 0, so add 1)
            int numLines = _sender.GetLineFromCharIndex(_sender.TextLength) + 1;
            // get border thickness
            int border = _sender.Height - _sender.ClientSize.Height;
            // set height (height of one line * number of lines + spacing)
            _sender.Height = _sender.Font.Height * numLines + padding + border;
            ScrollToBottom(messageFormatPanel);
        }

        private void ScrollToBottom(FlowLayoutPanel p)
        {
            using (Control c = new Control() { Parent = p, Dock = DockStyle.Bottom })
            {
                p.ScrollControlIntoView(c);
                c.Parent = null;
            }
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            string _formatMessage = messageFormatPanel.Controls[3].Text;
            string _recipient = messageFormatPanel.Controls[1].Text;

            this.parent.SendCPDLCMessage(_recipient, "TELEX", _formatMessage.Trim());
            Console.WriteLine(_formatMessage.Trim());
            this.Close();
        }
    }
}
