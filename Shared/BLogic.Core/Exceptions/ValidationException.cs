using System.Net;
using BLogicCodeBase.Exceptions.Models;

namespace BLogicCodeBase.Exceptions;

public sealed class ValidationException(
    ICollection<ValidationError> errors,
    string detail,
    string message,
    string errorCode,
    bool ignoreLog = true)
    : Exception(string.IsNullOrEmpty(message) ? "One or more validation errors occurred" : message)
{
    public int StatusCode { get; set; } = (int)HttpStatusCode.BadRequest;
    public string Detail { get; set; } = detail ?? "Validation Failure";
    public string ErrorCode { get; set; } = errorCode;
    public bool IgnoreLog { get; set; } = ignoreLog;
    public ICollection<ValidationError> Errors { get; } = errors;
}