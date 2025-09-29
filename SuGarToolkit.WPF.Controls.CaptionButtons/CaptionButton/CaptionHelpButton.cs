using System.Windows;

namespace SuGarToolkit.WPF.Controls.CaptionButtons;

public partial class CaptionHelpButton : CaptionButton
{
    static CaptionHelpButton() => DefaultStyleKeyProperty.OverrideMetadata(typeof(CaptionHelpButton), new FrameworkPropertyMetadata(typeof(CaptionHelpButton)));

    public CaptionHelpButton()
    {
        Kind = CaptionButtonKind.Help;
    }
}
