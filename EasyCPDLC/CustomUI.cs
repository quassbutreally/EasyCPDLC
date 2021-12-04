using System.Windows.Forms;
using System.Drawing;

namespace EasyCPDLC
{
    public class CustomUI
    {


        public static UITextBox CreateTextBox(string _text, int _maxLength, Color _controlFrontColor, Color _controlBackColor, Font _font)
        {
            UITextBox _temp = new UITextBox(_controlFrontColor);

            _temp.BackColor = _controlBackColor;
            _temp.ForeColor = _controlFrontColor;
            _temp.Font = _font;
            _temp.MaxLength = _maxLength;
            _temp.BorderStyle = BorderStyle.None;
            _temp.Text = _text;
            _temp.CharacterCasing = CharacterCasing.Upper;
            _temp.Top = 10;
            _temp.Padding = new Padding(3, 0, 3, -10);
            _temp.Margin = new Padding(3, 5, 3, -10);
            _temp.Height = 20;
            _temp.TextAlign = HorizontalAlignment.Center;

            using (Graphics G = _temp.CreateGraphics())
            {
                _temp.Width = (int)(_temp.MaxLength *
                              G.MeasureString("x", _temp.Font).Width);
            }

            return _temp;
        }

        public static Label CreateTemplate(string _text, Color _controlFrontColor, Color _controlBackColor, Font _font)
        {
            Label _temp = new Label();
            _temp.BackColor = _controlBackColor;
            _temp.ForeColor = _controlFrontColor;
            _temp.Font = _font;
            _temp.AutoSize = true;
            _temp.Text = _text;
            _temp.Top = 10;
            _temp.Height = 20;
            _temp.TextAlign = ContentAlignment.MiddleLeft;
            _temp.Padding = new Padding(0, 10, 0, 0);
            _temp.Margin = new Padding(0, 0, 0, 0);

            return _temp;
        }
    }
}
