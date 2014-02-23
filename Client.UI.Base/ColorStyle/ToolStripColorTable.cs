using System;
using System.Collections.Generic;
using System.Text;
using Client.UI.Base.Enums;
using System.Drawing;
using Client.UI.DefaultResource;

namespace Client.UI.Base.ColorStyle
{
    public class ToolStripColorTable
    {
        private Color _arrow = Color.Black;
        private Color _back = Color.White;
        private int _backradius = 4;
        private Rectangle _backrectangle = new Rectangle(10, 10, 10, 10);
        private Color _base = Color.FromArgb(0x69, 200, 0xfe);
        private Color _baseFore = Color.Black;
        private bool _baseForeAnamorphosis;
        private int _baseForeAnamorphosisBorder = 4;
        private Color _baseForeAnamorphosisColor = Color.White;
        private Color _baseHoverFore = Color.White;
        private bool _baseItemAnamorphosis = true;
        private Color _baseItemBorder = Color.FromArgb(60, 0x94, 0xd4);
        private bool _baseItemBorderShow = true;
        private Image _baseItemDown = GetDefaultResource.GetImage("Common.allbtn_down.png");
        private Color _baseItemHover = Color.FromArgb(60, 0x94, 0xd4);
        private Image _baseItemMouse = GetDefaultResource.GetImage("Common.allbtn_highlight.png");
        private Color _baseItemPressed = Color.FromArgb(60, 0x94, 0xd4);
        private int _baseItemRadius = 4;
        private RoundStyle _baseItemRadiusStyle = RoundStyle.All;
        private Color _baseItemSplitter = Color.FromArgb(60, 0x94, 0xd4);
        private Color _dropDownImageSeparator = Color.FromArgb(0xc5, 0xc5, 0xc5);
        private Color _fore = Color.Black;
        private Color _hoverFore = Color.White;
        private bool _itemAnamorphosis = true;
        private Color _itemborder = Color.FromArgb(60, 0x94, 0xd4);
        private bool _itemBorderShow = true;
        private Color _itemHover = Color.FromArgb(60, 0x94, 0xd4);
        private Color _itemPressed = Color.FromArgb(60, 0x94, 0xd4);
        private int _itemRadius = 4;
        private RoundStyle _itemRadiusStyle = RoundStyle.All;
        private RoundStyle _radiusstyle = RoundStyle.All;
        private bool _titleAnamorphosis = true;
        private Color _titleColor = Color.FromArgb(0xd1, 0xe4, 0xec);
        private int _titleRadius = 4;
        private RoundStyle _titleRadiusStyle = RoundStyle.All;

        public Color Arrow
        {
            get
            {
                return this._arrow;
            }
            set
            {
                this._arrow = value;
            }
        }

        public Color Back
        {
            get
            {
                return this._back;
            }
            set
            {
                this._back = value;
            }
        }

        public int BackRadius
        {
            get
            {
                return this._backradius;
            }
            set
            {
                this._backradius = value;
            }
        }

        public Rectangle BackRectangle
        {
            get
            {
                return this._backrectangle;
            }
            set
            {
                this._backrectangle = value;
            }
        }

        public Color Base
        {
            get
            {
                return this._base;
            }
            set
            {
                this._base = value;
            }
        }

        public Color BaseFore
        {
            get
            {
                return this._baseFore;
            }
            set
            {
                this._baseFore = value;
            }
        }

        public bool BaseForeAnamorphosis
        {
            get
            {
                return this._baseForeAnamorphosis;
            }
            set
            {
                this._baseForeAnamorphosis = value;
            }
        }

        public int BaseForeAnamorphosisBorder
        {
            get
            {
                return this._baseForeAnamorphosisBorder;
            }
            set
            {
                this._baseForeAnamorphosisBorder = value;
            }
        }

        public Color BaseForeAnamorphosisColor
        {
            get
            {
                return this._baseForeAnamorphosisColor;
            }
            set
            {
                this._baseForeAnamorphosisColor = value;
            }
        }

        public Color BaseHoverFore
        {
            get
            {
                return this._baseHoverFore;
            }
            set
            {
                this._baseHoverFore = value;
            }
        }

        public bool BaseItemAnamorphosis
        {
            get
            {
                return this._baseItemAnamorphosis;
            }
            set
            {
                this._baseItemAnamorphosis = value;
            }
        }

        public Color BaseItemBorder
        {
            get
            {
                return this._baseItemBorder;
            }
            set
            {
                this._baseItemBorder = value;
            }
        }

        public bool BaseItemBorderShow
        {
            get
            {
                return this._baseItemBorderShow;
            }
            set
            {
                this._baseItemBorderShow = value;
            }
        }

        public Image BaseItemDown
        {
            get
            {
                return this._baseItemDown;
            }
            set
            {
                this._baseItemDown = value;
            }
        }

        public Color BaseItemHover
        {
            get
            {
                return this._baseItemHover;
            }
            set
            {
                this._baseItemHover = value;
            }
        }

        public Image BaseItemMouse
        {
            get
            {
                return this._baseItemMouse;
            }
            set
            {
                this._baseItemMouse = value;
            }
        }

        public Color BaseItemPressed
        {
            get
            {
                return this._baseItemPressed;
            }
            set
            {
                this._baseItemPressed = value;
            }
        }

        public int BaseItemRadius
        {
            get
            {
                return this._baseItemRadius;
            }
            set
            {
                this._baseItemRadius = value;
            }
        }

        public RoundStyle BaseItemRadiusStyle
        {
            get
            {
                return this._baseItemRadiusStyle;
            }
            set
            {
                this._baseItemRadiusStyle = value;
            }
        }

        public Color BaseItemSplitter
        {
            get
            {
                return this._baseItemSplitter;
            }
            set
            {
                this._baseItemSplitter = value;
            }
        }

        public Color DropDownImageSeparator
        {
            get
            {
                return this._dropDownImageSeparator;
            }
            set
            {
                this._dropDownImageSeparator = value;
            }
        }

        public Color Fore
        {
            get
            {
                return this._fore;
            }
            set
            {
                this._fore = value;
            }
        }

        public Color HoverFore
        {
            get
            {
                return this._hoverFore;
            }
            set
            {
                this._hoverFore = value;
            }
        }

        public bool ItemAnamorphosis
        {
            get
            {
                return this._itemAnamorphosis;
            }
            set
            {
                this._itemAnamorphosis = value;
            }
        }

        public Color ItemBorder
        {
            get
            {
                return this._itemborder;
            }
            set
            {
                this._itemborder = value;
            }
        }

        public bool ItemBorderShow
        {
            get
            {
                return this._itemBorderShow;
            }
            set
            {
                this._itemBorderShow = value;
            }
        }

        public Color ItemHover
        {
            get
            {
                return this._itemHover;
            }
            set
            {
                this._itemHover = value;
            }
        }

        public Color ItemPressed
        {
            get
            {
                return this._itemPressed;
            }
            set
            {
                this._itemPressed = value;
            }
        }

        public int ItemRadius
        {
            get
            {
                return this._itemRadius;
            }
            set
            {
                this._itemRadius = value;
            }
        }

        public RoundStyle ItemRadiusStyle
        {
            get
            {
                return this._itemRadiusStyle;
            }
            set
            {
                this._itemRadiusStyle = value;
            }
        }

        public RoundStyle RadiusStyle
        {
            get
            {
                return this._radiusstyle;
            }
            set
            {
                this._radiusstyle = value;
            }
        }

        public bool TitleAnamorphosis
        {
            get
            {
                return this._titleAnamorphosis;
            }
            set
            {
                this._titleAnamorphosis = value;
            }
        }

        public Color TitleColor
        {
            get
            {
                return this._titleColor;
            }
            set
            {
                this._titleColor = value;
            }
        }

        public int TitleRadius
        {
            get
            {
                return this._titleRadius;
            }
            set
            {
                this._titleRadius = value;
            }
        }

        public RoundStyle TitleRadiusStyle
        {
            get
            {
                return this._titleRadiusStyle;
            }
            set
            {
                this._titleRadiusStyle = value;
            }
        }
    }
}
