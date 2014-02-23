using System;
using System.Collections.Generic;
using System.Text;

namespace Client.UI.Base.Enums
{
    public enum ControlBoxState
    {
        Normal,
        Hover,
        Pressed,
        PressedLeave
    }

    public enum ControlState
    {
        Normal,
        Hover,
        Pressed,
        Focused
    }

    public enum ControlBoxStyle
    {
        None,
        Minimize,
        Maximize,
        Close,
        CmSysBottom
    }

    public enum RoundStyle
    {
        None,
        All,
        Left,
        Right,
        Top,
        Bottom,
        BottomLeft,
        BottomRight
    }

    public enum DrawStyle
    {
        None,
        Img,
        Draw
    }
    public enum BackStyle
    {
        Tile,
        Stretch
    }

    public enum MouseOperate
    {
        Normal,
        Move,
        Down,
        Up,
        Leave,
        Hover
    }

    public enum MobileStyle
    {
        None,
        TitleMobile,
        Mobile
    }

    public enum GrayscaleStyle
    {
        BT907,
        RMY,
        Y,
    }

    public enum TitleType
    {
        None,
        EffectTitle,
        Title,
    }

    public enum StopStates
    {
        Normal,
        Hover,
        Pressed,
        NoStop,
    }

    public enum DecorationType
    {
        None,
        BottomMirror,
        Custom
    }

    public enum PlatformType
    {
        PC,
        WebQQ,
        Iphone,
        Aandroid,
    }

    public enum ChatListItemIcon
    {
        Small = 27,
        Large = 53,
    }

}
