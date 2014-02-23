using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Client.UI.Base.Enums;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using Client.UI.Base.Utils;

namespace Client.UI.Base.Controls
{
    [ToolboxBitmap(typeof(Button))]
    public class SkinButton : Button
    {
        private Color _baseColor = Color.FromArgb(0x33, 0xa1, 0xe0);
        private ControlState _controlState;
        private int _imageWidth = 0x12;
        private RoundStyle _roundStyle;
        private const int animationLength = 300;
        private Rectangle backrectangle = new Rectangle(10, 10, 10, 10);
        private bool create;
        private int currentFrame;
        private int direction;
        private Image downback;
        private DrawStyle drawType = DrawStyle.Draw;
        private bool fadeGlow = true;
        private bool foreColorSuit;
        private const int FRAME_ANIMATED = 3;
        private const int FRAME_DISABLED = 0;
        private const int FRAME_NORMAL = 2;
        private const int FRAME_PRESSED = 1;
        private List<Image> frames;
        private const int framesCount = 10;
        private Color glowColor = Color.White;
        private bool inheritColor = true;
        private Image mouseback;
        private Image normlback;
        private bool palace;
        private int radius = 8;
        private ControlState states;
        private StopStates stopState = StopStates.NoStop;
        private Timer timer = new Timer();

        public SkinButton()
        {
            this.timer.Tick += new EventHandler(this.timer_Tick);
            this.timer.Interval = 30;
            this.Init();
            base.ResizeRedraw = true;
            this.BackColor = Color.Transparent;
        }

        private void CalculateRect(out Rectangle imageRect, out Rectangle textRect)
        {
            imageRect = Rectangle.Empty;
            textRect = Rectangle.Empty;
            if (base.Image == null)
            {
                textRect = new Rectangle(2, 0, base.Width - 4, base.Height);
            }
            else
            {
                switch (base.TextImageRelation)
                {
                    case TextImageRelation.Overlay:
                        imageRect = new Rectangle(2, (base.Height - this.ImageWidth) / 2, this.ImageWidth, this.ImageWidth);
                        textRect = new Rectangle(2, 0, base.Width - 4, base.Height);
                        break;

                    case TextImageRelation.ImageAboveText:
                        imageRect = new Rectangle((base.Width - this.ImageWidth) / 2, 2, this.ImageWidth, this.ImageWidth);
                        textRect = new Rectangle(2, imageRect.Bottom, base.Width, (base.Height - imageRect.Bottom) - 2);
                        break;

                    case TextImageRelation.TextAboveImage:
                        imageRect = new Rectangle((base.Width - this.ImageWidth) / 2, (base.Height - this.ImageWidth) - 2, this.ImageWidth, this.ImageWidth);
                        textRect = new Rectangle(0, 2, base.Width, (base.Height - imageRect.Y) - 2);
                        break;

                    case TextImageRelation.ImageBeforeText:
                        imageRect = new Rectangle(2, (base.Height - this.ImageWidth) / 2, this.ImageWidth, this.ImageWidth);
                        textRect = new Rectangle(imageRect.Right + 2, 0, (base.Width - imageRect.Right) - 4, base.Height);
                        break;

                    case TextImageRelation.TextBeforeImage:
                        imageRect = new Rectangle((base.Width - this.ImageWidth) - 2, (base.Height - this.ImageWidth) / 2, this.ImageWidth, this.ImageWidth);
                        textRect = new Rectangle(2, 0, imageRect.X - 2, base.Height);
                        break;
                }
                if (this.RightToLeft == RightToLeft.Yes)
                {
                    imageRect.X = base.Width - imageRect.Right;
                    textRect.X = base.Width - textRect.Right;
                }
            }
        }

        public Image CreateBackgroundFrame(bool pressed, bool hovered, bool animating, bool enabled, float glowOpacity)
        {
            Rectangle clientRectangle = base.ClientRectangle;
            if (clientRectangle.Width <= 0)
            {
                clientRectangle.Width = 1;
            }
            if (clientRectangle.Height <= 0)
            {
                clientRectangle.Height = 1;
            }
            Image image = new Bitmap(clientRectangle.Width, clientRectangle.Height);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                graphics.Clear(Color.Transparent);
                DrawButtonBackground(graphics, clientRectangle, pressed, hovered, animating, enabled, this.glowColor, glowOpacity);
            }
            return image;
        }

        private static GraphicsPath CreateBottomRadialPath(Rectangle rectangle)
        {
            GraphicsPath path = new GraphicsPath();
            RectangleF rect = rectangle;
            rect.X -= rect.Width * 0.35f;
            rect.Y -= rect.Height * 0.15f;
            rect.Width *= 1.7f;
            rect.Height *= 2.3f;
            path.AddEllipse(rect);
            path.CloseFigure();
            return path;
        }

        private void CreateFrames()
        {
            this.CreateFrames(false);
        }

        private void CreateFrames(bool withAnimationFrames)
        {
            this.DestroyFrames();
            if (base.IsHandleCreated)
            {
                if (this.frames == null)
                {
                    this.frames = new List<Image>();
                }
                this.frames.Add(this.CreateBackgroundFrame(false, false, false, false, 0f));
                this.frames.Add(this.CreateBackgroundFrame(true, true, false, true, 0f));
                this.frames.Add(this.CreateBackgroundFrame(false, false, false, true, 0f));
                if (withAnimationFrames)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        this.frames.Add(this.CreateBackgroundFrame(false, true, true, true, ((float)i) / 9f));
                    }
                }
            }
        }

        private static GraphicsPath CreateRoundRectangle(Rectangle rectangle, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int left = rectangle.Left;
            int top = rectangle.Top;
            int width = rectangle.Width;
            int height = rectangle.Height;
            int num5 = radius << 1;
            path.AddArc(left, top, num5, num5, 180f, 90f);
            path.AddLine(left + radius, top, (left + width) - radius, top);
            path.AddArc((left + width) - num5, top, num5, num5, 270f, 90f);
            path.AddLine((int)(left + width), (int)(top + radius), (int)(left + width), (int)((top + height) - radius));
            path.AddArc((left + width) - num5, (top + height) - num5, num5, num5, 0f, 90f);
            path.AddLine((int)((left + width) - radius), (int)(top + height), (int)(left + radius), (int)(top + height));
            path.AddArc(left, (top + height) - num5, num5, num5, 90f, 90f);
            path.AddLine(left, (top + height) - radius, left, top + radius);
            path.CloseFigure();
            return path;
        }

        private void DestroyFrames()
        {
            if (this.frames != null)
            {
                while (this.frames.Count > 0)
                {
                    this.frames[this.frames.Count - 1].Dispose();
                    this.frames.RemoveAt(this.frames.Count - 1);
                }
            }
        }


        private static void DrawButtonBackground(Graphics g, Rectangle rectangle, bool pressed, bool hovered, bool animating, bool enabled, Color glowColor, float glowOpacity)
        {
            SmoothingMode smoothingMode = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rectangle2 = rectangle;
            rectangle2.Width--;
            rectangle2.Height--;
            rectangle2.X++;
            rectangle2.Y++;
            rectangle2.Width -= 2;
            rectangle2.Height -= 2;
            Rectangle rectangle3 = rectangle2;
            rectangle3.Height = rectangle3.Height >> 1;
            if ((hovered || animating) && !pressed)
            {
                using (GraphicsPath path = CreateRoundRectangle(rectangle2, 2))
                {
                    g.SetClip(path, CombineMode.Intersect);
                    using (GraphicsPath path2 = CreateBottomRadialPath(rectangle2))
                    {
                        using (PathGradientBrush brush = new PathGradientBrush(path2))
                        {
                            int alpha = (int)((178f * glowOpacity) + 0.5f);
                            RectangleF bounds = path2.GetBounds();
                            brush.CenterPoint = new PointF((bounds.Left + bounds.Right) / 2f, (bounds.Top + bounds.Bottom) / 2f);
                            brush.CenterColor = Color.FromArgb(alpha, glowColor);
                            brush.SurroundColors = new Color[] { Color.FromArgb(0, glowColor) };
                            g.FillPath(brush, path2);
                        }
                    }
                    g.ResetClip();
                }
            }
            g.SmoothingMode = smoothingMode;
        }

        private void DrawButtonBackgroundFromBuffer(Graphics graphics)
        {
            int num;
            if (!base.Enabled)
            {
                num = 0;
            }
            else if (this.ControlState == ControlState.Pressed)
            {
                num = 1;
            }
            else if (!this.isAnimating && (this.currentFrame == 0))
            {
                num = 2;
            }
            else
            {
                if (!this.HasAnimationFrames)
                {
                    this.CreateFrames(true);
                }
                num = 3 + this.currentFrame;
            }
            if ((this.frames == null) || (this.frames.Count == 0))
            {
                this.CreateFrames();
            }
            graphics.DrawImage(this.frames[num], Point.Empty);
        }

        private void DrawGlass(Graphics g, RectangleF glassRect, int alphaCenter, int alphaSurround)
        {
            this.DrawGlass(g, glassRect, Color.White, alphaCenter, alphaSurround);
        }

        private void DrawGlass(Graphics g, RectangleF glassRect, Color glassColor, int alphaCenter, int alphaSurround)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(glassRect);
                using (PathGradientBrush brush = new PathGradientBrush(path))
                {
                    brush.CenterColor = Color.FromArgb(alphaCenter, glassColor);
                    brush.SurroundColors = new Color[] { Color.FromArgb(alphaSurround, glassColor) };
                    brush.CenterPoint = new PointF(glassRect.X + (glassRect.Width / 2f), glassRect.Y + (glassRect.Height / 2f));
                    g.FillPath(brush, path);
                }
            }
        }

        private void FadeIn()
        {
            this.direction = 1;
            this.timer.Enabled = true;
        }

        private void FadeOut()
        {
            this.direction = -1;
            this.timer.Enabled = true;
        }

        private Color GetColor(Color colorBase, int a, int r, int g, int b)
        {
            int num = colorBase.A;
            int num2 = colorBase.R;
            int num3 = colorBase.G;
            int num4 = colorBase.B;
            if ((a + num) > 0xff)
            {
                a = 0xff;
            }
            else
            {
                a = Math.Max(a + num, 0);
            }
            if ((r + num2) > 0xff)
            {
                r = 0xff;
            }
            else
            {
                r = Math.Max(r + num2, 0);
            }
            if ((g + num3) > 0xff)
            {
                g = 0xff;
            }
            else
            {
                g = Math.Max(g + num3, 0);
            }
            if ((b + num4) > 0xff)
            {
                b = 0xff;
            }
            else
            {
                b = Math.Max(b + num4, 0);
            }
            return Color.FromArgb(a, r, g, b);
        }

        public static TextFormatFlags GetTextFormatFlags(ContentAlignment alignment, bool rightToleft)
        {
            TextFormatFlags flags = TextFormatFlags.SingleLine | TextFormatFlags.WordBreak;
            if (rightToleft)
            {
                flags |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;
            }
            ContentAlignment alignment2 = alignment;
            if (alignment2 <= ContentAlignment.MiddleCenter)
            {
                switch (alignment2)
                {
                    case ContentAlignment.TopLeft:
                        return flags;

                    case ContentAlignment.TopCenter:
                        return (flags | TextFormatFlags.HorizontalCenter);

                    case (ContentAlignment.TopCenter | ContentAlignment.TopLeft):
                        return flags;

                    case ContentAlignment.TopRight:
                        return (flags | TextFormatFlags.Right);

                    case ContentAlignment.MiddleLeft:
                        return (flags | TextFormatFlags.VerticalCenter);
                }
                if (alignment2 == ContentAlignment.MiddleCenter)
                {
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                }
                return flags;
            }
            if (alignment2 <= ContentAlignment.BottomLeft)
            {
                if (alignment2 != ContentAlignment.MiddleRight)
                {
                    if (alignment2 == ContentAlignment.BottomLeft)
                    {
                        flags |= TextFormatFlags.Bottom;
                    }
                    return flags;
                }
                return (flags | (TextFormatFlags.VerticalCenter | TextFormatFlags.Right));
            }
            if (alignment2 != ContentAlignment.BottomCenter)
            {
                if (alignment2 == ContentAlignment.BottomRight)
                {
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.Right;
                }
                return flags;
            }
            return (flags | (TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter));
        }

        public void Init()
        {
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            base.SetStyle(ControlStyles.StandardDoubleClick, false);
            base.SetStyle(ControlStyles.Selectable, true);
            base.UpdateStyles();
        }

        private void InitializeComponent()
        {
            base.SuspendLayout();
            base.ResumeLayout(false);
        }

        protected override void OnCreateControl()
        {
            if (base.Parent != null)
            {
                base.Parent.BackColorChanged += new EventHandler(this.SkinButton_BackColorChanged);
            }
            base.OnCreateControl();
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.Invalidate();
            base.OnEnabledChanged(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                this._controlState = ControlState.Pressed;
                base.Invalidate();
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.FadeIn();
            this._controlState =ControlState.Hover;
            base.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.FadeOut();
            this._controlState =ControlState.Normal;
            base.Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                this._controlState = ControlState.Hover;
                base.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rectangle;
            Rectangle rectangle2;
            Color controlDark;
            Color color3;
            base.OnPaint(e);
            base.OnPaintBackground(e);
            Graphics g = e.Graphics;
            Rectangle clientRectangle = base.ClientRectangle;
            this.CalculateRect(out rectangle, out rectangle2);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Color baseColor = this.BaseColor;
            Color innerBorderColor = Color.FromArgb(200, 0xff, 0xff, 0xff);
            Bitmap mouseBack = null;
            int num = 0;
            if (this.StopState != StopStates.NoStop)
            {
                this._controlState = (ControlState)this.StopState;
            }
            if (this.InheritColor)
            {
                baseColor = base.Parent.BackColor;
            }
            switch (this._controlState)
            {
                case ControlState.Hover:
                    mouseBack = (Bitmap)this.MouseBack;
                    controlDark = this.GetColor(baseColor, 0, -13, -8, -3);
                    color3 = baseColor;
                    break;

                case ControlState.Pressed:
                    mouseBack = (Bitmap)this.DownBack;
                    controlDark = this.GetColor(baseColor, 0, -35, -24, -9);
                    color3 = baseColor;
                    num = 1;
                    break;

                default:
                    mouseBack = (Bitmap)this.NormlBack;
                    controlDark = baseColor;
                    color3 = baseColor;
                    break;
            }
            if (!base.Enabled)
            {
                controlDark = SystemColors.ControlDark;
                color3 = SystemColors.ControlDark;
            }
            if ((mouseBack != null) && (this.DrawType == DrawStyle.Img))
            {
                SkinTools.CreateRegion(this, clientRectangle, this.Radius, this.RoundStyle);
                if (this.Create && (this._controlState != this.states))
                {
                    SkinTools.CreateControlRegion(this, mouseBack, 1);
                }
                if (this.Palace)
                {
                    ImageDrawRect.DrawRect(g, mouseBack, clientRectangle, Rectangle.FromLTRB(this.BackRectangle.X, this.BackRectangle.Y, this.BackRectangle.Width, this.BackRectangle.Height), 1, 1);
                }
                else
                {
                    g.DrawImage(mouseBack, 0, 0, base.Width, base.Height);
                }
            }
            else if (this.DrawType == DrawStyle.Draw)
            {
                this.RenderBackgroundInternal(g, clientRectangle, controlDark, color3, innerBorderColor, this.RoundStyle, this.Radius, 0.35f, true, true, LinearGradientMode.Vertical);
                if (this.FadeGlow)
                {
                    this.DrawButtonBackgroundFromBuffer(e.Graphics);
                }
            }
            Image image = null;
            Size empty = Size.Empty;
            if (base.Image != null)
            {
                if (string.IsNullOrEmpty(this.Text))
                {
                    image = base.Image;
                    empty = new Size(image.Width, image.Height);
                    clientRectangle.Inflate(-4, -4);
                    if ((empty.Width * empty.Height) != 0)
                    {
                        Rectangle withinThis = clientRectangle;
                        withinThis = ImageDrawRect.HAlignWithin(empty, withinThis, base.ImageAlign);
                        withinThis =ImageDrawRect.VAlignWithin(empty, withinThis, base.ImageAlign);
                        if (!base.Enabled)
                        {
                            ControlPaint.DrawImageDisabled(g, image, withinThis.Left, withinThis.Top, this.BackColor);
                        }
                        else
                        {
                            g.DrawImage(image, withinThis.Left + num, withinThis.Top + num, image.Width, image.Height);
                        }
                    }
                }
                else
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                    g.DrawImage(base.Image, rectangle, -num, -num, base.Image.Width, base.Image.Height, GraphicsUnit.Pixel);
                }
            }
            else if ((base.ImageList != null) && (base.ImageIndex != -1))
            {
                image = base.ImageList.Images[base.ImageIndex];
            }
            Color foreColor = base.Enabled ? this.ForeColor : SystemColors.ControlDark;
            if (this.ForeColorSuit)
            {
                if (SkinTools.ColorSlantsDarkOrBright(baseColor))
                {
                    foreColor = Color.White;
                }
                else
                {
                    foreColor = Color.Black;
                }
            }
            TextRenderer.DrawText(g, this.Text, this.Font, rectangle2, foreColor, GetTextFormatFlags(this.TextAlign, this.RightToLeft == RightToLeft.Yes));
            this.states = this._controlState;
        }

        public void RenderBackgroundInternal(Graphics g, Rectangle rect, Color baseColor, Color borderColor, Color innerBorderColor, RoundStyle style, int roundWidth, float basePosition, bool drawBorder, bool drawGlass, LinearGradientMode mode)
        {
            if (drawBorder)
            {
                rect.Width--;
                rect.Height--;
            }
            using (LinearGradientBrush brush = new LinearGradientBrush(rect, Color.Transparent, Color.Transparent, mode))
            {
                Color[] colorArray = new Color[] { this.GetColor(baseColor, 0, 0x23, 0x18, 9), this.GetColor(baseColor, 0, 13, 8, 3), baseColor, this.GetColor(baseColor, 0, 0x44, 0x45, 0x36) };
                ColorBlend blend = new ColorBlend();
                float[] numArray = new float[4];
                numArray[1] = basePosition;
                numArray[2] = basePosition + 0.05f;
                numArray[3] = 1f;
                blend.Positions = numArray;
                blend.Colors = colorArray;
                brush.InterpolationColors = blend;
                if (style != RoundStyle.None)
                {
                    using (GraphicsPath path = GraphicsPathHelper.CreatePath(rect, roundWidth, style, false))
                    {
                        g.FillPath(brush, path);
                    }
                    if (baseColor.A > 80)
                    {
                        Rectangle rectangle = rect;
                        if (mode == LinearGradientMode.Vertical)
                        {
                            rectangle.Height = (int)(rectangle.Height * basePosition);
                        }
                        else
                        {
                            rectangle.Width = (int)(rect.Width * basePosition);
                        }
                        using (GraphicsPath path2 = GraphicsPathHelper.CreatePath(rectangle, roundWidth, RoundStyle.Top, false))
                        {
                            using (SolidBrush brush2 = new SolidBrush(Color.FromArgb(80, 0xff, 0xff, 0xff)))
                            {
                                g.FillPath(brush2, path2);
                            }
                        }
                    }
                    if (drawGlass)
                    {
                        RectangleF glassRect = rect;
                        if (mode == LinearGradientMode.Vertical)
                        {
                            glassRect.Y = rect.Y + (rect.Height * basePosition);
                            glassRect.Height = (rect.Height - (rect.Height * basePosition)) * 2f;
                        }
                        else
                        {
                            glassRect.X = rect.X + (rect.Width * basePosition);
                            glassRect.Width = (rect.Width - (rect.Width * basePosition)) * 2f;
                        }
                        this.DrawGlass(g, glassRect, 170, 0);
                    }
                    if (!drawBorder)
                    {
                        return;
                    }
                    using (GraphicsPath path3 = GraphicsPathHelper.CreatePath(rect, roundWidth, style, false))
                    {
                        using (Pen pen = new Pen(borderColor))
                        {
                            g.DrawPath(pen, path3);
                        }
                    }
                    rect.Inflate(-1, -1);
                    using (GraphicsPath path4 = GraphicsPathHelper.CreatePath(rect, roundWidth, style, false))
                    {
                        using (Pen pen2 = new Pen(innerBorderColor))
                        {
                            g.DrawPath(pen2, path4);
                        }
                        return;
                    }
                }
                g.FillRectangle(brush, rect);
                if (baseColor.A > 80)
                {
                    Rectangle rectangle2 = rect;
                    if (mode == LinearGradientMode.Vertical)
                    {
                        rectangle2.Height = (int)(rectangle2.Height * basePosition);
                    }
                    else
                    {
                        rectangle2.Width = (int)(rect.Width * basePosition);
                    }
                    using (SolidBrush brush3 = new SolidBrush(Color.FromArgb(80, 0xff, 0xff, 0xff)))
                    {
                        g.FillRectangle(brush3, rectangle2);
                    }
                }
                if (drawGlass)
                {
                    RectangleF ef2 = rect;
                    if (mode == LinearGradientMode.Vertical)
                    {
                        ef2.Y = rect.Y + (rect.Height * basePosition);
                        ef2.Height = (rect.Height - (rect.Height * basePosition)) * 2f;
                    }
                    else
                    {
                        ef2.X = rect.X + (rect.Width * basePosition);
                        ef2.Width = (rect.Width - (rect.Width * basePosition)) * 2f;
                    }
                    this.DrawGlass(g, ef2, 200, 0);
                }
                if (drawBorder)
                {
                    using (Pen pen3 = new Pen(borderColor))
                    {
                        g.DrawRectangle(pen3, rect);
                    }
                    rect.Inflate(-1, -1);
                    using (Pen pen4 = new Pen(innerBorderColor))
                    {
                        g.DrawRectangle(pen4, rect);
                    }
                }
            }
        }

        private void SkinButton_BackColorChanged(object sender, EventArgs e)
        {
            base.Invalidate();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (this.timer.Enabled)
            {
                this.Refresh();
                this.currentFrame += this.direction;
                if (this.currentFrame == -1)
                {
                    this.currentFrame = 0;
                    this.timer.Enabled = false;
                    this.direction = 0;
                }
                else if (this.currentFrame == 10)
                {
                    this.currentFrame = 9;
                    this.timer.Enabled = false;
                    this.direction = 0;
                }
            }
        }

        [Category("Skin"), DefaultValue(typeof(Rectangle), "10,10,10,10"), Description("九宫绘画区域")]
        public Rectangle BackRectangle
        {
            get
            {
                return this.backrectangle;
            }
            set
            {
                if (this.backrectangle != value)
                {
                    this.backrectangle = value;
                }
                base.Invalidate();
            }
        }

        [Description("非图片绘制时Bottom色调"), Category("Skin"), DefaultValue(typeof(Color), "51, 161, 224")]
        public Color BaseColor
        {
            get
            {
                return this._baseColor;
            }
            set
            {
                this._baseColor = value;
                base.Invalidate();
            }
        }

        [Description("控件状态")]
        public ControlState ControlState
        {
            get
            {
                return this._controlState;
            }
            set
            {
                if (this._controlState != value)
                {
                    this._controlState = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(typeof(bool), "false"), Category("Skin"), Description("是否开启:根据所绘图限制控件范围")]
        public bool Create
        {
            get
            {
                return this.create;
            }
            set
            {
                if (this.create != value)
                {
                    this.create = value;
                    base.Invalidate();
                }
            }
        }

        [Description("点击时背景"), Category("MouseDown")]
        public Image DownBack
        {
            get
            {
                return this.downback;
            }
            set
            {
                if (this.downback != value)
                {
                    this.downback = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(typeof(DrawStyle), "2"), Description("按钮的绘画模式"), Category("Skin")]
        public DrawStyle DrawType
        {
            get
            {
                return this.drawType;
            }
            set
            {
                if (this.drawType != value)
                {
                    this.drawType = value;
                    base.Invalidate();
                }
            }
        }

        [Description("是否开启动画渐变效果(只有DrawType属性设置为Draw才有效)"), Category("Skin"), DefaultValue(typeof(bool), "true")]
        public bool FadeGlow
        {
            get
            {
                return this.fadeGlow;
            }
            set
            {
                if (this.fadeGlow != value)
                {
                    this.fadeGlow = value;
                    base.Invalidate();
                }
            }
        }

        [Category("Skin"), Description("是否根据背景色自动适应文本颜色。\n(背景色为暗色时文本显示白色，背景为亮色时文本显示黑色。)"), DefaultValue(false)]
        public bool ForeColorSuit
        {
            get
            {
                return this.foreColorSuit;
            }
            set
            {
                if (this.foreColorSuit != value)
                {
                    this.foreColorSuit = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(typeof(Color), "White"), Description("动画渐变Glow颜色"), Category("Skin")]
        public virtual Color GlowColor
        {
            get
            {
                return this.glowColor;
            }
            set
            {
                if (this.glowColor != value)
                {
                    this.glowColor = value;
                    this.CreateFrames();
                    if (base.IsHandleCreated)
                    {
                        base.Invalidate();
                    }
                }
            }
        }

        private bool HasAnimationFrames
        {
            get
            {
                return ((this.frames != null) && (this.frames.Count > 3));
            }
        }

        [Category("Skin"), Description("设置或获取图像的大小"), DefaultValue(0x12)]
        public int ImageWidth
        {
            get
            {
                return this._imageWidth;
            }
            set
            {
                if (value != this._imageWidth)
                {
                    this._imageWidth = (value < 12) ? 12 : value;
                    base.Invalidate();
                }
            }
        }

        [Category("Skin"), DefaultValue(typeof(bool), "true"), Description("是否继承所在窗体的色调。")]
        public bool InheritColor
        {
            get
            {
                return this.inheritColor;
            }
            set
            {
                if (this.inheritColor != value)
                {
                    this.inheritColor = value;
                    base.Invalidate();
                }
            }
        }

        private bool isAnimating
        {
            get
            {
                return (this.direction != 0);
            }
        }

        [Description("悬浮时背景"), Category("MouseEnter")]
        public Image MouseBack
        {
            get
            {
                return this.mouseback;
            }
            set
            {
                if (this.mouseback != value)
                {
                    this.mouseback = value;
                    base.Invalidate();
                }
            }
        }

        [Description("初始时背景"), Category("MouseNorml")]
        public Image NormlBack
        {
            get
            {
                return this.normlback;
            }
            set
            {
                if (this.normlback != value)
                {
                    this.normlback = value;
                    base.Invalidate();
                }
            }
        }

        [Description("是否开启九宫绘图"), DefaultValue(typeof(bool), "false"), Category("Skin")]
        public bool Palace
        {
            get
            {
                return this.palace;
            }
            set
            {
                if (this.palace != value)
                {
                    this.palace = value;
                    base.Invalidate();
                }
            }
        }

        [Category("Skin"), Description("圆角大小"), DefaultValue(typeof(int), "8")]
        public int Radius
        {
            get
            {
                return this.radius;
            }
            set
            {
                if (this.radius != value)
                {
                    this.radius = (value < 4) ? 4 : value;
                    base.Invalidate();
                }
            }
        }

        [Description("设置或获取按钮圆角的样式"), Category("Skin"), DefaultValue(typeof(RoundStyle), "0")]
        public RoundStyle RoundStyle
        {
            get
            {
                return this._roundStyle;
            }
            set
            {
                if (this._roundStyle != value)
                {
                    this._roundStyle = value;
                    base.Invalidate();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public StopStates StopState
        {
            get
            {
                return this.stopState;
            }
            set
            {
                this.stopState = value;
                base.Invalidate();
            }
        }
    }
}
