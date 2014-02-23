using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Client.UI.Base.Enums;

namespace Client.UI.Base.Imaging
{
    public sealed class ColorConverterEx
    {
        private static readonly int[] BT907 = new int[] { 0x84d, 0x1bf2, 0x2d1, 0x2710 };
        private static readonly int[] RMY = new int[] { 500, 0x1a3, 0x51, 0x3e8 };
        private static readonly int[] Y = new int[] { 0x12b, 0x24b, 0x72, 0x3e8 };

        private ColorConverterEx()
        {
        }

        private static byte GetGray(RGB rgb, int[] coefficient)
        {
            return (byte)((((rgb.R * coefficient[0]) + (rgb.G * coefficient[1])) + (rgb.B * coefficient[2])) / coefficient[3]);
        }

        public static RGB HslToRgb(HSL hsl)
        {
            RGB rgb = new RGB();
            HslToRgb(hsl, rgb);
            return rgb;
        }

        public static void HslToRgb(HSL hsl, RGB rgb)
        {
            if (hsl.Saturation == 0.0)
            {
                rgb.R = rgb.G = rgb.B = (byte)(hsl.Luminance * 255.0);
            }
            else
            {
                double vH = ((double)hsl.Hue) / 360.0;
                double num2 = (hsl.Luminance < 0.5) ? (hsl.Luminance * (1.0 + hsl.Saturation)) : ((hsl.Luminance + hsl.Saturation) - (hsl.Luminance * hsl.Saturation));
                double num = (2.0 * hsl.Luminance) - num2;
                rgb.R = (byte)(255.0 * HueToRGB(num, num2, vH + 0.33333333333333331));
                rgb.G = (byte)(255.0 * HueToRGB(num, num2, vH));
                rgb.B = (byte)(255.0 * HueToRGB(num, num2, vH - 0.33333333333333331));
            }
        }

        private static double HueToRGB(double v1, double v2, double vH)
        {
            if (vH < 0.0)
            {
                vH++;
            }
            if (vH > 1.0)
            {
                vH--;
            }
            if ((6.0 * vH) < 1.0)
            {
                return (v1 + (((v2 - v1) * 6.0) * vH));
            }
            if ((2.0 * vH) < 1.0)
            {
                return v2;
            }
            if ((3.0 * vH) < 2.0)
            {
                return (v1 + (((v2 - v1) * (0.66666666666666663 - vH)) * 6.0));
            }
            return v1;
        }

        public static RGB RgbToGray(RGB source)
        {
            RGB dest = new RGB();
            RgbToGray(source, dest);
            return dest;
        }

        public static RGB RgbToGray(RGB source, GrayscaleStyle style)
        {
            RGB dest = new RGB();
            RgbToGray(source, dest, style);
            return dest;
        }

        public static void RgbToGray(RGB source, RGB dest)
        {
            RgbToGray(source, dest, GrayscaleStyle.BT907);
        }

        public static void RgbToGray(RGB source, RGB dest, GrayscaleStyle style)
        {
            byte gray = 0x7f;
            switch (style)
            {
                case GrayscaleStyle.BT907:
                    gray = GetGray(source, BT907);
                    break;

                case GrayscaleStyle.RMY:
                    gray = GetGray(source, RMY);
                    break;

                case GrayscaleStyle.Y:
                    gray = GetGray(source, Y);
                    break;
            }
            dest.R = dest.G = dest.B = gray;
        }

        public static HSL RgbToHsl(RGB rgb)
        {
            HSL hsl = new HSL();
            RgbToHsl(rgb, hsl);
            return hsl;
        }

        public static void RgbToHsl(RGB rgb, HSL hsl)
        {
            double num = ((double)rgb.R) / 255.0;
            double num2 = ((double)rgb.G) / 255.0;
            double num3 = ((double)rgb.G) / 255.0;
            double num4 = Math.Min(Math.Min(num, num2), num3);
            double num5 = Math.Max(Math.Max(num, num2), num3);
            double num6 = num5 - num4;
            hsl.Luminance = (num5 + num4) / 2.0;
            if (num6 == 0.0)
            {
                hsl.Hue = 0;
                hsl.Saturation = 0.0;
            }
            else
            {
                double num10;
                hsl.Saturation = (hsl.Luminance < 0.5) ? (num6 / (num5 + num4)) : (num6 / ((2.0 - num5) - num4));
                double num7 = (((num5 - num) / 6.0) + (num6 / 2.0)) / num6;
                double num8 = (((num5 - num2) / 6.0) + (num6 / 2.0)) / num6;
                double num9 = (((num5 - num3) / 6.0) + (num6 / 2.0)) / num6;
                if (num == num5)
                {
                    num10 = num9 - num8;
                }
                else if (num2 == num5)
                {
                    num10 = (0.33333333333333331 + num7) - num9;
                }
                else
                {
                    num10 = (0.66666666666666663 + num8) - num7;
                }
                if (num10 < 0.0)
                {
                    num10++;
                }
                if (num10 > 1.0)
                {
                    num10--;
                }
                hsl.Hue = (int)(num10 * 360.0);
            }
        }

        public static HSL smethod_0(Color color)
        {
            int num;
            double num3;
            double num4 = ((double)color.R) / 255.0;
            double num5 = ((double)color.G) / 255.0;
            double num6 = ((double)color.B) / 255.0;
            double num7 = Math.Min(Math.Min(num4, num5), num6);
            double num8 = Math.Max(Math.Max(num4, num5), num6);
            double num9 = num8 - num7;
            double luminance = (num8 + num7) / 2.0;
            if (num9 == 0.0)
            {
                num = 0;
                num3 = 0.0;
            }
            else
            {
                double num13;
                num3 = (luminance < 0.5) ? (num9 / (num8 + num7)) : (num9 / ((2.0 - num8) - num7));
                double num10 = (((num8 - num4) / 6.0) + (num9 / 2.0)) / num9;
                double num11 = (((num8 - num5) / 6.0) + (num9 / 2.0)) / num9;
                double num12 = (((num8 - num6) / 6.0) + (num9 / 2.0)) / num9;
                if (num4 == num8)
                {
                    num13 = num12 - num11;
                }
                else if (num5 == num8)
                {
                    num13 = (0.33333333333333331 + num10) - num12;
                }
                else
                {
                    num13 = (0.66666666666666663 + num11) - num10;
                }
                if (num13 < 0.0)
                {
                    num13++;
                }
                if (num13 > 1.0)
                {
                    num13--;
                }
                num = (int)(num13 * 360.0);
            }
            return new HSL(num, num3, luminance);
        }

        public static Color smethod_1(HSL hsl)
        {
            byte num;
            byte num2;
            byte num3;
            if (hsl.Saturation == 0.0)
            {
                num = num2 = num3 = (byte)(hsl.Luminance * 255.0);
            }
            else
            {
                double vH = ((double)hsl.Hue) / 360.0;
                double num5 = (hsl.Luminance < 0.5) ? (hsl.Luminance * (1.0 + hsl.Saturation)) : ((hsl.Luminance + hsl.Saturation) - (hsl.Luminance * hsl.Saturation));
                double num4 = (2.0 * hsl.Luminance) - num5;
                num = (byte)(255.0 * HueToRGB(num4, num5, vH + 0.33333333333333331));
                num2 = (byte)(255.0 * HueToRGB(num4, num5, vH));
                num3 = (byte)(255.0 * HueToRGB(num4, num5, vH - 0.33333333333333331));
            }
            return Color.FromArgb(num, num2, num3);
        }
    }
}
