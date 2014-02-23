using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Client.UI.Base.Forms
{
    public class BackEventArgs
    {
        private Image afterBack;
        private Image beforeBack;

        public BackEventArgs(Image beforeBack, Image afterBack)
        {
            this.beforeBack = beforeBack;
            this.afterBack = afterBack;
        }

        public Image AfterBack
        {
            get
            {
                return this.afterBack;
            }
        }

        public Image BeforeBack
        {
            get
            {
                return this.beforeBack;
            }
        }
    }
}
