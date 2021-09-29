using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace EasyCPDLC
{
    public partial class RequestForm : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private ContextMenuStrip popupMenu = new ContextMenuStrip();
        private ToolStripMenuItem directRequestMenu;// = new ToolStripMenuItem();
        private ToolStripMenuItem levelRequestMenu;// = new ToolStripMenuItem();
        private ToolStripMenuItem speedRequestMenu;// = new ToolStripMenuItem();

        private MainForm parent;
        private PilotData userVATSIMData;
        private Color controlBackColor;
        private Color controlFrontColor;
        private Font controlFont;
        private Font controlFontBold;

        private bool _needsLogon;
        public bool needsLogon
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
                    //requestButton.Enabled = false;
                    //reportButton.Enabled = false;
                }
                else
                {
                    logonButton.Text = "LOGOFF";
                    //requestButton.Enabled = true;
                    //reportButton.Enabled = true;
                }
            }
        }

        public RequestForm(MainForm parent)
        {
            InitializeComponent();
            this.parent = parent;

            if (this.parent.currentATCUnit != null)
            {
                needsLogon = false;
            }
            else
            {
                needsLogon = true;
            }

            userVATSIMData = parent.userVATSIMData;
            controlBackColor = parent.controlBackColor;
            controlFrontColor = parent.controlFrontColor;
            controlFont = parent.controlFont;
            controlFontBold = new Font("Oxygen", 12.5F, FontStyle.Bold);

            InitialisePopupMenu();

        }

        private ToolStripMenuItem CreateMenuItem(string name)
        {
            ToolStripMenuItem _temp = new ToolStripMenuItem(name);
            _temp.BackColor = Color.FromArgb(28, 28, 28);
            _temp.ForeColor = controlFrontColor;
            _temp.Font = controlFontBold;
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
        }

        private void DirectRequestClick(object sender, EventArgs e)
        {
            messageFormatPanel.Controls.Clear();
            messageFormatPanel.Controls.Add(createTemplate("RECIPIENT:"));
            messageFormatPanel.Controls.Add(createTextBox(parent.currentATCUnit, 4, true));
            messageFormatPanel.SetFlowBreak(messageFormatPanel.Controls[messageFormatPanel.Controls.Count - 1], true);
            messageFormatPanel.Controls.Add(createTemplate("REQUEST DIRECT TO "));
            messageFormatPanel.Controls.Add(createTextBox("", 5));
            messageFormatPanel.SetFlowBreak(messageFormatPanel.Controls[messageFormatPanel.Controls.Count - 1], true);
            messageFormatPanel.Controls.Add(createCheckBox("DUE TO WX"));
            messageFormatPanel.Controls.Add(createCheckBox("DUE TO A/C PERFORMANCE"));

        }

        private void LevelRequestClick(object sender, EventArgs e)
        {
            messageFormatPanel.Controls.Clear();
            messageFormatPanel.Controls.Add(createTemplate("RECIPIENT:"));
            messageFormatPanel.Controls.Add(createTextBox(parent.currentATCUnit, 4, true));
            messageFormatPanel.SetFlowBreak(messageFormatPanel.Controls[messageFormatPanel.Controls.Count - 1], true);
            messageFormatPanel.Controls.Add(createTemplate("REQUEST FL"));
            messageFormatPanel.Controls.Add(createTextBox("", 3));
            messageFormatPanel.SetFlowBreak(messageFormatPanel.Controls[messageFormatPanel.Controls.Count - 1], true);
            messageFormatPanel.Controls.Add(createCheckBox("DUE TO WX"));
            messageFormatPanel.Controls.Add(createCheckBox("DUE TO A/C PERFORMANCE"));
        }

        private void SpeedRequestClick(object sender, EventArgs e)
        {
            messageFormatPanel.Controls.Clear();
            messageFormatPanel.Controls.Add(createTemplate("RECIPIENT:"));
            messageFormatPanel.Controls.Add(createTextBox(parent.currentATCUnit, 4, true));
            messageFormatPanel.SetFlowBreak(messageFormatPanel.Controls[messageFormatPanel.Controls.Count - 1], true);
            messageFormatPanel.Controls.Add(createTemplate("REQUEST MACH"));
            messageFormatPanel.Controls.Add(createTextBox("", 1, false, true));
            messageFormatPanel.Controls.Add(createTemplate("."));
            messageFormatPanel.Controls.Add(createTextBox("", 2, false, true));
            messageFormatPanel.SetFlowBreak(messageFormatPanel.Controls[messageFormatPanel.Controls.Count - 1], true);
            messageFormatPanel.Controls.Add(createCheckBox("DUE TO WX"));
            messageFormatPanel.Controls.Add(createCheckBox("DUE TO A/C PERFORMANCE"));
        }

        private void pdcButton_Click(object sender, EventArgs e)
        {
            messageFormatPanel.Controls.Clear();
            messageFormatPanel.Controls.Add(createTemplate("RECIPIENT:"));
            messageFormatPanel.Controls.Add(createTextBox(userVATSIMData.flight_plan.departure, 4));
            messageFormatPanel.SetFlowBreak(messageFormatPanel.Controls[messageFormatPanel.Controls.Count - 1], true);
            messageFormatPanel.Controls.Add(createTemplate("REQUEST PREDEP CLEARANCE"));
            messageFormatPanel.Controls.Add(createTextBox(userVATSIMData.callsign, 7));
            messageFormatPanel.Controls.Add(createTextBox(userVATSIMData.flight_plan.aircraft_short, 4));
            messageFormatPanel.SetFlowBreak(messageFormatPanel.Controls[messageFormatPanel.Controls.Count - 1], true);
            messageFormatPanel.Controls.Add(createTemplate("TO"));
            messageFormatPanel.Controls.Add(createTextBox(userVATSIMData.flight_plan.arrival, 4));
            messageFormatPanel.Controls.Add(createTemplate("AT"));
            messageFormatPanel.Controls.Add(createTextBox(userVATSIMData.flight_plan.departure, 4));
            messageFormatPanel.SetFlowBreak(messageFormatPanel.Controls[messageFormatPanel.Controls.Count - 1], true);
            messageFormatPanel.Controls.Add(createTemplate("STAND"));
            messageFormatPanel.Controls.Add(createTextBox("", 4));
            messageFormatPanel.Controls.Add(createTemplate("ATIS"));
            messageFormatPanel.Controls.Add(createTextBox("", 1));
            messageFormatPanel.SetFlowBreak(messageFormatPanel.Controls[messageFormatPanel.Controls.Count - 1], true);
            messageFormatPanel.Controls.Add(CreateMultiLineBox(""));

            pdcRadioButton.Checked = true;
        }

        private void logonButton_Click(object sender, EventArgs e)
        {
            messageFormatPanel.Controls.Clear();
            messageFormatPanel.Controls.Add(createTemplate("ATC UNIT:"));
            messageFormatPanel.Controls.Add(createTextBox("", 4));

            logonRadioButton.Checked = true;
        }

        private Label createTemplate(string _text)
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

        private UITextBox createTextBox(string _text, int _maxLength, bool _readOnly = false, bool _numsOnly = false)
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
            _temp.ReadOnly = _readOnly;
            _temp.TextAlign = HorizontalAlignment.Center;
            
            if(_numsOnly)
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
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }

        private CheckBox createCheckBox(string _text)
        {
            CheckBox _temp = new CheckBox();

            _temp.BackColor = controlBackColor;
            _temp.ForeColor = controlFrontColor;
            _temp.Font = controlFont;
            _temp.Text = _text;
            _temp.Padding = new Padding(3, 10, 3, -30);
            _temp.AutoSize = true;
            _temp.Click += DeselectCheckBox;
            return _temp;
        }

        private void DeselectCheckBox(object sender, EventArgs e)
        {
            CheckBox _sender = (CheckBox)sender;

            foreach (CheckBox box in messageFormatPanel.Controls.OfType<CheckBox>())
            {
                if (box.Text != _sender.Text)
                {
                    box.Checked = false;
                }
            }

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

        private void clearButton_Click(object sender, EventArgs e)
        {
            messageFormatPanel.Controls.Clear();
        }

        private void sendButton_Click(object sender, EventArgs e)
        {

            RadioButton radioBtn = radioContainer.Controls.OfType<RadioButton>()
                                       .Where(x => x.Checked).FirstOrDefault();

            Console.WriteLine(radioBtn.Name);
            if (radioBtn != null)
            {
                string _recipient = "";
                string _formatMessage = "";

                switch (radioBtn.Name)
                {
                    
                    case "pdcRadioButton":

                        for (int i = 0; i < messageFormatPanel.Controls.Count - 2; i++)
                        {
                            if (messageFormatPanel.Controls[i].Text.Length < 1)
                            {
                                Console.WriteLine(i);
                                return;
                            }
                        }

                        _recipient = messageFormatPanel.Controls[1].Text;

                        for (int i = 2; i < messageFormatPanel.Controls.Count; i++)
                        {
                            _formatMessage += messageFormatPanel.Controls[i].Text + " ";
                        }
                        parent.SendCPDLCMessage(_recipient, "TELEX", _formatMessage.Trim());
                        break;

                    case "logonRadioButton":

                        foreach (Control _control in messageFormatPanel.Controls)
                        {
                            if (_control.Text.Length < 1)
                            {
                                return;
                            }
                        }

                        _recipient = messageFormatPanel.Controls[1].Text;
                        if (needsLogon)
                        {
                            _formatMessage = String.Format("/data2/{0}//Y/REQUEST LOGON", parent.messageOutCounter);
                        }
                        else
                        {
                            _formatMessage = String.Format("/data2/{0}//Y/LOGOFF", parent.messageOutCounter);

                        }
                        parent.SendCPDLCMessage(_recipient, "CPDLC", _formatMessage);
                        parent.messageOutCounter += 1;

                        break;

                    case "requestRadioButton":

                        foreach (Control _control in messageFormatPanel.Controls)
                        {
                            if (_control.Text.Length < 1)
                            {
                                return;
                            }
                        }

                        _formatMessage = String.Format("/data2/{0}//Y/", parent.messageOutCounter);
                        _recipient = messageFormatPanel.Controls[1].Text;

                        for (int i = 2; i < messageFormatPanel.Controls.Count - 2; i++)
                        {
                            _formatMessage += messageFormatPanel.Controls[i].Text + "";
                        }

                        CheckBox dueToBox = messageFormatPanel.Controls.OfType<CheckBox>()
                                       .Where(x => x.Checked).FirstOrDefault();

                        if (dueToBox != default(CheckBox))
                        {
                            _formatMessage += " " + dueToBox.Name;
                        }

                        parent.SendCPDLCMessage(_recipient, "CPDLC", _formatMessage);
                        parent.messageOutCounter += 1;

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

        private void WindowDrag(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
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

        private void requestButton_Click(object sender, EventArgs e)
        {
            requestRadioButton.Checked = true;

            popupMenu.Items.Clear();
            popupMenu.Items.Add(directRequestMenu);
            popupMenu.Items.Add(levelRequestMenu);
            popupMenu.Items.Add(speedRequestMenu);
            //popupMenu.AutoSize = false;
            //popupMenu.Size = new Size(104, 114);
            popupMenu.Show(requestButton, new Point(requestButton.Width, 0));
        }
    }
}
