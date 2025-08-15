using BLogic.Helper.Extensions;
using BLogicCodeBase.Exceptions.Models;

namespace BLogicCodeBase.Dtos.Responses;

public sealed class ValidationBaseResponse<T>
{
    public T Data { get; set; }
    public int StatusCode { get; set; }
    public string Detail { get; set; }
    public bool IsError { get; set; }
    public string ErrorCode { get; set; }
    public ICollection<ValidationError> Errors { get; set; } = [];
    
    public ValidationBaseResponse()
    {
    }

    public ValidationBaseResponse(T data)
    {
        Data = data;
        IsError = false;
        Detail = "success";
    }

    public ValidationBaseResponse(bool isError, int statusCode, string message, ICollection<ValidationError> errors = null)
    {
        IsError = isError;
        StatusCode = statusCode;
        Detail = message;
        Errors = !isError || errors == null ? [] : errors;
    }

    public ValidationBaseResponse(bool isError, int statusCode, string message, string errorCode, ICollection<ValidationError> errors = null)
    {
        IsError = isError;
        StatusCode = statusCode;
        Detail = message;
        ErrorCode = errorCode;
        Errors = !isError || errors == null ? [] : errors;
    }

    public ValidationBaseResponse(T data, bool isError, int statusCode, string message, ICollection<ValidationError> errors = null)
    {
        Data = data;
        IsError = isError;
        StatusCode = statusCode;
        Detail = message;
        Errors = !isError || errors == null ? [] : errors;
    }

    public override string ToString()
    {
        return JsonExtension.Serialize(this);
    }
}