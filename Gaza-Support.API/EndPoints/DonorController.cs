using Gaza_Support.Domains.Dtos.ResponseDtos;
using Infrastructure.Application.Donors.Commands;
using Infrastructure.Application.Donors.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gaza_Support.API.EndPoints
{
    [Authorize]
    public class DonorController:ApiControllerBase
    {
        private readonly IMediator _mediator;

        public DonorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("donation")]
        public async Task<ActionResult<BaseResponse<string>>> CreateDonation([FromBody] CreateDonationCommand command)
        {
            return StatusCode(await _mediator.Send(command));
        }

        [HttpGet("recipients")]
        public async Task<ActionResult<BaseResponse<GetRecipientQueryDto>>> GetRecipients([FromQuery]GetRecipientQuery query)
        {
            return StatusCode(await _mediator.Send(query));
        }
    }
}
