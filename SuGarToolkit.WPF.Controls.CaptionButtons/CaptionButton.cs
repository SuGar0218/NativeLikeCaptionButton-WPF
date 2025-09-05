using SuGarToolkit.WPF.SourceGenerators;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SuGarToolkit.WPF.Controls.CaptionButtons;

public partial class CaptionButton : Button
{
    static CaptionButton() => DefaultStyleKeyProperty.OverrideMetadata(typeof(CaptionButton), new FrameworkPropertyMetadata(typeof(CaptionButton)));

    public CaptionButton()
    {
        if (Environment.OSVersion.Version.Major >= 10 && Environment.OSVersion.Version.Build >= 22000)
        {
            Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("/SuGarToolkit.WPF.Controls.CaptionButtons;component/CaptionButtonColors.Windows11.xaml", UriKind.Relative)
            });
        }
        else
        {
            Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("/SuGarToolkit.WPF.Controls.CaptionButtons;component/CaptionButtonColors.Windows10.xaml", UriKind.Relative)
            });
        }
    }

    [DependencyProperty]
    public partial bool IsActive { get; set; }
}
