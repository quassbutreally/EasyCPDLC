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



namespace EasyCPDLC
{
    partial class DataEntry
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataEntry));
            this.titleLabel = new System.Windows.Forms.Label();
            this.exitButton = new System.Windows.Forms.Button();
            this.hoppieLogonLabel = new System.Windows.Forms.Label();
            this.vatsimCIDLabel = new System.Windows.Forms.Label();
            this.rememberCheckBox = new System.Windows.Forms.CheckBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.roundPanel2 = new EasyCPDLC.RoundPanel();
            this.vatsimCIDTextBox = new System.Windows.Forms.TextBox();
            this.roundPanel1 = new EasyCPDLC.RoundPanel();
            this.hoppieCodeTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.roundPanel2.SuspendLayout();
            this.roundPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Oxygen", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.titleLabel.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.titleLabel.Location = new System.Drawing.Point(28, 158);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(186, 41);
            this.titleLabel.TabIndex = 1;
            this.titleLabel.Text = "EasyCPDLC";
            // 
            // exitButton
            // 
            this.exitButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.exitButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.exitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exitButton.Font = new System.Drawing.Font("Oxygen", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.exitButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.exitButton.Location = new System.Drawing.Point(217, 0);
            this.exitButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(28, 28);
            this.exitButton.TabIndex = 2;
            this.exitButton.Text = "X";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // hoppieLogonLabel
            // 
            this.hoppieLogonLabel.AutoSize = true;
            this.hoppieLogonLabel.Font = new System.Drawing.Font("Oxygen", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.hoppieLogonLabel.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.hoppieLogonLabel.Location = new System.Drawing.Point(48, 217);
            this.hoppieLogonLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.hoppieLogonLabel.Name = "hoppieLogonLabel";
            this.hoppieLogonLabel.Size = new System.Drawing.Size(147, 19);
            this.hoppieLogonLabel.TabIndex = 3;
            this.hoppieLogonLabel.Text = "Hoppie Logon Code";
            // 
            // vatsimCIDLabel
            // 
            this.vatsimCIDLabel.AutoSize = true;
            this.vatsimCIDLabel.Font = new System.Drawing.Font("Oxygen", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.vatsimCIDLabel.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.vatsimCIDLabel.Location = new System.Drawing.Point(76, 280);
            this.vatsimCIDLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.vatsimCIDLabel.Name = "vatsimCIDLabel";
            this.vatsimCIDLabel.Size = new System.Drawing.Size(90, 19);
            this.vatsimCIDLabel.TabIndex = 4;
            this.vatsimCIDLabel.Text = "VATSIM CID";
            // 
            // rememberCheckBox
            // 
            this.rememberCheckBox.AutoSize = true;
            this.rememberCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rememberCheckBox.Font = new System.Drawing.Font("Oxygen", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.rememberCheckBox.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.rememberCheckBox.Location = new System.Drawing.Point(56, 342);
            this.rememberCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rememberCheckBox.Name = "rememberCheckBox";
            this.rememberCheckBox.Size = new System.Drawing.Size(130, 23);
            this.rememberCheckBox.TabIndex = 7;
            this.rememberCheckBox.Text = "Remember Me?";
            this.rememberCheckBox.UseVisualStyleBackColor = true;
            // 
            // connectButton
            // 
            this.connectButton.Enabled = false;
            this.connectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.connectButton.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.connectButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.connectButton.Location = new System.Drawing.Point(40, 380);
            this.connectButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(163, 33);
            this.connectButton.TabIndex = 8;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::EasyCPDLC.Properties.Resources.Stretchboard;
            this.pictureBox1.Location = new System.Drawing.Point(28, -18);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(230, 173);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DataEntry_MouseDown);
            // 
            // roundPanel2
            // 
            this.roundPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(5)))), ((int)(((byte)(5)))));
            this.roundPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.roundPanel2.Controls.Add(this.vatsimCIDTextBox);
            this.roundPanel2.Location = new System.Drawing.Point(17, 302);
            this.roundPanel2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.roundPanel2.Name = "roundPanel2";
            this.roundPanel2.Size = new System.Drawing.Size(208, 34);
            this.roundPanel2.TabIndex = 11;
            // 
            // vatsimCIDTextBox
            // 
            this.vatsimCIDTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(5)))), ((int)(((byte)(5)))));
            this.vatsimCIDTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.vatsimCIDTextBox.Font = new System.Drawing.Font("Oxygen", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.vatsimCIDTextBox.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.vatsimCIDTextBox.Location = new System.Drawing.Point(7, 8);
            this.vatsimCIDTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.vatsimCIDTextBox.MaxLength = 7;
            this.vatsimCIDTextBox.Name = "vatsimCIDTextBox";
            this.vatsimCIDTextBox.Size = new System.Drawing.Size(192, 19);
            this.vatsimCIDTextBox.TabIndex = 1;
            this.vatsimCIDTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.vatsimCIDTextBox.TextChanged += new System.EventHandler(this.VatsimCIDTextBox_TextChanged);
            this.vatsimCIDTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NumsOnly);
            // 
            // roundPanel1
            // 
            this.roundPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(5)))), ((int)(((byte)(5)))));
            this.roundPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.roundPanel1.Controls.Add(this.hoppieCodeTextBox);
            this.roundPanel1.Location = new System.Drawing.Point(17, 239);
            this.roundPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.roundPanel1.Name = "roundPanel1";
            this.roundPanel1.Size = new System.Drawing.Size(208, 34);
            this.roundPanel1.TabIndex = 10;
            // 
            // hoppieCodeTextBox
            // 
            this.hoppieCodeTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(5)))), ((int)(((byte)(5)))));
            this.hoppieCodeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.hoppieCodeTextBox.Font = new System.Drawing.Font("Oxygen", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.hoppieCodeTextBox.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.hoppieCodeTextBox.Location = new System.Drawing.Point(7, 7);
            this.hoppieCodeTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.hoppieCodeTextBox.MaxLength = 18;
            this.hoppieCodeTextBox.Name = "hoppieCodeTextBox";
            this.hoppieCodeTextBox.Size = new System.Drawing.Size(192, 19);
            this.hoppieCodeTextBox.TabIndex = 0;
            this.hoppieCodeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.hoppieCodeTextBox.TextChanged += new System.EventHandler(this.HoppieCodeTextBox_TextChanged);
            // 
            // DataEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(244, 433);
            this.Controls.Add(this.roundPanel2);
            this.Controls.Add(this.roundPanel1);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.rememberCheckBox);
            this.Controls.Add(this.vatsimCIDLabel);
            this.Controls.Add(this.hoppieLogonLabel);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "DataEntry";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DataEntry";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DataEntry_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.roundPanel2.ResumeLayout(false);
            this.roundPanel2.PerformLayout();
            this.roundPanel1.ResumeLayout(false);
            this.roundPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Label hoppieLogonLabel;
        private System.Windows.Forms.Label vatsimCIDLabel;
        private System.Windows.Forms.CheckBox rememberCheckBox;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private RoundPanel roundPanel1;
        private System.Windows.Forms.TextBox hoppieCodeTextBox;
        private RoundPanel roundPanel2;
        private System.Windows.Forms.TextBox vatsimCIDTextBox;
    }
}