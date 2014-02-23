using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Client.UI.Base.Enums;
using Client.UI.Base.Render;
using Client.UI.Base.Utils;

namespace Client.UI.Base.Controls
{
    public sealed class ControlPaintEx
    {
        private ControlPaintEx()
        {
        }

        public static void DrawCheckedFlag(Graphics g, Rectangle rect, Color color)
        {
            PointF[] points = new PointF[3]
      {
        new PointF((float) rect.X + (float) rect.Width / 4.5f, (float) rect.Y + (float) rect.Height / 2.5f),
        new PointF((float) rect.X + (float) rect.Width / 2.5f, (float) rect.Bottom - (float) rect.Height / 3f),
        new PointF((float) rect.Right - (float) rect.Width / 4f, (float) rect.Y + (float) rect.Height / 4.5f)
      };
            using (Pen pen = new Pen(color, 2f))
                g.DrawLines(pen, points);
        }

        public static void DrawGlass(Graphics g, RectangleF glassRect, int alphaCenter, int alphaSurround)
        {
            ControlPaintEx.DrawGlass(g, glassRect, Color.White, alphaCenter, alphaSurround);
        }

        public static void DrawGlass(Graphics g, RectangleF glassRect, Color glassColor, int alphaCenter, int alphaSurround)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(glassRect);
                using (PathGradientBrush pathGradientBrush = new PathGradientBrush(path))
                {
                    pathGradientBrush.CenterColor = Color.FromArgb(alphaCenter, glassColor);
                    pathGradientBrush.SurroundColors = new Color[1]
          {
            Color.FromArgb(alphaSurround, glassColor)
          };
                    pathGradientBrush.CenterPoint = new PointF(glassRect.X + glassRect.Width / 2f, glassRect.Y + glassRect.Height / 2f);
                    g.FillPath((Brush)pathGradientBrush, path);
                }
            }
        }

        public static void DrawBackgroundImage(Graphics g, Image backgroundImage, Color backColor, ImageLayout backgroundImageLayout, Rectangle bounds, Rectangle clipRect)
        {
            ControlPaintEx.DrawBackgroundImage(g, backgroundImage, backColor, backgroundImageLayout, bounds, clipRect, Point.Empty, RightToLeft.No);
        }

        public static void DrawBackgroundImage(Graphics g, Image backgroundImage, Color backColor, ImageLayout backgroundImageLayout, Rectangle bounds, Rectangle clipRect, Point scrollOffset)
        {
            ControlPaintEx.DrawBackgroundImage(g, backgroundImage, backColor, backgroundImageLayout, bounds, clipRect, scrollOffset, RightToLeft.No);
        }

        public static void DrawBackgroundImage(Graphics g, Image backgroundImage, Color backColor, ImageLayout backgroundImageLayout, Rectangle bounds, Rectangle clipRect, Point scrollOffset, RightToLeft rightToLeft)
        {
            if (g == null)
                throw new ArgumentNullException("g");
            if (backgroundImageLayout == ImageLayout.Tile)
            {
                using (TextureBrush textureBrush = new TextureBrush(backgroundImage, WrapMode.Tile))
                {
                    if (scrollOffset != Point.Empty)
                    {
                        Matrix transform = textureBrush.Transform;
                        transform.Translate((float)scrollOffset.X, (float)scrollOffset.Y);
                        textureBrush.Transform = transform;
                    }
                    g.FillRectangle((Brush)textureBrush, clipRect);
                }
            }
            else
            {
                Rectangle rectangle1 = ControlPaintEx.CalculateBackgroundImageRectangle(bounds, backgroundImage, backgroundImageLayout);
                if (rightToLeft == RightToLeft.Yes && backgroundImageLayout == ImageLayout.None)
                    rectangle1.X += clipRect.Width - rectangle1.Width;
                using (SolidBrush solidBrush = new SolidBrush(backColor))
                    g.FillRectangle((Brush)solidBrush, clipRect);
                if (!clipRect.Contains(rectangle1))
                {
                    if (backgroundImageLayout != ImageLayout.Stretch && backgroundImageLayout != ImageLayout.Zoom)
                    {
                        if (backgroundImageLayout == ImageLayout.None)
                        {
                            rectangle1.Offset(clipRect.Location);
                            Rectangle destRect = rectangle1;
                            destRect.Intersect(clipRect);
                            Rectangle rectangle2 = new Rectangle(Point.Empty, destRect.Size);
                            g.DrawImage(backgroundImage, destRect, rectangle2.X, rectangle2.Y, rectangle2.Width, rectangle2.Height, GraphicsUnit.Pixel);
                        }
                        else
                        {
                            Rectangle destRect = rectangle1;
                            destRect.Intersect(clipRect);
                            Rectangle rectangle2 = new Rectangle(new Point(destRect.X - rectangle1.X, destRect.Y - rectangle1.Y), destRect.Size);
                            g.DrawImage(backgroundImage, destRect, rectangle2.X, rectangle2.Y, rectangle2.Width, rectangle2.Height, GraphicsUnit.Pixel);
                        }
                    }
                    else
                    {
                        rectangle1.Intersect(clipRect);
                        g.DrawImage(backgroundImage, rectangle1);
                    }
                }
                else
                {
                    ImageAttributes imageAttr = new ImageAttributes();
                    imageAttr.SetWrapMode(WrapMode.TileFlipXY);
                    g.DrawImage(backgroundImage, rectangle1, 0, 0, backgroundImage.Width, backgroundImage.Height, GraphicsUnit.Pixel, imageAttr);
                    imageAttr.Dispose();
                }
            }
        }

        public static void DrawScrollBarTrack(Graphics g, Rectangle rect, Color begin, Color end, Orientation orientation)
        {
            LinearGradientMode mode = orientation == Orientation.Horizontal ? LinearGradientMode.Vertical : LinearGradientMode.Horizontal;
            ControlPaintEx.DrawGradientRect(g, rect, begin, end, begin, begin, new Blend()
            {
                Factors = new float[3]
        {
          1f,
          0.5f,
          0.0f
        },
                Positions = new float[3]
        {
          0.0f,
          0.5f,
          1f
        }
            }, mode, true, false);
        }

        public static void DrawScrollBarThumb(Graphics g, Rectangle rect, Color begin, Color end, Color border, Color innerBorder, Orientation orientation, bool changeColor)
        {
            if (changeColor)
            {
                Color color = begin;
                begin = end;
                end = color;
            }
            bool flag;
            LinearGradientMode mode = (flag = orientation == Orientation.Horizontal) ? LinearGradientMode.Vertical : LinearGradientMode.Horizontal;
            Blend blend = new Blend();
            blend.Factors = new float[3]
      {
        1f,
        0.5f,
        0.0f
      };
            blend.Positions = new float[3]
      {
        0.0f,
        0.5f,
        1f
      };
            if (flag)
                rect.Inflate(0, -1);
            else
                rect.Inflate(-1, 0);
            ControlPaintEx.DrawGradientRoundRect(g, rect, begin, end, border, innerBorder, blend, mode, 4, RoundStyle.All, true, true);
        }

        public static void DrawScrollBarArraw(Graphics g, Rectangle rect, Color begin, Color end, Color border, Color innerBorder, Color fore, Orientation orientation, ArrowDirection arrowDirection, bool changeColor)
        {
            if (changeColor)
            {
                Color color = begin;
                begin = end;
                end = color;
            }
            LinearGradientMode mode = orientation == Orientation.Horizontal ? LinearGradientMode.Vertical : LinearGradientMode.Horizontal;
            rect.Inflate(-1, -1);
            ControlPaintEx.DrawGradientRoundRect(g, rect, begin, end, border, innerBorder, new Blend()
            {
                Factors = new float[3]
        {
          1f,
          0.5f,
          0.0f
        },
                Positions = new float[3]
        {
          0.0f,
          0.5f,
          1f
        }
            }, mode, 4, RoundStyle.All, true, true);
            using (SolidBrush solidBrush = new SolidBrush(fore))
                RenderHelper.RenderArrowInternal(g, rect, arrowDirection, (Brush)solidBrush);
        }

        public static void DrawScrollBarSizer(Graphics g, Rectangle rect, Color begin, Color end)
        {
            ControlPaintEx.DrawGradientRect(g, rect, begin, end, begin, begin, new Blend()
            {
                Factors = new float[3]
        {
          1f,
          0.5f,
          0.0f
        },
                Positions = new float[3]
        {
          0.0f,
          0.5f,
          1f
        }
            }, LinearGradientMode.Horizontal, true, false);
        }

        internal static void DrawGradientRect(Graphics g, Rectangle rect, Color begin, Color end, Color border, Color innerBorder, Blend blend, LinearGradientMode mode, bool drawBorder, bool drawInnerBorder)
        {
            using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rect, begin, end, mode))
            {
                linearGradientBrush.Blend = blend;
                g.FillRectangle((Brush)linearGradientBrush, rect);
            }
            if (drawBorder)
                ControlPaint.DrawBorder(g, rect, border, ButtonBorderStyle.Solid);
            if (!drawInnerBorder)
                return;
            rect.Inflate(-1, -1);
            ControlPaint.DrawBorder(g, rect, border, ButtonBorderStyle.Solid);
        }

        internal static void DrawGradientRoundRect(Graphics g, Rectangle rect, Color begin, Color end, Color border, Color innerBorder, Blend blend, LinearGradientMode mode, int radios, RoundStyle roundStyle, bool drawBorder, bool drawInnderBorder)
        {
            using (GraphicsPath path = GraphicsPathHelper.CreatePath(rect, radios, roundStyle, true))
            {
                using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rect, begin, end, mode))
                {
                    linearGradientBrush.Blend = blend;
                    g.FillPath((Brush)linearGradientBrush, path);
                }
                if (drawBorder)
                {
                    using (Pen pen = new Pen(border))
                        g.DrawPath(pen, path);
                }
            }
            if (!drawInnderBorder)
                return;
            rect.Inflate(-1, -1);
            using (GraphicsPath path = GraphicsPathHelper.CreatePath(rect, radios, roundStyle, true))
            {
                using (Pen pen = new Pen(innerBorder))
                    g.DrawPath(pen, path);
            }
        }

        internal static Rectangle CalculateBackgroundImageRectangle(Rectangle bounds, Image backgroundImage, ImageLayout imageLayout)
        {
            Rectangle rectangle = bounds;
            if (backgroundImage != null)
            {
                switch (imageLayout)
                {
                    case ImageLayout.None:
                        rectangle.Size = backgroundImage.Size;
                        return rectangle;
                    case ImageLayout.Tile:
                        return rectangle;
                    case ImageLayout.Center:
                        rectangle.Size = backgroundImage.Size;
                        Size size1 = bounds.Size;
                        if (size1.Width > rectangle.Width)
                            rectangle.X = (size1.Width - rectangle.Width) / 2;
                        if (size1.Height > rectangle.Height)
                            rectangle.Y = (size1.Height - rectangle.Height) / 2;
                        return rectangle;
                    case ImageLayout.Stretch:
                        rectangle.Size = bounds.Size;
                        return rectangle;
                    case ImageLayout.Zoom:
                        Size size2 = backgroundImage.Size;
                        float num1 = (float)bounds.Width / (float)size2.Width;
                        float num2 = (float)bounds.Height / (float)size2.Height;
                        if ((double)num1 >= (double)num2)
                        {
                            rectangle.Height = bounds.Height;
                            rectangle.Width = (int)((double)size2.Width * (double)num2 + 0.5);
                            if (bounds.X >= 0)
                                rectangle.X = (bounds.Width - rectangle.Width) / 2;
                            return rectangle;
                        }
                        else
                        {
                            rectangle.Width = bounds.Width;
                            rectangle.Height = (int)((double)size2.Height * (double)num1 + 0.5);
                            if (bounds.Y >= 0)
                                rectangle.Y = (bounds.Height - rectangle.Height) / 2;
                            return rectangle;
                        }
                }
            }
            return rectangle;
        }
    }
}
