using Gaza_Support.Domains.Dtos.ResponseDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Gaza_Support.API.EndPoints
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiControllerBase : ControllerBase
    {
        public ObjectResult StatusCode<T>(BaseResponse<T> result)
        {
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
