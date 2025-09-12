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

public partial class NativeLikeTitleBar : ContentControl
{
    static NativeLikeTitleBar() => DefaultStyleKeyProperty.OverrideMetadata(typeof(NativeLikeTitleBar), new FrameworkPropertyMetadata(typeof(NativeLikeTitleBar)));

    public event EventHandler? MinimizeButtonClick;
    public event EventHandler? MaximizeButtonClick;
    public event EventHandler? CloseButtonClick;

    [DependencyProperty(DefaultValue = Visibility.Visible)]
    public partial Visibility BackButtonVisibility { get; set; }

    [DependencyProperty(DefaultValue = Visibility.Visible)]
    public partial Visibility PaneToggleButtonVisibility { get; set; }

    [DependencyProperty(DefaultValue = Visibility.Visible)]
    public partial Visibility MinimizeButtonVisibility { get; set; }

    [DependencyProperty(DefaultValue = Visibility.Visible)]
    public partial Visibility MaximizeButtonVisibility { get; set; }

    [DependencyProperty(DefaultValue = Visibility.Visible)]
    public partial Visibility CloseButtonVisibility { get; set; }

    [DependencyProperty(DefaultValue = true)]
    public partial bool IsBackButtonEnabled { get; set; }

    [DependencyProperty(DefaultValue = true)]
    public partial bool IsPaneToggleButtonEnabled { get; set; }

    [DependencyProperty(DefaultValue = true)]
    public partial bool IsMinimizeButtonEnabled { get; set; }

    [DependencyProperty(DefaultValue = true)]
    public partial bool IsMaximizeButtonEnabled { get; set; }

    [DependencyProperty(DefaultValue = true)]
    public partial bool IsCloseButtonEnabled { get; set; }

    [DependencyProperty]
    public partial object? CustomHeader { get; set; }

    [DependencyProperty]
    public partial object? CustomFooter { get; set; }
}
