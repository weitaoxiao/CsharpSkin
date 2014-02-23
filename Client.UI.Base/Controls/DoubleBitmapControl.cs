using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Client.UI.Base.Animations;
using System.Windows.Forms;
using System.ComponentModel;

namespace Client.UI.Base.Controls
{
    internal class DoubleBitmapControl : Control, IFakeControl
    {
        private object bgBmp;
        private Container components;
        private object frame;

        public event EventHandler<PaintEventArgs> FramePainted;

        public event EventHandler<PaintEventArgs> FramePainting;

        public event EventHandler<TransfromNeededEventArg> TransfromNeeded;

        public DoubleBitmapControl()
        {
            this.InitializeComponent();
            base.Visible = false;
            base.SetStyle(ControlStyles.Selectable, false);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
        }

        public void InitParent(Control control, Padding padding)
        {
            base.Parent = control.Parent;
            int childIndex = control.Parent.Controls.GetChildIndex(control);
            control.Parent.Controls.SetChildIndex(this, childIndex);
            base.Bounds = new Rectangle(control.Left - padding.Left, control.Top - padding.Top, (control.Size.Width + padding.Left) + padding.Right, (control.Size.Height + padding.Top) + padding.Bottom);
        }

        protected virtual void OnFramePainted(PaintEventArgs e)
        {
            if (this.FramePainted != null)
            {
                this.FramePainted(this, e);
            }
        }

        protected virtual void OnFramePainting(PaintEventArgs e)
        {
            if (this.FramePainting != null)
            {
                this.FramePainting(this, e);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            this.OnFramePainting(e);
            try
            {
                graphics.DrawImage((Image)this.bgBmp, 0, 0);
                if (this.frame != null)
                {
                    TransfromNeededEventArg ea = new TransfromNeededEventArg
                    {
                        ClientRectangle = new Rectangle(0, 0, base.Width, base.Height)
                    };
                    this.OnTransfromNeeded(ea);
                    graphics.SetClip(ea.ClipRectangle);
                    graphics.Transform = ea.Matrix;
                    graphics.DrawImage((Image)this.frame, 0, 0);
                }
            }
            catch
            {
            }
            this.OnFramePainted(e);
        }

        private void OnTransfromNeeded(TransfromNeededEventArg ea)
        {
            if (this.TransfromNeeded != null)
            {
                this.TransfromNeeded(this, ea);
            }
        }

        Bitmap IFakeControl.BgBmp
        {
            get
            {
                return (Bitmap)this.bgBmp;
            }
            set
            {
                this.bgBmp = value;
            }
        }

        Bitmap IFakeControl.Frame
        {
            get
            {
                return (Bitmap)this.frame;
            }
            set
            {
                this.frame = value;
            }
        }
    }
}
