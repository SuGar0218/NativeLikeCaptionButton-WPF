using CommunityToolkit.Mvvm.ComponentModel;

namespace SuGarToolkit.WPF.Samples.CaptionButtons;

internal partial class TitleBarElementViewModel : ObservableObject
{
    [ObservableProperty]
    public partial bool IsBackButtonVisible { get; set; } = true;

    [ObservableProperty]
    public partial bool IsBackButtonEnabled { get; set; } = true;

    [ObservableProperty]
    public partial bool IsPaneToggleButtonVisible { get; set; } = true;

    [ObservableProperty]
    public partial bool IsPaneToggleButtonEnabled { get; set; } = true;

    [ObservableProperty]
    public partial bool IsCaptionTextVisible { get; set; } = true;

    [ObservableProperty]
    public partial bool IsMenuBarVisible { get; set; } = true;

    [ObservableProperty]
    public partial bool IsMenuBarEnabled { get; set; } = true;

    [ObservableProperty]
    public partial bool IsAutoSuggestBoxVisible { get; set; } = true;

    [ObservableProperty]
    public partial bool IsAutoSuggestBoxEnabled { get; set; } = true;

    [ObservableProperty]
    public partial bool IsPersonPictureVisible { get; set; } = true;

    [ObservableProperty]
    public partial bool IsPersonPictureEnabled { get; set; } = true;

    [ObservableProperty]
    public partial bool IsThemeToggleButtonVisible { get; set; } = true;

    [ObservableProperty]
    public partial bool IsThemeToggleButtonEnabled { get; set; } = true;

    [ObservableProperty]
    public partial bool IsSettingsButtonVisible { get; set; } = true;

    [ObservableProperty]
    public partial bool IsSettingsButtonEnabled { get; set; } = true;

    [ObservableProperty]
    public partial bool IsHelpButtonVisible { get; set; } = true;

    [ObservableProperty]
    public partial bool IsHelpButtonEnabled { get; set; } = true;

    [ObservableProperty]
    public partial bool IsFullScreenButtonVisible { get; set; } = true;

    [ObservableProperty]
    public partial bool IsFullScreenButtonEnabled { get; set; } = true;

    [ObservableProperty]
    public partial bool IsMinimizeButtonVisible { get; set; } = true;

    [ObservableProperty]
    public partial bool IsMinimizeButtonEnabled { get; set; } = true;

    [ObservableProperty]
    public partial bool IsMaximizeButtonVisible { get; set; } = true;

    [ObservableProperty]
    public partial bool IsMaximizeButtonEnabled { get; set; } = true;

    [ObservableProperty]
    public partial bool IsCloseButtonVisible { get; set; } = true;

    [ObservableProperty]
    public partial bool IsCloseButtonEnabled { get; set; } = true;

    [ObservableProperty]
    public partial double TitleBarHeight { get; set; } = 42;
}
