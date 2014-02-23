using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Client.UI.Base.Enums;
using System.ComponentModel;
using System.Drawing.Imaging;
using Client.UI.DefaultResource;
using System.Runtime.InteropServices;

namespace Client.UI.Base.Collection.Item
{
    public class ChatListSubItem : IComparable
    {
        private uint id;
        private object tag;
        private PlatformType platformTypes;
        private string nicName;
        private string displayName;
        private string personalMsg;
        private string ipAddress;
        private int updPort;
        private int tcpPort;
        private Image headImage;
        private ChatListSubItem.UserStatus status;
        private bool isVip;
        private bool isTwinkle;
        private bool isTwinkleHide;
        private Rectangle bounds;
        private Rectangle headRect;
        private ChatListItem ownerListItem;

        public uint ID
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        public object Tag
        {
            get
            {
                return this.tag;
            }
            set
            {
                this.tag = value;
            }
        }

        public PlatformType PlatformTypes
        {
            get
            {
                return this.platformTypes;
            }
            set
            {
                if (this.platformTypes == value)
                    return;
                this.platformTypes = value;
                this.RedrawSubItem();
            }
        }

        public string NicName
        {
            get
            {
                return this.nicName;
            }
            set
            {
                this.nicName = value;
                this.RedrawSubItem();
            }
        }

        public string DisplayName
        {
            get
            {
                return this.displayName;
            }
            set
            {
                this.displayName = value;
                this.RedrawSubItem();
            }
        }

        public string PersonalMsg
        {
            get
            {
                return this.personalMsg;
            }
            set
            {
                this.personalMsg = value;
                this.RedrawSubItem();
            }
        }

        public string IpAddress
        {
            get
            {
                return this.ipAddress;
            }
            set
            {
                if (!this.CheckIpAddress(value))
                    return;
                this.ipAddress = value;
            }
        }

        public int UpdPort
        {
            get
            {
                return this.updPort;
            }
            set
            {
                this.updPort = value;
            }
        }

        public int TcpPort
        {
            get
            {
                return this.tcpPort;
            }
            set
            {
                this.tcpPort = value;
            }
        }

        public Image HeadImage
        {
            get
            {
                return this.headImage;
            }
            set
            {
                this.headImage = value;
                this.RedrawSubItem();
            }
        }

        public ChatListSubItem.UserStatus Status
        {
            get
            {
                return this.status;
            }
            set
            {
                if (this.status == value)
                    return;
                this.status = value;
                if (this.ownerListItem == null)
                    return;
                this.ownerListItem.SubItems.Sort();
            }
        }

        public bool IsVip
        {
            get
            {
                return this.isVip;
            }
            set
            {
                this.isVip = value;
            }
        }

        public bool IsTwinkle
        {
            get
            {
                return this.isTwinkle;
            }
            set
            {
                if (this.isTwinkle == value || this.ownerListItem == null)
                    return;
                this.isTwinkle = value;
                if (this.isTwinkle)
                    ++this.ownerListItem.TwinkleSubItemNumber;
                else
                    --this.ownerListItem.TwinkleSubItemNumber;
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
        public Rectangle HeadRect
        {
            get
            {
                return this.headRect;
            }
            set
            {
                this.headRect = value;
            }
        }

        [Browsable(false)]
        public ChatListItem OwnerListItem
        {
            get
            {
                return this.ownerListItem;
            }
            set
            {
                this.ownerListItem = value;
            }
        }

        public ChatListSubItem()
        {
            this.status = ChatListSubItem.UserStatus.Online;
            this.displayName = "displayName";
            this.nicName = "nicName";
            this.personalMsg = "Personal Message ...";
            this.IsVip = false;
            this.PlatformTypes = PlatformType.PC;
            this.HeadImage = GetDefaultResource.GetImage("Common.no_photo.png");
        }

        public ChatListSubItem(string nicname)
        {
            this.nicName = nicname;
        }

        public ChatListSubItem(string nicname, ChatListSubItem.UserStatus status)
        {
            this.nicName = nicname;
            this.status = status;
        }

        public ChatListSubItem(string nicname, string displayname, string personalmsg)
        {
            this.nicName = nicname;
            this.displayName = displayname;
            this.personalMsg = personalmsg;
        }

        public ChatListSubItem(string nicname, string displayname, string personalmsg, ChatListSubItem.UserStatus status)
        {
            this.nicName = nicname;
            this.displayName = displayname;
            this.personalMsg = personalmsg;
            this.status = status;
        }

        public ChatListSubItem(uint id, string nicname, string displayname, string personalmsg, ChatListSubItem.UserStatus status, Bitmap head)
        {
            this.id = id;
            this.nicName = nicname;
            this.displayName = displayname;
            this.personalMsg = personalmsg;
            this.status = status;
            this.headImage = (Image)head;
        }

        public ChatListSubItem(uint id, string nicname, string displayname, string personalmsg, ChatListSubItem.UserStatus status, PlatformType platformTypes, Bitmap head)
        {
            this.id = id;
            this.nicName = nicname;
            this.displayName = displayname;
            this.personalMsg = personalmsg;
            this.status = status;
            this.PlatformTypes = platformTypes;
            this.headImage = (Image)head;
        }

        public ChatListSubItem(uint id, string nicname, string displayname, string personalmsg, ChatListSubItem.UserStatus status, PlatformType platformTypes, Bitmap head, bool isvip)
        {
            this.id = id;
            this.nicName = nicname;
            this.displayName = displayname;
            this.personalMsg = personalmsg;
            this.status = status;
            this.PlatformTypes = platformTypes;
            this.headImage = (Image)head;
            this.IsVip = isvip;
        }

        public ChatListSubItem Clone()
        {
            return new ChatListSubItem()
            {
                Bounds = this.Bounds,
                DisplayName = this.DisplayName,
                HeadImage = this.HeadImage,
                HeadRect = this.HeadRect,
                ID = this.ID,
                IpAddress = this.IpAddress,
                IsTwinkle = this.IsTwinkle,
                IsTwinkleHide = this.isTwinkleHide,
                NicName = this.NicName,
                OwnerListItem = this.OwnerListItem.Clone(),
                PersonalMsg = this.PersonalMsg,
                Status = this.Status,
                TcpPort = this.TcpPort,
                UpdPort = this.UpdPort,
                Tag = this.Tag,
                PlatformTypes = this.PlatformTypes,
                IsVip = this.IsVip
            };
        }

        private void RedrawSubItem()
        {
            if (this.ownerListItem == null || this.ownerListItem.OwnerChatListBox == null)
                return;
            this.ownerListItem.OwnerChatListBox.Invalidate(this.bounds);
        }

        public Bitmap GetDarkImage()
        {
            Bitmap bitmap1 = new Bitmap(this.headImage);
            Bitmap bitmap2 = bitmap1.Clone(new Rectangle(0, 0, this.headImage.Width, this.headImage.Height), PixelFormat.Format24bppRgb);
            bitmap1.Dispose();
            BitmapData bitmapdata = bitmap2.LockBits(new Rectangle(0, 0, bitmap2.Width, bitmap2.Height), ImageLockMode.ReadWrite, bitmap2.PixelFormat);
            byte[] numArray = new byte[bitmap2.Height * bitmapdata.Stride];
            Marshal.Copy(bitmapdata.Scan0, numArray, 0, numArray.Length);
            int num1 = 0;
            for (int width = bitmap2.Width; num1 < width; ++num1)
            {
                int num2 = 0;
                for (int height = bitmap2.Height; num2 < height; ++num2)
                    numArray[num2 * bitmapdata.Stride + num1 * 3] = numArray[num2 * bitmapdata.Stride + num1 * 3 + 1] = numArray[num2 * bitmapdata.Stride + num1 * 3 + 2] = this.GetAvg(numArray[num2 * bitmapdata.Stride + num1 * 3], numArray[num2 * bitmapdata.Stride + num1 * 3 + 1], numArray[num2 * bitmapdata.Stride + num1 * 3 + 2]);
            }
            Marshal.Copy(numArray, 0, bitmapdata.Scan0, numArray.Length);
            bitmap2.UnlockBits(bitmapdata);
            return bitmap2;
        }

        private byte GetAvg(byte b, byte g, byte r)
        {
            return (byte)(((int)r + (int)g + (int)b) / 3);
        }

        private bool CheckIpAddress(string str)
        {
            if (str == null)
                return false;
            if (str.Split(new char[1]
      {
        '.'
      }).Length != 4)
                return false;
            for (int index = 0; index < 4; ++index)
            {
                try
                {
                    if (Convert.ToInt32(str[index]) > (int)byte.MaxValue)
                        return false;
                }
                catch (FormatException ex)
                {
                    return false;
                }
            }
            return true;
        }

        int IComparable.CompareTo(object obj)
        {
            if (!(obj is ChatListSubItem))
                throw new NotImplementedException("obj is not ChatListSubItem");
            else
                return this.status.CompareTo((object)(obj as ChatListSubItem).status);
        }

        public enum UserStatus
        {
            QMe = 1,
            Online = 2,
            Away = 3,
            Busy = 4,
            DontDisturb = 5,
            OffLine = 6,
        }
    }
}
