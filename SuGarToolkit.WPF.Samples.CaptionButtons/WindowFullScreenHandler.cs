using System.Windows;
using System.Windows.Shell;

namespace SuGarToolkit.WPF.Samples.CaptionButtons;

public class WindowFullScreenHandler
{
    public WindowFullScreenHandler(Window window)
    {
        _window = window;
        _window.StateChanged += OnWindowStateChanged;
    }

    public void ToggleFullScreen()
    {
        if (IsFullScreen)
        {
            ExitFullScreen();
        }
        else
        {
            EnterFullScreen();
        }
    }

    public void EnterFullScreen()
    {
        if (IsFullScreen)
            return;

        IsFullScreen = true;

        _windowStyle = _window.WindowStyle;
        _window.WindowStyle = WindowStyle.None;

        _windowState = _window.WindowState;
        _window.WindowState = WindowState.Minimized;
        _window.WindowState = WindowState.Maximized;

        WindowChrome windowChrome = WindowChrome.GetWindowChrome(_window);
        if (windowChrome is not null)
        {
            _windowChrome = (WindowChrome) windowChrome.Clone();
        }
        WindowChrome.SetWindowChrome(_window, (WindowChrome) _fullScreenWindowChrome.Clone());
    }

    public void ExitFullScreen()
    {
        if (!IsFullScreen)
            return;

        IsFullScreen = false;

        _window.WindowStyle = _windowStyle;
        WindowChrome.SetWindowChrome(_window, _windowChrome);
        _window.WindowState = _windowState;
    }

    public void ExitFullScreen(WindowState state)
    {
        if (!IsFullScreen)
            return;

        IsFullScreen = false;

        _window.WindowStyle = _windowStyle;
        WindowChrome.SetWindowChrome(_window, _windowChrome);
        _window.WindowState = state;
    }

    private void OnWindowStateChanged(object? sender, EventArgs e)
    {
        if (_window.WindowState is WindowState.Normal)
        {
            ExitFullScreen(WindowState.Normal);
        }
    }

    public bool IsFullScreen { get; private set; }

    private readonly Window _window;
    private WindowStyle _windowStyle;
    private WindowState _windowState;
    private WindowChrome? _windowChrome;

    private static readonly WindowChrome _fullScreenWindowChrome = new()
    {
        GlassFrameThickness = new Thickness(double.Epsilon),
        UseAeroCaptionButtons = false,
        CaptionHeight = 0,
        CornerRadius = new CornerRadius(0)
    };
}
