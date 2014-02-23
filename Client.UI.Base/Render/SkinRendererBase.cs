using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using Client.UI.Base.Forms;
using System.Security.Permissions;
using Client.UI.Base.Enums;
using Client.UI.Base.Controls;

namespace Client.UI.Base.Render
{
    public abstract class SkinRendererBase
    {
        private EventHandlerList _events;
        private static readonly object EventRenderSkinFormBorder = new object();
        private static readonly object EventRenderSkinFormCaption = new object();
        private static readonly object EventRenderSkinFormControlBox = new object();

        public event SkinFormBorderRenderEventHandler RenderSkinFormBorder
        {
            add
            {
                this.AddHandler(EventRenderSkinFormBorder, value);
            }
            remove
            {
                this.RemoveHandler(EventRenderSkinFormBorder, value);
            }
        }

        public event SkinFormCaptionRenderEventHandler RenderSkinFormCaption
        {
            add
            {
                this.AddHandler(EventRenderSkinFormCaption, value);
            }
            remove
            {
                this.RemoveHandler(EventRenderSkinFormCaption, value);
            }
        }

        public event SkinFormControlBoxRenderEventHandler RenderSkinFormControlBox
        {
            add
            {
                this.AddHandler(EventRenderSkinFormControlBox, value);
            }
            remove
            {
                this.RemoveHandler(EventRenderSkinFormControlBox, value);
            }
        }



        [UIPermission(SecurityAction.Demand, Window=UIPermissionWindow.AllWindows)]
        protected void AddHandler(object key, Delegate value)
        {
            this.Events.AddHandler(key, value);
        }

        public abstract Region CreateRegion(FormBase form);
        public void DrawSkinFormBorder(SkinFormBorderRenderEventArgs e)
        {
            this.OnRenderSkinFormBorder(e);
            SkinFormBorderRenderEventHandler handler = this.Events[EventRenderSkinFormBorder] as SkinFormBorderRenderEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void DrawSkinFormCaption(SkinFormCaptionRenderEventArgs e)
        {
            this.OnRenderSkinFormCaption(e);
            SkinFormCaptionRenderEventHandler handler = this.Events[EventRenderSkinFormCaption] as SkinFormCaptionRenderEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void DrawSkinFormControlBox(SkinFormControlBoxRenderEventArgs e)
        {
            this.OnRenderSkinFormControlBox(e);
            SkinFormControlBoxRenderEventHandler handler = this.Events[EventRenderSkinFormControlBox] as SkinFormControlBoxRenderEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public abstract void InitSkinForm(FormBase form);
        protected abstract void OnRenderSkinFormBorder(SkinFormBorderRenderEventArgs e);
        protected abstract void OnRenderSkinFormCaption(SkinFormCaptionRenderEventArgs e);
        protected abstract void OnRenderSkinFormControlBox(SkinFormControlBoxRenderEventArgs e);
        [UIPermission(SecurityAction.Demand, Window=UIPermissionWindow.AllWindows)]
        protected void RemoveHandler(object key, Delegate value)
        {
            this.Events.RemoveHandler(key, value);
        }

        protected EventHandlerList Events
        {
            get
            {
                if (this._events == null)
                {
                    this._events = new EventHandlerList();
                }
                return this._events;
            }
        }
    }


    #region >自定义事件<
    public delegate void SkinFormBorderRenderEventHandler(object sender, SkinFormBorderRenderEventArgs e);

    public delegate void SkinFormCaptionRenderEventHandler(object sender, SkinFormCaptionRenderEventArgs e);

    public delegate void SkinFormControlBoxRenderEventHandler(object sender, SkinFormControlBoxRenderEventArgs e);

    #endregion

    #region >自定义事件参数<
    public class SkinFormBorderRenderEventArgs : PaintEventArgs
    {
        private bool _active;
        private FormBase _skinForm;

        public SkinFormBorderRenderEventArgs(FormBase skinForm, Graphics g, Rectangle clipRect, bool active) : base(g, clipRect)
        {
            this._skinForm = skinForm;
            this._active = active;
        }

        public bool Active
        {
            get
            {
                return this._active;
            }
        }

        public FormBase SkinForm
        {
            get
            {
                return this._skinForm;
            }
        }
    }

    public class SkinFormCaptionRenderEventArgs : PaintEventArgs
    {
        private bool _active;
        private FormBase _skinForm;

        public SkinFormCaptionRenderEventArgs(FormBase skinForm, Graphics g, Rectangle clipRect, bool active)
            : base(g, clipRect)
        {
            this._skinForm = skinForm;
            this._active = active;
        }

        public bool Active
        {
            get
            {
                return this._active;
            }
        }

        public FormBase SkinForm
        {
            get
            {
                return this._skinForm;
            }
        }
    }

    public class SkinFormControlBoxRenderEventArgs : PaintEventArgs
    {
        private bool _active;
        private CmSysButton _CmSysbutton;
        private ControlBoxState _controlBoxState;
        private ControlBoxStyle _controlBoxStyle;
        private FormBase _form;

        public SkinFormControlBoxRenderEventArgs(Forms.FormBase form, Graphics graphics, Rectangle clipRect, bool active,ControlBoxStyle controlBoxStyle, ControlBoxState controlBoxState, CmSysButton cmSysbutton = null) 
            : base(graphics, clipRect)
        {
            this._form = form;
            this._active = active;
            this._controlBoxState = controlBoxState;
            this._controlBoxStyle = controlBoxStyle;
            this._CmSysbutton = cmSysbutton;
        }

        public bool Active
        {
            get
            {
                return this._active;
            }
        }

        public CmSysButton CmSysButton
        {
            get
            {
                return this._CmSysbutton;
            }
        }

        public ControlBoxStyle ControlBoxStyle
        {
            get
            {
                return this._controlBoxStyle;
            }
        }

        public ControlBoxState ControlBoxtate
        {
            get
            {
                return this._controlBoxState;
            }
        }

        public FormBase Form
        {
            get
            {
                return this._form;
            }
        }
    }

    #endregion
}
