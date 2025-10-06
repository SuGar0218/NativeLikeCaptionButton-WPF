using SuGarToolkit.WPF.SourceGenerators;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SuGarToolkit.WPF.Controls.CaptionButtons;

[TemplatePart(Name = nameof(PART_CustomHeaderContentControl), Type = typeof(ContentControl))]
[TemplatePart(Name = nameof(PART_CenterContentPresenter), Type = typeof(ContentPresenter))]
[TemplatePart(Name = nameof(PART_CustomFooterContentControl), Type = typeof(ContentControl))]
[TemplatePart(Name = nameof(PART_CaptionButtonBar), Type = typeof(CaptionButtonBar))]
public partial class NativeLikeTitleBar : ContentControl
{
    static NativeLikeTitleBar() => DefaultStyleKeyProperty.OverrideMetadata(typeof(NativeLikeTitleBar), new FrameworkPropertyMetadata(typeof(NativeLikeTitleBar)));

    public NativeLikeTitleBar()
    {
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    public event EventHandler? BackButtonClick;
    public event EventHandler? PaneToggleButtonClick;

    public event EventHandler? MinimizeButtonClick;
    public event EventHandler? MaximizeButtonClick;
    public event EventHandler? CloseButtonClick;
    public event EventHandler? HelpButtonClick;

    [DependencyProperty]
    public partial ICommand? BackButtonCommand { get; set; }

    [DependencyProperty]
    public partial ICommand? PaneToggleButtonCommand { get; set; }

    [DependencyProperty]
    public partial ICommand? MinimizeButtonCommand { get; set; }

    [DependencyProperty]
    public partial ICommand? MaximizeButtonCommand { get; set; }

    [DependencyProperty]
    public partial ICommand? CloseButtonCommand { get; set; }

    [DependencyProperty]
    public partial ICommand? HelpButtonCommand { get; set; }

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

        PART_BackButton = (TitleBarButton) GetTemplateChild(nameof(PART_BackButton));
        PART_PaneToggleButton = (TitleBarButton) GetTemplateChild(nameof(PART_PaneToggleButton));
        PART_CaptionButtonBar = (CaptionButtonBar) GetTemplateChild(nameof(PART_CaptionButtonBar));

        PART_BackButton.Click += OnBackButtonClick;
        PART_PaneToggleButton.Click += OnPaneToggleButtonClick;
        PART_CaptionButtonBar.MinimizeButtonClick += OnMinimizeButtonClick;
        PART_CaptionButtonBar.MaximizeButtonClick += OnMaximizeButtonClick;
        PART_CaptionButtonBar.CloseButtonClick += OnCloseButtonClick;
        PART_CaptionButtonBar.HelpButtonClick += OnHelpButtonClick;
    }

    private void OnHelpButtonClick(object? sender, EventArgs e)
    {
        HelpButtonClick?.Invoke(this, e);
    }

    private void OnMinimizeButtonClick(object? sender, EventArgs e)
    {
        MinimizeButtonClick?.Invoke(this, e);
    }

    private void OnMaximizeButtonClick(object? sender, EventArgs e)
    {
        MaximizeButtonClick?.Invoke(this, e);
    }

    private void OnCloseButtonClick(object? sender, EventArgs e)
    {
        CloseButtonClick?.Invoke(this, e);
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

    private void OnBackButtonClick(object sender, RoutedEventArgs e)
    {
        BackButtonClick?.Invoke(this, e);
    }

    private void OnPaneToggleButtonClick(object sender, RoutedEventArgs e)
    {
        PaneToggleButtonClick?.Invoke(this, e);
    }

    private Window _ownerWindow;

    private ContentControl PART_CustomHeaderContentControl;
    private ContentPresenter PART_CenterContentPresenter;
    private ContentControl PART_CustomFooterContentControl;

    private TitleBarButton PART_BackButton;
    private TitleBarButton PART_PaneToggleButton;
    private CaptionButtonBar PART_CaptionButtonBar;
}
