using Gaza_Support.Domains.Dtos.ResponseDtos;
using Infrastructure.Application.Recipients.Commands.CompleteProfile;
using Infrastructure.Application.Recipients.Commands.MarkDonationAsRead;
using Infrastructure.Application.Recipients.Queries.GetDonationsWithPagination;
using Infrastructure.Application.Recipients.Queries.GetDonors;
using Infrastructure.Application.Recipients.Queries.GetProfile;
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
            return StatusCode(await _mediator.Send(command));
        }

        [HttpGet("donors")]
        public async Task<ActionResult<BaseResponse<List<GetDonorsQueryDto>>>> GetDonors()
        {
            return StatusCode(await _mediator.Send(new GetDonorsQuery()));
        }

        [HttpGet("donotions/pages")]
        public async Task<ActionResult<PaginatedResponse<GetDonationsWithPaginationQueryDto>>> GetDonotions([FromQuery] GetDonationsWithPaginationQuery query)
        {
            return StatusCode(await _mediator.Send(query));
        }

        [HttpPut("donotions/markAsRead")]
        public async Task<ActionResult<BaseResponse<string>>> MarkDonotionAsRead([FromBody] MarkDonationAsReadCommand command)
        {
            return StatusCode(await _mediator.Send(command));
        }

        [HttpGet("profile")]
        public async Task<ActionResult<BaseResponse<GetRecipientProfileQueryDto>>> GetProfile()
        {
            return StatusCode(await _mediator.Send(new GetRecipientProfileQuery()));
        }
    }
}
