using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Client.UI.Base.Enums;
using System.Windows.Forms;
using Client.UI.Base.Forms;
using Client.UI.Base.Collection;
using System.Collections;

namespace Client.UI.Base.Controls
{
    public class ControlBoxManager : IDisposable
    {
        private ControlBoxState _closBoxState;
        private ControlBoxState _maximizeBoxState;
        private ControlBoxState _minimizeBoxState;
        private bool _mouseDown;
        private FormBase _owner;
        private ControlBoxState _SysBottomState;

        public ControlBoxManager(FormBase owner)
        {
            this._owner = owner;
        }

        public void Dispose()
        {
            this._owner = null;
        }

        private void HideToolTip()
        {
            if (this._owner != null)
            {
                //this._owner.ToolTip.Active = false;
            }
        }

        private void Invalidate(Rectangle rect)
        {
            this._owner.Invalidate(rect);
        }

        ////private void ProcessMouseDown(Point mousePoint, Rectangle closeBoxRect, Rectangle minimizeBoxRect, Rectangle maximizeBoxRect, Rectangle sysbottomRect, bool closeBoxVisibale, bool minimizeBoxVisibale, bool maximizeBoxVisibale, bool sysbottomVisibale)
        ////{
        ////    this._mouseDown = true;
        ////    if (closeBoxVisibale && closeBoxRect.Contains(mousePoint))
        ////    {
        ////        this.CloseBoxState = ControlBoxState.Pressed;
        ////    }
        ////    else if (minimizeBoxVisibale && minimizeBoxRect.Contains(mousePoint))
        ////    {
        ////        this.MinimizeBoxState = ControlBoxState.Pressed;
        ////    }
        ////    else if (this.SysBottomVisibale && sysbottomRect.Contains(mousePoint))
        ////    {
        ////        this.SysBottomState = ControlBoxState.Pressed;
        ////    }
        ////    else if (maximizeBoxVisibale && maximizeBoxRect.Contains(mousePoint))
        ////    {
        ////        this.MaximizeBoxState = ControlBoxState.Pressed;
        ////    }
        ////}




        //private void ProcessMouseLeave(bool closeBoxVisibale, bool minimizeBoxVisibale, bool maximizeBoxVisibale, bool sysbottomVisibale)
        //{
        //    if (closeBoxVisibale)
        //    {
        //        if (this.CloseBoxState == ControlBoxState.Pressed)
        //        {
        //            this.CloseBoxState = ControlBoxState.PressedLeave;
        //        }
        //        else
        //        {
        //            this.CloseBoxState = ControlBoxState.Normal;
        //        }
        //    }
        //    if (minimizeBoxVisibale)
        //    {
        //        if (this.MinimizeBoxState == ControlBoxState.Pressed)
        //        {
        //            this.MinimizeBoxState = ControlBoxState.PressedLeave;
        //        }
        //        else
        //        {
        //            this.MinimizeBoxState = ControlBoxState.Normal;
        //        }
        //    }
        //    if (sysbottomVisibale)
        //    {
        //        if (this.SysBottomState == ControlBoxState.Pressed)
        //        {
        //            this.SysBottomState = ControlBoxState.PressedLeave;
        //        }
        //        else
        //        {
        //            this.SysBottomState = ControlBoxState.Normal;
        //        }
        //    }
        //    if (maximizeBoxVisibale)
        //    {
        //        if (this.MaximizeBoxState == ControlBoxState.Pressed)
        //        {
        //            this.MaximizeBoxState = ControlBoxState.PressedLeave;
        //        }
        //        else
        //        {
        //            this.MaximizeBoxState = ControlBoxState.Normal;
        //        }
        //    }
        //    this.HideToolTip();
        //}

        //private void ProcessMouseMove(Point mousePoint, Rectangle closeBoxRect, Rectangle minimizeBoxRect, Rectangle maximizeBoxRect, Rectangle sysbottomRect, bool closeBoxVisibale, bool minimizeBoxVisibale, bool maximizeBoxVisibale, bool sysbottomVisibale)
        //{
        //    string toolTipText = string.Empty;
        //    bool flag = true;
        //    if (closeBoxVisibale)
        //    {
        //        if (closeBoxRect.Contains(mousePoint))
        //        {
        //            flag = false;
        //            if (!this._mouseDown)
        //            {
        //                if (this.CloseBoxState != ControlBoxState.Hover)
        //                {
        //                    toolTipText = "关闭";
        //                }
        //                this.CloseBoxState = ControlBoxState.Hover;
        //            }
        //            else if (this.CloseBoxState == ControlBoxState.PressedLeave)
        //            {
        //                this.CloseBoxState = ControlBoxState.Pressed;
        //            }
        //        }
        //        else if (!this._mouseDown)
        //        {
        //            this.CloseBoxState = ControlBoxState.Normal;
        //        }
        //        else if (this.CloseBoxState == ControlBoxState.Pressed)
        //        {
        //            this.CloseBoxState = ControlBoxState.PressedLeave;
        //        }
        //    }
        //    if (minimizeBoxVisibale)
        //    {
        //        if (minimizeBoxRect.Contains(mousePoint))
        //        {
        //            flag = false;
        //            if (!this._mouseDown)
        //            {
        //                if (this.MinimizeBoxState != ControlBoxState.Hover)
        //                {
        //                    toolTipText = "最小化";
        //                }
        //                this.MinimizeBoxState = ControlBoxState.Hover;
        //            }
        //            else if (this.MinimizeBoxState == ControlBoxState.PressedLeave)
        //            {
        //                this.MinimizeBoxState = ControlBoxState.Pressed;
        //            }
        //        }
        //        else if (!this._mouseDown)
        //        {
        //            this.MinimizeBoxState = ControlBoxState.Normal;
        //        }
        //        else if (this.MinimizeBoxState == ControlBoxState.Pressed)
        //        {
        //            this.MinimizeBoxState = ControlBoxState.PressedLeave;
        //        }
        //    }
        //    if (maximizeBoxVisibale)
        //    {
        //        if (maximizeBoxRect.Contains(mousePoint))
        //        {
        //            flag = false;
        //            if (!this._mouseDown)
        //            {
        //                if (this.MaximizeBoxState != ControlBoxState.Hover)
        //                {
        //                    toolTipText = (this._owner.WindowState == FormWindowState.Maximized) ? "还原" : "最大化";
        //                }
        //                this.MaximizeBoxState = ControlBoxState.Hover;
        //            }
        //            else if (this.MaximizeBoxState == ControlBoxState.PressedLeave)
        //            {
        //                this.MaximizeBoxState = ControlBoxState.Pressed;
        //            }
        //        }
        //        else if (!this._mouseDown)
        //        {
        //            this.MaximizeBoxState = ControlBoxState.Normal;
        //        }
        //        else if (this.MaximizeBoxState == ControlBoxState.Pressed)
        //        {
        //            this.MaximizeBoxState = ControlBoxState.PressedLeave;
        //        }
        //    }
        //    if (sysbottomVisibale)
        //    {
        //        if (sysbottomRect.Contains(mousePoint))
        //        {
        //            flag = false;
        //            if (!this._mouseDown)
        //            {
        //                if (this.SysBottomState != ControlBoxState.Hover)
        //                {
        //                    //toolTipText = this._owner.SysBottomToolTip;
        //                }
        //                this.SysBottomState = ControlBoxState.Hover;
        //            }
        //            else if (this.SysBottomState == ControlBoxState.PressedLeave)
        //            {
        //                this.SysBottomState = ControlBoxState.Pressed;
        //            }
        //        }
        //        else if (!this._mouseDown)
        //        {
        //            this.SysBottomState = ControlBoxState.Normal;
        //        }
        //        else if (this.SysBottomState == ControlBoxState.Pressed)
        //        {
        //            this.SysBottomState = ControlBoxState.PressedLeave;
        //        }
        //    }
        //    if (toolTipText != string.Empty)
        //    {
        //        this.HideToolTip();
        //        this.ShowTooTip(toolTipText);
        //    }
        //    if (flag)
        //    {
        //        this.HideToolTip();
        //    }
        //}

        //public void ProcessMouseOperate(Point mousePoint, MouseOperate operate)
        //{
        //    if (this._owner.ControlBox)
        //    {
        //        Rectangle closeBoxRect = this.CloseBoxRect;
        //        Rectangle minimizeBoxRect = this.MinimizeBoxRect;
        //        Rectangle maximizeBoxRect = this.MaximizeBoxRect;
        //        Rectangle sysBottomRect = this.SysBottomRect;
        //        bool closeBoxVisibale = this.CloseBoxVisibale;
        //        bool minimizeBoxVisibale = this.MinimizeBoxVisibale;
        //        bool maximizeBoxVisibale = this.MaximizeBoxVisibale;
        //        bool sysBottomVisibale = this.SysBottomVisibale;
        //        switch (operate)
        //        {
        //            case MouseOperate.Move:
        //                this.ProcessMouseMove(mousePoint, closeBoxRect, minimizeBoxRect, maximizeBoxRect, sysBottomRect, closeBoxVisibale, minimizeBoxVisibale, maximizeBoxVisibale, sysBottomVisibale);
        //                return;

        //            case MouseOperate.Down:
        //                this.ProcessMouseDown(mousePoint, closeBoxRect, minimizeBoxRect, maximizeBoxRect, sysBottomRect, closeBoxVisibale, minimizeBoxVisibale, maximizeBoxVisibale, sysBottomVisibale);
        //                return;

        //            case MouseOperate.Up:
        //                this.ProcessMouseUP(mousePoint, closeBoxRect, minimizeBoxRect, maximizeBoxRect, sysBottomRect, closeBoxVisibale, minimizeBoxVisibale, maximizeBoxVisibale, sysBottomVisibale);
        //                return;

        //            case MouseOperate.Leave:
        //                this.ProcessMouseLeave(closeBoxVisibale, minimizeBoxVisibale, maximizeBoxVisibale, sysBottomVisibale);
        //                return;

        //            case MouseOperate.Hover:
        //                return;
        //        }
        //    }
        //}

        //private void ProcessMouseUP(Point mousePoint, Rectangle closeBoxRect, Rectangle minimizeBoxRect, Rectangle maximizeBoxRect, Rectangle sysbottomRect, bool closeBoxVisibale, bool minimizeBoxVisibale, bool maximizeBoxVisibale, bool sysbottomVisible)
        //{
        //    this._mouseDown = false;
        //    if (closeBoxVisibale)
        //    {
        //        if (closeBoxRect.Contains(mousePoint) && (this.CloseBoxState == ControlBoxState.Pressed))
        //        {
        //            this._owner.Close();
        //            this.CloseBoxState = ControlBoxState.Normal;
        //            return;
        //        }
        //        this.CloseBoxState = ControlBoxState.Normal;
        //    }
        //    if (minimizeBoxVisibale)
        //    {
        //        if (minimizeBoxRect.Contains(mousePoint) && (this.MinimizeBoxState == ControlBoxState.Pressed))
        //        {
        //            if (this._owner.ShowInTaskbar)
        //            {
        //                this._owner.WindowState = FormWindowState.Minimized;
        //            }
        //            else
        //            {
        //                this._owner.Hide();
        //            }
        //            this.MinimizeBoxState = ControlBoxState.Normal;
        //            return;
        //        }
        //        this.MinimizeBoxState = ControlBoxState.Normal;
        //    }
        //    if (sysbottomVisible)
        //    {
        //        if (sysbottomRect.Contains(mousePoint) && (this.SysBottomState == ControlBoxState.Pressed))
        //        {
        //            //this._owner.SysbottomAv(this._owner, mousePoint);
        //            this._owner.OnSysBottomClick(this._owner, mousePoint);
        //            this.SysBottomState = ControlBoxState.Normal;
        //            return;
        //        }
        //        this.MinimizeBoxState = ControlBoxState.Normal;
        //    }
        //    if (maximizeBoxVisibale)
        //    {
        //        if (maximizeBoxRect.Contains(mousePoint) && (this.MaximizeBoxState == ControlBoxState.Pressed))
        //        {
        //            if (this._owner.WindowState == FormWindowState.Maximized)
        //            {
        //                this._owner.WindowState = FormWindowState.Normal;
        //            }
        //            else
        //            {
        //                this._owner.WindowState = FormWindowState.Maximized;
        //            }
        //            this.MaximizeBoxState = ControlBoxState.Normal;
        //        }
        //        else
        //        {
        //            this.MaximizeBoxState = ControlBoxState.Normal;
        //        }
        //    }
        //}


        public void ProcessMouseOperate(Point mousePoint, MouseOperate operate)
        {
            if (!this._owner.ControlBox)
                return;
            Rectangle closeBoxRect = this.CloseBoxRect;
            Rectangle minimizeBoxRect = this.MinimizeBoxRect;
            Rectangle maximizeBoxRect = this.MaximizeBoxRect;
            bool closeBoxVisibale = this.CloseBoxVisibale;
            bool minimizeBoxVisibale = this.MinimizeBoxVisibale;
            bool maximizeBoxVisibale = this.MaximizeBoxVisibale;
            switch (operate)
            {
                case MouseOperate.Move:
                    this.ProcessMouseMove(mousePoint, closeBoxRect, minimizeBoxRect, maximizeBoxRect, closeBoxVisibale, minimizeBoxVisibale, maximizeBoxVisibale);
                    break;
                case MouseOperate.Down:
                    this.ProcessMouseDown(mousePoint, closeBoxRect, minimizeBoxRect, maximizeBoxRect, closeBoxVisibale, minimizeBoxVisibale, maximizeBoxVisibale);
                    break;
                case MouseOperate.Up:
                    this.ProcessMouseUP(mousePoint, closeBoxRect, minimizeBoxRect, maximizeBoxRect, closeBoxVisibale, minimizeBoxVisibale, maximizeBoxVisibale);
                    break;
                case MouseOperate.Leave:
                    this.ProcessMouseLeave(closeBoxVisibale, minimizeBoxVisibale, maximizeBoxVisibale);
                    break;
            }
        }

        private void ProcessMouseMove(Point mousePoint, Rectangle closeBoxRect, Rectangle minimizeBoxRect, Rectangle maximizeBoxRect, bool closeBoxVisibale, bool minimizeBoxVisibale, bool maximizeBoxVisibale)
        {
            string toolTipText = string.Empty;
            bool flag = true;
            if (closeBoxVisibale)
            {
                if (closeBoxRect.Contains(mousePoint))
                {
                    flag = false;
                    if (!this._mouseDown)
                    {
                        if (this.CloseBoxState != ControlBoxState.Hover)
                            toolTipText = "关闭";
                        this.CloseBoxState = ControlBoxState.Hover;
                    }
                    else if (this.CloseBoxState == ControlBoxState.PressedLeave)
                        this.CloseBoxState = ControlBoxState.Pressed;
                }
                else if (!this._mouseDown)
                    this.CloseBoxState = ControlBoxState.Normal;
                else if (this.CloseBoxState == ControlBoxState.Pressed)
                    this.CloseBoxState = ControlBoxState.PressedLeave;
            }
            if (minimizeBoxVisibale)
            {
                if (minimizeBoxRect.Contains(mousePoint))
                {
                    flag = false;
                    if (!this._mouseDown)
                    {
                        if (this.MinimizeBoxState != ControlBoxState.Hover)
                            toolTipText = "最小化";
                        this.MinimizeBoxState = ControlBoxState.Hover;
                    }
                    else if (this.MinimizeBoxState == ControlBoxState.PressedLeave)
                        this.MinimizeBoxState = ControlBoxState.Pressed;
                }
                else if (!this._mouseDown)
                    this.MinimizeBoxState = ControlBoxState.Normal;
                else if (this.MinimizeBoxState == ControlBoxState.Pressed)
                    this.MinimizeBoxState = ControlBoxState.PressedLeave;
            }
            if (maximizeBoxVisibale)
            {
                if (maximizeBoxRect.Contains(mousePoint))
                {
                    flag = false;
                    if (!this._mouseDown)
                    {
                        if (this.MaximizeBoxState != ControlBoxState.Hover)
                            toolTipText = this._owner.WindowState == FormWindowState.Maximized ? "还原" : "最大化";
                        this.MaximizeBoxState = ControlBoxState.Hover;
                    }
                    else if (this.MaximizeBoxState == ControlBoxState.PressedLeave)
                        this.MaximizeBoxState = ControlBoxState.Pressed;
                }
                else if (!this._mouseDown)
                    this.MaximizeBoxState = ControlBoxState.Normal;
                else if (this.MaximizeBoxState == ControlBoxState.Pressed)
                    this.MaximizeBoxState = ControlBoxState.PressedLeave;
            }
            foreach (CmSysButton cmSysButton in (IEnumerable)this.SysButtonItems)
            {
                if (cmSysButton.Visibale)
                {
                    if (cmSysButton.Bounds.Contains(mousePoint))
                    {
                        flag = false;
                        if (!this._mouseDown)
                        {
                            if (cmSysButton.BoxState != ControlBoxState.Hover)
                                toolTipText = cmSysButton.ToolTip;
                            cmSysButton.BoxState = ControlBoxState.Hover;
                        }
                        else if (cmSysButton.BoxState == ControlBoxState.PressedLeave)
                            cmSysButton.BoxState = ControlBoxState.Pressed;
                    }
                    else if (!this._mouseDown)
                        cmSysButton.BoxState = ControlBoxState.Normal;
                    else if (cmSysButton.BoxState == ControlBoxState.Pressed)
                        cmSysButton.BoxState = ControlBoxState.PressedLeave;
                }
            }
            if (toolTipText != string.Empty)
            {
                this.HideToolTip();
                this.ShowTooTip(toolTipText);
            }
            if (!flag)
                return;
            this.HideToolTip();
        }

        private void ProcessMouseDown(Point mousePoint, Rectangle closeBoxRect, Rectangle minimizeBoxRect, Rectangle maximizeBoxRect, bool closeBoxVisibale, bool minimizeBoxVisibale, bool maximizeBoxVisibale)
        {
            this._mouseDown = true;
            if (closeBoxVisibale && closeBoxRect.Contains(mousePoint))
                this.CloseBoxState = ControlBoxState.Pressed;
            else if (minimizeBoxVisibale && minimizeBoxRect.Contains(mousePoint))
            {
                this.MinimizeBoxState = ControlBoxState.Pressed;
            }
            else
            {
                foreach (CmSysButton cmSysButton in (IEnumerable)this.SysButtonItems)
                {
                    if (cmSysButton.Visibale && cmSysButton.Bounds.Contains(mousePoint))
                    {
                        cmSysButton.BoxState = ControlBoxState.Pressed;
                        return;
                    }
                }
                if (!maximizeBoxVisibale || !maximizeBoxRect.Contains(mousePoint))
                    return;
                this.MaximizeBoxState = ControlBoxState.Pressed;
            }
        }

        private void ProcessMouseUP(Point mousePoint, Rectangle closeBoxRect, Rectangle minimizeBoxRect, Rectangle maximizeBoxRect, bool closeBoxVisibale, bool minimizeBoxVisibale, bool maximizeBoxVisibale)
        {
            this._mouseDown = false;
            if (closeBoxVisibale)
            {
                if (closeBoxRect.Contains(mousePoint) && this.CloseBoxState == ControlBoxState.Pressed)
                {
                    this._owner.Close();
                    this.CloseBoxState = ControlBoxState.Normal;
                    return;
                }
                else
                    this.CloseBoxState = ControlBoxState.Normal;
            }
            if (minimizeBoxVisibale)
            {
                if (minimizeBoxRect.Contains(mousePoint) && this.MinimizeBoxState == ControlBoxState.Pressed)
                {
                    if (this._owner.ShowInTaskbar)
                        this._owner.WindowState = FormWindowState.Minimized;
                    else
                        this._owner.Hide();
                    this.MinimizeBoxState = ControlBoxState.Normal;
                    return;
                }
                else
                    this.MinimizeBoxState = ControlBoxState.Normal;
            }
            foreach (CmSysButton sysButton in (IEnumerable)this.SysButtonItems)
            {
                if (sysButton.Visibale)
                {
                    if (!sysButton.Bounds.Contains(mousePoint) || sysButton.BoxState != ControlBoxState.Pressed)
                    {
                        sysButton.BoxState = ControlBoxState.Normal;
                    }
                    else
                    {
                        //this._owner.SysbottomAv((object)this._owner, new SysButtonEventArgs(sysButton));
                        this._owner.OnSysBottomClick(this._owner, new SysButtonEventArgs(sysButton));
                        sysButton.BoxState = ControlBoxState.Normal;
                        return;
                    }
                }
            }
            if (!maximizeBoxVisibale)
                return;
            if (maximizeBoxRect.Contains(mousePoint) && this.MaximizeBoxState == ControlBoxState.Pressed)
            {
                if (this._owner.WindowState == FormWindowState.Maximized)
                    this._owner.WindowState = FormWindowState.Normal;
                else
                    this._owner.WindowState = FormWindowState.Maximized;
                this.MaximizeBoxState = ControlBoxState.Normal;
            }
            else
                this.MaximizeBoxState = ControlBoxState.Normal;
        }

        private void ProcessMouseLeave(bool closeBoxVisibale, bool minimizeBoxVisibale, bool maximizeBoxVisibale)
        {
            if (closeBoxVisibale)
                this.CloseBoxState = this.CloseBoxState != ControlBoxState.Pressed ? ControlBoxState.Normal : ControlBoxState.PressedLeave;
            if (minimizeBoxVisibale)
                this.MinimizeBoxState = this.MinimizeBoxState != ControlBoxState.Pressed ? ControlBoxState.Normal : ControlBoxState.PressedLeave;
            foreach (CmSysButton cmSysButton in (IEnumerable)this.SysButtonItems)
            {
                if (cmSysButton.Visibale)
                    cmSysButton.BoxState = cmSysButton.BoxState != ControlBoxState.Pressed ? ControlBoxState.Normal : ControlBoxState.PressedLeave;
            }
            if (maximizeBoxVisibale)
                this.MaximizeBoxState = this.MaximizeBoxState != ControlBoxState.Pressed ? ControlBoxState.Normal : ControlBoxState.PressedLeave;
            this.HideToolTip();
        }

        


        private void ShowTooTip(string toolTipText)
        {
            if (this._owner != null)
            {
                //this._owner.ToolTip.Active = true;
                //this._owner.ToolTip.SetToolTip(this._owner, toolTipText);
            }
        }

        public Rectangle CloseBoxRect
        {
            get
            {
                if (this.CloseBoxVisibale)
                {
                    Point controlBoxOffset = this.ControlBoxOffset;
                    Size closeBoxSize = this._owner.CloseBoxSize;
                    return new Rectangle((this._owner.Width - controlBoxOffset.X) - closeBoxSize.Width, controlBoxOffset.Y, closeBoxSize.Width, closeBoxSize.Height);
                }
                return Rectangle.Empty;
            }
        }

        public ControlBoxState CloseBoxState
        {
            get
            {
                return this._closBoxState;
            }
            protected set
            {
                if (this._closBoxState != value)
                {
                    this._closBoxState = value;
                    if (this._owner != null)
                    {
                        this.Invalidate(this.CloseBoxRect);
                    }
                }
            }
        }

        public bool CloseBoxVisibale
        {
            get
            {
                return this._owner.ControlBox;
            }
        }

        public Point ControlBoxOffset
        {
            get
            {
                return this._owner.ControlBoxOffset;
            }
        }

        public int ControlBoxSpace
        {
            get
            {
                return this._owner.ControlBoxSpace;
            }
        }

        public Rectangle MaximizeBoxRect
        {
            get
            {
                if (this.MaximizeBoxVisibale)
                {
                    Point controlBoxOffset = this.ControlBoxOffset;
                    Size maxSize = this._owner.MaxSize;
                    return new Rectangle((this.CloseBoxRect.X - this.ControlBoxSpace) - maxSize.Width, controlBoxOffset.Y, maxSize.Width, maxSize.Height);
                }
                return Rectangle.Empty;
            }
        }

        public ControlBoxState MaximizeBoxState
        {
            get
            {
                return this._maximizeBoxState;
            }
            protected set
            {
                if (this._maximizeBoxState != value)
                {
                    this._maximizeBoxState = value;
                    if (this._owner != null)
                    {
                        this.Invalidate(this.MaximizeBoxRect);
                    }
                }
            }
        }

        public bool MaximizeBoxVisibale
        {
            get
            {
                return (this._owner.ControlBox && this._owner.MaximizeBox);
            }
        }

        public Rectangle MinimizeBoxRect
        {
            get
            {
                if (this.MinimizeBoxVisibale)
                {
                    Point controlBoxOffset = this.ControlBoxOffset;
                    Size miniSize = this._owner.MiniSize;
                    return new Rectangle(this.MaximizeBoxVisibale ? ((this.MaximizeBoxRect.X - this.ControlBoxSpace) - miniSize.Width) : ((this.CloseBoxRect.X - this.ControlBoxSpace) - miniSize.Width), controlBoxOffset.Y, miniSize.Width, miniSize.Height);
                }
                return Rectangle.Empty;
            }
        }

        public ControlBoxState MinimizeBoxState
        {
            get
            {
                return this._minimizeBoxState;
            }
            protected set
            {
                if (this._minimizeBoxState != value)
                {
                    this._minimizeBoxState = value;
                    if (this._owner != null)
                    {
                        this.Invalidate(this.MinimizeBoxRect);
                    }
                }
            }
        }

        public bool MinimizeBoxVisibale
        {
            get
            {
                return (this._owner.ControlBox && this._owner.MinimizeBox);
            }
        }

        public CustomSysButtonCollection SysButtonItems
        {
            get
            {

                CmSysButton button = null;
                foreach (CmSysButton button2 in _owner.SysButtonItems)
                {
                    Size size = button2.Size;
                    int x = this.MinimizeBoxVisibale ? ((this.MinimizeBoxRect.X - this.ControlBoxSpace) - size.Width) : (this.MaximizeBoxVisibale ? ((this.MaximizeBoxRect.X - this.ControlBoxSpace) - size.Width) : ((this.CloseBoxRect.X - this.ControlBoxSpace) - size.Width));
                    if ((button != null) && button.Visibale)
                    {
                        x = (button.Bounds.X - this.ControlBoxSpace) - size.Width;
                    }
                    Rectangle rectangle2 = new Rectangle(new Point(x, this._owner.ControlBoxOffset.Y), button2.Bounds.Size);
                    button2.Bounds = rectangle2;
                    button = button2;
                }
                return this._owner.SysButtonItems;
            }
        }

        //public ControlBoxState SysBottomState
        //{
        //    get
        //    {
        //        return this._SysBottomState;
        //    }
        //    protected set
        //    {
        //        if (this._SysBottomState != value)
        //        {
        //            this._SysBottomState = value;
        //            if (this._owner != null)
        //            {
        //                this.Invalidate(this.SysBottomRect);
        //            }
        //        }
        //    }
        //}

        //public bool SysBottomVisibale
        //{
        //    get
        //    {
        //        return this._owner.SysBottomVisibale;
        //    }
        //}
    }
}
