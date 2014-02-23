using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing.Imaging;

namespace Client.UI.Base.Controls
{
    [ToolboxBitmap(typeof(RichTextBox))]
    public class RtfRichTextBox : RichTextBox
    {
        private RtfRichTextBox.RtfColor highlightColor = RtfRichTextBox.RtfColor.White;
        private Dictionary<string, Bitmap> emotions = new Dictionary<string, Bitmap>();
        private Dictionary<RtfRichTextBox.RtfColor, string> rtfColor = new Dictionary<RtfRichTextBox.RtfColor, string>();
        private Dictionary<string, string> rtfFontFamily = new Dictionary<string, string>();
        private const string RTF_HEADER = "{\\rtf1\\ansi\\ansicpg936\\deff0\\deflang1033\\deflangfe2052";
        private static bool hasGdiPlus;
        private float xDpi;
        private float yDpi;
        private RtfRichTextBox.RtfColor textColor;

        public Dictionary<string, Bitmap> Emotions
        {
            get
            {
                return this.emotions;
            }
        }

        public bool HasEmotion
        {
            get
            {
                if (RtfRichTextBox.hasGdiPlus)
                {
                    foreach (string str in this.emotions.Keys)
                    {
                        if (this.Text.IndexOf(str, StringComparison.CurrentCultureIgnoreCase) > -1)
                            return true;
                    }
                }
                return false;
            }
        }

        public RtfRichTextBox.RtfColor HiglightColor
        {
            get
            {
                return this.highlightColor;
            }
            set
            {
                this.highlightColor = value;
            }
        }

        public new string Rtf
        {
            get
            {
                return this.RemoveBadChars(base.Rtf);
            }
            set
            {
                base.Rtf = value;
            }
        }

        public RtfRichTextBox.RtfColor TextColor
        {
            get
            {
                return this.textColor;
            }
            set
            {
                this.textColor = value;
            }
        }

        static RtfRichTextBox()
        {
            try
            {
                int num = (int)RtfRichTextBox.GdipEmfToWmfBits(IntPtr.Zero, 0U, (byte[])null, 0, RtfRichTextBox.EmfToWmfBitsFlags.EmfToWmfBitsFlagsDefault);
                RtfRichTextBox.hasGdiPlus = true;
            }
            catch (Exception ex)
            {
            }
        }

        public RtfRichTextBox()
        {
            this.rtfColor.Add(RtfRichTextBox.RtfColor.Aqua, "\\red0\\green255\\blue255");
            this.rtfColor.Add(RtfRichTextBox.RtfColor.Black, "\\red0\\green0\\blue0");
            this.rtfColor.Add(RtfRichTextBox.RtfColor.Blue, "\\red0\\green0\\blue255");
            this.rtfColor.Add(RtfRichTextBox.RtfColor.Fuchsia, "\\red255\\green0\\blue255");
            this.rtfColor.Add(RtfRichTextBox.RtfColor.Gray, "\\red128\\green128\\blue128");
            this.rtfColor.Add(RtfRichTextBox.RtfColor.Green, "\\red0\\green128\\blue0");
            this.rtfColor.Add(RtfRichTextBox.RtfColor.Lime, "\\red0\\green255\\blue0");
            this.rtfColor.Add(RtfRichTextBox.RtfColor.Maroon, "\\red128\\green0\\blue0");
            this.rtfColor.Add(RtfRichTextBox.RtfColor.Navy, "\\red0\\green0\\blue128");
            this.rtfColor.Add(RtfRichTextBox.RtfColor.Olive, "\\red128\\green128\\blue0");
            this.rtfColor.Add(RtfRichTextBox.RtfColor.Purple, "\\red128\\green0\\blue128");
            this.rtfColor.Add(RtfRichTextBox.RtfColor.Red, "\\red255\\green0\\blue0");
            this.rtfColor.Add(RtfRichTextBox.RtfColor.Silver, "\\red192\\green192\\blue192");
            this.rtfColor.Add(RtfRichTextBox.RtfColor.Teal, "\\red0\\green128\\blue128");
            this.rtfColor.Add(RtfRichTextBox.RtfColor.White, "\\red255\\green255\\blue255");
            this.rtfColor.Add(RtfRichTextBox.RtfColor.Yellow, "\\red255\\green255\\blue0");
            this.rtfFontFamily.Add(FontFamily.GenericMonospace.Name, "\\fmodern");
            this.rtfFontFamily.Add(FontFamily.GenericSansSerif.Name, "\\fswiss");
            this.rtfFontFamily.Add(FontFamily.GenericSerif.Name, "\\froman");
            this.rtfFontFamily.Add("UNKNOWN", "\\fnil");
            using (Graphics graphics = this.CreateGraphics())
            {
                this.xDpi = graphics.DpiX;
                this.yDpi = graphics.DpiY;
            }
        }

        public RtfRichTextBox(RtfRichTextBox.RtfColor _textColor)
            : this()
        {
            this.textColor = _textColor;
        }

        public RtfRichTextBox(RtfRichTextBox.RtfColor _textColor, RtfRichTextBox.RtfColor _highlightColor)
            : this()
        {
            this.textColor = _textColor;
            this.highlightColor = _highlightColor;
        }

        [DllImport("gdiplus.dll")]
        private static extern uint GdipEmfToWmfBits(IntPtr _hEmf, uint _bufferSize, byte[] _buffer, int _mappingMode, RtfRichTextBox.EmfToWmfBitsFlags _flags);

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
        }

        public void AppendRtf(string _rtf)
        {
            this.Select(this.TextLength, 0);
            this.SelectionColor = Color.Black;
            this.SelectedRtf = _rtf;
        }

        public void AppendTextAsRtf(string _text)
        {
            this.AppendTextAsRtf(_text, this.Font);
        }

        public void AppendTextAsRtf(string _text, Font _font)
        {
            this.AppendTextAsRtf(_text, _font, this.textColor);
        }

        public void AppendTextAsRtf(string _text, Font _font, RtfRichTextBox.RtfColor _textColor)
        {
            this.AppendTextAsRtf(_text, _font, _textColor, this.highlightColor);
        }

        public void AppendTextAsRtf(string _text, Font _font, RtfRichTextBox.RtfColor _textColor, RtfRichTextBox.RtfColor _backColor)
        {
            this.Select(this.TextLength, 0);
            this.InsertTextAsRtf(_text, _font, _textColor, _backColor);
        }

        private string GetColorTable(RtfRichTextBox.RtfColor _textColor, RtfRichTextBox.RtfColor _backColor)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{\\colortbl ;");
            stringBuilder.Append(this.rtfColor[_textColor]);
            stringBuilder.Append(";");
            stringBuilder.Append(this.rtfColor[_backColor]);
            stringBuilder.Append(";}\\n");
            return ((object)stringBuilder).ToString();
        }

        private string GetDocumentArea(string _text, Font _font)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("\\viewkind4\\uc1\\pard\\cf1\\f0\\fs20");
            stringBuilder.Append("\\highlight2");
            if (_font.Bold)
                stringBuilder.Append("\\b");
            if (_font.Italic)
                stringBuilder.Append("\\i");
            if (_font.Strikeout)
                stringBuilder.Append("\\strike");
            if (_font.Underline)
                stringBuilder.Append("\\ul");
            stringBuilder.Append("\\f0");
            stringBuilder.Append("\\fs");
            stringBuilder.Append((int)Math.Round(2.0 * (double)_font.SizeInPoints));
            stringBuilder.Append(" ");
            stringBuilder.Append(_text.Replace("\n", "\\par "));
            stringBuilder.Append("\\highlight0");
            if (_font.Bold)
                stringBuilder.Append("\\b0");
            if (_font.Italic)
                stringBuilder.Append("\\i0");
            if (_font.Strikeout)
                stringBuilder.Append("\\strike0");
            if (_font.Underline)
                stringBuilder.Append("\\ulnone");
            stringBuilder.Append("\\f0");
            stringBuilder.Append("\\fs20");
            stringBuilder.Append("\\cf0\\fs17}");
            return ((object)stringBuilder).ToString();
        }

        private string GetFontTable(Font _font)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{\\fonttbl{\\f0");
            stringBuilder.Append("\\");
            if (this.rtfFontFamily.ContainsKey(_font.FontFamily.Name))
                stringBuilder.Append(this.rtfFontFamily[_font.FontFamily.Name]);
            else
                stringBuilder.Append(this.rtfFontFamily["UNKNOWN"]);
            stringBuilder.Append("\\fcharset134 ");
            stringBuilder.Append(_font.Name);
            stringBuilder.Append(";}}");
            return ((object)stringBuilder).ToString();
        }

        private string GetImagePrefix(Image _image)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int num1 = (int)Math.Round((double)_image.Width / (double)this.xDpi * 2540.0);
            int num2 = (int)Math.Round((double)_image.Height / (double)this.yDpi * 2540.0);
            int num3 = (int)Math.Round((double)_image.Width / (double)this.xDpi * 1440.0);
            int num4 = (int)Math.Round((double)_image.Height / (double)this.yDpi * 1440.0);
            stringBuilder.Append("{\\pict\\wmetafile8");
            stringBuilder.Append("\\picw");
            stringBuilder.Append(num1);
            stringBuilder.Append("\\pich");
            stringBuilder.Append(num2);
            stringBuilder.Append("\\picwgoal");
            stringBuilder.Append(num3);
            stringBuilder.Append("\\pichgoal");
            stringBuilder.Append(num4);
            stringBuilder.Append(" ");
            return ((object)stringBuilder).ToString();
        }

        private string GetRtfImage(Image _image)
        {
            MemoryStream stream = null;
            Graphics graphics = null;
            Metafile image = null;
            string str;
            try
            {
                stream = new MemoryStream();
                using (graphics = base.CreateGraphics())
                {
                    IntPtr hdc = graphics.GetHdc();
                    image = new Metafile(stream, hdc);
                    graphics.ReleaseHdc(hdc);
                }
                using (graphics = Graphics.FromImage(image))
                {
                    graphics.DrawImage(_image, new Rectangle(0, 0, _image.Width, _image.Height));
                }
                IntPtr henhmetafile = image.GetHenhmetafile();
                uint num = GdipEmfToWmfBits(henhmetafile, 0, null, 1, EmfToWmfBitsFlags.EmfToWmfBitsFlagsDefault);
                byte[] buffer = new byte[num];
                GdipEmfToWmfBits(henhmetafile, num, buffer, 1, EmfToWmfBitsFlags.EmfToWmfBitsFlagsDefault);
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < buffer.Length; i++)
                {
                    builder.Append(string.Format("{0:X2}", buffer[i]));
                }
                str = builder.ToString();
            }
            finally
            {
                if (graphics != null)
                {
                    graphics.Dispose();
                }
                if (image != null)
                {
                    image.Dispose();
                }
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return str;

        }

        public void InsertEmotion()
        {
            if (!RtfRichTextBox.hasGdiPlus)
                return;
            foreach (string str in this.emotions.Keys)
            {
                int start = this.Find(str, RichTextBoxFinds.None);
                if (start > -1)
                {
                    this.Select(start, str.Length);
                    this.InsertImage((Image)this.emotions[str]);
                }
            }
        }

        public void InsertImage(Image _image)
        {
            if (!RtfRichTextBox.hasGdiPlus)
                return;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{\\rtf1\\ansi\\ansicpg936\\deff0\\deflang1033\\deflangfe2052");
            stringBuilder.Append(this.GetFontTable(this.Font));
            stringBuilder.Append(this.GetImagePrefix(_image));
            stringBuilder.Append(this.GetRtfImage(_image));
            stringBuilder.Append("}");
            this.SelectedRtf = ((object)stringBuilder).ToString();
        }

        public void InsertRtf(string _rtf)
        {
            this.SelectedRtf = _rtf;
        }

        public void InsertTextAsRtf(string _text)
        {
            this.InsertTextAsRtf(_text, this.Font);
        }

        public void InsertTextAsRtf(string _text, Font _font)
        {
            this.InsertTextAsRtf(_text, _font, this.textColor);
        }

        public void InsertTextAsRtf(string _text, Font _font, RtfRichTextBox.RtfColor _textColor)
        {
            this.InsertTextAsRtf(_text, _font, _textColor, this.highlightColor);
        }

        public void InsertTextAsRtf(string _text, Font _font, RtfRichTextBox.RtfColor _textColor, RtfRichTextBox.RtfColor _backColor)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{\\rtf1\\ansi\\ansicpg936\\deff0\\deflang1033\\deflangfe2052");
            stringBuilder.Append(this.GetFontTable(_font));
            stringBuilder.Append(this.GetColorTable(_textColor, _backColor));
            stringBuilder.Append(this.GetDocumentArea(_text, _font));
            this.SelectedRtf = ((object)stringBuilder).ToString();
        }

        private string RemoveBadChars(string _originalRtf)
        {
            return _originalRtf.Replace("\0", "");
        }

        [Flags]
        private enum EmfToWmfBitsFlags
        {
            EmfToWmfBitsFlagsDefault = 0,
            EmfToWmfBitsFlagsEmbedEmf = 1,
            EmfToWmfBitsFlagsIncludePlaceable = 2,
            EmfToWmfBitsFlagsNoXORClip = 4,
        }

        public enum RtfColor
        {
            Black,
            Maroon,
            Green,
            Olive,
            Navy,
            Purple,
            Teal,
            Gray,
            Silver,
            Red,
            Lime,
            Yellow,
            Blue,
            Fuchsia,
            Aqua,
            White,
        }
    }
}
