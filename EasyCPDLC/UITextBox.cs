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

            Rectangle fRect = ClientRectangle;
        }
    }
}
