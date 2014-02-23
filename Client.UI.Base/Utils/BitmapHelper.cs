using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Imaging;
using System.Security.Permissions;
using System.Drawing;
namespace Client.UI.Base.Utils
{
    public static class BitmapHelper
    {
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static unsafe Color GetImageAverageColor(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                throw new ArgumentNullException("bitmap");
            }
            int width = bitmap.Width;
            int height = bitmap.Height;
            Rectangle rect = new Rectangle(0, 0, width, height);
            try
            {
                BitmapData bitmapdata = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                byte* numPtr = (byte*)bitmapdata.Scan0;
                int num3 = bitmapdata.Stride - (bitmapdata.Width * 4);
                int num4 = width * height;
                int num5 = 0;
                int red = 0;
                int green = 0;
                int blue = 0;
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        numPtr++;
                        blue += numPtr[0];
                        numPtr++;
                        green += numPtr[0];
                        numPtr++;
                        red += numPtr[0];
                        numPtr++;
                        num5 += numPtr[0];
                    }
                    numPtr += num3;
                }
                bitmap.UnlockBits(bitmapdata);
                num5 /= num4;
                red /= num4;
                green /= num4;
                blue /= num4;
                return Color.FromArgb(0xff, red, green, blue);
            }
            catch
            {
                return Color.FromArgb(0x7f, 0x7f, 0x7f);
            }
        }
    }
}
