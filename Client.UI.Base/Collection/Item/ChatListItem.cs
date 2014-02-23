using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Client.UI.Base.Controls;
using System.ComponentModel;
using System.Collections;

namespace Client.UI.Base.Collection.Item
{
    public class ChatListItem
    {
        private string text = "Item";
        private bool isOpen;
        private int twinkleSubItemNumber;
        private bool isTwinkleHide;
        private Rectangle bounds;
        private ChatListBox ownerChatListBox;
        private ChatListItem.ChatListSubItemCollection subItems;

        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
                if (this.ownerChatListBox == null)
                    return;
                this.ownerChatListBox.Invalidate(this.bounds);
            }
        }

        [DefaultValue(false)]
        public bool IsOpen
        {
            get
            {
                return this.isOpen;
            }
            set
            {
                this.isOpen = value;
                if (this.ownerChatListBox == null)
                    return;
                this.ownerChatListBox.Invalidate();
            }
        }

        [Browsable(false)]
        public int TwinkleSubItemNumber
        {
            get
            {
                return this.twinkleSubItemNumber;
            }
            set
            {
                this.twinkleSubItemNumber = value;
            }
        }

        public bool IsTwinkleHide
        {
            get
            {
                return this.isTwinkleHide;
            }
            set
            {
                this.isTwinkleHide = value;
            }
        }

        [Browsable(false)]
        public Rectangle Bounds
        {
            get
            {
                return this.bounds;
            }
            set
            {
                this.bounds = value;
            }
        }

        [Browsable(false)]
        public ChatListBox OwnerChatListBox
        {
            get
            {
                return this.ownerChatListBox;
            }
            set
            {
                this.ownerChatListBox = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChatListItem.ChatListSubItemCollection SubItems
        {
            get
            {
                if (this.subItems == null)
                    this.subItems = new ChatListItem.ChatListSubItemCollection(this);
                return this.subItems;
            }
            set
            {
                if (this.subItems == value)
                    return;
                this.subItems = value;
            }
        }

        public ChatListItem()
        {
            if (this.text != null)
                return;
            this.text = string.Empty;
        }

        public ChatListItem(string text)
        {
            this.text = text;
        }

        public ChatListItem(string text, bool bOpen)
        {
            this.text = text;
            this.isOpen = bOpen;
        }

        public ChatListItem(ChatListSubItem[] subItems)
        {
            this.subItems.AddRange(subItems);
        }

        public ChatListItem(string text, ChatListSubItem[] subItems)
        {
            this.text = text;
            this.subItems.AddRange(subItems);
        }

        public ChatListItem(string text, bool bOpen, ChatListSubItem[] subItems)
        {
            this.text = text;
            this.isOpen = bOpen;
            this.subItems.AddRange(subItems);
        }

        public ChatListItem Clone()
        {
            return new ChatListItem()
            {
                Bounds = this.Bounds,
                IsOpen = this.IsOpen,
                IsTwinkleHide = this.IsTwinkleHide,
                OwnerChatListBox = this.OwnerChatListBox,
                SubItems = this.SubItems,
                Text = this.text,
                TwinkleSubItemNumber = this.TwinkleSubItemNumber
            };
        }

        public class ChatListSubItemCollection : IList, ICollection, IEnumerable
        {
            private int count;
            private ChatListSubItem[] m_arrSubItems;
            private ChatListItem owner;

            public ChatListSubItemCollection(ChatListItem owner)
            {
                this.owner = owner;
            }

            public void Add(ChatListSubItem subItem)
            {
                if (subItem == null)
                {
                    throw new ArgumentNullException("SubItem cannot be null");
                }
                this.EnsureSpace(1);
                if (-1 == this.IndexOf(subItem))
                {
                    subItem.OwnerListItem = this.owner;
                    this.m_arrSubItems[this.count++] = subItem;
                    if (this.owner.OwnerChatListBox != null)
                    {
                        this.owner.OwnerChatListBox.Invalidate();
                    }
                }
            }

            public void AddAccordingToStatus(ChatListSubItem subItem)
            {
                if (subItem.Status == ChatListSubItem.UserStatus.OffLine)
                {
                    this.Add(subItem);
                }
                else
                {
                    int index = 0;
                    int count = this.count;
                    while (index < count)
                    {
                        if (subItem.Status <= this.m_arrSubItems[index].Status)
                        {
                            this.Insert(index, subItem);
                            return;
                        }
                        index++;
                    }
                    this.Add(subItem);
                }
            }

            public void AddRange(ChatListSubItem[] subItems)
            {
                if (subItems == null)
                {
                    throw new ArgumentNullException("SubItems cannot be null");
                }
                this.EnsureSpace(subItems.Length);
                try
                {
                    foreach (ChatListSubItem item in subItems)
                    {
                        if (item == null)
                        {
                            throw new ArgumentNullException("SubItem cannot be null");
                        }
                        if (-1 == this.IndexOf(item))
                        {
                            item.OwnerListItem = this.owner;
                            this.m_arrSubItems[this.count++] = item;
                        }
                    }
                }
                finally
                {
                    if (this.owner.OwnerChatListBox != null)
                    {
                        this.owner.OwnerChatListBox.Invalidate();
                    }
                }
            }

            public void Clear()
            {
                this.count = 0;
                this.m_arrSubItems = null;
                if (this.owner.OwnerChatListBox != null)
                {
                    this.owner.OwnerChatListBox.Invalidate();
                }
            }

            public bool Contains(ChatListSubItem subItem)
            {
                return (this.IndexOf(subItem) != -1);
            }

            public void CopyTo(Array array, int index)
            {
                if (array == null)
                {
                    throw new ArgumentNullException("Array cannot be null");
                }
                this.m_arrSubItems.CopyTo(array, index);
            }

            private void EnsureSpace(int elements)
            {
                if (this.m_arrSubItems == null)
                {
                    this.m_arrSubItems = new ChatListSubItem[Math.Max(elements, 4)];
                }
                else if ((elements + this.count) > this.m_arrSubItems.Length)
                {
                    ChatListSubItem[] array = new ChatListSubItem[Math.Max((int)(this.m_arrSubItems.Length * 2), (int)(elements + this.count))];
                    this.m_arrSubItems.CopyTo(array, 0);
                    this.m_arrSubItems = array;
                }
            }

            public int GetOnLineNumber()
            {
                int num = 0;
                int index = 0;
                int count = this.count;
                while (index < count)
                {
                    if (this.m_arrSubItems[index].Status != ChatListSubItem.UserStatus.OffLine)
                    {
                        num++;
                    }
                    index++;
                }
                return num;
            }

            public int IndexOf(ChatListSubItem subItem)
            {
                return Array.IndexOf<ChatListSubItem>(this.m_arrSubItems, subItem);
            }

            public void Insert(int index, ChatListSubItem subItem)
            {
                if ((index < 0) || (index > this.count))
                {
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array");
                }
                if (subItem == null)
                {
                    throw new ArgumentNullException("SubItem cannot be null");
                }
                this.EnsureSpace(1);
                for (int i = this.count; i > index; i--)
                {
                    this.m_arrSubItems[i] = this.m_arrSubItems[i - 1];
                }
                subItem.OwnerListItem = this.owner;
                this.m_arrSubItems[index] = subItem;
                this.count++;
                if (this.owner.OwnerChatListBox != null)
                {
                    this.owner.OwnerChatListBox.Invalidate();
                }
            }

            public void Remove(ChatListSubItem subItem)
            {
                int index = this.IndexOf(subItem);
                if (-1 != index)
                {
                    this.RemoveAt(index);
                }
            }

            public void RemoveAt(int index)
            {
                if ((index < 0) || (index > this.count))
                {
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array");
                }
                this.count--;
                int num = index;
                int count = this.count;
                while (num < count)
                {
                    this.m_arrSubItems[num] = this.m_arrSubItems[num + 1];
                    num++;
                }
                if (this.owner.OwnerChatListBox != null)
                {
                    this.owner.OwnerChatListBox.Invalidate();
                }
            }

            public void Sort()
            {
                Array.Sort<ChatListSubItem>(this.m_arrSubItems, 0, this.count, null);
                if (this.owner.ownerChatListBox != null)
                {
                    this.owner.ownerChatListBox.Invalidate(this.owner.bounds);
                }
            }

            void ICollection.CopyTo(Array array, int index)
            {
                this.CopyTo(array, index);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                int index = 0;
                int count = this.count;
                while (true)
                {
                    if (index >= count)
                    {
                        yield break;
                    }
                    yield return this.m_arrSubItems[index];
                    index++;
                }
            }

            int IList.Add(object value)
            {
                if (!(value is ChatListSubItem))
                {
                    throw new ArgumentException("Value cannot convert to ListSubItem");
                }
                this.Add((ChatListSubItem)value);
                return this.IndexOf((ChatListSubItem)value);
            }

            void IList.Clear()
            {
                this.Clear();
            }

            bool IList.Contains(object value)
            {
                if (!(value is ChatListSubItem))
                {
                    throw new ArgumentException("Value cannot convert to ListSubItem");
                }
                return this.Contains((ChatListSubItem)value);
            }

            int IList.IndexOf(object value)
            {
                if (!(value is ChatListSubItem))
                {
                    throw new ArgumentException("Value cannot convert to ListSubItem");
                }
                return this.IndexOf((ChatListSubItem)value);
            }

            void IList.Insert(int index, object value)
            {
                if (!(value is ChatListSubItem))
                {
                    throw new ArgumentException("Value cannot convert to ListSubItem");
                }
                this.Insert(index, (ChatListSubItem)value);
            }

            void IList.Remove(object value)
            {
                if (!(value is ChatListSubItem))
                {
                    throw new ArgumentException("Value cannot convert to ListSubItem");
                }
                this.Remove((ChatListSubItem)value);
            }

            void IList.RemoveAt(int index)
            {
                this.RemoveAt(index);
            }

            public int Count
            {
                get
                {
                    return this.count;
                }
            }

            public ChatListSubItem this[int index]
            {
                get
                {
                    if ((index < 0) || (index > this.count))
                    {
                        throw new IndexOutOfRangeException("Index was outside the bounds of the array");
                    }
                    return this.m_arrSubItems[index];
                }
                set
                {
                    if ((index < 0) || (index > this.count))
                    {
                        throw new IndexOutOfRangeException("Index was outside the bounds of the array");
                    }
                    this.m_arrSubItems[index] = value;
                    if (this.owner.OwnerChatListBox != null)
                    {
                        this.owner.OwnerChatListBox.Invalidate(this.m_arrSubItems[index].Bounds);
                    }
                }
            }

            int ICollection.Count
            {
                get
                {
                    return this.count;
                }
            }

            bool ICollection.IsSynchronized
            {
                get
                {
                    return true;
                }
            }

            object ICollection.SyncRoot
            {
                get
                {
                    return this;
                }
            }

            bool IList.IsFixedSize
            {
                get
                {
                    return false;
                }
            }

            bool IList.IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            object IList.this[int index]
            {
                get
                {
                    return this[index];
                }
                set
                {
                    if (!(value is ChatListSubItem))
                    {
                        throw new ArgumentException("Value cannot convert to ListSubItem");
                    }
                    this[index] = (ChatListSubItem)value;
                }
            }

            //[CompilerGenerated]
            //private sealed class GetEnumerator>d__0 : IEnumerator<object>, IEnumerator, IDisposable
            //{
            //    private int <>1__state;
            //    private object <>2__current;
            //    public ChatListItem.ChatListSubItemCollection <>4__this;
            //    public int <i>5__1;
            //    public int <Len>5__2;

            //    [DebuggerHidden]
            //    public GetEnumerator>d__0(int <>1__state)
            //    {
            //        this.<>1__state = <>1__state;
            //    }

            //private bool MoveNext()
            //{
            //    switch (this.<>1__state)
            //    {
            //        case 0:
            //            this.<>1__state = -1;
            //            this.<i>5__1 = 0;
            //            this.<Len>5__2 = this.<>4__this.count;
            //            break;

            //        case 1:
            //            this.<>1__state = -1;
            //            this.<i>5__1++;
            //            break;

            //        default:
            //            goto Label_007C;
            //    }
            //    if (this.<i>5__1 < this.<Len>5__2)
            //    {
            //        this.<>2__current = this.<>4__this.m_arrSubItems[this.<i>5__1];
            //        this.<>1__state = 1;
            //        return true;
            //    }
            //Label_007C:
            //    return false;
            //}

            //[DebuggerHidden]
            //void IEnumerator.Reset()
            //{
            //    throw new NotSupportedException();
            //}

            //void IDisposable.Dispose()
            //{
            //}

            //object IEnumerator<object>.Current
            //{
            //    [DebuggerHidden]
            //    get
            //    {
            //        return this.<>2__current;
            //    }
            //}

            //object IEnumerator.Current
            //{
            //    [DebuggerHidden]
            //    get
            //    {
            //        return this.<>2__current;
            //    }
            //}
            //}
        }
    }
}
