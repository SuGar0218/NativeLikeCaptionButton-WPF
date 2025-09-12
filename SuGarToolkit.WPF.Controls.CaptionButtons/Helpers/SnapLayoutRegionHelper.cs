using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Shell;

namespace SuGarToolkit.WPF.Controls.CaptionButtons.Helpers;

internal class SnapLayoutRegionHelper
{
    public SnapLayoutRegionHelper(Window window)
    {
        _window = window;
        _hwnd = new HWND(new WindowInteropHelper(window).Handle);
        _regions = [];

        //PInvoke.SetWindowSubclass(_hwnd, )
    }

    public SnapLayoutRegionHelper Add(UIElement element)
    {
        _regions.Add(element, default);
        return this;
    }

    public SnapLayoutRegionHelper Refresh(UIElement element)
    {
        GeneralTransformHelper generalTransformHelper = new(element);
        _regions[element] = generalTransformHelper.GetPixelRect(generalTransformHelper.GetRegionRect());
        return this;
    }

    //private LRESULT SubclassHitTestProc(HWND hwnd, uint uMsg, WPARAM wParam, LPARAM lParam, nuint uIdSubclass, nuint dwRefData)
    //{
    //    if (uMsg == 0x0084/*WM_NCHITTEST*/)
    //    {
    //        System.Drawing.Point position = new()
    //        {
    //            X = (int) lParam.Value & 0xFFFF,
    //            Y = (int) (lParam.Value >> 16) & 0xFFFF
    //        };
    //        PInvoke.ScreenToClient(hwnd, ref position);
    //        foreach (UIElement element in _regions.Keys)
    //        {
    //            Int32Rect rect = _regions[element];
    //            if (position.X >= rect.X && position.X <= rect.X + rect.Width && position.Y >= rect.Y && position.Y <= rect.Y + rect.Height)
    //            {
    //                element.IsMouseCaptured = true;
    //                return new LRESULT(9/*HTMAXBUTTON*/);
    //            }
    //        }
    //    }
    //}

    private SUBCLASSPROC _subclassProc;

    private readonly Window _window;
    private readonly HWND _hwnd;
    private readonly Dictionary<UIElement, Int32Rect> _regions;
}
