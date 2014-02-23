using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Client.UI.Base.Enums;

namespace Client.UI.Base.Utils
{
    internal class RenderHelperStrip
    {
        internal static void RenderBackgroundInternal(Graphics g, Rectangle rect, Color baseColor, Color borderColor, Color innerBorderColor, RoundStyle style, bool drawBorder, bool drawGlass, LinearGradientMode mode)
        {
            RenderHelperStrip.RenderBackgroundInternal(g, rect, baseColor, borderColor, innerBorderColor, style, 8, drawBorder, drawGlass, mode);
        }

        internal static void RenderBackgroundInternal(Graphics g, Rectangle rect, Color baseColor, Color borderColor, Color innerBorderColor, RoundStyle style, int roundWidth, bool drawBorder, bool drawGlass, LinearGradientMode mode)
        {
            RenderHelperStrip.RenderBackgroundInternal(g, rect, baseColor, borderColor, innerBorderColor, style, roundWidth, 0.45f, drawBorder, drawGlass, mode);
        }

        internal static void RenderBackgroundInternal(Graphics g, Rectangle rect, Color baseColor, Color borderColor, Color innerBorderColor, RoundStyle style, int roundWidth, float basePosition, bool drawBorder, bool drawGlass, LinearGradientMode mode)
        {
            if (drawBorder)
            {
                --rect.Width;
                --rect.Height;
            }
            using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rect, Color.Transparent, Color.Transparent, mode))
            {
                Color[] colorArray = new Color[4]
        {
          RenderHelperStrip.GetColor(baseColor, 0, 35, 24, 9),
          RenderHelperStrip.GetColor(baseColor, 0, 13, 8, 3),
          baseColor,
          RenderHelperStrip.GetColor(baseColor, 0, 35, 24, 9)
        };
                linearGradientBrush.InterpolationColors = new ColorBlend()
                {
                    Positions = new float[4]
          {
            0.0f,
            basePosition,
            basePosition + 0.05f,
            1f
          },
                    Colors = colorArray
                };
                if (style != RoundStyle.None)
                {
                    using (GraphicsPath path = GraphicsPathHelper.CreatePath(rect, roundWidth, style, false))
                        g.FillPath((Brush)linearGradientBrush, path);
                    if (drawGlass)
                    {
                        RectangleF glassRect = (RectangleF)rect;
                        if (mode == LinearGradientMode.Vertical)
                        {
                            glassRect.Y = (float)rect.Y + (float)rect.Height * basePosition;
                            glassRect.Height = (float)(((double)rect.Height - (double)rect.Height * (double)basePosition) * 2.0);
                        }
                        else
                        {
                            glassRect.X = (float)rect.X + (float)rect.Width * basePosition;
                            glassRect.Width = (float)(((double)rect.Width - (double)rect.Width * (double)basePosition) * 2.0);
                        }
                        ControlPaintEx.DrawGlass(g, glassRect, 170, 0);
                        if ((int)baseColor.A > 0)
                        {
                            Rectangle rect1 = rect;
                            if (mode == LinearGradientMode.Vertical)
                                rect1.Height = (int)((double)rect1.Height * (double)basePosition);
                            else
                                rect1.Width = (int)((double)rect.Width * (double)basePosition);
                            using (GraphicsPath path = GraphicsPathHelper.CreatePath(rect1, roundWidth, RoundStyle.Top, false))
                            {
                                using (SolidBrush solidBrush = new SolidBrush(Color.FromArgb(128, (int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue)))
                                    g.FillPath((Brush)solidBrush, path);
                            }
                        }
                    }
                    if (!drawBorder)
                        return;
                    using (GraphicsPath path = GraphicsPathHelper.CreatePath(rect, roundWidth, style, false))
                    {
                        using (Pen pen = new Pen(borderColor))
                            g.DrawPath(pen, path);
                    }
                    rect.Inflate(-1, -1);
                    using (GraphicsPath path = GraphicsPathHelper.CreatePath(rect, roundWidth, style, false))
                    {
                        using (Pen pen = new Pen(innerBorderColor))
                            g.DrawPath(pen, path);
                    }
                }
                else
                {
                    g.FillRectangle((Brush)linearGradientBrush, rect);
                    if (drawGlass)
                    {
                        RectangleF glassRect = (RectangleF)rect;
                        if (mode == LinearGradientMode.Vertical)
                        {
                            glassRect.Y = (float)rect.Y + (float)rect.Height * basePosition;
                            glassRect.Height = (float)(((double)rect.Height - (double)rect.Height * (double)basePosition) * 2.0);
                        }
                        else
                        {
                            glassRect.X = (float)rect.X + (float)rect.Width * basePosition;
                            glassRect.Width = (float)(((double)rect.Width - (double)rect.Width * (double)basePosition) * 2.0);
                        }
                        ControlPaintEx.DrawGlass(g, glassRect, 200, 0);
                        if ((int)baseColor.A > 80)
                        {
                            Rectangle rect1 = rect;
                            if (mode == LinearGradientMode.Vertical)
                                rect1.Height = (int)((double)rect1.Height * (double)basePosition);
                            else
                                rect1.Width = (int)((double)rect.Width * (double)basePosition);
                            using (SolidBrush solidBrush = new SolidBrush(Color.FromArgb(128, (int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue)))
                                g.FillRectangle((Brush)solidBrush, rect1);
                        }
                    }
                    if (!drawBorder)
                        return;
                    using (Pen pen = new Pen(borderColor))
                        g.DrawRectangle(pen, rect);
                    rect.Inflate(-1, -1);
                    using (Pen pen = new Pen(innerBorderColor))
                        g.DrawRectangle(pen, rect);
                }
            }
        }

        internal static void RenderArrowInternal(Graphics g, Rectangle dropDownRect, ArrowDirection direction, Brush brush)
        {
            Point point = new Point(dropDownRect.Left + dropDownRect.Width / 2, dropDownRect.Top + dropDownRect.Height / 2);
            Point[] points;
            switch (direction)
            {
                case ArrowDirection.Left:
                    points = new Point[3]
          {
            new Point(point.X + 2, point.Y - 3),
            new Point(point.X + 2, point.Y + 3),
            new Point(point.X - 1, point.Y)
          };
                    break;
                case ArrowDirection.Up:
                    points = new Point[3]
          {
            new Point(point.X - 3, point.Y + 2),
            new Point(point.X + 3, point.Y + 2),
            new Point(point.X, point.Y - 2)
          };
                    break;
                case ArrowDirection.Right:
                    points = new Point[3]
          {
            new Point(point.X - 2, point.Y - 3),
            new Point(point.X - 2, point.Y + 3),
            new Point(point.X + 1, point.Y)
          };
                    break;
                default:
                    points = new Point[3]
          {
            new Point(point.X - 3, point.Y - 1),
            new Point(point.X + 3, point.Y - 1),
            new Point(point.X, point.Y + 2)
          };
                    break;
            }
            g.FillPolygon(brush, points);
        }

        internal static Color GetColor(Color colorBase, int a, int r, int g, int b)
        {
            int num1 = (int)colorBase.A;
            int num2 = (int)colorBase.R;
            int num3 = (int)colorBase.G;
            int num4 = (int)colorBase.B;
            a = a + num1 <= (int)byte.MaxValue ? Math.Max(0, a + num1) : (int)byte.MaxValue;
            r = r + num2 <= (int)byte.MaxValue ? Math.Max(0, r + num2) : (int)byte.MaxValue;
            g = g + num3 <= (int)byte.MaxValue ? Math.Max(0, g + num3) : (int)byte.MaxValue;
            b = b + num4 <= (int)byte.MaxValue ? Math.Max(0, b + num4) : (int)byte.MaxValue;
            return Color.FromArgb(a, r, g, b);
        }
    }
}
