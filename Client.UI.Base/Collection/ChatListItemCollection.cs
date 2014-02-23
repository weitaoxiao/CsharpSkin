using System;
using System.Collections.Generic;
using System.Text;
using Client.UI.Base.Collection.Item;
using Client.UI.Base.Controls;
using System.Collections;

namespace Client.UI.Base.Collection
{
    public class ChatListItemCollection : IList, ICollection, IEnumerable
    {
        private int count;
        private ChatListItem[] m_arrItem;
        private ChatListBox owner;

        public int Count
        {
            get
            {
                return this.count;
            }
        }

        public ChatListItem this[int index]
        {
            get
            {
                if (index < 0 || index >= this.count)
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array");
                else
                    return this.m_arrItem[index];
            }
            set
            {
                if (index < 0 || index >= this.count)
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array");
                this.m_arrItem[index] = value;
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
                return (object)this;
            }
        }

        public ChatListItemCollection(ChatListBox owner)
        {
            this.owner = owner;
        }

        private void EnsureSpace(int elements)
        {
            if (this.m_arrItem == null)
            {
                this.m_arrItem = new ChatListItem[Math.Max(elements, 4)];
            }
            else
            {
                if (this.count + elements <= this.m_arrItem.Length)
                    return;
                ChatListItem[] chatListItemArray = new ChatListItem[Math.Max(this.count + elements, this.m_arrItem.Length * 2)];
                this.m_arrItem.CopyTo((Array)chatListItemArray, 0);
                this.m_arrItem = chatListItemArray;
            }
        }

        public int IndexOf(ChatListItem item)
        {
            return Array.IndexOf<ChatListItem>(this.m_arrItem, item);
        }

        public void Add(ChatListItem item)
        {
            if (item == null)
                throw new ArgumentNullException("Item cannot be null");
            this.EnsureSpace(1);
            if (-1 != this.IndexOf(item))
                return;
            item.OwnerChatListBox = this.owner;
            this.m_arrItem[this.count++] = item;
            this.owner.Invalidate();
        }

        public void AddRange(ChatListItem[] items)
        {
            if (items == null)
                throw new ArgumentNullException("Items cannot be null");
            this.EnsureSpace(items.Length);
            try
            {
                foreach (ChatListItem chatListItem in items)
                {
                    if (chatListItem == null)
                        throw new ArgumentNullException("Item cannot be null");
                    if (-1 == this.IndexOf(chatListItem))
                    {
                        chatListItem.OwnerChatListBox = this.owner;
                        this.m_arrItem[this.count++] = chatListItem;
                    }
                }
            }
            finally
            {
                this.owner.Invalidate();
            }
        }

        public void Remove(ChatListItem item)
        {
            int index = this.IndexOf(item);
            if (-1 == index)
                return;
            this.RemoveAt(index);
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= this.count)
                throw new IndexOutOfRangeException("Index was outside the bounds of the array");
            --this.count;
            int index1 = index;
            for (int index2 = this.count; index1 < index2; ++index1)
                this.m_arrItem[index1] = this.m_arrItem[index1 + 1];
            this.owner.Invalidate();
        }

        public void Clear()
        {
            this.count = 0;
            this.m_arrItem = (ChatListItem[])null;
            this.owner.Invalidate();
        }

        public void Insert(int index, ChatListItem item)
        {
            if (index < 0 || index >= this.count)
                throw new IndexOutOfRangeException("Index was outside the bounds of the array");
            if (item == null)
                throw new ArgumentNullException("Item cannot be null");
            this.EnsureSpace(1);
            for (int index1 = this.count; index1 > index; --index1)
                this.m_arrItem[index1] = this.m_arrItem[index1 - 1];
            item.OwnerChatListBox = this.owner;
            this.m_arrItem[index] = item;
            ++this.count;
            this.owner.Invalidate();
        }

        public bool Contains(ChatListItem item)
        {
            return this.IndexOf(item) != -1;
        }

        public void CopyTo(Array array, int index)
        {
            if (array == null)
                throw new ArgumentNullException("array cannot be null");
            this.m_arrItem.CopyTo(array, index);
        }

        int IList.Add(object value)
        {
            if (!(value is ChatListItem))
                throw new ArgumentException("Value cannot convert to ListItem");
            this.Add((ChatListItem)value);
            return this.IndexOf((ChatListItem)value);
        }

        void IList.Clear()
        {
            this.Clear();
        }

        bool IList.Contains(object value)
        {
            if (!(value is ChatListItem))
                throw new ArgumentException("Value cannot convert to ListItem");
            else
                return this.Contains((ChatListItem)value);
        }

        int IList.IndexOf(object value)
        {
            if (!(value is ChatListItem))
                throw new ArgumentException("Value cannot convert to ListItem");
            else
                return this.IndexOf((ChatListItem)value);
        }

        void IList.Insert(int index, object value)
        {
            if (!(value is ChatListItem))
                throw new ArgumentException("Value cannot convert to ListItem");
            this.Insert(index, (ChatListItem)value);
        }

        void IList.Remove(object value)
        {
            if (!(value is ChatListItem))
                throw new ArgumentException("Value cannot convert to ListItem");
            this.Remove((ChatListItem)value);
        }

        void IList.RemoveAt(int index)
        {
            this.RemoveAt(index);
        }



        void ICollection.CopyTo(Array array, int index)
        {
            this.CopyTo(array, index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            int i = 0;
            for (int Len = this.count; i < Len; ++i)
                yield return (object)this.m_arrItem[i];
        }


        object IList.this[int index]
        {
            get
            {
                return (object)this[index];
            }
            set
            {
                if (!(value is ChatListItem))
                    throw new ArgumentException("Value cannot convert to ListItem");
                this[index] = (ChatListItem)value;
            }
        }
    }
}
