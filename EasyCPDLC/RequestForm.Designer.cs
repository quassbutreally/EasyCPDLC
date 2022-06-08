namespace EasyCPDLC
{
    partial class RequestForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RequestForm));
            this.titleLabel = new System.Windows.Forms.Label();
            this.exitButton = new System.Windows.Forms.Button();
            this.sendButton = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.pdcButton = new System.Windows.Forms.Button();
            this.logonButton = new System.Windows.Forms.Button();
            this.requestButton = new System.Windows.Forms.Button();
            this.radioContainer = new System.Windows.Forms.Panel();
            this.ocnClxRadioButton = new System.Windows.Forms.RadioButton();
            this.reportRadioButton = new System.Windows.Forms.RadioButton();
            this.requestRadioButton = new System.Windows.Forms.RadioButton();
            this.logonRadioButton = new System.Windows.Forms.RadioButton();
            this.depClxRadioButton = new System.Windows.Forms.RadioButton();
            this.reportButton = new System.Windows.Forms.Button();
            this.requestContainer = new System.Windows.Forms.Panel();
            this.wcwRadioButton = new System.Windows.Forms.RadioButton();
            this.directRadioButton = new System.Windows.Forms.RadioButton();
            this.speedRadioButton = new System.Windows.Forms.RadioButton();
            this.levelRadioButton = new System.Windows.Forms.RadioButton();
            this.messageFormatPanel = new System.Windows.Forms.TableLayoutPanel();
            this.radioContainer.SuspendLayout();
            this.requestContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Oxygen", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.titleLabel.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.titleLabel.Location = new System.Drawing.Point(481, 9);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(186, 41);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "EasyCPDLC";
            this.titleLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WindowDrag);
            // 
            // exitButton
            // 
            this.exitButton.AccessibleDescription = "Exit CPDLC Form";
            this.exitButton.AccessibleName = "Exit";
            this.exitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exitButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.exitButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.exitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exitButton.Font = new System.Drawing.Font("Oxygen", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.exitButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.exitButton.Location = new System.Drawing.Point(686, 0);
            this.exitButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(28, 28);
            this.exitButton.TabIndex = 8;
            this.exitButton.Text = "X";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // sendButton
            // 
            this.sendButton.AccessibleDescription = "Send Request";
            this.sendButton.AccessibleName = "Send";
            this.sendButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sendButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sendButton.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.sendButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.sendButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.sendButton.Location = new System.Drawing.Point(576, 280);
            this.sendButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(121, 43);
            this.sendButton.TabIndex = 7;
            this.sendButton.Text = "SEND";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // clearButton
            // 
            this.clearButton.AccessibleDescription = "Clear Request";
            this.clearButton.AccessibleName = "Clear";
            this.clearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.clearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearButton.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.clearButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.clearButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.clearButton.Location = new System.Drawing.Point(448, 280);
            this.clearButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(121, 43);
            this.clearButton.TabIndex = 6;
            this.clearButton.Text = "CLEAR";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // pdcButton
            // 
            this.pdcButton.AccessibleName = "Request Clearance";
            this.pdcButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pdcButton.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.pdcButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.pdcButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.pdcButton.Location = new System.Drawing.Point(14, 9);
            this.pdcButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pdcButton.Name = "pdcButton";
            this.pdcButton.Size = new System.Drawing.Size(110, 43);
            this.pdcButton.TabIndex = 1;
            this.pdcButton.Text = "REQ CLX";
            this.pdcButton.UseVisualStyleBackColor = true;
            this.pdcButton.Click += new System.EventHandler(this.PdcButton_Click);
            // 
            // logonButton
            // 
            this.logonButton.AccessibleName = "Logon";
            this.logonButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.logonButton.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.logonButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.logonButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.logonButton.Location = new System.Drawing.Point(131, 9);
            this.logonButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.logonButton.Name = "logonButton";
            this.logonButton.Size = new System.Drawing.Size(110, 43);
            this.logonButton.TabIndex = 2;
            this.logonButton.Text = "LOGON";
            this.logonButton.UseVisualStyleBackColor = true;
            this.logonButton.Click += new System.EventHandler(this.LogonButton_Click);
            // 
            // requestButton
            // 
            this.requestButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.requestButton.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.requestButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.requestButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.requestButton.Location = new System.Drawing.Point(247, 9);
            this.requestButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.requestButton.Name = "requestButton";
            this.requestButton.Size = new System.Drawing.Size(110, 43);
            this.requestButton.TabIndex = 3;
            this.requestButton.Text = "REQUEST";
            this.requestButton.UseVisualStyleBackColor = true;
            this.requestButton.Click += new System.EventHandler(this.RequestButton_Click);
            // 
            // radioContainer
            // 
            this.radioContainer.Controls.Add(this.ocnClxRadioButton);
            this.radioContainer.Controls.Add(this.reportRadioButton);
            this.radioContainer.Controls.Add(this.requestRadioButton);
            this.radioContainer.Controls.Add(this.logonRadioButton);
            this.radioContainer.Controls.Add(this.depClxRadioButton);
            this.radioContainer.Location = new System.Drawing.Point(280, 272);
            this.radioContainer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radioContainer.Name = "radioContainer";
            this.radioContainer.Size = new System.Drawing.Size(119, 30);
            this.radioContainer.TabIndex = 12;
            this.radioContainer.Visible = false;
            // 
            // ocnClxRadioButton
            // 
            this.ocnClxRadioButton.AutoSize = true;
            this.ocnClxRadioButton.Location = new System.Drawing.Point(97, 7);
            this.ocnClxRadioButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ocnClxRadioButton.Name = "ocnClxRadioButton";
            this.ocnClxRadioButton.Size = new System.Drawing.Size(14, 13);
            this.ocnClxRadioButton.TabIndex = 5;
            this.ocnClxRadioButton.TabStop = true;
            this.ocnClxRadioButton.UseVisualStyleBackColor = true;
            this.ocnClxRadioButton.Visible = false;
            // 
            // reportRadioButton
            // 
            this.reportRadioButton.AutoSize = true;
            this.reportRadioButton.Location = new System.Drawing.Point(75, 8);
            this.reportRadioButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.reportRadioButton.Name = "reportRadioButton";
            this.reportRadioButton.Size = new System.Drawing.Size(14, 13);
            this.reportRadioButton.TabIndex = 3;
            this.reportRadioButton.TabStop = true;
            this.reportRadioButton.UseVisualStyleBackColor = true;
            this.reportRadioButton.Visible = false;
            // 
            // requestRadioButton
            // 
            this.requestRadioButton.AutoSize = true;
            this.requestRadioButton.Location = new System.Drawing.Point(51, 8);
            this.requestRadioButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.requestRadioButton.Name = "requestRadioButton";
            this.requestRadioButton.Size = new System.Drawing.Size(14, 13);
            this.requestRadioButton.TabIndex = 2;
            this.requestRadioButton.TabStop = true;
            this.requestRadioButton.UseVisualStyleBackColor = true;
            this.requestRadioButton.Visible = false;
            // 
            // logonRadioButton
            // 
            this.logonRadioButton.AutoSize = true;
            this.logonRadioButton.Location = new System.Drawing.Point(28, 8);
            this.logonRadioButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.logonRadioButton.Name = "logonRadioButton";
            this.logonRadioButton.Size = new System.Drawing.Size(14, 13);
            this.logonRadioButton.TabIndex = 1;
            this.logonRadioButton.TabStop = true;
            this.logonRadioButton.UseVisualStyleBackColor = true;
            this.logonRadioButton.Visible = false;
            // 
            // depClxRadioButton
            // 
            this.depClxRadioButton.AutoSize = true;
            this.depClxRadioButton.Location = new System.Drawing.Point(5, 8);
            this.depClxRadioButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.depClxRadioButton.Name = "depClxRadioButton";
            this.depClxRadioButton.Size = new System.Drawing.Size(14, 13);
            this.depClxRadioButton.TabIndex = 0;
            this.depClxRadioButton.TabStop = true;
            this.depClxRadioButton.UseVisualStyleBackColor = true;
            this.depClxRadioButton.Visible = false;
            // 
            // reportButton
            // 
            this.reportButton.Enabled = false;
            this.reportButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reportButton.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.reportButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.reportButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.reportButton.Location = new System.Drawing.Point(364, 9);
            this.reportButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.reportButton.Name = "reportButton";
            this.reportButton.Size = new System.Drawing.Size(110, 43);
            this.reportButton.TabIndex = 4;
            this.reportButton.Text = "REPORT";
            this.reportButton.UseVisualStyleBackColor = true;
            this.reportButton.Click += new System.EventHandler(this.ReportButton_Click);
            // 
            // requestContainer
            // 
            this.requestContainer.Controls.Add(this.wcwRadioButton);
            this.requestContainer.Controls.Add(this.directRadioButton);
            this.requestContainer.Controls.Add(this.speedRadioButton);
            this.requestContainer.Controls.Add(this.levelRadioButton);
            this.requestContainer.Location = new System.Drawing.Point(178, 272);
            this.requestContainer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.requestContainer.Name = "requestContainer";
            this.requestContainer.Size = new System.Drawing.Size(94, 30);
            this.requestContainer.TabIndex = 13;
            this.requestContainer.Visible = false;
            // 
            // wcwRadioButton
            // 
            this.wcwRadioButton.AutoSize = true;
            this.wcwRadioButton.Location = new System.Drawing.Point(75, 8);
            this.wcwRadioButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.wcwRadioButton.Name = "wcwRadioButton";
            this.wcwRadioButton.Size = new System.Drawing.Size(14, 13);
            this.wcwRadioButton.TabIndex = 3;
            this.wcwRadioButton.TabStop = true;
            this.wcwRadioButton.UseVisualStyleBackColor = true;
            this.wcwRadioButton.Visible = false;
            // 
            // directRadioButton
            // 
            this.directRadioButton.AutoSize = true;
            this.directRadioButton.Location = new System.Drawing.Point(51, 8);
            this.directRadioButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.directRadioButton.Name = "directRadioButton";
            this.directRadioButton.Size = new System.Drawing.Size(14, 13);
            this.directRadioButton.TabIndex = 2;
            this.directRadioButton.TabStop = true;
            this.directRadioButton.UseVisualStyleBackColor = true;
            this.directRadioButton.Visible = false;
            // 
            // speedRadioButton
            // 
            this.speedRadioButton.AutoSize = true;
            this.speedRadioButton.Location = new System.Drawing.Point(28, 8);
            this.speedRadioButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.speedRadioButton.Name = "speedRadioButton";
            this.speedRadioButton.Size = new System.Drawing.Size(14, 13);
            this.speedRadioButton.TabIndex = 1;
            this.speedRadioButton.TabStop = true;
            this.speedRadioButton.UseVisualStyleBackColor = true;
            this.speedRadioButton.Visible = false;
            // 
            // levelRadioButton
            // 
            this.levelRadioButton.AutoSize = true;
            this.levelRadioButton.Location = new System.Drawing.Point(5, 8);
            this.levelRadioButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.levelRadioButton.Name = "levelRadioButton";
            this.levelRadioButton.Size = new System.Drawing.Size(14, 13);
            this.levelRadioButton.TabIndex = 0;
            this.levelRadioButton.TabStop = true;
            this.levelRadioButton.UseVisualStyleBackColor = true;
            this.levelRadioButton.Visible = false;
            // 
            // messageFormatPanel
            // 
            this.messageFormatPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messageFormatPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(5)))), ((int)(((byte)(5)))));
            this.messageFormatPanel.ColumnCount = 6;
            this.messageFormatPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.messageFormatPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.messageFormatPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.messageFormatPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.messageFormatPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.messageFormatPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.messageFormatPanel.Location = new System.Drawing.Point(14, 58);
            this.messageFormatPanel.Name = "messageFormatPanel";
            this.messageFormatPanel.RowCount = 7;
            this.messageFormatPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.messageFormatPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.messageFormatPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.messageFormatPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.messageFormatPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.messageFormatPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.messageFormatPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.messageFormatPanel.Size = new System.Drawing.Size(683, 215);
            this.messageFormatPanel.TabIndex = 5;
            // 
            // RequestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(714, 335);
            this.Controls.Add(this.messageFormatPanel);
            this.Controls.Add(this.requestContainer);
            this.Controls.Add(this.reportButton);
            this.Controls.Add(this.requestButton);
            this.Controls.Add(this.radioContainer);
            this.Controls.Add(this.logonButton);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.pdcButton);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.titleLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximumSize = new System.Drawing.Size(714, 335);
            this.MinimumSize = new System.Drawing.Size(714, 335);
            this.Name = "RequestForm";
            this.Text = "RequestForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RequestForm_FormClosing);
            this.Load += new System.EventHandler(this.RequestForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WindowDrag);
            this.radioContainer.ResumeLayout(false);
            this.radioContainer.PerformLayout();
            this.requestContainer.ResumeLayout(false);
            this.requestContainer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Button pdcButton;
        private System.Windows.Forms.Button logonButton;
        private System.Windows.Forms.Button requestButton;
        private System.Windows.Forms.Panel radioContainer;
        private System.Windows.Forms.RadioButton logonRadioButton;
        private System.Windows.Forms.RadioButton depClxRadioButton;
        private System.Windows.Forms.RadioButton requestRadioButton;
        private System.Windows.Forms.RadioButton reportRadioButton;
        private System.Windows.Forms.Button reportButton;
        private System.Windows.Forms.Panel requestContainer;
        private System.Windows.Forms.RadioButton wcwRadioButton;
        private System.Windows.Forms.RadioButton directRadioButton;
        private System.Windows.Forms.RadioButton speedRadioButton;
        private System.Windows.Forms.RadioButton levelRadioButton;
        private System.Windows.Forms.RadioButton ocnClxRadioButton;
        private System.Windows.Forms.TableLayoutPanel messageFormatPanel;
    }
}