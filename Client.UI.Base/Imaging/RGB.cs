using System;
using System.Collections.Generic;
using System.Text;

namespace Client.UI.Base.Imaging
{
    public class RGB
    {
        private byte _b;
        private byte _g;
        private byte _r;
        public const short BIndex = 0;
        public const short GIndex = 1;
        public const short RIndex = 2;

        public RGB()
        {
        }

        public RGB(System.Drawing.Color color)
        {
            this._r = color.R;
            this._g = color.G;
            this._b = color.B;
        }

        public RGB(byte r, byte g, byte b)
        {
            this._r = r;
            this._g = g;
            this._b = b;
        }

        public override string ToString()
        {
            return string.Format("RGB [R={0}, G={1}, B={2}]", this._r, this._g, this._b);
        }

        public byte B
        {
            get
            {
                return this._b;
            }
            set
            {
                this._b = value;
            }
        }

        public System.Drawing.Color Color
        {
            get
            {
                return System.Drawing.Color.FromArgb(this._r, this._g, this._b);
            }
            set
            {
                this._r = value.R;
                this._g = value.G;
                this._b = value.B;
            }
        }

        public byte G
        {
            get
            {
                return this._g;
            }
            set
            {
                this._g = value;
            }
        }

        public byte R
        {
            get
            {
                return this._r;
            }
            set
            {
                this._r = value;
            }
        }
    }
}
