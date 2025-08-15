using System.Net;

namespace BLogicCodeBase.Exceptions;

public class CommonException : Exception
{
    public CommonException(string _message, HttpStatusCode _statusCode, bool _ignoreLog = false)
    {
        StatusCode = (int)_statusCode;
        Detail = _message;
        IgnoreLog = _ignoreLog;
    }
    
    public CommonException(string _message, HttpStatusCode _statusCode, string _errorCode, bool _ignoreLog = false)
    {
        StatusCode = (int)_statusCode;
        Detail = _message;
        ErrorCode = _errorCode;
        IgnoreLog = _ignoreLog;
    }
    
    public CommonException(string _message, int _statusCode, bool _ignoreLog = false)
    {
        StatusCode = _statusCode;
        Detail = _message;
        IgnoreLog = _ignoreLog;
    }
    public CommonException(string _message, int _statusCode, string _errorCode, bool _ignoreLog = false)
    {
        StatusCode = _statusCode;
        Detail = _message;
        ErrorCode = _errorCode;
        IgnoreLog = _ignoreLog;
    }

    public CommonException(string _message, bool _ignoreLog = false)
    {
        StatusCode = (int)HttpStatusCode.InternalServerError;
        Detail = _message;
        IgnoreLog = _ignoreLog;
    }

    public int StatusCode { get; set; }
    public string Detail { get; set; }
    public string ErrorCode { get; set; }
    public bool IgnoreLog { get; set; }
}