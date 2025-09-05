using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuGarToolkit.WPF.Samples.CaptionButtons;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    public partial bool IsMinimizeButtonVisible { get; set; } = true;

    [ObservableProperty]
    public partial bool IsMaximizeButtonVisible { get; set; } = true;

    [ObservableProperty]
    public partial bool IsCloseButtonVisible { get; set; } = true;

    [ObservableProperty]
    public partial bool IsMinimizeButtonEnabled { get; set; } = true;

    [ObservableProperty]
    public partial bool IsMaximizeButtonEnabled { get; set; } = true;

    [ObservableProperty]
    public partial bool IsCloseButtonEnabled { get; set; } = true;

    [ObservableProperty]
    public partial double CaptionButtonHeight { get; set; } = 30;
}
