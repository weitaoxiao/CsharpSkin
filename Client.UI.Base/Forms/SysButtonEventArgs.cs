using System;
using System.Collections.Generic;
using System.Text;
using Client.UI.Base.Controls;

namespace Client.UI.Base.Forms
{
    public class SysButtonEventArgs
    {
        private CmSysButton sysButton;

        public SysButtonEventArgs(CmSysButton sysButton)
        {
            this.SysButton = sysButton;
        }

        public CmSysButton SysButton
        {
            get
            {
                return this.sysButton;
            }
            set
            {
                this.sysButton = value;
            }
        }
    }
}
