using DataAccess.Interface;
using Gaza_Support.Domains.Contstants;
using Gaza_Support.Domains.Dtos.ResponseDtos;
using MediatR;
using MongoDB.Driver;
using System.Net;

namespace Infrastructure.Application.Admin.Command.DeleteAdminAccountCommand
{
    public class DeleteAdminAccountCommand : IRequest<BaseResponse<string>>
    {
        public string UserId { get; set; }
    }

    internal class DeleteAccountCommandHandler : IRequestHandler<DeleteAdminAccountCommand, BaseResponse<string>>
    {
        private readonly IunitOfWork _unitOfWork;

        public DeleteAccountCommandHandler(IunitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<string>> Handle(DeleteAdminAccountCommand command, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepo.Collection
                .Find(x => x.Id == command.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                return BaseResponse<string>.Failure("user not found.", HttpStatusCode.NotFound);
            }

            var userRole = await _unitOfWork.RoleRepo.FindOneByAsync(user.RoleId);

            if(userRole.Name!= Roles.Admin)
            {
                return BaseResponse<string>.Failure("Not allowed.", HttpStatusCode.MethodNotAllowed);
            }

            await _unitOfWork.UserRepo.DeleteAsync(user);

            return BaseResponse<string>.Success("Account deleted.");
        }
    }
}
