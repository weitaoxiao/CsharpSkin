using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace Client.UI.Base.Utils
{
    public class InterpolationModeGraphics : IDisposable
    {
        private InterpolationMode _oldMode;
        private Graphics _graphics;

        public InterpolationModeGraphics(Graphics graphics)
            : this(graphics, InterpolationMode.HighQualityBicubic)
        {
        }

        public InterpolationModeGraphics(Graphics graphics, InterpolationMode newMode)
        {
            this._graphics = graphics;
            this._oldMode = graphics.InterpolationMode;
            graphics.InterpolationMode = newMode;
        }

        public void Dispose()
        {
            this._graphics.InterpolationMode = this._oldMode;
        }
    }
}
