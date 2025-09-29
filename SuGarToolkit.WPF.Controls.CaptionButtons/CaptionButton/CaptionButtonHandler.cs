using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;
using System.Windows.Media;

namespace SuGarToolkit.WPF.Controls.CaptionButtons.Helpers;

public class CaptionButtonHandler
{
    public CaptionButtonHandler(HwndSource hwndSource)
    {
        _hwndSource = hwndSource;
        _hwndSource.AddHook(OnHwndSourceMessage);
    }

    public void Add(CaptionButton button)
    {
        _buttons.Add(button);
        button.IsVisibleChanged += OnButtonIsVisibleChanged;
        if (button.Visibility is Visibility.Visible)
        {
            DependencyObject child = VisualTreeHelper.GetChild(button, 0);
            _cacheButtonToChild.Add(button, child);
            _cacheChildToButton.Add(child, button);
        }
    }

    private void OnButtonIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        CaptionButton button = (CaptionButton) sender;
        if (button.IsVisible)
        {
            DependencyObject child = VisualTreeHelper.GetChild(button, 0);
            _cacheButtonToChild.Add(button, child);
            _cacheChildToButton.Add(child, button);
        }
        else if (_cacheButtonToChild.TryGetValue(button, out DependencyObject? child))
        {
            _cacheChildToButton.Remove(child);
            _cacheButtonToChild.Remove(button);
        }
    }

    private nint OnHwndSourceMessage(nint hwnd, int msg, nint wParam, nint lParam, ref bool handled)
    {
        switch (msg)
        {
            case WM_NCHITTEST:
                {
                    CaptionButton? button = GetPointedButton(lParam);
                    if (button is null)  // 鼠标不在任何标题栏按钮上
                    {
                        hoveredButton?.IsMouseOverInTitleBar = false;
                        hoveredButton = null;
                        break;
                    }
                    if (button.IsEnabled)
                    {
                        if (pressedButton is not null && pressedButton != button)  // 已经按下了其他按钮
                        {
                            pressedButton.IsMouseOverInTitleBar = false;
                            pressedButton.IsPressedInTitleBar = false;
                            break;
                        }
                        else if (pressedButton == button)
                        {
                            pressedButton.IsPressedInTitleBar = true;
                        }
                        hoveredButton?.IsMouseOverInTitleBar = false;
                        button.IsMouseOverInTitleBar = true;
                        hoveredButton = button;
                    }
                    handled = true;
                    return (nint) button.Kind;
                }

            case WM_NCLBUTTONDOWN:
                {
                    CaptionButton? button = GetPointedButton(lParam);
                    if (button is null)  // 没点到标题栏按钮上
                    {
                        hoveredButton?.IsMouseOverInTitleBar = false;
                        hoveredButton = null;
                        pressedButton?.IsPressedInTitleBar = false;
                        pressedButton = null;
                        break;
                    }
                    if (button.IsEnabled)
                    {
                        button.IsPressedInTitleBar = true;
                        pressedButton = button;
                    }
                    handled = true;
                    break;
                }

            case WM_NCLBUTTONUP:
                {
                    CaptionButton? button = GetPointedButton(lParam);
                    if (button is null)
                    {
                        pressedButton?.IsPressedInTitleBar = false;
                        pressedButton = null;
                        break;
                    }

                    if (button.IsEnabled && button == pressedButton)
                    {
                        button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    }
                    pressedButton?.IsPressedInTitleBar = false;
                    pressedButton = null;
                    handled = true;
                    break;
                }

            case WM_NCMOUSELEAVE:
                {
                    hoveredButton?.IsMouseOverInTitleBar = false;
                    hoveredButton = null;
                    pressedButton?.IsPressedInTitleBar = false;
                    pressedButton = null;
                    break;
                }
        }
        return 0;
    }

    private CaptionButton? GetPointedButton(nint lParam)
    {
        Point pointerScreenPosition = GetPointerScreenPixelPosition(lParam);
        Window ownerWindow = Window.GetWindow(_cacheChildToButton.Keys.First());
        return ownerWindow.InputHitTest(ownerWindow.PointFromScreen(pointerScreenPosition)) is DependencyObject hit && _cacheChildToButton.TryGetValue(hit, out CaptionButton? button) ? button : null;
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

    private readonly HwndSource? _hwndSource;
    private readonly HashSet<CaptionButton> _buttons = [];
    private readonly Dictionary<CaptionButton, DependencyObject> _cacheButtonToChild = [];
    private readonly Dictionary<DependencyObject, CaptionButton> _cacheChildToButton = [];

    private CaptionButton? hoveredButton;
    private CaptionButton? pressedButton;

    private const int WM_NCHITTEST = 0x0084;
    private const int WM_NCLBUTTONDOWN = 0x00A1;
    private const int WM_NCLBUTTONUP = 0x00A2;
    private const int WM_NCMOUSELEAVE = 0x02A2;
}
