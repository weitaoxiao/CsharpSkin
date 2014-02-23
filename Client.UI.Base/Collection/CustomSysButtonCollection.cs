using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Client.UI.Base.Controls;

namespace Client.UI.Base.Collection
{
    public class CustomSysButtonCollection : IList, ICollection, IEnumerable
    {
        private int count;
        private CmSysButton[] m_arrItem;
        private Forms.FormBase owner;

        public CustomSysButtonCollection(Forms.FormBase owner)
        {
            this.owner = owner;
        }

        public void Add(CmSysButton item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Item cannot be null");
            }
            this.EnsureSpace(1);
            if (-1 == this.IndexOf(item))
            {
                item.OwnerForm = this.owner;
                this.m_arrItem[this.count++] = item;
                this.owner.Invalidate();
            }
        }

        public void AddRange(CmSysButton[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("Items cannot be null");
            }
            this.EnsureSpace(items.Length);
            try
            {
                foreach (CmSysButton button in items)
                {
                    if (button == null)
                    {
                        throw new ArgumentNullException("Item cannot be null");
                    }
                    if (-1 == this.IndexOf(button))
                    {
                        button.OwnerForm = this.owner;
                        this.m_arrItem[this.count++] = button;
                    }
                }
            }
            finally
            {
                this.owner.Invalidate();
            }
        }

        public void Clear()
        {
            this.count = 0;
            this.m_arrItem = null;
            this.owner.Invalidate();
        }

        public bool Contains(CmSysButton item)
        {
            return (this.IndexOf(item) != -1);
        }

        public void CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array cannot be null");
            }
            this.m_arrItem.CopyTo(array, index);
        }

        private void EnsureSpace(int elements)
        {
            if (this.m_arrItem == null)
            {
                this.m_arrItem = new CmSysButton[Math.Max(elements, 4)];
            }
            else if ((this.count + elements) > this.m_arrItem.Length)
            {
                CmSysButton[] array = new CmSysButton[Math.Max((int) (this.count + elements), (int) (this.m_arrItem.Length * 2))];
                this.m_arrItem.CopyTo(array, 0);
                this.m_arrItem = array;
            }
        }

        public int IndexOf(CmSysButton item)
        {
            return Array.IndexOf<CmSysButton>(this.m_arrItem, item);
        }

        public void Insert(int index, CmSysButton item)
        {
            if ((index < 0) || (index >= this.count))
            {
                throw new IndexOutOfRangeException("Index was outside the bounds of the array");
            }
            if (item == null)
            {
                throw new ArgumentNullException("Item cannot be null");
            }
            this.EnsureSpace(1);
            for (int i = this.count; i > index; i--)
            {
                this.m_arrItem[i] = this.m_arrItem[i - 1];
            }
            item.OwnerForm = this.owner;
            this.m_arrItem[index] = item;
            this.count++;
            this.owner.Invalidate();
        }

        public void Remove(CmSysButton item)
        {
            int index = this.IndexOf(item);
            if (-1 != index)
            {
                this.RemoveAt(index);
            }
        }

        public void RemoveAt(int index)
        {
            if ((index < 0) || (index >= this.count))
            {
                throw new IndexOutOfRangeException("Index was outside the bounds of the array");
            }
            this.count--;
            int num = index;
            int count = this.count;
            while (num < count)
            {
                this.m_arrItem[num] = this.m_arrItem[num + 1];
                num++;
            }
            this.owner.Invalidate();
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
                yield return this.m_arrItem[index];
                index++;
            }
        }

        int IList.Add(object value)
        {
            if (!(value is CmSysButton))
            {
                throw new ArgumentException("Value cannot convert to ListItem");
            }
            this.Add((CmSysButton) value);
            return this.IndexOf((CmSysButton) value);
        }

        void IList.Clear()
        {
            this.Clear();
        }

        bool IList.Contains(object value)
        {
            if (!(value is CmSysButton))
            {
                throw new ArgumentException("Value cannot convert to ListItem");
            }
            return this.Contains((CmSysButton) value);
        }

        int IList.IndexOf(object value)
        {
            if (!(value is CmSysButton))
            {
                throw new ArgumentException("Value cannot convert to ListItem");
            }
            return this.IndexOf((CmSysButton) value);
        }

        void IList.Insert(int index, object value)
        {
            if (!(value is CmSysButton))
            {
                throw new ArgumentException("Value cannot convert to ListItem");
            }
            this.Insert(index, (CmSysButton) value);
        }

        void IList.Remove(object value)
        {
            if (!(value is CmSysButton))
            {
                throw new ArgumentException("Value cannot convert to ListItem");
            }
            this.Remove((CmSysButton) value);
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

        public CmSysButton this[int index]
        {
            get
            {
                if ((index < 0) || (index >= this.count))
                {
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array");
                }
                return this.m_arrItem[index];
            }
            set
            {
                if ((index < 0) || (index >= this.count))
                {
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array");
                }
                this.m_arrItem[index] = value;
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
                if (!(value is CmSysButton))
                {
                    throw new ArgumentException("Value cannot convert to ListItem");
                }
                this[index] = (CmSysButton) value;
            }
        }

        
    }
}
