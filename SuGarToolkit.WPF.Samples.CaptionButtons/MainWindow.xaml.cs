using System.Diagnostics;
using System.Windows;

namespace SuGarToolkit.WPF.Samples.CaptionButtons;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        _fullScreenHandler = new WindowFullScreenHandler(this);
    }

    private void Window_StateChanged(object sender, EventArgs e)
    {
        switch (WindowState)
        {
            case WindowState.Maximized:
                double left = SystemParameters.ResizeFrameVerticalBorderWidth + SystemParameters.FixedFrameVerticalBorderWidth + SystemParameters.BorderWidth;
                double top = SystemParameters.ResizeFrameHorizontalBorderHeight + SystemParameters.FixedFrameHorizontalBorderHeight + SystemParameters.BorderWidth;
                LayoutRoot.Margin = new Thickness(left, top, left, top);
                break;
            default:
                LayoutRoot.Margin = new Thickness(0);
                break;
        }
    }

    private void PaneToggleButtonClick(object sender, EventArgs e)
    {
        if (PART_SettingsScrollViewer.Visibility is Visibility.Visible)
        {
            PART_SettingsScrollViewer.Visibility = Visibility.Collapsed;
        }
        else
        {
            PART_SettingsScrollViewer.Visibility = Visibility.Visible;
        }
    }

    private void HelpButtonClick(object sender, EventArgs e)
    {
        using Process process = new()
        {
            StartInfo = new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = @"https://github.com/SuGar0218/NativeLikeCaptionButton-WPF"
            }
        };
        process.Start();
    }

    private void FullScreenButtonClick(object sender, RoutedEventArgs e)
    {
        _fullScreenHandler.ToggleFullScreen();
    }

    private readonly WindowFullScreenHandler _fullScreenHandler;
}