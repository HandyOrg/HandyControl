using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HandyControl.Controls
{
    public class TimeButton : ListBoxItem
    {
        #region Private属性
        #endregion

        #region 依赖属性定义

        #endregion

        #region 依赖属性set get

        #endregion

        #region Constructors
        static TimeButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimeButton), new FrameworkPropertyMetadata(typeof(TimeButton)));
        }
        #endregion

        #region Override方法
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
        #endregion

        #region Private方法

        #endregion
    }
}