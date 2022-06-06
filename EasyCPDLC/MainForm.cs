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
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public Pilot userVATSIMData;
        private VATSIMRootobject vatsimData;
        private Navlog simbriefData;
        public string[] reportFixes;
        public string nextFix = null;

        public FSUIPCData fsuipc = new();
        public bool fsConnectionOpen = false;
        public int fsuipcErrorCount = 1;

        public Random random = new();

        readonly private List<Contract> contracts = new();

        private static readonly HttpClient webclient = new();
        private string logonCode;
        private int cid;
        public string callsign;

        private RequestForm rForm;
        private TelexForm tForm;
        private SettingsForm sForm;

        public bool StayOnTop
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

        public static bool PlaySound
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

        public static bool UseFSUIPC
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

        public static int SavedCID
        {
            get
            {
                return Properties.Settings.Default.CID;
            }
            set
            {
                Properties.Settings.Default.CID = value;

            }
        }

        public static string SavedHoppieCode
        {
            get
            {
                return Properties.Settings.Default.HoppieCode;
            }
            set
            {
                Properties.Settings.Default.HoppieCode = value;

            }
        }

        public static string SimbriefID
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
        public bool Connected
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
        public string CurrentATCUnit
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
                        rForm.NeedsLogon = true;

                    }
                    else
                    {
                        atcUnitDisplay.Text = _currentATCUnit;
                        rForm.NeedsLogon = false;

                    }
                }
                catch (NullReferenceException)
                {

                }
            }
        }

        public Font controlFont = new("Oxygen", 10.0f, FontStyle.Regular);
        public Font controlFontBold = new("Oxygen", 10.0f, FontStyle.Bold);
        public Font textFont = new("B612 Mono", 10.0f, FontStyle.Regular);
        public Font textFontBold = new("B612 Mono", 12.5f, FontStyle.Bold);
        public Font dataEntryFont = new("B612 Mono", 11.0f, FontStyle.Regular);
        public Color controlBackColor = Color.FromArgb(5, 5, 5);
        public Color controlFrontColor = SystemColors.ControlLight;

        private readonly ContextMenuStrip popupMenu = new();
        ToolStripMenuItem deleteAllMenu;

        private AccessibleLabel wilcoLabel;
        private AccessibleLabel rogerLabel;
        private AccessibleLabel affirmativeLabel;
        private AccessibleLabel negativeLabel;
        private AccessibleLabel standbyLabel;
        private AccessibleLabel unableLabel;
        private AccessibleLabel deleteLabel;
        private AccessibleLabel freeTextLabel;
        private AccessibleLabel returnLabel;

        private AccessibleLabel[] replyOptionsList;

        private CPDLCMessage previewMessage;

        private readonly SoundPlayer player = new();

        private static readonly Regex hoppieParse = new(@"{(.*?)}");
        private static readonly Regex cpdlcHeaderParse = new(@"(\/\s*)\w*");
        private static readonly Regex cpdlcUnitParse = new(@"_@([\w]*)@_");

        private static readonly TimeSpan updateTimer = TimeSpan.FromSeconds(20);
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
            this.TopMost = StayOnTop;
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            CurrentATCUnit = null;
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

        private static async void CheckNewVersion()
        {
            try
            {
                var client = new GitHubClient(new ProductHeaderValue("EasyCPDLC"));
                var releases = await client.Repository.Release.GetAll("josh-seagrave", "EasyCPDLC");
                var latest = releases[0];
                string latestVersion = latest.TagName.Replace("cpdlc", "");
                if (latestVersion != System.Windows.Forms.Application.ProductVersion)
                {
                    DialogResult updateBox = MessageBox.Show(String.Format("New Version {0} Available to download from Github. Would you like me to take you to the Github page for the latest release?", latestVersion), "New Version Available", MessageBoxButtons.YesNo);
                    if (updateBox == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(latest.HtmlUrl);
                    }
                }
            }

            catch { }
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
            ToolStripMenuItem _temp = new(name)
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
            rogerLabel.Click += (_sender, e) => ReplyMessage(e, previewMessage, "ROGER");
            rogerLabel.KeyDown += (_sender, e) => ReplyMessage(e, previewMessage, "ROGER");

            wilcoLabel = CreateSpecialLabel("> WILCO", false);
            wilcoLabel.Click += (_sender, e) => ReplyMessage(e, previewMessage, "WILCO");
            wilcoLabel.KeyDown += (_sender, e) => ReplyMessage(e, previewMessage, "WILCO");

            standbyLabel = CreateSpecialLabel("> STANDBY", false);
            standbyLabel.Click += (_sender, e) => ReplyMessage(e, previewMessage, "STANDBY");
            standbyLabel.KeyDown += (_sender, e) => ReplyMessage(e, previewMessage, "STANDBY");

            unableLabel = CreateSpecialLabel("> UNABLE", false);
            unableLabel.Click += (_sender, e) => ReplyMessage(e, previewMessage, "UNABLE");
            unableLabel.KeyDown += (_sender, e) => ReplyMessage(e, previewMessage, "UNABLE");

            affirmativeLabel = CreateSpecialLabel("> AFFIRMATIVE", false);
            affirmativeLabel.Click += (_sender, e) => ReplyMessage(e, previewMessage, "AFFIRMATIVE");
            affirmativeLabel.KeyDown += (_sender, e) => ReplyMessage(e, previewMessage, "AFFIRMATIVE");

            negativeLabel = CreateSpecialLabel("> NEGATIVE", false);
            negativeLabel.Click += (_sender, e) => ReplyMessage(e, previewMessage, "NEGATIVE");
            negativeLabel.KeyDown += (_sender, e) => ReplyMessage(e, previewMessage, "NEGATIVE");

            freeTextLabel = CreateSpecialLabel("> FREE TEXT", false);
            freeTextLabel.Click += (_sender, e) => FreeTextMessage(previewMessage);
            freeTextLabel.KeyDown += (_sender, e) => FreeTextMessage(previewMessage);

            deleteLabel = CreateSpecialLabel("> DELETE", false);
            deleteLabel.Click += (_sender, e) => DeleteElement(e, previewMessage);
            deleteLabel.KeyDown += (_sender, e) => DeleteElement(e, previewMessage);

            returnLabel = CreateSpecialLabel("< RETURN", false);
            returnLabel.Click += ReturnMessage;
            returnLabel.KeyDown += ReturnMessage;

            deleteAllMenu = CreateMenuItem("DELETE ALL");
            deleteAllMenu.Click += DeleteAllElement;

            replyOptionsList = new AccessibleLabel[]
            {
                wilcoLabel, rogerLabel, unableLabel, affirmativeLabel, negativeLabel, standbyLabel, freeTextLabel
            };

            Logger.Info("Login menu initialised");
        }

        private void FreeTextMessage(CPDLCMessage message)
        {

            tForm = new TelexForm(this, message.recipient)
            {
                TopMost = StayOnTop
            };
            tForm.Show();
        }

        public void ClearPreview()
        {
            messageFormatPanel.Controls.Clear();
            outputTable.Visible = true;
            messageFormatPanel.Visible = false;

        }

        private void ReturnMessage(object sender, EventArgs e)
        {
            try
            {
                KeyEventArgs kE = (KeyEventArgs)e;
                if(kE.KeyCode == Keys.Enter || kE.KeyCode == Keys.Space)
                {
                    throw new Exception();
                }
                else
                {
                    return;
                }
            }
            catch
            {
                ClearPreview();
            }
        }
        private void ReplyMessage(EventArgs e, CPDLCMessage message, string reply)
        {
            try
            {
                KeyEventArgs kE = (KeyEventArgs)e;
                if (kE.KeyCode == Keys.Enter || kE.KeyCode == Keys.Space)
                {
                    throw new Exception();
                }
                else
                {
                    return;
                }
            }
            catch
            {
                foreach (AccessibleLabel _label in replyOptionsList)
                {
                    _label.Enabled = false;
                }

                message.header.ResponseID = messageOutCounter;

                if (reply != "STANDBY")
                {
                    message.acknowledged = true;
                    int index = outputTable.Controls.GetChildIndex(message);
                    ((TimerLabel)outputTable.Controls[index + 1]).CanFlash = false;
                    outputTable.Controls[index + 1].ForeColor = controlFrontColor;
                    message.ForeColor = SystemColors.ControlDark;
                }
                
                _ = Task.Run(() => SendCPDLCMessage(message.recipient, message.type, String.Format("/data2/{0}/{1}/N/{2}", message.header.ResponseID, message.header.MessageID, reply)));
                messageOutCounter += 1;
                ClearPreview();

                foreach (AccessibleLabel _label in replyOptionsList)
                {
                    _label.Enabled = true;
                }
            }
        }
        private void ShowSetupForm()
        {

            Logger.Info("Login Form Displayed");

            DataEntry dataEntry = new(SavedHoppieCode == String.Empty ? null : SavedHoppieCode, SavedCID == new int() ? null : global::EasyCPDLC.MainForm.SavedCID);

            if (dataEntry.ShowDialog(this) == DialogResult.OK)
            {
                logonCode = dataEntry.HoppieLogonCode;
                cid = dataEntry.VatsimCID;
                if (dataEntry.Remember)
                {
                    Logger.Info("REMEMBER ME: TRUE. REGISTRY SET.");
                    SavedHoppieCode = logonCode;
                    SavedCID = cid;
                }
                else
                {
                    SavedCID = new int();
                    SavedHoppieCode = String.Empty;
                }
            }
            else
            {
                Logger.Info("Goodbye");
                LogManager.Shutdown();
                FSUIPCData.CloseConnection();
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
                if(UseFSUIPC && fsConnectionOpen)
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
                                fsConnectionOpen = FSUIPCData.OpenConnection();
                            }
                            catch { }
                            WriteMessage(String.Format("UNABLE TO CHECK FLIGHT SIM DATA. RETRYING (ATTEMPT {0} OF 3)", fsuipcErrorCount), "SYSTEM", "SYSTEM");
                            fsuipcErrorCount += 1;
                        }
                        else
                        {
                            WriteMessage("FLIGHT SIM DATA RETRIEVAL FAILED 3 TIMES CONSECUTIVELY. DISCONNECTING FROM FLIGHT SIM", "SYSTEM", "SYSTEM");
                            fsConnectionOpen = FSUIPCData.CloseConnection();
                            fsuipcErrorCount = 1;
                        }

                    }
                }
                
                
            }
        }
        public async Task SendCPDLCMessage(string recipient, string messageType, string packetData, bool _write = true)
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

                if (_write && messageType != "poll")
                {
                    SendingProgress.Invoke(() => SendingProgress.Visible = true);
                    SendingProgress.Invoke(() => SendingProgress.Value = 0);
                    SendingProgress.Invoke(() => SendingProgress.PerformStep());
                }

                var response = await webclient.PostAsync("http://www.hoppie.nl/acars/system/connect.html", content);

                SendingProgress.Invoke(() => SendingProgress.PerformStep());

                Logger.Debug(String.Format("PACKET SENT: {0} | {1} | {2} | {3} | {4}", recipient, messageType, packetData, true, _write));
                var responseString = await response.Content.ReadAsStringAsync();
                string printString = responseString.ToString().ToUpper().Trim();

                Logger.Debug("RECEIVED: " + responseString);

                if (printString.Contains("ERROR"))
                {
                    //handle error here
                }
                else
                {
                    SendingProgress.Invoke(() => SendingProgress.PerformStep());

                    if (_write && messageType != "poll")
                    {
                        WriteMessage(messageType == "CPDLC" ? packetData.Split('/').Last() : packetData, messageType, recipient, true);
                    }
                }

                if (printString != "OK")
                {
                    await TelexParser(printString);
                }

                SendingProgress.Invoke(() => SendingProgress.Visible = false);
            }

            catch (Exception e)
            {
                Logger.Error(String.Format("{0}: {1}", e.GetType().FullName, e.Message));
                WriteMessage("ERROR CHECKING FOR NEW MESSAGES. RETRYING...", "SYSTEM", "SYSTEM");

                SendingProgress.Invoke(() => SendingProgress.Visible = false);
            }

            return;

        }
        private CPDLCMessage CreateCPDLCMessage(string _contents, string _type, string _recipient, bool _outbound = false, CPDLCResponse _header = null)
        {
            CPDLCMessage _message = new(_type, _recipient, _contents, _outbound, _header)
            {
                AutoSize = true,
                BackColor = controlBackColor,
                ForeColor = SystemColors.ControlDark,
                Font = textFont,
                Text = _type == "SYSTEM" ? "SYSTEM MESSAGE" : _outbound ? String.Format("{1} MESSAGE TO {0}", _recipient, _type.ToUpper()) : String.Format("{1} MESSAGE FROM {0}", _recipient, _type.ToUpper()),
                BorderStyle = BorderStyle.None,
                TabStop = true,
                TabIndex = 0
            };
            _message.Margin = new Padding(0, 3, 0, 0);

            return _message;
        }

        private AccessibleLabel CreateLabel(string _text, bool _useMaxSize = true)
        {
            Size maxSize = new()
            {
                Width = 65
            };
            AccessibleLabel _message = new(SystemColors.ControlDark)
            {
                Width = 65,
                AutoSize = true,
                BackColor = controlBackColor,
                ForeColor = SystemColors.ControlDark,
                Font = textFont,
                Text = _text,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(5, 3, 0, 0),
                TabStop = true,
                TabIndex = 0
            };
            if (_useMaxSize)
            {
                _message.MaximumSize = maxSize;
            }
            SetStyle(ControlStyles.Selectable, true);
            return _message;
        }

        private AccessibleLabel CreateSpecialLabel(string _text, bool _useMaxSize = true)
        {
            Size maxSize = new()
            {
                Width = 65
            };
            AccessibleLabel _message = new(SystemColors.ControlLight)
            {
                Width = 65,
                AutoSize = true,
                BackColor = controlBackColor,
                ForeColor = SystemColors.ControlLight,
                Font = textFont,
                Text = _text,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(5, 3, 0, 0),
                TabStop = true,
                TabIndex = 0,
            };
            if (_useMaxSize)
            {
                _message.MaximumSize = maxSize;
            }
            return _message;
        }

        private TimerLabel CreateTimerLabel(string _text, bool _useMaxSize = true)
        {
            Size maxSize = new()
            {
                Width = 65
            };
            TimerLabel _message = new()
            {
                Width = 65,
                AutoSize = true,
                BackColor = controlBackColor,
                ForeColor = SystemColors.ControlLight,
                Font = textFontBold,
                Text = _text,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(10, 0, 0, 0),
            };
            if (_useMaxSize)
            {
                _message.MaximumSize = maxSize;
            }
            return _message;
        }
        private void DeleteElement(EventArgs e, CPDLCMessage control)
        {
            try
            {
                KeyEventArgs kE = (KeyEventArgs)e;
                if (kE.KeyCode == Keys.Enter || kE.KeyCode == Keys.Space)
                {
                    throw new Exception();
                }
                else
                {
                    return;
                }
            }
            catch
            {
                TableLayoutHelper.RemoveArbitraryRow(outputTable, outputTable.GetPositionFromControl(control).Row);
                ClearPreview();
            }
        }

        private void DeleteAllElement(object sender, EventArgs e)
        {
            outputTable.Controls.Clear();
        }

        private void MessageClicked(object sender, EventArgs e)
        {
            int messageIndex = outputTable.Controls.GetChildIndex((System.Windows.Forms.Label)sender) - 1;
            try
            {
                KeyEventArgs kE = (KeyEventArgs)e;
                if (kE.KeyCode == Keys.Enter || kE.KeyCode == Keys.Space)
                {
                    messageIndex++;
                    throw new Exception();
                }
                else
                {
                    return;
                }
            }
            catch
            {
                CPDLCMessage _sender = (CPDLCMessage)outputTable.Controls[messageIndex];
                previewMessage = _sender;
                System.Windows.Forms.Label _timeStamp = (System.Windows.Forms.Label)outputTable.Controls[messageIndex - 1];
                List<System.Windows.Forms.Label> responses = new();


                if (_sender.type == "CPDLC" && !_sender.outbound && !_sender.acknowledged)
                {
                    if (_sender.message.Contains("CLR TO") || _sender.message.Contains("CLRD TO") || _sender.message.Contains("CLEARED TO"))
                    {
                        AccessibleLabel acceptLabel = CreateSpecialLabel("> ACCEPT", false);
                        AccessibleLabel rejectLabel = CreateSpecialLabel("> REJECT", false);
                        switch (_sender.header.Responses)
                        {
                            case "WU":
                                acceptLabel.Click += (lSender, le) => ReplyMessage(e, previewMessage, "WILCO");
                                acceptLabel.KeyDown += (lSender, le) => ReplyMessage(e, previewMessage, "WILCO");
                                rejectLabel.Click += (lSender, le) => ReplyMessage(e, previewMessage, "UNABLE");
                                rejectLabel.KeyDown += (lSender, le) => ReplyMessage(e, previewMessage, "UNABLE");
                                break;

                            case "AN":
                                acceptLabel.Click += (lSender, le) => ReplyMessage(e, previewMessage, "AFFIRMATIVE");
                                acceptLabel.KeyDown += (lSender, le) => ReplyMessage(e, previewMessage, "AFFIRMATIVE");
                                rejectLabel.Click += (lSender, le) => ReplyMessage(e, previewMessage, "NEGATIVE");
                                rejectLabel.KeyDown += (lSender, le) => ReplyMessage(e, previewMessage, "NEGATIVE");
                                break;

                            case "R":
                                acceptLabel.Click += (lSender, le) => ReplyMessage(e, previewMessage, "ROGER");
                                acceptLabel.KeyDown += (lSender, le) => ReplyMessage(e, previewMessage, "ROGER");
                                rejectLabel.Click += (lSender, le) => ReplyMessage(e, previewMessage, "UNABLE");
                                rejectLabel.KeyDown += (lSender, le) => ReplyMessage(e, previewMessage, "UNABLE");
                                break;


                        }

                        responses.Add(acceptLabel);
                        responses.Add(rejectLabel);
                        responses.Add(standbyLabel);
                    }

                    else
                    {
                        switch (_sender.header.Responses)
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
                outputTable.Visible = false;
                messageFormatPanel.Controls.Add(returnLabel);
                messageFormatPanel.SetFlowBreak(returnLabel, true);
                foreach (string line in _sender.message.Split('\n'))
                {
                    messageFormatPanel.Controls.Add(CreateLabel(line, false));
                    messageFormatPanel.SetFlowBreak(messageFormatPanel.Controls[messageFormatPanel.Controls.Count - 1], true);
                }
                foreach (System.Windows.Forms.Label _response in responses)
                {
                    messageFormatPanel.Controls.Add(_response);
                }
                if (_sender.type != "CPDLC")
                {
                    messageFormatPanel.Controls.Add(deleteLabel);
                }
                messageFormatPanel.Controls[1].Focus();
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
                            if (UseFSUIPC)
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
                CurrentATCUnit = unit.Value.Trim('_', '@');
            }

            var responses = cpdlcHeaderParse.Matches(_response);
            CPDLCResponse header = new()
            {
                DataType = responses[0].Value.Trim('/'),
                MessageID = Convert.ToInt32(responses[1].Value.Trim('/')),
                ResponseID = responses[2].Value.Trim('/').Length < 1 ? 0 : Convert.ToInt32(responses[2].Value.Trim('/')),
                Responses = responses[3].Value.Trim('/')
            };

            string[] messageContent = _response.Split(new string[] { header.Responses + "/" }, StringSplitOptions.None);
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
                CurrentATCUnit = null;
                await SendCPDLCMessage(nextATCUnit, "CPDLC", String.Format("/data2/{0}//Y/REQUEST LOGON", messageOutCounter), false);
                pendingLogon = nextATCUnit;
                messageOutCounter += 1;
                _showUser = false;
            }
            else if (messageString.StartsWith("LOGON ACCEPTED"))
            {
                CurrentATCUnit = pendingLogon;
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
                CurrentATCUnit = null;
                pendingLogon = null;
            }

            if(_showUser)
            {
                WriteMessage(message, "CPDLC", _sender, false, header);

                FlashWindow.Flash(this);
            }

            return;
        }

        public CPDLCMessage WriteMessage(string _response, string _type, string _recipient, bool _outbound = false, CPDLCResponse _header = null)
        {
            CPDLCMessage message;
            if (_outbound)
            {
                message = CreateCPDLCMessage(_response, _type, _recipient, _outbound, _header);
            }
            else
            {
                message = CreateCPDLCMessage(_response, _type, _recipient, _outbound, _header);
                if (PlaySound && _recipient != "SYSTEM") { player.Play(); }
            }

            Logger.Debug("Writing message: " + _response);

            TimerLabel menuLabel = CreateTimerLabel(">>", true);
            if (_type == "CPDLC" && !_outbound && _header.Responses != "NE")
            {
                menuLabel.CanFlash = true;
            }
            menuLabel.Click += MessageClicked;
            message.KeyDown += MessageClicked;

            outputTable.Invoke(new Action(() => outputTable.Controls.Add(CreateLabel(DateTime.Now.ToString("HH:mm")), 0, outputTable.RowCount - 1)));
            outputTable.Invoke(new Action(() => outputTable.Controls.Add(message, 1, outputTable.RowCount - 1)));
            outputTable.Invoke(new Action(() => outputTable.Controls.Add(menuLabel, 2, outputTable.RowCount - 1)));
            outputTable.Invoke(new Action(() => outputTable.RowCount += 1));
            outputTable.Invoke(new Action(() => outputTable.RowStyles.Add(new RowStyle(SizeType.AutoSize))));
            outputTable.Invoke(new Action(() => outputTable.ScrollControlIntoView(message)));

            return message;
        }

        public async void ArtificialDelay(string _message, string _type, string _sender, int _minDelay = 5, int _maxDelay = 15)
        {
            await Task.Delay(random.Next(_minDelay, _maxDelay) * 1000);
            await SendCPDLCMessage(_sender, _type, _message, false);
            return;
        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
            try
            {
                requestCancellationTokenSource.Cancel();
            }
            catch (NullReferenceException) { }
            this.Close();

            LogManager.Shutdown();
            FSUIPCData.CloseConnection();
            System.Windows.Forms.Application.Exit();
        }
        private void MoveWindow(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                _ = SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        private async void RetrieveButton_Click(object sender, EventArgs e)
        {
            string response = "";

            if (!Connected)
            {
                try
                {
                    using (HttpClient wc = new())
                    {
                        vatsimData = JsonConvert.DeserializeObject<VATSIMRootobject>(wc.GetStringAsync("https://data.vatsim.net/v3/vatsim-data.json").Result);
                        Logger.Debug("VATSIM Data Retrieved and Parsed");

                    }

                    userVATSIMData = vatsimData.pilots.Where(i => i.cid == cid).FirstOrDefault();
                    if(userVATSIMData is null)
                    {
                        throw new IndexOutOfRangeException();
                    }

                    string _fpTest = userVATSIMData.flight_plan.altitude;
                    callsign = userVATSIMData.callsign;

                    Connected = true;

                    requestCancellationTokenSource = new CancellationTokenSource();
                    requestCancellationToken = requestCancellationTokenSource.Token;
                    _ = PeriodicCheckMessage(updateTimer, requestCancellationToken);                

                }
                catch (IndexOutOfRangeException)
                {
                    response += "VATSIM: ERROR. WAIT 60 SECONDS AND RETRY.\n";
                    atcButton.Enabled = false;
                    telexButton.Enabled = false;
                    Connected = false;
                    WriteMessage(response, "SYSTEM", "SYSTEM");
                    return;
                }

                catch (NullReferenceException)
                {
                    response += "FLIGHT PLAN: ERROR. WAIT 60 SECONDS AND RETRY.\n";
                    atcButton.Enabled = false;
                    telexButton.Enabled = false;
                    Connected = false;
                    WriteMessage(response, "SYSTEM", "SYSTEM");
                    return;
                }

                response += "LOGON SUCCESSFUL.";

                try
                {

                    using HttpClient wc = new();
                    var simbriefjson = wc.GetStringAsync(String.Format("https://www.simbrief.com/api/xml.fetcher.php?userid={0}&json=1", SimbriefID)).Result;
                    var simbriefNavlog = JObject.Parse(simbriefjson)["navlog"].ToString();
                    simbriefData = JsonConvert.DeserializeObject<Navlog>(simbriefNavlog);

                    Logger.Debug("Simbrief Data Retrieved and Parsed");

                    reportFixes = simbriefData.fix.Where(x => x.is_sid_star == "0" && !new string[] { "apt" }.Contains(x.type)).Select(x => x.ident).ToArray();
                    response += " SIMBRIEF OK,";
                }

                catch
                {
                    response += "SIMBRIEF ERROR,";
                }

                if (UseFSUIPC)
                {
                    try
                    {
                        fsConnectionOpen = FSUIPCData.OpenConnection();
                        if (fsConnectionOpen)
                        {
                            response += "SIMULATOR OK.";
                        }
                        else
                        {
                            throw new FSUIPCException(FSUIPCError.FSUIPC_ERR_NOTOPEN, "CONNECTION FAILED");
                        }
                    }
                    catch
                    {
                        response += "SIMULATOR ERROR.";
                    }
                }
                WriteMessage(response, "SYSTEM", "SYSTEM");

            }
            else
            {
                if (CurrentATCUnit is not null)
                {
                    await SendCPDLCMessage(CurrentATCUnit, "CPDLC", String.Format("/data2/{0}//N/LOGOFF", messageOutCounter), false);
                }
                foreach(Contract _contract in contracts)
                {
                    await SendCPDLCMessage(_contract.sender, "ADS-C", "REJECT " + _contract.contractLength, false);
                }
                requestCancellationTokenSource.Cancel();
                callsign = "";
                response = "DISCONNECTED CLIENT";
                vatsimData = new VATSIMRootobject();
                userVATSIMData = new Pilot();
                simbriefData = new Navlog();
                fsConnectionOpen = FSUIPCData.CloseConnection();

                atcButton.Enabled = false;
                telexButton.Enabled = false;
                Connected = false;

                WriteMessage(response, "SYSTEM", "SYSTEM");

            }
        }
        private void TelexButton_Click(object sender, EventArgs e)
        {
            tForm = new TelexForm(this);
            tForm.Show();
        }
        private void RequestButton_Click(object sender, EventArgs e)
        {
            rForm = new RequestForm(this)
            {
                TopMost = StayOnTop
            };
            rForm.Show();
        }

        private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.MainWindowLocation = Location;
            Properties.Settings.Default.MainWindowSize = Size;
            Properties.Settings.Default.Save();

            if (CurrentATCUnit is not null)
            {
                await SendCPDLCMessage(CurrentATCUnit, "CPDLC", String.Format("/data2/{0}//N/LOGOFF", messageOutCounter), false);
                requestCancellationTokenSource.Cancel();
            }
        }

        private void OutputTable_Click(object sender, EventArgs e)
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

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            sForm = new SettingsForm(this)
            {
                TopMost = StayOnTop
            };
            sForm.Show();
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
            //worst bodge I've ever had to pull, thanks to this: github.com/dotnet/runtime/issues/17938
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("https://github.com/josh-seagrave/EasyCPDLC/wiki"){ UseShellExecute = true });
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
