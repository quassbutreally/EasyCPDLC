/*  EASYCPDLC: CPDLC Client for the VATSIM Network
    Copyright (C) 2021 Joshua Seagrave joshseagrave@googlemail.com

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/



using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FSUIPC;

namespace EasyCPDLC
{

    public partial class TelexForm : Form
    {

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        private const int cGrip = 16;
        private const int cCaption = 32;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        private bool isReply = false;

        private readonly MainForm parent;
        private readonly Color controlBackColor;
        private readonly Color controlFrontColor;
        private readonly Font textFont;
        private readonly Font textFontBold;
        private readonly string recipient;
        public TelexForm(MainForm _parent, string _recipient = null)
        {
            InitializeComponent();
            parent = _parent;
            controlBackColor = parent.controlBackColor;
            controlFrontColor = parent.controlFrontColor;
            textFont = parent.textFont;
            textFontBold = parent.textFontBold;
            recipient = _recipient is null ? null : _recipient;
            isReply = _recipient is null ? false : true;

            this.TopMost = parent.TopMost;
        }

        private AccessibleLabel CreateTemplate(string _text)
        {
            AccessibleLabel _temp = new(controlFrontColor)
            {
                BackColor = controlBackColor,
                ForeColor = controlFrontColor,
                Font = textFont,
                AutoSize = true,
                Text = _text,
                Top = 10,
                Height = 20,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(0, 10, 0, 0),
                Margin = new Padding(0, 0, 0, 0),
                TabStop = true,
                TabIndex = 0
            };

            return _temp;
        }

        private UITextBox CreateTextBox(string _text, int _maxLength)
        {
            UITextBox _temp = new(controlFrontColor)
            {
                BackColor = controlBackColor,
                ForeColor = controlFrontColor,
                Font = textFontBold,
                MaxLength = _maxLength,
                BorderStyle = BorderStyle.None,
                Text = _text,
                CharacterCasing = CharacterCasing.Upper,
                Top = 10,
                Padding = new Padding(3, 0, 3, -10),
                Margin = new Padding(3, 5, 3, -10),
                Height = 20,
                TextAlign = HorizontalAlignment.Center,
                TabIndex = 0
            };

            using (Graphics G = _temp.CreateGraphics())
            {
                _temp.Width = (int)(_temp.MaxLength *
                              G.MeasureString("x", _temp.Font).Width);
            }

            return _temp;
        }


        private UITextBox CreateMultiLineBox(string _text)
        {
            UITextBox _temp = new(controlFrontColor)
            {
                BackColor = controlBackColor,
                ForeColor = controlFrontColor,
                Font = textFontBold,
                BorderStyle = BorderStyle.None,
                Width = messageFormatPanel.Width - 50,
                Multiline = true,
                WordWrap = true,
                Text = _text,
                MaxLength = 255,
                Height = 20,
                TabIndex = 0
            };
            _temp.TextChanged += ExpandMultiLineBox;

            _temp.CharacterCasing = CharacterCasing.Upper;
            _temp.Padding = new Padding(3, 0, 3, -10);
            _temp.Margin = new Padding(3, 5, 3, -10);
            _temp.TextAlign = HorizontalAlignment.Left;

            return _temp;
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WindowDrag(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                _ = SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        private void ReloadPanel(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.TelexWindowLocation != new Point(0, 0))
            {
                Location = Properties.Settings.Default.TelexWindowLocation;
                Size = Properties.Settings.Default.TelexWindowSize;
            }
            freeTextButton.PerformClick();
        }

        private void ResetPanel(object sender, EventArgs e)
        {
            freeTextButton.PerformClick();
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

        private static void ScrollToBottom(FlowLayoutPanel p)
        {
            using Control c = new() { Parent = p, Dock = DockStyle.Bottom };
            p.ScrollControlIntoView(c);
            c.Parent = null;
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            RadioButton radioBtn = radioContainer.Controls.OfType<RadioButton>()
                                       .Where(x => x.Checked).FirstOrDefault();

            if (radioBtn != null || messageFormatPanel.Controls[1].Text.Length < 4)
            {

                string _recipient = messageFormatPanel.Controls[1].Text;

                switch (radioBtn.Name)
                {
                    case "freeTextRadioButton":
                        string _formatMessage = messageFormatPanel.Controls[3].Text;
                        _ = Task.Run(() => this.parent.SendCPDLCMessage(_recipient, "TELEX", _formatMessage.Trim()));
                        break;

                    case "metarRadioButton":
                        this.parent.WriteMessage("METAR REQUEST", "METAR", _recipient, true);
                        this.parent.ArtificialDelay("METAR " + _recipient, "INFOREQ", "REQUEST");

                        break;

                    case "atisRadioButton":

                        this.parent.WriteMessage("ATIS REQUEST", "ATIS", _recipient, true);
                        this.parent.ArtificialDelay("VATATIS " + _recipient, "INFOREQ", "REQUEST");

                        break;

                    default:
                        break;
                }
                if(isReply)
                {
                    parent.ClearPreview();
                }
                this.Close();


            }
            else
            {

            }


        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x84)
            {  // Trap WM_NCHITTEST
                Point pos = new(m.LParam.ToInt32());
                pos = this.PointToClient(pos);
                if (pos.Y < cCaption)
                {
                    m.Result = (IntPtr)2;  // HTCAPTION
                    return;
                }
                if (pos.X >= this.ClientSize.Width - cGrip && pos.Y >= this.ClientSize.Height - cGrip)
                {
                    m.Result = (IntPtr)17; // HTBOTTOMRIGHT
                    return;
                }
            }
            base.WndProc(ref m);
        }

        private void FreeTextButton_Click(object sender, EventArgs e)
        {
            messageFormatPanel.Controls.Clear();
            messageFormatPanel.Controls.Add(CreateTemplate("RECIPIENT:"));
            messageFormatPanel.Controls.Add(CreateTextBox(recipient is null ? "" : recipient, 7));
            messageFormatPanel.SetFlowBreak(messageFormatPanel.Controls[messageFormatPanel.Controls.Count - 1], true);
            messageFormatPanel.Controls.Add(CreateTemplate("MSG:"));
            messageFormatPanel.SetFlowBreak(messageFormatPanel.Controls[messageFormatPanel.Controls.Count - 1], true);
            messageFormatPanel.Controls.Add(CreateMultiLineBox(""));

            freeTextRadioButton.Checked = true;
        }

        private void MetarButton_Click(object sender, EventArgs e)
        {
            messageFormatPanel.Controls.Clear();
            messageFormatPanel.Controls.Add(CreateTemplate("STATION:"));
            try
            {
                if (parent.fsuipc.groundspeed < 100)
                {
                    messageFormatPanel.Controls.Add(CreateTextBox(parent.userVATSIMData.flight_plan.departure, 4));
                }
                else
                {
                    messageFormatPanel.Controls.Add(CreateTextBox(parent.userVATSIMData.flight_plan.arrival, 4));
                }
            }
            catch
            {
                messageFormatPanel.Controls.Add(CreateTextBox("", 4));
            }


            metarRadioButton.Checked = true;
        }

        private void AtisButton_Click(object sender, EventArgs e)
        {
            messageFormatPanel.Controls.Clear();
            messageFormatPanel.Controls.Add(CreateTemplate("STATION:"));
            try
            {
                if (parent.fsuipc.groundspeed < 100)
                {
                    messageFormatPanel.Controls.Add(CreateTextBox(parent.userVATSIMData.flight_plan.departure, 4));
                }
                else
                {
                    messageFormatPanel.Controls.Add(CreateTextBox(parent.userVATSIMData.flight_plan.arrival, 4));
                }
            }
            catch
            {
                messageFormatPanel.Controls.Add(CreateTextBox("", 4));
            }
            

            atisRadioButton.Checked = true;
        }

        private void TelexForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.TelexWindowLocation = Location;
            Properties.Settings.Default.TelexWindowSize = Size;
            Properties.Settings.Default.Save();
        }
    }
}
