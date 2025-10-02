using SuGarToolkit.WPF.SourceGenerators;

using System.Windows;

namespace SuGarToolkit.WPF.Controls.CaptionButtons;

public partial class CaptionMaximizeButton : CaptionButton
{
    static CaptionMaximizeButton() => DefaultStyleKeyProperty.OverrideMetadata(typeof(CaptionMaximizeButton), new FrameworkPropertyMetadata(typeof(CaptionMaximizeButton)));

    public CaptionMaximizeButton()
    {
        Kind = CaptionButtonKind.Maximize;
    }

    [DependencyProperty]
    public partial bool IsRestoreButton { get; set; }
}
