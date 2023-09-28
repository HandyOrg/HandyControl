namespace HandyControl.Data;

/// <summary>
///     表示一个操作的返回结果信息
/// </summary>
public class OperationResult<T> : OperationResult
{
    /// <summary>
    ///     操作结果
    /// </summary>
    public ResultType ResultType { get; set; }

    /// <summary>
    ///     返回数据
    /// </summary>
    public T Data { get; set; }

    /// <summary>
    ///     操作消息（包含错误原因等数据）
    /// </summary>
    public string Message { get; set; }
}
