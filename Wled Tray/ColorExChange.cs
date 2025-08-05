using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wled_Tray
{
    internal class ColorExChange
    {
        public static int RgbToHue255(byte r, byte g, byte b)
        {
            double rNorm = r / 255.0;
            double gNorm = g / 255.0;
            double bNorm = b / 255.0;

            double max = Math.Max(rNorm, Math.Max(gNorm, bNorm));
            double min = Math.Min(rNorm, Math.Min(gNorm, bNorm));
            double delta = max - min;

            double hue = 0;

            if (delta == 0)
                hue = 0;
            else if (max == rNorm)
                hue = 60 * (((gNorm - bNorm) / delta) % 6);
            else if (max == gNorm)
                hue = 60 * (((bNorm - rNorm) / delta) + 2);
            else if (max == bNorm)
                hue = 60 * (((rNorm - gNorm) / delta) + 4);

            if (hue < 0)
                hue += 360;

            int hue255 = (int)Math.Round((hue / 360.0) * 255);
            return hue255;
        }
        public static (byte R, byte G, byte B) Hue255ToRgb(int hue255)
        {
            // Переводим 0-255 в 0-360 градусов
            double hue = (hue255 % 256) * 360.0 / 255.0;

            double c = 1.0;
            double x = 1.0 - Math.Abs((hue / 60.0) % 2 - 1);
            double r = 0, g = 0, b = 0;

            if (hue < 60)
            {
                r = c; g = x;
            }
            else if (hue < 120)
            {
                r = x; g = c;
            }
            else if (hue < 180)
            {
                g = c; b = x;
            }
            else if (hue < 240)
            {
                g = x; b = c;
            }
            else if (hue < 300)
            {
                r = x; b = c;
            }
            else
            {
                r = c; b = x;
            }

            // Приводим к диапазону 0–255
            return (
                R: (byte)(r * 255),
                G: (byte)(g * 255),
                B: (byte)(b * 255)
            );
        }
    }
}
