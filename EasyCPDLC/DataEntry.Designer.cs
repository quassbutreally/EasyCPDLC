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
            this.hoppieCodeTextBox = new System.Windows.Forms.TextBox();
            this.rememberCheckBox = new System.Windows.Forms.CheckBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.vatsimCIDTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Oxygen", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.titleLabel.Location = new System.Drawing.Point(11, 137);
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
            this.exitButton.Font = new System.Drawing.Font("Oxygen", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exitButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.exitButton.Location = new System.Drawing.Point(186, 0);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(24, 24);
            this.exitButton.TabIndex = 2;
            this.exitButton.Text = "X";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // hoppieLogonLabel
            // 
            this.hoppieLogonLabel.AutoSize = true;
            this.hoppieLogonLabel.Font = new System.Drawing.Font("Oxygen", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hoppieLogonLabel.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.hoppieLogonLabel.Location = new System.Drawing.Point(31, 184);
            this.hoppieLogonLabel.Name = "hoppieLogonLabel";
            this.hoppieLogonLabel.Size = new System.Drawing.Size(147, 19);
            this.hoppieLogonLabel.TabIndex = 3;
            this.hoppieLogonLabel.Text = "Hoppie Logon Code";
            // 
            // vatsimCIDLabel
            // 
            this.vatsimCIDLabel.AutoSize = true;
            this.vatsimCIDLabel.Font = new System.Drawing.Font("Oxygen", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vatsimCIDLabel.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.vatsimCIDLabel.Location = new System.Drawing.Point(59, 237);
            this.vatsimCIDLabel.Name = "vatsimCIDLabel";
            this.vatsimCIDLabel.Size = new System.Drawing.Size(90, 19);
            this.vatsimCIDLabel.TabIndex = 4;
            this.vatsimCIDLabel.Text = "VATSIM CID";
            // 
            // hoppieCodeTextBox
            // 
            this.hoppieCodeTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(5)))), ((int)(((byte)(5)))));
            this.hoppieCodeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hoppieCodeTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.hoppieCodeTextBox.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.hoppieCodeTextBox.Location = new System.Drawing.Point(33, 206);
            this.hoppieCodeTextBox.MaxLength = 14;
            this.hoppieCodeTextBox.Name = "hoppieCodeTextBox";
            this.hoppieCodeTextBox.Size = new System.Drawing.Size(143, 24);
            this.hoppieCodeTextBox.TabIndex = 5;
            this.hoppieCodeTextBox.TextChanged += new System.EventHandler(this.hoppieCodeTextBox_TextChanged);
            // 
            // rememberCheckBox
            // 
            this.rememberCheckBox.AutoSize = true;
            this.rememberCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rememberCheckBox.Font = new System.Drawing.Font("Oxygen", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rememberCheckBox.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.rememberCheckBox.Location = new System.Drawing.Point(39, 289);
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
            this.connectButton.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.connectButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.connectButton.Location = new System.Drawing.Point(34, 318);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(140, 29);
            this.connectButton.TabIndex = 8;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::EasyCPDLC.Properties.Resources.Stretchboard;
            this.pictureBox1.Location = new System.Drawing.Point(13, -19);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(197, 150);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DataEntry_MouseDown);
            // 
            // vatsimCIDTextBox
            // 
            this.vatsimCIDTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(5)))), ((int)(((byte)(5)))));
            this.vatsimCIDTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.vatsimCIDTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vatsimCIDTextBox.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.vatsimCIDTextBox.Location = new System.Drawing.Point(33, 255);
            this.vatsimCIDTextBox.MaxLength = 7;
            this.vatsimCIDTextBox.Name = "vatsimCIDTextBox";
            this.vatsimCIDTextBox.Size = new System.Drawing.Size(143, 24);
            this.vatsimCIDTextBox.TabIndex = 6;
            this.vatsimCIDTextBox.TextChanged += new System.EventHandler(this.vatsimCIDTextBox_TextChanged);
            // 
            // DataEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(209, 365);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.rememberCheckBox);
            this.Controls.Add(this.vatsimCIDTextBox);
            this.Controls.Add(this.hoppieCodeTextBox);
            this.Controls.Add(this.vatsimCIDLabel);
            this.Controls.Add(this.hoppieLogonLabel);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DataEntry";
            this.Text = "DataEntry";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DataEntry_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Label hoppieLogonLabel;
        private System.Windows.Forms.Label vatsimCIDLabel;
        private System.Windows.Forms.TextBox hoppieCodeTextBox;
        private System.Windows.Forms.CheckBox rememberCheckBox;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox vatsimCIDTextBox;
    }
}