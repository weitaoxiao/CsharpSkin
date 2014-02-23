using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Client.UI.Base.Controls
{
    public class ChatListVScroll
    {
        private Rectangle bounds;
        private Rectangle upBounds;
        private Rectangle downBounds;
        private Rectangle sliderBounds;
        private Color backColor;
        private Color sliderDefaultColor;
        private Color sliderDownColor;
        private Color arrowBackColor;
        private Color arrowColor;
        private Control ctrl;
        private int virtualHeight;
        private int value;
        private bool shouldBeDraw;
        private bool isMouseDown;
        private bool isMouseOnSlider;
        private bool isMouseOnUp;
        private bool isMouseOnDown;
        private int mouseDownY;
        private int m_nLastSliderY;

        public Rectangle Bounds
        {
            get
            {
                return this.bounds;
            }
        }

        public Rectangle UpBounds
        {
            get
            {
                return this.upBounds;
            }
        }

        public Rectangle DownBounds
        {
            get
            {
                return this.downBounds;
            }
        }

        public Rectangle SliderBounds
        {
            get
            {
                return this.sliderBounds;
            }
        }

        public Color BackColor
        {
            get
            {
                return this.backColor;
            }
            set
            {
                this.backColor = value;
            }
        }

        public Color SliderDefaultColor
        {
            get
            {
                return this.sliderDefaultColor;
            }
            set
            {
                if (this.sliderDefaultColor == value)
                    return;
                this.sliderDefaultColor = value;
                this.ctrl.Invalidate(this.sliderBounds);
            }
        }

        public Color SliderDownColor
        {
            get
            {
                return this.sliderDownColor;
            }
            set
            {
                if (this.sliderDownColor == value)
                    return;
                this.sliderDownColor = value;
                this.ctrl.Invalidate(this.sliderBounds);
            }
        }

        public Color ArrowBackColor
        {
            get
            {
                return this.arrowBackColor;
            }
            set
            {
                if (this.arrowBackColor == value)
                    return;
                this.arrowBackColor = value;
                this.ctrl.Invalidate(this.bounds);
            }
        }

        public Color ArrowColor
        {
            get
            {
                return this.arrowColor;
            }
            set
            {
                if (this.arrowColor == value)
                    return;
                this.arrowColor = value;
                this.ctrl.Invalidate(this.bounds);
            }
        }

        public Control Ctrl
        {
            get
            {
                return this.ctrl;
            }
            set
            {
                this.ctrl = value;
            }
        }

        public int VirtualHeight
        {
            get
            {
                return this.virtualHeight;
            }
            set
            {
                if (value <= this.ctrl.Height)
                {
                    if (!this.shouldBeDraw)
                        return;
                    this.shouldBeDraw = false;
                    if (this.value != 0)
                    {
                        this.value = 0;
                        this.ctrl.Invalidate();
                    }
                }
                else
                {
                    this.shouldBeDraw = true;
                    if (value - this.value < this.ctrl.Height)
                    {
                        this.value -= this.ctrl.Height - value + this.value;
                        this.ctrl.Invalidate();
                    }
                }
                this.virtualHeight = value;
            }
        }

        public int Value
        {
            get
            {
                return this.value;
            }
            set
            {
                if (!this.shouldBeDraw)
                    return;
                if (value < 0)
                {
                    if (this.value == 0)
                        return;
                    this.value = 0;
                    this.ctrl.Invalidate();
                }
                else if (value > this.virtualHeight - this.ctrl.Height)
                {
                    if (this.value == this.virtualHeight - this.ctrl.Height)
                        return;
                    this.value = this.virtualHeight - this.ctrl.Height;
                    this.ctrl.Invalidate();
                }
                else
                {
                    this.value = value;
                    this.ctrl.Invalidate();
                }
            }
        }

        public bool ShouldBeDraw
        {
            get
            {
                return this.shouldBeDraw;
            }
        }

        public bool IsMouseDown
        {
            get
            {
                return this.isMouseDown;
            }
            set
            {
                if (value)
                    this.m_nLastSliderY = this.sliderBounds.Y;
                this.isMouseDown = value;
            }
        }

        public bool IsMouseOnSlider
        {
            get
            {
                return this.isMouseOnSlider;
            }
            set
            {
                if (this.isMouseOnSlider == value)
                    return;
                this.isMouseOnSlider = value;
                this.ctrl.Invalidate(this.SliderBounds);
            }
        }

        public bool IsMouseOnUp
        {
            get
            {
                return this.isMouseOnUp;
            }
            set
            {
                if (this.isMouseOnUp == value)
                    return;
                this.isMouseOnUp = value;
                this.ctrl.Invalidate(this.UpBounds);
            }
        }

        public bool IsMouseOnDown
        {
            get
            {
                return this.isMouseOnDown;
            }
            set
            {
                if (this.isMouseOnDown == value)
                    return;
                this.isMouseOnDown = value;
                this.ctrl.Invalidate(this.DownBounds);
            }
        }

        public int MouseDownY
        {
            get
            {
                return this.mouseDownY;
            }
            set
            {
                this.mouseDownY = value;
            }
        }

        public ChatListVScroll(Control c)
        {
            this.ctrl = c;
            this.virtualHeight = 400;
            this.bounds = new Rectangle(0, 0, 5, 5);
            this.upBounds = new Rectangle(0, 0, 5, 5);
            this.downBounds = new Rectangle(0, 0, 5, 5);
            this.sliderBounds = new Rectangle(0, 0, 5, 5);
            this.backColor = Color.FromArgb(50, 224, 239, 235);
            this.sliderDefaultColor = Color.FromArgb(100, 110, 111, 112);
            this.sliderDownColor = Color.FromArgb(200, 110, 111, 112);
            this.arrowBackColor = Color.Transparent;
            this.arrowColor = Color.FromArgb(200, 148, 150, 151);
        }

        public void ClearAllMouseOn()
        {
            if (!this.isMouseOnDown && !this.isMouseOnSlider && !this.isMouseOnUp)
                return;
            ChatListVScroll chatListVscroll1 = this;
            ChatListVScroll chatListVscroll2 = this;
            this.isMouseOnUp = false;
            int num1 = 0;
            chatListVscroll2.isMouseOnDown = num1 != 0;
            int num2 = 0;
            chatListVscroll1.isMouseOnSlider = num2 != 0;
            this.ctrl.Invalidate(this.bounds);
        }

        public void MoveSliderToLocation(int nCurrentMouseY)
        {
            this.sliderBounds.Y = nCurrentMouseY - this.sliderBounds.Height / 2 >= 11 ? (nCurrentMouseY + this.sliderBounds.Height / 2 <= this.ctrl.Height - 11 ? nCurrentMouseY - this.sliderBounds.Height / 2 : this.ctrl.Height - this.sliderBounds.Height - 11) : 11;
            this.value = (int)((double)(this.sliderBounds.Y - 11) / (double)(this.ctrl.Height - 22 - this.SliderBounds.Height) * (double)(this.virtualHeight - this.ctrl.Height));
            this.ctrl.Invalidate();
        }

        public void MoveSliderFromLocation(int nCurrentMouseY)
        {
            if (this.m_nLastSliderY + nCurrentMouseY - this.mouseDownY < 11)
            {
                if (this.sliderBounds.Y == 11)
                    return;
                this.sliderBounds.Y = 11;
            }
            else if (this.m_nLastSliderY + nCurrentMouseY - this.mouseDownY > this.ctrl.Height - 11 - this.SliderBounds.Height)
            {
                if (this.sliderBounds.Y == this.ctrl.Height - 11 - this.sliderBounds.Height)
                    return;
                this.sliderBounds.Y = this.ctrl.Height - 11 - this.sliderBounds.Height;
            }
            else
                this.sliderBounds.Y = this.m_nLastSliderY + nCurrentMouseY - this.mouseDownY;
            this.value = (int)((double)(this.sliderBounds.Y - 11) / (double)(this.ctrl.Height - 22 - this.SliderBounds.Height) * (double)(this.virtualHeight - this.ctrl.Height));
            this.ctrl.Invalidate();
        }

        public void ReDrawScroll(Graphics g)
        {
            if (!this.shouldBeDraw)
                return;
            this.bounds.X = this.ctrl.Width - 7;
            this.bounds.Height = this.ctrl.Height;
            this.upBounds.X = this.downBounds.X = this.bounds.X;
            this.downBounds.Y = this.ctrl.Height - 5;
            this.sliderBounds.X = this.bounds.X;
            this.sliderBounds.Height = (int)((double)this.ctrl.Height / (double)this.virtualHeight * (double)(this.ctrl.Height - 22));
            if (this.sliderBounds.Height < 3)
                this.sliderBounds.Height = 3;
            this.sliderBounds.Y = 11 + (int)((double)this.value / (double)(this.virtualHeight - this.ctrl.Height) * (double)(this.ctrl.Height - 22 - this.sliderBounds.Height));
            SolidBrush solidBrush = new SolidBrush(this.backColor);
            try
            {
                g.FillRectangle((Brush)solidBrush, this.bounds);
                solidBrush.Color = this.arrowBackColor;
                g.FillRectangle((Brush)solidBrush, this.upBounds);
                g.FillRectangle((Brush)solidBrush, this.downBounds);
                solidBrush.Color = this.isMouseDown || this.isMouseOnSlider ? this.sliderDownColor : this.sliderDefaultColor;
                g.FillRectangle((Brush)solidBrush, this.sliderBounds);
                solidBrush.Color = this.arrowColor;
            }
            finally
            {
                solidBrush.Dispose();
            }
        }
    }
}
