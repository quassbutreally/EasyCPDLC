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

using Microsoft.Win32;
using NLog;
using Octokit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Http;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Principal;
using FSUIPC;


namespace EasyCPDLC
{
    public partial class MainForm : Form
    {

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        private const int cGrip = 16;
        private const int cCaption = 32;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public Pilot userVATSIMData;
        private VATSIMRootobject vatsimData;
        private Navlog simbriefData;
        public string[] reportFixes;
        public string nextFix = null;

        public FSUIPCData fsuipc = new FSUIPCData();
        public bool fsConnectionOpen = false;
        public int fsuipcErrorCount = 1;

        public Random random = new Random();

        readonly private List<Contract> contracts = new List<Contract>();

        private static readonly HttpClient webclient = new HttpClient();
        private string logonCode;
        private int cid;
        public string callsign;

        private RequestForm rForm;
        private TelexForm tForm;
        private SettingsForm sForm;

        public bool stayOnTop
        {
            get
            {
                return Properties.Settings.Default.StayOnTop;
            }
            set
            {
                Properties.Settings.Default.StayOnTop = value;
                this.TopMost = value;
            }
        }

        public bool playSound
        {
            get
            {
                return Properties.Settings.Default.PlayAudibleAlert;
            }
            set
            {
                Properties.Settings.Default.PlayAudibleAlert = value;
            }
        }

        public bool useFSUIPC
        {
            get
            {
                return Properties.Settings.Default.UseFSUIPC;
            }
            set
            {
                Properties.Settings.Default.UseFSUIPC = value;
            }
        }

        public string simbriefID
        {
            get
            {
                return Properties.Settings.Default.SimbriefUsername;
            }
            set
            {
                Properties.Settings.Default.SimbriefUsername = value;
            }
        }

        public int messageOutCounter = 1;
        private bool _connected;
        public bool connected
        {
            get
            {
                return _connected;
            }
            set
            {
                _connected = value;
                if (_connected)
                {
                    retrieveButton.Text = "DISCONNECT";
                    atcButton.Enabled = true;
                    telexButton.Enabled = true;
                }
                else
                {
                    retrieveButton.Text = "CONNECT";
                    atcButton.Enabled = false;
                    telexButton.Enabled = false;
                }
            }
        }

        public string pendingLogon = null;
        private string _currentATCUnit;
        public string currentATCUnit
        {
            get
            {
                return _currentATCUnit;
            }
            set
            {
                _currentATCUnit = value;
                try
                {
                    if (_currentATCUnit is null)
                    {
                        atcUnitDisplay.Text = "----";
                        rForm.needsLogon = true;

                    }
                    else
                    {
                        atcUnitDisplay.Text = _currentATCUnit;
                        rForm.needsLogon = false;

                    }
                }
                catch (NullReferenceException)
                {

                }
            }
        }

        public Font controlFont = new Font("Oxygen", 10.0f, FontStyle.Regular);
        public Font controlFontBold = new Font("Oxygen", 10.0f, FontStyle.Bold);
        public Font textFont = new Font("B612 Mono", 10.0f, FontStyle.Regular);
        public Font textFontBold = new Font("B612 Mono", 12.5f, FontStyle.Bold);
        public Font dataEntryFont = new Font("B612 Mono", 11.0f, FontStyle.Regular);
        public Color controlBackColor = Color.FromArgb(5, 5, 5);
        public Color controlFrontColor = SystemColors.ControlLight;

        private readonly ContextMenuStrip popupMenu = new ContextMenuStrip();
        ToolStripMenuItem deleteAllMenu;

        private System.Windows.Forms.Label wilcoLabel;
        private System.Windows.Forms.Label rogerLabel;
        private System.Windows.Forms.Label affirmativeLabel;
        private System.Windows.Forms.Label negativeLabel;
        private System.Windows.Forms.Label standbyLabel;
        private System.Windows.Forms.Label unableLabel;
        private System.Windows.Forms.Label deleteLabel;
        private System.Windows.Forms.Label freeTextLabel;
        private System.Windows.Forms.Label returnLabel;

        private CPDLCMessage previewMessage;

        private readonly SoundPlayer player = new SoundPlayer();
        private readonly RegistryKey regKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\EasyCPDLC");

        private static readonly Regex hoppieParse = new Regex(@"{(.*?)}");
        private static readonly Regex cpdlcHeaderParse = new Regex(@"(\/\s*)\w*");
        private static readonly Regex cpdlcUnitParse = new Regex(@"_@([\w]*)@_");

        private static readonly TimeSpan updateTimer = TimeSpan.FromSeconds(10);
        private CancellationTokenSource requestCancellationTokenSource;
        private CancellationToken requestCancellationToken;

        public MainForm()
        {
            var config = new NLog.Config.LoggingConfiguration();
            var logFile = new NLog.Targets.FileTarget("logfile") { FileName = "EasyCPDLCLog.txt" };
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logFile);
            LogManager.Configuration = config;
            Logger.Info("Logging initialised, beginning setup");

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            string _customSoundFile = System.Windows.Forms.Application.StartupPath + @"\Sounds\Notification.wav";
            if(File.Exists(_customSoundFile))
            {
                player.SoundLocation = _customSoundFile;
            }
            else
            {
                player.Stream = Properties.Resources.notification;
            }
            player.Play();
            InitializeComponent();
            this.TopMost = stayOnTop;
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            currentATCUnit = null;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (new AssemblyName(args.Name).Name == "System.Runtime.CompilerServices.Unsafe")
                return Assembly.LoadFrom(
                    Path.Combine(System.Windows.Forms.Application.StartupPath, "System.Runtime.CompilerServices.Unsafe.dll"));
            throw new Exception();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            outputTable.BorderStyle = BorderStyle.FixedSingle;
            outputTable.HorizontalScroll.Maximum = 0;
            outputTable.AutoScroll = false;
            outputTable.VerticalScroll.Visible = false;
            outputTable.AutoScroll = true;

            CheckNewVersion();
            //CheckAdministrator();
            InitialisePopupMenu();
            ShowSetupForm();
            Setup();

            if(Properties.Settings.Default.MainWindowLocation != new Point(0, 0))
            {
                Location = Properties.Settings.Default.MainWindowLocation;
                Size = Properties.Settings.Default.MainWindowSize;
            }
            
            Logger.Info("Setup completed successfully");
        }

        private async void CheckNewVersion()
        {
            try
            {
                var client = new GitHubClient(new ProductHeaderValue("EasyCPDLC"));
                var releases = await client.Repository.Release.GetAll("josh-seagrave", "EasyCPDLC");
                var latest = releases[0];
                string latestVersion = latest.TagName.Replace("cpdlc", "");
                if (latestVersion != System.Windows.Forms.Application.ProductVersion)
                {
                    DialogResult updateBox = MessageBox.Show(String.Format("New Version {0} Available to download from Github. This application will now exit. Would you like me to take you to the Github page for the latest release?", latestVersion), "New Version Available", MessageBoxButtons.YesNo);
                    if (updateBox == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(latest.HtmlUrl);
                    }
                }
            }

            catch
            {

            }
        }

        public static void CheckAdministrator()
        {
            if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator)) 
            {
                MessageBox.Show("EasyCPDLC does not appear to be running in Administrator mode. This will limit certain functionalities of the program. Please restart EasyCPDLC in admin mode. The program will now exit.", "Error");
                System.Windows.Forms.Application.Exit();
            }
        }

        private ToolStripMenuItem CreateMenuItem(string name)
        {
            ToolStripMenuItem _temp = new ToolStripMenuItem(name)
            {
                BackColor = controlBackColor,
                ForeColor = controlFrontColor,
                Font = controlFont,
                TextAlign = ContentAlignment.TopLeft
            };

            return _temp;
        }
        private void InitialisePopupMenu()
        {
            popupMenu.BackColor = controlBackColor;
            popupMenu.ForeColor = controlFrontColor;
            popupMenu.Font = controlFont;
            popupMenu.ShowImageMargin = false;

            rogerLabel = CreateSpecialLabel("> ROGER", false);
            rogerLabel.Click += (_sender, e) => RogerMessage(_sender, e, previewMessage);

            wilcoLabel = CreateSpecialLabel("> WILCO", false);
            wilcoLabel.Click += (_sender, e) => WilcoMessage(_sender, e, previewMessage);

            standbyLabel = CreateSpecialLabel("> STANDBY", false);
            standbyLabel.Click += (_sender, e) => StandbyMessage(_sender, e, previewMessage);

            unableLabel = CreateSpecialLabel("> UNABLE", false);
            unableLabel.Click += (_sender, e) => UnableMessage(_sender, e, previewMessage);

            affirmativeLabel = CreateSpecialLabel("> AFFIRM", false);
            affirmativeLabel.Click += (_sender, e) => AffirmativeMessage(_sender, e, previewMessage);

            negativeLabel = CreateSpecialLabel("> NEGATIVE", false);
            negativeLabel.Click += (_sender, e) => NegativeMessage(_sender, e, previewMessage);

            freeTextLabel = CreateSpecialLabel("> FREE TEXT", false);
            freeTextLabel.Click += (_sender, e) => FreeTextMessage(_sender, e, previewMessage);

            deleteLabel = CreateSpecialLabel("> DELETE", false);
            deleteLabel.Click += (_sender, e) => DeleteElement(_sender, e, previewMessage);

            returnLabel = CreateSpecialLabel("< RETURN", false);
            returnLabel.Click += ReturnMessage;

            deleteAllMenu = CreateMenuItem("DELETE ALL");
            deleteAllMenu.Click += DeleteAllElement;

            Logger.Info("Login menu initialised");
        }

        private void FreeTextMessage(object sender, EventArgs e, CPDLCMessage message)
        {

            tForm = new TelexForm(this, message.recipient)
            {
                TopMost = stayOnTop
            };
            tForm.Show();
        }

        private void ClearPreview()
        {
            messageFormatPanel.Controls.Clear();
            messageFormatPanel.Visible = false;
        }

        private void ReturnMessage(object sender, EventArgs e)
        {
            ClearPreview();
        }
        private async void RogerMessage(object sender, EventArgs e, CPDLCMessage message)
        {
            message.acknowledged = true;
            int index = outputTable.Controls.GetChildIndex(message);
            ((TimerLabel)outputTable.Controls[index + 1]).canFlash = false;
            outputTable.Controls[index + 1].ForeColor = controlFrontColor;
            message.ForeColor = SystemColors.ControlDark;
            message.header.responseID = messageOutCounter;
            await SendCPDLCMessage(message.recipient, message.type, String.Format("/data2/{0}/{1}/N/ROGER", messageOutCounter, message.header.messageID));
            messageOutCounter += 1;

            ClearPreview();
        }

        private async void WilcoMessage(object sender, EventArgs e, CPDLCMessage message)
        {
            message.acknowledged = true;
            int index = outputTable.Controls.GetChildIndex(message);
            ((TimerLabel)outputTable.Controls[index + 1]).canFlash = false;
            outputTable.Controls[index + 1].ForeColor = controlFrontColor;
            message.ForeColor = SystemColors.ControlDark;
            await SendCPDLCMessage(message.recipient, message.type, String.Format("/data2/{0}/{1}/N/WILCO", messageOutCounter, message.header.messageID));
            messageOutCounter += 1;

            ClearPreview();
        }

        private async void StandbyMessage(object sender, EventArgs e, CPDLCMessage message)
        {
            await SendCPDLCMessage(message.recipient, message.type, String.Format("/data2/{0}/{1}/N/STANDBY", messageOutCounter, message.header.messageID));
            messageOutCounter += 1;

            ClearPreview();
        }

        private async void UnableMessage(object sender, EventArgs e, CPDLCMessage message)
        {
            message.acknowledged = true;
            int index = outputTable.Controls.GetChildIndex(message);
            ((TimerLabel)outputTable.Controls[index + 1]).canFlash = false;
            outputTable.Controls[index + 1].ForeColor = controlFrontColor;
            message.ForeColor = SystemColors.ControlDark;
            await SendCPDLCMessage(message.recipient, message.type, String.Format("/data2/{0}/{1}/N/UNABLE", messageOutCounter, message.header.messageID));
            messageOutCounter += 1;

            ClearPreview();
        }

        private async void AffirmativeMessage(object sender, EventArgs e, CPDLCMessage message)
        {
            message.acknowledged = true;
            int index = outputTable.Controls.GetChildIndex(message);
            ((TimerLabel)outputTable.Controls[index + 1]).canFlash = false;
            outputTable.Controls[index + 1].ForeColor = controlFrontColor;
            message.ForeColor = SystemColors.ControlDark;
            await SendCPDLCMessage(message.recipient, message.type, String.Format("/data2/{0}/{1}/N/AFFIRMATIVE", messageOutCounter, message.header.messageID));
            messageOutCounter += 1;

            ClearPreview();
        }

        private async void NegativeMessage(object sender, EventArgs e, CPDLCMessage message)
        {
            message.acknowledged = true;
            int index = outputTable.Controls.GetChildIndex(message);
            ((TimerLabel)outputTable.Controls[index + 1]).canFlash = false;
            outputTable.Controls[index + 1].ForeColor = controlFrontColor;
            message.ForeColor = SystemColors.ControlDark;
            await SendCPDLCMessage(message.recipient, message.type, String.Format("/data2/{0}/{1}/N/NEGATIVE", messageOutCounter, message.header.messageID));
            messageOutCounter += 1;

            ClearPreview();
        }

        private void ShowSetupForm()
        {

            Logger.Info("Login Form Displayed");

            DataEntry dataEntry = new DataEntry(regKey.GetValue("hoppieCode"), regKey.GetValue("vatsimCID"));

            if (dataEntry.ShowDialog(this) == DialogResult.OK)
            {
                logonCode = dataEntry.hoppieLogonCode;
                cid = dataEntry.vatsimCID;
                if (dataEntry.remember)
                {
                    Logger.Info("REMEMBER ME: TRUE. REGISTRY SET.");
                    regKey.SetValue("hoppieCode", logonCode);
                    regKey.SetValue("vatsimCID", cid);
                }
                else
                {
                    Logger.Info("REMEMBER ME: FALSE. REGISTRY CLEARED.");
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
                Logger.Info("Goodbye");
                LogManager.Shutdown();
                fsuipc.CloseConnection();
                System.Windows.Forms.Application.Exit();
            }
        }
        private void Setup()
        {
            retrieveButton.Enabled = true;
            Logger.Info("Setup Complete.");
        }
        private async Task PeriodicCheckMessage(TimeSpan interval, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Logger.Debug("Attempting to poll Hoppie for new messages");

                await SendCPDLCMessage("NONE", "poll", "");
                await Task.Delay(interval, cancellationToken);
                if(useFSUIPC && fsConnectionOpen)
                {
                    try
                    {
                        await fsuipc.RefreshData();
                        fsuipcErrorCount = 1;
                    }
                    catch (FSUIPCException)
                    {
                        if (fsuipcErrorCount <= 3)
                        {
                            try
                            {
                                fsConnectionOpen = fsuipc.OpenConnection();
                            }
                            catch { }
                            WriteMessage(String.Format("UNABLE TO CHECK FLIGHT SIM DATA. RETRYING (ATTEMPT {0} OF 3)", fsuipcErrorCount), "SYSTEM", "SYSTEM");
                            fsuipcErrorCount += 1;
                        }
                        else
                        {
                            WriteMessage("FLIGHT SIM DATA RETRIEVAL FAILED 3 TIMES CONSECUTIVELY. DISCONNECTING FROM FLIGHT SIM", "SYSTEM", "SYSTEM");
                            fsConnectionOpen = fsuipc.CloseConnection();
                            fsuipcErrorCount = 1;
                        }

                    }
                }
                
                
            }
        }
        public async Task SendCPDLCMessage(string recipient, string messageType, string packetData, bool _outbound = true, bool _write = true)
        {
            var connectionValues = new Dictionary<string, string> {
                {"logon", logonCode},
                {"from", callsign},
                {"to", recipient},
                {"type", messageType},
                {"packet", packetData}
            };

            var content = new FormUrlEncodedContent(connectionValues);
            try
            {
                var response = await webclient.PostAsync("http://www.hoppie.nl/acars/system/connect.html", content);
                
                Logger.Debug(String.Format("PACKET SENT: {0} | {1} | {2} | {3} | {4}", recipient, messageType, packetData, _outbound, _write));
                var responseString = await response.Content.ReadAsStringAsync();
                string printString = responseString.ToString().ToUpper();

                Logger.Debug("RECEIVED: " + responseString);

                if (_write)
                {
                    if (messageType == "CPDLC" && _outbound)
                    {
                        WriteMessage(packetData.Split('/').Last(), messageType, recipient, _outbound);
                    }
                    else if (messageType != "poll")
                    {
                        WriteMessage(packetData, messageType, recipient, _outbound);
                    }
                }

                if (printString.Contains("ERROR"))
                {
                    //handle error here
                }
                if (printString != "OK")
                {
                    await TelexParser(printString);
                }
            }

            catch (Exception e)
            {
                Logger.Error(String.Format("{0}: {1}", e.GetType().FullName, e.Message));
                WriteMessage("ERROR CHECKING FOR NEW MESSAGES. RETRYING...", "SYSTEM", "SYSTEM");
            }

            return;

        }
        private CPDLCMessage CreateCPDLCMessage(string _contents, string _type, string _recipient, bool _outbound = false, CPDLCResponse _header = null)
        {
            CPDLCMessage _message = new CPDLCMessage(_type, _recipient, _contents, _outbound, _header)
            {
                AutoSize = true,
                BackColor = controlBackColor,
                ForeColor = SystemColors.ControlDark,
                Font = textFont,
                Text = _type == "SYSTEM" ? "SYSTEM MESSAGE" : _outbound ? String.Format("{1} MESSAGE => {0}", _recipient, _type.ToUpper()) : String.Format("{1} MESSAGE <= {0}", _recipient, _type.ToUpper()),
                BorderStyle = BorderStyle.None
            };
            _message.Margin = new Padding(0, 3, 0, 0);

            return _message;
        }

        private System.Windows.Forms.Label CreateLabel(string _text, bool _useMaxSize = true)
        {
            Size maxSize = new Size
            {
                Width = 65
            };
            System.Windows.Forms.Label _message = new System.Windows.Forms.Label
            {
                Width = 65,
                AutoSize = true,
                BackColor = controlBackColor,
                ForeColor = SystemColors.ControlDark,
                Font = textFont,
                Text = _text,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(5, 3, 0, 0)
            };
            if (_useMaxSize)
            {
                _message.MaximumSize = maxSize;
            }
            return _message;
        }

        private System.Windows.Forms.Label CreateSpecialLabel(string _text, bool _useMaxSize = true)
        {
            Size maxSize = new Size
            {
                Width = 65
            };
            System.Windows.Forms.Label _message = new System.Windows.Forms.Label
            {
                Width = 65,
                AutoSize = true,
                BackColor = controlBackColor,
                ForeColor = SystemColors.ControlLight,
                Font = textFont,
                Text = _text,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(5, 3, 0, 0)
            };
            if (_useMaxSize)
            {
                _message.MaximumSize = maxSize;
            }
            return _message;
        }

        private TimerLabel CreateTimerLabel(string _text, bool _useMaxSize = true)
        {
            Size maxSize = new Size
            {
                Width = 65
            };
            TimerLabel _message = new TimerLabel
            {
                Width = 65,
                AutoSize = true,
                BackColor = controlBackColor,
                ForeColor = SystemColors.ControlLight,
                Font = textFontBold,
                Text = _text,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(10, 0, 0, 0)
            };
            if (_useMaxSize)
            {
                _message.MaximumSize = maxSize;
            }
            return _message;
        }
        private void DeleteElement(object sender, EventArgs e, CPDLCMessage control)
        {
            TableLayoutHelper.RemoveArbitraryRow(outputTable, outputTable.GetPositionFromControl(control).Row);
            ClearPreview();
        }

        private void DeleteAllElement(object sender, EventArgs e)
        {
            outputTable.Controls.Clear();
        }

        private void MessageClicked(object sender, EventArgs e)
        {
            int messageIndex = outputTable.Controls.GetChildIndex((System.Windows.Forms.Label)sender) - 1;
            CPDLCMessage _sender = (CPDLCMessage)outputTable.Controls[messageIndex];
            previewMessage = _sender;
            System.Windows.Forms.Label _timeStamp = (System.Windows.Forms.Label)outputTable.Controls[messageIndex - 1];
            List<System.Windows.Forms.Label> responses = new List<System.Windows.Forms.Label>();


            if (_sender.type == "CPDLC" && !_sender.outbound && !_sender.acknowledged)
            {
                if (_sender.message.Contains("CLR TO") || _sender.message.Contains("CLRD TO") || _sender.message.Contains("CLEARED TO"))
                {
                    System.Windows.Forms.Label acceptLabel = CreateSpecialLabel("> ACCEPT", false);
                    System.Windows.Forms.Label rejectLabel = CreateSpecialLabel("> REJECT", false);
                    switch (_sender.header.responses)
                    {
                        case "WU":
                            acceptLabel.Click += (lSender, le) => WilcoMessage(_sender, e, previewMessage);
                            rejectLabel.Click += (lSender, le) => UnableMessage(_sender, e, previewMessage);
                            break;

                        case "AN":
                            acceptLabel.Click += (lSender, le) => AffirmativeMessage(_sender, e, previewMessage);
                            rejectLabel.Click += (lSender, le) => NegativeMessage(_sender, e, previewMessage);
                            break;

                        case "R":
                            acceptLabel.Click += (lSender, le) => RogerMessage(_sender, e, previewMessage);
                            rejectLabel.Click += (lSender, le) => UnableMessage(_sender, e, previewMessage);
                            break;


                    }

                    responses.Add(acceptLabel);
                    responses.Add(rejectLabel);
                    responses.Add(standbyLabel);
                }

                else
                {
                    switch (_sender.header.responses)
                    {
                        case "WU":
                            responses.Add(wilcoLabel);
                            responses.Add(unableLabel);
                            responses.Add(standbyLabel);
                            break;

                        case "AN":
                            responses.Add(affirmativeLabel);
                            responses.Add(negativeLabel);
                            responses.Add(standbyLabel);
                            break;

                        case "R":
                            responses.Add(rogerLabel);
                            responses.Add(standbyLabel);
                            break;
                    }
                }

                responses.Add(freeTextLabel);
            }
            else if (_sender.type == "TELEX" && !_sender.outbound)
            {
                responses.Add(freeTextLabel);
            }
            
            messageFormatPanel.Size = outputTable.Size;
            messageFormatPanel.Visible = true;
            messageFormatPanel.Controls.Add(returnLabel);
            messageFormatPanel.SetFlowBreak(returnLabel, true);
            foreach(string line in _sender.message.Split('\n'))
            {
                messageFormatPanel.Controls.Add(CreateLabel(line, false));
                messageFormatPanel.SetFlowBreak(messageFormatPanel.Controls[messageFormatPanel.Controls.Count - 1], true);
            }
            foreach (System.Windows.Forms.Label _response in responses)
            {
                messageFormatPanel.Controls.Add(_response);
            }
            if(_sender.type != "CPDLC")
            {
                messageFormatPanel.Controls.Add(deleteLabel);
            }
            

        }
        private Task ADSCParser(string _response, string _sender)
        {
            string[] responseElements = _response.Split(' ');
            try
            {
                Convert.ToInt32(responseElements[2]);
            }
            catch
            {
                return Task.CompletedTask;
            }
            Contract _contract;
            switch(responseElements[1])
            {
                case "PERIODIC":
                    _contract = new Contract(this, _sender, responseElements[2]);
                    contracts.Add(_contract);
                    _contract.StartContract();

                    break;

                case "EVENTS":
                    break;

                case "CANCEL":
                    _contract = contracts.Where(x => x.sender == _sender && x.contractLength == responseElements[2]).FirstOrDefault();
                    if(!_contract.Equals(default(Contract)))
                    {
                        _contract.StopContract();
                        contracts.Remove(_contract);
                    }

                    break;
            }

            return Task.CompletedTask;
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

                for (int i = 0; i < _modify.Length; i++)
                {
                    if (i > 0 && _modify[i].Length > 2)
                    {
                        if (_modify[1].StartsWith("/DATA2/"))
                        {
                            Logger.Debug("CPDLC Message identified, attempting to parse");
                            await CPDLCParser(_modify[1], sender);
                            break;
                        }

                        if (type == "ADS-C")
                        {
                            if (useFSUIPC)
                            {
                                Logger.Debug("ADS-C Message identified, attempting to parse");
                                await ADSCParser(_modify[1], sender);
                                break;
                            }
                            else
                            {
                                Logger.Debug("ADS-C Message identified, but no simulator connection was recognised. Ignoring.");
                            }
                        }

                        format_response += _modify[1];
                        WriteMessage(format_response, type, sender);
                        FlashWindow.Flash(this);
                    }
                }
            }
            return;
        }

        private async Task CPDLCParser(string _response, string _sender)
        {
            bool _showUser = true;
            string messageString;

            var unit = cpdlcUnitParse.Match(_response);
            if (unit.Success)
            {
                currentATCUnit = unit.Value.Trim('_', '@');
            }

            var responses = cpdlcHeaderParse.Matches(_response);
            CPDLCResponse header = new CPDLCResponse
            {
                dataType = responses[0].Value.Trim('/'),
                messageID = Convert.ToInt32(responses[1].Value.Trim('/')),
                responseID = responses[2].Value.Trim('/').Length < 1 ? 0 : Convert.ToInt32(responses[2].Value.Trim('/')),
                responses = responses[3].Value.Trim('/')
            };

            string[] messageContent = _response.Split(new string[] { header.responses + "/" }, StringSplitOptions.None);
            if (messageContent[1].Contains(callsign))
            {
                messageString = messageContent[1].Split(new string[] { callsign }, StringSplitOptions.None).Last();
            }
            else
            {
                messageString = messageContent[1];
            }
            if (messageString.StartsWith("HANDOVER"))
            {
                string nextATCUnit = messageString.Split(' ').Last().Trim('@').Trim();
                currentATCUnit = null;
                await SendCPDLCMessage(nextATCUnit, "CPDLC", String.Format("/data2/{0}//Y/REQUEST LOGON", messageOutCounter), true, false);
                pendingLogon = nextATCUnit;
                messageOutCounter += 1;
                _showUser = false;
            }
            else if (messageString.StartsWith("LOGON ACCEPTED"))
            {
                currentATCUnit = pendingLogon;
                WriteMessage("CURRENT ATS UNIT: " + pendingLogon, "CPDLC", _sender, false, header);
                _showUser = false;
            }
            else if (messageString.StartsWith("CURRENT ATS UNIT"))
            {
                _showUser = false;
            }

            string message = callsign + " " + messageString.Replace("@@", "N/A").Replace("@", Environment.NewLine).Replace("_", "");
            message = Regex.Replace(message, @"\s+", " ");

            Logger.Debug(message);

            if (message.Contains("LOGOFF"))
            {
                currentATCUnit = null;
                pendingLogon = null;
            }

            if(_showUser)
            {
                WriteMessage(message, "CPDLC", _sender, false, header);

                FlashWindow.Flash(this);
            }

            return;
        }

        public void WriteMessage(string _response, string _type, string _recipient, bool _outbound = false, CPDLCResponse _header = null)
        {
            CPDLCMessage message;
            if (_outbound)
            {
                message = CreateCPDLCMessage(_response, _type, _recipient, _outbound, _header);
            }
            else
            {
                message = CreateCPDLCMessage(_response, _type, _recipient, _outbound, _header);
                if (playSound && _recipient != "SYSTEM") { player.Play(); }
            }

            Logger.Debug("Writing message: " + _response);

            TimerLabel menuLabel = CreateTimerLabel(">>", true);
            if (_type == "CPDLC" && !_outbound && _header.responses != "NE")
            {
                menuLabel.canFlash = true;
            }
            menuLabel.Click += MessageClicked;

            outputTable.Invoke(new Action(() => outputTable.Controls.Add(CreateLabel(DateTime.Now.ToString("HH:mm")), 0, outputTable.RowCount - 1)));
            outputTable.Invoke(new Action(() => outputTable.Controls.Add(message, 1, outputTable.RowCount - 1)));
            outputTable.Invoke(new Action(() => outputTable.Controls.Add(menuLabel, 2, outputTable.RowCount - 1)));
            outputTable.Invoke(new Action(() => outputTable.RowCount += 1));
            outputTable.Invoke(new Action(() => outputTable.RowStyles.Add(new RowStyle(SizeType.AutoSize))));
            outputTable.Invoke(new Action(() => outputTable.ScrollControlIntoView(message)));
        }

        public async void ArtificialDelay(string _message, string _type, string _sender, int _minDelay = 5, int _maxDelay = 15)
        {
            await Task.Delay(random.Next(_minDelay, _maxDelay) * 1000);
            await SendCPDLCMessage(_sender, _type, _message, true, false);
            return;
        }
        private void exitButton_Click(object sender, EventArgs e)
        {
            try
            {
                requestCancellationTokenSource.Cancel();
            }
            catch (NullReferenceException) { }
            this.Close();

            LogManager.Shutdown();
            fsuipc.CloseConnection();
            System.Windows.Forms.Application.Exit();
        }
        private void MoveWindow(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        private async void retrieveButton_Click(object sender, EventArgs e)
        {
            string response = "";

            if (!connected)
            {
                try
                {

                    using (WebClient wc = new WebClient())
                    {
                        vatsimData = JsonConvert.DeserializeObject<VATSIMRootobject>(wc.DownloadString("https://data.vatsim.net/v3/vatsim-data.json"));
                        Logger.Debug("VATSIM Data Retrieved and Parsed");

                    }

                    userVATSIMData = vatsimData.pilots.Where(i => i.cid == cid).FirstOrDefault();

                    string _fpTest = userVATSIMData.flight_plan.altitude;

                    response += "VATSIM: OK" + Environment.NewLine;
                    callsign = userVATSIMData.callsign;

                    connected = true;

                    requestCancellationTokenSource = new CancellationTokenSource();
                    requestCancellationToken = requestCancellationTokenSource.Token;
                    _ = PeriodicCheckMessage(updateTimer, requestCancellationToken);                

                }

                catch
                {
                    response += "VATSIM: ERROR.\n";
                    atcButton.Enabled = false;
                    telexButton.Enabled = false;
                    connected = false;

                    return;
                }

                try
                {

                    using (WebClient wc = new WebClient())
                    {
                        var simbriefjson = wc.DownloadString(String.Format("https://www.simbrief.com/api/xml.fetcher.php?userid={0}&json=1", simbriefID));
                        var simbriefNavlog = JObject.Parse(simbriefjson)["navlog"].ToString();
                        simbriefData = JsonConvert.DeserializeObject<Navlog>(simbriefNavlog);

                        Logger.Debug("Simbrief Data Retrieved and Parsed");

                        reportFixes = simbriefData.fix.Where(x => x.is_sid_star == "0" && !new string[] { "apt" }.Contains(x.type)).Select(x => x.ident).ToArray();
                        response += "SIMBRIEF: OK\n";
                    }
                }

                catch
                {
                    response += "SIMBRIEF: ERROR\n";
                }

                if(useFSUIPC)
                {
                    try
                    {
                        fsConnectionOpen = fsuipc.OpenConnection();
                        if (fsConnectionOpen)
                        {
                            response += "SIMULATOR: OK\n";
                        }
                        else
                        {
                            throw new FSUIPCException(FSUIPCError.FSUIPC_ERR_NOTOPEN, "CONNECTION FAILED");
                        }


                    }
                    catch
                    {
                        response += "SIMULATOR: ERROR\n";
                    }
                }
                response += "LOGON SUCCESSFUL.";
                WriteMessage(response, "SYSTEM", "SYSTEM");

            }
            else
            {
                if (!(currentATCUnit is null))
                {
                    await SendCPDLCMessage(currentATCUnit, "CPDLC", String.Format("/data2/{0}//N/LOGOFF", messageOutCounter), true, false);
                }
                foreach(Contract _contract in contracts)
                {
                    await SendCPDLCMessage(_contract.sender, "ADS-C", "REJECT " + _contract.contractLength, true, false);
                }
                requestCancellationTokenSource.Cancel();
                callsign = "";
                response = "DISCONNECTED CLIENT";
                vatsimData = new VATSIMRootobject();
                userVATSIMData = new Pilot();
                simbriefData = new Navlog();
                fsConnectionOpen = fsuipc.CloseConnection();

                atcButton.Enabled = false;
                telexButton.Enabled = false;
                connected = false;

                WriteMessage(response, "SYSTEM", "SYSTEM");

            }
        }
        private void telexButton_Click(object sender, EventArgs e)
        {
            tForm = new TelexForm(this);
            tForm.Show();
        }
        private void requestButton_Click(object sender, EventArgs e)
        {
            rForm = new RequestForm(this)
            {
                TopMost = stayOnTop
            };
            rForm.Show();
        }

        private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.MainWindowLocation = Location;
            Properties.Settings.Default.MainWindowSize = Size;
            Properties.Settings.Default.Save();

            if (!(currentATCUnit is null))
            {
                await SendCPDLCMessage(currentATCUnit, "CPDLC", String.Format("/data2/{0}//N/LOGOFF", messageOutCounter), true, false);
                requestCancellationTokenSource.Cancel();
            }
        }

        private void outputTable_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            TableLayoutPanel _sender = (TableLayoutPanel)sender;

            if (me.Button == MouseButtons.Right)
            {
                popupMenu.Items.Clear();
                popupMenu.Items.Add(deleteAllMenu);

                popupMenu.Show(_sender, _sender.PointToClient(Cursor.Position));
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

        private void settingsButton_Click(object sender, EventArgs e)
        {
            sForm = new SettingsForm(this)
            {
                TopMost = stayOnTop
            };
            sForm.Show();
        }

        private void helpButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/josh-seagrave/EasyCPDLC/wiki");
            MessageBox.Show(
                @"EasyCPDLC
Copyright(C) 2022 Joshua Seagrave

This program is free software: you can redistribute it and / or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.If not, see <https://www.gnu.org/licenses/>.", String.Format("EasyCPDLC v{0} Licensing & Copyright Notice", System.Windows.Forms.Application.ProductVersion), MessageBoxButtons.OK);
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

            panel.AutoScroll = false;
            panel.AutoScroll = true;
        }
    }
}
