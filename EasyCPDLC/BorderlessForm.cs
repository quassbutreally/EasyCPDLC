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
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace EasyCPDLC
{
    /// <summary>
    /// Holds the colours and fonts that all forms share.
    /// Populated from MainForm and passed down via child form constructors.
    /// </summary>
    public record Theme(
        Color BackColor,
        Color ForeColor,
        Font TextFont,
        Font TextFontBold
    );

    /// <summary>
    /// Base form providing:
    ///   - Borderless window chrome (drag-to-move, bottom-right resize)
    ///   - Shared UI factory methods for labels, text boxes and checkboxes
    /// </summary>
    public class BorderlessForm : Form
    {
        // ── Window chrome ────────────────────────────────────────────────────

        private const int WmNchittest     = 0x0084;
        private const int WmNclbuttondown = 0xA1;
        private const int HtCaption       = 0x2;
        private const int HtBottomright   = 17;
        private const int GripSize        = 16;  // px — resize corner hit area
        private const int CaptionHeight   = 32;  // px — drag zone at top of window

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        protected void DragWindow(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WmNclbuttondown, HtCaption, 0);
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WmNchittest)
            {
                var pos = PointToClient(new Point(m.LParam.ToInt32()));

                if (pos.Y < CaptionHeight)
                {
                    m.Result = HtCaption;
                    return;
                }

                if (pos.X >= ClientSize.Width - GripSize && pos.Y >= ClientSize.Height - GripSize)
                {
                    m.Result = HtBottomright;
                    return;
                }
            }

            base.WndProc(ref m);
        }

        // ── Theme ────────────────────────────────────────────────────────────

        /// <summary>
        /// Set by child form constructors before calling factory methods.
        /// </summary>
        protected Theme Theme { get; init; }

        // ── Shared UI factories ──────────────────────────────────────────────

        protected AccessibleLabel CreateLabel(string text, int height = 20)
        {
            return new AccessibleLabel(Theme.ForeColor)
            {
                BackColor = Theme.BackColor,
                ForeColor = Theme.ForeColor,
                Font      = Theme.TextFont,
                AutoSize  = true,
                Text      = text,
                Top       = 10,
                Height    = height,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding   = new Padding(0, 10, 0, 0),
                Margin    = new Padding(0, 0, 0, 0),
                TabStop   = true,
                TabIndex  = 0
            };
        }

        /// <summary>
        /// Non-focusable bracket label used to visually frame the remarks box.
        /// </summary>
        protected AccessibleLabel CreateBracketLabel(string text, AnchorStyles anchor)
        {
            var label = CreateLabel(text, 30);
            label.TabStop = false;
            label.Anchor  = anchor;
            return label;
        }

        /// <summary>
        /// Fixed-width uppercase single-line text box, sized to its max length.
        /// </summary>
        protected UiTextBox CreateTextBox(
            string text,
            int    maxLength,
            bool   readOnly = false,
            bool   numsOnly = false)
        {
            var box = new UiTextBox
            {
                BackColor       = Theme.BackColor,
                ForeColor       = Theme.ForeColor,
                Font            = Theme.TextFontBold,
                MaxLength       = maxLength,
                BorderStyle     = BorderStyle.None,
                Text            = text,
                CharacterCasing = CharacterCasing.Upper,
                Top             = 10,
                PlaceholderText = new string('▯', maxLength),
                Height          = 30,
                ReadOnly        = readOnly,
                TextAlign       = HorizontalAlignment.Left,
                TabIndex        = 0,
                Anchor          = AnchorStyles.Left
            };

            if (numsOnly)
                box.KeyPress += NumsOnly;

            using var g = box.CreateGraphics();
            box.Width = (int)(maxLength * g.MeasureString("▯", box.Font).Width * 1.5);

            return box;
        }

        /// <summary>
        /// Text box pre-wired with autocomplete from the given source array.
        /// </summary>
        protected UiTextBox CreateAutoFillTextBox(string text, int maxLength, string[] source)
        {
            var box = CreateTextBox(text, maxLength);
            box.AutoCompleteMode   = AutoCompleteMode.Append;
            box.AutoCompleteSource = AutoCompleteSource.CustomSource;
            var collection = new AutoCompleteStringCollection();
            if (source != null)
                collection.AddRange(source);
            box.AutoCompleteCustomSource = collection;
            box.TextAlign = HorizontalAlignment.Center;
            return box;
        }

        /// <summary>
        /// Multi-line text box that expands vertically as the user types.
        /// </summary>
        protected UiTextBox CreateMultiLineBox(
            string text,
            int    containerWidth,
            int    maxLength    = 255,
            string placeholder  = null)
        {
            var box = new UiTextBox
            {
                BackColor       = Theme.BackColor,
                ForeColor       = Theme.ForeColor,
                Font            = Theme.TextFontBold,
                BorderStyle     = BorderStyle.None,
                Width           = containerWidth - 50,
                Multiline       = true,
                WordWrap        = true,
                PlaceholderText = placeholder ?? "",
                Text            = text,
                MaxLength       = maxLength,
                Height          = 30,
                TabIndex        = 0,
                CharacterCasing = CharacterCasing.Upper,
                Padding         = new Padding(3, 0, 3, -10),
                Margin          = new Padding(3, 5, 3, -10),
                TextAlign       = HorizontalAlignment.Left
            };
            box.TextChanged += ExpandMultiLineBox;
            return box;
        }

        /// <summary>
        /// Radio-style checkbox — only one per group can be checked at a time.
        /// </summary>
        protected UiCheckBox CreateCheckBox(string text, string group)
        {
            var box = new UiCheckBox(group)
            {
                BackColor = Theme.BackColor,
                ForeColor = Theme.ForeColor,
                Font      = Theme.TextFont,
                Text      = text,
                Padding   = new Padding(3, 10, 10, -30),
                AutoSize  = true,
                TabIndex  = 0
            };
            box.Click += DeselectSiblingCheckBoxes;
            return box;
        }

        // ── Shared event handlers ────────────────────────────────────────────

        private static void NumsOnly(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private static void ExpandMultiLineBox(object sender, EventArgs e)
        {
            var box = (TextBox)sender;
            const int padding = 3;
            var numLines = box.GetLineFromCharIndex(box.TextLength) + 1;
            var border = box.Height - box.ClientSize.Height;
            box.Height = box.Font.Height * numLines + padding + border;
        }

        private static void DeselectSiblingCheckBoxes(object sender, EventArgs e)
        {
            var clicked = (UiCheckBox)sender;
            if (clicked.Parent == null) return;
            foreach (var box in clicked.Parent.Controls.OfType<UiCheckBox>())
            {
                if (box != clicked && box.group == clicked.group)
                    box.Checked = false;
            }
        }
    }
}