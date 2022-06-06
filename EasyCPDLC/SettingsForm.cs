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
using System.Windows.Forms;

namespace EasyCPDLC
{

    public partial class SettingsForm : Form
    {

        UICheckBox stayOnTopBox;
        UICheckBox audiblePingBox;
        UICheckBox useFSUIPCBox;
        UITextBox simbriefTextBox;

        private readonly MainForm parent;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        private const int cGrip = 16;
        private const int cCaption = 32;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        public SettingsForm(MainForm _parent)
        {
            parent = _parent;
            InitializeComponent();
            InitialiseSettings();
        }

        private void InitialiseSettings()
        {
            settingsFormatPanel.Controls.Clear();
            stayOnTopBox = CreateCheckBox("Keep Window On Top", "0");
            stayOnTopBox.Checked = parent.StayOnTop;
            audiblePingBox = CreateCheckBox("Play Sound on Message Receive", "1");
            audiblePingBox.Checked = MainForm.PlaySound;
            useFSUIPCBox = CreateCheckBox("Use Simulator Connection (req. FSUIPC/XPUIPC)", "2");
            useFSUIPCBox.Checked = MainForm.UseFSUIPC;
            simbriefTextBox = CreateTextBox(MainForm.SimbriefID, 7, false, true);

            settingsFormatPanel.Controls.Add(stayOnTopBox);
            settingsFormatPanel.SetFlowBreak(stayOnTopBox, true);
            settingsFormatPanel.Controls.Add(audiblePingBox);
            settingsFormatPanel.SetFlowBreak(audiblePingBox, true);
            settingsFormatPanel.Controls.Add(useFSUIPCBox);
            settingsFormatPanel.SetFlowBreak(useFSUIPCBox, true);
            settingsFormatPanel.Controls.Add(CreateTemplate("SIMBRIEF PILOT ID: "));
            settingsFormatPanel.Controls.Add(simbriefTextBox);
        }

        private UICheckBox CreateCheckBox(string _text, string _group)
        {
            UICheckBox _temp = new(_group)
            {
                BackColor = parent.controlBackColor,
                ForeColor = parent.controlFrontColor,
                Font = parent.textFont,
                Text = _text,
                Padding = new Padding(3, 10, 3, -30),
                AutoSize = true
            };
            return _temp;
        }

        private Label CreateTemplate(string _text)
        {
            Label _temp = new()
            {
                BackColor = parent.controlBackColor,
                ForeColor = parent.controlFrontColor,
                Font = parent.textFont,
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

        private UITextBox CreateTextBox(string _text, int _maxLength, bool _readOnly = false, bool _numsOnly = false)
        {
            UITextBox _temp = new(parent.controlFrontColor)
            {
                BackColor = parent.controlBackColor,
                ForeColor = parent.controlFrontColor,
                Font = parent.textFontBold,
                MaxLength = _maxLength,
                BorderStyle = BorderStyle.None,
                Text = _text,
                CharacterCasing = CharacterCasing.Upper,
                Top = 10,
                Padding = new Padding(3, 0, 3, -10),
                Height = 20,
                ReadOnly = _readOnly,
                TextAlign = HorizontalAlignment.Center
            };

            if (_numsOnly)
            {
                _temp.KeyPress += NumsOnly;
            }

            using (Graphics G = _temp.CreateGraphics())
            {
                _temp.Width = (int)(_temp.MaxLength *
                              G.MeasureString("x", _temp.Font).Width);
            }

            return _temp;
        }

        private void NumsOnly(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            parent.StayOnTop = stayOnTopBox.Checked;
            MainForm.PlaySound = audiblePingBox.Checked;
            MainForm.UseFSUIPC = useFSUIPCBox.Checked;
            MainForm.SimbriefID = simbriefTextBox.Text;

            Properties.Settings.Default.Save();
            this.Close();
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
    }
}
