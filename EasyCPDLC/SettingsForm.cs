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
using System.Drawing;
using System.Windows.Forms;

namespace EasyCPDLC;

public partial class SettingsForm
{
    private UiCheckBox stayOnTopBox;
    private UiCheckBox audiblePingBox;
    private UiCheckBox useFsuipcBox;
    private UiTextBox simbriefTextBox;

    private readonly MainForm parent;

    public SettingsForm(MainForm parentForm)
    {
        parent = parentForm;
        Theme = new Theme(parentForm.controlBackColor, parentForm.controlFrontColor, parentForm.textFont, parentForm.textFontBold);
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
        useFsuipcBox = CreateCheckBox("Use Simulator Connection (req. FSUIPC/XPUIPC)", "2");
        useFsuipcBox.Checked = MainForm.UseFsuipc;
        simbriefTextBox = CreateTextBox(MainForm.SimbriefId, 7, false, true);

        settingsFormatPanel.Controls.Add(stayOnTopBox);
        settingsFormatPanel.SetFlowBreak(stayOnTopBox, true);
        settingsFormatPanel.Controls.Add(audiblePingBox);
        settingsFormatPanel.SetFlowBreak(audiblePingBox, true);
        settingsFormatPanel.Controls.Add(useFsuipcBox);
        settingsFormatPanel.SetFlowBreak(useFsuipcBox, true);
        settingsFormatPanel.Controls.Add(CreateLabel("SIMBRIEF PILOT ID: "));
        settingsFormatPanel.Controls.Add(simbriefTextBox);
    }

    private void CancelButton_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    private void OkButton_Click(object sender, EventArgs e)
    {
        parent.StayOnTop = stayOnTopBox.Checked;
        MainForm.PlaySound = audiblePingBox.Checked;
        MainForm.UseFsuipc = useFsuipcBox.Checked;
        MainForm.SimbriefId = simbriefTextBox.Text;

        Properties.Settings.Default.Save();
        this.Close();
    }

    private void ExitButton_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    private void WindowDrag(object sender, MouseEventArgs e) => DragWindow(sender, e);
}