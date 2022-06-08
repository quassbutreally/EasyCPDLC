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

namespace EasyCPDLC
{
    public partial class RequestForm : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        private const int cGrip = 16;
        private const int cCaption = 32;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        private readonly ContextMenuStrip popupMenu = new();
        private ToolStripMenuItem directRequestMenu;
        private ToolStripMenuItem levelRequestMenu;
        private ToolStripMenuItem speedRequestMenu;
        private ToolStripMenuItem whenCanWeRequestMenu;

        private readonly ContextMenuStrip clxMenu = new();
        private ToolStripMenuItem depClxMenu;
        private ToolStripMenuItem ocnClxMenu;

        UITextBox fix1;
        UITextBox fix2;
        UITextBox fix3;

        private readonly MainForm MainForm;
        private readonly Pilot userVATSIMData;
        private readonly Color controlBackColor;
        private readonly Color controlFrontColor;

        private readonly Dictionary<string, string> rsnConversion = new();

        private readonly Font controlFontBold;
        private readonly Font textFont;
        private readonly Font textFontBold;

        private bool _needsLogon;
        public bool NeedsLogon
        {
            get
            {
                return this._needsLogon;
            }

            set
            {
                this._needsLogon = value;

                if (this._needsLogon)
                {
                    logonButton.Text = "LOGON";
                    requestButton.Enabled = false;
                    reportButton.Enabled = false;
                }
                else
                {
                    logonButton.Text = "LOGOFF";
                    requestButton.Enabled = true;
                    reportButton.Enabled = true;
                }
            }
        }

        public RequestForm(MainForm parent)
        {
            InitializeComponent();
            this.MainForm = parent;
            this.TopMost = parent.TopMost;
            if (this.MainForm.CurrentATCUnit != null)
            {
                NeedsLogon = false;
            }
            else
            {
                NeedsLogon = true;
            }

            userVATSIMData = parent.userVATSIMData;
            controlBackColor = parent.controlBackColor;
            controlFrontColor = parent.controlFrontColor;
            controlFontBold = new Font("Oxygen", 12.5F, FontStyle.Bold);
            textFont = parent.textFont;
            textFontBold = parent.textFontBold;

            rsnConversion.Add("DUE TO WX", "DUE TO WEATHER");
            rsnConversion.Add("DUE TO A/C PERFORMANCE", "DUE TO PERFORMANCE");

            InitialisePopupMenu();

        }

        private ToolStripMenuItem CreateMenuItem(string name)
        {
            ToolStripMenuItem _temp = new(name)
            {
                BackColor = Color.FromArgb(28, 28, 28),
                ForeColor = controlFrontColor,
                Font = controlFontBold
            };
            //_temp.AutoSize = false;
            //_temp.Size = new Size(104, 37);

            return _temp;
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

        private void AddRemarksField(TableLayoutPanel _control)
        {
            _control.Controls.Add(CreateTemplate("REMARKS: "), 1, 4);
            UITextBox remarksBox = CreateMultiLineBox("");
            _control.Controls.Add(remarksBox, 1, 5);
            _control.SetColumnSpan(remarksBox, 4);
            _control.SetRowSpan(remarksBox, 2);
            _control.Controls.Add(CreateBoxTemplate("[", AnchorStyles.Left), 0, 5);
            _control.Controls.Add(CreateBoxTemplate("[", AnchorStyles.Left), 0, 6);
            _control.Controls.Add(CreateBoxTemplate("]", AnchorStyles.Right), 5, 5);
            _control.Controls.Add(CreateBoxTemplate("]", AnchorStyles.Right), 5, 6);
        }

        private void DepClxClick(object sender, EventArgs e)
        {
            depClxRadioButton.Checked = true;

            messageFormatPanel.Controls.Clear();
            messageFormatPanel.Controls.Add(CreateTemplate("RECIPIENT:"), 1, 0);
            messageFormatPanel.Controls.Add(CreateTextBox("", 4), 2, 0);
            messageFormatPanel.Controls.Add(CreateTemplate("CALLSIGN: "), 1, 1);
            messageFormatPanel.Controls.Add(CreateTextBox(userVATSIMData.callsign, 7), 2, 1);
            messageFormatPanel.Controls.Add(CreateTemplate("A/C TYPE: "), 3, 1);
            messageFormatPanel.Controls.Add(CreateTextBox(userVATSIMData.flight_plan.aircraft_short, 4), 4, 1);
            messageFormatPanel.Controls.Add(CreateTemplate("DEP ARPT: "), 1, 2);
            messageFormatPanel.Controls.Add(CreateTextBox(userVATSIMData.flight_plan.departure, 4), 2, 2);
            messageFormatPanel.Controls.Add(CreateTemplate("ARR ARPT: "), 3, 2);
            messageFormatPanel.Controls.Add(CreateTextBox(userVATSIMData.flight_plan.arrival, 4), 4, 2);
            messageFormatPanel.Controls.Add(CreateTemplate("STAND: "), 1, 3);
            messageFormatPanel.Controls.Add(CreateTextBox("", 4), 2, 3);
            messageFormatPanel.Controls.Add(CreateTemplate("ATIS: "), 3, 3);
            messageFormatPanel.Controls.Add(CreateTextBox("", 1), 4, 3);

            AddRemarksField(messageFormatPanel);
        }
        private void OcnClxClick(object sender, EventArgs e)
        {
            ocnClxRadioButton.Checked = true;

            messageFormatPanel.Controls.Clear();
            messageFormatPanel.Controls.Add(CreateTemplate("RECIPIENT:"), 1, 0);
            messageFormatPanel.Controls.Add(CreateTextBox("", 4), 2, 0);
            messageFormatPanel.Controls.Add(CreateTemplate("CALLSIGN: "), 1, 1);
            messageFormatPanel.Controls.Add(CreateTextBox(userVATSIMData.callsign, 7), 2, 1);
            messageFormatPanel.Controls.Add(CreateTemplate("ENTRY PT: "), 3, 1);
            messageFormatPanel.Controls.Add(CreateTextBox("", 7), 4, 1);
            messageFormatPanel.Controls.Add(CreateTemplate("ETA: "), 1, 2);
            messageFormatPanel.Controls.Add(CreateTextBox("", 4), 2, 2);
            messageFormatPanel.Controls.Add(CreateTemplate("MACH: M0."), 3, 2);
            messageFormatPanel.Controls.Add(CreateTextBox("", 2), 4, 2);
            messageFormatPanel.Controls.Add(CreateTemplate("FLT LVL: "), 1, 3);
            messageFormatPanel.Controls.Add(CreateTextBox("", 3), 2, 3);

            AddRemarksField(messageFormatPanel);
        }

        private void DirectRequestClick(object sender, EventArgs e)
        {
            directRadioButton.Checked = true;

            messageFormatPanel.Controls.Clear();
            messageFormatPanel.Controls.Add(CreateTemplate("RECIPIENT:"), 1, 0);
            messageFormatPanel.Controls.Add(CreateTextBox(MainForm.CurrentATCUnit, 4, true), 2, 0);
            messageFormatPanel.Controls.Add(CreateTemplate("REQUEST DIRECT TO "), 1, 1);
            messageFormatPanel.Controls.Add(CreateAutoFillTextBox("", 7, MainForm.reportFixes), 2, 1);
            messageFormatPanel.Controls.Add(CreateCheckBox("DUE TO WX", "rsnParam"), 1 , 2);
            messageFormatPanel.Controls.Add(CreateCheckBox("DUE TO A/C PERFORMANCE", "rsnParam"), 3, 2);

            AddRemarksField(messageFormatPanel);


        }

        private void LevelRequestClick(object sender, EventArgs e)
        {
            levelRadioButton.Checked = true;

            messageFormatPanel.Controls.Clear();
            messageFormatPanel.Controls.Add(CreateTemplate("RECIPIENT:"), 1, 0);
            messageFormatPanel.Controls.Add(CreateTextBox(MainForm.CurrentATCUnit, 4, true), 2, 0);
            messageFormatPanel.Controls.Add(CreateTemplate("REQUESTED FL: "), 1, 1);
            messageFormatPanel.Controls.Add(CreateTextBox("", 3, false, true), 2, 1);
            messageFormatPanel.Controls.Add(CreateCheckBox("DUE TO WX", "rsnParam"), 1, 2);
            messageFormatPanel.Controls.Add(CreateCheckBox("DUE TO A/C PERFORMANCE", "rsnParam"), 3, 2);

            AddRemarksField(messageFormatPanel);
        }

        private void SpeedRequestClick(object sender, EventArgs e)
        {
            speedRadioButton.Checked = true;

            messageFormatPanel.Controls.Clear();
            messageFormatPanel.Controls.Add(CreateTemplate("RECIPIENT:"), 1, 0);
            messageFormatPanel.Controls.Add(CreateTextBox(MainForm.CurrentATCUnit, 4, true), 2, 0);
            messageFormatPanel.Controls.Add(CreateTemplate("REQUEST"), 1, 1);
            messageFormatPanel.Controls.Add(CreateCheckBox("MACH: M0.", "unitParam"), 1, 2);
            messageFormatPanel.Controls.Add(CreateTextBox("", 2, false, true), 2, 2);
            messageFormatPanel.Controls.Add(CreateCheckBox("SPEED: ", "unitParam"), 3, 2);
            messageFormatPanel.Controls.Add(CreateTextBox("", 3, false, true), 4, 2);
            messageFormatPanel.Controls.Add(CreateCheckBox("DUE TO WX", "rsnParam"), 1, 3);
            messageFormatPanel.Controls.Add(CreateCheckBox("DUE TO A/C PERFORMANCE", "rsnParam"), 4, 3);

            AddRemarksField(messageFormatPanel);
        }

        private void WhenCanWeRequestClick(object sender, EventArgs e)
        {
            wcwRadioButton.Checked = true;

            messageFormatPanel.Controls.Clear();
            messageFormatPanel.Controls.Add(CreateTemplate("RECIPIENT:"), 1 , 0);
            messageFormatPanel.Controls.Add(CreateTextBox(MainForm.CurrentATCUnit, 4, true), 2 , 0);
            messageFormatPanel.Controls.Add(CreateTemplate("WHEN CAN WE EXPECT:"), 1, 1);
            messageFormatPanel.Controls.Add(CreateCheckBox("HIGHER LEVEL?", "wcwParam"), 1, 2);
            messageFormatPanel.Controls.Add(CreateCheckBox("LOWER LEVEL?", "wcwParam"), 2, 2);
            messageFormatPanel.Controls.Add(CreateCheckBox("BACK ON ROUTE?", "wcwParam"), 3, 2);
            messageFormatPanel.Controls.Add(CreateCheckBox("CLIMB TO FL: ", "wcwParam"), 1, 3);
            messageFormatPanel.Controls.Add(CreateTextBox("", 3, false, true), 2, 3);
            messageFormatPanel.Controls.Add(CreateCheckBox("DESCENT TO FL:", "wcwParam"), 1, 4);
            messageFormatPanel.Controls.Add(CreateTextBox("", 3, false, true), 2, 4);
            messageFormatPanel.Controls.Add(CreateCheckBox("MACH: M0.", "wcwParam"), 3, 3);
            messageFormatPanel.Controls.Add(CreateTextBox("", 2, false, true), 4, 3);
            messageFormatPanel.Controls.Add(CreateCheckBox("SPEED: ", "wcwParam"), 3, 4);
            messageFormatPanel.Controls.Add(CreateTextBox("", 3, false, true), 4, 4);
        }

        private void PdcButton_Click(object sender, EventArgs e)
        {
            clxMenu.Items.Add(depClxMenu);
            clxMenu.Items.Add(ocnClxMenu);

            clxMenu.Show(pdcButton, new Point(0, pdcButton.Height));
        }

        private void ReportButton_Click(object sender, EventArgs e)
        {
            fix1 = CreateAutoFillTextBox("", 7, MainForm.reportFixes);
            fix1.TextChanged += PreFill;
            fix2 = CreateTextBox("", 7);
            fix3 = CreateTextBox("", 7);

            fix1.Text = MainForm.nextFix ?? "";

            reportRadioButton.Checked = true;
            messageFormatPanel.Controls.Clear();
            messageFormatPanel.Controls.Add(CreateTemplate("RECIPIENT:"), 1, 0);
            messageFormatPanel.Controls.Add(CreateTextBox(MainForm.CurrentATCUnit, 4, true), 2, 0);
            messageFormatPanel.Controls.Add(CreateTemplate("FIX: "), 1, 1);
            messageFormatPanel.Controls.Add(fix1, 2, 1);
            messageFormatPanel.Controls.Add(CreateTemplate("AT: "), 1, 2);
            messageFormatPanel.Controls.Add(CreateTextBox(DateTime.UtcNow.ToString("HHmm"), 4), 2, 2);
            messageFormatPanel.Controls.Add(CreateTemplate("FL: "), 3, 2);
            messageFormatPanel.Controls.Add(CreateTextBox(MainForm.UseFSUIPC ? (Math.Round(MainForm.fsuipc.altitude.Feet / 1000) * 10).ToString() : userVATSIMData.flight_plan.altitude[..3], 3), 4, 2);
            messageFormatPanel.Controls.Add(CreateTemplate("NEXT: "), 1, 3);
            messageFormatPanel.Controls.Add(fix2, 2, 3);
            messageFormatPanel.Controls.Add(CreateTemplate("AT: "), 3 , 3);
            messageFormatPanel.Controls.Add(CreateTextBox("", 4), 4, 3);
            messageFormatPanel.Controls.Add(CreateTemplate("THEN: "), 1, 4);
            messageFormatPanel.Controls.Add(fix3, 2, 4);
        }
        private void PreFill(object sender, EventArgs e)
        {
            if (MainForm.reportFixes != null && MainForm.reportFixes.Contains(fix1.Text))
            {
                int refIndex = Array.IndexOf(MainForm.reportFixes, fix1.Text);
                try
                {
                    fix3.Text = MainForm.reportFixes[refIndex + 2];
                }
                catch
                {
                    fix3.Clear();
                }
                try
                {
                    fix2.Text = MainForm.reportFixes[refIndex + 1];
                }
                catch
                {
                    fix2.Clear();
                }
            }
            else
            {
                fix2.Clear();
                fix3.Clear();
            }

        }

        private void LogonButton_Click(object sender, EventArgs e)
        {
            messageFormatPanel.Controls.Clear();
            messageFormatPanel.Controls.Add(CreateTemplate("ATC UNIT:"), 1, 0);
            messageFormatPanel.Controls.Add(CreateTextBox(NeedsLogon ? "" : MainForm.CurrentATCUnit, 4),2 , 0);

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

        private UITextBox CreateAutoFillTextBox(string _text, int _maxLength, string[] _source)
        {
            UITextBox _temp = CreateTextBox(_text, _maxLength);
            _temp.AutoCompleteMode = AutoCompleteMode.Append;
            _temp.AutoCompleteSource = AutoCompleteSource.CustomSource;
            var autoComplete = new AutoCompleteStringCollection();
            if (_source != null)
            {
                autoComplete.AddRange(_source);
            }
            _temp.AutoCompleteCustomSource = autoComplete;
            _temp.TextAlign = HorizontalAlignment.Center;

            return _temp;
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
                Height = 30,

                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(0, 10, 0, 0),
                Margin = new Padding(0, 0, 0, 0),
                TabStop = true,
                TabIndex = 0
            };

            return _temp;
        }

        private AccessibleLabel CreateBoxTemplate(string _text, AnchorStyles _leftOrRight)
        {
            AccessibleLabel _temp = new(controlFrontColor)
            {
                BackColor = controlBackColor,
                ForeColor = controlFrontColor,
                Font = textFont,
                AutoSize = true,
                Text = _text,
                Top = 10,
                Height = 30,

                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(0, 10, 0, 0),
                Margin = new Padding(0, 0, 0, 0),
                TabStop = false,
                Anchor = _leftOrRight
            };

            return _temp;
        }

        private UITextBox CreateTextBox(string _text, int _maxLength, bool _readOnly = false, bool _numsOnly = false)
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
                PlaceholderText = new string('▯', _maxLength),
                Height = 30,
                ReadOnly = _readOnly,
                TextAlign = HorizontalAlignment.Left,
                TabIndex = 0,
                Anchor = AnchorStyles.Left
            };

            if (_numsOnly)
            {
                _temp.KeyPress += NumsOnly;
            }

            using (Graphics G = _temp.CreateGraphics())
            {
                _temp.Width = (int)(_temp.MaxLength *
                              G.MeasureString("▯", _temp.Font).Width * 1.5);
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

        private UICheckBox CreateCheckBox(string _text, string _group)
        {         
            UICheckBox _temp = new(_group)
            {
                BackColor = controlBackColor,
                ForeColor = controlFrontColor,
                Font = textFont,
                Text = _text,
                Padding = new Padding(3, 10, 10, -30),
                AutoSize = true,
                TabIndex = 0
            };
            _temp.Click += DeselectCheckBox;
            return _temp;
        }

        private void DeselectCheckBox(object sender, EventArgs e)
        {
            UICheckBox _sender = (UICheckBox)sender;

            foreach (UICheckBox box in messageFormatPanel.Controls.OfType<UICheckBox>())
            {
                if (box.Text != _sender.Text && box.group == _sender.group)
                {
                    box.Checked = false;
                }
            }

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
                MaxLength = 99,
                Height = 30,
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

        private void ClearButton_Click(object sender, EventArgs e)
        {
            messageFormatPanel.Controls.Clear();
        }

        private void SendButton_Click(object sender, EventArgs e)
        {

            RadioButton radioBtn = radioContainer.Controls.OfType<RadioButton>()
                                       .Where(x => x.Checked).FirstOrDefault();

            if (radioBtn != null)
            {
                string _recipient = messageFormatPanel.Controls[1].Text;
                string _formatMessage = "";
                string _messageType = "";

                switch (radioBtn.Name)
                {

                    case "ocnClxRadioButton":

                        for (int i = 0; i <= 13; i++)
                        {
                            if (messageFormatPanel.Controls[i].Text.Length < 1)
                            {
                                return;
                            }
                        }

                        _formatMessage = string.Format("OCEANIC CLEARANCE REQUEST CALLSIGN: {0} ENTRY POINT: {1} AT: {2} REQ: M.{3} FL{4} RMKS: {5}",
                            messageFormatPanel.Controls[3].Text,
                            messageFormatPanel.Controls[5].Text,
                            messageFormatPanel.Controls[7].Text,
                            messageFormatPanel.Controls[9].Text,
                            messageFormatPanel.Controls[11].Text,
                            messageFormatPanel.Controls[13].Text
                            );
                        _messageType = "TELEX";
                        break;

                    case "depClxRadioButton":

                        for (int i = 0; i <= 13; i++)
                        {
                            if (messageFormatPanel.Controls[i].Text.Length < 1)
                            {
                                return;
                            }
                        }

                        _formatMessage = string.Format("REQUEST PREDEP CLEARANCE {0} {1} TO {2} AT {3} STAND {4} ATIS {5}",
                             messageFormatPanel.Controls[3].Text,
                             messageFormatPanel.Controls[5].Text,
                             messageFormatPanel.Controls[9].Text,
                             messageFormatPanel.Controls[7].Text,
                             messageFormatPanel.Controls[11].Text,
                             messageFormatPanel.Controls[13].Text
                             );
                        _messageType = "TELEX";
                        break;

                    case "logonRadioButton":

                        foreach (Control _control in messageFormatPanel.Controls)
                        {
                            if (_control.Text.Length < 1)
                            {
                                return;
                            }
                        }

                        if (NeedsLogon)
                        {
                            _formatMessage = String.Format("/data2/{0}//Y/REQUEST LOGON", MainForm.messageOutCounter);
                            MainForm.pendingLogon = _recipient;
                        }
                        else
                        {
                            _formatMessage = String.Format("/data2/{0}//N/LOGOFF", MainForm.messageOutCounter);
                            MainForm.CurrentATCUnit = null;

                        }

                        _messageType = "CPDLC";

                        break;

                    case "requestRadioButton":

                        _formatMessage = String.Format("/data2/{0}//Y/", MainForm.messageOutCounter);
                        string parsedMessage = ParseRequest();

                        if (parsedMessage == "")
                        {
                            MainForm.WriteMessage("ERROR PARSING CPDLC MESSAGE. NO MESSAGE SENT", "SYSTEM", "SYSTEM");
                            break;
                        }

                        _formatMessage += parsedMessage;

                        _messageType = "CPDLC";

                        break;

                    case "reportRadioButton":

                        _formatMessage = String.Format("/data2/{0}//N/", MainForm.messageOutCounter);
                        string _messageContent = String.Format("POSITION REPORT PPOS {0} AT {1}Z FL{2} TO {3} AT {4}Z NEXT {5}",
                            fix1.Text,
                            messageFormatPanel.Controls[6].Text,
                            messageFormatPanel.Controls[9].Text,
                            fix2.Text,
                            messageFormatPanel.Controls[13].Text,
                            fix3.Text);
                        _formatMessage += _messageContent;
                        _messageType = "CPDLC";
                        MainForm.nextFix = fix2.Text;
                        break;



                    default:
                        break;



                }

                if(_messageType == "CPDLC") { MainForm.messageOutCounter += 1; }
                _ = Task.Run(() => MainForm.SendCPDLCMessage(_recipient, _messageType, _formatMessage.Trim()));

                this.Close();
            }
            else
            {

            }
        }

        private string ParseRequest()
        {
            RadioButton radioBtn = requestContainer.Controls.OfType<RadioButton>()
                                      .Where(x => x.Checked).FirstOrDefault();

            UICheckBox dueToBox = messageFormatPanel.Controls.OfType<UICheckBox>()
                                   .Where(x => x.Checked && x.group == "rsnParam").FirstOrDefault();

            UICheckBox unitBox = messageFormatPanel.Controls.OfType<UICheckBox>()
                                   .Where(x => x.Checked && x.group == "unitParam").FirstOrDefault();

            UICheckBox wcwBox = messageFormatPanel.Controls.OfType<UICheckBox>()
                                   .Where(x => x.Checked && x.group == "wcwParam").FirstOrDefault();

            string _request = "";

            switch (radioBtn.Name)
            {
                case "levelRadioButton":

                    if (messageFormatPanel.Controls[3].Text == "")
                    {
                        return string.Empty;
                    }
                    _request = "REQUEST FL";
                    _request += messageFormatPanel.Controls[3].Text;

                    dueToBox = messageFormatPanel.Controls.OfType<UICheckBox>()
                                   .Where(x => x.Checked && x.group == "rsnParam").FirstOrDefault();

                    if (dueToBox != default(UICheckBox))
                    {
                        _request += " " + rsnConversion[dueToBox.Text];
                    }
                    break;

                case "directRadioButton":

                    if (messageFormatPanel.Controls[3].Text == "")
                    {
                        return string.Empty;
                    }

                    _request = "REQUEST DIRECT TO ";
                    _request += messageFormatPanel.Controls[3].Text;

                    dueToBox = messageFormatPanel.Controls.OfType<UICheckBox>()
                                   .Where(x => x.Checked && x.group == "rsnParam").FirstOrDefault();

                    if (dueToBox != default(UICheckBox))
                    {
                        _request += " " + rsnConversion[dueToBox.Text];
                    }
                    break;

                case "speedRadioButton":

                    if (messageFormatPanel.Controls[messageFormatPanel.Controls.IndexOf(unitBox) + 1].Text == "")
                    {
                        return string.Empty;
                    }

                    _request += "REQUEST ";
                    if (unitBox != default(UICheckBox))
                    {
                        if (unitBox.Text == "MACH: M0.")
                        {
                            _request += "M" + messageFormatPanel.Controls[messageFormatPanel.Controls.IndexOf(unitBox) + 1].Text;
                        }
                        else
                        {
                            _request += messageFormatPanel.Controls[messageFormatPanel.Controls.IndexOf(unitBox) + 1].Text + "K";
                        }
                    }
                    else
                    {
                        return string.Empty;
                    }

                    dueToBox = messageFormatPanel.Controls.OfType<UICheckBox>()
                                   .Where(x => x.Checked && x.group == "rsnParam").FirstOrDefault();

                    if (dueToBox != default(UICheckBox))
                    {
                        _request += " " + rsnConversion[dueToBox.Text];
                    }
                    break;

                case "wcwRadioButton":

                    if (wcwBox is null)
                    {
                        return string.Empty;
                    }

                    _request = "WHEN CAN WE EXPECT ";

                    switch (wcwBox.Text)
                    {
                        case "HIGHER LEVEL?":
                            _request += "HIGHER LEVEL";
                            break;

                        case "LOWER LEVEL?":
                            _request += "LOWER LEVEL";
                            break;

                        case "BACK ON ROUTE?":
                            _request += "BACK ON ROUTE";
                            break;

                        case "CLIMB TO: FL":
                            _request += "CLIMB TO FL" + messageFormatPanel.Controls[messageFormatPanel.Controls.IndexOf(wcwBox) + 1];
                            break;

                        case "DESCENT TO: FL":
                            _request += "DESCENT TO FL" + messageFormatPanel.Controls[messageFormatPanel.Controls.IndexOf(wcwBox) + 1];
                            break;

                        case "MACH: M0.":
                            _request += "M" + messageFormatPanel.Controls[messageFormatPanel.Controls.IndexOf(wcwBox) + 1];
                            break;

                        case "SPEED: ":
                            _request += messageFormatPanel.Controls[messageFormatPanel.Controls.IndexOf(wcwBox) + 1] + "K";
                            break;

                        default:
                            break;
                    }

                    break;

                default:
                    break;
            }

            return _request;
        }

        private void WindowDrag(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                _ = SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
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

            //ScrollToBottom(messageFormatPanel);
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

        private void RequestForm_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.CPDLCWindowLocation != new Point(0, 0))
            {
                Location = Properties.Settings.Default.CPDLCWindowLocation;
                Size = Properties.Settings.Default.CPDLCWindowSize;
            }
        }

        private void RequestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.CPDLCWindowLocation = Location;
            Properties.Settings.Default.CPDLCWindowSize = Size;
            Properties.Settings.Default.Save();
        }
    }
}
