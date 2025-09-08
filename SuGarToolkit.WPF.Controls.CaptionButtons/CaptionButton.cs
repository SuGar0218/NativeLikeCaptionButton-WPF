using SuGarToolkit.WPF.SourceGenerators;

using System.Windows;
using System.Windows.Controls;

namespace SuGarToolkit.WPF.Controls.CaptionButtons;

public partial class CaptionButton : Button
{
    static CaptionButton() => DefaultStyleKeyProperty.OverrideMetadata(typeof(CaptionButton), new FrameworkPropertyMetadata(typeof(CaptionButton)));

    public CaptionButton()
    {
        //Resources.MergedDictionaries.Add(new CaptionButtonColorDictionary());
    }

    [DependencyProperty]
    public partial bool IsActive { get; set; }
}
