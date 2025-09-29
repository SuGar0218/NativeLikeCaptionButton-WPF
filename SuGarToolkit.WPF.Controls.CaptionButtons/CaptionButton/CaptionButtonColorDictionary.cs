using System.Windows;

namespace SuGarToolkit.WPF.Controls.CaptionButtons;

public class CaptionButtonColorDictionary : ResourceDictionary
{
    public CaptionButtonColorDictionary()
    {
        if (Environment.OSVersion.Version.Major >= 10 && Environment.OSVersion.Version.Build >= 22000)
        {
            Source = new Uri("/SuGarToolkit.WPF.Controls.CaptionButtons;component/CaptionButton/CaptionButtonColors.Windows11.xaml", UriKind.Relative);
        }
        else
        {
            Source = new Uri("/SuGarToolkit.WPF.Controls.CaptionButtons;component/CaptionButton/CaptionButtonColors.Windows10.xaml", UriKind.Relative);
        }
    }
}
