using SuGarToolkit.WPF.SourceGenerators;

using System.Windows;
using System.Windows.Controls;

namespace SuGarToolkit.WPF.Controls.CaptionButtons;

public partial class FontIcon : Control
{
    static FontIcon() => DefaultStyleKeyProperty.OverrideMetadata(typeof(FontIcon), new FrameworkPropertyMetadata(typeof(FontIcon)));

    [DependencyProperty]
    public partial string Glyph { get; set; }
}
