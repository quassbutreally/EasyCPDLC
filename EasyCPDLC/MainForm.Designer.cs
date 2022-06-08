namespace EasyCPDLC
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.titleLabel = new System.Windows.Forms.Label();
            this.exitButton = new System.Windows.Forms.Button();
            this.outputTable = new System.Windows.Forms.TableLayoutPanel();
            this.atcButton = new System.Windows.Forms.Button();
            this.telexButton = new System.Windows.Forms.Button();
            this.retrieveButton = new System.Windows.Forms.Button();
            this.atcUnitLabel = new System.Windows.Forms.Label();
            this.atcUnitDisplay = new System.Windows.Forms.Label();
            this.helpButton = new System.Windows.Forms.Button();
            this.messageFormatPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.settingsButton = new System.Windows.Forms.Button();
            this.iconList = new System.Windows.Forms.ImageList(this.components);
            this.SendingProgress = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Oxygen", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.titleLabel.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.titleLabel.Location = new System.Drawing.Point(397, 6);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(186, 41);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "EasyCPDLC";
            this.titleLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveWindow);
            // 
            // exitButton
            // 
            this.exitButton.AccessibleName = "Exit";
            this.exitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exitButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.exitButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.exitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exitButton.Font = new System.Drawing.Font("Oxygen", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.exitButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.exitButton.Location = new System.Drawing.Point(617, 4);
            this.exitButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(28, 28);
            this.exitButton.TabIndex = 7;
            this.exitButton.Text = "X";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // outputTable
            // 
            this.outputTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outputTable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(5)))), ((int)(((byte)(5)))));
            this.outputTable.ColumnCount = 3;
            this.outputTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.outputTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 77.5F));
            this.outputTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.outputTable.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(5)))), ((int)(((byte)(5)))));
            this.outputTable.Location = new System.Drawing.Point(14, 76);
            this.outputTable.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.outputTable.Name = "outputTable";
            this.outputTable.Padding = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.outputTable.RowCount = 1;
            this.outputTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.outputTable.Size = new System.Drawing.Size(616, 147);
            this.outputTable.TabIndex = 3;
            this.outputTable.TabStop = true;
            this.outputTable.Click += new System.EventHandler(this.OutputTable_Click);
            // 
            // atcButton
            // 
            this.atcButton.Enabled = false;
            this.atcButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.atcButton.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.atcButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.atcButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.atcButton.Location = new System.Drawing.Point(284, 14);
            this.atcButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.atcButton.Name = "atcButton";
            this.atcButton.Size = new System.Drawing.Size(105, 55);
            this.atcButton.TabIndex = 2;
            this.atcButton.Text = "ATC";
            this.atcButton.UseVisualStyleBackColor = true;
            this.atcButton.Click += new System.EventHandler(this.RequestButton_Click);
            // 
            // telexButton
            // 
            this.telexButton.Enabled = false;
            this.telexButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.telexButton.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.telexButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.telexButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.telexButton.Location = new System.Drawing.Point(172, 14);
            this.telexButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.telexButton.Name = "telexButton";
            this.telexButton.Size = new System.Drawing.Size(105, 55);
            this.telexButton.TabIndex = 1;
            this.telexButton.Text = "TELEX";
            this.telexButton.UseVisualStyleBackColor = true;
            this.telexButton.Click += new System.EventHandler(this.TelexButton_Click);
            // 
            // retrieveButton
            // 
            this.retrieveButton.Enabled = false;
            this.retrieveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.retrieveButton.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.retrieveButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.retrieveButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.retrieveButton.Location = new System.Drawing.Point(14, 14);
            this.retrieveButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.retrieveButton.Name = "retrieveButton";
            this.retrieveButton.Size = new System.Drawing.Size(150, 55);
            this.retrieveButton.TabIndex = 0;
            this.retrieveButton.Text = "CONNECT";
            this.retrieveButton.UseVisualStyleBackColor = true;
            this.retrieveButton.Click += new System.EventHandler(this.RetrieveButton_Click);
            // 
            // atcUnitLabel
            // 
            this.atcUnitLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.atcUnitLabel.AutoSize = true;
            this.atcUnitLabel.Font = new System.Drawing.Font("Oxygen", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.atcUnitLabel.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.atcUnitLabel.Location = new System.Drawing.Point(403, 49);
            this.atcUnitLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.atcUnitLabel.Name = "atcUnitLabel";
            this.atcUnitLabel.Size = new System.Drawing.Size(134, 19);
            this.atcUnitLabel.TabIndex = 0;
            this.atcUnitLabel.Text = "Current ATS Unit: ";
            // 
            // atcUnitDisplay
            // 
            this.atcUnitDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.atcUnitDisplay.AutoSize = true;
            this.atcUnitDisplay.Font = new System.Drawing.Font("Oxygen", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.atcUnitDisplay.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.atcUnitDisplay.Location = new System.Drawing.Point(551, 49);
            this.atcUnitDisplay.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.atcUnitDisplay.Name = "atcUnitDisplay";
            this.atcUnitDisplay.Size = new System.Drawing.Size(29, 19);
            this.atcUnitDisplay.TabIndex = 0;
            this.atcUnitDisplay.Text = "----";
            // 
            // helpButton
            // 
            this.helpButton.AccessibleName = "About";
            this.helpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.helpButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.helpButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.helpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.helpButton.Font = new System.Drawing.Font("Oxygen", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.helpButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.helpButton.Location = new System.Drawing.Point(598, 1);
            this.helpButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(27, 31);
            this.helpButton.TabIndex = 6;
            this.helpButton.Text = "?";
            this.helpButton.UseVisualStyleBackColor = true;
            this.helpButton.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // messageFormatPanel
            // 
            this.messageFormatPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messageFormatPanel.AutoScroll = true;
            this.messageFormatPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(5)))), ((int)(((byte)(5)))));
            this.messageFormatPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.messageFormatPanel.Location = new System.Drawing.Point(14, 76);
            this.messageFormatPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.messageFormatPanel.Name = "messageFormatPanel";
            this.messageFormatPanel.Padding = new System.Windows.Forms.Padding(12, 0, 0, 35);
            this.messageFormatPanel.Size = new System.Drawing.Size(614, 146);
            this.messageFormatPanel.TabIndex = 4;
            this.messageFormatPanel.TabStop = true;
            this.messageFormatPanel.Visible = false;
            // 
            // settingsButton
            // 
            this.settingsButton.AccessibleName = "Settings";
            this.settingsButton.BackgroundImage = global::EasyCPDLC.Properties.Resources.cog_wheel_gear_setting;
            this.settingsButton.FlatAppearance.BorderSize = 0;
            this.settingsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.settingsButton.ImageIndex = 0;
            this.settingsButton.ImageList = this.iconList;
            this.settingsButton.Location = new System.Drawing.Point(576, 5);
            this.settingsButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(23, 23);
            this.settingsButton.TabIndex = 5;
            this.settingsButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.settingsButton.UseVisualStyleBackColor = true;
            this.settingsButton.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // iconList
            // 
            this.iconList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.iconList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("iconList.ImageStream")));
            this.iconList.TransparentColor = System.Drawing.Color.Transparent;
            this.iconList.Images.SetKeyName(0, "cog-wheel-gear-setting.png");
            // 
            // SendingProgress
            // 
            this.SendingProgress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.SendingProgress.Location = new System.Drawing.Point(14, 223);
            this.SendingProgress.MarqueeAnimationSpeed = 10;
            this.SendingProgress.Maximum = 30;
            this.SendingProgress.Name = "SendingProgress";
            this.SendingProgress.Size = new System.Drawing.Size(614, 2);
            this.SendingProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.SendingProgress.TabIndex = 8;
            this.SendingProgress.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(644, 233);
            this.Controls.Add(this.SendingProgress);
            this.Controls.Add(this.settingsButton);
            this.Controls.Add(this.atcUnitDisplay);
            this.Controls.Add(this.atcUnitLabel);
            this.Controls.Add(this.retrieveButton);
            this.Controls.Add(this.telexButton);
            this.Controls.Add(this.atcButton);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.messageFormatPanel);
            this.Controls.Add(this.outputTable);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MinimumSize = new System.Drawing.Size(644, 233);
            this.Name = "MainForm";
            this.Text = "9";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveWindow);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.TableLayoutPanel outputTable;
        private System.Windows.Forms.Button atcButton;
        private System.Windows.Forms.Button telexButton;
        private System.Windows.Forms.Button retrieveButton;
        private System.Windows.Forms.Label atcUnitLabel;
        private System.Windows.Forms.Label atcUnitDisplay;
        private System.Windows.Forms.Button helpButton;
        private System.Windows.Forms.FlowLayoutPanel messageFormatPanel;
        private System.Windows.Forms.Button settingsButton;
        private System.Windows.Forms.ImageList iconList;
        private System.Windows.Forms.ProgressBar SendingProgress;
    }
}

