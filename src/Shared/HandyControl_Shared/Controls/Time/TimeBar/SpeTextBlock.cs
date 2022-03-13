using System;
using System.Windows.Controls;


namespace HandyControl.Controls;

/// <summary>
///     作为刻度使用的文字块
/// </summary>
internal class SpeTextBlock : TextBlock
{
    public double X { get; set; }

    public SpeTextBlock() => Width = 60;

    public SpeTextBlock(double x) : this()
    {
        X = x;
        Canvas.SetLeft(this, X);
    }

    /// <summary>
    ///     时间
    /// </summary>
    private DateTime _time;

    /// <summary>
    ///     时间
    /// </summary>
    public DateTime Time
    {
        get => _time;
        set
        {
            _time = value;
            Text = $"{value.ToString(TimeFormat)}\r\n|";
        }
    }

    /// <summary>
    ///     时间格式
    /// </summary>
    public string TimeFormat { get; set; } = "HH:mm";

    /// <summary>
    ///     横向移动
    /// </summary>
    /// <param name="offsetX"></param>
    public void MoveX(double offsetX) => Canvas.SetLeft(this, X + offsetX);
}
