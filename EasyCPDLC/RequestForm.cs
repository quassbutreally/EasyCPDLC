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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Globalization;

namespace EasyCPDLC;

public partial class RequestForm
{
    private readonly ContextMenuStrip popupMenu = new();
    private ToolStripMenuItem directRequestMenu;
    private ToolStripMenuItem levelRequestMenu;
    private ToolStripMenuItem speedRequestMenu;
    private ToolStripMenuItem whenCanWeRequestMenu;

    private readonly ContextMenuStrip clxMenu = new();
    private ToolStripMenuItem depClxMenu;
    private ToolStripMenuItem ocnClxMenu;

    // Position report fields
    private UiTextBox fix1;
    private UiTextBox fix2;
    private UiTextBox fix3;
    private UiTextBox reportAtTime;
    private UiTextBox reportFl;
    private UiTextBox reportNextAtTime;

    // Dep CLX fields
    private UiTextBox depRecipient;
    private UiTextBox depCallsign;
    private UiTextBox depAcType;
    private UiTextBox depArpt;
    private UiTextBox depArrArpt;
    private UiTextBox depStand;
    private UiTextBox depAtis;
    private UiTextBox depRemarks;

    // Oceanic CLX fields
    private UiTextBox ocnRecipient;
    private UiTextBox ocnCallsign;
    private UiTextBox ocnEntryPt;
    private UiTextBox ocnEta;
    private UiTextBox ocnMach;
    private UiTextBox ocnFltLvl;
    private UiTextBox ocnRemarks;

    // Request/logon fields
    private UiTextBox requestRecipient;
    private UiTextBox logonUnit;
    private UiTextBox levelFl;
    private UiTextBox directFix;
    private UiTextBox reportRecipient;

    private readonly MainForm mainForm;
    private readonly Pilot userVatsimData;
    private readonly Color controlBackColor;
    private readonly Color controlFrontColor;

    private readonly Dictionary<string, string> rsnConversion = new();

    private readonly Font controlFontBold;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool NeedsLogon
    {
        get;

        set
        {
            field = value;

            if (field)
            {
                logonButton.Text = @"LOGON";
                requestButton.Enabled = false;
                reportButton.Enabled = false;
            }
            else
            {
                logonButton.Text = @"LOGOFF";
                requestButton.Enabled = true;
                reportButton.Enabled = true;
            }
        }
    }

    public RequestForm(MainForm parent)
    {
        InitializeComponent();
        mainForm = parent;
        TopMost = parent.TopMost;
        NeedsLogon = mainForm.CurrentAtcUnit == null;

        userVatsimData = parent.userVatsimData;
        controlBackColor = parent.controlBackColor;
        controlFrontColor = parent.controlFrontColor;
        controlFontBold = new Font(MainForm.Fonts.Families[1], 12.5F, FontStyle.Bold);
        var textFont1 = parent.textFont;
        var textFontBold1 = parent.textFontBold;

        Theme = new Theme(controlBackColor, controlFrontColor, textFont1, textFontBold1);

        rsnConversion.Add("DUE TO WX", "DUE TO WEATHER");
        rsnConversion.Add("DUE TO A/C PERFORMANCE", "DUE TO PERFORMANCE");

        InitialisePopupMenu();
    }

    private ToolStripMenuItem CreateMenuItem(string name)
    {
        return new ToolStripMenuItem(name)
        {
            BackColor = Color.FromArgb(28, 28, 28),
            ForeColor = controlFrontColor,
            Font = controlFontBold
        };
    }

    private void InitialisePopupMenu()
    {
        popupMenu.BackColor = controlBackColor;
        popupMenu.ForeColor = controlFrontColor;
        popupMenu.Font = controlFontBold;
        popupMenu.ShowImageMargin = false;

        directRequestMenu = CreateMenuItem("DIRECT");
        directRequestMenu.Click += DirectRequestClick;
        levelRequestMenu = CreateMenuItem("LEVEL");
        levelRequestMenu.Click += LevelRequestClick;
        speedRequestMenu = CreateMenuItem("SPEED");
        speedRequestMenu.Click += SpeedRequestClick;
        whenCanWeRequestMenu = CreateMenuItem("WHEN CAN WE?");
        whenCanWeRequestMenu.Click += WhenCanWeRequestClick;

        clxMenu.BackColor = controlBackColor;
        clxMenu.ForeColor = controlFrontColor;
        clxMenu.Font = controlFontBold;
        clxMenu.ShowImageMargin = false;

        depClxMenu = CreateMenuItem("DEP CLX");
        depClxMenu.Click += DepClxClick;
        ocnClxMenu = CreateMenuItem("OCN CLX");
        ocnClxMenu.Click += OcnClxClick;
    }

    private void DepClxClick(object sender, EventArgs e)
    {
        depClxRadioButton.Checked = true;

        depRecipient  = CreateTextBox("", 4);
        depCallsign   = CreateTextBox(userVatsimData.callsign, 7);
        depAcType     = CreateTextBox(userVatsimData.flight_plan.aircraft_short, 4);
        depArpt       = CreateTextBox(userVatsimData.flight_plan.departure, 4);
        depArrArpt    = CreateTextBox(userVatsimData.flight_plan.arrival, 4);
        depStand      = CreateTextBox("", 4);
        depAtis       = CreateTextBox("", 1);
        depRemarks    = CreateMultiLineBox("", messageFormatPanel.Width);

        messageFormatPanel.Controls.Clear();
        messageFormatPanel.Controls.Add(CreateLabel("RECIPIENT:"), 1, 0);
        messageFormatPanel.Controls.Add(depRecipient, 2, 0);
        messageFormatPanel.Controls.Add(CreateLabel("CALLSIGN: "), 1, 1);
        messageFormatPanel.Controls.Add(depCallsign, 2, 1);
        messageFormatPanel.Controls.Add(CreateLabel("A/C TYPE: "), 3, 1);
        messageFormatPanel.Controls.Add(depAcType, 4, 1);
        messageFormatPanel.Controls.Add(CreateLabel("DEP ARPT: "), 1, 2);
        messageFormatPanel.Controls.Add(depArpt, 2, 2);
        messageFormatPanel.Controls.Add(CreateLabel("ARR ARPT: "), 3, 2);
        messageFormatPanel.Controls.Add(depArrArpt, 4, 2);
        messageFormatPanel.Controls.Add(CreateLabel("STAND: "), 1, 3);
        messageFormatPanel.Controls.Add(depStand, 2, 3);
        messageFormatPanel.Controls.Add(CreateLabel("ATIS: "), 3, 3);
        messageFormatPanel.Controls.Add(depAtis, 4, 3);
        messageFormatPanel.Controls.Add(CreateLabel("REMARKS: "), 1, 4);
        messageFormatPanel.Controls.Add(depRemarks, 1, 5);
        messageFormatPanel.SetColumnSpan(depRemarks, 4);
        messageFormatPanel.SetRowSpan(depRemarks, 2);
        messageFormatPanel.Controls.Add(CreateBracketLabel("[", AnchorStyles.Left), 0, 5);
        messageFormatPanel.Controls.Add(CreateBracketLabel("[", AnchorStyles.Left), 0, 6);
        messageFormatPanel.Controls.Add(CreateBracketLabel("]", AnchorStyles.Right), 5, 5);
        messageFormatPanel.Controls.Add(CreateBracketLabel("]", AnchorStyles.Right), 5, 6);
    }

    private void OcnClxClick(object sender, EventArgs e)
    {
        ocnClxRadioButton.Checked = true;

        ocnRecipient = CreateTextBox("", 4);
        ocnCallsign  = CreateTextBox(userVatsimData.callsign, 7);
        ocnEntryPt   = CreateTextBox("", 7);
        ocnEta       = CreateTextBox("", 4);
        ocnMach      = CreateTextBox("", 2);
        ocnFltLvl    = CreateTextBox("", 3);
        ocnRemarks   = CreateMultiLineBox("", messageFormatPanel.Width);

        messageFormatPanel.Controls.Clear();
        messageFormatPanel.Controls.Add(CreateLabel("RECIPIENT:"), 1, 0);
        messageFormatPanel.Controls.Add(ocnRecipient, 2, 0);
        messageFormatPanel.Controls.Add(CreateLabel("CALLSIGN: "), 1, 1);
        messageFormatPanel.Controls.Add(ocnCallsign, 2, 1);
        messageFormatPanel.Controls.Add(CreateLabel("ENTRY PT: "), 3, 1);
        messageFormatPanel.Controls.Add(ocnEntryPt, 4, 1);
        messageFormatPanel.Controls.Add(CreateLabel("ETA: "), 1, 2);
        messageFormatPanel.Controls.Add(ocnEta, 2, 2);
        messageFormatPanel.Controls.Add(CreateLabel("MACH: M0."), 3, 2);
        messageFormatPanel.Controls.Add(ocnMach, 4, 2);
        messageFormatPanel.Controls.Add(CreateLabel("FLT LVL: "), 1, 3);
        messageFormatPanel.Controls.Add(ocnFltLvl, 2, 3);
        messageFormatPanel.Controls.Add(CreateLabel("REMARKS: "), 1, 4);
        messageFormatPanel.Controls.Add(ocnRemarks, 1, 5);
        messageFormatPanel.SetColumnSpan(ocnRemarks, 4);
        messageFormatPanel.SetRowSpan(ocnRemarks, 2);
        messageFormatPanel.Controls.Add(CreateBracketLabel("[", AnchorStyles.Left), 0, 5);
        messageFormatPanel.Controls.Add(CreateBracketLabel("[", AnchorStyles.Left), 0, 6);
        messageFormatPanel.Controls.Add(CreateBracketLabel("]", AnchorStyles.Right), 5, 5);
        messageFormatPanel.Controls.Add(CreateBracketLabel("]", AnchorStyles.Right), 5, 6);
    }

    private void DirectRequestClick(object sender, EventArgs e)
    {
        directRadioButton.Checked = true;

        requestRecipient = CreateTextBox(mainForm.CurrentAtcUnit, 4, true);
        directFix = CreateAutoFillTextBox("", 7, mainForm.reportFixes);

        messageFormatPanel.Controls.Clear();
        messageFormatPanel.Controls.Add(CreateLabel("RECIPIENT:"), 1, 0);
        messageFormatPanel.Controls.Add(requestRecipient, 2, 0);
        messageFormatPanel.Controls.Add(CreateLabel("REQUEST DIRECT TO "), 1, 1);
        messageFormatPanel.Controls.Add(directFix, 2, 1);
        messageFormatPanel.Controls.Add(CreateCheckBox("DUE TO WX", "rsnParam"), 1, 2);
        messageFormatPanel.Controls.Add(CreateCheckBox("DUE TO A/C PERFORMANCE", "rsnParam"), 3, 2);
        messageFormatPanel.Controls.Add(CreateLabel("REMARKS: "), 1, 4);
        var remarks = CreateMultiLineBox("", messageFormatPanel.Width);
        messageFormatPanel.Controls.Add(remarks, 1, 5);
        messageFormatPanel.SetColumnSpan(remarks, 4);
        messageFormatPanel.SetRowSpan(remarks, 2);
        messageFormatPanel.Controls.Add(CreateBracketLabel("[", AnchorStyles.Left), 0, 5);
        messageFormatPanel.Controls.Add(CreateBracketLabel("[", AnchorStyles.Left), 0, 6);
        messageFormatPanel.Controls.Add(CreateBracketLabel("]", AnchorStyles.Right), 5, 5);
        messageFormatPanel.Controls.Add(CreateBracketLabel("]", AnchorStyles.Right), 5, 6);
    }

    private void LevelRequestClick(object sender, EventArgs e)
    {
        levelRadioButton.Checked = true;

        requestRecipient = CreateTextBox(mainForm.CurrentAtcUnit, 4, true);
        levelFl = CreateTextBox("", 3, false, true);

        messageFormatPanel.Controls.Clear();
        messageFormatPanel.Controls.Add(CreateLabel("RECIPIENT:"), 1, 0);
        messageFormatPanel.Controls.Add(requestRecipient, 2, 0);
        messageFormatPanel.Controls.Add(CreateLabel("REQUESTED FL: "), 1, 1);
        messageFormatPanel.Controls.Add(levelFl, 2, 1);
        messageFormatPanel.Controls.Add(CreateCheckBox("DUE TO WX", "rsnParam"), 1, 2);
        messageFormatPanel.Controls.Add(CreateCheckBox("DUE TO A/C PERFORMANCE", "rsnParam"), 3, 2);
        messageFormatPanel.Controls.Add(CreateLabel("REMARKS: "), 1, 4);
        var remarks = CreateMultiLineBox("", messageFormatPanel.Width);
        messageFormatPanel.Controls.Add(remarks, 1, 5);
        messageFormatPanel.SetColumnSpan(remarks, 4);
        messageFormatPanel.SetRowSpan(remarks, 2);
        messageFormatPanel.Controls.Add(CreateBracketLabel("[", AnchorStyles.Left), 0, 5);
        messageFormatPanel.Controls.Add(CreateBracketLabel("[", AnchorStyles.Left), 0, 6);
        messageFormatPanel.Controls.Add(CreateBracketLabel("]", AnchorStyles.Right), 5, 5);
        messageFormatPanel.Controls.Add(CreateBracketLabel("]", AnchorStyles.Right), 5, 6);
    }

    private void SpeedRequestClick(object sender, EventArgs e)
    {
        speedRadioButton.Checked = true;

        requestRecipient = CreateTextBox(mainForm.CurrentAtcUnit, 4, true);

        messageFormatPanel.Controls.Clear();
        messageFormatPanel.Controls.Add(CreateLabel("RECIPIENT:"), 1, 0);
        messageFormatPanel.Controls.Add(requestRecipient, 2, 0);
        messageFormatPanel.Controls.Add(CreateLabel("REQUEST"), 1, 1);
        messageFormatPanel.Controls.Add(CreateCheckBox("MACH: M0.", "unitParam"), 1, 2);
        messageFormatPanel.Controls.Add(CreateTextBox("", 2, false, true), 2, 2);
        messageFormatPanel.Controls.Add(CreateCheckBox("SPEED: ", "unitParam"), 3, 2);
        messageFormatPanel.Controls.Add(CreateTextBox("", 3, false, true), 4, 2);
        messageFormatPanel.Controls.Add(CreateCheckBox("DUE TO WX", "rsnParam"), 1, 3);
        messageFormatPanel.Controls.Add(CreateCheckBox("DUE TO A/C PERFORMANCE", "rsnParam"), 4, 3);
        messageFormatPanel.Controls.Add(CreateLabel("REMARKS: "), 1, 4);
        var remarks = CreateMultiLineBox("", messageFormatPanel.Width);
        messageFormatPanel.Controls.Add(remarks, 1, 5);
        messageFormatPanel.SetColumnSpan(remarks, 4);
        messageFormatPanel.SetRowSpan(remarks, 2);
        messageFormatPanel.Controls.Add(CreateBracketLabel("[", AnchorStyles.Left), 0, 5);
        messageFormatPanel.Controls.Add(CreateBracketLabel("[", AnchorStyles.Left), 0, 6);
        messageFormatPanel.Controls.Add(CreateBracketLabel("]", AnchorStyles.Right), 5, 5);
        messageFormatPanel.Controls.Add(CreateBracketLabel("]", AnchorStyles.Right), 5, 6);
    }

    private void WhenCanWeRequestClick(object sender, EventArgs e)
    {
        wcwRadioButton.Checked = true;

        requestRecipient = CreateTextBox(mainForm.CurrentAtcUnit, 4, true);

        messageFormatPanel.Controls.Clear();
        messageFormatPanel.Controls.Add(CreateLabel("RECIPIENT:"), 1, 0);
        messageFormatPanel.Controls.Add(requestRecipient, 2, 0);
        messageFormatPanel.Controls.Add(CreateLabel("WHEN CAN WE EXPECT:"), 1, 1);
        messageFormatPanel.Controls.Add(CreateCheckBox("HIGHER LEVEL?", "wcwParam"), 1, 2);
        messageFormatPanel.Controls.Add(CreateCheckBox("LOWER LEVEL?", "wcwParam"), 2, 2);
        messageFormatPanel.Controls.Add(CreateCheckBox("BACK ON ROUTE?", "wcwParam"), 3, 2);
        messageFormatPanel.Controls.Add(CreateCheckBox("CLIMB TO: FL ", "wcwParam"), 1, 3);
        messageFormatPanel.Controls.Add(CreateTextBox("", 3, false, true), 2, 3);
        messageFormatPanel.Controls.Add(CreateCheckBox("DESCENT TO: FL", "wcwParam"), 1, 4);
        messageFormatPanel.Controls.Add(CreateTextBox("", 3, false, true), 2, 4);
        messageFormatPanel.Controls.Add(CreateCheckBox("MACH: M0.", "wcwParam"), 3, 3);
        messageFormatPanel.Controls.Add(CreateTextBox("", 2, false, true), 4, 3);
        messageFormatPanel.Controls.Add(CreateCheckBox("SPEED: ", "wcwParam"), 3, 4);
        messageFormatPanel.Controls.Add(CreateTextBox("", 3, false, true), 4, 4);
    }

    private void PdcButton_Click(object sender, EventArgs e)
    {
        clxMenu.Items.Clear();
        clxMenu.Items.Add(depClxMenu);
        clxMenu.Items.Add(ocnClxMenu);
        clxMenu.Show(pdcButton, new Point(0, pdcButton.Height));
    }

    private void ReportButton_Click(object sender, EventArgs e)
    {
        fix1 = CreateAutoFillTextBox("", 7, mainForm.reportFixes);
        fix1.TextChanged += PreFill;
        fix2 = CreateTextBox("", 7);
        fix3 = CreateTextBox("", 7);
        reportAtTime    = CreateTextBox(DateTime.UtcNow.ToString("HHmm"), 4);
        reportFl        = CreateTextBox(
            MainForm.UseFsuipc
                ? (Math.Round(mainForm.fsuipc.altitude.Feet / 1000) * 10).ToString(CultureInfo.InvariantCulture)
                : userVatsimData.flight_plan.altitude[..3],
            3);
        reportNextAtTime = CreateTextBox("", 4);
        reportRecipient  = CreateTextBox(mainForm.CurrentAtcUnit, 4, true);

        fix1.Text = mainForm.nextFix ?? "";

        reportRadioButton.Checked = true;
        messageFormatPanel.Controls.Clear();
        messageFormatPanel.Controls.Add(CreateLabel("RECIPIENT:"), 1, 0);
        messageFormatPanel.Controls.Add(reportRecipient, 2, 0);
        messageFormatPanel.Controls.Add(CreateLabel("FIX: "), 1, 1);
        messageFormatPanel.Controls.Add(fix1, 2, 1);
        messageFormatPanel.Controls.Add(CreateLabel("AT: "), 1, 2);
        messageFormatPanel.Controls.Add(reportAtTime, 2, 2);
        messageFormatPanel.Controls.Add(CreateLabel("FL: "), 3, 2);
        messageFormatPanel.Controls.Add(reportFl, 4, 2);
        messageFormatPanel.Controls.Add(CreateLabel("NEXT: "), 1, 3);
        messageFormatPanel.Controls.Add(fix2, 2, 3);
        messageFormatPanel.Controls.Add(CreateLabel("AT: "), 3, 3);
        messageFormatPanel.Controls.Add(reportNextAtTime, 4, 3);
        messageFormatPanel.Controls.Add(CreateLabel("THEN: "), 1, 4);
        messageFormatPanel.Controls.Add(fix3, 2, 4);
    }

    private void PreFill(object sender, EventArgs e)
    {
        if (mainForm.reportFixes != null && mainForm.reportFixes.Contains(fix1.Text))
        {
            var refIndex = Array.IndexOf(mainForm.reportFixes, fix1.Text);
            fix2.Text = refIndex + 1 < mainForm.reportFixes.Length ? mainForm.reportFixes[refIndex + 1] : "";
            fix3.Text = refIndex + 2 < mainForm.reportFixes.Length ? mainForm.reportFixes[refIndex + 2] : "";
        }
        else
        {
            fix2.Clear();
            fix3.Clear();
        }
    }

    private void LogonButton_Click(object sender, EventArgs e)
    {
        logonUnit = CreateTextBox(NeedsLogon ? "" : mainForm.CurrentAtcUnit, 4);

        messageFormatPanel.Controls.Clear();
        messageFormatPanel.Controls.Add(CreateLabel("ATC UNIT:"), 1, 0);
        messageFormatPanel.Controls.Add(logonUnit, 2, 0);

        logonRadioButton.Checked = true;
    }

    private void RequestButton_Click(object sender, EventArgs e)
    {
        requestRadioButton.Checked = true;

        popupMenu.Items.Clear();
        popupMenu.Items.Add(directRequestMenu);
        popupMenu.Items.Add(levelRequestMenu);
        popupMenu.Items.Add(speedRequestMenu);
        popupMenu.Items.Add(whenCanWeRequestMenu);
        popupMenu.Show(requestButton, new Point(0, requestButton.Height));
    }

    private void ExitButton_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void ClearButton_Click(object sender, EventArgs e)
    {
        messageFormatPanel.Controls.Clear();
    }

    private void SendButton_Click(object sender, EventArgs e)
    {
        var radioBtn = radioContainer.Controls
            .OfType<RadioButton>().FirstOrDefault(x => x.Checked);

        if (radioBtn == null) return;

        var recipient = "";
        var formatMessage = "";
        var messageType = "";

        switch (radioBtn.Name)
        {
            case "ocnClxRadioButton":
                if (new[] { ocnRecipient, ocnCallsign, ocnEntryPt, ocnEta, ocnMach, ocnFltLvl }
                    .Any(tb => tb.Text.Length < 1)) return;

                recipient     = ocnRecipient.Text;
                formatMessage = $"OCEANIC CLEARANCE REQUEST CALLSIGN: {ocnCallsign.Text} ENTRY POINT: {ocnEntryPt.Text} AT: {ocnEta.Text} REQ: M.{ocnMach.Text} FL{ocnFltLvl.Text} RMKS: {ocnRemarks.Text}";
                messageType   = "CPDLC";
                break;

            case "depClxRadioButton":
                if (new[] { depRecipient, depCallsign, depAcType, depArpt, depArrArpt, depStand, depAtis }
                    .Any(tb => tb.Text.Length < 1)) return;

                recipient     = depRecipient.Text;
                formatMessage = $"REQUEST PREDEP CLEARANCE {depCallsign.Text} {depAcType.Text} TO {depArrArpt.Text} AT {depArpt.Text} STAND {depStand.Text} ATIS {depAtis.Text}";
                messageType   = "TELEX";
                break;

            case "logonRadioButton":
                if (logonUnit.Text.Length < 1) return;

                recipient = logonUnit.Text;
                if (NeedsLogon)
                {
                    formatMessage        = $@"/data2/{mainForm.messageOutCounter}//Y/REQUEST LOGON";
                    mainForm.pendingLogon = recipient;
                }
                else
                {
                    formatMessage            = $@"/data2/{mainForm.messageOutCounter}//N/LOGOFF";
                    mainForm.CurrentAtcUnit  = null;
                }
                messageType = "CPDLC";
                break;

            case "requestRadioButton":
                recipient     = requestRecipient.Text;
                formatMessage = $@"/data2/{mainForm.messageOutCounter}//Y/";
                var parsedMessage = ParseRequest();

                if (parsedMessage == "")
                {
                    mainForm.WriteMessage("ERROR PARSING CPDLC MESSAGE. NO MESSAGE SENT", "SYSTEM", "SYSTEM");
                    return;
                }

                formatMessage += parsedMessage;
                messageType    = "CPDLC";
                break;

            case "reportRadioButton":
                recipient     = reportRecipient.Text;
                formatMessage = $@"/data2/{mainForm.messageOutCounter}//N/POSITION REPORT PPOS {fix1.Text} AT {reportAtTime.Text}Z FL{reportFl.Text} TO {fix2.Text} AT {reportNextAtTime.Text}Z NEXT {fix3.Text}";
                messageType   = "CPDLC";
                mainForm.nextFix = fix2.Text;
                break;
        }

        if (messageType == "CPDLC") mainForm.messageOutCounter += 1;
        _ = Task.Run(() => mainForm.SendCpdlcMessage(recipient, messageType, formatMessage.Trim()));
        Close();
    }

    private string ParseRequest()
    {
        var radioBtn = requestContainer.Controls
            .OfType<RadioButton>().FirstOrDefault(x => x.Checked);

        var unitBox = messageFormatPanel.Controls
            .OfType<UiCheckBox>().FirstOrDefault(x => x.Checked && x.group == "unitParam");

        var wcwBox = messageFormatPanel.Controls
            .OfType<UiCheckBox>().FirstOrDefault(x => x.Checked && x.group == "wcwParam");

        var request = "";

        switch (radioBtn?.Name)
        {
            case "levelRadioButton":
                if (levelFl.Text == "") return string.Empty;

                request = "REQUEST FL" + levelFl.Text;
                AppendReason(ref request);
                break;

            case "directRadioButton":
                if (directFix.Text == "") return string.Empty;

                request = "REQUEST DIRECT TO " + directFix.Text;
                AppendReason(ref request);
                break;

            case "speedRadioButton":
                if (unitBox == null) return string.Empty;

                var speedBox = (UiTextBox)messageFormatPanel.Controls[messageFormatPanel.Controls.IndexOf(unitBox) + 1];
                if (speedBox.Text == "") return string.Empty;

                request = unitBox.Text == @"MACH: M0."
                    ? "REQUEST M" + speedBox.Text
                    : "REQUEST " + speedBox.Text + "K";

                AppendReason(ref request);
                break;

            case "wcwRadioButton":
                if (wcwBox == null) return string.Empty;

                request = "WHEN CAN WE EXPECT ";
                switch (wcwBox.Text)
                {
                    case "HIGHER LEVEL?":  request += "HIGHER LEVEL"; break;
                    case "LOWER LEVEL?":   request += "LOWER LEVEL"; break;
                    case "BACK ON ROUTE?": request += "BACK ON ROUTE"; break;
                    case "CLIMB TO FL: ":
                    case "DESCENT TO FL:":
                    case "MACH: M0.":
                    case "SPEED: ":
                        var wcwValueBox = (UiTextBox)messageFormatPanel.Controls[messageFormatPanel.Controls.IndexOf(wcwBox) + 1];
                        request += wcwBox.Text switch
                        {
                            @"MACH: M0." => "M" + wcwValueBox.Text,
                            "SPEED: " => wcwValueBox.Text + "K",
                            _ => wcwBox.Text.TrimEnd(':', ' ').Replace("TO FL", "TO FL") + wcwValueBox.Text
                        };
                        break;
                }
                break;
        }

        return request;
    }

    private void AppendReason(ref string request)
    {
        var dueToBox = messageFormatPanel.Controls
            .OfType<UiCheckBox>().FirstOrDefault(x => x.Checked && x.group == "rsnParam");
        if (dueToBox != null)
            request += " " + rsnConversion[dueToBox.Text];
    }

    private void WindowDrag(object sender, MouseEventArgs e) => DragWindow(sender, e);

    private void RequestForm_Load(object sender, EventArgs e)
    {
        if (Properties.Settings.Default.CPDLCWindowLocation == new Point(0, 0)) return;
        Location = Properties.Settings.Default.CPDLCWindowLocation;
        Size = Properties.Settings.Default.CPDLCWindowSize;
    }

    private void RequestForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        Properties.Settings.Default.CPDLCWindowLocation = Location;
        Properties.Settings.Default.CPDLCWindowSize = Size;
        Properties.Settings.Default.Save();
    }
}