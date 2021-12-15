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
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyCPDLC
{

    public partial class TelexForm : Form
    {

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        private const int cGrip = 16;
        private const int cCaption = 32;

        private VATSIMRootobject atisList;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

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

            this.TopMost = parent.TopMost;
        }

        private Label CreateTemplate(string _text)
        {
            Label _temp = new Label
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
                Margin = new Padding(0, 0, 0, 0)
            };

            return _temp;
        }

        private UITextBox CreateTextBox(string _text, int _maxLength)
        {
            UITextBox _temp = new UITextBox(controlFrontColor)
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
                TextAlign = HorizontalAlignment.Center
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
            UITextBox _temp = new UITextBox(controlFrontColor)
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
                Height = 20
            };
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

        private void ScrollToBottom(FlowLayoutPanel p)
        {
            using (Control c = new Control() { Parent = p, Dock = DockStyle.Bottom })
            {
                p.ScrollControlIntoView(c);
                c.Parent = null;
            }
        }

        private async void sendButton_Click(object sender, EventArgs e)
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
                        Task.Run(() => this.parent.SendCPDLCMessage(_recipient, "TELEX", _formatMessage.Trim()));
                        break;

                    case "metarRadioButton":

                        this.parent.WriteMessage(String.Format("METAR REQUESTED FOR {0}", _recipient), "SYSTEM", "SYSTEM");

                        try
                        {
                            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(String.Format("https://avwx.rest/api/metar/{0}", _recipient));
                            request.Method = "GET";
                            request.Headers["Authorization"] = "OE1u2m0EZie5oFrAwSb-GYPGqV-9ASDNfuZhrBexzjM";
                            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                            {
                                Stream dataStream = response.GetResponseStream();
                                StreamReader reader = new StreamReader(dataStream);
                                Metar metar = JsonSerializer.Deserialize<Metar>(reader.ReadToEnd());
                                Task.Run(() => this.parent.ArtificialDelay(metar.sanitized, "SYSTEM", "SYSTEM"));
                            }
                        }

                        catch
                        {
                            Task.Run(() => this.parent.ArtificialDelay(String.Format("ERROR RETRIEVING METAR FOR {0}", _recipient), "SYSTEM", "METAR"));
                        }

                        break;

                    case "atisRadioButton":

                        this.parent.WriteMessage(String.Format("ATIS REQUESTED FOR {0}", _recipient), "SYSTEM", "SYSTEM");

                        try
                        {
                            using (WebClient wc = new WebClient())
                            {
                                var json = wc.DownloadString("https://data.vatsim.net/v3/vatsim-data.json");
                                atisList = JsonSerializer.Deserialize<VATSIMRootobject>(json);
                            }

                            Atis atisStation = atisList.atis.Where(i => i.callsign == String.Format("{0}_ATIS", _recipient)).FirstOrDefault();
                            if (atisStation != default(Atis))
                            {
                                string atisData = String.Join(" ", atisStation.text_atis);
                                Task.Run(() => this.parent.ArtificialDelay(atisData, "SYSTEM", "ATIS"));
                            }
                            else
                            {
                                throw new NullReferenceException();
                            }
                        }

                        catch
                        {
                            Task.Run(() => this.parent.ArtificialDelay(String.Format("NO ATIS AVAILABLE FOR {0}", _recipient), "SYSTEM", "ATIS"));
                        }

                        break;

                    default:
                        break;
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
                Point pos = new Point(m.LParam.ToInt32());
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

        private void freeTextButton_Click(object sender, EventArgs e)
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

        private void metarButton_Click(object sender, EventArgs e)
        {
            messageFormatPanel.Controls.Clear();
            messageFormatPanel.Controls.Add(CreateTemplate("STATION:"));
            messageFormatPanel.Controls.Add(CreateTextBox("", 4));

            metarRadioButton.Checked = true;
        }

        private void atisButton_Click(object sender, EventArgs e)
        {
            messageFormatPanel.Controls.Clear();
            messageFormatPanel.Controls.Add(CreateTemplate("STATION:"));
            messageFormatPanel.Controls.Add(CreateTextBox("", 4));

            atisRadioButton.Checked = true;
        }
    }
}
