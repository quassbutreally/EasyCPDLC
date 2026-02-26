/*  EASYCPDLC: CPDLC Client for the VATSIM Network
    Copyright (C) 2026 Joshua Seagrave joshseagrave@googlemail.com

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
using System.Windows.Forms;
using System.ComponentModel;

namespace EasyCPDLC
{
    public partial class DataEntry : Form
    {
        private const int WmNclbuttondown = 0xA1;
        private const int HtCaption = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ReleaseCapture();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string HoppieLogonCode { get; private set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int VatsimCid { get; private set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Remember { get; private set; }
        public DataEntry(object hoppieLogonCode = null, object vatsimCid = null)
        {
            InitializeComponent();
    
            // Set tab order
            hoppieCodeTextBox.TabIndex = 0;
            vatsimCIDTextBox.TabIndex = 1;
            rememberCheckBox.TabIndex = 2;
            connectButton.TabIndex = 3;
    
            // Set accessible properties
            hoppieCodeTextBox.AccessibleName = @"Hoppie Logon Code";
            hoppieCodeTextBox.AccessibleDescription = @"Enter your Hoppie ACARS logon code";
            vatsimCIDTextBox.AccessibleName = @"VATSIM CID";
            vatsimCIDTextBox.AccessibleDescription = @"Enter your VATSIM CID number";
            rememberCheckBox.AccessibleName = @"Remember credentials";
            rememberCheckBox.AccessibleDescription = @"Save credentials for next time";
            connectButton.AccessibleName = @"Connect";
    
            // Set as default button
            AcceptButton = connectButton;
    
            // Handle Enter/Escape
            KeyPreview = true;
            KeyDown += (s, e) => {
                if (e.KeyCode == Keys.Escape) this.Close();
            };

            try
            {
                hoppieCodeTextBox.Text = hoppieLogonCode?.ToString() ?? throw new InvalidOperationException();
                vatsimCIDTextBox.Text = vatsimCid?.ToString() ?? throw new InvalidOperationException();
                rememberCheckBox.Checked = true;
            }

            catch (Exception)
            {
                hoppieCodeTextBox.Text = "";
                vatsimCIDTextBox.Text = "";
                rememberCheckBox.Checked = false;
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            try
            {
                HoppieLogonCode = hoppieCodeTextBox.Text;
                VatsimCid = Convert.ToInt32(vatsimCIDTextBox.Text);
                Remember = rememberCheckBox.Checked;

                this.DialogResult = DialogResult.OK;
            }
            catch (FormatException)
            {
                MessageBox.Show(@"Invalid CID/Code, please check and try again.", @"Error!", MessageBoxButtons.OK);
            }

        }

        private void HoppieCodeTextBox_TextChanged(object sender, EventArgs e)
        {

            if (vatsimCIDTextBox.Text.Length < 1 || hoppieCodeTextBox.Text.Length < 1 || !int.TryParse(vatsimCIDTextBox.Text, out _))
            {
                connectButton.Enabled = false;
                return;
            }


            connectButton.Enabled = true;
        }

        private void VatsimCIDTextBox_TextChanged(object sender, EventArgs e)
        {
            if (vatsimCIDTextBox.Text.Length < 1 || hoppieCodeTextBox.Text.Length < 1 || !int.TryParse(vatsimCIDTextBox.Text, out _))
            {
                connectButton.Enabled = false;
                return;
            }


            connectButton.Enabled = true;
        }

        private void DataEntry_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            ReleaseCapture();
            _ = SendMessage(Handle, WmNclbuttondown, HtCaption, 0);
        }

        private void NumsOnly(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}