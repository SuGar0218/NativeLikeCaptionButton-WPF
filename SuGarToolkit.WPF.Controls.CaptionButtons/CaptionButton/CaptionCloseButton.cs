using System.Windows;

namespace SuGarToolkit.WPF.Controls.CaptionButtons;

public partial class CaptionCloseButton : CaptionButton
{
    static CaptionCloseButton() => DefaultStyleKeyProperty.OverrideMetadata(typeof(CaptionCloseButton), new FrameworkPropertyMetadata(typeof(CaptionCloseButton)));

    public CaptionCloseButton()
    {
        Kind = CaptionButtonKind.Close;
    }
}
