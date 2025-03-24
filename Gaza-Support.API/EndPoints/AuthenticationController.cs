using Gaza_Support.Domains.Dtos.ResponseDtos;
using Infrastructure.Application.Authentication.DonorRegister;
using Infrastructure.Application.Authentication.Login;
using Infrastructure.Application.Authentication.RecipientLogin;
using Infrastructure.Application.Authentication.RecipientRegister;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gaza_Support.API.EndPoints
{
    public class AuthenticationController:ApiControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost("register-donor")]
        public async Task<ActionResult<BaseResponse<string>>>DonorRegister(DonorRegisterCommand command)
        {
            return StatusCode(await _mediator.Send(command));
        }

        [HttpPost("register-recipient")]
        public async Task<ActionResult<BaseResponse<string>>> RecipientRegister(RecipientRegisterCommand command)
        {
            return StatusCode(await _mediator.Send(command));
        }

        [HttpPost("login")]
        public async Task<ActionResult<BaseResponse<LoginQueryDto>>> Login(LoginQuery query)
        {
            return StatusCode(await _mediator.Send(query));
        }

        [HttpPost("login-recipient")]
        public async Task<ActionResult<BaseResponse<RecipientLoginQueryDto>>>RecipientLogin(RecipientLoginQuery query)
        {
            return StatusCode(await _mediator.Send(query));
        }
    }
}
