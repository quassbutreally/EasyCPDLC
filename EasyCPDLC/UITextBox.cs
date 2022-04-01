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



using System;
using System.Drawing;
using System.Windows.Forms;

namespace EasyCPDLC
{
    public class UITextBox : TextBox
    {

        public UITextBox(Color _color)
        {
            BorderStyle = BorderStyle.None;
            AutoSize = false;
            Margin = new Padding(0, 5, 0, -5);
            Controls.Add(new Label()
            { Height = 1, Dock = DockStyle.Bottom, BackColor = _color, Margin = new Padding(0, -10, 0, 0) });
        }
    }

    public class TimerLabel : Label
    {
        readonly Timer blinkTimer = new Timer();
        private bool _canFlash = false;
        public bool canFlash
        {
            get
            {
                return _canFlash;
            }
            set
            {
                _canFlash = value;
                if(_canFlash)
                {
                    blinkTimer.Start();
                }
                else
                {
                    blinkTimer.Stop();
                    ForeColor = SystemColors.ControlLight;
                }
            }
        }
        public TimerLabel()
        {
            blinkTimer.Interval = 500;
            blinkTimer.Tick += new EventHandler(FlashElement);
            SetStyle(ControlStyles.Selectable, true);
        }

        public void FlashElement(object sender, EventArgs e)
        {
            this.ForeColor = this.ForeColor == Color.Orange ? SystemColors.ControlLight : Color.Orange;
        }
    }

    public class RoundPanel : Panel
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
    (
        int nLeftRect, // X-coordinate of upper-left corner or padding at start
        int nTopRect,// Y-coordinate of upper-left corner or padding at the top of the textbox
        int nRightRect, // X-coordinate of lower-right corner or Width of the object
        int nBottomRect,// Y-coordinate of lower-right corner or Height of the object
                        //RADIUS, how round do you want it to be?
        int nheightRect, //height of ellipse 
        int nweightRect //width of ellipse
    );
        protected override void OnResize(EventArgs e)
        {
            base.OnCreateControl();
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(2, 3, this.Width, this.Height, 15, 15));
            this.BorderStyle = BorderStyle.FixedSingle;
        }
    }

    public class UICheckBox : CheckBox
    {
        public string group;
        public UICheckBox(string _group)
        {
            group = _group;
            FlatStyle = FlatStyle.Flat;
            TextAlign = ContentAlignment.MiddleRight;
            FlatAppearance.BorderSize = 0;
            AutoSize = false;
            Margin = new Padding(0, 5, 0, -10);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            pevent.Graphics.Clear(BackColor);

            using (SolidBrush brush = new SolidBrush(ForeColor))
                pevent.Graphics.DrawString(Text, Font, brush, 27, 4);

            Point pt = new Point(4, 4);
            Rectangle rect = new Rectangle(pt, new Size(16, 16));

            pevent.Graphics.FillRectangle(new SolidBrush(BackColor), rect);

            if (Checked)
            {
                using (SolidBrush brush = new SolidBrush(ForeColor))
                    pevent.Graphics.FillEllipse(brush, pt.X + 4, pt.Y + 4, 8, 8);
            }
            pevent.Graphics.DrawRectangle(SystemPens.ControlLight, rect);
        }
    }
}
