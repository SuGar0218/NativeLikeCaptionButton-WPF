using System.Windows;

namespace SuGarToolkit.WPF.Controls.CaptionButtons;

public partial class CaptionMinimizeButton : CaptionButton
{
    static CaptionMinimizeButton() => DefaultStyleKeyProperty.OverrideMetadata(typeof(CaptionMinimizeButton), new FrameworkPropertyMetadata(typeof(CaptionMinimizeButton)));
}
