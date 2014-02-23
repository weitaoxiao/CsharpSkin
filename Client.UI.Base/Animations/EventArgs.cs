using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Client.UI.Base.Enums;

namespace Client.UI.Base.Animations
{
    public class AnimationCompletedEventArg : EventArgs
    {
        public AnimationCompletedEventArg()
        {

        }

        public Animation Animation { get; set; }

        public System.Windows.Forms.Control Control { get; internal set; }

        public AnimateMode Mode { get; internal set; }
    }

    public class TransfromNeededEventArg : EventArgs
    {
        public TransfromNeededEventArg()
        {
            this.Matrix = new System.Drawing.Drawing2D.Matrix(1f, 0f, 0f, 1f, 0f, 0f);
        }

        public Animation Animation { get; set; }

        public Rectangle ClientRectangle { get; internal set; }

        public Rectangle ClipRectangle { get; internal set; }

        public System.Windows.Forms.Control Control { get; internal set; }

        public float CurrentTime { get; internal set; }

        public System.Drawing.Drawing2D.Matrix Matrix { get; set; }

        public AnimateMode Mode { get; internal set; }

        public bool UseDefaultMatrix { get; set; }
    }

    public class NonLinearTransfromNeededEventArg : EventArgs
    {
        public NonLinearTransfromNeededEventArg()
        {
        }

        public Animation Animation { get; set; }

        public Rectangle ClientRectangle { get; internal set; }

        public System.Windows.Forms.Control Control { get; internal set; }

        public float CurrentTime { get; internal set; }

        public AnimateMode Mode { get; internal set; }

        public byte[] Pixels { get; internal set; }

        public Rectangle SourceClientRectangle { get; internal set; }

        public byte[] SourcePixels { get; internal set; }

        public int SourceStride { get; set; }

        public int Stride { get; internal set; }

        public bool UseDefaultTransform { get; set; }
    }
}
