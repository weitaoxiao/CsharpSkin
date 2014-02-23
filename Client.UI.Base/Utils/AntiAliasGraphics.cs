using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Client.UI.Base.Utils
{
    public class AntiAliasGraphics : IDisposable
    {
        private Graphics _graphics;
        private SmoothingMode _oldMode;

        public AntiAliasGraphics(Graphics graphics)
        {
            this._graphics = graphics;
            this._oldMode = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
        }

        public void Dispose()
        {
            this._graphics.SmoothingMode = this._oldMode;
        }
    }
}
