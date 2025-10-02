using SuGarToolkit.WPF.SourceGenerators;

using System.Windows;

namespace SuGarToolkit.WPF.Controls.CaptionButtons;

/// <summary>
/// 系统标题栏按钮，特指 Windows 窗口上的帮助、最小化、最大化/还原/关闭按钮，如果不是这些功能请不要使用 CaptionButton 而使用 TitleBarButton。
/// </summary>
public partial class CaptionButton : TitleBarButton
{
    static CaptionButton() => DefaultStyleKeyProperty.OverrideMetadata(typeof(CaptionButton), new FrameworkPropertyMetadata(typeof(CaptionButton)));

    /// <summary>
    /// 用于窗口消息处理时判断，此属性本身并不起作用。
    /// </summary>
    [DependencyProperty(DefaultValue = CaptionButtonKind.None)]
    public partial CaptionButtonKind Kind { get; set; }

    [DependencyProperty]
    public partial bool IsMouseOverInTitleBar { get; set; }

    [DependencyProperty]
    public partial bool IsPressedInTitleBar { get; set; }
}
