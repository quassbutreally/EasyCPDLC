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
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Media;
using System.Net.Http;
using System.Text.Json;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FSUIPC;
using System.ComponentModel;

namespace EasyCPDLC;

public sealed partial class MainForm
{

    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public Pilot userVatsimData;
    private VatsimRootobject vatsimData;
    private Navlog simbriefData;
    public string[] reportFixes;
    public string nextFix = null;

    public readonly FsuipcData fsuipc = new();
    private bool fsConnectionOpen;
    private int fsuipcErrorCount = 1;

    private bool isErrorState;

    private readonly Random random = new();

    private readonly List<Contract> contracts = [];

    private static readonly HttpClient Webclient = new();
    private string logonCode;
    private int cid;
    public string callsign;

    private RequestForm rForm;
    private TelexForm tForm;
    private SettingsForm sForm;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool StayOnTop
    {
        get => Properties.Settings.Default.StayOnTop;
        set
        {
            Properties.Settings.Default.StayOnTop = value;
            this.TopMost = value;
        }
    }

    public static bool PlaySound
    {
        get => Properties.Settings.Default.PlayAudibleAlert;
        set => Properties.Settings.Default.PlayAudibleAlert = value;
    }

    public static bool UseFsuipc
    {
        get => Properties.Settings.Default.UseFSUIPC;
        set => Properties.Settings.Default.UseFSUIPC = value;
    }

    private static int SavedCid
    {
        get => Properties.Settings.Default.CID;
        set => Properties.Settings.Default.CID = value;
    }

    private static string SavedHoppieCode
    {
        get => Properties.Settings.Default.HoppieCode;
        set => Properties.Settings.Default.HoppieCode = value;
    }

    public static string SimbriefId
    {
        get => Properties.Settings.Default.SimbriefUsername;
        set => Properties.Settings.Default.SimbriefUsername = value;
    }

    public int messageOutCounter = 1;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    private bool Connected
    {
        get;
        set
        {
            field = value;
            if (field)
            {
                retrieveButton.Text = @"DISCONNECT";
                atcButton.Enabled = true;
                telexButton.Enabled = true;
            }
            else
            {
                retrieveButton.Text = @"CONNECT";
                atcButton.Enabled = false;
                telexButton.Enabled = false;
            }
        }
    }

    public string pendingLogon;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string CurrentAtcUnit
    {
        get;
        set
        {
            field = value;
            try
            {
                if (field is null)
                {
                    atcUnitDisplay.Text = @"----";
                    rForm.NeedsLogon = true;
                }
                else
                {
                    atcUnitDisplay.Text = field;
                    rForm.NeedsLogon = false;
                }
            }
            catch (NullReferenceException)
            {
            }
        }
    }

    [System.Runtime.InteropServices.DllImport("gdi32.dll")]
    private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
        IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);


    private readonly byte[][] fontResources = [
        Properties.Resources.B612Mono_Bold,
        Properties.Resources.B612Mono_Regular,
        Properties.Resources.Oxygen_Regular,
        Properties.Resources.Oxygen_Bold
    ];
        
    public static readonly PrivateFontCollection Fonts = new();

    private readonly Font controlFont;// = new("Oxygen", 10.0f, FontStyle.Regular);
    public readonly Font textFont;// = new("B612 Mono", 10.0f, FontStyle.Regular);
    public readonly Font textFontBold;// = new("B612 Mono", 12.5f, FontStyle.Bold);
    public Color controlBackColor = Color.FromArgb(5, 5, 5);
    public Color controlFrontColor = SystemColors.ControlLight;

    private readonly ContextMenuStrip popupMenu = new();
    private ToolStripMenuItem deleteAllMenu;

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

    private CpdlcMessage previewMessage;

    private readonly SoundPlayer player = new();

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private static readonly Regex HoppieParse = HoppieRegex();
    private static readonly Regex CpdlcHeaderParse = CpdlcHeaderRegex();
    private static readonly Regex CpdlcUnitParse = CpdlcUnitRegex();

    private static readonly TimeSpan UpdateTimer = TimeSpan.FromSeconds(20);
    private CancellationTokenSource requestCancellationTokenSource;
    private CancellationToken requestCancellationToken;
        
    public MainForm()
    {
        var config = new NLog.Config.LoggingConfiguration();
        var logFile = new NLog.Targets.FileTarget("logfile") { FileName = "EasyCPDLCLog.txt" };
        config.AddRule(LogLevel.Debug, LogLevel.Fatal, logFile);
        LogManager.Configuration = config;
        Logger.Info("Logging initialised, beginning setup");

        foreach(byte[] fontData in fontResources)
        {
            var fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
            System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            uint dummy = 0;
            Fonts.AddMemoryFont(fontPtr, fontData.Length);
            AddFontMemResourceEx(fontPtr, (uint)fontData.Length, IntPtr.Zero, ref dummy);
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontPtr);
        }

        textFont = new Font(Fonts.Families[0], 10.0f, FontStyle.Regular);
        textFontBold = new Font(Fonts.Families[0], 12.5f, FontStyle.Bold);
        controlFont = new Font(Fonts.Families[1], 10.0f, FontStyle.Regular);

        AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

        var customSoundFile = System.Windows.Forms.Application.StartupPath + @"\Sounds\Notification.wav";
        if(File.Exists(customSoundFile))
        {
            player.SoundLocation = customSoundFile;
        }
        else
        {
            player.Stream = Properties.Resources.notification;
        }
        player.Play();
        InitializeComponent();
        TopMost = StayOnTop;
        FormBorderStyle = FormBorderStyle.None;
        DoubleBuffered = true;
        SetStyle(ControlStyles.ResizeRedraw, true);
        CurrentAtcUnit = null;
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
            var latestVersion = latest.TagName.Replace("cpdlc", "");
            if (latestVersion == System.Windows.Forms.Application.ProductVersion) return;
            var updateBox = MessageBox.Show(
                $@"New Version {latestVersion} Available to download from Github. Would you like me to take you to the Github page for the latest release?", @"New Version Available", MessageBoxButtons.YesNo);
            if (updateBox == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(latest.HtmlUrl);
            }
        }

        catch
        {
            // ignored
        }
    }

    private ToolStripMenuItem CreateMenuItem(string name)
    {
        ToolStripMenuItem temp = new(name)
        {
            BackColor = controlBackColor,
            ForeColor = controlFrontColor,
            Font = controlFont,
            TextAlign = ContentAlignment.TopLeft
        };

        return temp;
    }
    private void InitialisePopupMenu()
    {
        popupMenu.BackColor = controlBackColor;
        popupMenu.ForeColor = controlFrontColor;
        popupMenu.Font = controlFont;
        popupMenu.ShowImageMargin = false;

        rogerLabel = CreateSpecialLabel("> ROGER", false);
        rogerLabel.Click += (_, e) => ReplyMessage(e, previewMessage, "ROGER");
        rogerLabel.KeyDown += (_, e) => ReplyMessage(e, previewMessage, "ROGER");

        wilcoLabel = CreateSpecialLabel("> WILCO", false);
        wilcoLabel.Click += (_, e) => ReplyMessage(e, previewMessage, "WILCO");
        wilcoLabel.KeyDown += (_, e) => ReplyMessage(e, previewMessage, "WILCO");

        standbyLabel = CreateSpecialLabel("> STANDBY", false);
        standbyLabel.Click += (_, e) => ReplyMessage(e, previewMessage, "STANDBY");
        standbyLabel.KeyDown += (_, e) => ReplyMessage(e, previewMessage, "STANDBY");

        unableLabel = CreateSpecialLabel("> UNABLE", false);
        unableLabel.Click += (_, e) => ReplyMessage(e, previewMessage, "UNABLE");
        unableLabel.KeyDown += (_, e) => ReplyMessage(e, previewMessage, "UNABLE");

        affirmativeLabel = CreateSpecialLabel("> AFFIRMATIVE", false);
        affirmativeLabel.Click += (_, e) => ReplyMessage(e, previewMessage, "AFFIRMATIVE");
        affirmativeLabel.KeyDown += (_, e) => ReplyMessage(e, previewMessage, "AFFIRMATIVE");

        negativeLabel = CreateSpecialLabel("> NEGATIVE", false);
        negativeLabel.Click += (_, e) => ReplyMessage(e, previewMessage, "NEGATIVE");
        negativeLabel.KeyDown += (_, e) => ReplyMessage(e, previewMessage, "NEGATIVE");

        freeTextLabel = CreateSpecialLabel("> FREE TEXT", false);
        freeTextLabel.Click += (_, _) => FreeTextMessage(previewMessage);
        freeTextLabel.KeyDown += (_, _) => FreeTextMessage(previewMessage);

        deleteLabel = CreateSpecialLabel("> DELETE", false);
        deleteLabel.Click += (_, e) => DeleteElement(e, previewMessage);
        deleteLabel.KeyDown += (_, e) => DeleteElement(e, previewMessage);

        returnLabel = CreateSpecialLabel("< RETURN", false);
        returnLabel.Click += ReturnMessage;
        returnLabel.KeyDown += ReturnMessage;

        deleteAllMenu = CreateMenuItem("DELETE ALL");
        deleteAllMenu.Click += DeleteAllElement;

        replyOptionsList =
        [
            wilcoLabel, 
            rogerLabel, 
            unableLabel, 
            affirmativeLabel,
            negativeLabel, 
            standbyLabel, 
            freeTextLabel
        ];

        Logger.Info("Login menu initialised");
    }

    private void FreeTextMessage(CpdlcMessage message)
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
        if (e is MouseEventArgs || (e is KeyEventArgs kE && (kE.KeyCode == Keys.Enter || kE.KeyCode == Keys.Space)))
        {
            ClearPreview();
        }
    }
    private void ReplyMessage(EventArgs e, CpdlcMessage message, string reply)
    {
        if (!(e is MouseEventArgs || (e is KeyEventArgs kE && (kE.KeyCode == Keys.Enter || kE.KeyCode == Keys.Space)))) return;

        foreach (var label in replyOptionsList)
        {
            label.Enabled = false;
        }

        message.header.ResponseId = messageOutCounter;

        if (reply != "STANDBY")
        {
            message.acknowledged = true;
            int index = outputTable.Controls.GetChildIndex(message);
            ((TimerLabel)outputTable.Controls[index + 1]).CanFlash = false;
            outputTable.Controls[index + 1].ForeColor = controlFrontColor;
            message.ForeColor = SystemColors.ControlDark;
        }
            
        _ = Task.Run(() => SendCpdlcMessage(message.recipient, message.type,
            $"/data2/{message.header.ResponseId}/{message.header.MessageId}/N/{reply}"), requestCancellationToken);
        messageOutCounter += 1;
        ClearPreview();

        foreach (var label in replyOptionsList)
        {
            label.Enabled = true;
        }
    }
    private void ShowSetupForm()
    {

        Logger.Info("Login Form Displayed");

        DataEntry dataEntry = new(SavedHoppieCode == String.Empty ? null : SavedHoppieCode, SavedCid == 0 ? null : SavedCid);

        if (dataEntry.ShowDialog(this) == DialogResult.OK)
        {
            logonCode = dataEntry.HoppieLogonCode;
            cid = dataEntry.VatsimCid;
            if (dataEntry.Remember)
            {
                Logger.Info("REMEMBER ME: TRUE. REGISTRY SET.");
                SavedHoppieCode = logonCode;
                SavedCid = cid;
            }
            else
            {
                SavedCid = 0;
                SavedHoppieCode = string.Empty;
            }
        }
        else
        {
            Logger.Info("Goodbye");
            LogManager.Shutdown();
            FsuipcData.CloseConnection();
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

            await SendCpdlcMessage("NONE", "poll", "");
            await Task.Delay(interval, cancellationToken);
            if(UseFsuipc && fsConnectionOpen)
            {
                try
                {
                    fsuipc.RefreshData();
                    fsuipcErrorCount = 1;
                }
                catch (FSUIPCException)
                {
                    if (fsuipcErrorCount <= 3)
                    {
                        try
                        {
                            fsConnectionOpen = FsuipcData.OpenConnection();
                        }
                        catch
                        {
                            // ignored
                        }

                        WriteMessage(
                            $@"UNABLE TO CHECK FLIGHT SIM DATA. RETRYING (ATTEMPT {fsuipcErrorCount} OF 3)", "SYSTEM", "SYSTEM");
                        fsuipcErrorCount += 1;
                    }
                    else
                    {
                        WriteMessage("FLIGHT SIM DATA RETRIEVAL FAILED 3 TIMES CONSECUTIVELY. DISCONNECTING FROM FLIGHT SIM", "SYSTEM", "SYSTEM");
                        fsConnectionOpen = FsuipcData.CloseConnection();
                        fsuipcErrorCount = 1;
                    }

                }
            }
                
                
        }
    }
    public async Task SendCpdlcMessage(string recipient, string messageType, string packetData, bool write = true)
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

            if (write && messageType != "poll")
            {
                SendingProgress.Invoke(() => SendingProgress.Visible = true);
                SendingProgress.Invoke(() => SendingProgress.Value = 0);
                await SendingProgress.InvokeAsync(() => SendingProgress.PerformStep(), requestCancellationToken);
            }

            var response = await Webclient.PostAsync("https://www.hoppie.nl/acars/system/connect.html", content, requestCancellationToken);

            await SendingProgress.InvokeAsync(() => SendingProgress.PerformStep(), requestCancellationToken);

            Logger.Debug($"PACKET SENT: {recipient} | {messageType} | {packetData} | {true} | {write}");
            var responseString = await response.Content.ReadAsStringAsync(requestCancellationToken);
            var printString = responseString.ToUpper().Trim();
            Logger.Debug("RECEIVED: " + responseString);

            if (printString.Contains("ERROR"))
            {
                throw new HttpRequestException();
            }
            else
            {
                if(isErrorState)
                {
                    WriteMessage("HOPPIE CONNECTIVITY RESTORED.", "SYSTEM", "SYSTEM");
                    isErrorState = false;
                }
                    
                await SendingProgress.InvokeAsync(() => SendingProgress.PerformStep(), requestCancellationToken);

                if (write && messageType != "poll")
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
            if(!isErrorState)
            {
                Logger.Error($"{e.GetType().FullName}: {e.Message}");
                WriteMessage("ERROR CHECKING FOR NEW MESSAGES. THIS IS LIKELY AN ERROR WITH THE HOPPIE NETWORK. THE SYSTEM WILL CONTINUE ATTEMPTING TO CONTACT THE SERVER AND LET YOU KNOW WHEN CONNECTION IS RE-ESTABLISHED.", "SYSTEM", "SYSTEM");
                isErrorState = true;
            }
            SendingProgress.Invoke(() => SendingProgress.Visible = false);
        }
    }
    private CpdlcMessage CreateCpdlcMessage(string contents, string type, string recipient, bool outbound = false, CpdlcResponse header = null)
    {
        CpdlcMessage message = new(type, recipient, contents, outbound, header)
        {
            AutoSize = true,
            BackColor = controlBackColor,
            ForeColor = SystemColors.ControlDark,
            Font = textFont,
            Text = type == "SYSTEM" ? "SYSTEM MESSAGE" : outbound ? string.Format("{1} MESSAGE TO {0}", recipient, type.ToUpper()) : string.Format("{1} MESSAGE FROM {0}", recipient, type.ToUpper()),
            BorderStyle = BorderStyle.None,
            TabStop = true,
            TabIndex = 0,
            Margin = new Padding(0, 3, 0, 0)
        };

        return message;
    }

    private AccessibleLabel CreateLabel(string text, bool useMaxSize = true)
    {
        Size maxSize = new()
        {
            Width = 65
        };
        AccessibleLabel message = new(SystemColors.ControlDark)
        {
            Width = 65,
            AutoSize = true,
            BackColor = controlBackColor,
            ForeColor = SystemColors.ControlDark,
            Font = textFont,
            Text = text,
            BorderStyle = BorderStyle.None,
            Margin = new Padding(5, 3, 0, 0),
            TabStop = true,
            TabIndex = 0
        };
        if (useMaxSize)
        {
            message.MaximumSize = maxSize;
        }
        SetStyle(ControlStyles.Selectable, true);
        return message;
    }

    private AccessibleLabel CreateSpecialLabel(string text, bool useMaxSize = true)
    {
        Size maxSize = new()
        {
            Width = 65
        };
        AccessibleLabel message = new(SystemColors.ControlLight)
        {
            Width = 65,
            AutoSize = true,
            BackColor = controlBackColor,
            ForeColor = SystemColors.ControlLight,
            Font = textFont,
            Text = text,
            BorderStyle = BorderStyle.None,
            Margin = new Padding(5, 3, 0, 0),
            TabStop = true,
            TabIndex = 0,
        };
        if (useMaxSize)
        {
            message.MaximumSize = maxSize;
        }
        return message;
    }

    private TimerLabel CreateTimerLabel(string text, bool useMaxSize = true)
    {
        Size maxSize = new()
        {
            Width = 65
        };
        TimerLabel message = new()
        {
            Width = 65,
            AutoSize = true,
            BackColor = controlBackColor,
            ForeColor = SystemColors.ControlLight,
            Font = textFontBold,
            Text = text,
            BorderStyle = BorderStyle.None,
            Margin = new Padding(10, 0, 0, 0),
        };
        if (useMaxSize)
        {
            message.MaximumSize = maxSize;
        }
        return message;
    }
    private void DeleteElement(EventArgs e, CpdlcMessage control)
    {
        if (e is MouseEventArgs || (e is KeyEventArgs kE && (kE.KeyCode == Keys.Enter || kE.KeyCode == Keys.Space)))
        {
            TableLayoutHelper.RemoveArbitraryRow(outputTable, outputTable.GetPositionFromControl(control).Row);
            ClearPreview();
        }
    }

    private void DeleteAllElement(object sender, EventArgs e)
    {
        outputTable.Controls.Clear();
    }

    private void MessageClicked(object s, EventArgs e)
    {
        var messageIndex = outputTable.Controls.GetChildIndex((System.Windows.Forms.Label)s) - 1;
        if (e is KeyEventArgs kE)
        {
            if (kE.KeyCode != Keys.Enter && kE.KeyCode != Keys.Space) return;
            messageIndex++;
        }

        var sender = (CpdlcMessage)outputTable.Controls[messageIndex];
        previewMessage = sender;
        List<System.Windows.Forms.Label> responses = [];


            switch (sender.type)
            {
                case "CPDLC" when !sender.outbound && !sender.acknowledged:
                {
                    if (sender.contents.Contains("CLR TO") || sender.contents.Contains("CLRD TO") || sender.contents.Contains("CLEARED TO"))
                    {
                        var acceptLabel = CreateSpecialLabel("> ACCEPT", false);
                        var rejectLabel = CreateSpecialLabel("> REJECT", false);
                        switch (sender.header.Responses)
                        {
                            case "WU":
                                acceptLabel.Click += (_, _) => ReplyMessage(e, previewMessage, "WILCO");
                                acceptLabel.KeyDown += (_, _) => ReplyMessage(e, previewMessage, "WILCO");
                                rejectLabel.Click += (_, _) => ReplyMessage(e, previewMessage, "UNABLE");
                                rejectLabel.KeyDown += (_, _) => ReplyMessage(e, previewMessage, "UNABLE");
                                break;

                            case "AN":
                                acceptLabel.Click += (_, _) => ReplyMessage(e, previewMessage, "AFFIRMATIVE");
                                acceptLabel.KeyDown += (_, _) => ReplyMessage(e, previewMessage, "AFFIRMATIVE");
                                rejectLabel.Click += (_, _) => ReplyMessage(e, previewMessage, "NEGATIVE");
                                rejectLabel.KeyDown += (_, _) => ReplyMessage(e, previewMessage, "NEGATIVE");
                                break;

                            case "R":
                                acceptLabel.Click += (_, _) => ReplyMessage(e, previewMessage, "ROGER");
                                acceptLabel.KeyDown += (_, _) => ReplyMessage(e, previewMessage, "ROGER");
                                rejectLabel.Click += (_, _) => ReplyMessage(e, previewMessage, "UNABLE");
                                rejectLabel.KeyDown += (_, _) => ReplyMessage(e, previewMessage, "UNABLE");
                                break;


                        }

                        responses.Add(acceptLabel);
                        responses.Add(rejectLabel);
                        responses.Add(standbyLabel);
                    }

                    else
                    {
                        switch (sender.header.Responses)
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
                    break;
                }
                case "TELEX" when !sender.outbound:
                    responses.Add(freeTextLabel);
                    break;
            }

            messageFormatPanel.Size = outputTable.Size;
            messageFormatPanel.Visible = true;
            outputTable.Visible = false;
            messageFormatPanel.Controls.Add(returnLabel);
            messageFormatPanel.SetFlowBreak(returnLabel, true);
            foreach (var line in sender.contents.Split('\n'))
            {
                messageFormatPanel.Controls.Add(CreateLabel(line, false));
                messageFormatPanel.SetFlowBreak(messageFormatPanel.Controls[^1], true);
            }
            foreach (var response in responses)
            {
                messageFormatPanel.Controls.Add(response);
            }
            if (sender.type != "CPDLC")
            {
                messageFormatPanel.Controls.Add(deleteLabel);
            }
            messageFormatPanel.Controls[1].Focus();
    }
    private Task AdscParser(string response, string sender)
    {
        var responseElements = response.Split(' ');
        if (!int.TryParse(responseElements[2], out _))
        {
            return Task.CompletedTask;
        }
            
        Contract contract;
        switch(responseElements[1])
        {
            case "PERIODIC":
                contract = new Contract(
                    sender,
                    responseElements[2],
                    (to, type, msg) => SendCpdlcMessage(to, type, msg, false),
                    () => fsuipc,
                    () => callsign);
                contracts.Add(contract);
                contract.StartContract();

                break;

            case "CANCEL":
                contract = contracts.FirstOrDefault(x => x.Sender == sender && x.ContractLength == responseElements[2]);
                if  (contract != null)
                {
                    contract.StopContract();
                    contracts.Remove(contract);
                }

                break;
        }

        return Task.CompletedTask;
    }

    private async Task TelexParser(string telex)
    {
        var responses = HoppieParse.Matches(telex);

        foreach (Match response in responses)
        {
            var formatResponse = "";
            var modify = response.Groups[1].Value.Replace("}", "").Split('{');
            var sender = modify[0].Split(' ')[0];
            var type = modify[0].Split(' ')[1];

            for (var i = 0; i < modify.Length; i++)
            {
                if (i <= 0 || modify[i].Length <= 2) continue;
                if (modify[1].StartsWith("/DATA2/"))
                {
                    Logger.Debug("CPDLC Message identified, attempting to parse");
                    await CpdlcParser(modify[1], sender);
                    break;
                }

                if (type == "ADS-C")
                {
                    if (UseFsuipc)
                    {
                        Logger.Debug("ADS-C Message identified, attempting to parse");
                        await AdscParser(modify[1], sender);
                        break;
                    }
                    else
                    {
                        Logger.Debug("ADS-C Message identified, but no simulator connection was recognised. Ignoring.");
                    }
                }

                formatResponse += modify[1];
                WriteMessage(formatResponse, type, sender);
                FlashWindow.Flash(this);
            }
        }
    }

    private async Task CpdlcParser(string response, string sender)
    {
        var showUser = true;

        var unit = CpdlcUnitParse.Match(response);
        if (unit.Success)
        {
            CurrentAtcUnit = unit.Value.Trim('_', '@');
        }

        var responses = CpdlcHeaderParse.Matches(response);
        CpdlcResponse header = new()
        {
            MessageId = Convert.ToInt32(responses[1].Value.Trim('/')),
            ResponseId = responses[2].Value.Trim('/').Length < 1 ? 0 : Convert.ToInt32(responses[2].Value.Trim('/')),
            Responses = responses[3].Value.Trim('/')
        };

        var messageContent = response.Split([header.Responses + "/"], StringSplitOptions.None);
        var messageString = messageContent[1].Contains(callsign) ? messageContent[1].Split([callsign], StringSplitOptions.None).Last() : messageContent[1];
        if (messageString.StartsWith("HANDOVER"))
        {
            var nextAtcUnit = messageString.Split(' ').Last().Trim('@').Trim();
            CurrentAtcUnit = null;
            await SendCpdlcMessage(nextAtcUnit, "CPDLC", $"/data2/{messageOutCounter}//Y/REQUEST LOGON", false);
            pendingLogon = nextAtcUnit;
            messageOutCounter += 1;
            showUser = false;
        }
        else if (messageString.StartsWith("LOGON ACCEPTED"))
        {
            CurrentAtcUnit = pendingLogon;
            WriteMessage("CURRENT ATS UNIT: " + pendingLogon, "CPDLC", sender, false, header);
            showUser = false;
        }
        else if (messageString.StartsWith("CURRENT ATC UNIT") || messageString.StartsWith("CURRENT ATS UNIT"))
        {
            showUser = false;
        }

        var message = callsign + " " + messageString.Replace("@@", "N/A").Replace("@", Environment.NewLine).Replace("_", "");
        message = Regex.Replace(message, @"\s+", " ");

        Logger.Debug(message);

        if (message.Contains("LOGOFF"))
        {
            CurrentAtcUnit = null;
            pendingLogon = null;
        }

        if(showUser)
        {
            WriteMessage(message, "CPDLC", sender, false, header);

            FlashWindow.Flash(this);
        }
    }

    public void WriteMessage(string response, string type, string recipient, bool outbound = false,
        CpdlcResponse header = null)
    {
        CpdlcMessage message;
        if (outbound)
        {
            message = CreateCpdlcMessage(response, type, recipient, true, header);
        }
        else
        {
            message = CreateCpdlcMessage(response, type, recipient, false, header);
            if (PlaySound && recipient != "SYSTEM") { player.Play(); }
        }

        Logger.Debug("Writing message: " + response);

        var menuLabel = CreateTimerLabel(">>");
        if (type == "CPDLC" && !outbound && header is not { Responses: "NE" })
        {
            menuLabel.CanFlash = true;
        }
        menuLabel.Click += MessageClicked;
        message.KeyDown += MessageClicked;

        outputTable.Invoke(() => outputTable.Controls.Add(CreateLabel(DateTime.Now.ToString("HH:mm")), 0, outputTable.RowCount - 1));
        outputTable.Invoke(() => outputTable.Controls.Add(message, 1, outputTable.RowCount - 1));
        outputTable.Invoke(() => outputTable.Controls.Add(menuLabel, 2, outputTable.RowCount - 1));
        outputTable.Invoke(new Action(() => outputTable.RowCount += 1));
        outputTable.Invoke(new Action(() => outputTable.RowStyles.Add(new RowStyle(SizeType.AutoSize))));
        outputTable.Invoke(() => outputTable.ScrollControlIntoView(message));
    }

    public async void ArtificialDelay(string message, string type, string sender, int minDelay = 5, int maxDelay = 15)
    {
        try
        {
            await Task.Delay(random.Next(minDelay, maxDelay) * 1000, requestCancellationToken);
            await SendCpdlcMessage(sender, type, message, false);
        }
        catch (Exception)
        {
            // ignored
        }
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
        FsuipcData.CloseConnection();
        System.Windows.Forms.Application.Exit();
    }
    private void MoveWindow(object sender, MouseEventArgs e) => DragWindow(sender, e);
    private async void RetrieveButton_Click(object sender, EventArgs e)
    {
        try
        {
            string response = "";

            if (!Connected)
            {

                try
                {
                    using (HttpClient wc = new())
                    {
                        vatsimData = JsonSerializer.Deserialize<VatsimRootobject>(await wc.GetStringAsync("https://data.vatsim.net/v3/vatsim-data.json", requestCancellationToken), JsonOptions);
                        Logger.Debug("VATSIM Data Retrieved and Parsed");

                    }

                    userVatsimData = vatsimData.pilots.FirstOrDefault(i => i.cid == cid);
                    if(userVatsimData is null)
                    {
                        throw new IndexOutOfRangeException();
                    }
                    
                    callsign = userVatsimData.callsign;

                    Connected = true;

                    requestCancellationTokenSource = new CancellationTokenSource();
                    requestCancellationToken = requestCancellationTokenSource.Token;
                    _ = PeriodicCheckMessage(UpdateTimer, requestCancellationToken);                

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
                    var simbriefjson = wc.GetStringAsync(
                        $"https://www.simbrief.com/api/xml.fetcher.php?userid={SimbriefId}&json=1", requestCancellationToken).Result;
                    using var simbriefDoc = JsonDocument.Parse(simbriefjson);
                    var simbriefNavlog = simbriefDoc.RootElement.GetProperty("navlog").GetRawText();
                    simbriefData = JsonSerializer.Deserialize<Navlog>(simbriefNavlog, JsonOptions);

                    Logger.Debug("Simbrief Data Retrieved and Parsed");

                    reportFixes = simbriefData.fix.Where(x => x.is_sid_star == "0" && !new[] { "apt" }.Contains(x.type)).Select(x => x.ident).ToArray();
                    response += " SIMBRIEF OK,";
                }

                catch
                {
                    response += "SIMBRIEF ERROR,";
                }

                if (UseFsuipc)
                {
                    try
                    {
                        fsConnectionOpen = FsuipcData.OpenConnection();
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
                if (CurrentAtcUnit is not null)
                {
                    await SendCpdlcMessage(CurrentAtcUnit, "CPDLC", $@"/data2/{messageOutCounter}//N/LOGOFF", false);
                }
                foreach(var contract in contracts)
                {
                    await SendCpdlcMessage(contract.Sender, "ADS-C", "REJECT " + contract.ContractLength, false);
                }
                await requestCancellationTokenSource.CancelAsync();
                callsign = "";
                response = "DISCONNECTED CLIENT";
                vatsimData = new VatsimRootobject();
                userVatsimData = new Pilot();
                simbriefData = new Navlog();
                fsConnectionOpen = FsuipcData.CloseConnection();

                atcButton.Enabled = false;
                telexButton.Enabled = false;
                Connected = false;

                WriteMessage(response, "SYSTEM", "SYSTEM");

            }
        }
        catch (Exception)
        {
            // ignored
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
        try
        {
            Properties.Settings.Default.MainWindowLocation = Location;
            Properties.Settings.Default.MainWindowSize = Size;
            Properties.Settings.Default.Save();

            if (CurrentAtcUnit is null) return;
            await SendCpdlcMessage(CurrentAtcUnit, "CPDLC", $@"/data2/{messageOutCounter}//N/LOGOFF", false);
            await requestCancellationTokenSource.CancelAsync();
        }
        catch (Exception)
        {
            // ignored
        }
    }

    private void OutputTable_Click(object s, EventArgs e)
    {
        var me = (MouseEventArgs)e;

        var sender = (TableLayoutPanel)s;

        if (me.Button != MouseButtons.Right) return;
        popupMenu.Items.Clear();
        popupMenu.Items.Add(deleteAllMenu);

        popupMenu.Show(sender, sender.PointToClient(Cursor.Position));
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
            """
            EasyCPDLC
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
            along with this program.If not, see <https://www.gnu.org/licenses/>.
            """,
            $@"EasyCPDLC v{System.Windows.Forms.Application.ProductVersion} Licensing & Copyright Notice", MessageBoxButtons.OK);
    }

    [GeneratedRegex(@"{([\S\s]*?)}")]
    private static partial Regex HoppieRegex();
    [GeneratedRegex(@"(\/\s*)\w*")]
    private static partial Regex CpdlcHeaderRegex();
    [GeneratedRegex(@"_@([\w]*)@_")]
    private static partial Regex CpdlcUnitRegex();
}