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

    public partial class SettingsForm : Form
    {
        private MainForm parent;
        public bool[] settings;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        private const int cGrip = 16;
        private const int cCaption = 32;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        public SettingsForm(MainForm _parent, bool[] _settings)
        {
            parent = _parent;
            settings = _settings;
            InitializeComponent();
            InitialiseSettings();
        }

        private void InitialiseSettings()
        {
            settingsFormatPanel.Controls.Clear();
            UICheckBox stayOnTopBox = createCheckBox("Keep Window On Top", "0");
            stayOnTopBox.Checked = settings[0];
            UICheckBox audiblePingBox = createCheckBox("Play Sound on Message Receive", "1");
            audiblePingBox.Checked = settings[1];

            settingsFormatPanel.Controls.Add(stayOnTopBox);
            settingsFormatPanel.SetFlowBreak(stayOnTopBox, true);
            settingsFormatPanel.Controls.Add(audiblePingBox);
        }

        private void ModifySettings(object sender, EventArgs e)
        {
            UICheckBox box = (UICheckBox)sender;
            settings[Convert.ToInt32(box.group)] = box.Checked;
            Console.WriteLine(settings[Convert.ToInt32(box.group)]);
        }
        private UICheckBox createCheckBox(string _text, string _group)
        {
            UICheckBox _temp = new UICheckBox(_group);

            _temp.BackColor = parent.controlBackColor;
            _temp.ForeColor = parent.controlFrontColor;
            _temp.Font = parent.textFont;
            _temp.Text = _text;
            _temp.Padding = new Padding(3, 10, 3, -30);
            _temp.AutoSize = true;
            _temp.Click += ModifySettings;
            return _temp;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;   
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void WindowDrag(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
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
    }
}
