namespace EasyCPDLC
{
    partial class TelexForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TelexForm));
            this.messageFormatPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.clearButton = new System.Windows.Forms.Button();
            this.sendButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.freeTextButton = new System.Windows.Forms.Button();
            this.metarButton = new System.Windows.Forms.Button();
            this.atisButton = new System.Windows.Forms.Button();
            this.radioContainer = new System.Windows.Forms.Panel();
            this.atisRadioButton = new System.Windows.Forms.RadioButton();
            this.metarRadioButton = new System.Windows.Forms.RadioButton();
            this.freeTextRadioButton = new System.Windows.Forms.RadioButton();
            this.radioContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // messageFormatPanel
            // 
            this.messageFormatPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messageFormatPanel.AutoScroll = true;
            this.messageFormatPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(5)))), ((int)(((byte)(5)))));
            this.messageFormatPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.messageFormatPanel.Location = new System.Drawing.Point(14, 55);
            this.messageFormatPanel.Margin = new System.Windows.Forms.Padding(5);
            this.messageFormatPanel.Name = "messageFormatPanel";
            this.messageFormatPanel.Padding = new System.Windows.Forms.Padding(10, 0, 0, 30);
            this.messageFormatPanel.Size = new System.Drawing.Size(504, 127);
            this.messageFormatPanel.TabIndex = 4;
            // 
            // clearButton
            // 
            this.clearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.clearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearButton.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Bold);
            this.clearButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.clearButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.clearButton.Location = new System.Drawing.Point(304, 190);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(104, 37);
            this.clearButton.TabIndex = 8;
            this.clearButton.Text = "CLEAR";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.ResetPanel);
            // 
            // sendButton
            // 
            this.sendButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sendButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sendButton.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Bold);
            this.sendButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.sendButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.sendButton.Location = new System.Drawing.Point(414, 190);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(104, 37);
            this.sendButton.TabIndex = 9;
            this.sendButton.Text = "SEND";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exitButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.exitButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.exitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exitButton.Font = new System.Drawing.Font("Oxygen", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exitButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.exitButton.Location = new System.Drawing.Point(509, 0);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(24, 24);
            this.exitButton.TabIndex = 11;
            this.exitButton.Text = "X";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // titleLabel
            // 
            this.titleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Oxygen", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.titleLabel.Location = new System.Drawing.Point(329, 9);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(186, 41);
            this.titleLabel.TabIndex = 10;
            this.titleLabel.Text = "EasyCPDLC";
            this.titleLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WindowDrag);
            // 
            // freeTextButton
            // 
            this.freeTextButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.freeTextButton.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Bold);
            this.freeTextButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.freeTextButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.freeTextButton.Location = new System.Drawing.Point(14, 10);
            this.freeTextButton.Name = "freeTextButton";
            this.freeTextButton.Size = new System.Drawing.Size(100, 37);
            this.freeTextButton.TabIndex = 12;
            this.freeTextButton.Text = "FREE TEXT";
            this.freeTextButton.UseVisualStyleBackColor = true;
            this.freeTextButton.Click += new System.EventHandler(this.freeTextButton_Click);
            // 
            // metarButton
            // 
            this.metarButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.metarButton.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Bold);
            this.metarButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.metarButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.metarButton.Location = new System.Drawing.Point(120, 10);
            this.metarButton.Name = "metarButton";
            this.metarButton.Size = new System.Drawing.Size(100, 37);
            this.metarButton.TabIndex = 13;
            this.metarButton.Text = "METAR";
            this.metarButton.UseVisualStyleBackColor = true;
            this.metarButton.Click += new System.EventHandler(this.metarButton_Click);
            // 
            // atisButton
            // 
            this.atisButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.atisButton.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Bold);
            this.atisButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.atisButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.atisButton.Location = new System.Drawing.Point(226, 10);
            this.atisButton.Name = "atisButton";
            this.atisButton.Size = new System.Drawing.Size(100, 37);
            this.atisButton.TabIndex = 14;
            this.atisButton.Text = "ATIS";
            this.atisButton.UseVisualStyleBackColor = true;
            this.atisButton.Click += new System.EventHandler(this.atisButton_Click);
            // 
            // radioContainer
            // 
            this.radioContainer.Controls.Add(this.atisRadioButton);
            this.radioContainer.Controls.Add(this.metarRadioButton);
            this.radioContainer.Controls.Add(this.freeTextRadioButton);
            this.radioContainer.Location = new System.Drawing.Point(198, 190);
            this.radioContainer.Name = "radioContainer";
            this.radioContainer.Size = new System.Drawing.Size(62, 26);
            this.radioContainer.TabIndex = 15;
            this.radioContainer.Visible = false;
            // 
            // atisRadioButton
            // 
            this.atisRadioButton.AutoSize = true;
            this.atisRadioButton.Location = new System.Drawing.Point(44, 7);
            this.atisRadioButton.Name = "atisRadioButton";
            this.atisRadioButton.Size = new System.Drawing.Size(14, 13);
            this.atisRadioButton.TabIndex = 2;
            this.atisRadioButton.TabStop = true;
            this.atisRadioButton.UseVisualStyleBackColor = true;
            this.atisRadioButton.Visible = false;
            // 
            // metarRadioButton
            // 
            this.metarRadioButton.AutoSize = true;
            this.metarRadioButton.Location = new System.Drawing.Point(24, 7);
            this.metarRadioButton.Name = "metarRadioButton";
            this.metarRadioButton.Size = new System.Drawing.Size(14, 13);
            this.metarRadioButton.TabIndex = 1;
            this.metarRadioButton.TabStop = true;
            this.metarRadioButton.UseVisualStyleBackColor = true;
            this.metarRadioButton.Visible = false;
            // 
            // freeTextRadioButton
            // 
            this.freeTextRadioButton.AutoSize = true;
            this.freeTextRadioButton.Location = new System.Drawing.Point(4, 7);
            this.freeTextRadioButton.Name = "freeTextRadioButton";
            this.freeTextRadioButton.Size = new System.Drawing.Size(14, 13);
            this.freeTextRadioButton.TabIndex = 0;
            this.freeTextRadioButton.TabStop = true;
            this.freeTextRadioButton.UseVisualStyleBackColor = true;
            this.freeTextRadioButton.Visible = false;
            // 
            // TelexForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(532, 241);
            this.Controls.Add(this.radioContainer);
            this.Controls.Add(this.atisButton);
            this.Controls.Add(this.metarButton);
            this.Controls.Add(this.freeTextButton);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.messageFormatPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(532, 241);
            this.Name = "TelexForm";
            this.Text = "TelexForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TelexForm_FormClosing);
            this.Load += new System.EventHandler(this.ReloadPanel);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WindowDrag);
            this.radioContainer.ResumeLayout(false);
            this.radioContainer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel messageFormatPanel;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Button freeTextButton;
        private System.Windows.Forms.Button metarButton;
        private System.Windows.Forms.Button atisButton;
        private System.Windows.Forms.Panel radioContainer;
        private System.Windows.Forms.RadioButton atisRadioButton;
        private System.Windows.Forms.RadioButton metarRadioButton;
        private System.Windows.Forms.RadioButton freeTextRadioButton;
    }
}