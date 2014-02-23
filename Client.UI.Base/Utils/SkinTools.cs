using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Client.Core.Win32;
using System.Drawing.Drawing2D;
using Client.UI.Base.Enums;
using Client.UI.Base.Imaging;
using System.Drawing.Text;
using Client.UI.Base.Controls;

namespace Client.UI.Base.Utils
{
    public class SkinTools
    {
        public static Bitmap GaryImg(Bitmap b)
        {
            Bitmap bitmap = b.Clone(new Rectangle(0, 0, b.Width, b.Height), PixelFormat.Format24bppRgb);
            b.Dispose();
            BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            byte[] numArray = new byte[bitmap.Height * bitmapdata.Stride];
            Marshal.Copy(bitmapdata.Scan0, numArray, 0, numArray.Length);
            int num1 = 0;
            for (int width = bitmap.Width; num1 < width; ++num1)
            {
                int num2 = 0;
                for (int height = bitmap.Height; num2 < height; ++num2)
                    numArray[num2 * bitmapdata.Stride + num1 * 3] = numArray[num2 * bitmapdata.Stride + num1 * 3 + 1] = numArray[num2 * bitmapdata.Stride + num1 * 3 + 2] = SkinTools.GetAvg(numArray[num2 * bitmapdata.Stride + num1 * 3], numArray[num2 * bitmapdata.Stride + num1 * 3 + 1], numArray[num2 * bitmapdata.Stride + num1 * 3 + 2]);
            }
            Marshal.Copy(numArray, 0, bitmapdata.Scan0, numArray.Length);
            bitmap.UnlockBits(bitmapdata);
            return bitmap;
        }

        private static byte GetAvg(byte b, byte g, byte r)
        {
            return (byte)(((int)r + (int)g + (int)b) / 3);
        }

        public static Color GetImageAverageColor(Bitmap back)
        {
            return BitmapHelper.GetImageAverageColor(back);
        }

        public static bool ColorSlantsDarkOrBright(Color c)
        {
            HSL hsl = ColorConverterEx.smethod_0(c);
            if (hsl.Luminance < 0.15 || hsl.Luminance < 0.35)
                return true;
            return hsl.Luminance < 0.85 ? false : false;
        }

        public static void CreateRegion(Control ctrl, int RgnRadius)
        {
            int roundRectRgn = NativeMethods.CreateRoundRectRgn(0, 0, ctrl.ClientRectangle.Width + 1, ctrl.ClientRectangle.Height + 1, RgnRadius, RgnRadius);
            NativeMethods.SetWindowRgn(ctrl.Handle, roundRectRgn, true);
        }

        public static void CreateRegion(Control control, Rectangle bounds, int radius, RoundStyle roundStyle)
        {
            using (GraphicsPath path = GraphicsPathHelper.CreatePath(bounds, radius, roundStyle, true))
            {
                Region region = new Region(path);
                path.Widen(Pens.White);
                region.Union(path);
                control.Region = region;
            }
        }

        public static void CreateRegion(Control control, Rectangle bounds)
        {
            SkinTools.CreateRegion(control, bounds, 8, RoundStyle.All);
        }

        public static void CreateRegion(IntPtr hWnd, int radius, RoundStyle roundStyle, bool redraw)
        {

            Client.Core.Win32.RECT lpRect = new Client.Core.Win32.RECT();
            NativeMethods.GetWindowRect(hWnd, ref lpRect);
            Rectangle rect = new Rectangle(System.Drawing.Point.Empty, lpRect.Size);
            if (roundStyle != RoundStyle.None)
            {
                using (GraphicsPath path = GraphicsPathHelper.CreatePath(rect, radius, roundStyle, true))
                {
                    using (Region region = new Region(path))
                    {
                        path.Widen(Pens.White);
                        region.Union(path);
                        IntPtr windowDc = NativeMethods.GetWindowDC(hWnd);
                        try
                        {
                            using (Graphics g = Graphics.FromHdc(windowDc))
                                NativeMethods.SetWindowRgn(hWnd, region.GetHrgn(g), redraw);
                        }
                        finally
                        {
                            NativeMethods.ReleaseDC(hWnd, windowDc);
                        }
                    }
                }
            }
            else
            {
                IntPtr rectRgn = NativeMethods.CreateRectRgn(0, 0, rect.Width, rect.Height);
                NativeMethods.SetWindowRgn(hWnd, rectRgn, redraw);
            }
        }

        public static Bitmap BothAlpha(Bitmap p_Bitmap, bool p_CentralTransparent, bool p_Crossdirection)
        {
            Bitmap bitmap1 = new Bitmap(p_Bitmap.Width, p_Bitmap.Height);
            Graphics graphics1 = Graphics.FromImage((Image)bitmap1);
            graphics1.DrawImage((Image)p_Bitmap, new Rectangle(0, 0, p_Bitmap.Width, p_Bitmap.Height));
            graphics1.Dispose();
            Bitmap bitmap2 = new Bitmap(bitmap1.Width, bitmap1.Height);
            Graphics graphics2 = Graphics.FromImage((Image)bitmap2);
            System.Drawing.Point point1 = new System.Drawing.Point(0, 0);
            System.Drawing.Point point2 = new System.Drawing.Point(bitmap2.Width, 0);
            System.Drawing.Point point3 = new System.Drawing.Point(bitmap2.Width, bitmap2.Height / 2);
            System.Drawing.Point point4 = new System.Drawing.Point(0, bitmap2.Height / 2);
            if (p_Crossdirection)
            {
                point1 = new System.Drawing.Point(0, 0);
                point2 = new System.Drawing.Point(bitmap2.Width / 2, 0);
                point3 = new System.Drawing.Point(bitmap2.Width / 2, bitmap2.Height);
                point4 = new System.Drawing.Point(0, bitmap2.Height);
            }
            PathGradientBrush pathGradientBrush = new PathGradientBrush(new System.Drawing.Point[4]
      {
        point1,
        point2,
        point3,
        point4
      }, WrapMode.TileFlipY);
            pathGradientBrush.CenterPoint = new PointF(0.0f, 0.0f);
            pathGradientBrush.FocusScales = new PointF((float)(bitmap2.Width / 2), 0.0f);
            pathGradientBrush.CenterColor = Color.FromArgb(0, (int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue);
            pathGradientBrush.SurroundColors = new Color[1]
      {
        Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)
      };
            if (p_Crossdirection)
            {
                pathGradientBrush.FocusScales = new PointF(0.0f, (float)bitmap2.Height);
                pathGradientBrush.WrapMode = WrapMode.TileFlipX;
            }
            if (p_CentralTransparent)
            {
                pathGradientBrush.CenterColor = Color.FromArgb((int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue);
                pathGradientBrush.SurroundColors = new Color[1]
        {
          Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)
        };
            }
            graphics2.FillRectangle((Brush)pathGradientBrush, new Rectangle(0, 0, bitmap2.Width, bitmap2.Height));
            graphics2.Dispose();
            BitmapData bitmapdata1 = bitmap2.LockBits(new Rectangle(0, 0, bitmap2.Width, bitmap2.Height), ImageLockMode.ReadOnly, bitmap2.PixelFormat);
            byte[] destination = new byte[bitmapdata1.Stride * bitmapdata1.Height];
            Marshal.Copy(bitmapdata1.Scan0, destination, 0, destination.Length);
            bitmap2.UnlockBits(bitmapdata1);
            BitmapData bitmapdata2 = bitmap1.LockBits(new Rectangle(0, 0, bitmap1.Width, bitmap1.Height), ImageLockMode.ReadWrite, bitmap1.PixelFormat);
            byte[] numArray = new byte[bitmapdata2.Stride * bitmapdata2.Height];
            Marshal.Copy(bitmapdata2.Scan0, numArray, 0, numArray.Length);
            for (int index1 = 0; index1 != bitmapdata2.Height; ++index1)
            {
                int index2 = index1 * bitmapdata2.Stride + 3;
                for (int index3 = 0; index3 != bitmapdata2.Width; ++index3)
                {
                    numArray[index2] = destination[index2];
                    index2 += 4;
                }
            }
            Marshal.Copy(numArray, 0, bitmapdata2.Scan0, numArray.Length);
            bitmap1.UnlockBits(bitmapdata2);
            return bitmap1;
        }

        public static Image ImageLightEffect(string Str, Font F, Color ColorFore, Color ColorBack, int BlurConsideration)
        {
            Bitmap bitmap1 = (Bitmap)null;
            using (Graphics graphics1 = Graphics.FromHwnd(IntPtr.Zero))
            {
                SizeF sizeF = graphics1.MeasureString(Str, F);
                using (Bitmap bitmap2 = new Bitmap((int)sizeF.Width, (int)sizeF.Height))
                {
                    using (Graphics graphics2 = Graphics.FromImage((Image)bitmap2))
                    {
                        using (SolidBrush solidBrush1 = new SolidBrush(Color.FromArgb(16, (int)ColorBack.R, (int)ColorBack.G, (int)ColorBack.B)))
                        {
                            using (SolidBrush solidBrush2 = new SolidBrush(ColorFore))
                            {
                                graphics2.SmoothingMode = SmoothingMode.HighQuality;
                                graphics2.InterpolationMode = InterpolationMode.HighQualityBilinear;
                                graphics2.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                                graphics2.DrawString(Str, F, (Brush)solidBrush1, 0.0f, 0.0f);
                                bitmap1 = new Bitmap(bitmap2.Width + BlurConsideration, bitmap2.Height + BlurConsideration);
                                using (Graphics graphics3 = Graphics.FromImage((Image)bitmap1))
                                {
                                    graphics3.SmoothingMode = SmoothingMode.HighQuality;
                                    graphics3.InterpolationMode = InterpolationMode.HighQualityBilinear;
                                    graphics3.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                                    for (int x = 0; x <= BlurConsideration; ++x)
                                    {
                                        for (int y = 0; y <= BlurConsideration; ++y)
                                            graphics3.DrawImageUnscaled((Image)bitmap2, x, y);
                                    }
                                    graphics3.DrawString(Str, F, (Brush)solidBrush2, (float)(BlurConsideration / 2), (float)(BlurConsideration / 2));
                                }
                            }
                        }
                    }
                }
            }
            return (Image)bitmap1;
        }

        public static Image ImageLightEffect(string Str, Font F, Color ColorFore, Color ColorBack, int BlurConsideration, Rectangle rc, bool auto)
        {
            Bitmap bitmap1 = (Bitmap)null;
            StringFormat format = new StringFormat(StringFormatFlags.NoWrap);
            format.Trimming = auto ? StringTrimming.EllipsisWord : StringTrimming.None;
            using (Graphics graphics1 = Graphics.FromHwnd(IntPtr.Zero))
            {
                SizeF sizeF = graphics1.MeasureString(Str, F);
                using (Bitmap bitmap2 = new Bitmap((int)sizeF.Width, (int)sizeF.Height))
                {
                    using (Graphics graphics2 = Graphics.FromImage((Image)bitmap2))
                    {
                        using (SolidBrush solidBrush1 = new SolidBrush(Color.FromArgb(16, (int)ColorBack.R, (int)ColorBack.G, (int)ColorBack.B)))
                        {
                            using (SolidBrush solidBrush2 = new SolidBrush(ColorFore))
                            {
                                graphics2.SmoothingMode = SmoothingMode.HighQuality;
                                graphics2.InterpolationMode = InterpolationMode.HighQualityBilinear;
                                graphics2.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                                graphics2.DrawString(Str, F, (Brush)solidBrush1, (RectangleF)rc, format);
                                bitmap1 = new Bitmap(bitmap2.Width + BlurConsideration, bitmap2.Height + BlurConsideration);
                                using (Graphics graphics3 = Graphics.FromImage((Image)bitmap1))
                                {
                                    if (ColorBack != Color.Transparent)
                                    {
                                        graphics3.SmoothingMode = SmoothingMode.HighQuality;
                                        graphics3.InterpolationMode = InterpolationMode.HighQualityBilinear;
                                        graphics3.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                                        for (int x = 0; x <= BlurConsideration; ++x)
                                        {
                                            for (int y = 0; y <= BlurConsideration; ++y)
                                                graphics3.DrawImageUnscaled((Image)bitmap2, x, y);
                                        }
                                    }
                                    graphics3.DrawString(Str, F, (Brush)solidBrush2, (RectangleF)new Rectangle(new System.Drawing.Point(Convert.ToInt32(BlurConsideration / 2), Convert.ToInt32(BlurConsideration / 2)), rc.Size), format);
                                }
                            }
                        }
                    }
                }
            }
            return (Image)bitmap1;
        }

        public static void CursorClick(int x, int y)
        {
            NativeMethods.mouse_event(2, x * 65536 / 1024, y * 65536 / 768, 0, 0);
            NativeMethods.mouse_event(4, x * 65536 / 1024, y * 65536 / 768, 0, 0);
        }

        public static Bitmap ResizeBitmap(Bitmap b, int dstWidth, int dstHeight)
        {
            Bitmap bitmap = new Bitmap(dstWidth, dstHeight);
            Graphics graphics = Graphics.FromImage((Image)bitmap);
            graphics.InterpolationMode = InterpolationMode.Bilinear;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.DrawImage((Image)b, new Rectangle(0, 0, bitmap.Width, bitmap.Height), new Rectangle(0, 0, b.Width, b.Height), GraphicsUnit.Pixel);
            graphics.Save();
            graphics.Dispose();
            return bitmap;
        }

        public static void CreateControlRegion(Control control, Bitmap bitmap, int Alpha)
        {
            if (control == null || bitmap == null)
                return;
            control.Width = bitmap.Width;
            control.Height = bitmap.Height;
            if (control is Form)
            {
                Form form = (Form)control;
                form.Width = control.Width;
                form.Height = control.Height;
                form.FormBorderStyle = FormBorderStyle.None;
                form.BackgroundImage = (Image)bitmap;
                GraphicsPath path = SkinTools.CalculateControlGraphicsPath(bitmap, Alpha);
                form.Region = new Region(path);
            }
            else if (control is SkinButton)
            {
                control.Region = new Region(SkinTools.CalculateControlGraphicsPath(bitmap, Alpha));
            }
            else
            {
                if (!(control is SkinProgressBar))
                    return;
                control.Region = new Region(SkinTools.CalculateControlGraphicsPath(bitmap, Alpha));
            }
        }

        public static GraphicsPath CalculateControlGraphicsPath(Bitmap bitmap, int Alpha)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            int num = 0;
            for (int y = 0; y < bitmap.Height; ++y)
            {
                num = 0;
                for (int x1 = 0; x1 < bitmap.Width; ++x1)
                {
                    if ((int)bitmap.GetPixel(x1, y).A >= Alpha)
                    {
                        int x2 = x1;
                        int x3 = x2;
                        while (x3 < bitmap.Width && (int)bitmap.GetPixel(x3, y).A >= Alpha)
                            ++x3;
                        graphicsPath.AddRectangle(new Rectangle(x2, y, x3 - x2, 1));
                        x1 = x3;
                    }
                }
            }
            return graphicsPath;
        }
    }
}
