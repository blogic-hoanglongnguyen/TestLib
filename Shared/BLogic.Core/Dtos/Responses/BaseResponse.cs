using BLogic.Helper.Extensions;

namespace BLogicCodeBase.Dtos.Responses;

public sealed class BaseResponse<T>
{
    public T Data { get; set; }
    public int StatusCode { get; set; }
    public string Detail { get; set; }
    public bool IsError { get; set; }
    public string ErrorCode { get; set; }


    public BaseResponse()
    {
    }

    public BaseResponse(T data)
    {
        Data = data;
        IsError = false;
        Detail = "success";
    }

    public BaseResponse(bool isError, int statusCode, string message)
    {
        IsError = isError;
        StatusCode = statusCode;
        Detail = message;
    }

    public BaseResponse(bool isError, int statusCode, string message, string errorCode)
    {
        IsError = isError;
        StatusCode = statusCode;
        Detail = message;
        ErrorCode = errorCode;
    }

    public BaseResponse(T data, bool isError, int statusCode, string message)
    {
        Data = data;
        IsError = isError;
        StatusCode = statusCode;
        Detail = message;
    }

    public override string ToString()
    {
        return JsonExtension.Serialize(this);
    }
}