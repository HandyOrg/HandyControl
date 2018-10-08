using System.Collections.Generic;
using System.Globalization;
using GalaSoft.MvvmLight;
using HandyControlDemo.Tools.Converter;

namespace HandyControlDemo.ViewModel
{
    public class ComboBoxDemoViewModel : ViewModelBase
    {
        /// <summary>
        ///     数据列表
        /// </summary>
        private List<string> _dataList;

        /// <summary>
        ///     数据列表
        /// </summary>
        public List<string> DataList
        {
            get => _dataList;
            set => Set(ref _dataList, value);
        }

        public ComboBoxDemoViewModel()
        {
            var converter = new StringRepeatConverter();
            var list = new List<string>();
            for (var i = 1; i <= 9; i++)
            {
                list.Add(converter.Convert(Properties.Langs.Lang.Text, null, i, CultureInfo.CurrentCulture)?.ToString());
            }

            DataList = list;
        }
    }
}