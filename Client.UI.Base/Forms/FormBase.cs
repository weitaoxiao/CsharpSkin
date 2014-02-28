using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using Client.Core.Win32;
using System.Drawing.Text;
using Client.UI.Base.Utils;
using Client.UI.Base.Enums;
using Client.UI.Base.Render;
using Client.UI.Base.Controls;
using System.IO;
using System.Reflection;
using System.Collections.Specialized;
using Client.UI.Base.Collection;
using System.Collections;

namespace Client.UI.Base.Forms
{
    public class FormBase : Form
    {
        private int _captionHeight = 24;
        private int _borderWidth = 3;
        private int _radius = 6;
        private int _controlBoxSpace;
        private int _effectWidth = 6;
        private int _shadowWidth = 4;
        private int _inWmWindowPosChanged;

        private bool _isMouseDown;
        private bool _dropBack = true;
        private bool _backLayout = true;
        private bool _showFormIcon = true;
        private bool _showSystemMenu;
        private bool _canResize = true;
        private bool _active;
        private bool _inPosChanged;

        private bool _backToColor = true;
        private bool _special = true;
        private bool _inheritBack;
        private bool _titleSuitColor;
        private bool _shadow = true;
        private bool _clientSizeSet;

        private Image _back;
        private Image _backPalace;
        private Image _borderPalace;
        private Image _maxDownBack;
        private Image _maxMouseBack;
        private Image _maxNormlBack;
        private Image _miniDownBack;
        private Image _miniMouseBack;
        private Image _miniNormlBack;
        private Image _closeDownBack;
        private Image _closeMouseBack;
        private Image _closeNormlBack;
        private Image _restoreDownBack;
        private Image _restoreMouseBack;
        private Image _restoreNormlBack;

        private RoundStyle _roundStyle = RoundStyle.All;
        private Size _maxBoxSize = new Size(32, 18);
        private Size _minBoxiSize = new Size(32, 18);
        private Size _closeBoxSize = new Size(32, 18);

        private Point _controlBoxOffset = new Point(6, 0);
        private Color _titleColor = Color.Black;
        private Point _titleOffset = new Point(0, 0);
        private Font _captionFont = SystemFonts.CaptionFont;
        private Color _effectBack = Color.White;
        private Color _shadowColor = Color.Black;
        private SkinRendererBase _renderer;
        private CustomSysButtonCollection _sysButtonItems;
        private ControlBoxManager _controlBoxManager;
        private MobileStyle _mobile = MobileStyle.Mobile;
        private TitleType _effectCaption = TitleType.EffectTitle;
        private double _skinOpacity = 1.0;
        private FormShadow _formShadow;

        private Padding _borderPadding = new Padding(4);

        private Rectangle _deltaRect;
        private Rectangle _backRectangle = new Rectangle(10, 10, 10, 10);
        private Rectangle _borderRectangle = new Rectangle(10, 10, 10, 10);

        [Category("Skin"), Description("Back属性值更改时引发的事件")]
        public event BackEventHandler BackChanged;

        private static readonly object EventRendererChanged = new object();


        #region >重写系统属性<
        [DefaultValue(true), Description("是否在窗体上绘画ICO图标"), Category("窗口样式")]
        public new bool ShowIcon
        {
            get
            {
                return this._showFormIcon;
            }
            set
            {
                if (this._showFormIcon != value)
                {
                    this._showFormIcon = value;
                    base.Invalidate();
                }
            }
        }
        #endregion

        #region >皮肤属性 Skin<
        [Description("获取或设置窗体是否显示系统菜单"), Category("Skin"), DefaultValue(false)]
        public bool ShowSystemMenu
        {
            get
            {
                return this._showSystemMenu;
            }
            set
            {
                this._showSystemMenu = value;
            }
        }

        [Description("设置或获取窗体是否可以改变大小"), Category("Skin"), DefaultValue(true)]
        public bool CanResize
        {
            get
            {
                return this._canResize;
            }
            set
            {
                this._canResize = value;
            }
        }

        [DefaultValue(0x18), Description("设置或获取窗体标题栏的高度"), Category("Skin")]
        public int CaptionHeight
        {
            get
            {
                return this._captionHeight;
            }
            set
            {
                if (this._captionHeight != value)
                {
                    this._captionHeight = (value < this._borderWidth) ? this._borderWidth : value;
                    base.Invalidate();
                }
            }
        }

        [Description("设置或获取窗体的边框的宽度"), DefaultValue(3), Category("Skin")]
        public int BorderWidth
        {
            get
            {
                return this._borderWidth;
            }
            set
            {
                if (this._borderWidth != value)
                {
                    this._borderWidth = (value < 1) ? 1 : value;
                }
            }
        }

        [DefaultValue(6), Description("设置或获取窗体的圆角的大小"), Category("Skin")]
        public int Radius
        {
            get
            {
                return this._radius;
            }
            set
            {
                if (this._radius != value)
                {
                    this._radius = value;
                    this.SetReion();
                    base.Invalidate();
                }
            }
        }

        [Description("设置或获取窗体的圆角样式"), Category("Skin"), DefaultValue(typeof(RoundStyle), "1")]
        public RoundStyle FormRoundStyle
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
                    this.SetReion();
                    base.Invalidate();
                }
            }
        }

        //[Description("自定义系统按钮是否显示"), Category("Skin")]
        //public bool SysBottomVisibale
        //{
        //    get
        //    {
        //        return this._sysBottomVisibale;
        //    }
        //    set
        //    {
        //        if (this._sysBottomVisibale != value)
        //        {
        //            this._sysBottomVisibale = value;
        //            base.Invalidate();
        //        }
        //    }
        //}

        [Description("设置或获取关闭按钮的大小"), Category("CloseBox"), DefaultValue(typeof(System.Drawing.Size), "32, 18")]
        public System.Drawing.Size CloseBoxSize
        {
            get
            {
                return this._closeBoxSize;
            }
            set
            {
                if (this._closeBoxSize != value)
                {
                    this._closeBoxSize = value;
                    base.Invalidate();
                }
            }
        }

        [Category("Skin"), Description("设置或获取控制按钮的偏移"), DefaultValue(typeof(System.Drawing.Point), "6, 0")]
        public System.Drawing.Point ControlBoxOffset
        {
            get
            {
                return this._controlBoxOffset;
            }
            set
            {
                if (this._controlBoxOffset != value)
                {
                    this._controlBoxOffset = value;
                    base.Invalidate();
                }
            }
        }

        [Description("设置或获取最大化（还原）按钮的大小"), DefaultValue(typeof(System.Drawing.Size), "32, 18"), Category("MaximizeBox")]
        public Size MaxSize
        {
            get
            {
                return this._maxBoxSize;
            }
            set
            {
                if (this._maxBoxSize != value)
                {
                    this._maxBoxSize = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(typeof(System.Drawing.Size), "32, 18"), Description("设置或获取最小化按钮的大小"), Category("MinimizeBox")]
        public System.Drawing.Size MiniSize
        {
            get
            {
                return this._minBoxiSize;
            }
            set
            {
                if (this._minBoxiSize != value)
                {
                    this._minBoxiSize = value;
                    base.Invalidate();
                }
            }
        }

        [Description("设置或获取控制按钮的间距"), DefaultValue(0), Category("Skin")]
        public int ControlBoxSpace
        {
            get
            {
                return this._controlBoxSpace;
            }
            set
            {
                if (this._controlBoxSpace != value)
                {
                    this._controlBoxSpace = value;
                    base.Invalidate();
                }
            }
        }

        //[Category("SysBottom"), DefaultValue(typeof(System.Drawing.Size), "28, 20"), Description("设置或获取自定义系统按钮的大小")]
        //public System.Drawing.Size SysBottomSize
        //{
        //    get
        //    {
        //        return this._sysBottomSize;
        //    }
        //    set
        //    {
        //        if (this._sysBottomSize != value)
        //        {
        //            this._sysBottomSize = value;
        //            base.Invalidate();
        //        }
        //    }
        //}

        [Description("设置或获取窗体标题的字体"), DefaultValue(typeof(Font), "CaptionFont"), Category("Caption")]
        public Font CaptionFont
        {
            get
            {
                return this._captionFont;
            }
            set
            {
                if (value == null)
                {
                    this._captionFont = SystemFonts.CaptionFont;
                }
                else
                {
                    this._captionFont = value;
                }
                base.Invalidate(this.CaptionRect);
            }
        }

        public Rectangle CaptionRect
        {
            get
            {
                return new Rectangle(0, 0, base.Width, this.CaptionHeight);
            }
        }

        [Category("Caption"), Description("发光字体背景色"), DefaultValue(typeof(Color), "White")]
        public Color EffectBack
        {
            get
            {
                return this._effectBack;
            }
            set
            {
                if (this._effectBack != value)
                {
                    this._effectBack = value;
                    base.Invalidate();
                }
            }
        }

        [Category("Caption")]
        [Description("获取或设置标题的绘制模式")]
        [DefaultValue(TitleType.EffectTitle)]
        public TitleType EffectCaption
        {
            get
            {
                return this._effectCaption;
            }
            set
            {
                if (this._effectCaption != value)
                {
                    this._effectCaption = value;
                    base.Invalidate();
                }
            }
        }

        [Category("Caption"), Description("光圈大小"), DefaultValue(typeof(int), "6")]
        public int EffectWidth
        {
            get
            {
                return this._effectWidth;
            }
            set
            {
                if (this._effectWidth != value)
                {
                    this._effectWidth = value;
                    base.Invalidate();
                }
            }
        }


        [Category("CloseBox"), Description("关闭按钮点击时背景")]
        public Image CloseDownBack
        {
            get
            {
                return this._closeDownBack;
            }
            set
            {
                if (this._closeDownBack != value)
                {
                    this._closeDownBack = value;
                    base.Invalidate();
                }
            }
        }
        [Category("CloseBox"), Description("关闭按钮悬浮时背景")]
        public Image CloseMouseBack
        {
            get
            {
                return this._closeMouseBack;
            }
            set
            {
                if (this._closeMouseBack != value)
                {
                    this._closeMouseBack = value;
                    base.Invalidate();
                }
            }
        }

        [Description("关闭按钮初始时背景"), Category("CloseBox")]
        public Image CloseNormlBack
        {
            get
            {
                return this._closeNormlBack;
            }
            set
            {
                if (this._closeNormlBack != value)
                {
                    this._closeNormlBack = value;
                    base.Invalidate();
                }
            }
        }


        [Description("还原按钮悬浮时背景"), Category("MaximizeBox")]
        public Image RestoreMouseBack
        {
            get
            {
                return this._restoreMouseBack;
            }
            set
            {
                if (this._restoreMouseBack != value)
                {
                    this._restoreMouseBack = value;
                    base.Invalidate();
                }
            }
        }

        [Description("还原按钮初始时背景"), Category("MaximizeBox")]
        public Image RestoreNormlBack
        {
            get
            {
                return this._restoreNormlBack;
            }
            set
            {
                if (this._restoreNormlBack != value)
                {
                    this._restoreNormlBack = value;
                    base.Invalidate();
                }
            }
        }

        [Category("MaximizeBox"), Description("还原按钮点击时背景")]
        public Image RestoreDownBack
        {
            get
            {
                return this._restoreDownBack;
            }
            set
            {
                if (this._restoreDownBack != value)
                {
                    this._restoreDownBack = value;
                    base.Invalidate();
                }
            }
        }


        [Description("最小化按钮点击时背景"), Category("MinimizeBox")]
        public Image MiniDownBack
        {
            get
            {
                return this._miniDownBack;
            }
            set
            {
                if (this._miniDownBack != value)
                {
                    this._miniDownBack = value;
                    base.Invalidate();
                }
            }
        }

        [Category("MinimizeBox"), Description("最小化按钮悬浮时背景")]
        public Image MiniMouseBack
        {
            get
            {
                return this._miniMouseBack;
            }
            set
            {
                if (this._miniMouseBack != value)
                {
                    this._miniMouseBack = value;
                    base.Invalidate();
                }
            }
        }

        [Description("最小化按钮初始时背景"), Category("MinimizeBox")]
        public Image MiniNormlBack
        {
            get
            {
                return this._miniNormlBack;
            }
            set
            {
                if (this._miniNormlBack != value)
                {
                    this._miniNormlBack = value;
                    base.Invalidate();
                }
            }
        }

        [Description("最大化按钮点击时背景"), Category("MaximizeBox")]
        public Image MaxDownBack
        {
            get
            {
                return this._maxDownBack;
            }
            set
            {
                if (this._maxDownBack != value)
                {
                    this._maxDownBack = value;
                    base.Invalidate();
                }
            }
        }

        [Description("最大化按钮悬浮时背景"), Category("MaximizeBox")]
        public Image MaxMouseBack
        {
            get
            {
                return this._maxMouseBack;
            }
            set
            {
                if (this._maxMouseBack != value)
                {
                    this._maxMouseBack = value;
                    base.Invalidate();
                }
            }
        }

        [Description("最大化按钮初始时背景"), Category("MaximizeBox")]
        public Image MaxNormlBack
        {
            get
            {
                return this._maxNormlBack;
            }
            set
            {
                if (this._maxNormlBack != value)
                {
                    this._maxNormlBack = value;
                    base.Invalidate();
                }
            }
        }

        //[Description("自定义系统按钮点击时"), Category("SysBottom")]
        //public Image SysBottomDown
        //{
        //    get
        //    {
        //        return this._sysBottomDown;
        //    }
        //    set
        //    {
        //        if (this._sysBottomDown != value)
        //        {
        //            this._sysBottomDown = value;
        //            base.Invalidate();
        //        }
        //    }
        //}

        //[Description("自定义系统按钮悬浮时"), Category("SysBottom")]
        //public Image SysBottomMouse
        //{
        //    get
        //    {
        //        return this._sysBottomMouse;
        //    }
        //    set
        //    {
        //        if (this._sysBottomMouse != value)
        //        {
        //            this._sysBottomMouse = value;
        //            base.Invalidate();
        //        }
        //    }
        //}

        //[Description("自定义系统按钮初始时"), Category("SysBottom")]
        //public Image SysBottomNorml
        //{
        //    get
        //    {
        //        return this._sysBottomNorml;
        //    }
        //    set
        //    {
        //        if (this._sysBottomNorml != value)
        //        {
        //            this._sysBottomNorml = value;
        //            base.Invalidate();
        //        }
        //    }
        //}

        [Description("背景"), Category("Skin")]
        public Image Back
        {
            get
            {
                return this._back;
            }
            set
            {
                if (this._back != value)
                {
                    this.OnBackChanged(new BackEventArgs(this._back, value));
                    this._back = value;
                    if (this.BackToColor && (this._back != null))
                    {
                        this.BackColor = BitmapHelper.GetImageAverageColor((Bitmap)this._back);
                    }
                    base.Invalidate();
                }
            }
        }

        [Category("Skin"), Description("是否从左绘制背景")]
        public bool BackLayout
        {
            get
            {
                return this._backLayout;
            }
            set
            {
                if (this._backLayout != value)
                {
                    this._backLayout = value;
                    base.Invalidate();
                }
            }
        }

        [Category("Skin"), Description("质感层背景")]
        public Image BackPalace
        {
            get
            {
                return this._backPalace;
            }
            set
            {
                if (this._backPalace != value)
                {
                    this._backPalace = value;
                    base.Invalidate();
                }
            }
        }

        [Category("Skin"), Description("质感层九宫绘画区域"), DefaultValue(typeof(Rectangle), "10,10,10,10")]
        public Rectangle BackRectangle
        {
            get
            {
                return this._backRectangle;
            }
            set
            {
                if (this._backRectangle != value)
                {
                    this._backRectangle = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(true), Description("是否根据背景图决定背景色，并加入背景渐变效果"), Category("Skin")]
        public bool BackToColor
        {
            get
            {
                return this._backToColor;
            }
            set
            {
                if (this._backToColor != value)
                {
                    this._backToColor = value;
                    base.Invalidate();
                }
            }
        }

        [Description("边框层背景"), Category("Skin")]
        public Image BorderPalace
        {
            get
            {
                return this._borderPalace;
            }
            set
            {
                if (this._borderPalace != value)
                {
                    this._borderPalace = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(typeof(Rectangle), "10,10,10,10"), Description("边框质感层九宫绘画区域"), Category("Skin")]
        public Rectangle BorderRectangle
        {
            get
            {
                return this._borderRectangle;
            }
            set
            {
                if (this._borderRectangle != value)
                {
                    this._borderRectangle = value;
                    base.Invalidate();
                }
            }
        }

        [Description("移动窗体的条件"), DefaultValue(typeof(MobileStyle), "2"), Category("Skin")]
        public MobileStyle Mobile
        {
            get
            {
                return this._mobile;
            }
            set
            {
                if (this._mobile != value)
                {
                    this._mobile = value;
                }
            }
        }

        [Description("指示控件是否可以将用户拖动到背景上的图片作为背景(注意:开启前请设置AllowDrop为true,否则无效)"), DefaultValue(true), Category("Skin")]
        public bool DropBack
        {
            get
            {
                return this._dropBack;
            }
            set
            {
                if (this._dropBack != value)
                {
                    this._dropBack = value;
                }
            }
        }

        [Category("Skin"), DefaultValue(true), Description("是否启用窗口淡入淡出")]
        public bool Special
        {
            get
            {
                return this._special;
            }
            set
            {
                if (this._special != value)
                {
                    this._special = value;
                }
            }
        }

        [Category("Skin"), Description("窗体渐变后透明度")]
        public double SkinOpacity
        {
            get
            {
                return this._skinOpacity;
            }
            set
            {
                if (this._skinOpacity != value)
                {
                    this._skinOpacity = value;
                }
            }
        }

        [Description("是否启用窗体阴影"), DefaultValue(true), Category("Skin")]
        public bool Shadow
        {
            get
            {
                return this._shadow;
            }
            set
            {
                if (this._shadow != value)
                {
                    this._shadow = value;
                }
            }
        }


        [Category("Shadow"), DefaultValue(typeof(Color), "Black"), Description("窗体阴影颜色")]
        public Color ShadowColor
        {
            get
            {
                return this._shadowColor;
            }
            set
            {
                if (this._shadowColor != value)
                {
                    this._shadowColor = value;
                    if (this._formShadow != null)
                    {
                        this._formShadow.SetBits();
                    }
                }
            }
        }

        [Category("Shadow"), DefaultValue(typeof(int), "4"), Description("窗体阴影宽度")]
        public int ShadowWidth
        {
            get
            {
                return this._shadowWidth;
            }
            set
            {
                if (this._shadowWidth != value)
                {
                    this._shadowWidth = (value < 1) ? 1 : value;
                    if (this._formShadow != null)
                    {
                        this._formShadow.SetBits();
                    }
                }
            }
        }

        [Description("是否继承所属窗体的背景")]
        [Category("Skin")]
        public bool InheritBack
        {
            get
            {
                return this._inheritBack;
            }
            set
            {
                this._inheritBack = value;
            }
        }

        [Category("Caption")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("标题颜色")]
        public Color TitleColor
        {
            get
            {
                return this._titleColor;
            }
            set
            {
                if (!(this._titleColor != value))
                    return;
                this._titleColor = value;
                this.Invalidate();
            }
        }

        [Category("Caption")]
        [Description("设置或获取标题的偏移")]
        [DefaultValue(typeof(System.Drawing.Point), "0,0")]
        public System.Drawing.Point TitleOffset
        {
            get
            {
                return this._titleOffset;
            }
            set
            {
                if (!(this._titleOffset != value))
                    return;
                this._titleOffset = value;
                this.Invalidate();
            }
        }

        [DefaultValue(false)]
        [Category("Caption")]
        [Description("是否根据背景色自动适应标题颜色。\n(背景色为暗色时标题显示白色，背景为亮色时标题显示黑色。)")]
        public bool TitleSuitColor
        {
            get
            {
                return this._titleSuitColor;
            }
            set
            {
                if (this._titleSuitColor == value)
                    return;
                this._titleSuitColor = value;
                this.Invalidate();
            }
        }

        [Description("设置或获取窗体的圆角样式")]
        [Category("Skin")]
        [DefaultValue(typeof(RoundStyle), "1")]
        public RoundStyle RoundStyle
        {
            get
            {
                return this._roundStyle;
            }
            set
            {
                if (this._roundStyle == value)
                    return;
                this._roundStyle = value;
                this.SetReion();
                this.Invalidate();
            }
        }

        [Category("SysButton"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("自定义系统按钮集合的项")]
        public CustomSysButtonCollection SysButtonItems
        {
            get
            {
                if (this._sysButtonItems == null)
                {
                    this._sysButtonItems = new CustomSysButtonCollection(this);
                }
                return this._sysButtonItems;
            }
        }

        #endregion

        #region >扩展属性<

        protected internal Padding BorderPadding
        {
            get
            {
                return this._borderPadding;
            }
            set
            {
                this._borderPadding = value;
            }
        }

        public Rectangle IconRect
        {
            get
            {
                if (!this.ShowIcon || (base.Icon == null))
                {
                    return Rectangle.Empty;
                }
                int width = SystemInformation.SmallIconSize.Width;
                if (((this.CaptionHeight - this.BorderWidth) - 4) < width)
                {
                    width = (this.CaptionHeight - this.BorderWidth) - 4;
                }
                return new Rectangle(this.BorderWidth, this.BorderWidth + (((this.CaptionHeight - this.BorderWidth) - width) / 2), width, width);
            }
        }

        public ControlBoxManager ControlBoxManager
        {
            get
            {
                if (this._controlBoxManager == null)
                {
                    this._controlBoxManager = new ControlBoxManager(this);
                }
                return this._controlBoxManager;
            }
        }

        protected Rectangle RealClientRect
        {
            get
            {
                if (base.WindowState == FormWindowState.Maximized)
                {
                    return new Rectangle(this._deltaRect.X, this._deltaRect.Y, base.Width - this._deltaRect.Width, base.Height - this._deltaRect.Height);
                }
                return new Rectangle(System.Drawing.Point.Empty, base.Size);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("设置或获取窗体的绘制方法")]
        public SkinRendererBase Renderer
        {
            get
            {
                if (this._renderer == null)
                {
                    this._renderer = new FormSkinRenderer();
                }
                return this._renderer;
            }
            set
            {
                this._renderer = value;
                this.OnRendererChanged(EventArgs.Empty);
            }
        }

        public event EventHandler RendererChangled
        {
            add
            {
                base.Events.AddHandler(EventRendererChanged, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventRendererChanged, value);
            }
        }

        public bool IsMouseDown
        {
            get { return _isMouseDown; }
        }

        public AnchorStyles Aanhor { get; set; }
        [Category("Skin"), Description("自定义按钮被点击时引发的事件")]
        public event SysBottomEventHandler SysBottomClick;
        #endregion >扩展属性<

        public FormBase()
        {
            Initialize();
        }

        #region 自定义方法
        /// <summary>
        /// 最原始初始化方法
        /// </summary>
        public virtual void Initialize()
        {
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            base.UpdateStyles();
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            base.BackgroundImageLayout = ImageLayout.None;
        }

        protected virtual void OnBackChanged(BackEventArgs e)
        {
            if (this.BackChanged != null)
            {
                this.BackChanged(this, e);
            }
        }

        protected virtual void OnRendererChanged(EventArgs e)
        {
            this.Renderer.InitSkinForm(this);
            EventHandler handler = base.Events[EventRendererChanged] as EventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
            base.Invalidate();
        }

        private void SetReion()
        {
            if (this.Region != null)
                this.Region.Dispose();
            SkinTools.CreateRegion((Control)this, this.RealClientRect, this.Radius, this.RoundStyle);
        }

        private void main_BackChanged(object sender, BackEventArgs e)
        {
            if (this.InheritBack)
            {
                FormBase main = (FormBase)sender;
                this.BackToColor = true;
                this.Back = main.Back;
                this.BackLayout = main.BackLayout;
            }
        }

        private void main_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (this.InheritBack)
            {
                Form form = (Form)sender;
                this.Back = form.BackgroundImage;
                this.BackLayout = true;
                this.BackColor = (form.BackgroundImage == null) ? form.BackColor : SkinTools.GetImageAverageColor((Bitmap)form.BackgroundImage);
            }
        }

        protected virtual void ResizeCore()
        {
            this.CalcDeltaRect();
            this.SetReion();
        }

        protected void CalcDeltaRect()
        {
            if (base.WindowState == FormWindowState.Maximized)
            {
                Rectangle bounds = base.Bounds;
                Rectangle workingArea = Screen.GetWorkingArea(this);
                workingArea.X -= this._borderPadding.Left;
                workingArea.Y -= this._borderPadding.Top;
                workingArea.Width += this._borderPadding.Horizontal;
                workingArea.Height += this._borderPadding.Vertical;
                int x = 0;
                int y = 0;
                int width = 0;
                int height = 0;
                if (bounds.Left < workingArea.Left)
                {
                    x = workingArea.Left - bounds.Left;
                }
                if (bounds.Top < workingArea.Top)
                {
                    y = workingArea.Top - bounds.Top;
                }
                if (bounds.Width > workingArea.Width)
                {
                    width = bounds.Width - workingArea.Width;
                }
                if (bounds.Height > workingArea.Height)
                {
                    height = bounds.Height - workingArea.Height;
                }
                this._deltaRect = new Rectangle(x, y, width, height);
            }
            else
            {
                this._deltaRect = Rectangle.Empty;
            }
        }

        public virtual void OnSysBottomClick(object sender, SysButtonEventArgs e)
        {
            if (this.SysBottomClick != null)
            {
                this.SysBottomClick(this, e);
            }
        }

        private void WmGetMinMaxInfo(ref Message m)
        {
            MINMAXINFO structure = (MINMAXINFO)Marshal.PtrToStructure(m.LParam, typeof(MINMAXINFO));
            if (this.MaximumSize != System.Drawing.Size.Empty)
            {
                structure.maxTrackSize = this.MaximumSize;
            }
            else
            {
                Rectangle workingArea = Screen.GetWorkingArea(this);
                int num = (this.FormBorderStyle == System.Windows.Forms.FormBorderStyle.None) ? 0 : -1;
                structure.maxPosition = new System.Drawing.Point(workingArea.X, workingArea.Y);
                structure.maxTrackSize = new System.Drawing.Size(workingArea.Width, workingArea.Height + num);
            }
            if (this.MinimumSize != System.Drawing.Size.Empty)
            {
                structure.minTrackSize = this.MinimumSize;
            }
            else
            {
                this.GetDefaultMinTrackSize();
                structure.minTrackSize = new System.Drawing.Size((((this.AllButtonWidth(true) + this.ControlBoxOffset.X) + SystemInformation.SmallIconSize.Width) + (this.BorderPadding.Left * 2)) + 3, this.CaptionHeight);
            }
            Marshal.StructureToPtr(structure, m.LParam, false);
        }

        protected virtual System.Drawing.Size GetDefaultMinTrackSize()
        {
            return new System.Drawing.Size(((this.AllButtonWidth(true) + this._borderPadding.Horizontal) + SystemInformation.SmallIconSize.Width) + 20, (this.CaptionHeight + this._borderPadding.Vertical) + 2);
        }

        protected int AllButtonWidth(bool space)
        {
            int num = 0;
            foreach (CmSysButton button in ControlBoxManager.SysButtonItems)
            {
                if (button.Visibale)
                {
                    num += button.Size.Width;
                    if (space)
                    {
                        num += this.ControlBoxSpace;
                    }
                }
            }
            return (num + ((this.CloseBoxSize.Width + (base.MinimizeBox ? (this.MiniSize.Width + (space ? this.ControlBoxSpace : 0)) : 0)) + (base.MaximizeBox ? (this.MaxSize.Width + (space ? this.ControlBoxSpace : 0)) : 0)));
        }

        protected int AllSysButtonWidth(bool space)
        {
            int num = 0;
            foreach (CmSysButton button in (IEnumerable)this.ControlBoxManager.SysButtonItems)
            {
                if (button.Visibale)
                {
                    num += button.Size.Width;
                    if (space)
                    {
                        num += this.ControlBoxSpace;
                    }
                }
            }
            return num;
        }

        protected virtual void WmNcCalcSize(ref Message m)
        {
            if (base.Opacity != 1.0)
            {
                base.Invalidate();
            }
        }

        protected virtual void WmNcRButtonUp(ref Message m)
        {
            this.TrackPopupSysMenu(ref m);
            base.WndProc(ref m);
        }

        protected void TrackPopupSysMenu(ref Message m)
        {
            if (m.WParam.ToInt32() == 2)
            {
                this.TrackPopupSysMenu(m.HWnd, new System.Drawing.Point(m.LParam.ToInt32()));
            }
        }

        protected void TrackPopupSysMenu(IntPtr hWnd, System.Drawing.Point point)
        {
            if (this._showSystemMenu && (point.Y <= (((base.Top + this._borderPadding.Top) + this._deltaRect.Y) + this._captionHeight)))
            {
                IntPtr wParam = NativeMethods.TrackPopupMenu(NativeMethods.GetSystemMenu(hWnd, false), 0x100, point.X, point.Y, 0, hWnd, IntPtr.Zero);
                NativeMethods.PostMessage(hWnd, 0x112, wParam, IntPtr.Zero);
            }
        }

        protected virtual void WmWindowPosChanged(ref Message m)
        {
            this._inWmWindowPosChanged++;
            base.WndProc(ref m);
            this._inWmWindowPosChanged--;
        }

        //public void SysbottomAv(object e, Point mousePoint)
        //{

        //}

        #endregion 自定义方法

        #region >重写底层方法<
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.SetReion();
            if ((base.Owner is FormBase) && this.InheritBack)
            {
                FormBase owner = (FormBase)base.Owner;
                this.BackToColor = true;
                this.Back = owner.Back;
                this.BackLayout = owner.BackLayout;
                owner.BackChanged += new BackEventHandler(this.main_BackChanged);
            }
            else if ((base.Owner != null) && this.InheritBack)
            {
                Form form = base.Owner;
                this.Back = form.BackgroundImage;
                this.BackLayout = true;
                this.BackColor = (form.BackgroundImage == null) ? form.BackColor : SkinTools.GetImageAverageColor((Bitmap)form.BackgroundImage);
                form.BackgroundImageChanged += new EventHandler(this.main_BackgroundImageChanged);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (this._formShadow != null)
            {
                this._formShadow.Close();
            }
            if (this.Special && !base.DesignMode)
            {
                NativeMethods.AnimateWindow(base.Handle, 150, 0x90000);
                base.Update();
            }
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            if (this.DropBack)
            {
                string[] data = (string[])drgevent.Data.GetData(DataFormats.FileDrop);
                FileInfo info = new FileInfo(data[0]);
                if (data != null)
                {
                    string str = info.Extension.Substring(1);
                    List<string> strArray2 = new List<string>(new string[] { "png", "bmp", "jpg", "jpeg", "gif" });
                    if (strArray2.Contains(str.ToLower()))
                    {
                        this.Back = Image.FromFile(data[0]);
                    }
                }
            }
            base.OnDragDrop(drgevent);
        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            if (this.DropBack)
            {
                drgevent.Effect = DragDropEffects.Link;
            }
            base.OnDragEnter(drgevent);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.ResizeCore();
        }

        //protected override void OnSizeChanged(EventArgs e)
        //{
        //    base.OnSizeChanged(e);
        //    this.SetReion();
        //}

        protected override void OnStyleChanged(EventArgs e)
        {
            if (this._clientSizeSet)
            {
                this.ClientSize = this.ClientSize;
                this._clientSizeSet = false;
            }
            base.OnStyleChanged(e);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && this.MaximizeBox)
            {
                bool flag = true;
                System.Drawing.Point location = e.Location;
                foreach (CmSysButton cmSysButton in (IEnumerable)this.ControlBoxManager.SysButtonItems)
                {
                    if (cmSysButton.Bounds.Contains(location))
                    {
                        flag = false;
                        break;
                    }
                }
                if (!this.ControlBoxManager.CloseBoxRect.Contains(location) && !this.ControlBoxManager.MaximizeBoxRect.Contains(location) && (!this.ControlBoxManager.MinimizeBoxRect.Contains(location) && flag))
                {
                    if (this.Mobile == MobileStyle.Mobile)
                    {
                        base.WindowState = (base.WindowState == FormWindowState.Maximized) ? FormWindowState.Normal : FormWindowState.Maximized;
                    }
                    else if ((this.Mobile == MobileStyle.TitleMobile) && (e.Y < this.CaptionHeight))
                    {
                        base.WindowState = (base.WindowState == FormWindowState.Maximized) ? FormWindowState.Normal : FormWindowState.Maximized;
                    }
                }
            }
            base.OnMouseDoubleClick(e);

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            if (this.Back != null)
            {
                if (this.BackLayout)
                {
                    graphics.DrawImage(this.Back, 0, 0, this.Back.Width, this.Back.Height);
                }
                else
                {
                    graphics.DrawImage(this.Back, -(this.Back.Width - base.Width), 0, this.Back.Width, this.Back.Height);
                }
            }
            if ((this.Back != null) && this.BackToColor)
            {
                if (this.BackLayout)
                {
                    LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(this.Back.Width - 50, 0, 50, this.Back.Height), this.BackColor, Color.Transparent, 180f);
                    LinearGradientBrush brush2 = new LinearGradientBrush(new Rectangle(0, this.Back.Height - 50, this.Back.Width, 50), this.BackColor, Color.Transparent, 270f);
                    graphics.FillRectangle(brush, (this.Back.Width - brush.Rectangle.Width) + 1f, 0f, brush.Rectangle.Width, brush.Rectangle.Height);
                    graphics.FillRectangle(brush2, 0f, (this.Back.Height - brush2.Rectangle.Height) + 1f, brush2.Rectangle.Width, brush2.Rectangle.Height);
                }
                else
                {
                    LinearGradientBrush brush3 = new LinearGradientBrush(new Rectangle(-(this.Back.Width - base.Width), 0, 50, this.Back.Height), this.BackColor, Color.Transparent, 360f);
                    LinearGradientBrush brush4 = new LinearGradientBrush(new Rectangle(-(this.Back.Width - base.Width), this.Back.Height - 50, this.Back.Width, 50), this.BackColor, Color.Transparent, 270f);
                    graphics.FillRectangle(brush3, (float)-(this.Back.Width - base.Width), 0f, brush3.Rectangle.Width, brush3.Rectangle.Height);
                    graphics.FillRectangle(brush4, (float)-(this.Back.Width - base.Width), (float)(this.Back.Height - 50), brush4.Rectangle.Width, brush4.Rectangle.Height);
                }
            }
            base.OnPaint(e);
            Rectangle clientRectangle = base.ClientRectangle;
            SkinRendererBase renderer = this.Renderer;
            if (this.ControlBoxManager.CloseBoxVisibale)
            {
                renderer.DrawSkinFormControlBox(new SkinFormControlBoxRenderEventArgs(this, graphics, this.ControlBoxManager.CloseBoxRect, this._active, ControlBoxStyle.Close, this.ControlBoxManager.CloseBoxState, null));
            }
            if (this.ControlBoxManager.MaximizeBoxVisibale)
            {
                renderer.DrawSkinFormControlBox(new SkinFormControlBoxRenderEventArgs(this, graphics, this.ControlBoxManager.MaximizeBoxRect, this._active, ControlBoxStyle.Maximize, this.ControlBoxManager.MaximizeBoxState, null));
            }
            if (this.ControlBoxManager.MinimizeBoxVisibale)
            {
                renderer.DrawSkinFormControlBox(new SkinFormControlBoxRenderEventArgs(this, graphics, this.ControlBoxManager.MinimizeBoxRect, this._active, ControlBoxStyle.Minimize, this.ControlBoxManager.MinimizeBoxState, null));
            }
            foreach (CmSysButton button in (IEnumerable) this.ControlBoxManager.SysButtonItems)
            {
                if (button.Visibale)
                {
                    renderer.DrawSkinFormControlBox(new SkinFormControlBoxRenderEventArgs(this, graphics, button.Bounds, this._active, ControlBoxStyle.CmSysBottom, button.BoxState, button));
                }
            }
            //if (this.ShowBorder)
            //{
            //    renderer.DrawSkinFormBorder(new SkinFormBorderRenderEventArgs(this, graphics, clientRectangle, this._active));
            //}
            if (this.BackPalace != null)
            {
                ImageDrawRect.DrawRect(graphics, (Bitmap)this.BackPalace, new Rectangle(base.ClientRectangle.X - 5, base.ClientRectangle.Y - 5, base.ClientRectangle.Width + 10, base.ClientRectangle.Height + 10), Rectangle.FromLTRB(this.BackRectangle.X, this.BackRectangle.Y, this.BackRectangle.Width, this.BackRectangle.Height), 1, 1);
            }
            if (this.BorderPalace != null)
            {
                ImageDrawRect.DrawRect(graphics, (Bitmap)this.BorderPalace, new Rectangle(base.ClientRectangle.X - 5, base.ClientRectangle.Y - 5, base.ClientRectangle.Width + 10, base.ClientRectangle.Height + 10), Rectangle.FromLTRB(this.BorderRectangle.X, this.BorderRectangle.Y, this.BorderRectangle.Width, this.BorderRectangle.Height), 1, 1);
            }
            renderer.DrawSkinFormCaption(new SkinFormCaptionRenderEventArgs(this, graphics, this.CaptionRect, this._active));
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            System.Drawing.Point location = e.Location;
            if ((e.Button == MouseButtons.Left) && (e.Clicks == 1))
            {
                bool flag = true;
                foreach (CmSysButton cmSysButton in (IEnumerable)this.ControlBoxManager.SysButtonItems)
                {
                    if (cmSysButton.Bounds.Contains(location))
                    {
                        flag = false;
                        break;
                    }
                }

                if (((!this.ControlBoxManager.CloseBoxRect.Contains(location) && !this.ControlBoxManager.MaximizeBoxRect.Contains(location)) && (!this.ControlBoxManager.MinimizeBoxRect.Contains(location) && (this.Mobile != MobileStyle.None))) && flag)
                {
                    this._isMouseDown = true;
                    if (this.Mobile == MobileStyle.Mobile)
                    {
                        NativeMethods.ReleaseCapture();
                        NativeMethods.SendMessage(base.Handle, 0x112, 0xf011, 0);
                    }
                    else if ((this.Mobile == MobileStyle.TitleMobile) && (location.Y < this.CaptionHeight))
                    {
                        NativeMethods.ReleaseCapture();
                        NativeMethods.SendMessage(base.Handle, 0x112, 0xf011, 0);
                    }
                    this.OnClick(e);
                    this.OnMouseClick(e);
                    this.OnMouseUp(e);
                }
                else
                {
                    this.ControlBoxManager.ProcessMouseOperate(e.Location, MouseOperate.Down);
                }
            }
        }

        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);
            this.ControlBoxManager.ProcessMouseOperate(base.PointToClient(Control.MousePosition), MouseOperate.Hover);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.ControlBoxManager.ProcessMouseOperate(System.Drawing.Point.Empty, MouseOperate.Leave);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            this.ControlBoxManager.ProcessMouseOperate(e.Location, MouseOperate.Move);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            this._isMouseDown = false;
            base.OnMouseUp(e);
            this.ControlBoxManager.ProcessMouseOperate(e.Location, MouseOperate.Up);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (base.Visible)
            {
                if (this.Special && !base.DesignMode)
                {
                    NativeMethods.AnimateWindow(base.Handle, 150, 0xa0000);
                    base.Opacity = this.SkinOpacity;
                }
                if ((!base.DesignMode && (this._formShadow == null)) && this.Shadow)
                {
                    this._formShadow = new FormShadow(this);
                    this._formShadow.Show(this);
                }
                base.OnVisibleChanged(e);
            }
            else
            {
                base.OnVisibleChanged(e);
                if (this.Special)
                {
                    base.Opacity = 1.0;
                    NativeMethods.AnimateWindow(base.Handle, 150, 0x90000);
                }
            }
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            this.StopAnthor();
        }

        protected override void OnResize(EventArgs e)
        {
            this.ResizeCore();
            base.OnResize(e);
        }

        private void StopAnthor()
        {
            if (this.Left <= 0)
                this.Aanhor = AnchorStyles.Left;
            else if (this.Left >= Screen.PrimaryScreen.Bounds.Width - this.Width)
                this.Aanhor = AnchorStyles.Right;
            else if (this.Top <= 0)
                this.Aanhor = AnchorStyles.Top;
            else
                this.Aanhor = AnchorStyles.None;
        }

        protected override void SetClientSizeCore(int x, int y)
        {
            this._clientSizeSet = false;
            System.Type type = typeof(Control);
            System.Type type2 = typeof(Form);
            FieldInfo field = type.GetField("clientWidth", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo info2 = type.GetField("clientHeight", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo info3 = type2.GetField("FormStateSetClientSize", BindingFlags.NonPublic | BindingFlags.Static);
            FieldInfo info4 = type2.GetField("formState", BindingFlags.NonPublic | BindingFlags.Instance);
            if (((field != null) && (info2 != null)) && ((info4 != null) && (info3 != null)))
            {
                this._clientSizeSet = true;
                base.Size = new System.Drawing.Size(x, y);
                field.SetValue(this, x);
                info2.SetValue(this, y);
                BitVector32.Section section = (BitVector32.Section)info3.GetValue(this);
                BitVector32 vector = (BitVector32)info4.GetValue(this);
                vector[section] = 1;
                info4.SetValue(this, vector);
                this.OnClientSizeChanged(EventArgs.Empty);
                vector[section] = 0;
                info4.SetValue(this, vector);
            }
            else
            {
                base.SetClientSizeCore(x, y);
            }
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (this._inWmWindowPosChanged != 0)
            {
                try
                {
                    System.Type type = typeof(Form);
                    FieldInfo field = type.GetField("FormStateExWindowBoundsWidthIsClientSize", BindingFlags.NonPublic | BindingFlags.Static);
                    FieldInfo info2 = type.GetField("formStateEx", BindingFlags.NonPublic | BindingFlags.Instance);
                    FieldInfo info3 = type.GetField("restoredWindowBounds", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (((field != null) && (info2 != null)) && (info3 != null))
                    {
                        Rectangle rectangle = (Rectangle)info3.GetValue(this);
                        BitVector32.Section section = (BitVector32.Section)field.GetValue(this);
                        BitVector32 vector = (BitVector32)info2.GetValue(this);
                        if (vector[section] == 1)
                        {
                            width = rectangle.Width;
                            height = rectangle.Height;
                        }
                    }
                }
                catch
                {
                }
            }
            base.SetBoundsCore(x, y, width, height, specified);
        }

        protected override System.Drawing.Size SizeFromClientSize(System.Drawing.Size clientSize)
        {
            return clientSize;
        }

        #endregion >重写底层方法<



        #region >WindowsMessage<
        private void WmNcActive(ref Message m)
        {
            if (m.WParam.ToInt32() == 1)
            {
                this._active = true;
            }
            else
            {
                this._active = false;
            }
            m.Result = Result.TRUE;
            base.Invalidate();
        }

        private void WmNcHitTest(ref Message m)
        {
            System.Drawing.Point p = new System.Drawing.Point(m.LParam.ToInt32());
            p = base.PointToClient(p);
            if (this.IconRect.Contains(p) && this.ShowSystemMenu)
            {
                m.Result = new IntPtr(3);
            }
            else
            {
                if (this._canResize)
                {
                    if ((p.X < 5) && (p.Y < 5))
                    {
                        m.Result = new IntPtr(13);
                        return;
                    }
                    if ((p.X > (base.Width - 5)) && (p.Y < 5))
                    {
                        m.Result = new IntPtr(14);
                        return;
                    }
                    if ((p.X < 5) && (p.Y > (base.Height - 5)))
                    {
                        m.Result = new IntPtr(0x10);
                        return;
                    }
                    if ((p.X > (base.Width - 5)) && (p.Y > (base.Height - 5)))
                    {
                        m.Result = new IntPtr(0x11);
                        return;
                    }
                    if (p.Y < 3)
                    {
                        m.Result = new IntPtr(12);
                        return;
                    }
                    if (p.Y > (base.Height - 3))
                    {
                        m.Result = new IntPtr(15);
                        return;
                    }
                    if (p.X < 3)
                    {
                        m.Result = new IntPtr(10);
                        return;
                    }
                    if (p.X > (base.Width - 3))
                    {
                        m.Result = new IntPtr(11);
                        return;
                    }
                }
                m.Result = new IntPtr(1);
            }
        }


        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case (int)WindowsMessage.WM_GETMINMAXINFO:
                    this.WmGetMinMaxInfo(ref m);
                    return;

                case (int)WindowsMessage.WM_WINDOWPOSCHANGED:
                    this.WmWindowPosChanged(ref m);
                    return;

                case (int)WindowsMessage.WM_NCCALCSIZE:
                    this.WmNcCalcSize(ref m);
                    return;

                case (int)WindowsMessage.WM_NCHITTEST:
                    this.WmNcHitTest(ref m);
                    return;

                case (int)WindowsMessage.WM_NCPAINT:
                    break;

                case (int)WindowsMessage.WM_NCACTIVATE:
                    this.WmNcActive(ref m);
                    return;

                case (int)WindowsMessage.WM_NCRBUTTONUP:
                    this.WmNcRButtonUp(ref m);
                    return;

                case 0xae:
                case 0xaf:
                    m.Result = Result.TRUE;
                    return;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        #endregion >WindowsMessage<

        public delegate void BackEventHandler(object sender, BackEventArgs e);
        public delegate void SysBottomEventHandler(object sender, SysButtonEventArgs e);
    }
}
