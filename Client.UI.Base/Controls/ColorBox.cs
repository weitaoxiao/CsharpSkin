using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Client.UI.DefaultResource;

namespace Client.UI.Base.Controls
{
    public class ColorBox : Control
    {
        private Bitmap m_clrImage = new Bitmap(GetDefaultResource.GetImage("Common.color.png"));
        private Color selectedColor;
        private Point m_ptCurrent;
        private Rectangle m_rectSelected;
        private Color m_lastColor;

        public Color SelectedColor
        {
            get
            {
                return this.selectedColor;
            }
        }

        public event ColorBox.ColorChangedHandler ColorChanged;

        public ColorBox()
        {
            this.InitializeComponent();
            this.selectedColor = Color.Red;
            this.m_rectSelected = new Rectangle(-100, -100, 14, 14);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }


        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Name = "ColorBox";
            this.Size = new Size(203, 50);
            this.ResumeLayout(false);
        }

        protected virtual void OnColorChanged(ColorChangedEventArgs e)
        {
            if (this.ColorChanged == null)
                return;
            this.ColorChanged((object)this, e);
        }

        protected override void OnClick(EventArgs e)
        {
            Color pixel = this.m_clrImage.GetPixel(this.m_ptCurrent.X, this.m_ptCurrent.Y);
            if (pixel.ToArgb() != Color.FromArgb((int)byte.MaxValue, 254, 254, 254).ToArgb() && pixel.ToArgb() != Color.FromArgb((int)byte.MaxValue, 133, 141, 151).ToArgb() && pixel.ToArgb() != Color.FromArgb((int)byte.MaxValue, 110, 126, 149).ToArgb())
            {
                if (this.selectedColor != pixel)
                    this.selectedColor = pixel;
                this.Invalidate();
                this.OnColorChanged(new ColorChangedEventArgs(pixel));
            }
            base.OnClick(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            this.m_ptCurrent = e.Location;
            try
            {
                if (!this.ClientRectangle.Contains(this.m_ptCurrent))
                    return;
                Color pixel = this.m_clrImage.GetPixel(this.m_ptCurrent.X, this.m_ptCurrent.Y);
                if (pixel != this.m_lastColor)
                {
                    if (pixel.ToArgb() != Color.FromArgb((int)byte.MaxValue, 254, 254, 254).ToArgb() && pixel.ToArgb() != Color.FromArgb((int)byte.MaxValue, 133, 141, 151).ToArgb() && (pixel.ToArgb() != Color.FromArgb((int)byte.MaxValue, 110, 126, 149).ToArgb() && e.X > 39))
                    {
                        this.m_rectSelected.Y = e.Y > 17 ? 17 : 2;
                        this.m_rectSelected.X = (e.X - 39) / 15 * 15 + 38;
                        this.Invalidate();
                    }
                    else
                    {
                        //// ISSUE: explicit reference operation
                        //// ISSUE: variable of a reference type
                        //Rectangle& local = @this.m_rectSelected;
                        //this.m_rectSelected.Y = -100;
                        //int num = -100;
                        //// ISSUE: explicit reference operation
                        //(^local).X = num;
                        this.m_rectSelected.X = this.m_rectSelected.Y = -100;
                        base.Invalidate();
                        this.Invalidate();
                    }
                }
                this.m_lastColor = pixel;
            }
            finally
            {
                base.OnMouseMove(e);
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            //// ISSUE: explicit reference operation
            //// ISSUE: variable of a reference type
            //Rectangle& local = @this.m_rectSelected;
            //this.m_rectSelected.Y = -100;
            //int num = -100;
            //// ISSUE: explicit reference operation
            //(^local).X = num;
            this.m_rectSelected.X = this.m_rectSelected.Y = -100;
            this.Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.DrawImage(GetDefaultResource.GetImage("Common.color.png"), new Rectangle(0, 0, 165, 35));
            graphics.DrawRectangle(Pens.SteelBlue, 0, 0, 164, 34);
            SolidBrush solidBrush = new SolidBrush(this.selectedColor);
            graphics.FillRectangle((Brush)solidBrush, 9, 5, 24, 24);
            graphics.DrawRectangle(Pens.DarkCyan, this.m_rectSelected);
            base.OnPaint(e);
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, 165, 35, specified);
        }

        public delegate void ColorChangedHandler(object sender, ColorChangedEventArgs e);
    }

    public class ColorChangedEventArgs : EventArgs
    {
        private Color color;

        public Color Color
        {
            get
            {
                return this.color;
            }
        }

        public ColorChangedEventArgs(Color clr)
        {
            this.color = clr;
        }
    }
}
