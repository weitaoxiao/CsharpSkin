using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Client.UI.DefaultResource;

namespace Client.UI.Base.Controls
{
    public class ToolButton : Control
    {
        private Image btnImage;
        private bool isSelectedBtn;
        private bool isSingleSelectedBtn;
        private bool isSelected;
        private bool m_bMouseEnter;

        public Image BtnImage
        {
            get
            {
                return this.btnImage;
            }
            set
            {
                this.btnImage = value;
                this.Invalidate();
            }
        }

        public bool IsSelectedBtn
        {
            get
            {
                return this.isSelectedBtn;
            }
            set
            {
                this.isSelectedBtn = value;
                if (this.isSelectedBtn)
                    return;
                this.isSingleSelectedBtn = false;
            }
        }

        public bool IsSingleSelectedBtn
        {
            get
            {
                return this.isSingleSelectedBtn;
            }
            set
            {
                this.isSingleSelectedBtn = value;
                if (!this.isSingleSelectedBtn)
                    return;
                this.isSelectedBtn = true;
            }
        }

        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                if (value == this.isSelected)
                    return;
                this.isSelected = value;
                this.Invalidate();
            }
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                this.Width = TextRenderer.MeasureText(this.Text, this.Font).Width + 21;
            }
        }

        public ToolButton()
        {
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            this.m_bMouseEnter = true;
            this.Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this.m_bMouseEnter = false;
            this.Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnClick(EventArgs e)
        {
            if (this.isSelectedBtn)
            {
                if (this.isSelected)
                {
                    if (!this.isSingleSelectedBtn)
                    {
                        this.isSelected = false;
                        this.Invalidate();
                    }
                }
                else
                {
                    this.isSelected = true;
                    this.Invalidate();
                    int index = 0;
                    for (int count = this.Parent.Controls.Count; index < count; ++index)
                    {
                        if (this.Parent.Controls[index] is ToolButton && this.Parent.Controls[index] != this && ((ToolButton)this.Parent.Controls[index]).isSelected)
                            ((ToolButton)this.Parent.Controls[index]).IsSelected = false;
                    }
                }
            }
            this.Focus();
            base.OnClick(e);
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            this.OnClick(e);
            base.OnDoubleClick(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            if (this.m_bMouseEnter)
            {
                graphics.FillRectangle(Brushes.LightBlue, this.ClientRectangle);
                graphics.DrawRectangle(Pens.DarkCyan, new Rectangle(0, 0, this.Width - 1, this.Height - 1));
            }
            if (this.btnImage == null)
                graphics.DrawImage(GetDefaultResource.GetImage("Common.none.png"), new Rectangle(2, 2, 17, 17));
            else
                graphics.DrawImage(this.btnImage, new Rectangle(2, 2, 17, 17));
            graphics.DrawString(this.Text, this.Font, Brushes.Black, 21f, (float)((this.Height - this.Font.Height) / 2));
            if (this.isSelected)
                graphics.DrawRectangle(Pens.DarkCyan, new Rectangle(0, 0, this.Width - 1, this.Height - 1));
            base.OnPaint(e);
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, TextRenderer.MeasureText(this.Text, this.Font).Width + 21, 21, specified);
        }
    }
}
