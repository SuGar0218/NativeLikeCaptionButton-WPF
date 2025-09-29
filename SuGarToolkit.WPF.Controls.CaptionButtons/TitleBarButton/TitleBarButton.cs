using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using SuGarToolkit.WPF.SourceGenerators;

namespace SuGarToolkit.WPF.Controls.CaptionButtons;

public partial class TitleBarButton : ButtonBase
{
    static TitleBarButton() => DefaultStyleKeyProperty.OverrideMetadata(typeof(TitleBarButton), new FrameworkPropertyMetadata(typeof(TitleBarButton)));

    [DependencyProperty(DefaultValue = true)]
    public partial bool IsActive { get; set; }
}
