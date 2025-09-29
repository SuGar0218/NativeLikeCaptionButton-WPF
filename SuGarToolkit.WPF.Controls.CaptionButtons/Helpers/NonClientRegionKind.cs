using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuGarToolkit.WPF.Controls.CaptionButtons.Helpers;

public enum NonClientRegionKind
{
    /// <summary>
    /// 在屏幕背景上，或在窗口之间的分隔线上。
    /// </summary>
    None = 0,

    /// <summary>
    /// 在工作区中。
    /// </summary>
    Client = 1,

    /// <summary>
    /// 在标题栏中。
    /// </summary>
    CaptionBar = 2,

    /// <summary>
    /// 在窗口菜单或子窗口的关闭按钮中。
    /// </summary>
    SystemMenu = 3,

    /// <summary>
    /// 在大小框中（与 GROWBOX 相同）。
    /// </summary>
    Resize = 4,

    /// <summary>
    /// 在菜单中。
    /// </summary>
    Menu = 5,

    /// <summary>
    /// 在水平滚动条中。
    /// </summary>
    HSCROLL = 6,

    /// <summary>
    /// 在垂直滚动条中。
    /// </summary>
    VSCROLL = 7,

    /// <summary>
    /// 在"最小化"按钮中。
    /// </summary>
    MinimizeButton = 8,

    /// <summary>
    /// 在"最大化"按钮中。
    /// </summary>
    MaximizeButton = 9,

    /// <summary>
    /// 在可调整大小的窗口的左边框中（用户可以单击鼠标以水平调整窗口大小）。
    /// </summary>
    Left = 10,

    /// <summary>
    /// 在可调整大小的窗口的右左边框中（用户可以单击鼠标以水平调整窗口大小）。
    /// </summary>
    Right = 11,

    /// <summary>
    /// 在窗口的上水平边框中。
    /// </summary>
    Top = 12,

    /// <summary>
    /// 在窗口边框的左上角。
    /// </summary>
    TopLeft = 13,

    /// <summary>
    /// 在窗口边框的右上角。
    /// </summary>
    TopRight = 14,

    /// <summary>
    /// 在可调整大小的窗口的下水平边框中（用户可以单击鼠标以垂直调整窗口大小）。
    /// </summary>
    Bottom = 15,

    /// <summary>
    /// 在可调整大小的窗口的边框左下角（用户可以单击鼠标以对角线调整窗口大小）。
    /// </summary>
    BottomLeft = 16,

    /// <summary>
    /// 在可调整大小的窗口的边框右下角（用户可以单击鼠标以对角线调整窗口大小）。
    /// </summary>
    BottomRight = 17,

    /// <summary>
    /// 在没有大小调整边框的窗口边框中。
    /// </summary>
    Border = 18,

    /// <summary>
    /// 在"关闭"按钮中。
    /// </summary>
    CloseButton = 20,

    /// <summary>
    /// 在"帮助"按钮中。
    /// </summary>
    HelpButton = 21,

    /// <summary>
    /// 在同一线程当前由另一个窗口覆盖的窗口中（消息将发送到同一线程中的基础窗口，直到其中一个窗口返回不是 TRANSPARENT 的代码）。
    /// </summary>
    Transparent = -1,

    /// <summary>
    /// 在屏幕背景上或窗口之间的分割线上（与 NOWHERE 相同，只是 DefWindowProc 函数会生成系统蜂鸣音以指示错误）。
    /// </summary>
    Error = -2
}
