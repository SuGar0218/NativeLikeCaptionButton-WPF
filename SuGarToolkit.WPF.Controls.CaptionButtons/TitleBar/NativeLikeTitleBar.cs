using SuGarToolkit.WPF.SourceGenerators;

using System.Windows;
using System.Windows.Controls;

namespace SuGarToolkit.WPF.Controls.CaptionButtons;

[TemplatePart(Name = nameof(PART_CustomHeaderContentControl), Type = typeof(ContentControl))]
[TemplatePart(Name = nameof(PART_CenterContentPresenter), Type = typeof(ContentPresenter))]
[TemplatePart(Name = nameof(PART_CustomFooterContentControl), Type = typeof(ContentControl))]
public partial class NativeLikeTitleBar : ContentControl
{
    static NativeLikeTitleBar() => DefaultStyleKeyProperty.OverrideMetadata(typeof(NativeLikeTitleBar), new FrameworkPropertyMetadata(typeof(NativeLikeTitleBar)));

    public NativeLikeTitleBar()
    {
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

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

    [DependencyProperty(DefaultValue = Visibility.Collapsed)]
    public partial Visibility HelpButtonVisibility { get; set; }

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

    [DependencyProperty(DefaultValue = true)]
    public partial bool IsHelpButtonEnabled { get; set; }

    [DependencyProperty(DefaultValue = true)]
    public partial bool IsActive { get; set; }

    [DependencyProperty]
    public partial object? CustomHeader { get; set; }

    [DependencyProperty]
    public partial object? CustomFooter { get; set; }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        PART_CustomHeaderContentControl = (ContentControl) GetTemplateChild(nameof(PART_CustomHeaderContentControl));
        PART_CenterContentPresenter = (ContentPresenter) GetTemplateChild(nameof(PART_CenterContentPresenter));
        PART_CustomFooterContentControl = (ContentControl) GetTemplateChild(nameof(PART_CustomFooterContentControl));
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        _ownerWindow = Window.GetWindow(this);
        _ownerWindow.Activated += OnOwnerWindowActivated;
        _ownerWindow.Deactivated += OnOwnerWindowDeactivated;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        _ownerWindow.Activated -= OnOwnerWindowActivated;
        _ownerWindow.Deactivated -= OnOwnerWindowDeactivated;
    }

    private void OnOwnerWindowActivated(object? sender, EventArgs e)
    {
        IsActive = true;
    }

    private void OnOwnerWindowDeactivated(object? sender, EventArgs e)
    {
        IsActive = false;
    }

    private Window _ownerWindow;
    private ContentControl PART_CustomHeaderContentControl;
    private ContentPresenter PART_CenterContentPresenter;
    private ContentControl PART_CustomFooterContentControl;
}
