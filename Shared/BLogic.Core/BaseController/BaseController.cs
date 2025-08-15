using System.Net;
using BLogicCodeBase.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BLogicCodeBase.BaseController;

[ApiController]
public abstract class BaseController : ControllerBase
{
    protected ActionResult GetResponse<T>(T response)
    {
        return Ok(new BaseResponse<T>(response));
    }

    protected ActionResult GetResponse()
    {
        return Ok(new BaseResponse<object>()
        {
            IsError = false,
            StatusCode = (int)HttpStatusCode.OK,
            Detail = "success"
        });
    }
}
