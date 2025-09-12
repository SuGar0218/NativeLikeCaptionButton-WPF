using System.Windows;
using System.Windows.Media;

namespace SuGarToolkit.WPF.Controls.CaptionButtons.Helpers;

public class GeneralTransformHelper
{
    public GeneralTransformHelper(UIElement element)
    {
        _element = element;
        _transform = element.TransformToVisual(null);
        _presentationSource = PresentationSource.FromVisual(_element);
    }

    public Point GetPositionPoint()
    {
        return _transform.Transform(new Point(0, 0));
    }

    public Rect GetRegionRect()
    {
        return _transform.TransformBounds(new Rect
        {
            Width = _element.RenderSize.Width,
            Height = _element.RenderSize.Height
        });
    }

    public Point GetPixelPoint(Point dipPoint)
    {
        return new()
        {
            X = dipPoint.X * RasterizationScale,
            Y = dipPoint.Y * RasterizationScale
        };
    }

    public Int32Rect GetPixelRect(Rect dipRect)
    {
        return new()
        {
            X = (int) (dipRect.X * RasterizationScale),
            Y = (int) (dipRect.Y * RasterizationScale),
            Width = (int) (dipRect.Width * RasterizationScale),
            Height = (int) (dipRect.Height * RasterizationScale)
        };
    }

    private double RasterizationScale => _presentationSource.CompositionTarget.TransformToDevice.M11;

    private readonly UIElement _element;
    private readonly GeneralTransform _transform;
    private readonly PresentationSource _presentationSource;
}
