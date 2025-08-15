using BLogicCodeBase.Exceptions.Models;

namespace BLogicCodeBase.Exceptions;

public sealed class ValidationModel
{
    public int StatusCode { get; set; }
    public string Detail { get; set; }
    public string ErrorCode { get; set; }
    public bool IgnoreLog { get; set; }
    public ICollection<ValidationError> Errors { get; set; } = [];
}