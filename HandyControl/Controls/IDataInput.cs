using System;
using HandyControl.Data;

namespace HandyControl.Controls
{
    /// <summary>
    ///     表示可以成为一个数据输入控件
    /// </summary>
    public interface IDataInput
    {
        bool VerifyData();

        Func<string, OperationResult<bool>> VerifyFunc { get; set; }

        bool IsError { get; set; }

        string ErrorStr { get; set; }

        TextType TextType { get; set; }

        bool ShowClearButton { get; set; }
    }
}