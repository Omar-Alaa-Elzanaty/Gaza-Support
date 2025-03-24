using Gaza_Support.Domains.Dtos.ResponseDtos;
using Infrastructure.Application.Recipients.Commands.CompleteProfile;
using Infrastructure.Application.Recipients.Queries.GetDonors;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gaza_Support.API.EndPoints
{
    [Authorize(Roles = "Recipient")]
    public class RecipientController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public RecipientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("profile")]
        public async Task<ActionResult<BaseResponse<string>>> CompleteProfile([FromBody] CompleteProfileCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpGet("donors")]
        public async Task<ActionResult<BaseResponse<List<GetDonorsQueryDto>>>> GetDonors()
        {
            return await _mediator.Send(new GetDonorsQuery());
        }
    }
}
