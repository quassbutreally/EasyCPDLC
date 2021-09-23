using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyCPDLC
{
    public partial class MainForm : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        public PilotData userVATSIMData;
        private static readonly HttpClient webclient = new HttpClient();
        private string logonCode;
        private int cid;
        private string callsign;
        public int messageOutCounter = 1;
        public string currentATCUnit = null;
        public Font controlFont = new Font("Oxygen", 10.0f, FontStyle.Regular);
        public Font controlFontBold = new Font("Oxygen", 10.0f, FontStyle.Bold);
        public Color controlBackColor = Color.FromArgb(5, 5, 5);
        public Color controlFrontColor = SystemColors.ControlLight;
        private ContextMenuStrip popupMenu = new ContextMenuStrip();
        private ToolStripMenuItem replyMenu;
        ToolStripMenuItem wilcoMenu;
        ToolStripMenuItem rogerMenu;
        ToolStripMenuItem affirmativeMenu;
        ToolStripMenuItem negativeMenu;
        ToolStripMenuItem standbyMenu;
        ToolStripMenuItem unableMenu;
        ToolStripMenuItem deleteMenu;
        ToolStripMenuItem freeTextMenu;
        private SoundPlayer player = new SoundPlayer(Properties.Resources.honk);
        private RegistryKey regKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\EasyCPDLC");
        private Pilots pilotList;
        private static Regex hoppieParse = new Regex(@"{(.*?)}");
        private static Regex cpdlcHeaderParse = new Regex(@"(\/\s*)\w*");
        private static Regex cpdlcUnitParse = new Regex(@"_@([\w]*)@_");
        private static readonly TimeSpan updateTimer = TimeSpan.FromSeconds(10);
        private static readonly CancellationTokenSource requestCancellationTokenSource = new CancellationTokenSource();
        private static readonly CancellationToken requestCancellationToken = requestCancellationTokenSource.Token;

        public MainForm()
        {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            outputTable.BorderStyle = BorderStyle.FixedSingle;
            outputTable.HorizontalScroll.Maximum = 0;
            outputTable.AutoScroll = false;
            outputTable.VerticalScroll.Visible = false;
            outputTable.AutoScroll = true;
            for (int i = 1; i < 250; i++)
            {
                outputTable.RowCount += 1;

                outputTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }

            InitialisePopupMenu();
            ShowSetupForm();
            Setup();

        }
        private ToolStripMenuItem createMenuItem(string name)
        {
            ToolStripMenuItem _temp = new ToolStripMenuItem(name);
            _temp.BackColor = controlBackColor;
            _temp.ForeColor = controlFrontColor;
            _temp.Font = controlFont;
            _temp.TextAlign = ContentAlignment.TopLeft;

            return _temp;
        }
        private void InitialisePopupMenu()
        {
            popupMenu.BackColor = controlBackColor;
            popupMenu.ForeColor = controlFrontColor;
            popupMenu.Font = controlFont;
            popupMenu.ShowImageMargin = false;

            replyMenu = createMenuItem("REPLY");

            rogerMenu = createMenuItem("ROGER");
            rogerMenu.Click += RogerMessage;

            wilcoMenu = createMenuItem("WILCO");
            wilcoMenu.Click += WilcoMessage;

            unableMenu = createMenuItem("UNABLE");
            unableMenu.Click += UnableMessage;

            affirmativeMenu = createMenuItem("AFFIRMATIVE");
            affirmativeMenu.Click += AffirmativeMessage;

            negativeMenu = createMenuItem("NEGATIVE");
            negativeMenu.Click += NegativeMessage;

            standbyMenu = createMenuItem("STANDBY");
            standbyMenu.Click += StandbyMessage;

            freeTextMenu = createMenuItem("FREE TEXT");
            freeTextMenu.Click += freeTextMessage;


            deleteMenu = createMenuItem("DELETE");
            deleteMenu.Click += DeleteElement;
        }

        private Control SenderToControl(object sender)
        {
            ToolStripDropDownItem _sender = (ToolStripDropDownItem)sender;
            ContextMenuStrip menu = (ContextMenuStrip)_sender.DropDown.OwnerItem.OwnerItem.Owner;
            Control sourceControl = menu.SourceControl;
            return sourceControl;
        }

        private void freeTextMessage(object sender, EventArgs e)
        {
            Control sourceControl = SenderToControl(sender);
            CPDLCMessage message = (CPDLCMessage)sourceControl;

            //message.acknowledged = true;
            //message.ForeColor = controlFrontColor;
            TelexForm tForm = new TelexForm(this, message.recipient);
            tForm.Show();
        }
        private void RogerMessage(object sender, EventArgs e)
        {
            Control sourceControl = SenderToControl(sender);
            CPDLCMessage message = (CPDLCMessage)sourceControl;

            message.acknowledged = true;
            message.ForeColor = controlFrontColor;
            message.header.responseID = messageOutCounter;
            SendCPDLCMessage(message.recipient, message.type, String.Format("/data2/{0}/{1}/N/ROGER", messageOutCounter, message.header.messageID));
            messageOutCounter += 1;
        }

        private void WilcoMessage(object sender, EventArgs e)
        {
            Control sourceControl = SenderToControl(sender);
            CPDLCMessage message = (CPDLCMessage)sourceControl;

            message.acknowledged = true;
            message.ForeColor = controlFrontColor;
            SendCPDLCMessage(message.recipient, message.type, String.Format("/data2/{0}/{1}/N/WILCO", messageOutCounter, message.header.messageID));
            messageOutCounter += 1;
        }

        private void StandbyMessage(object sender, EventArgs e)
        {
            Control sourceControl = SenderToControl(sender);
            CPDLCMessage message = (CPDLCMessage)sourceControl;
            SendCPDLCMessage(message.recipient, message.type, String.Format("/data2/{0}/{1}/N/STANDBY", messageOutCounter, message.header.messageID));
            messageOutCounter += 1;
        }

        private void UnableMessage(object sender, EventArgs e)
        {
            Control sourceControl = SenderToControl(sender);
            CPDLCMessage message = (CPDLCMessage)sourceControl;

            message.acknowledged = true;
            message.ForeColor = controlFrontColor;
            SendCPDLCMessage(message.recipient, message.type, String.Format("/data2/{0}/{1}/N/UNABLE", messageOutCounter, message.header.messageID));
            messageOutCounter += 1;
        }

        private void AffirmativeMessage(object sender, EventArgs e)
        {
            Control sourceControl = SenderToControl(sender);
            CPDLCMessage message = (CPDLCMessage)sourceControl;

            message.acknowledged = true;
            message.ForeColor = controlFrontColor;
            SendCPDLCMessage(message.recipient, message.type, String.Format("/data2/{0}/{1}/N/AFFIRMATIVE", messageOutCounter, message.header.messageID));
            messageOutCounter += 1;
        }

        private void NegativeMessage(object sender, EventArgs e)
        {
            Control sourceControl = SenderToControl(sender);
            CPDLCMessage message = (CPDLCMessage)sourceControl;

            message.acknowledged = true;
            message.ForeColor = controlFrontColor;
            SendCPDLCMessage(message.recipient, message.type, String.Format("/data2/{0}/{1}/N/NEGATIVE", messageOutCounter, message.header.messageID));
            messageOutCounter += 1;
        }

        private void ShowSetupForm()
        {

            DataEntry dataEntry = new DataEntry(regKey.GetValue("hoppieCode"), regKey.GetValue("vatsimCID"));

            if (dataEntry.ShowDialog(this) == DialogResult.OK)
            {
                logonCode = dataEntry.hoppieLogonCode;
                cid = dataEntry.vatsimCID;
                if (dataEntry.remember)
                {
                    regKey.SetValue("hoppieCode", logonCode);
                    regKey.SetValue("vatsimCID", cid);
                }
                else
                {
                    if (!(regKey.GetValue("hoppieCode") is null))
                    {
                        regKey.DeleteValue("hoppieCode");
                    }
                    if (!(regKey.GetValue("vatsimCID") is null))
                    {
                        regKey.DeleteValue("vatsimCID");
                    }

                }
            }
            else
            {
                Application.Exit();
            }
        }
        private void Setup()
        {
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString("https://data.vatsim.net/v3/vatsim-data.json");
                pilotList = JsonSerializer.Deserialize<Pilots>(json);
            }

            this.Invoke(new Action(() => this.retrieveButton.Enabled = true));

        }
        private async Task PeriodicCheckMessage(TimeSpan interval, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await SendCPDLCMessage("NONE", "poll", "");
                await Task.Delay(interval, cancellationToken);
            }
        }
        public async Task SendCPDLCMessage(string recipient, string messageType, string packetData, bool _outbound = true)
        {
            var connectionValues = new Dictionary<string, string> {
                {"logon", logonCode},
                {"from", callsign},
                {"to", recipient},
                {"type", messageType},
                {"packet", packetData}
            };

            var content = new FormUrlEncodedContent(connectionValues);
            var response = await webclient.PostAsync("http://www.hoppie.nl/acars/system/connect.html", content);
            var responseString = await response.Content.ReadAsStringAsync();
            string printString = responseString.ToString().ToUpper();

            if (messageType == "CPDLC" && _outbound)
            {
                WriteMessage(packetData.Split('/').Last(), messageType, recipient, _outbound);
            }
            else if (messageType != "poll")
            {
                WriteMessage(packetData, messageType, recipient, _outbound);
            }
            

            if (printString != "OK")
            {
                if (printString.StartsWith("/data2/"))
                {
                    CPDLCParser(printString);
                }
                else
                {
                    TelexParser(printString);
                }
                
            }

            Console.WriteLine(printString);

            return;

        }
        private CPDLCMessage createCPDLCMessage(string _text, string _type, string _recipient, bool _outbound = false, CPDLCResponse _header = null)
        {
            CPDLCMessage _message = new CPDLCMessage(_type, _recipient, _outbound, _header);
            Size maxSize = new Size();
            maxSize.Width = this.outputTable.Width - 20;
            _message.MaximumSize = maxSize;
            _message.Width = this.outputTable.Width - 20;
            _message.AutoSize = true;
            _message.BackColor = controlBackColor;
            _message.ForeColor = Color.Orange;
            _message.Font = controlFont;
            _message.Text = _text;
            _message.BorderStyle = BorderStyle.None;
            _message.MouseDown += MessageClicked;



            return _message;
        }
        private void DeleteElement(object sender, EventArgs e)
        {
            ToolStripItem _sender = (ToolStripItem)sender;
            ContextMenuStrip menu = (ContextMenuStrip)_sender.Owner;
            Control sourceControl = menu.SourceControl;

            TableLayoutHelper.RemoveArbitraryRow(outputTable, outputTable.GetPositionFromControl(sourceControl).Row);
        }
        private void MessageClicked(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            CPDLCMessage _sender = (CPDLCMessage)sender;
            if (_sender.type != "CPDLC" || _sender.outbound || _sender.header.responses == "NE")
            {
                _sender.ForeColor = controlFrontColor;
            }
            if (me.Button == MouseButtons.Right)
            {
                popupMenu.Items.Clear();
                popupMenu.Items.Add(deleteMenu);
                deleteMenu.Enabled = true;
                if(_sender.type == "CPDLC" && !_sender.outbound && !_sender.acknowledged)
                {
                    deleteMenu.Enabled = false;
                }
                if (_sender.type != "SYSTEM" && !_sender.acknowledged && !_sender.outbound)
                {
                    popupMenu.Items.Add(replyMenu);

                    switch(_sender.header.responses)
                    {
                        case "WU":
                            replyMenu.DropDownItems.Add(wilcoMenu);
                            replyMenu.DropDownItems.Add(unableMenu);
                            replyMenu.DropDownItems.Add(standbyMenu);
                            break;

                        case "AN":
                            replyMenu.DropDownItems.Add(affirmativeMenu);
                            replyMenu.DropDownItems.Add(negativeMenu);
                            replyMenu.DropDownItems.Add(standbyMenu);
                            break;

                        case "R":
                            replyMenu.DropDownItems.Add(rogerMenu);
                            break;

                        default:
                            break;

                    }

                    replyMenu.DropDownItems.Add(freeTextMenu);
                }

                popupMenu.Show(_sender, _sender.PointToClient(Cursor.Position));
            }
        }

        private async Task CPDLCParser(string _response)
        {

            var unit = cpdlcUnitParse.Match(_response);
            if (unit.Success)
            {
                currentATCUnit = unit.Value.Trim('_', '@');
            }

            var responses = cpdlcHeaderParse.Matches(_response);
            foreach(Match response in responses)
            {
                Console.WriteLine(response.Value);
            }
            CPDLCResponse header = new CPDLCResponse();
            header.dataType = responses[0].Value.Trim('/');
            header.messageID = Convert.ToInt32(responses[1].Value.Trim('/'));
            header.responseID = responses[2].Value.Trim('/').Length < 1 ? 0 : Convert.ToInt32(responses[2].Value.Trim('/'));
            header.responses = responses[3].Value.Trim('/');
            string sender = responses[4].Value.Trim('/');

            string message = callsign + " " + _response.Split(new string[] { callsign }, StringSplitOptions.None)[1].Replace("@@", "N/A").Replace('@', ' ').Replace("_","");
            message = Regex.Replace(message, @"\s+", " ");

            if(message == "LOGOFF")
            {
                currentATCUnit = null;
            }

            WriteMessage(message, "CPDLC", sender, false, header);

            player.Play();

            return;
        }
        private async Task TelexParser(string response)
        {
            var responses = hoppieParse.Matches(response);

            foreach (Match _response in responses)
            {
                string format_response = "";
                string[] _modify = _response.Groups[1].Value.Replace("}", "").Split('{');
                string sender = _modify[0].Split(' ')[0];
                string type = _modify[0].Split(' ')[1];
                format_response += sender + ": " + _modify[1];

                WriteMessage(format_response, type, sender);

                player.Play();

            }
            return;
        }
        private void WriteMessage(string _response, string _type, string _recipient, bool _outbound = false, CPDLCResponse _header = null)
        {
            CPDLCMessage message;
            if (_outbound)
            {
                message = createCPDLCMessage(DateTime.Now.ToString("HH:mm") + "  -  " + callsign + ": " + _recipient + " " + _response + ".", _type, _recipient, _outbound, _header);
            }
            else 
            {
                message = createCPDLCMessage(DateTime.Now.ToString("HH:mm") + "  -  " + _recipient + ": " + _response + ".", _type, _recipient, _outbound, _header);
            }
            outputTable.Invoke(new Action(() => outputTable.Controls.Add(message, 0, outputTable.RowCount - 1)));
            outputTable.Invoke(new Action(() => outputTable.RowCount += 1));
            outputTable.Invoke(new Action(() => outputTable.RowStyles.Add(new RowStyle(SizeType.AutoSize))));
            outputTable.Invoke(new Action(() => outputTable.ScrollControlIntoView(message)));
        }
        private void exitButton_Click(object sender, EventArgs e)
        {
            requestCancellationTokenSource.Cancel();
            Application.Exit();
        }
        private void MoveWindow(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        private void retrieveButton_Click(object sender, EventArgs e)
        {
            string response = "";
            try
            {
                userVATSIMData = pilotList.pilots.Where(i => i.cid == cid).FirstOrDefault();
                response = "DETAILS RETRIEVED FOR " + userVATSIMData.callsign;
                callsign = userVATSIMData.callsign;
                PeriodicCheckMessage(updateTimer, requestCancellationToken);
                atcButton.Enabled = true;
                telexButton.Enabled = true;

                CPDLCParser("/data2/5//WU/EDDM PDC 002 . . . . . CLD 1919 210921 EDDM PDC 002 @ACA81@ CLRD TO @LFLL@ OFF @26L@ VIA @MERSI5S@ SQUAWK @1000@ ADT @MDI@ CLIMB TO @FL070@ NEXT FREQ @@ STARTUP APPROVED");
            }
            catch (Exception)
            {
                response = "ERROR RETRIEVING DETAILS. ENSURE A FLIGHT PLAN HAS BEEN FILED AND TRY AGAIN";
                atcButton.Enabled = false;
                telexButton.Enabled = false;
            }

            finally
            {
                WriteMessage(response, "SYSTEM", "SYSTEM");
            }
        }
        private void telexButton_Click(object sender, EventArgs e)
        {
            TelexForm tForm = new TelexForm(this);
            tForm.Show();
        }
        private void requestButton_Click(object sender, EventArgs e)
        {
            RequestForm rForm = new RequestForm(this);
            rForm.Show();
        }
    }
    internal class NoHighlightRenderer : ToolStripProfessionalRenderer
    {
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.OwnerItem == null)
            {
                base.OnRenderMenuItemBackground(e);
            }
        }
    }
    public static class TableLayoutHelper
    {
        public static void RemoveArbitraryRow(TableLayoutPanel panel, int rowIndex)
        {
            if (rowIndex >= panel.RowCount)
            {
                return;
            }

            // delete all controls of row that we want to delete
            for (int i = 0; i < panel.ColumnCount; i++)
            {
                var control = panel.GetControlFromPosition(i, rowIndex);
                panel.Controls.Remove(control);
            }

            // move up row controls that comes after row we want to remove
            for (int i = rowIndex + 1; i < panel.RowCount; i++)
            {
                for (int j = 0; j < panel.ColumnCount; j++)
                {
                    var control = panel.GetControlFromPosition(j, i);
                    if (control != null)
                    {
                        panel.SetRow(control, i - 1);
                    }
                }
            }

            var removeStyle = panel.RowCount - 1;

            if (panel.RowStyles.Count > removeStyle)
                panel.RowStyles.RemoveAt(removeStyle);

            panel.RowCount--;
        }
    }
}
