using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using Client.UI.Base.Forms;
using Client.UI.Base.Utils;
using Client.UI.Base.Enums;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Client.UI.Base.Controls;
using System.Collections;

namespace Client.UI.Base.Render
{
    public class FormSkinRenderer:SkinRendererBase
    {
        private FormSkinColorTable _colorTable;

        public FormSkinRenderer()
        { }
        public FormSkinRenderer(FormSkinColorTable colortable)
        {
            this._colorTable = colortable;
        }

        private GraphicsPath CreateCloseFlagPath(Rectangle rect)
        {
            PointF tf = new PointF(rect.X + (((float) rect.Width) / 2f), rect.Y + (((float) rect.Height) / 2f));
            GraphicsPath path = new GraphicsPath();
            path.AddLine(tf.X, tf.Y - 2f, tf.X - 2f, tf.Y - 4f);
            path.AddLine((float) (tf.X - 2f), (float) (tf.Y - 4f), (float) (tf.X - 6f), (float) (tf.Y - 4f));
            path.AddLine(tf.X - 6f, tf.Y - 4f, tf.X - 2f, tf.Y);
            path.AddLine(tf.X - 2f, tf.Y, tf.X - 6f, tf.Y + 4f);
            path.AddLine((float) (tf.X - 6f), (float) (tf.Y + 4f), (float) (tf.X - 2f), (float) (tf.Y + 4f));
            path.AddLine(tf.X - 2f, tf.Y + 4f, tf.X, tf.Y + 2f);
            path.AddLine(tf.X, tf.Y + 2f, tf.X + 2f, tf.Y + 4f);
            path.AddLine((float) (tf.X + 2f), (float) (tf.Y + 4f), (float) (tf.X + 6f), (float) (tf.Y + 4f));
            path.AddLine(tf.X + 6f, tf.Y + 4f, tf.X + 2f, tf.Y);
            path.AddLine(tf.X + 2f, tf.Y, tf.X + 6f, tf.Y - 4f);
            path.AddLine((float) (tf.X + 6f), (float) (tf.Y - 4f), (float) (tf.X + 2f), (float) (tf.Y - 4f));
            path.CloseFigure();
            return path;
        }

        private GraphicsPath CreateMaximizeFlafPath(Rectangle rect, bool maximize)
        {
            PointF tf = new PointF(rect.X + (((float) rect.Width) / 2f), rect.Y + (((float) rect.Height) / 2f));
            GraphicsPath path = new GraphicsPath();
            if (maximize)
            {
                path.AddLine((float) (tf.X - 3f), (float) (tf.Y - 3f), (float) (tf.X - 6f), (float) (tf.Y - 3f));
                path.AddLine((float) (tf.X - 6f), (float) (tf.Y - 3f), (float) (tf.X - 6f), (float) (tf.Y + 5f));
                path.AddLine((float) (tf.X - 6f), (float) (tf.Y + 5f), (float) (tf.X + 3f), (float) (tf.Y + 5f));
                path.AddLine((float) (tf.X + 3f), (float) (tf.Y + 5f), (float) (tf.X + 3f), (float) (tf.Y + 1f));
                path.AddLine((float) (tf.X + 3f), (float) (tf.Y + 1f), (float) (tf.X + 6f), (float) (tf.Y + 1f));
                path.AddLine((float) (tf.X + 6f), (float) (tf.Y + 1f), (float) (tf.X + 6f), (float) (tf.Y - 6f));
                path.AddLine((float) (tf.X + 6f), (float) (tf.Y - 6f), (float) (tf.X - 3f), (float) (tf.Y - 6f));
                path.CloseFigure();
                path.AddRectangle(new RectangleF(tf.X - 4f, tf.Y, 5f, 3f));
                path.AddLine((float) (tf.X - 1f), (float) (tf.Y - 4f), (float) (tf.X + 4f), (float) (tf.Y - 4f));
                path.AddLine((float) (tf.X + 4f), (float) (tf.Y - 4f), (float) (tf.X + 4f), (float) (tf.Y - 1f));
                path.AddLine((float) (tf.X + 4f), (float) (tf.Y - 1f), (float) (tf.X + 3f), (float) (tf.Y - 1f));
                path.AddLine((float) (tf.X + 3f), (float) (tf.Y - 1f), (float) (tf.X + 3f), (float) (tf.Y - 3f));
                path.AddLine((float) (tf.X + 3f), (float) (tf.Y - 3f), (float) (tf.X - 1f), (float) (tf.Y - 3f));
                path.CloseFigure();
                return path;
            }
            path.AddRectangle(new RectangleF(tf.X - 6f, tf.Y - 4f, 12f, 8f));
            path.AddRectangle(new RectangleF(tf.X - 3f, tf.Y - 1f, 6f, 3f));
            return path;
        }

        private GraphicsPath CreateMinimizeFlagPath(Rectangle rect)
        {
            PointF tf = new PointF(rect.X + (((float) rect.Width) / 2f), rect.Y + (((float) rect.Height) / 2f));
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new RectangleF(tf.X - 6f, tf.Y + 1f, 12f, 3f));
            return path;
        }

        public override Region CreateRegion(FormBase form)
        {
            Rectangle rect = new Rectangle(Point.Empty, form.Size);
            using (GraphicsPath path = GraphicsPathHelper.CreatePath(rect, form.Radius, form.RoundStyle, false))
            {
                return new Region(path);
            }
        }

        private void DrawBorder(Graphics g, Rectangle rect, RoundStyle roundStyle, int radius, FormBase frm)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;
            rect.Width--;
            rect.Height--;
            using (GraphicsPath path = GraphicsPathHelper.CreatePath(rect, radius, roundStyle, false))
            {
                using (Pen pen = new Pen(this.ColorTable.Border))
                {
                    g.DrawPath(pen, path);
                }
            }
            rect.Inflate(-1, -1);
            using (GraphicsPath path2 = GraphicsPathHelper.CreatePath(rect, radius, roundStyle, false))
            {
                using (Pen pen2 = new Pen(this.ColorTable.InnerBorder))
                {
                    g.DrawPath(pen2, path2);
                }
            }
        }

        private void DrawCaptionBackground(Graphics g, Rectangle captionRect, bool active)
        {
            Color baseColor = active ? this.ColorTable.CaptionActive : this.ColorTable.CaptionDeactive;
            RenderHelper.RenderBackgroundInternal(g, captionRect, baseColor, this.ColorTable.Border, this.ColorTable.InnerBorder, RoundStyle.None, 0, 0.25f, false, false, LinearGradientMode.Vertical);
        }

        private void DrawCaptionText(Graphics g, Rectangle textRect, string text, Font font, TitleType Effect, Color EffetBack, int EffectWidth, Color FrmColor, Point TitleOffset)
        {
            if (Effect == TitleType.EffectTitle)
            {
                Size size = TextRenderer.MeasureText(text, font);
                Image image = SkinTools.ImageLightEffect(text, font, FrmColor, EffetBack, EffectWidth, new Rectangle(0, 0, textRect.Width, size.Height), true);
                g.DrawImage(image, (int)((textRect.X - (EffectWidth / 2)) + TitleOffset.X), (int)((textRect.Y - (EffectWidth / 2)) + TitleOffset.Y));
            }
            else if (Effect == TitleType.Title)
            {
                textRect.X += TitleOffset.X;
                textRect.Y += TitleOffset.Y;
                TextRenderer.DrawText(g, text, font, textRect, FrmColor, TextFormatFlags.WordEllipsis | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter);
            }
        }

        private void DrawIcon(Graphics g, Rectangle iconRect, Icon icon)
        {
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawIcon(icon, iconRect);
        }

        public override void InitSkinForm(FormBase form)
        {
            form.BackColor = this.ColorTable.Back;
        }

        protected override void OnRenderSkinFormBorder(SkinFormBorderRenderEventArgs e)
        {
            Graphics graphics = e.Graphics;
            using (new AntiAliasGraphics(graphics))
            {
                this.DrawBorder(graphics, e.ClipRectangle, e.SkinForm.RoundStyle, e.SkinForm.Radius, e.SkinForm);
            }
        }

        protected override void OnRenderSkinFormCaption(SkinFormCaptionRenderEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Rectangle clipRectangle = e.ClipRectangle;
            FormBase skinForm = e.SkinForm;
            Rectangle iconRect = skinForm.IconRect;
            Rectangle empty = Rectangle.Empty;
            bool controlBox = skinForm.ControlBox;
            bool flag2 = skinForm.ControlBox && skinForm.MinimizeBox;
            bool flag3 = skinForm.ControlBox && skinForm.MaximizeBox;
            //bool flag4 = skinForm.ControlBox && skinForm.SysBottomVisibale;
            int num = 0;
            if (controlBox)
            {
                num += skinForm.CloseBoxSize.Width + skinForm.ControlBoxOffset.X;
            }
            if (flag3)
            {
                num += skinForm.MaxSize.Width + skinForm.ControlBoxSpace;
            }
            if (flag2)
            {
                num += skinForm.MiniSize.Width + skinForm.ControlBoxSpace;
            }
            foreach (CmSysButton button in (IEnumerable)skinForm.ControlBoxManager.SysButtonItems)
            {
                if (skinForm.ControlBox && button.Visibale)
                {
                    num += button.Size.Width + skinForm.ControlBoxSpace;
                }
            }
            empty = new Rectangle(iconRect.Right + 3, skinForm.BorderPadding.Left, ((clipRectangle.Width - iconRect.Right) - num) - 6, clipRectangle.Height - skinForm.BorderPadding.Left);
            using (new AntiAliasGraphics(graphics))
            {
                this.DrawCaptionBackground(graphics, clipRectangle, e.Active);
                if (skinForm.ShowIcon && (skinForm.Icon != null))
                {
                    this.DrawIcon(graphics, iconRect, skinForm.Icon);
                }
                if (!string.IsNullOrEmpty(skinForm.Text))
                {
                    Color effectBack = skinForm.EffectBack;
                    Color titleColor = skinForm.TitleColor;
                    if (skinForm.TitleSuitColor)
                    {
                        if (SkinTools.ColorSlantsDarkOrBright(skinForm.BackColor))
                        {
                            titleColor = Color.White;
                            effectBack = Color.Black;
                        }
                        else
                        {
                            titleColor = Color.Black;
                            effectBack = Color.White;
                        }
                    }
                    this.DrawCaptionText(graphics, empty, skinForm.Text, skinForm.CaptionFont, skinForm.EffectCaption, effectBack, skinForm.EffectWidth, titleColor, skinForm.TitleOffset);
                }
            }
        }

        protected override void OnRenderSkinFormControlBox(SkinFormControlBoxRenderEventArgs e)
        {
            FormBase form = e.Form;
            Graphics g = e.Graphics;
            Rectangle clipRectangle = e.ClipRectangle;
            ControlBoxState controlBoxtate = e.ControlBoxtate;
            CmSysButton cmSysButton = e.CmSysButton;
            bool active = e.Active;
            bool minimizeBox = form.ControlBox && form.MinimizeBox;
            bool maximizeBox = form.ControlBox && form.MaximizeBox;
            switch (e.ControlBoxStyle)
            {
                case ControlBoxStyle.Minimize:
                    this.RenderSkinFormMinimizeBoxInternal(g, clipRectangle, controlBoxtate, active, form);
                    return;

                case ControlBoxStyle.Maximize:
                    this.RenderSkinFormMaximizeBoxInternal(g, clipRectangle, controlBoxtate, active, minimizeBox, form.WindowState == FormWindowState.Maximized, form);
                    return;

                case ControlBoxStyle.Close:
                    this.RenderSkinFormCloseBoxInternal(g, clipRectangle, controlBoxtate, active, minimizeBox, maximizeBox, form);
                    return;

                case ControlBoxStyle.CmSysBottom:
                    this.RenderSkinFormCmSysBottomInternal(g, clipRectangle, controlBoxtate, active, form, cmSysButton);
                    return;
            }
        }

        private void RenderSkinFormCmSysBottomInternal(Graphics g, Rectangle rect, ControlBoxState state, bool active, FormBase form, CmSysButton cmSysButton)
        {
            Bitmap image = null;
            Color controlBoxActive = this.ColorTable.ControlBoxActive;
            if (cmSysButton.BoxState == ControlBoxState.Pressed)
            {
                image = (Bitmap)cmSysButton.SysButtonDown;
                controlBoxActive = this.ColorTable.ControlBoxPressed;
            }
            else if (state == ControlBoxState.Hover)
            {
                image = (Bitmap)cmSysButton.SysButtonMouse;
                controlBoxActive = this.ColorTable.ControlBoxHover;
            }
            else
            {
                image = (Bitmap)cmSysButton.SysButtonNorml;
                controlBoxActive = active ? this.ColorTable.ControlBoxActive : this.ColorTable.ControlBoxDeactive;
            }
            if (image != null)
            {
                g.DrawImage(image, rect);
            }
            else
            {
                RoundStyle bottomLeft = RoundStyle.BottomLeft;
                using (new AntiAliasGraphics(g))
                {
                    RenderHelper.RenderBackgroundInternal(g, rect, controlBoxActive, controlBoxActive, this.ColorTable.ControlBoxInnerBorder, bottomLeft, 6, 0.38f, true, false, LinearGradientMode.Vertical);
                    using (Pen pen = new Pen(this.ColorTable.Border))
                    {
                        g.DrawLine(pen, rect.X, rect.Y, rect.Right, rect.Y);
                    }
                }
            }
        }

        private void RenderSkinFormCloseBoxInternal(Graphics g, Rectangle rect, ControlBoxState state, bool active, bool minimizeBox, bool maximizeBox, FormBase form)
        {
            Bitmap image = null;
            Color controlBoxActive = this.ColorTable.ControlBoxActive;
            if (state == ControlBoxState.Pressed)
            {
                image = (Bitmap) form.CloseDownBack;
                controlBoxActive = this.ColorTable.ControlCloseBoxPressed;
            }
            else if (state == ControlBoxState.Hover)
            {
                image = (Bitmap) form.CloseMouseBack;
                controlBoxActive = this.ColorTable.ControlCloseBoxHover;
            }
            else
            {
                image = (Bitmap) form.CloseNormlBack;
                controlBoxActive = active ? this.ColorTable.ControlBoxActive : this.ColorTable.ControlBoxDeactive;
            }
            if (image != null)
            {
                g.DrawImage(image, rect);
            }
            else
            {
                RoundStyle style = (minimizeBox || maximizeBox) ? RoundStyle.BottomRight : RoundStyle.Bottom;
                using (new AntiAliasGraphics(g))
                {
                    RenderHelper.RenderBackgroundInternal(g, rect, controlBoxActive, controlBoxActive, this.ColorTable.ControlBoxInnerBorder, style, 6, 0.38f, true, false, LinearGradientMode.Vertical);
                    using (Pen pen = new Pen(this.ColorTable.Border))
                    {
                        g.DrawLine(pen, rect.X, rect.Y, rect.Right, rect.Y);
                    }
                    using (GraphicsPath path = this.CreateCloseFlagPath(rect))
                    {
                        g.FillPath(Brushes.White, path);
                        using (Pen pen2 = new Pen(controlBoxActive))
                        {
                            g.DrawPath(pen2, path);
                        }
                    }
                }
            }
        }

        private void RenderSkinFormMaximizeBoxInternal(Graphics g, Rectangle rect, ControlBoxState state, bool active, bool minimizeBox, bool maximize, FormBase form)
        {
            Bitmap image = null;
            Color controlBoxActive = this.ColorTable.ControlBoxActive;
            if (state == ControlBoxState.Pressed)
            {
                image = maximize ? ((Bitmap) form.RestoreDownBack) : ((Bitmap) form.MaxDownBack);
                controlBoxActive = this.ColorTable.ControlBoxPressed;
            }
            else if (state == ControlBoxState.Hover)
            {
                image = maximize ? ((Bitmap) form.RestoreMouseBack) : ((Bitmap) form.MaxMouseBack);
                controlBoxActive = this.ColorTable.ControlBoxHover;
            }
            else
            {
                image = maximize ? ((Bitmap) form.RestoreNormlBack) : ((Bitmap) form.MaxNormlBack);
                controlBoxActive = active ? this.ColorTable.ControlBoxActive : this.ColorTable.ControlBoxDeactive;
            }
            if (image != null)
            {
                g.DrawImage(image, rect);
            }
            else
            {
                RoundStyle style = minimizeBox ? RoundStyle.None : RoundStyle.BottomLeft;
                using (new AntiAliasGraphics(g))
                {
                    RenderHelper.RenderBackgroundInternal(g, rect, controlBoxActive, controlBoxActive, this.ColorTable.ControlBoxInnerBorder, style, 6, 0.38f, true, false, LinearGradientMode.Vertical);
                    using (Pen pen = new Pen(this.ColorTable.Border))
                    {
                        g.DrawLine(pen, rect.X, rect.Y, rect.Right, rect.Y);
                    }
                    using (GraphicsPath path = this.CreateMaximizeFlafPath(rect, maximize))
                    {
                        g.FillPath(Brushes.White, path);
                        using (Pen pen2 = new Pen(controlBoxActive))
                        {
                            g.DrawPath(pen2, path);
                        }
                    }
                }
            }
        }

        private void RenderSkinFormMinimizeBoxInternal(Graphics g, Rectangle rect, ControlBoxState state, bool active, FormBase form)
        {
            Bitmap image = null;
            Color controlBoxActive = this.ColorTable.ControlBoxActive;
            if (state == ControlBoxState.Pressed)
            {
                image = (Bitmap) form.MiniDownBack;
                controlBoxActive = this.ColorTable.ControlBoxPressed;
            }
            else if (state == ControlBoxState.Hover)
            {
                image = (Bitmap) form.MiniMouseBack;
                controlBoxActive = this.ColorTable.ControlBoxHover;
            }
            else
            {
                image = (Bitmap) form.MiniNormlBack;
                controlBoxActive = active ? this.ColorTable.ControlBoxActive : this.ColorTable.ControlBoxDeactive;
            }
            if (image != null)
            {
                g.DrawImage(image, rect);
            }
            else
            {
                RoundStyle bottomLeft = RoundStyle.BottomLeft;
                using (new AntiAliasGraphics(g))
                {
                    RenderHelper.RenderBackgroundInternal(g, rect, controlBoxActive, controlBoxActive, this.ColorTable.ControlBoxInnerBorder, bottomLeft, 6, 0.38f, true, false, LinearGradientMode.Vertical);
                    using (Pen pen = new Pen(this.ColorTable.Border))
                    {
                        g.DrawLine(pen, rect.X, rect.Y, rect.Right, rect.Y);
                    }
                    using (GraphicsPath path = this.CreateMinimizeFlagPath(rect))
                    {
                        g.FillPath(Brushes.White, path);
                        using (Pen pen2 = new Pen(controlBoxActive))
                        {
                            g.DrawPath(pen2, path);
                        }
                    }
                }
            }
        }

        //private void RenderSkinFormSysBottomInternal(Graphics g, Rectangle rect, ControlBoxState state, bool active, FormBase form)
        //{
        //    Bitmap image = null;
        //    Color controlBoxActive = this.ColorTable.ControlBoxActive;
        //    if (state == ControlBoxState.Pressed)
        //    {
        //        image = (Bitmap) form.SysBottomDown;
        //        controlBoxActive = this.ColorTable.ControlBoxPressed;
        //    }
        //    else if (state == ControlBoxState.Hover)
        //    {
        //        image = (Bitmap) form.SysBottomMouse;
        //        controlBoxActive = this.ColorTable.ControlBoxHover;
        //    }
        //    else
        //    {
        //        image = (Bitmap) form.SysBottomNorml;
        //        controlBoxActive = active ? this.ColorTable.ControlBoxActive : this.ColorTable.ControlBoxDeactive;
        //    }
        //    if (image != null)
        //    {
        //        g.DrawImage(image, rect);
        //    }
        //    else
        //    {
        //        RoundStyle bottomLeft = RoundStyle.BottomLeft;
        //        using (new AntiAliasGraphics(g))
        //        {
        //            RenderHelper.RenderBackgroundInternal(g, rect, controlBoxActive, controlBoxActive, this.ColorTable.ControlBoxInnerBorder, bottomLeft, 6, 0.38f, true, false, LinearGradientMode.Vertical);
        //            using (Pen pen = new Pen(this.ColorTable.Border))
        //            {
        //                g.DrawLine(pen, rect.X, rect.Y, rect.Right, rect.Y);
        //            }
        //        }
        //    }
        //}

        public ImageAttributes Trank(Bitmap btm, int Alphas)
        {
            Bitmap image = (Bitmap)btm.Clone();
            Graphics.FromImage(image);
            float num = ((float)Alphas) / 100f;
            float[][] numArray = new float[5][];
            float[] numArray2 = new float[5];
            numArray2[0] = 1f;
            numArray[0] = numArray2;
            float[] numArray3 = new float[5];
            numArray3[1] = 1f;
            numArray[1] = numArray3;
            float[] numArray4 = new float[5];
            numArray4[2] = 1f;
            numArray[2] = numArray4;
            float[] numArray5 = new float[5];
            numArray5[3] = num;
            numArray[3] = numArray5;
            float[] numArray6 = new float[5];
            numArray6[4] = 1f;
            numArray[4] = numArray6;
            float[][] newColorMatrix = numArray;
            ColorMatrix matrix = new ColorMatrix(newColorMatrix);
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            return attributes;
        }

        public FormSkinColorTable ColorTable
        {
            get
            {
                if (this._colorTable == null)
                {
                    this._colorTable = new FormSkinColorTable();
                }
                return this._colorTable;
            }
        }
    }
}
