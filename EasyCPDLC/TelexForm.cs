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
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyCPDLC;

public partial class TelexForm
{

    private readonly bool isReply;

    private readonly MainForm parent;
    private readonly Color controlBackColor;
    private readonly Color controlFrontColor;
    private readonly Font textFont;
    private readonly Font textFontBold;
    private readonly string recipient;
    public TelexForm(MainForm parentForm, string recipientCallsign = null)
    {
        InitializeComponent();
        parent = parentForm;
        controlBackColor = parent.controlBackColor;
        controlFrontColor = parent.controlFrontColor;
        textFont = parent.textFont;
        textFontBold = parent.textFontBold;
        recipient = recipientCallsign;
        isReply = parentForm is not null;
        Theme = new Theme(controlBackColor, controlFrontColor, textFont, textFontBold);

        freeTextButton.TabIndex = 0;
        metarButton.TabIndex = 1;
        atisButton.TabIndex = 2;
        sendButton.TabIndex = 3;
    
        // Set accessible properties
        freeTextButton.AccessibleName = @"Free Text Message";
        metarButton.AccessibleName = @"Request METAR";
        atisButton.AccessibleName = @"Request ATIS";
        sendButton.AccessibleName = @"Send Message";
    
        // Handle Enter/Escape
        this.KeyPreview = true;
        this.KeyDown += (s, e) => {
            if (e.KeyCode == Keys.Enter && sendButton.Enabled) sendButton.PerformClick();
            if (e.KeyCode == Keys.Escape) this.Close();
        };
    
        TopMost = parent.TopMost;
    }


    private void ExitButton_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    private void WindowDrag(object sender, MouseEventArgs e) => DragWindow(sender, e);

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

    private static void ScrollToBottom(FlowLayoutPanel p)
    {
        using Control c = new();
        c.Parent = p;
        c.Dock = DockStyle.Bottom;
        p.ScrollControlIntoView(c);
        c.Parent = null;
    }

    private void SendButton_Click(object sender, EventArgs e)
    {
        var radioBtn = radioContainer.Controls
            .OfType<RadioButton>().FirstOrDefault(x => x.Checked);

        if (radioBtn == null && messageFormatPanel.Controls[1].Text.Length >= 4) return;
        var recipientCallsign = messageFormatPanel.Controls[1].Text;

        switch (radioBtn?.Name)
        {
            case "freeTextRadioButton":
                var formatMessage = messageFormatPanel.Controls[3].Text;
                _ = Task.Run(() => parent.SendCpdlcMessage(recipientCallsign, "TELEX", formatMessage.Trim()));
                break;

            case "metarRadioButton":
                parent.WriteMessage("METAR REQUEST", "METAR", recipientCallsign, true);
                parent.ArtificialDelay("METAR " + recipientCallsign, "INFOREQ", "REQUEST");

                break;

            case "atisRadioButton":

                parent.WriteMessage("ATIS REQUEST", "ATIS", recipientCallsign, true);
                parent.ArtificialDelay("VATATIS " + recipientCallsign, "INFOREQ", "REQUEST");

                break;

        }
        if(isReply)
        {
            parent.ClearPreview();
        }
        Close();
    }

    private void FreeTextButton_Click(object sender, EventArgs e)
    {
        messageFormatPanel.Controls.Clear();
        messageFormatPanel.Controls.Add(CreateLabel("RECIPIENT:"));
        messageFormatPanel.Controls.Add(CreateTextBox(recipient ?? "", 7));
        messageFormatPanel.SetFlowBreak(messageFormatPanel.Controls[^1], true);
        messageFormatPanel.Controls.Add(CreateLabel("MSG:"));
        messageFormatPanel.SetFlowBreak(messageFormatPanel.Controls[^1], true);
        messageFormatPanel.Controls.Add(CreateMultiLineBox("", messageFormatPanel.Width, placeholder: "Your message here..."));

        freeTextRadioButton.Checked = true;
    }

    private void MetarButton_Click(object sender, EventArgs e)
    {
        messageFormatPanel.Controls.Clear();
        messageFormatPanel.Controls.Add(CreateLabel("STATION:"));
        try
        {
            messageFormatPanel.Controls.Add(parent.fsuipc.groundspeed < 100
                ? CreateTextBox(parent.userVatsimData.flight_plan.departure, 4)
                : CreateTextBox(parent.userVatsimData.flight_plan.arrival, 4));
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
        messageFormatPanel.Controls.Add(CreateLabel("STATION:"));
        try
        {
            messageFormatPanel.Controls.Add(parent.fsuipc.groundspeed < 100
                ? CreateTextBox(parent.userVatsimData.flight_plan.departure, 4)
                : CreateTextBox(parent.userVatsimData.flight_plan.arrival, 4));
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