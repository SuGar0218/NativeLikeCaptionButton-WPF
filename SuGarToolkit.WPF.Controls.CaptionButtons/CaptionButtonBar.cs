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

[TemplatePart(Name = nameof(MinimizeButton))]
[TemplatePart(Name = nameof(MaximizeButton))]
[TemplatePart(Name = nameof(RestoreButton))]
[TemplatePart(Name = nameof(CloseButton))]
public partial class CaptionButtonBar : Control
{
    static CaptionButtonBar() => DefaultStyleKeyProperty.OverrideMetadata(typeof(CaptionButtonBar), new FrameworkPropertyMetadata(typeof(CaptionButtonBar)));
    
    public CaptionButtonBar()
    {
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        OwnerWindow = Window.GetWindow(this);
        OwnerWindow.Activated += OnActivated;
        OwnerWindow.Deactivated += OnDeactivated;
        OwnerWindow.StateChanged += OnOwnerWindowStateChanged;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        OwnerWindow.StateChanged -= OnOwnerWindowStateChanged;
        OwnerWindow.Activated -= OnActivated;
        OwnerWindow.StateChanged -= OnOwnerWindowStateChanged;
        OwnerWindow = null!;
    }

    private void OnOwnerWindowStateChanged(object? sender, EventArgs e)
    {
        OwnerWindowState = OwnerWindow.WindowState;
    }

    private void OnActivated(object? sender, EventArgs e)
    {
        MinimizeButton.IsActive = true;
        MaximizeButton.IsActive = true;
        RestoreButton.IsActive = true;
        CloseButton.IsActive = true;
    }

    private void OnDeactivated(object? sender, EventArgs e)
    {
        MinimizeButton.IsActive = false;
        MaximizeButton.IsActive = false;
        RestoreButton.IsActive = false;
        CloseButton.IsActive = false;
    }

    public CaptionMinimizeButton MinimizeButton { get; private set; }
    public CaptionMaximizeButton MaximizeButton { get; private set; }
    public CaptionRestoreButton RestoreButton { get; private set; }
    public CaptionCloseButton CloseButton { get; private set; }
    private Window OwnerWindow { get; set; }

    [DependencyProperty]
    public partial WindowState OwnerWindowState { get; private set; }

    public event EventHandler? MinimizeButtonClick;
    public event EventHandler? MaximizeButtonClick;
    public event EventHandler? CloseButtonClick;

    [DependencyProperty(DefaultValue = Visibility.Visible)]
    public partial Visibility MinimizeButtonVisibility { get; set; }

    [DependencyProperty(DefaultValue = Visibility.Visible)]
    public partial Visibility MaximizeButtonVisibility { get; set; }

    [DependencyProperty(DefaultValue = Visibility.Visible)]
    public partial Visibility CloseButtonVisibility { get; set; }

    [DependencyProperty(DefaultValue = true)]
    public partial bool IsMinimizeButtonEnabled { get; set; }

    [DependencyProperty(DefaultValue = true)]
    public partial bool IsMaximizeButtonEnabled { get; set; }

    [DependencyProperty(DefaultValue = true)]
    public partial bool IsCloseButtonEnabled { get; set; }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        MinimizeButton = (CaptionMinimizeButton) GetTemplateChild(nameof(MinimizeButton));
        MaximizeButton = (CaptionMaximizeButton) GetTemplateChild(nameof(MaximizeButton));
        RestoreButton = (CaptionRestoreButton) GetTemplateChild(nameof(RestoreButton));
        CloseButton = (CaptionCloseButton) GetTemplateChild(nameof(CloseButton));

        MinimizeButton.Click += OnMinimizeButtonClick;
        MaximizeButton.Click += OnMaximizeButtonClick;
        RestoreButton.Click += OnRestoreButtonClick;
        CloseButton.Click += OnCloseButtonClick;
    }

    private void OnMinimizeButtonClick(object sender, RoutedEventArgs e)
    {
        OwnerWindow.WindowState = WindowState.Minimized;
    }

    private void OnMaximizeButtonClick(object sender, RoutedEventArgs e)
    {
        OwnerWindow.WindowState = WindowState.Maximized;
    }

    private void OnRestoreButtonClick(object sender, RoutedEventArgs e)
    {
        OwnerWindow.WindowState = WindowState.Normal;
    }

    private void OnCloseButtonClick(object sender, RoutedEventArgs e)
    {
        OwnerWindow.Close();
    }
}
