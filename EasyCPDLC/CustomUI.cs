using System.Drawing;
using System.Windows.Forms;

namespace EasyCPDLC
{
    public class CustomUI
    {

        public static UITextBox CreateTextBox(string _text, int _maxLength, Color _controlFrontColor, Color _controlBackColor, Font _font)
        {
            UITextBox _temp = new UITextBox(_controlFrontColor)
            {
                BackColor = _controlBackColor,
                ForeColor = _controlFrontColor,
                Font = _font,
                MaxLength = _maxLength,
                BorderStyle = BorderStyle.None,
                Text = _text,
                CharacterCasing = CharacterCasing.Upper,
                Top = 10,
                Padding = new Padding(3, 0, 3, -10),
                Margin = new Padding(3, 5, 3, -10),
                Height = 20,
                TextAlign = HorizontalAlignment.Center
            };

            using (Graphics G = _temp.CreateGraphics())
            {
                _temp.Width = (int)(_temp.MaxLength *
                              G.MeasureString("x", _temp.Font).Width);
            }

            return _temp;
        }

        public static Label CreateTemplate(string _text, Color _controlFrontColor, Color _controlBackColor, Font _font)
        {
            Label _temp = new Label
            {
                BackColor = _controlBackColor,
                ForeColor = _controlFrontColor,
                Font = _font,
                AutoSize = true,
                Text = _text,
                Top = 10,
                Height = 20,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(0, 10, 0, 0),
                Margin = new Padding(0, 0, 0, 0)
            };

            return _temp;
        }
    }
}
