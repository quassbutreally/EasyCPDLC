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
            this.messageFormatPanel.Size = new System.Drawing.Size(527, 127);
            this.messageFormatPanel.TabIndex = 4;
            // 
            // clearButton
            // 
            this.clearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.clearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearButton.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Bold);
            this.clearButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.clearButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.clearButton.Location = new System.Drawing.Point(327, 190);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(104, 37);
            this.clearButton.TabIndex = 8;
            this.clearButton.Text = "CLEAR";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.ReloadPanel);
            // 
            // sendButton
            // 
            this.sendButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sendButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sendButton.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Bold);
            this.sendButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.sendButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.sendButton.Location = new System.Drawing.Point(437, 190);
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
            this.exitButton.Location = new System.Drawing.Point(532, 0);
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
            this.titleLabel.Location = new System.Drawing.Point(352, 9);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(186, 41);
            this.titleLabel.TabIndex = 10;
            this.titleLabel.Text = "EasyCPDLC";
            this.titleLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WindowDrag);
            // 
            // TelexForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(555, 241);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.messageFormatPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(555, 241);
            this.Name = "TelexForm";
            this.Text = "TelexForm";
            this.Load += new System.EventHandler(this.ReloadPanel);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WindowDrag);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel messageFormatPanel;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Label titleLabel;
    }
}