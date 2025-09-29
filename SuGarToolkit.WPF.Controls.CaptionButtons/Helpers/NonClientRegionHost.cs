using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;

namespace SuGarToolkit.WPF.Controls.CaptionButtons.Helpers;

public class NonClientRegionHost
{
    public NonClientRegionHost(Window window)
    {
        _window = window;
        if (_window.IsInitialized)
        {
            _hwndSource = (HwndSource) PresentationSource.FromVisual(_window);
            _hwndSource.AddHook(OnHwndSourceMessage);
            Refresh();
        }
        else
        {
            _window.SourceInitialized += OnWindowSourceInitialized;
        }
        _window.SizeChanged += OnWindowSizeChanged;
        _window.Unloaded += OnWindowUnloaded;
    }

    private void OnWindowSourceInitialized(object? sender, EventArgs e)
    {
        _hwndSource = (HwndSource) PresentationSource.FromVisual(_window);
        _hwndSource.AddHook(OnHwndSourceMessage);
        Refresh();
    }

    private void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
    {
        Refresh();
    }

    private void OnWindowUnloaded(object sender, RoutedEventArgs e)
    {
        _hwndSource!.RemoveHook(OnHwndSourceMessage);
        _hwndSource = null;
        _window.SizeChanged -= OnWindowSizeChanged;
    }

    public void Add(NonClientRegionKind nonClientRegionKind, UIElement element)
    {
        if (!_regions.TryGetValue(nonClientRegionKind, out Dictionary<UIElement, Rect>? region))
        {
            region = [];
            _regions.Add(nonClientRegionKind, region);
        }
        region.Add(element, default);
    }

    public void Refresh()
    {
        foreach (NonClientRegionKind nonClientRegionKind in Enum.GetValues<NonClientRegionKind>())
        {
            Refresh(nonClientRegionKind);
        }
    }

    public void Refresh(NonClientRegionKind nonClientRegionKind)
    {
        if (!_regions.TryGetValue(nonClientRegionKind, out Dictionary<UIElement, Rect>? value))
            return;

        foreach (UIElement element in value.Keys)
        {
            Refresh(nonClientRegionKind, element);
        }
    }

    public void Refresh(NonClientRegionKind nonClientRegionKind, UIElement element)
    {
        _regions[nonClientRegionKind][element] = new Rect(
            location: element.TransformToVisual(_window).Transform(new Point(0, 0)),
            size: element.RenderSize);
    }

    private CaptionButton? lastHoveredButton;
    private CaptionButton? lastPressedButton;

    private nint OnHwndSourceMessage(nint hwnd, int msg, nint wParam, nint lParam, ref bool handled)
    {
        switch (msg)
        {
            case WM_NCHITTEST:
                {
                    handled = TryGetPointerNonClientRegionKind(lParam, out NonClientRegionKind nonClientRegionKind, out UIElement? element);

                    lastHoveredButton?.IsMouseOverInTitleBar = false;
                    lastHoveredButton = null;

                    if (element is CaptionButton button && button.IsEnabled)
                    {
                        if (lastPressedButton is not null && lastPressedButton != button)
                        {
                            handled = false;
                            break;
                        }
                        button.IsMouseOverInTitleBar = true;
                        lastHoveredButton = button;
                    }

                    return (nint) nonClientRegionKind;
                }

            case WM_NCLBUTTONDOWN:
                {
                    handled = TryGetPointerNonClientRegionKind(lParam, out _, out UIElement? element);

                    lastHoveredButton?.IsMouseOverInTitleBar = false;
                    lastHoveredButton = null;
                    lastPressedButton?.IsPressedInTitleBar = false;
                    lastPressedButton = null;

                    if (element is CaptionButton button && button.IsEnabled)
                    {
                        button.IsPressedInTitleBar = true;
                        lastPressedButton = button;
                    }

                    break;
                }

            case WM_NCLBUTTONUP:
                {
                    handled = TryGetPointerNonClientRegionKind(lParam, out _, out UIElement? element);

                    if (element is CaptionButton button && button.IsEnabled)
                    {
                        button.IsPressedInTitleBar = false;
                        if (button == lastPressedButton)
                        {
                            button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                        }
                    }

                    lastHoveredButton?.IsMouseOverInTitleBar = false;
                    lastHoveredButton = null;
                    lastPressedButton?.IsPressedInTitleBar = false;
                    lastPressedButton = null;

                    break;
                }

            case WM_NCMOUSELEAVE:
                {
                    lastHoveredButton?.IsMouseOverInTitleBar = false;
                    lastHoveredButton = null;
                    lastPressedButton?.IsPressedInTitleBar = false;
                    lastPressedButton = null;
                    break;
                }
        }
        return 0;
    }

    private bool TryGetPointerNonClientRegionKind(
        nint lParam,
        out NonClientRegionKind nonClientRegionKind,
        out UIElement? pointerOnElement)
    {
        Point pointerScreenPosition = GetPointerScreenPixelPosition(lParam);
        Point pointerPosition = _window.PointFromScreen(pointerScreenPosition);  // PointFromScreen 把屏幕上的像素位置转换为 DIP 位置
        foreach (NonClientRegionKind kind in _regions.Keys)
        {
            foreach (UIElement element in _regions[kind].Keys)
            {
                if (_regions[kind][element].Contains(pointerPosition))
                {
                    pointerOnElement = element;
                    nonClientRegionKind = kind;
                    return true;
                }
            }
        }
        pointerOnElement = null;
        nonClientRegionKind = NonClientRegionKind.None;
        return false;
    }

    /// <summary>
    /// 根据 HwndSourceHook 参数获取鼠标的屏幕位置。
    /// <br/>
    /// 此函数专为 GetPointerNonClientRegion 所用。
    /// </summary>
    /// <param name="lParam"></param>
    /// <returns></returns>
    private static Point GetPointerScreenPixelPosition(nint lParam) => new()
    {
        X = GET_X_LPARAM(lParam),
        Y = GET_Y_LPARAM(lParam)
    };

    private static nint GET_X_LPARAM(nint lParam) => lParam & 0x0000FFFF;

    private static nint GET_Y_LPARAM(nint lParam) => (lParam >> 16) & 0x0000FFFF;

    private HwndSource? _hwndSource;
    private readonly Window _window;
    private readonly Dictionary<NonClientRegionKind, Dictionary<UIElement, Rect>> _regions = [];

    private const int WM_NCHITTEST = 0x0084;
    private const int WM_NCLBUTTONDOWN = 0x00A1;
    private const int WM_NCLBUTTONUP = 0x00A2;
    private const int WM_NCMOUSELEAVE = 0x02A2;
}
