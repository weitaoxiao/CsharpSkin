using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using Client.UI.Base.Collection.Item;
using Client.UI.Base.Enums;
using Client.UI.Base.Collection;
using System.Threading;
using System.Drawing.Drawing2D;
using Client.UI.DefaultResource;

namespace Client.UI.Base.Controls
{
    public class ChatListBox : Control
    {
        private Color arrowColor = Color.FromArgb(101, 103, 103);
        private Color itemColor = Color.Transparent;
        private Color subItemColor = Color.Transparent;
        private Color itemMouseOnColor = Color.FromArgb(150, 230, 238, 241);
        private Color subItemMouseOnColor = Color.FromArgb(200, 252, 240, 193);
        private Color subItemSelectColor = Color.FromArgb(200, 252, 236, 172);
        private Color vipFontColor = Color.FromArgb(0, (int)byte.MaxValue, 0, 0);
        private ContextMenuStrip subItemMenu;
        private ContextMenuStrip listsubItemMenu;
        private ChatListItemIcon iconSizeMode;
        private ChatListItemCollection items;
        private ChatListSubItem selectSubItem;
        private ChatListItem selectItem;
        private Point m_ptMousePos;
        public ChatListVScroll chatListVScroll_0;
        private ChatListItem m_mouseOnItem;
        private bool m_bOnMouseEnterHeaded;
        private ChatListSubItem m_mouseOnSubItem;
        private bool MouseDowns;
        private ChatListSubItem MouseDowmSubItems;
        private int CursorY;
        private bool MouseMoveItems;
        private List<ChatListItem> listHadOpenGroup;
        private IContainer components;

        [Description("当用户右击分组时显示的快捷菜单。")]
        [Category("Skin")]
        public ContextMenuStrip SubItemMenu
        {
            get
            {
                return this.subItemMenu;
            }
            set
            {
                if (this.subItemMenu == value)
                    return;
                this.subItemMenu = value;
            }
        }

        [Category("Skin")]
        [Description("当用户右击好友时显示的快捷菜单。")]
        public ContextMenuStrip ListSubItemMenu
        {
            get
            {
                return this.listsubItemMenu;
            }
            set
            {
                if (this.listsubItemMenu == value)
                    return;
                this.listsubItemMenu = value;
            }
        }

        [DefaultValue(ChatListItemIcon.Large)]
        [Description("与列表关联的图标模式")]
        [Category("Skin")]
        public ChatListItemIcon IconSizeMode
        {
            get
            {
                return this.iconSizeMode;
            }
            set
            {
                if (this.iconSizeMode == value)
                    return;
                this.iconSizeMode = value;
                this.Invalidate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Skin")]
        [Description("列表框中的项")]
        public ChatListItemCollection Items
        {
            get
            {
                if (this.items == null)
                    this.items = new ChatListItemCollection(this);
                return this.items;
            }
        }

        [Browsable(false)]
        public ChatListSubItem SelectSubItem
        {
            get
            {
                return this.selectSubItem;
            }
            set
            {
                this.selectSubItem = value;
                this.MouseDowmSubItems = value;
                if (value == null)
                    return;
                this.selectSubItem.OwnerListItem.IsOpen = true;
            }
        }

        [Browsable(false)]
        public ChatListItem SelectItem
        {
            get
            {
                return this.selectItem;
            }
        }

        [Category("滚动条")]
        [DefaultValue(typeof(Color), "50, 224, 239, 235")]
        [Description("滚动条的背景颜色")]
        public Color ScrollBackColor
        {
            get
            {
                return this.chatListVScroll_0.BackColor;
            }
            set
            {
                this.chatListVScroll_0.BackColor = value;
            }
        }

        [Category("滚动条")]
        [Description("滚动条滑块默认情况下的颜色")]
        [DefaultValue(typeof(Color), "100, 110, 111, 112")]
        public Color ScrollSliderDefaultColor
        {
            get
            {
                return this.chatListVScroll_0.SliderDefaultColor;
            }
            set
            {
                this.chatListVScroll_0.SliderDefaultColor = value;
            }
        }

        [Category("滚动条")]
        [DefaultValue(typeof(Color), "200, 110, 111, 112")]
        [Description("滚动条滑块被点击或者鼠标移动到上面时候的颜色")]
        public Color ScrollSliderDownColor
        {
            get
            {
                return this.chatListVScroll_0.SliderDownColor;
            }
            set
            {
                this.chatListVScroll_0.SliderDownColor = value;
            }
        }

        [Category("滚动条")]
        [Description("滚动条箭头的背景颜色")]
        [DefaultValue(typeof(Color), "Transparent")]
        public Color ScrollArrowBackColor
        {
            get
            {
                return this.chatListVScroll_0.ArrowBackColor;
            }
            set
            {
                this.chatListVScroll_0.ArrowBackColor = value;
            }
        }

        [Category("滚动条")]
        [Description("滚动条箭头的颜色")]
        [DefaultValue(typeof(Color), "200, 148, 150, 151")]
        public Color ScrollArrowColor
        {
            get
            {
                return this.chatListVScroll_0.ArrowColor;
            }
            set
            {
                this.chatListVScroll_0.ArrowColor = value;
            }
        }

        [Description("列表项上面的箭头的颜色")]
        [DefaultValue(typeof(Color), "101, 103, 103")]
        [Category("分组")]
        public Color ArrowColor
        {
            get
            {
                return this.arrowColor;
            }
            set
            {
                if (this.arrowColor == value)
                    return;
                this.arrowColor = value;
                this.Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "Transparent")]
        [Description("列表项的背景色")]
        [Category("分组")]
        public Color ItemColor
        {
            get
            {
                return this.itemColor;
            }
            set
            {
                if (this.itemColor == value)
                    return;
                this.itemColor = value;
            }
        }

        [Description("列表子项的背景色")]
        [DefaultValue(typeof(Color), "Transparent")]
        [Category("好友")]
        public Color SubItemColor
        {
            get
            {
                return this.subItemColor;
            }
            set
            {
                if (this.subItemColor == value)
                    return;
                this.subItemColor = value;
            }
        }

        [Category("分组")]
        [DefaultValue(typeof(Color), "150, 230, 238, 241")]
        [Description("当鼠标移动到列表项上面的颜色")]
        public Color ItemMouseOnColor
        {
            get
            {
                return this.itemMouseOnColor;
            }
            set
            {
                this.itemMouseOnColor = value;
            }
        }

        [DefaultValue(typeof(Color), "200, 252, 240, 193")]
        [Description("当鼠标移动到子项上面的颜色")]
        [Category("好友")]
        public Color SubItemMouseOnColor
        {
            get
            {
                return this.subItemMouseOnColor;
            }
            set
            {
                this.subItemMouseOnColor = value;
            }
        }

        [Description("当列表子项被选中时候的颜色")]
        [Category("好友")]
        [DefaultValue(typeof(Color), "200, 252, 236, 172")]
        public Color SubItemSelectColor
        {
            get
            {
                return this.subItemSelectColor;
            }
            set
            {
                this.subItemSelectColor = value;
            }
        }

        [Category("好友")]
        [DefaultValue(typeof(Color), "0, 255, 0, 0")]
        [Description("Vip用户备注字体的颜色")]
        public Color VipFontColor
        {
            get
            {
                return this.vipFontColor;
            }
            set
            {
                this.vipFontColor = value;
            }
        }

        public List<ChatListItem> ListHadOpenGroup
        {
            get
            {
                return this.listHadOpenGroup;
            }
            set
            {
                this.listHadOpenGroup = value;
            }
        }

        [Description("用鼠标双击子项时发生")]
        [Category("子项操作")]
        public event ChatListBox.ChatListEventHandler DoubleClickSubItem;

        [Category("子项操作")]
        [Description("在鼠标进入子项中的头像时发生")]
        public event ChatListBox.ChatListEventHandler MouseEnterHead;

        [Description("在鼠标离开子项中的头像时发生")]
        [Category("子项操作")]
        public event ChatListBox.ChatListEventHandler MouseLeaveHead;

        [Description("拖动子项操作完成后发生")]
        [Category("子项操作")]
        public event ChatListBox.DragListEventHandler DragSubItemDrop;

        public ChatListBox()
        {
            this.InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.ResizeRedraw = true;
            this.Size = new Size(150, 250);
            this.iconSizeMode = ChatListItemIcon.Large;
            this.Font = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte)134);
            this.ForeColor = Color.Black;
            this.items = new ChatListItemCollection(this);
            this.chatListVScroll_0 = new ChatListVScroll((Control)this);
            this.BackColor = Color.FromArgb(50, (int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue);
        }

        protected virtual void OnDoubleClickSubItem(ChatListEventArgs e)
        {
            if (this.DoubleClickSubItem == null)
                return;
            this.DoubleClickSubItem((object)this, e);
        }

        protected virtual void OnMouseEnterHead(ChatListEventArgs e)
        {
            if (this.MouseEnterHead == null)
                return;
            this.MouseEnterHead((object)this, e);
        }

        protected virtual void OnMouseLeaveHead(ChatListEventArgs e)
        {
            if (this.MouseLeaveHead == null)
                return;
            this.MouseLeaveHead((object)this, e);
        }

        protected virtual void OnDragSubItemDrop(DragListEventArgs e)
        {
            if (this.DragSubItemDrop == null)
                return;
            this.DragSubItemDrop((object)this, e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.chatListVScroll_0.IsMouseDown = false;
            this.MouseDowns = false;
            if (e.Button == MouseButtons.Left)
            {
                int index = 0;
                for (int count = this.items.Count; index < count; ++index)
                {
                    if (this.items[index].Bounds.Contains(this.m_ptMousePos) && !this.items[index].IsOpen && (this.MouseMoveItems && this.m_mouseOnItem != this.MouseDowmSubItems.OwnerListItem))
                    {
                        ChatListSubItem QSubItem = this.MouseDowmSubItems.Clone();
                        this.MouseDowmSubItems.OwnerListItem.SubItems.Remove(this.MouseDowmSubItems);
                        this.MouseDowmSubItems.OwnerListItem.IsOpen = true;
                        this.MouseDowmSubItems.OwnerListItem = this.m_mouseOnItem;
                        this.m_mouseOnItem.SubItems.AddAccordingToStatus(this.MouseDowmSubItems);
                        this.m_mouseOnItem.IsOpen = true;
                        this.OnDragSubItemDrop(new DragListEventArgs(QSubItem, this.MouseDowmSubItems));
                    }
                }
            }
            this.MouseMoveItems = false;
            this.MouseDowmSubItems = (ChatListSubItem)null;
            base.OnMouseUp(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.Focus();
            if (this.chatListVScroll_0.SliderBounds.Contains(this.m_ptMousePos))
            {
                if (e.Button == MouseButtons.Left)
                {
                    this.chatListVScroll_0.IsMouseDown = true;
                    this.chatListVScroll_0.MouseDownY = e.Y;
                }
            }
            else
            {
                this.m_ptMousePos = e.Location;
                this.m_ptMousePos.Y += this.chatListVScroll_0.Value;
                foreach (ChatListItem chatListItem in items)
                {
                    this.selectItem = chatListItem;
                    if (chatListItem.Bounds.Contains(this.m_ptMousePos))
                    {
                        if (chatListItem.IsOpen)
                        {
                            foreach (ChatListSubItem chatListSubItem in chatListItem.SubItems)
                            {
                                if (chatListSubItem.Bounds.Contains(this.m_ptMousePos))
                                {
                                    this.selectSubItem = chatListSubItem;
                                    this.Invalidate();
                                    if (e.Button == MouseButtons.Left)
                                    {
                                        this.CursorY = Cursor.Position.Y;
                                        this.MouseDowns = true;
                                        this.MouseDowmSubItems = chatListSubItem;
                                        return;
                                    }
                                    else
                                    {
                                        if (this.ListSubItemMenu == null)
                                            return;
                                        this.ListSubItemMenu.Show(Cursor.Position);
                                        return;
                                    }
                                }
                            }
                            if (new Rectangle(0, chatListItem.Bounds.Top, this.Width, 20).Contains(this.m_ptMousePos))
                            {
                                this.selectSubItem = (ChatListSubItem)null;
                                this.Invalidate();
                                if (this.listHadOpenGroup != null && this.listHadOpenGroup.Contains(chatListItem))
                                    this.listHadOpenGroup.Remove(chatListItem);
                                if (e.Button == MouseButtons.Left)
                                {
                                    chatListItem.IsOpen = !chatListItem.IsOpen;
                                    return;
                                }
                                else
                                {
                                    if (this.SubItemMenu == null)
                                        return;
                                    this.SubItemMenu.Show(Cursor.Position);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            this.selectSubItem = (ChatListSubItem)null;
                            this.Invalidate();
                            if (e.Button == MouseButtons.Left)
                            {
                                chatListItem.IsOpen = !chatListItem.IsOpen;
                                if (this.listHadOpenGroup == null)
                                    this.listHadOpenGroup = new List<ChatListItem>();
                                this.listHadOpenGroup.Add(chatListItem);
                                return;
                            }
                            else
                            {
                                if (this.SubItemMenu == null)
                                    return;
                                this.SubItemMenu.Show(Cursor.Position);
                                return;
                            }
                        }
                    }
                    else
                        this.selectItem = (ChatListItem)null;
                }
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta > 0)
                this.chatListVScroll_0.Value -= 50;
            if (e.Delta < 0)
                this.chatListVScroll_0.Value += 50;
            base.OnMouseWheel(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.TranslateTransform(0.0f, (float)-this.chatListVScroll_0.Value);
            int width = this.chatListVScroll_0.ShouldBeDraw ? this.Width - 9 : this.Width;
            Rectangle rectItem = new Rectangle(0, 1, width, 25);
            Rectangle rectSubItem = new Rectangle(0, 26, width, (int)this.iconSizeMode);
            SolidBrush sb = new SolidBrush(this.itemColor);
            try
            {
                int index1 = 0;
                for (int count1 = this.items.Count; index1 < count1; ++index1)
                {
                    this.DrawItem(graphics, this.items[index1], rectItem, sb);
                    if (this.items[index1].IsOpen)
                    {
                        rectSubItem.Y = rectItem.Bottom + 1;
                        int index2 = 0;
                        for (int count2 = this.items[index1].SubItems.Count; index2 < count2; ++index2)
                        {
                            this.DrawSubItem(graphics, this.items[index1].SubItems[index2], ref rectSubItem, sb);
                            rectSubItem.Y = rectSubItem.Bottom + 1;
                            rectSubItem.Height = (int)this.iconSizeMode;
                        }
                        rectItem.Height = (int)(rectSubItem.Bottom - rectItem.Top - this.iconSizeMode - 1);
                    }
                    this.items[index1].Bounds = new Rectangle(rectItem.Location, rectItem.Size);
                    rectItem.Y = rectItem.Bottom + 1;
                    rectItem.Height = 25;
                }
                graphics.ResetTransform();
                this.chatListVScroll_0.VirtualHeight = rectItem.Bottom - 26;
                if (this.chatListVScroll_0.ShouldBeDraw)
                    this.chatListVScroll_0.ReDrawScroll(graphics);
            }
            finally
            {
                sb.Dispose();
            }
            base.OnPaint(e);
        }

        protected override void OnCreateControl()
        {
            new Thread((ThreadStart)(() =>
            {
                Rectangle local_0 = new Rectangle(0, 0, this.Width, this.Height);
                while (true)
                {
                    int local_1 = 0;
                    for (int local_2 = this.items.Count; local_1 < local_2; ++local_1)
                    {
                        if (this.items[local_1].IsOpen)
                        {
                            int local_3 = 0;
                            for (int local_4 = this.items[local_1].SubItems.Count; local_3 < local_4; ++local_3)
                            {
                                if (this.items[local_1].SubItems[local_3].IsTwinkle)
                                {
                                    this.items[local_1].SubItems[local_3].IsTwinkleHide = !this.items[local_1].SubItems[local_3].IsTwinkleHide;
                                    local_0.Y = this.items[local_1].SubItems[local_3].Bounds.Y - this.chatListVScroll_0.Value;
                                    local_0.Height = this.items[local_1].SubItems[local_3].Bounds.Height;
                                    this.Invalidate(local_0);
                                }
                                else if (this.items[local_1].SubItems[local_3].IsTwinkleHide)
                                {
                                    this.items[local_1].SubItems[local_3].IsTwinkleHide = false;
                                    local_0.Y = this.items[local_1].SubItems[local_3].Bounds.Y - this.chatListVScroll_0.Value;
                                    local_0.Height = this.items[local_1].SubItems[local_3].Bounds.Height;
                                    this.Invalidate(local_0);
                                }
                            }
                        }
                        else
                        {
                            local_0.Y = this.items[local_1].Bounds.Y - this.chatListVScroll_0.Value;
                            local_0.Height = this.items[local_1].Bounds.Height;
                            if (this.items[local_1].TwinkleSubItemNumber > 0)
                            {
                                this.items[local_1].IsTwinkleHide = !this.items[local_1].IsTwinkleHide;
                                this.Invalidate(local_0);
                            }
                            else if (this.items[local_1].IsTwinkleHide)
                            {
                                this.items[local_1].IsTwinkleHide = !this.items[local_1].IsTwinkleHide;
                                this.Invalidate(local_0);
                            }
                        }
                    }
                    Thread.Sleep(210);
                }
            }))
            {
                IsBackground = true
            }.Start();
            base.OnCreateControl();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            this.m_ptMousePos = e.Location;
            if (this.chatListVScroll_0.IsMouseDown)
            {
                this.chatListVScroll_0.MoveSliderFromLocation(e.Y);
            }
            else
            {
                if (this.chatListVScroll_0.ShouldBeDraw)
                {
                    if (this.chatListVScroll_0.Bounds.Contains(this.m_ptMousePos))
                    {
                        this.ClearItemMouseOn();
                        this.ClearSubItemMouseOn();
                        this.chatListVScroll_0.IsMouseOnSlider = true;
                        this.chatListVScroll_0.IsMouseOnUp = true;
                        this.chatListVScroll_0.IsMouseOnDown = true;
                        return;
                    }
                    else
                        this.chatListVScroll_0.ClearAllMouseOn();
                }
                this.m_ptMousePos.Y += this.chatListVScroll_0.Value;
                int index1 = 0;
                for (int count1 = this.items.Count; index1 < count1; ++index1)
                {
                    if (this.items[index1].Bounds.Contains(this.m_ptMousePos))
                    {
                        if (this.items[index1].IsOpen)
                        {
                            int index2 = 0;
                            for (int count2 = this.items[index1].SubItems.Count; index2 < count2; ++index2)
                            {
                                if (this.items[index1].SubItems[index2].Bounds.Contains(this.m_ptMousePos))
                                {
                                    if (this.m_mouseOnSubItem != null)
                                    {
                                        if (this.items[index1].SubItems[index2].HeadRect.Contains(this.m_ptMousePos))
                                        {
                                            if (!this.m_bOnMouseEnterHeaded)
                                            {
                                                this.OnMouseEnterHead(new ChatListEventArgs(this.m_mouseOnSubItem, this.selectSubItem));
                                                this.m_bOnMouseEnterHeaded = true;
                                            }
                                        }
                                        else if (this.m_bOnMouseEnterHeaded)
                                        {
                                            this.OnMouseLeaveHead(new ChatListEventArgs(this.m_mouseOnSubItem, this.selectSubItem));
                                            this.m_bOnMouseEnterHeaded = false;
                                        }
                                        if (this.MouseDowns && Math.Abs(this.CursorY - Cursor.Position.Y) > 4)
                                        {
                                            for (int index3 = 0; index3 < this.Items.Count; ++index3)
                                            {
                                                if (this.Items[index3].IsOpen)
                                                    this.Items[index3].IsOpen = false;
                                            }
                                            this.m_mouseOnSubItem.OwnerListItem.IsOpen = false;
                                            this.MouseMoveItems = true;
                                            Color color = Color.FromArgb(250, (int)this.SubItemSelectColor.R, (int)this.SubItemSelectColor.G, (int)this.SubItemSelectColor.B);
                                            string displayName = this.m_mouseOnSubItem.DisplayName;
                                            Size size = TextRenderer.MeasureText(displayName, this.Font);
                                            int num1 = 45 + size.Width + 10;
                                            int num2 = 45;
                                            Bitmap bitmap = new Bitmap(num1 * 2, 45 * 2);
                                            Graphics graphics = Graphics.FromImage((Image)bitmap);
                                            graphics.FillRectangle((Brush)new SolidBrush(color), num1, 45, num1, 45);
                                            graphics.DrawImage(this.m_mouseOnSubItem.HeadImage, num1, 45, 45, 45);
                                            Brush brush = Brushes.Black;
                                            if (this.m_mouseOnSubItem.IsVip)
                                                brush = (Brush)new SolidBrush(this.VipFontColor);
                                            if (size.Width > 0)
                                                graphics.DrawString(displayName, this.Font, brush, (float)(num1 + num2 + 5), (float)(num2 + (num2 - size.Height) / 2));
                                            else
                                                graphics.DrawString(this.m_mouseOnSubItem.NicName, this.Font, brush, (float)(num1 + num2 + 5), (float)(num2 + (num2 - size.Height) / 2));
                                            Cursor.Current = new Cursor(bitmap.GetHicon());
                                        }
                                    }
                                    if (this.items[index1].SubItems[index2].Equals((object)this.m_mouseOnSubItem))
                                        return;
                                    this.ClearSubItemMouseOn();
                                    this.ClearItemMouseOn();
                                    this.m_mouseOnSubItem = this.items[index1].SubItems[index2];
                                    this.Invalidate(new Rectangle(this.m_mouseOnSubItem.Bounds.X, this.m_mouseOnSubItem.Bounds.Y - this.chatListVScroll_0.Value, this.m_mouseOnSubItem.Bounds.Width, this.m_mouseOnSubItem.Bounds.Height));
                                    return;
                                }
                            }
                            this.ClearSubItemMouseOn();
                            if (new Rectangle(0, this.items[index1].Bounds.Top - this.chatListVScroll_0.Value, this.Width, 20).Contains(e.Location))
                            {
                                if (this.items[index1].Equals((object)this.m_mouseOnItem))
                                    return;
                                this.ClearItemMouseOn();
                                this.m_mouseOnItem = this.items[index1];
                                this.Invalidate(new Rectangle(this.m_mouseOnItem.Bounds.X, this.m_mouseOnItem.Bounds.Y - this.chatListVScroll_0.Value, this.m_mouseOnItem.Bounds.Width, this.m_mouseOnItem.Bounds.Height));
                                return;
                            }
                        }
                        else
                        {
                            if (this.items[index1].Equals((object)this.m_mouseOnItem))
                                return;
                            this.ClearItemMouseOn();
                            this.ClearSubItemMouseOn();
                            this.m_mouseOnItem = this.items[index1];
                            this.Invalidate(new Rectangle(this.m_mouseOnItem.Bounds.X, this.m_mouseOnItem.Bounds.Y - this.chatListVScroll_0.Value, this.m_mouseOnItem.Bounds.Width, this.m_mouseOnItem.Bounds.Height));
                            return;
                        }
                    }
                }
                this.ClearItemMouseOn();
                this.ClearSubItemMouseOn();
                base.OnMouseMove(e);
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this.ClearItemMouseOn();
            this.ClearSubItemMouseOn();
            this.chatListVScroll_0.ClearAllMouseOn();
            if (this.m_bOnMouseEnterHeaded)
            {
                this.OnMouseLeaveHead(new ChatListEventArgs(this.m_mouseOnSubItem, this.selectSubItem));
                this.m_bOnMouseEnterHeaded = false;
            }
            base.OnMouseLeave(e);
        }

        protected override void OnClick(EventArgs e)
        {
            if (this.chatListVScroll_0.IsMouseDown)
                return;
            if (this.chatListVScroll_0.ShouldBeDraw && this.chatListVScroll_0.Bounds.Contains(this.m_ptMousePos))
            {
                if (this.chatListVScroll_0.UpBounds.Contains(this.m_ptMousePos))
                    this.chatListVScroll_0.Value -= 50;
                else if (this.chatListVScroll_0.DownBounds.Contains(this.m_ptMousePos))
                {
                    this.chatListVScroll_0.Value += 50;
                }
                else
                {
                    if (this.chatListVScroll_0.SliderBounds.Contains(this.m_ptMousePos))
                        return;
                    this.chatListVScroll_0.MoveSliderToLocation(this.m_ptMousePos.Y);
                }
            }
            else
                base.OnClick(e);
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            this.OnClick(e);
            if (this.chatListVScroll_0.Bounds.Contains(this.PointToClient(Control.MousePosition)))
                return;
            if (this.selectSubItem != null)
                this.OnDoubleClickSubItem(new ChatListEventArgs(this.m_mouseOnSubItem, this.selectSubItem));
            base.OnDoubleClick(e);
        }

        protected virtual void DrawItem(Graphics g, ChatListItem item, Rectangle rectItem, SolidBrush sb)
        {
            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.SetTabStops(0.0f, new float[1]
      {
        20f
      });
            sb.Color = !item.Equals((object)this.m_mouseOnItem) ? this.itemColor : this.itemMouseOnColor;
            g.FillRectangle((Brush)sb, rectItem);
            if (item.IsOpen)
            {
                sb.Color = this.arrowColor;
                g.FillPolygon((Brush)sb, new Point[3]
        {
          new Point(2, rectItem.Top + 11),
          new Point(12, rectItem.Top + 11),
          new Point(7, rectItem.Top + 16)
        });
            }
            else
            {
                sb.Color = this.arrowColor;
                g.FillPolygon((Brush)sb, new Point[3]
        {
          new Point(5, rectItem.Top + 8),
          new Point(5, rectItem.Top + 18),
          new Point(10, rectItem.Top + 13)
        });
                if (item.TwinkleSubItemNumber > 0 && item.IsTwinkleHide)
                    return;
            }
            string s = "\t" + item.Text;
            sb.Color = this.ForeColor;
            format.Alignment = StringAlignment.Near;
            g.DrawString(s, this.Font, (Brush)sb, (RectangleF)rectItem, format);
            Size size = TextRenderer.MeasureText(item.Text, this.Font);
            format.Alignment = StringAlignment.Near;
            g.DrawString("[" + item.SubItems.GetOnLineNumber() + "/" + item.SubItems.Count + "]", this.Font, (Brush)sb, (RectangleF)new Rectangle(rectItem.X + Convert.ToInt32(size.Width) + 25, rectItem.Y, rectItem.Width - 15, rectItem.Height), format);
        }

        protected virtual void DrawSubItem(Graphics g, ChatListSubItem subItem, ref Rectangle rectSubItem, SolidBrush sb)
        {
            if (subItem.Equals((object)this.selectSubItem))
            {
                rectSubItem.Height = 53;
                sb.Color = this.subItemSelectColor;
                g.FillRectangle((Brush)sb, rectSubItem);
                this.DrawHeadImage(g, subItem, rectSubItem);
                this.DrawLargeSubItem(g, subItem, rectSubItem);
                subItem.Bounds = new Rectangle(rectSubItem.Location, rectSubItem.Size);
            }
            else
            {
                sb.Color = !subItem.Equals((object)this.m_mouseOnSubItem) ? this.subItemColor : this.subItemMouseOnColor;
                g.FillRectangle((Brush)sb, rectSubItem);
                this.DrawHeadImage(g, subItem, rectSubItem);
                if (this.iconSizeMode == ChatListItemIcon.Large)
                    this.DrawLargeSubItem(g, subItem, rectSubItem);
                else
                    this.DrawSmallSubItem(g, subItem, rectSubItem);
                subItem.Bounds = new Rectangle(rectSubItem.Location, rectSubItem.Size);
            }
        }

        protected virtual void DrawHeadImage(Graphics g, ChatListSubItem subItem, Rectangle rectSubItem)
        {
            if (!subItem.IsTwinkle || !subItem.IsTwinkleHide)
            {
                int width = (rectSubItem.Height == 0x35) ? 40 : 20;
                subItem.HeadRect = new Rectangle(5, rectSubItem.Top + ((rectSubItem.Height - width) / 2), width, width);
                if (subItem.HeadImage == null)
                {
                    subItem.HeadImage = GetDefaultResource.GetImage("Common.no_photo.png");
                }
                if (subItem.Status == ChatListSubItem.UserStatus.OffLine)
                {
                    g.DrawImage(subItem.GetDarkImage(), subItem.HeadRect);
                }
                else
                {
                    g.DrawImage(subItem.HeadImage, subItem.HeadRect);
                    if (this.IconSizeMode == ChatListItemIcon.Small)
                    {
                        if (subItem.PlatformTypes == PlatformType.PC)
                        {
                            if (subItem.Status == ChatListSubItem.UserStatus.QMe)
                            {
                                g.DrawImage(GetDefaultResource.GetImage("Icons.Small_Qme.png"), new Rectangle(subItem.HeadRect.Right - 9, subItem.HeadRect.Bottom - 9, 9, 9));
                            }
                            if (subItem.Status == ChatListSubItem.UserStatus.Away)
                            {
                                g.DrawImage(GetDefaultResource.GetImage("Icons.Small_away.png"), new Rectangle(subItem.HeadRect.Right - 9, subItem.HeadRect.Bottom - 9, 9, 9));
                            }
                            if (subItem.Status == ChatListSubItem.UserStatus.Busy)
                            {
                                g.DrawImage(GetDefaultResource.GetImage("Icons.Small_busy.png"), new Rectangle(subItem.HeadRect.Right - 9, subItem.HeadRect.Bottom - 9, 9, 9));
                            }
                            if (subItem.Status == ChatListSubItem.UserStatus.DontDisturb)
                            {
                                g.DrawImage(GetDefaultResource.GetImage("Icons.Small_mute.png"), new Rectangle(subItem.HeadRect.Right - 9, subItem.HeadRect.Bottom - 9, 9, 9));
                            }
                        }

                    }
                    else if (subItem.PlatformTypes == PlatformType.PC)
                    {
                        int i = 1;
                    }

                }
                if (subItem.Equals(this.selectSubItem))
                {
                    g.DrawImage(GetDefaultResource.GetImage("Common.main_panel.png"), subItem.HeadRect.X - 3, subItem.HeadRect.Y - 3, 0x2e, 0x2e);
                }
                else
                {
                    Pen pen = new Pen(Color.FromArgb(200, 0xff, 0xff, 0xff));
                    g.DrawRectangle(pen, subItem.HeadRect);
                }
            }

        }

        protected virtual void DrawLargeSubItem(Graphics g, ChatListSubItem subItem, Rectangle rectSubItem)
        {
            rectSubItem.Height = 53;
            string displayName = subItem.DisplayName;
            Size size1 = TextRenderer.MeasureText(displayName, this.Font);
            TextRenderer.MeasureText(subItem.NicName, this.Font);
            StringFormat format = new StringFormat(StringFormatFlags.NoWrap);
            format.Trimming = StringTrimming.Word;
            Rectangle rectangle1 = new Rectangle(new Point(rectSubItem.Height, rectSubItem.Top + 8), new Size(this.Width - 9 - rectSubItem.Height, size1.Height));
            Rectangle rectangle2 = new Rectangle(new Point(rectSubItem.Height + size1.Width, rectSubItem.Top + 8), new Size(this.Width - 9 - rectSubItem.Height - size1.Width, size1.Height));
            SolidBrush solidBrush = new SolidBrush(this.ForeColor);
            if (subItem.IsVip)
                solidBrush = new SolidBrush(this.VipFontColor);
            if (size1.Width > 0)
            {
                g.DrawString(displayName, this.Font, (Brush)solidBrush, (RectangleF)rectangle1, format);
                g.DrawString("(" + subItem.NicName + ")", this.Font, Brushes.Gray, (RectangleF)rectangle2, format);
            }
            else
            {
                Rectangle rectangle3 = new Rectangle(new Point(rectSubItem.Height, rectSubItem.Top + 8), new Size(this.Width - 9 - rectSubItem.Height, size1.Height));
                g.DrawString(subItem.NicName, this.Font, (Brush)solidBrush, (RectangleF)rectangle3, format);
            }
            Size size2 = TextRenderer.MeasureText(subItem.PersonalMsg, this.Font);
            Rectangle rectangle4 = new Rectangle(new Point(rectSubItem.Height, rectSubItem.Top + 11 + this.Font.Height), new Size(this.Width - rectSubItem.Height, size2.Height));
            g.DrawString(subItem.PersonalMsg, this.Font, Brushes.Gray, (RectangleF)rectangle4, format);
        }

        protected virtual void DrawSmallSubItem(Graphics g, ChatListSubItem subItem, Rectangle rectSubItem)
        {
            rectSubItem.Height = 27;
            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.FormatFlags = StringFormatFlags.NoWrap;
            string text = subItem.DisplayName;
            SolidBrush solidBrush = new SolidBrush(this.ForeColor);
            if (subItem.IsVip)
                solidBrush = new SolidBrush(this.VipFontColor);
            if (string.IsNullOrEmpty(text))
                text = subItem.NicName;
            Size size = TextRenderer.MeasureText(text, this.Font);
            format.SetTabStops(0.0f, new float[1]
      {
        (float) rectSubItem.Height
      });
            g.DrawString("\t" + text, this.Font, (Brush)solidBrush, (RectangleF)rectSubItem, format);
            format.SetTabStops(0.0f, new float[1]
      {
        (float) (rectSubItem.Height + 5 + size.Width)
      });
            g.DrawString("\t" + subItem.PersonalMsg, this.Font, Brushes.Gray, (RectangleF)rectSubItem, format);
        }

        private void ClearItemMouseOn()
        {
            if (this.m_mouseOnItem == null)
                return;
            int num = 0;
            if (this.chatListVScroll_0.ShouldBeDraw)
                num = 9;
            this.Invalidate(new Rectangle(this.m_mouseOnItem.Bounds.X, this.m_mouseOnItem.Bounds.Y - this.chatListVScroll_0.Value, this.m_mouseOnItem.Bounds.Width + num, this.m_mouseOnItem.Bounds.Height));
            this.m_mouseOnItem = (ChatListItem)null;
        }

        private void ClearSubItemMouseOn()
        {
            if (this.m_mouseOnSubItem == null)
                return;
            int num = 0;
            if (this.chatListVScroll_0.ShouldBeDraw)
                num = 9;
            this.Invalidate(new Rectangle(this.m_mouseOnSubItem.Bounds.X, this.m_mouseOnSubItem.Bounds.Y - this.chatListVScroll_0.Value, this.m_mouseOnSubItem.Bounds.Width + num, this.m_mouseOnSubItem.Bounds.Height));
            this.m_mouseOnSubItem = (ChatListSubItem)null;
        }

        public ChatListSubItem GetSubItemsById(int userId)
        {
            ChatListSubItem chatListSubItem = (ChatListSubItem)null;
            int index1 = 0;
            for (int count1 = this.items.Count; index1 < count1; ++index1)
            {
                int index2 = 0;
                for (int count2 = this.items[index1].SubItems.Count; index2 < count2; ++index2)
                {
                    if ((long)userId == (long)this.items[index1].SubItems[index2].ID)
                    {
                        chatListSubItem = this.items[index1].SubItems[index2];
                        break;
                    }
                }
            }
            return chatListSubItem;
        }

        public ChatListSubItem[] GetSubItemsByNicName(string nicName)
        {
            List<ChatListSubItem> list = new List<ChatListSubItem>();
            int index1 = 0;
            for (int count1 = this.items.Count; index1 < count1; ++index1)
            {
                int index2 = 0;
                for (int count2 = this.items[index1].SubItems.Count; index2 < count2; ++index2)
                {
                    if (nicName == this.items[index1].SubItems[index2].NicName)
                        list.Add(this.items[index1].SubItems[index2]);
                }
            }
            return list.ToArray();
        }

        public ChatListSubItem[] GetSubItemsByDisplayName(string displayName)
        {
            List<ChatListSubItem> list = new List<ChatListSubItem>();
            int index1 = 0;
            for (int count1 = this.items.Count; index1 < count1; ++index1)
            {
                int index2 = 0;
                for (int count2 = this.items[index1].SubItems.Count; index2 < count2; ++index2)
                {
                    if (displayName == this.items[index1].SubItems[index2].DisplayName)
                        list.Add(this.items[index1].SubItems[index2]);
                }
            }
            return list.ToArray();
        }

        public ChatListSubItem[] GetSubItemsByIp(string Ip)
        {
            List<ChatListSubItem> list = new List<ChatListSubItem>();
            int index1 = 0;
            for (int count1 = this.items.Count; index1 < count1; ++index1)
            {
                int index2 = 0;
                for (int count2 = this.items[index1].SubItems.Count; index2 < count2; ++index2)
                {
                    if (Ip == this.items[index1].SubItems[index2].IpAddress)
                        list.Add(this.items[index1].SubItems[index2]);
                }
            }
            return list.ToArray();
        }

        public ChatListSubItem[] GetSubItemsByText(string text)
        {
            List<ChatListSubItem> list = new List<ChatListSubItem>();
            int index1 = 0;
            for (int count1 = this.items.Count; index1 < count1; ++index1)
            {
                int index2 = 0;
                for (int count2 = this.items[index1].SubItems.Count; index2 < count2; ++index2)
                {
                    if (this.items[index1].SubItems[index2].NicName != null && this.items[index1].SubItems[index2].NicName.ToLower().Contains(text))
                        list.Add(this.items[index1].SubItems[index2]);
                    else if (this.items[index1].SubItems[index2].DisplayName != null && this.items[index1].SubItems[index2].DisplayName.ToLower().Contains(text))
                        list.Add(this.items[index1].SubItems[index2]);
                }
            }
            return list.ToArray();
        }

        public void CollapseAll()
        {
            foreach (ChatListItem chatListItem in this.items)
                chatListItem.IsOpen = false;
            this.Invalidate();
        }

        public void ExpandAll()
        {
            foreach (ChatListItem chatListItem in this.items)
                chatListItem.IsOpen = true;
            this.Invalidate();
        }

        public void Regain()
        {
            if (this.listHadOpenGroup == null || this.listHadOpenGroup.Count <= 0)
                return;
            foreach (ChatListItem chatListItem in this.listHadOpenGroup)
                chatListItem.IsOpen = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);
        }

        //[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        //public class UC_SmartTagChatListBoxDesigner : ControlDesigner
        //{
        //  private DesignerActionListCollection actionLists;

        //  public override DesignerActionListCollection ActionLists
        //  {
        //    get
        //    {
        //      if (this.actionLists == null)
        //      {
        //        this.actionLists = new DesignerActionListCollection();
        //        this.actionLists.Add((DesignerActionList) new ChatListBox.UC_SmartTagSupportActionList(this.Component));
        //      }
        //      return this.actionLists;
        //    }
        //  }
        //}

        //public class UC_SmartTagSupportActionList : DesignerActionList
        //{
        //  private ChatListBox control;
        //  private DesignerActionUIService designerActionUISvc;

        //  public ChatListItemCollection Items
        //  {
        //    get
        //    {
        //      return this.control.Items;
        //    }
        //    set
        //    {
        //      this.GetPropertyByName("列表中的项").SetValue((object) this.control, (object) value);
        //    }
        //  }

        //  public UC_SmartTagSupportActionList(IComponent component)
        //    : base(component)
        //  {
        //    this.control = component as ChatListBox;
        //    this.designerActionUISvc = this.GetService(typeof (DesignerActionUIService)) as DesignerActionUIService;
        //  }

        //  private PropertyDescriptor GetPropertyByName(string propName)
        //  {
        //    PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties((object) this.control)[propName];
        //    if (propertyDescriptor == null)
        //      throw new ArgumentException("Matching ColorLabel property not found!", propName);
        //    else
        //      return propertyDescriptor;
        //  }

        //  public override DesignerActionItemCollection GetSortedActionItems()
        //  {
        //    DesignerActionItemCollection actionItemCollection = new DesignerActionItemCollection();
        //    actionItemCollection.Add((DesignerActionItem) new DesignerActionHeaderItem("Skin"));
        //    actionItemCollection.Add((DesignerActionItem) new DesignerActionPropertyItem("Items", "列表中的项", "Skin", "列表中的项，包括分组和好友"));
        //    StringBuilder stringBuilder1 = new StringBuilder("位置: ");
        //    stringBuilder1.Append((object) this.control.Location);
        //    StringBuilder stringBuilder2 = new StringBuilder("大小: ");
        //    stringBuilder2.Append((object) this.control.Size);
        //    actionItemCollection.Add((DesignerActionItem) new DesignerActionTextItem(((object) stringBuilder1).ToString(), "Information"));
        //    actionItemCollection.Add((DesignerActionItem) new DesignerActionTextItem(((object) stringBuilder2).ToString(), "Information"));
        //    return actionItemCollection;
        //  }
        //}

        public delegate void ChatListEventHandler(object sender, ChatListEventArgs e);

        public delegate void DragListEventHandler(object sender, DragListEventArgs e);
    }

    public class DragListEventArgs
    {
        private ChatListSubItem qsubitem;
        private ChatListSubItem hsubitem;

        public ChatListSubItem QSubItem
        {
            get
            {
                return this.qsubitem;
            }
        }

        public ChatListSubItem HSubItem
        {
            get
            {
                return this.hsubitem;
            }
        }

        public DragListEventArgs(ChatListSubItem QSubItem, ChatListSubItem HSubItem)
        {
            this.qsubitem = QSubItem;
            this.hsubitem = HSubItem;
        }
    }

    public class ChatListEventArgs
    {
        private ChatListSubItem mouseOnSubItem;
        private ChatListSubItem selectSubItem;

        public ChatListSubItem MouseOnSubItem
        {
            get
            {
                return this.mouseOnSubItem;
            }
        }

        public ChatListSubItem SelectSubItem
        {
            get
            {
                return this.selectSubItem;
            }
        }

        public ChatListEventArgs(ChatListSubItem mouseonsubitem, ChatListSubItem selectsubitem)
        {
            this.mouseOnSubItem = mouseonsubitem;
            this.selectSubItem = selectsubitem;
        }
    }
}
