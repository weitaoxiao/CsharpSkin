using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Client.UI.Base.Render
{
    public class FormSkinColorTable
    {
        public static readonly Color _back = Color.FromArgb(0x80, 0xd0, 0xff);
        public static readonly Color _border = Color.FromArgb(100, 0, 0, 0);
        public static readonly Color _captionActive = Color.Transparent;
        public static readonly Color _captionDeactive = Color.Transparent;
        public static readonly Color _captionText = Color.Black;
        public static Color _controlBoxActive = Color.FromArgb(0x33, 0x99, 0xcc);
        public static Color _controlBoxDeactive = Color.FromArgb(0x58, 0xac, 0xda);
        public static readonly Color _controlBoxHover = Color.FromArgb(150, 0x27, 0xaf, 0xe7);
        public static readonly Color _controlBoxInnerBorder = Color.FromArgb(0x80, 250, 250, 250);
        public static readonly Color _controlBoxPressed = Color.FromArgb(150, 0x1d, 0x8e, 190);
        private static readonly Color _controlCloseBoxHover = Color.FromArgb(0xd5, 0x42, 0x16);
        private static readonly Color _controlCloseBoxPressed = Color.FromArgb(0xab, 0x35, 0x11);
        public static readonly Color _innerBorder = Color.FromArgb(100, 250, 250, 250);

        public virtual Color Back
        {
            get
            {
                return _back;
            }
        }

        public virtual Color Border
        {
            get
            {
                return _border;
            }
        }

        public virtual Color CaptionActive
        {
            get
            {
                return _captionActive;
            }
        }

        public virtual Color CaptionDeactive
        {
            get
            {
                return _captionDeactive;
            }
        }

        public virtual Color CaptionText
        {
            get
            {
                return _captionText;
            }
        }

        public virtual Color ControlBoxActive
        {
            get
            {
                return _controlBoxActive;
            }
            set
            {
                _controlBoxActive = value;
            }
        }

        public virtual Color ControlBoxDeactive
        {
            get
            {
                return _controlBoxDeactive;
            }
            set
            {
                _controlBoxDeactive = value;
            }
        }

        public virtual Color ControlBoxHover
        {
            get
            {
                return _controlBoxHover;
            }
        }

        public virtual Color ControlBoxInnerBorder
        {
            get
            {
                return _controlBoxInnerBorder;
            }
        }

        public virtual Color ControlBoxPressed
        {
            get
            {
                return _controlBoxPressed;
            }
        }

        public virtual Color ControlCloseBoxHover
        {
            get
            {
                return _controlCloseBoxHover;
            }
        }

        public virtual Color ControlCloseBoxPressed
        {
            get
            {
                return _controlCloseBoxPressed;
            }
        }

        public virtual Color InnerBorder
        {
            get
            {
                return _innerBorder;
            }
        }
    }
}
