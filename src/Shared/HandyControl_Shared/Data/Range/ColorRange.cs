using System.Windows.Media;


namespace HandyControl.Data;

/// <summary>
///     颜色范围
/// </summary>
public class ColorRange : IValueRange<Color>
{
    private Color _start;

    public Color Start
    {
        get => _start;
        set
        {
            _start = value;
            Update();
        }
    }

    private Color _end;

    public Color End
    {
        get => _end;
        set
        {
            _end = value;
            Update();
        }
    }

    /// <summary>
    ///     颜色差值
    /// </summary>
    private readonly int[] _subColorArr = new int[4];

    /// <summary>
    ///     更新
    /// </summary>
    private void Update()
    {
        _subColorArr[0] = _start.A - _end.A;
        _subColorArr[1] = _start.R - _end.R;
        _subColorArr[2] = _start.G - _end.G;
        _subColorArr[3] = _start.B - _end.B;
    }

    /// <summary>
    ///     获取指定比例处的颜色
    /// </summary>
    /// <param name="range">范围（0-1）</param>
    /// <returns></returns>
    public Color GetColor(double range)
    {
        if (range < 0 || range > 1) return default;
        return Color.FromArgb((byte) (_start.A - _subColorArr[0] * range), (byte) (_start.R - _subColorArr[1] * range),
            (byte) (_start.G - _subColorArr[2] * range), (byte) (_start.B - _subColorArr[3] * range));
    }
}
