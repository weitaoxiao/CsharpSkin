using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Client.UI.Base.Controls;
using Client.Core.Win32;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Client.UI.Base.Forms
{
    public partial class FormPrintScreen : Form
    {
        private Panel panel1;
        private ToolButton tBtn_Ellipse;
        private ToolButton tBtn_Rect;
        private ToolButton tBtn_Arrow;
        private ToolButton tBtn_Brush;
        private ToolButton tBtn_Text;
        private ToolButton tBtn_Finish;
        private ToolButton tBtn_Close;
        private ToolButton tBtn_Save;
        private ToolButton tBtn_Cancel;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private ImageProcessBox imageProcessBox1;
        private Panel panel2;
        private ColorBox colorBox1;
        private ToolButton toolButton1;
        private ToolButton toolButton3;
        private ToolButton toolButton2;
        private TextBox textBox1;
        private Timer timer1;
        private ToolButton tBtn_Out;
        private RtfRichTextBox RcTxt;
        private bool isCaptureCursor;
        private bool isFromClipBoard;
        private MouseHook m_MHook;
        private List<Bitmap> m_layer;
        private bool m_isStartDraw;
        private System.Drawing.Point m_ptOriginal;
        private System.Drawing.Point m_ptCurrent;
        private Bitmap m_bmpLayerCurrent;
        private Bitmap m_bmpLayerShow;
        private delegate void ControlHandler();

        public bool IsCaptureCursor
        {
            get
            {
                return this.isCaptureCursor;
            }
            set
            {
                this.isCaptureCursor = value;
            }
        }

        public bool IsFromClipBoard
        {
            get
            {
                return this.isFromClipBoard;
            }
            set
            {
                this.isFromClipBoard = value;
            }
        }

        public bool ImgProcessBoxIsShowInfo
        {
            get
            {
                return this.imageProcessBox1.IsShowInfo;
            }
            set
            {
                this.imageProcessBox1.IsShowInfo = value;
            }
        }

        public Color ImgProcessBoxDotColor
        {
            get
            {
                return this.imageProcessBox1.DotColor;
            }
            set
            {
                this.imageProcessBox1.DotColor = value;
            }
        }

        public Color ImgProcessBoxLineColor
        {
            get
            {
                return this.imageProcessBox1.LineColor;
            }
            set
            {
                this.imageProcessBox1.LineColor = value;
            }
        }

        public System.Drawing.Size ImgProcessBoxMagnifySize
        {
            get
            {
                return this.imageProcessBox1.MagnifySize;
            }
            set
            {
                this.imageProcessBox1.MagnifySize = value;
            }
        }

        public int ImgProcessBoxMagnifyTimes
        {
            get
            {
                return this.imageProcessBox1.MagnifyTimes;
            }
            set
            {
                this.imageProcessBox1.MagnifyTimes = value;
            }
        }

        public FormPrintScreen()
        {
            this.InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Location = new System.Drawing.Point(0, 0);
            this.Size = new System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            this.TopMost = true;
            this.ShowInTaskbar = false;
            this.m_MHook = new MouseHook();
            this.FormClosing += (FormClosingEventHandler)((sender, e) =>
            {
                this.m_MHook.UnLoadHook();
                this.DelResource();
            });
            this.imageProcessBox1.MouseLeave += (EventHandler)((sender, e) => this.Cursor = Cursors.Default);
            this.m_layer = new List<Bitmap>();
        }

        public FormPrintScreen(RtfRichTextBox rcTxt)
        {
            this.InitializeComponent();
            this.RcTxt = rcTxt;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Location = new System.Drawing.Point(0, 0);
            this.Size = new System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            this.TopMost = true;
            this.ShowInTaskbar = false;
            this.m_MHook = new MouseHook();
            this.FormClosing += (FormClosingEventHandler)((sender, e) =>
            {
                this.m_MHook.UnLoadHook();
                this.DelResource();
            });
            this.imageProcessBox1.MouseLeave += (EventHandler)((sender, e) => this.Cursor = Cursors.Default);
            this.m_layer = new List<Bitmap>();
        }





        private void DelResource()
        {
            if (this.m_bmpLayerCurrent != null)
                this.m_bmpLayerCurrent.Dispose();
            if (this.m_bmpLayerShow != null)
                this.m_bmpLayerShow.Dispose();
            this.m_layer.Clear();
            this.imageProcessBox1.DeleResource();
            GC.Collect();
        }

        private void InitMember()
        {
            this.panel1.Visible = false;
            this.panel2.Visible = false;
            this.panel1.BackColor = Color.White;
            this.panel2.BackColor = Color.White;
            this.panel1.Height = this.tBtn_Finish.Bottom + 3;
            this.panel1.Width = this.tBtn_Finish.Right + 3;
            this.panel2.Height = this.colorBox1.Height;
            this.panel1.Paint += (PaintEventHandler)((sender, e) => e.Graphics.DrawRectangle(Pens.SteelBlue, 0, 0, this.panel1.Width - 1, this.panel1.Height - 1));
            this.panel2.Paint += (PaintEventHandler)((sender, e) => e.Graphics.DrawRectangle(Pens.SteelBlue, 0, 0, this.panel2.Width - 1, this.panel2.Height - 1));
            this.tBtn_Rect.Click += new EventHandler(this.selectToolButton_Click);
            this.tBtn_Ellipse.Click += new EventHandler(this.selectToolButton_Click);
            this.tBtn_Arrow.Click += new EventHandler(this.selectToolButton_Click);
            this.tBtn_Brush.Click += new EventHandler(this.selectToolButton_Click);
            this.tBtn_Text.Click += new EventHandler(this.selectToolButton_Click);
            this.tBtn_Close.Click += (EventHandler)((sender, e) => this.Close());
            this.textBox1.BorderStyle = BorderStyle.None;
            this.textBox1.Visible = false;
            this.textBox1.ForeColor = Color.Red;
            this.colorBox1.ColorChanged += (ColorBox.ColorChangedHandler)((sender, e) => this.textBox1.ForeColor = e.Color);
        }

        private void FrmCapture_Load(object sender, EventArgs e)
        {
            this.InitMember();
            this.imageProcessBox1.BaseImage = (Image)FormPrintScreen.GetScreen(this.isCaptureCursor, this.isFromClipBoard);
            this.m_MHook.SetHook();
            this.m_MHook.MHookEvent += new MouseHook.MHookEventHandler(this.m_MHook_MHookEvent);
            this.imageProcessBox1.IsDrawOperationDot = false;
            //this.BeginInvoke((Delegate) (() => this.Enabled = false));
            ControlHandler handler = delegate()
            {
                base.Enabled = false;
            };
            base.BeginInvoke(handler);
            this.timer1.Interval = 500;
            this.timer1.Enabled = true;
        }

        private void m_MHook_MHookEvent(object sender, MHookEventArgs e)
        {
            if (!this.Enabled)
                this.imageProcessBox1.SetInfoPoint(Control.MousePosition.X, Control.MousePosition.Y);
            if (e.MButton == ButtonStatus.LeftDown || e.MButton == ButtonStatus.RightDown)
            {
                this.Enabled = true;
                this.imageProcessBox1.IsDrawOperationDot = true;
            }
            if (e.MButton == ButtonStatus.RightUp && !this.imageProcessBox1.IsDrawed)
            {
                //this.BeginInvoke((Delegate)(() => this.Close()));

                ControlHandler handler = delegate()
                {
                    this.Close();
                };
                base.BeginInvoke(handler);

            }
            if (this.Enabled)
                return;
            this.FoundAndDrawWindowRect();
        }

        private void selectToolButton_Click(object sender, EventArgs e)
        {
            this.panel2.Visible = ((ToolButton)sender).IsSelected;
            this.imageProcessBox1.CanReset = !this.panel2.Visible && this.m_layer.Count == 0;
            this.SetToolBarLocation();
        }

        private void imageProcessBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.imageProcessBox1.Cursor != Cursors.SizeAll && this.imageProcessBox1.Cursor != Cursors.Default)
                this.panel1.Visible = false;
            if (e.Button == MouseButtons.Left && this.imageProcessBox1.IsDrawed && this.HaveSelectedToolButton() && this.imageProcessBox1.SelectedRectangle.Contains(e.Location))
            {
                if (this.tBtn_Text.IsSelected)
                {
                    this.textBox1.Location = e.Location;
                    this.textBox1.Visible = true;
                    this.textBox1.Focus();
                    return;
                }
                else
                {
                    this.m_isStartDraw = true;
                    Cursor.Clip = this.imageProcessBox1.SelectedRectangle;
                }
            }
            this.m_ptOriginal = e.Location;
        }

        private void imageProcessBox1_MouseMove(object sender, MouseEventArgs e)
        {
            this.m_ptCurrent = e.Location;
            if (this.imageProcessBox1.SelectedRectangle.Contains(e.Location) && this.HaveSelectedToolButton() && this.imageProcessBox1.IsDrawed)
                this.Cursor = Cursors.Cross;
            else if (!this.imageProcessBox1.SelectedRectangle.Contains(e.Location))
                this.Cursor = Cursors.Default;
            if (this.imageProcessBox1.IsStartDraw && this.panel1.Visible)
                this.SetToolBarLocation();
            if (!this.m_isStartDraw || this.m_bmpLayerShow == null)
                return;
            using (Graphics graphics = Graphics.FromImage((Image)this.m_bmpLayerShow))
            {
                int num1 = 1;
                if (this.toolButton2.IsSelected)
                    num1 = 3;
                if (this.toolButton3.IsSelected)
                    num1 = 5;
                Pen pen = new Pen(this.colorBox1.SelectedColor, (float)num1);
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                if (this.tBtn_Rect.IsSelected)
                {
                    int num2 = e.X - this.m_ptOriginal.X > 0 ? this.m_ptOriginal.X : e.X;
                    int num3 = e.Y - this.m_ptOriginal.Y > 0 ? this.m_ptOriginal.Y : e.Y;
                    graphics.Clear(Color.Transparent);
                    graphics.DrawRectangle(pen, num2 - this.imageProcessBox1.SelectedRectangle.Left, num3 - this.imageProcessBox1.SelectedRectangle.Top, Math.Abs(e.X - this.m_ptOriginal.X), Math.Abs(e.Y - this.m_ptOriginal.Y));
                    this.imageProcessBox1.Invalidate();
                }
                if (this.tBtn_Ellipse.IsSelected)
                {
                    graphics.DrawLine(Pens.Red, 0, 0, 200, 200);
                    graphics.Clear(Color.Transparent);
                    graphics.DrawEllipse(pen, this.m_ptOriginal.X - this.imageProcessBox1.SelectedRectangle.Left, this.m_ptOriginal.Y - this.imageProcessBox1.SelectedRectangle.Top, e.X - this.m_ptOriginal.X, e.Y - this.m_ptOriginal.Y);
                    this.imageProcessBox1.Invalidate();
                }
                if (this.tBtn_Arrow.IsSelected)
                {
                    graphics.Clear(Color.Transparent);
                    AdjustableArrowCap adjustableArrowCap = new AdjustableArrowCap(5f, 5f, true);
                    pen.CustomEndCap = (CustomLineCap)adjustableArrowCap;
                    graphics.DrawLine(pen, (System.Drawing.Point)((System.Drawing.Size)this.m_ptOriginal - (System.Drawing.Size)this.imageProcessBox1.SelectedRectangle.Location), (System.Drawing.Point)((System.Drawing.Size)this.m_ptCurrent - (System.Drawing.Size)this.imageProcessBox1.SelectedRectangle.Location));
                    this.imageProcessBox1.Invalidate();
                }
                if (this.tBtn_Brush.IsSelected)
                {
                    System.Drawing.Point pt1 = (System.Drawing.Point)((System.Drawing.Size)this.m_ptOriginal - (System.Drawing.Size)this.imageProcessBox1.SelectedRectangle.Location);
                    pen.LineJoin = LineJoin.Round;
                    graphics.DrawLine(pen, pt1, (System.Drawing.Point)((System.Drawing.Size)e.Location - (System.Drawing.Size)this.imageProcessBox1.SelectedRectangle.Location));
                    this.m_ptOriginal = e.Location;
                    this.imageProcessBox1.Invalidate();
                }
                pen.Dispose();
            }
        }

        private void imageProcessBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.IsDisposed)
                return;
            if (e.Button == MouseButtons.Right)
            {
                this.Enabled = false;
                this.imageProcessBox1.ClearDraw();
                this.imageProcessBox1.CanReset = true;
                this.imageProcessBox1.IsDrawOperationDot = false;
                this.m_layer.Clear();
                this.m_bmpLayerCurrent = (Bitmap)null;
                this.m_bmpLayerShow = (Bitmap)null;
                this.ClearToolBarBtnSelected();
                this.panel1.Visible = false;
                this.panel2.Visible = false;
            }
            if (!this.imageProcessBox1.IsDrawed)
            {
                this.Enabled = false;
                this.imageProcessBox1.IsDrawOperationDot = false;
            }
            else if (!this.panel1.Visible)
            {
                this.SetToolBarLocation();
                this.panel1.Visible = true;
                this.m_bmpLayerCurrent = this.imageProcessBox1.GetResultBmp();
                this.m_bmpLayerShow = new Bitmap(this.m_bmpLayerCurrent.Width, this.m_bmpLayerCurrent.Height);
            }
            if (this.imageProcessBox1.Cursor == Cursors.SizeAll && this.m_ptOriginal != e.Location)
            {
                this.m_bmpLayerCurrent.Dispose();
                this.m_bmpLayerCurrent = this.imageProcessBox1.GetResultBmp();
            }
            if (!this.m_isStartDraw)
                return;
            Cursor.Clip = Rectangle.Empty;
            this.m_isStartDraw = false;
            if (e.Location == this.m_ptOriginal && !this.tBtn_Brush.IsSelected)
                return;
            this.SetLayer();
        }

        private void imageProcessBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            if (this.m_layer.Count > 0)
                graphics.DrawImage((Image)this.m_layer[this.m_layer.Count - 1], this.imageProcessBox1.SelectedRectangle.Location);
            if (this.m_bmpLayerShow == null)
                return;
            graphics.DrawImage((Image)this.m_bmpLayerShow, this.imageProcessBox1.SelectedRectangle.Location);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            System.Drawing.Size size = TextRenderer.MeasureText(this.textBox1.Text, this.textBox1.Font);
            this.textBox1.Size = size.IsEmpty ? new System.Drawing.Size(50, this.textBox1.Font.Height) : size;
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            this.textBox1.Visible = false;
            if (string.IsNullOrEmpty(this.textBox1.Text.Trim()))
            {
                this.textBox1.Text = "";
            }
            else
            {
                using (Graphics graphics = Graphics.FromImage((Image)this.m_bmpLayerCurrent))
                {
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    SolidBrush solidBrush = new SolidBrush(this.colorBox1.SelectedColor);
                    graphics.DrawString(this.textBox1.Text, this.textBox1.Font, (Brush)solidBrush, (float)(this.textBox1.Left - this.imageProcessBox1.SelectedRectangle.Left), (float)(this.textBox1.Top - this.imageProcessBox1.SelectedRectangle.Top));
                    solidBrush.Dispose();
                    this.textBox1.Text = "";
                    this.SetLayer();
                    this.imageProcessBox1.Invalidate();
                }
            }
        }

        private void textBox1_Resize(object sender, EventArgs e)
        {
            int num = 10;
            if (this.toolButton2.IsSelected)
                num = 12;
            if (this.toolButton3.IsSelected)
                num = 14;
            if (this.textBox1.Font.Height == num)
                return;
            this.textBox1.Font = new Font(this.Font.FontFamily, (float)num);
        }

        private void tBtn_Cancel_Click(object sender, EventArgs e)
        {
            using (Graphics graphics = Graphics.FromImage((Image)this.m_bmpLayerShow))
            {
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.Clear(Color.Transparent);
            }
            if (this.m_layer.Count > 0)
            {
                this.m_layer.RemoveAt(this.m_layer.Count - 1);
                this.m_bmpLayerCurrent = this.m_layer.Count <= 0 ? this.imageProcessBox1.GetResultBmp() : ((Image)this.m_layer[this.m_layer.Count - 1]).Clone() as Bitmap;
                this.imageProcessBox1.Invalidate();
                this.imageProcessBox1.CanReset = this.m_layer.Count == 0 && !this.HaveSelectedToolButton();
            }
            else
            {
                this.Enabled = false;
                this.imageProcessBox1.ClearDraw();
                this.imageProcessBox1.IsDrawOperationDot = false;
                this.panel1.Visible = false;
                this.panel2.Visible = false;
            }
        }

        private void tBtn_Save_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Bitmap(*.bmp)|*.bmp|JPEG(*.jpg)|*.jpg";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.FileName = "CAPTURE_" + this.GetTimeString();
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;
            switch (saveFileDialog.FilterIndex)
            {
                case 1:
                    this.m_bmpLayerCurrent.Clone(new Rectangle(0, 0, this.m_bmpLayerCurrent.Width, this.m_bmpLayerCurrent.Height), PixelFormat.Format24bppRgb).Save(saveFileDialog.FileName, ImageFormat.Bmp);
                    this.Close();
                    break;
                case 2:
                    this.m_bmpLayerCurrent.Save(saveFileDialog.FileName, ImageFormat.Jpeg);
                    this.Close();
                    break;
            }
        }

        private void tBtn_Finish_Click(object sender, EventArgs e)
        {
            Clipboard.SetImage((Image)this.m_bmpLayerCurrent);
            if (this.RcTxt != null)
                ((TextBoxBase)this.RcTxt).Paste();
            this.Close();
        }

        //private void tBtn_Out_Click(object sender, EventArgs e)
        //{
        //    ((Control)new FrmOut(((Image)this.m_bmpLayerCurrent).Clone() as Bitmap)).Show();
        //    this.Close();
        //}

        private void imageProcessBox1_DoubleClick(object sender, EventArgs e)
        {
            Clipboard.SetImage((Image)this.m_bmpLayerCurrent);
            if (this.RcTxt != null)
                ((TextBoxBase)this.RcTxt).Paste();
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.Enabled)
                return;
            this.imageProcessBox1.SetInfoPoint(Control.MousePosition.X, Control.MousePosition.Y);
        }

        private void FoundAndDrawWindowRect()
        {
            NativeMethods.Point lpPoint = new NativeMethods.Point();
            lpPoint.x = Control.MousePosition.X;
            lpPoint.y = Control.MousePosition.Y;
            IntPtr num = NativeMethods.ChildWindowFromPointEx(NativeMethods.GetDesktopWindow(), lpPoint, 3U);
            if (!(num != IntPtr.Zero))
                return;
            IntPtr hWnd = num;
            while (true)
            {
                NativeMethods.ScreenToClient(hWnd, ref lpPoint);
                hWnd = NativeMethods.ChildWindowFromPointEx(num, lpPoint, 1U);
                if (!(hWnd == IntPtr.Zero) && !(hWnd == num))
                {
                    num = hWnd;
                    lpPoint.x = Control.MousePosition.X;
                    lpPoint.y = Control.MousePosition.Y;
                }
                else
                    break;
            }
            RECT lpRect = new RECT();
            NativeMethods.GetWindowRect(num, ref lpRect);
            this.imageProcessBox1.SetSelectRect(new Rectangle(lpRect.Left, lpRect.Top, lpRect.Right - lpRect.Left, lpRect.Bottom - lpRect.Top));
        }

        private static Bitmap GetScreen(bool bCaptureCursor, bool bFromClipBoard)
        {
            Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            if (bCaptureCursor)
                FormPrintScreen.DrawCurToScreen();
            using (Graphics graphics = Graphics.FromImage((Image)bitmap))
            {
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
                if (!bFromClipBoard)
                    return bitmap;
                using (Image image = Clipboard.GetImage())
                {
                    if (image != null)
                    {
                        using (SolidBrush solidBrush = new SolidBrush(Color.FromArgb(150, 0, 0, 0)))
                        {
                            graphics.FillRectangle((Brush)solidBrush, 0, 0, bitmap.Width, bitmap.Height);
                            graphics.DrawImage(image, bitmap.Width - image.Width >> 1, bitmap.Height - image.Height >> 1, image.Width, image.Height);
                        }
                    }
                }
            }
            return bitmap;
        }

        public static Rectangle DrawCurToScreen()
        {
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                NativeMethods.PCURSORINFO pci;
                pci.cbSize = Marshal.SizeOf(typeof(NativeMethods.PCURSORINFO));
                NativeMethods.GetCursorInfo(out pci);
                if (!(pci.hCursor != IntPtr.Zero))
                    return Rectangle.Empty;
                Cursor cursor = new Cursor(pci.hCursor);
                Rectangle targetRect = new Rectangle((System.Drawing.Point)((System.Drawing.Size)Control.MousePosition - (System.Drawing.Size)cursor.HotSpot), cursor.Size);
                g.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size);
                cursor.Draw(g, targetRect);
                return targetRect;
            }
        }

        private void SetToolBarLocation()
        {
            int num1 = this.imageProcessBox1.SelectedRectangle.Left;
            int num2 = this.imageProcessBox1.SelectedRectangle.Bottom + 5;
            int num3 = this.panel2.Visible ? this.panel2.Height + 2 : 0;
            if (num2 + this.panel1.Height + num3 >= this.Height)
                num2 = this.imageProcessBox1.SelectedRectangle.Top - this.panel1.Height - 10 - this.imageProcessBox1.Font.Height;
            if (num2 - num3 <= 0)
                num2 = this.imageProcessBox1.SelectedRectangle.Top - 5 - this.imageProcessBox1.Font.Height < 0 ? this.imageProcessBox1.SelectedRectangle.Top + 10 + this.imageProcessBox1.Font.Height : this.imageProcessBox1.SelectedRectangle.Top + 5;
            if (num1 + this.panel1.Width >= this.Width)
                num1 = this.Width - this.panel1.Width - 5;
            this.panel1.Left = num1;
            this.panel2.Left = num1;
            this.panel1.Top = num2;
            this.panel2.Top = this.imageProcessBox1.SelectedRectangle.Top > num2 ? num2 - num3 : this.panel1.Bottom + 2;
        }

        private bool HaveSelectedToolButton()
        {
            if (!this.tBtn_Rect.IsSelected && !this.tBtn_Ellipse.IsSelected && (!this.tBtn_Arrow.IsSelected && !this.tBtn_Brush.IsSelected))
                return this.tBtn_Text.IsSelected;
            else
                return true;
        }

        private void ClearToolBarBtnSelected()
        {
            ToolButton toolButton1 = this.tBtn_Rect;
            ToolButton toolButton2 = this.tBtn_Ellipse;
            ToolButton toolButton3 = this.tBtn_Arrow;
            ToolButton toolButton4 = this.tBtn_Brush;
            this.tBtn_Text.IsSelected = false;
            int num1 = 0;
            toolButton4.IsSelected = num1 != 0;
            int num2 = 0;
            toolButton3.IsSelected = num2 != 0;
            int num3 = 0;
            toolButton2.IsSelected = num3 != 0;
            int num4 = 0;
            toolButton1.IsSelected = num4 != 0;
        }

        private void SetLayer()
        {
            if (this.IsDisposed)
                return;
            using (Graphics graphics = Graphics.FromImage((Image)this.m_bmpLayerCurrent))
            {
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.DrawImage((Image)this.m_bmpLayerShow, 0, 0);
            }
            this.m_layer.Add(((Image)this.m_bmpLayerCurrent).Clone() as Bitmap);
        }

        private string GetTimeString()
        {
            DateTime now = DateTime.Now;
            return now.Date.ToShortDateString().Replace("/", "") + "_" + now.ToLongTimeString().Replace(":", "");
        }

        private void FormPrintScreen_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
                this.Close();
        }
    }
}
