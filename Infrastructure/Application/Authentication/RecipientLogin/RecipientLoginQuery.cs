using DataAccess.Interface;
using DataAccess.Repo;
using Gaza_Support.API.Settings;
using Gaza_Support.Domains.Contstants;
using Gaza_Support.Domains.Dtos.ResponseDtos;
using Gaza_Support.Domains.Models;
using Infrastructure.Application.Authentication.Login;
using Infrastructure.IServices;
using Mapster;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Infrastructure.Application.Authentication.RecipientLogin
{
    public class RecipientLoginQuery:IRequest<BaseResponse<RecipientLoginQueryDto>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    internal class RecipientLoginQueryHandler : IRequestHandler<RecipientLoginQuery, BaseResponse<RecipientLoginQueryDto>>
    {
        private readonly IAuthService _authService;
        private readonly IunitOfWork _unitOfWork;

        public RecipientLoginQueryHandler(IAuthService authService, IunitOfWork unitOfWork)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<RecipientLoginQueryDto>> Handle(RecipientLoginQuery query, CancellationToken cancellationToken)
        {

            var entity = await _unitOfWork.RecipientRepo.Collection
                        .Find(x => x.Email == query.Email)
                        .FirstOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                return BaseResponse<RecipientLoginQueryDto>.Failure("User not found");
            }

            if (!_authService.VerifyPassword(entity, query.Password))
            {
                return BaseResponse<RecipientLoginQueryDto>.Failure("Unauthorized.", HttpStatusCode.Unauthorized);
            }

            var user = entity.Adapt<RecipientLoginQueryDto>();

            var role = await _unitOfWork.RoleRepo.Collection
                .Find(x => x.Id == entity.RoleId)
                .Project(x => x.Name)
                .FirstOrDefaultAsync(cancellationToken);

            user.Token = _authService.GenerateToken(entity.Id, entity.Email!, role);

            return BaseResponse<RecipientLoginQueryDto>.Success(user);
        }
    }
}
