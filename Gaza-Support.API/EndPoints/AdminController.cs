using Gaza_Support.Domains.Contstants;
using Gaza_Support.Domains.Dtos.ResponseDtos;
using Infrastructure.Application.Admin.Command.CreateAdminCommand;
using Infrastructure.Application.Admin.Command.DeleteAdminAccountCommand;
using Infrastructure.Application.Admin.Queries;
using Infrastructure.Application.Admin.Queries.GetRecipientById;
using Infrastructure.Application.Admin.Queries.GetRecipientWitPagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Gaza_Support.API.EndPoints
{
    [Authorize(Roles =Roles.Admin)]
    public class AdminController:ApiControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("account")]
        public async Task<ActionResult<BaseResponse<string>>> CreateAdminAccount([FromBody] CreateAdminCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("account")]
        public async Task<ActionResult<BaseResponse<string>>> DeleteAdminAccount([FromBody] DeleteAdminAccountCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet("recipients")]
        public async Task<ActionResult<PaginatedResponse<GetRecipientWithPaginationQueryDto>>> GetRecipients([FromQuery]GetRecipientWithPaginationQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("recipients/{id}")]
        public async Task<ActionResult<GetRecipientByIdQueryDto>> GetRecipientById(string id)
        {
            return Ok(await _mediator.Send(new GetRecipientByIdQuery(id)));
        }
    }
}
