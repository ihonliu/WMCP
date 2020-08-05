//    WMCP Windows media control panel
//    Copyright (c) 2020 - 2020 Ihon Liu
//    
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <https://www.gnu.org/licenses/>.
using System;
using System.Drawing;

namespace WMCP.Libs {
    public static class TrayIconFontConstructor {
        public static Icon GenerateFontIcon() {
            Font fontToUse = new Font("Segoe MDL2 Assets", 48, System.Drawing.FontStyle.Regular, GraphicsUnit.Pixel);
            System.Drawing.Brush brushToUse = new SolidBrush(System.Drawing.Color.White);
            Bitmap bitmapText = new Bitmap(48, 48);
            Graphics g = System.Drawing.Graphics.FromImage(bitmapText);

            g.Clear(System.Drawing.Color.Transparent);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            g.DrawString("\uE768", fontToUse, brushToUse, -4, 0);
            IntPtr hIcon = (bitmapText.GetHicon());
            return Icon.FromHandle(hIcon);
        }
    }
}
