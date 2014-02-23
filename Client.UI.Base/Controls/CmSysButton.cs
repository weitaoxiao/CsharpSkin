using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using Client.UI.Base.Enums;

namespace Client.UI.Base.Controls
{
    public class CmSysButton
    {
        private Rectangle bounds;
        private ControlBoxState boxState;
        private Point location;
        private string name;
        private Forms.FormBase ownerForm;
        private System.Drawing.Size size;
        private Image sysButtonDown;
        private Image sysButtonMouse;
        private Image sysButtonNorml;
        private string toolTip;
        private bool visibale;

        public CmSysButton()
        {
            this.location = new Point(0, 0);
            this.size = new System.Drawing.Size(0x1c, 20);
            this.visibale = true;
        }

        public CmSysButton Clone()
        {
            return new CmSysButton { Bounds = this.Bounds, Location = this.Location, size = this.Size, ToolTip = this.ToolTip, SysButtonNorml = this.SysButtonNorml, SysButtonMouse = this.SysButtonMouse, SysButtonDown = this.SysButtonDown, OwnerForm = this.OwnerForm, Name = this.Name };
        }

        [Browsable(false)]
        public Rectangle Bounds
        {
            get
            {
                if (this.bounds == Rectangle.Empty)
                {
                    this.bounds = new Rectangle();
                }
                this.bounds.Location = this.Location;
                this.bounds.Size = this.Size;
                return this.bounds;
            }
            set
            {
                this.bounds = value;
                this.Location = this.bounds.Location;
                this.Size = this.bounds.Size;
            }
        }

        [Browsable(false)]
        public ControlBoxState BoxState
        {
            get
            {
                return this.boxState;
            }
            set
            {
                if (this.boxState != value)
                {
                    this.boxState = value;
                    if (this.OwnerForm != null)
                    {
                        this.OwnerForm.Invalidate(this.Bounds);
                    }
                }
            }
        }

        [Browsable(false), Category("按钮的位置")]
        public Point Location
        {
            get
            {
                return this.location;
            }
            set
            {
                if (this.location != value)
                {
                    this.location = value;
                }
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        [Browsable(false)]
        public Forms.FormBase OwnerForm
        {
            get
            {
                return this.ownerForm;
            }
            set
            {
                this.ownerForm = value;
            }
        }

        [DefaultValue(typeof(System.Drawing.Size), "28, 20"), Description("设置或获取自定义系统按钮的大小"), Category("按钮大小")]
        public System.Drawing.Size Size
        {
            get
            {
                return this.size;
            }
            set
            {
                if (this.size != value)
                {
                    this.size = value;
                }
            }
        }

        [Description("自定义系统按钮点击时"), Category("按钮图像")]
        public Image SysButtonDown
        {
            get
            {
                return this.sysButtonDown;
            }
            set
            {
                if (this.sysButtonDown != value)
                {
                    this.sysButtonDown = value;
                }
            }
        }

        [Category("按钮图像"), Description("自定义系统按钮悬浮时")]
        public Image SysButtonMouse
        {
            get
            {
                return this.sysButtonMouse;
            }
            set
            {
                if (this.sysButtonMouse != value)
                {
                    this.sysButtonMouse = value;
                }
            }
        }

        [Category("按钮图像"), Description("自定义系统按钮初始时")]
        public Image SysButtonNorml
        {
            get
            {
                return this.sysButtonNorml;
            }
            set
            {
                if (this.sysButtonNorml != value)
                {
                    this.sysButtonNorml = value;
                }
            }
        }

        [Category("悬浮提示"), Description("自定义系统按钮悬浮提示")]
        public string ToolTip
        {
            get
            {
                return this.toolTip;
            }
            set
            {
                if (this.toolTip != value)
                {
                    this.toolTip = value;
                }
            }
        }

        [Description("自定义系统按钮是否显示"), DefaultValue(typeof(bool), "true"), Category("是否显示")]
        public bool Visibale
        {
            get
            {
                return this.visibale;
            }
            set
            {
                if (this.visibale != value)
                {
                    this.visibale = value;
                }
            }
        }
    }
}
