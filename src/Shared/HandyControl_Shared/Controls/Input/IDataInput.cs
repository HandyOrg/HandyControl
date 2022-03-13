using System;
using HandyControl.Data;


namespace HandyControl.Controls;

/// <summary>
///     表示可以成为一个数据输入控件
/// </summary>
public interface IDataInput
{
    /// <summary>
    ///     验证数据
    /// </summary>
    /// <returns></returns>
    bool VerifyData();

    /// <summary>
    ///     数据验证委托
    /// </summary>
    Func<string, OperationResult<bool>> VerifyFunc { get; set; }

    /// <summary>
    ///     数据是否错误
    /// </summary>
    bool IsError { get; set; }

    /// <summary>
    ///     错误提示
    /// </summary>
    string ErrorStr { get; set; }

    /// <summary>
    ///     文本类型
    /// </summary>
    TextType TextType { get; set; }

    /// <summary>
    ///     是否显示清除按钮
    /// </summary>
    bool ShowClearButton { get; set; }
}
