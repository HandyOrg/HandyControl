using System.Windows.Media;


namespace HandyControl.Data
{
    /// <summary>
    ///     颜色范围
    /// </summary>
    public class ColorRange
    {
        /// <summary>
        ///     颜色1
        /// </summary>
        private Color _color1;

        /// <summary>
        ///     颜色1
        /// </summary>
        public Color Color1
        {
            get => _color1;
            set
            {
                _color1 = value;
                Update();
            }
        }

        /// <summary>
        ///     颜色2
        /// </summary>
        private Color _color2;

        /// <summary>
        ///     颜色2
        /// </summary>
        public Color Color2
        {
            get => _color2;
            set
            {
                _color2 = value;
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
            _subColorArr[0] = Color1.A - Color2.A;
            _subColorArr[1] = Color1.R - Color2.R;
            _subColorArr[2] = Color1.G - Color2.G;
            _subColorArr[3] = Color1.B - Color2.B;
        }

        /// <summary>
        ///     获取指定比例处的颜色
        /// </summary>
        /// <param name="range">范围（0-1）</param>
        /// <returns></returns>
        public Color GetColor(double range)
        {
            if (range < 0 || range > 1) return default;
            return Color.FromArgb((byte)(_color1.A - _subColorArr[0] * range), (byte)(_color1.R - _subColorArr[1] * range),
                (byte)(_color1.G - _subColorArr[2] * range), (byte)(_color1.B - _subColorArr[3] * range));
        }
    }
}