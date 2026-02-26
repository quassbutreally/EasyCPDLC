/*  EASYCPDLC: CPDLC Client for the VATSIM Network
    Copyright (C) 2026 Joshua Seagrave joshseagrave@googlemail.com

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
using System.ComponentModel;

namespace EasyCPDLC
{
    public class UiTextBox : TextBox
    {

        public UiTextBox()
        {
            BorderStyle = BorderStyle.None;
            AutoSize = false;
            Margin = new Padding(0, 5, 0, -5);
        }

        public sealed override bool AutoSize
        {
            get => base.AutoSize;
            set => base.AutoSize = value;
        }
    }

    public class TimerLabel : Label
    {
        private readonly Timer blinkTimer = new();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanFlash
        {
            set
            {
                field = value;
                if (field)
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
            blinkTimer.Tick += FlashElement;
            SetStyle(ControlStyles.Selectable, true);
        }

        private void FlashElement(object sender, EventArgs e)
        {
            ForeColor = ForeColor == Color.Orange ? SystemColors.ControlLight : Color.Orange;
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
            Region = Region.FromHrgn(CreateRoundRectRgn(2, 3, Width, Height, 15, 15));
            BorderStyle = BorderStyle.FixedSingle;
        }
    }

    public class UiCheckBox : CheckBox
    {
        public readonly string group;
        public UiCheckBox(string boxGroup)
        {
            group = boxGroup;
            FlatStyle = FlatStyle.Flat;
            TextAlign = ContentAlignment.MiddleRight;
            FlatAppearance.BorderSize = 0;
            AutoSize = false;
            Margin = new Padding(0, 5, 0, -10);
        }

        public sealed override bool AutoSize
        {
            get => base.AutoSize;
            set => base.AutoSize = value;
        }

        public sealed override ContentAlignment TextAlign
        {
            get => base.TextAlign;
            set => base.TextAlign = value;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            pevent.Graphics.Clear(BackColor);

            using (SolidBrush brush = new(ForeColor))
                pevent.Graphics.DrawString(Text, Font, brush, 27, 4);

            Point pt = new(4, 4);
            Rectangle rect = new(pt, new Size(16, 16));

            pevent.Graphics.FillRectangle(new SolidBrush(BackColor), rect);

            if (Checked)
            {
                using SolidBrush brush = new(ForeColor);
                pevent.Graphics.FillEllipse(brush, pt.X + 4, pt.Y + 4, 8, 8);
            }
            pevent.Graphics.DrawRectangle(SystemPens.ControlLight, rect);
        }
    }
}
