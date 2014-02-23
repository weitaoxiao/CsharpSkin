using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Client.UI.Base.Utils
{
    public class ImageDrawRect
    {
        public static ContentAlignment anyBottom = (ContentAlignment.BottomRight | ContentAlignment.BottomCenter | ContentAlignment.BottomLeft);
        public static ContentAlignment anyCenter = (ContentAlignment.BottomCenter | ContentAlignment.MiddleCenter | ContentAlignment.TopCenter);
        public static ContentAlignment anyMiddle = (ContentAlignment.MiddleRight | ContentAlignment.MiddleCenter | ContentAlignment.MiddleLeft);
        public static ContentAlignment anyRight = (ContentAlignment.BottomRight | ContentAlignment.MiddleRight | ContentAlignment.TopRight);
        public static ContentAlignment anyTop = (ContentAlignment.TopRight | ContentAlignment.TopCenter | ContentAlignment.TopLeft);

        public static void DrawRect(Graphics g, Bitmap img, Rectangle r, int index, int Totalindex)
        {
            if (img != null)
            {
                int width = img.Width / Totalindex;
                int height = img.Height;
                int x = (index - 1) * width;
                int y = 0;
                int left = r.Left;
                int top = r.Top;
                Rectangle srcRect = new Rectangle(x, y, width, height);
                Rectangle destRect = new Rectangle(left, top, r.Width, r.Height);
                g.DrawImage(img, destRect, srcRect, GraphicsUnit.Pixel);
            }
        }

        public static void DrawRect(Graphics g, Bitmap img, Rectangle r, Rectangle lr, int index, int Totalindex)
        {
            if (img != null)
            {
                Rectangle rectangle;
                Rectangle rectangle2;
                int x = ((index - 1) * img.Width) / Totalindex;
                int y = 0;
                int left = r.Left;
                int top = r.Top;
                if ((r.Height > img.Height) && (r.Width <= (img.Width / Totalindex)))
                {
                    rectangle = new Rectangle(x, y, img.Width / Totalindex, lr.Top);
                    rectangle2 = new Rectangle(left, top, r.Width, lr.Top);
                    g.DrawImage(img, rectangle2, rectangle, GraphicsUnit.Pixel);
                    rectangle = new Rectangle(x, y + lr.Top, img.Width / Totalindex, (img.Height - lr.Top) - lr.Bottom);
                    rectangle2 = new Rectangle(left, top + lr.Top, r.Width, (r.Height - lr.Top) - lr.Bottom);
                    if ((lr.Top + lr.Bottom) == 0)
                    {
                        rectangle.Height--;
                    }
                    g.DrawImage(img, rectangle2, rectangle, GraphicsUnit.Pixel);
                    rectangle = new Rectangle(x, (y + img.Height) - lr.Bottom, img.Width / Totalindex, lr.Bottom);
                    rectangle2 = new Rectangle(left, (top + r.Height) - lr.Bottom, r.Width, lr.Bottom);
                    g.DrawImage(img, rectangle2, rectangle, GraphicsUnit.Pixel);
                }
                else if ((r.Height <= img.Height) && (r.Width > (img.Width / Totalindex)))
                {
                    rectangle = new Rectangle(x, y, lr.Left, img.Height);
                    rectangle2 = new Rectangle(left, top, lr.Left, r.Height);
                    g.DrawImage(img, rectangle2, rectangle, GraphicsUnit.Pixel);
                    rectangle = new Rectangle(x + lr.Left, y, ((img.Width / Totalindex) - lr.Left) - lr.Right, img.Height);
                    rectangle2 = new Rectangle(left + lr.Left, top, (r.Width - lr.Left) - lr.Right, r.Height);
                    g.DrawImage(img, rectangle2, rectangle, GraphicsUnit.Pixel);
                    rectangle = new Rectangle((x + (img.Width / Totalindex)) - lr.Right, y, lr.Right, img.Height);
                    rectangle2 = new Rectangle((left + r.Width) - lr.Right, top, lr.Right, r.Height);
                    g.DrawImage(img, rectangle2, rectangle, GraphicsUnit.Pixel);
                }
                else if ((r.Height <= img.Height) && (r.Width <= (img.Width / Totalindex)))
                {
                    rectangle = new Rectangle(((index - 1) * img.Width) / Totalindex, 0, img.Width / Totalindex, img.Height - 1);
                    g.DrawImage(img, new Rectangle(left, top, r.Width, r.Height), rectangle, GraphicsUnit.Pixel);
                }
                else if ((r.Height > img.Height) && (r.Width > (img.Width / Totalindex)))
                {
                    rectangle = new Rectangle(x, y, lr.Left, lr.Top);
                    rectangle2 = new Rectangle(left, top, lr.Left, lr.Top);
                    g.DrawImage(img, rectangle2, rectangle, GraphicsUnit.Pixel);
                    rectangle = new Rectangle(x, (y + img.Height) - lr.Bottom, lr.Left, lr.Bottom);
                    rectangle2 = new Rectangle(left, (top + r.Height) - lr.Bottom, lr.Left, lr.Bottom);
                    g.DrawImage(img, rectangle2, rectangle, GraphicsUnit.Pixel);
                    rectangle = new Rectangle(x, y + lr.Top, lr.Left, (img.Height - lr.Top) - lr.Bottom);
                    rectangle2 = new Rectangle(left, top + lr.Top, lr.Left, (r.Height - lr.Top) - lr.Bottom);
                    g.DrawImage(img, rectangle2, rectangle, GraphicsUnit.Pixel);
                    rectangle = new Rectangle(x + lr.Left, y, ((img.Width / Totalindex) - lr.Left) - lr.Right, lr.Top);
                    rectangle2 = new Rectangle(left + lr.Left, top, (r.Width - lr.Left) - lr.Right, lr.Top);
                    g.DrawImage(img, rectangle2, rectangle, GraphicsUnit.Pixel);
                    rectangle = new Rectangle((x + (img.Width / Totalindex)) - lr.Right, y, lr.Right, lr.Top);
                    rectangle2 = new Rectangle((left + r.Width) - lr.Right, top, lr.Right, lr.Top);
                    g.DrawImage(img, rectangle2, rectangle, GraphicsUnit.Pixel);
                    rectangle = new Rectangle((x + (img.Width / Totalindex)) - lr.Right, y + lr.Top, lr.Right, (img.Height - lr.Top) - lr.Bottom);
                    rectangle2 = new Rectangle((left + r.Width) - lr.Right, top + lr.Top, lr.Right, (r.Height - lr.Top) - lr.Bottom);
                    g.DrawImage(img, rectangle2, rectangle, GraphicsUnit.Pixel);
                    rectangle = new Rectangle((x + (img.Width / Totalindex)) - lr.Right, (y + img.Height) - lr.Bottom, lr.Right, lr.Bottom);
                    rectangle2 = new Rectangle((left + r.Width) - lr.Right, (top + r.Height) - lr.Bottom, lr.Right, lr.Bottom);
                    g.DrawImage(img, rectangle2, rectangle, GraphicsUnit.Pixel);
                    rectangle = new Rectangle(x + lr.Left, (y + img.Height) - lr.Bottom, ((img.Width / Totalindex) - lr.Left) - lr.Right, lr.Bottom);
                    rectangle2 = new Rectangle(left + lr.Left, (top + r.Height) - lr.Bottom, (r.Width - lr.Left) - lr.Right, lr.Bottom);
                    g.DrawImage(img, rectangle2, rectangle, GraphicsUnit.Pixel);
                    rectangle = new Rectangle(x + lr.Left, y + lr.Top, ((img.Width / Totalindex) - lr.Left) - lr.Right, (img.Height - lr.Top) - lr.Bottom);
                    rectangle2 = new Rectangle(left + lr.Left, top + lr.Top, (r.Width - lr.Left) - lr.Right, (r.Height - lr.Top) - lr.Bottom);
                    g.DrawImage(img, rectangle2, rectangle, GraphicsUnit.Pixel);
                }
            }
        }

        public static Rectangle HAlignWithin(Size alignThis, Rectangle withinThis, ContentAlignment align)
        {
            if ((align & anyRight) != ((ContentAlignment)0))
            {
                withinThis.X += withinThis.Width - alignThis.Width;
            }
            else if ((align & anyCenter) != ((ContentAlignment)0))
            {
                withinThis.X += ((withinThis.Width - alignThis.Width) + 1) / 2;
            }
            withinThis.Width = alignThis.Width;
            return withinThis;
        }

        public static Rectangle VAlignWithin(Size alignThis, Rectangle withinThis, ContentAlignment align)
        {
            if ((align & anyBottom) != ((ContentAlignment)0))
            {
                withinThis.Y += withinThis.Height - alignThis.Height;
            }
            else if ((align & anyMiddle) != ((ContentAlignment)0))
            {
                withinThis.Y += ((withinThis.Height - alignThis.Height) + 1) / 2;
            }
            withinThis.Height = alignThis.Height;
            return withinThis;
        }
    }
}
