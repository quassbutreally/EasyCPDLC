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
        public Font controlFont = new Font("Oxygen", 10.0f, FontStyle.Regular);
        public Font controlFontBold = new Font("Oxygen", 10.0f, FontStyle.Bold);
        public Color controlBackColor = Color.FromArgb(5, 5, 5);
        public Color controlFrontColor = SystemColors.ControlLight;
        private ContextMenuStrip popupMenu = new ContextMenuStrip();
        private ToolStripMenuItem replyMenu;
        ToolStripMenuItem acknowledgeMenu;
        ToolStripMenuItem wilcoMenu;
        ToolStripMenuItem standbyMenu;
        ToolStripMenuItem unableMenu;
        private SoundPlayer player = new SoundPlayer(Properties.Resources.honk);
        private RegistryKey regKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\EasyCPDLC");
        private Pilots pilotList;
        private static Regex hoppieParse = new Regex("{(.*?)}");
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
            replyMenu.Click += ReplyMessage;

            acknowledgeMenu = createMenuItem("ACKNOWLEDGE");
            //replyMenu.DropDownItems.Add(acknowledgeMenu);
            acknowledgeMenu.Click += AcknowledgeMessage;

            wilcoMenu = createMenuItem("WILCO");
            //replyMenu.DropDownItems.Add(wilcoMenu);
            wilcoMenu.Click += WilcoMessage;

            unableMenu = createMenuItem("UNABLE");
            //replyMenu.DropDownItems.Add(unableMenu);
            unableMenu.Click += UnableMessage;

            standbyMenu = createMenuItem("STANDBY");
            //replyMenu.DropDownItems.Add(standbyMenu);
            standbyMenu.Click += StandbyMessage;

            ToolStripMenuItem deleteMenu = createMenuItem("DELETE");
            deleteMenu.Click += DeleteElement;
            popupMenu.Items.Add(deleteMenu);
        }

        private Control SenderToControl(object sender)
        {
            ToolStripItem _sender = (ToolStripItem)sender;
            ContextMenuStrip menu = (ContextMenuStrip)_sender.Owner;
            Control sourceControl = menu.SourceControl;
            return sourceControl;
        }

        private void ReplyMessage(object sender, EventArgs e)
        {
            Control sourceControl = SenderToControl(sender);
            CPDLCMessage message = (CPDLCMessage)sourceControl;

            if (message.type == "TELEX")
            {
                TelexForm tForm = new TelexForm(this, message.recipient);
                tForm.Show();
            }
        }

        private void AcknowledgeMessage(object sender, EventArgs e)
        {
            Control sourceControl = SenderToControl(sender);
            CPDLCMessage message = (CPDLCMessage)sourceControl;

            message.acknowledged = true;
            SendCPDLCMessage(message.recipient, message.type, "ACKNOWLEDGED");
        }

        private void WilcoMessage(object sender, EventArgs e)
        {
            Control sourceControl = SenderToControl(sender);
            CPDLCMessage message = (CPDLCMessage)sourceControl;

            message.acknowledged = true;
            SendCPDLCMessage(message.recipient, message.type, "WILCO");
        }

        private void StandbyMessage(object sender, EventArgs e)
        {
            Control sourceControl = SenderToControl(sender);
            CPDLCMessage message = (CPDLCMessage)sourceControl;

            message.acknowledged = true;
            SendCPDLCMessage(message.recipient, message.type, "STANDBY");
        }

        private void UnableMessage(object sender, EventArgs e)
        {
            Control sourceControl = SenderToControl(sender);
            CPDLCMessage message = (CPDLCMessage)sourceControl;

            message.acknowledged = true;
            SendCPDLCMessage(message.recipient, message.type, "UNABLE");
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
        public async Task SendCPDLCMessage(string recipient, string messageType, string packetData)
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

            if (messageType != "poll")
            {
                WriteMessage(packetData, messageType, "POLL");
            }

            if (printString != "OK")
            {
                ResponseHandler(printString);
            }

            Console.WriteLine(printString);

            return;

        }
        private CPDLCMessage createCPDLCMessage(string _text, string _type, string _recipient)
        {
            CPDLCMessage _message = new CPDLCMessage(_type, _recipient);
            _message.MaximumSize = new Size(this.outputTable.Width - 20, 50);
            _message.AutoSize = true;
            _message.BackColor = controlBackColor;
            _message.ForeColor = Color.Orange;
            _message.Font = controlFont;
            _message.Text = _text;
            _message.BorderStyle = BorderStyle.None;
            _message.Width = this.outputTable.Width - 20;
            _message.Height = 20;
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
            _sender.ForeColor = controlFrontColor;
            if (me.Button == MouseButtons.Right)
            {
                if (_sender.type != "SYSTEM" && !_sender.acknowledged)
                {
                    popupMenu.Items.Add(replyMenu);

                    if (_sender.type != "TELEX")
                    {
                        //add logic for adding other reply menus here
                        replyMenu.DropDownItems.Add(acknowledgeMenu);
                        replyMenu.DropDownItems.Add(wilcoMenu);
                        replyMenu.DropDownItems.Add(standbyMenu);
                        replyMenu.DropDownItems.Add(unableMenu);
                    }
                }

                popupMenu.Show(_sender, _sender.PointToClient(Cursor.Position));
            }
        }
        private async Task ResponseHandler(string response)
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
        private void WriteMessage(string _response, string _type, string _recipient)
        {
            outputTable.Invoke(new Action(() => outputTable.Controls.Add(createCPDLCMessage(DateTime.Now.ToString("HH:mm") + "  -  " + _response + ".", _type, _recipient), 0, outputTable.RowCount - 1)));
            outputTable.Invoke(new Action(() => outputTable.RowCount += 1));
            outputTable.Invoke(new Action(() => outputTable.RowStyles.Add(new RowStyle(SizeType.AutoSize))));
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
                response = "SYSTEM: DETAILS RETRIEVED FOR " + userVATSIMData.callsign;
                callsign = userVATSIMData.callsign;
                PeriodicCheckMessage(updateTimer, requestCancellationToken);
                requestButton.Enabled = true;
                telexButton.Enabled = true;
            }
            catch (Exception)
            {
                response = "SYSTEM: ERROR RETRIEVING DETAILS. ENSURE A FLIGHT PLAN HAS BEEN FILED AND TRY AGAIN";
                requestButton.Enabled = false;
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
