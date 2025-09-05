using SuGarToolkit.WPF.SourceGenerators;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SuGarToolkit.WPF.Controls.CaptionButtons;

public partial class FontIcon : Control
{
    static FontIcon() => DefaultStyleKeyProperty.OverrideMetadata(typeof(FontIcon), new FrameworkPropertyMetadata(typeof(FontIcon)));

    [DependencyProperty]
    public partial string Glyph { get; set; }
}
