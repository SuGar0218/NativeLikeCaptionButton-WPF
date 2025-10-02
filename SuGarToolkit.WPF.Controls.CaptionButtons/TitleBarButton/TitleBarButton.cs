using SuGarToolkit.WPF.SourceGenerators;

using System.Windows;
using System.Windows.Controls.Primitives;

namespace SuGarToolkit.WPF.Controls.CaptionButtons;

public partial class TitleBarButton : ButtonBase
{
    static TitleBarButton() => DefaultStyleKeyProperty.OverrideMetadata(typeof(TitleBarButton), new FrameworkPropertyMetadata(typeof(TitleBarButton)));

    [DependencyProperty(DefaultValue = true)]
    public partial bool IsActive { get; set; }
}
