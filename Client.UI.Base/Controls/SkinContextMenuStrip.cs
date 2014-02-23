using System;
using System.Collections.Generic;
using System.Text;
using Client.UI.Base.Enums;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Client.UI.Base.ColorStyle;
using Client.UI.Base.Render;

namespace Client.UI.Base.Controls
{
    [ToolboxBitmap(typeof(ContextMenuStrip))]
    public class SkinContextMenuStrip : ContextMenuStrip
    {
        private ToolStripColorTable colorTable;

        public SkinContextMenuStrip()
        {
            this.Init();
            this.colorTable = new ToolStripColorTable();
            this.PaintRenderer();
        }

        public void Init()
        {
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            base.UpdateStyles();
        }

        protected override void OnRendererChanged(EventArgs e)
        {
            if ((base.RenderMode == ToolStripRenderMode.ManagerRenderMode) || (base.RenderMode == ToolStripRenderMode.Professional))
            {
                base.Renderer = new ProfessionalToolStripRendererEx(this.colorTable);
            }
            base.OnRendererChanged(e);
        }

        public void PaintRenderer()
        {
            if (base.RenderMode != ToolStripRenderMode.System)
            {
                base.Renderer = new ProfessionalToolStripRendererEx(this.colorTable);
            }
        }

        [Description("箭头颜色"), Category("Skin")]
        public Color Arrow
        {
            get
            {
                return this.colorTable.Arrow;
            }
            set
            {
                this.colorTable.Arrow = value;
                this.PaintRenderer();
            }
        }

        [Description("控件背景色"), Category("Skin")]
        public Color Back
        {
            get
            {
                return this.colorTable.Back;
            }
            set
            {
                this.colorTable.Back = value;
                this.PaintRenderer();
            }
        }

        [Description("控件圆角大小"), Category("Skin")]
        public int BackRadius
        {
            get
            {
                return this.colorTable.BackRadius;
            }
            set
            {
                this.colorTable.BackRadius = (value < 1) ? 1 : value;
                this.PaintRenderer();
            }
        }

        [Description("Base背景颜色"), Category("Base")]
        public Color Base
        {
            get
            {
                return this.colorTable.Base;
            }
            set
            {
                this.colorTable.Base = value;
                this.PaintRenderer();
            }
        }

        [Category("Skin"), Description("弹出菜单分隔符与边框的颜色")]
        public Color DropDownImageSeparator
        {
            get
            {
                return this.colorTable.DropDownImageSeparator;
            }
            set
            {
                this.colorTable.DropDownImageSeparator = value;
                this.PaintRenderer();
            }
        }

        [Category("Skin"), Description("控件字体颜色")]
        public Color Fore
        {
            get
            {
                return this.colorTable.Fore;
            }
            set
            {
                this.colorTable.Fore = value;
                this.PaintRenderer();
            }
        }

        [Category("Skin"), Description("控件悬浮时字体颜色")]
        public Color HoverFore
        {
            get
            {
                return this.colorTable.HoverFore;
            }
            set
            {
                this.colorTable.HoverFore = value;
                this.PaintRenderer();
            }
        }

        [Category("Item"), Description("Item是否启用渐变")]
        public bool ItemAnamorphosis
        {
            get
            {
                return this.colorTable.ItemAnamorphosis;
            }
            set
            {
                this.colorTable.ItemAnamorphosis = value;
                this.PaintRenderer();
            }
        }

        [Category("Item"), Description("Item边框颜色")]
        public Color ItemBorder
        {
            get
            {
                return this.colorTable.ItemBorder;
            }
            set
            {
                this.colorTable.ItemBorder = value;
                this.PaintRenderer();
            }
        }

        [Category("Item"), Description("Item背景色是否启用渐变")]
        public bool ItemBorderShow
        {
            get
            {
                return this.colorTable.ItemBorderShow;
            }
            set
            {
                this.colorTable.ItemBorderShow = value;
                this.PaintRenderer();
            }
        }

        [Description("Item悬浮时背景色"), Category("Item")]
        public Color ItemHover
        {
            get
            {
                return this.colorTable.ItemHover;
            }
            set
            {
                this.colorTable.ItemHover = value;
                this.PaintRenderer();
            }
        }

        [Description("Item按下时背景色"), Category("Item")]
        public Color ItemPressed
        {
            get
            {
                return this.colorTable.ItemPressed;
            }
            set
            {
                this.colorTable.ItemPressed = value;
                this.PaintRenderer();
            }
        }

        [Category("Item"), Description("Item圆角大小")]
        public int ItemRadius
        {
            get
            {
                return this.colorTable.ItemRadius;
            }
            set
            {
                this.colorTable.ItemRadius = (value < 1) ? 1 : value;
                this.PaintRenderer();
            }
        }

        [Category("Item"), Description("Item圆角样式")]
        public RoundStyle ItemRadiusStyle
        {
            get
            {
                return this.colorTable.ItemRadiusStyle;
            }
            set
            {
                this.colorTable.ItemRadiusStyle = value;
                this.PaintRenderer();
            }
        }

        [Description("Item分隔符颜色"), Category("Item")]
        public Color ItemSplitter
        {
            get
            {
                return this.colorTable.BaseItemSplitter;
            }
            set
            {
                this.colorTable.BaseItemSplitter = value;
                this.PaintRenderer();
            }
        }

        [Category("Skin"), Description("控件圆角样式")]
        public RoundStyle RadiusStyle
        {
            get
            {
                return this.colorTable.RadiusStyle;
            }
            set
            {
                this.colorTable.RadiusStyle = value;
                this.PaintRenderer();
            }
        }

        [Category("Title"), Description("菜单标头背景色是否启用渐变")]
        public bool TitleAnamorphosis
        {
            get
            {
                return this.colorTable.TitleAnamorphosis;
            }
            set
            {
                this.colorTable.TitleAnamorphosis = value;
                this.PaintRenderer();
            }
        }

        [Category("Title"), Description("菜单标头背景色")]
        public Color TitleColor
        {
            get
            {
                return this.colorTable.TitleColor;
            }
            set
            {
                this.colorTable.TitleColor = value;
                this.PaintRenderer();
            }
        }

        [Category("Title"), Description("菜单标头圆角大小")]
        public int TitleRadius
        {
            get
            {
                return this.colorTable.TitleRadius;
            }
            set
            {
                this.colorTable.TitleRadius = (value < 1) ? 1 : value;
                this.PaintRenderer();
            }
        }

        [Category("Title"), Description("菜单标头圆角样式")]
        public RoundStyle TitleRadiusStyle
        {
            get
            {
                return this.colorTable.TitleRadiusStyle;
            }
            set
            {
                this.colorTable.TitleRadiusStyle = value;
                this.PaintRenderer();
            }
        }
    }
}
