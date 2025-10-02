using SuGarToolkit.WPF.Controls.CaptionButtons.Helpers;
using SuGarToolkit.WPF.SourceGenerators;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace SuGarToolkit.WPF.Controls.CaptionButtons;

/// <summary>
/// CaptionButtonBar 会自行根据所在窗口是否在前台改变样式，按钮也会操作所在窗口的状态。
/// </summary>
[TemplatePart(Name = nameof(MinimizeButton), Type = typeof(CaptionMinimizeButton))]
[TemplatePart(Name = nameof(MaximizeButton), Type = typeof(CaptionMaximizeButton))]
[TemplatePart(Name = nameof(CloseButton), Type = typeof(CaptionCloseButton))]
[TemplatePart(Name = nameof(HelpButton), Type = typeof(CaptionHelpButton))]
public partial class CaptionButtonBar : Control
{
    static CaptionButtonBar() => DefaultStyleKeyProperty.OverrideMetadata(typeof(CaptionButtonBar), new FrameworkPropertyMetadata(typeof(CaptionButtonBar)));

    public CaptionButtonBar()
    {
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    public CaptionMinimizeButton MinimizeButton { get; private set; }
    public CaptionMaximizeButton MaximizeButton { get; private set; }
    public CaptionCloseButton CloseButton { get; private set; }
    public CaptionHelpButton HelpButton { get; private set; }

    /// <summary>
    /// 用于触发切换最大化/还原按钮的触发器
    /// </summary>
    [DependencyProperty]
    public partial WindowState OwnerWindowState { get; private set; }

    public event EventHandler? MinimizeButtonClick;
    public event EventHandler? MaximizeButtonClick;
    public event EventHandler? CloseButtonClick;
    public event EventHandler? HelpButtonClick;

    [DependencyProperty(DefaultValue = Visibility.Visible)]
    public partial Visibility MinimizeButtonVisibility { get; set; }

    [DependencyProperty(DefaultValue = Visibility.Visible)]
    public partial Visibility MaximizeButtonVisibility { get; set; }

    [DependencyProperty(DefaultValue = Visibility.Visible)]
    public partial Visibility CloseButtonVisibility { get; set; }

    [DependencyProperty(DefaultValue = Visibility.Collapsed)]
    public partial Visibility HelpButtonVisibility { get; set; }

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

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        MinimizeButton = (CaptionMinimizeButton) GetTemplateChild(nameof(MinimizeButton));
        MaximizeButton = (CaptionMaximizeButton) GetTemplateChild(nameof(MaximizeButton));
        CloseButton = (CaptionCloseButton) GetTemplateChild(nameof(CloseButton));
        HelpButton = (CaptionHelpButton) GetTemplateChild(nameof(HelpButton));

        MinimizeButton.Click += OnMinimizeButtonClick;
        MaximizeButton.Click += OnMaximizeButtonClick;
        CloseButton.Click += OnCloseButtonClick;
        HelpButton.Click += OnHelpButtonClick;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        _ownerWindow = Window.GetWindow(this);
        _ownerWindow.Activated += OnActivated;
        _ownerWindow.Deactivated += OnDeactivated;
        _ownerWindow.StateChanged += OnOwnerWindowStateChanged;

        _ownerHwndSource = HwndSource.FromHwnd(new WindowInteropHelper(_ownerWindow).Handle);
        _captionButtonHandler = new CaptionButtonHandler(_ownerHwndSource);
        _captionButtonHandler.Add(MinimizeButton);
        _captionButtonHandler.Add(MaximizeButton);
        _captionButtonHandler.Add(CloseButton);
        _captionButtonHandler.Add(HelpButton);

        HWND hWnd = new(_ownerHwndSource.Handle);
        int style = PInvoke.GetWindowLong(hWnd, WINDOW_LONG_PTR_INDEX.GWL_STYLE);
        style &= ~WS_SYSMENU;
        _ = PInvoke.SetWindowLong(hWnd, WINDOW_LONG_PTR_INDEX.GWL_STYLE, style);
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        _ownerWindow.StateChanged -= OnOwnerWindowStateChanged;
        _ownerWindow.Activated -= OnActivated;
        _ownerWindow.StateChanged -= OnOwnerWindowStateChanged;
        _ownerWindow = null!;
    }

    private void OnOwnerWindowStateChanged(object? sender, EventArgs e)
    {
        OwnerWindowState = _ownerWindow.WindowState;
    }

    private void OnActivated(object? sender, EventArgs e)
    {
        IsActive = true;
    }

    private void OnDeactivated(object? sender, EventArgs e)
    {
        IsActive = false;
    }

    private void OnMinimizeButtonClick(object sender, RoutedEventArgs e)
    {
        _ownerWindow.WindowState = WindowState.Minimized;
    }

    private void OnMaximizeButtonClick(object sender, RoutedEventArgs e)
    {
        if (_ownerWindow.WindowState is WindowState.Maximized)
        {
            _ownerWindow.WindowState = WindowState.Normal;
        }
        else
        {
            _ownerWindow.WindowState = WindowState.Maximized;
        }
    }

    private void OnRestoreButtonClick(object sender, RoutedEventArgs e)
    {
        _ownerWindow.WindowState = WindowState.Normal;
    }

    private void OnCloseButtonClick(object sender, RoutedEventArgs e)
    {
        _ownerWindow.Close();
    }

    private void OnHelpButtonClick(object sender, RoutedEventArgs e)
    {
        HelpButtonClick?.Invoke(this, EventArgs.Empty);
    }

    private Window _ownerWindow;
    private HwndSource _ownerHwndSource;
    private CaptionButtonHandler _captionButtonHandler;

    private const int WS_MAXIMIZEBOX = 0x00010000;
    private const int WS_MINIMIZEBOX = 0x00020000;
    private const int WS_SYSMENU = 0x00080000;
}
