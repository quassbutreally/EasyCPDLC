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
            Controls.Add(new Label()
            { Height = 1, Dock = DockStyle.Bottom, BackColor = _color, Margin = new Padding(-10, 0, 0, 0)});
        }
    }
}
