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
        }
        else
        {
            _window.SourceInitialized += OnWindowSourceInitialized;
        }
        _window.Unloaded += OnWindowUnloaded;
    }

    private void OnWindowSourceInitialized(object? sender, EventArgs e)
    {
        _hwndSource = (HwndSource) PresentationSource.FromVisual(_window);
        _hwndSource.AddHook(OnHwndSourceMessage);
    }

    private void OnWindowUnloaded(object sender, RoutedEventArgs e)
    {
        _hwndSource!.RemoveHook(OnHwndSourceMessage);
        _hwndSource = null;
    }

    public void Add(NonClientRegionKind nonClientRegionKind, IInputElement element)
    {
        if (!_regions.TryGetValue(nonClientRegionKind, out HashSet<IInputElement>? region))
        {
            region = [];
            _regions.Add(nonClientRegionKind, region);
        }
        region.Add(element);
    }

    private CaptionButton? lastHoveredButton;
    private CaptionButton? lastPressedButton;

    private nint OnHwndSourceMessage(nint hwnd, int msg, nint wParam, nint lParam, ref bool handled)
    {
        switch (msg)
        {
            case WM_NCHITTEST:
                {
                    NonClientRegionKind kind = NonClientRegionHitTest(lParam, out IInputElement? element);
                    if (kind == NonClientRegionKind.None)
                        break;

                    handled = true;
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

                    return (nint) kind;
                }

            case WM_NCLBUTTONDOWN:
                {
                    NonClientRegionKind kind = NonClientRegionHitTest(lParam, out IInputElement? element);
                    if (kind == NonClientRegionKind.None)
                        break;

                    handled = true;
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
                    NonClientRegionKind kind = NonClientRegionHitTest(lParam, out IInputElement? element);
                    if (kind == NonClientRegionKind.None)
                        break;

                    handled = true;
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

    private NonClientRegionKind NonClientRegionHitTest(
        nint lParam,
        out IInputElement? hitElement)
    {
        Point pointerScreenPosition = GetPointerScreenPixelPosition(lParam);
        Point pointerPosition = _window.PointFromScreen(pointerScreenPosition);  // PointFromScreen 把屏幕上的像素位置转换为 DIP 位置
        hitElement = _window.InputHitTest(pointerPosition);
        foreach (NonClientRegionKind kind in _regions.Keys)
        {
            if (_regions[kind].Contains(hitElement))
                return kind;
        }
        return NonClientRegionKind.None;
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
    private readonly Dictionary<NonClientRegionKind, HashSet<IInputElement>> _regions = [];

    private const int WM_NCHITTEST = 0x0084;
    private const int WM_NCLBUTTONDOWN = 0x00A1;
    private const int WM_NCLBUTTONUP = 0x00A2;
    private const int WM_NCMOUSELEAVE = 0x02A2;
}
