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

using System.Windows.Forms;

namespace EasyCPDLC;

public static class TableLayoutHelper
{
    public static void RemoveArbitraryRow(TableLayoutPanel panel, int rowIndex)
    {
        if (rowIndex >= panel.RowCount)
            return;

        for (int i = 0; i < panel.ColumnCount; i++)
            panel.Controls.Remove(panel.GetControlFromPosition(i, rowIndex));

        for (int i = rowIndex + 1; i < panel.RowCount; i++)
        {
            for (int j = 0; j < panel.ColumnCount; j++)
            {
                var control = panel.GetControlFromPosition(j, i);
                if (control != null)
                    panel.SetRow(control, i - 1);
            }
        }

        if (panel.RowStyles.Count > panel.RowCount - 1)
            panel.RowStyles.RemoveAt(panel.RowCount - 1);

        panel.RowCount--;
        panel.AutoScroll = false;
        panel.AutoScroll = true;
    }
}
