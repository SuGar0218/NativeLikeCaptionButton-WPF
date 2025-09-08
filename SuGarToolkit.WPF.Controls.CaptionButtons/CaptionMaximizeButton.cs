using System.Windows;

namespace SuGarToolkit.WPF.Controls.CaptionButtons;

public partial class CaptionMaximizeButton : CaptionButton
{
    static CaptionMaximizeButton() => DefaultStyleKeyProperty.OverrideMetadata(typeof(CaptionMaximizeButton), new FrameworkPropertyMetadata(typeof(CaptionMaximizeButton)));
}
