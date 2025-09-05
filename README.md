# Native-like Caption Button for WPF

Provides caption buttons in native-like style for WindowChrome.

## Features

You can decide for every button:

- Visibility (Visible, Collapsed, Hidden)
- IsEnabled (true, false)
- Height (typically 30 and 48 DIP)

<img width="3313" height="1457" alt="examples" src="https://github.com/user-attachments/assets/d22adf3e-71ae-4831-a4e5-bddab9517c7e" />

## Simple example

Just add ```CaptionButtonBar``` to your window, every button will change the window state properly. ```WindowChrome.IsHitTestVisibleInChrome``` has been set in the ```ControlTemplate``` and it will find owner window by ```Window.GetWindow(this)``` itself and react to ```Window.Activated```, ```Window.Deactivated```, ```Window.StateChanged``` events.

``` xml
<Window
    xmlns:CaptionButtons="clr-namespace:SuGarToolkit.WPF.Controls.CaptionButtons;assembly=SuGarToolkit.WPF.Controls.CaptionButtons"
    xmlns:system="clr-namespace:System;assembly=netstandard"
    Title="SuGarToolkit.WPF.Controls.CaptionButtons"
    StateChanged="Window_StateChanged"
    ...>

    <WindowChrome.WindowChrome>
        <WindowChrome
            x:Name="WindowChrome"
            CaptionHeight="30"
            UseAeroCaptionButtons="False" />
    </WindowChrome.WindowChrome>

    <Grid x:Name="LayoutRoot">
        <Grid.RowDefinitions>
            <RowDefinition Height="auto" />
            <RowDefinition Height="*" />
        </Grid.RowDefinitions>

        <Grid x:Name="TitleBarArea" Grid.Row="0">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="*" />
                <ColumnDefinition Width="auto" />
            </Grid.ColumnDefinitions>

            <CaptionButtons:CaptionButtonBar
                Grid.Column="1"
                Height="30"
                IsMaximizeButtonEnabled="False"
                IsMinimizeButtonEnabled="False" />
        </Grid>

        <StackPanel Grid.Row="1">
            ...
        </StackPanel>
    </Grid>

</Window>
```

When the window maximized, content near the edge of the window will be out of the range of the screen. To solve this problem, there is a feasible way: change the content margin when Window.StateChanged.

``` C#
private void Window_StateChanged(object sender, EventArgs e)
{
    switch (WindowState)
    {
        case WindowState.Maximized:
            double top = SystemParameters.ResizeFrameHorizontalBorderHeight + SystemParameters.FixedFrameHorizontalBorderHeight;
            double left = SystemParameters.ResizeFrameVerticalBorderWidth + SystemParameters.FixedFrameVerticalBorderWidth;
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
```
