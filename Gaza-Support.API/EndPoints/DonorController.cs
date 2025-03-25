using Gaza_Support.Domains.Dtos.ResponseDtos;
using Infrastructure.Application.Donors.Commands.AddSubscribe;
using Infrastructure.Application.Donors.Commands.CancelSubscribe;
using Infrastructure.Application.Donors.Commands.CreateDonation;
using Infrastructure.Application.Donors.Queries.GetRecipient;
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

        [HttpPost("subscribe")]
        public async Task<ActionResult<BaseResponse<string>>> Subscribe([FromBody] AddSubscribeCommand command)
        {
            return StatusCode(await _mediator.Send(command));
        }

        [HttpDelete("subscribe")]
        public async Task<ActionResult<BaseResponse<string>>> UnSubscribe([FromBody] CancelSubscribeCommand command)
        {
            return StatusCode(await _mediator.Send(command));
        }
    }
}
