using System.Windows;

namespace SuGarToolkit.WPF.Controls.CaptionButtons;

public partial class CaptionRestoreButton : CaptionButton
{
    static CaptionRestoreButton() => DefaultStyleKeyProperty.OverrideMetadata(typeof(CaptionRestoreButton), new FrameworkPropertyMetadata(typeof(CaptionRestoreButton)));
}
