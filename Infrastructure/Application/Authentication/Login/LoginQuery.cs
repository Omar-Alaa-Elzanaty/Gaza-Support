using DataAccess.Interface;
using Gaza_Support.Domains.Dtos.ResponseDtos;
using Gaza_Support.Domains.Models;
using Infrastructure.IServices;
using Mapster;
using MediatR;
using MongoDB.Driver;
using System.Net;

namespace Infrastructure.Application.Authentication.Login
{
    public class LoginQuery : IRequest<BaseResponse<LoginQueryDto>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    internal class LoginQueryHandler : IRequestHandler<LoginQuery, BaseResponse<LoginQueryDto>>
    {
        private readonly IAuthService _authService;
        private readonly IunitOfWork _unitOfWork;

        public LoginQueryHandler(
            IAuthService authService,
            IunitOfWork unitOfWork)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<LoginQueryDto>> Handle(LoginQuery query, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.UserRepo.Collection
                            .Find(x => x.Email == query.Email)
                            .FirstOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                return BaseResponse<LoginQueryDto>.Failure("User not found");
            }

            if (!_authService.VerifyPassword(entity, query.Password))
            {
                return BaseResponse<LoginQueryDto>.Failure("Unauthorized.", HttpStatusCode.Unauthorized);
            }

            var user = entity.Adapt<LoginQueryDto>();

            var role = await _unitOfWork.RoleRepo.Collection
                .Find(x => x.Id == entity.RoleId)
                .Project(x => x.Name)
                .FirstOrDefaultAsync(cancellationToken);

            user.Token = _authService.GenerateToken(entity.Id, entity.Email!, role);

            return BaseResponse<LoginQueryDto>.Success(user);
        }
    }
}