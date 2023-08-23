using API.Core;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseApiController : ControllerBase
{

    protected ActionResult HandleResult<T>(Result<T> res)
    {
        if (res is null) return NotFound();

        return res.IsSuccess switch
        {
            true when res.Value is null => NotFound(),
            true when res.Value is not null => Ok(res.Value),
            false => BadRequest(res.Error),
            _ => BadRequest()
        };
    }
}