using Avalonia;
using Avalonia.Layout;

namespace HandyControl.Data;

internal struct PanelUvSize
{
    private readonly Orientation _orientation;

    public Size ScreenSize => new(U, V);

    public double U { get; set; }

    public double V { get; set; }

    public double Width
    {
        get => _orientation == Orientation.Horizontal ? U : V;
        private set
        {
            if (_orientation == Orientation.Horizontal)
            {
                U = value;
            }
            else
            {
                V = value;
            }
        }
    }

    public double Height
    {
        get => _orientation == Orientation.Horizontal ? V : U;
        private set
        {
            if (_orientation == Orientation.Horizontal)
            {
                V = value;
            }
            else
            {
                U = value;
            }
        }
    }

    public PanelUvSize(Orientation orientation, double width, double height)
    {
        U = V = 0d;
        _orientation = orientation;
        Width = width;
        Height = height;
    }

    public PanelUvSize(Orientation orientation, Size size)
    {
        U = V = 0d;
        _orientation = orientation;
        Width = size.Width;
        Height = size.Height;
    }

    public PanelUvSize(Orientation orientation)
    {
        U = V = 0d;
        _orientation = orientation;
    }
}
