using System.Windows;

namespace SuGarToolkit.WPF.Samples.CaptionButtons;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_StateChanged(object sender, EventArgs e)
    {
        switch (WindowState)
        {
            case WindowState.Maximized:
                double top = SystemParameters.ResizeFrameHorizontalBorderHeight + SystemParameters.FixedFrameHorizontalBorderHeight + SystemParameters.BorderWidth;
                double left = SystemParameters.ResizeFrameVerticalBorderWidth + SystemParameters.FixedFrameVerticalBorderWidth + SystemParameters.BorderWidth;
                LayoutRoot.Margin = new Thickness
                {
                    Left = left,
                    Right = left,
                    Top = top,
                    Bottom = top
                };
                break;
            default:
                LayoutRoot.Margin = new Thickness(0);
                break;
        }
    }
}