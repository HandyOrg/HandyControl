namespace HandyControl.Data;

public class OperationResult
{
    public static OperationResult<bool> Failed(string message = "")
    {
        return new()
        {
            ResultType = ResultType.Failed,
            Message = message,
            Data = false
        };
    }

    public static OperationResult<bool> Success(string message = "")
    {
        return new()
        {
            ResultType = ResultType.Success,
            Message = message,
            Data = true
        };
    }
}
