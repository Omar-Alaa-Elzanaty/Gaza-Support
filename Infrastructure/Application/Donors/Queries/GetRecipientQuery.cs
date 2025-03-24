using DataAccess.Interface;
using Gaza_Support.Domains.Contstants;
using Gaza_Support.Domains.Dtos.ResponseDtos;
using Mapster;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Application.Donors.Queries
{
    public class GetRecipientQuery : IRequest<BaseResponse<List<GetRecipientQueryDto>>>
    {
        public bool SortAscendingOnSubscribeDonation { get; set; }
    }

    internal class GetRecipientQueryHandler : IRequestHandler<GetRecipientQuery, BaseResponse<List<GetRecipientQueryDto>>>
    {
        private readonly IunitOfWork _unitOfWork;

        public GetRecipientQueryHandler(IunitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<List<GetRecipientQueryDto>>> Handle(GetRecipientQuery request, CancellationToken cancellationToken)
        {
            var roleId = await _unitOfWork.RoleRepo.Collection
                        .Find(x => x.Name == Roles.Recipient)
                        .Project(x => x.Id)
                        .FirstOrDefaultAsync(cancellationToken);

            if (roleId == null)
            {
                return BaseResponse<List<GetRecipientQueryDto>>.Failure("Role not found");
            }

            var entities = _unitOfWork.RecipientRepo.Collection.AsQueryable()
                .Where(x => x.RoleId == roleId)
                .GroupJoin(_unitOfWork.SubscribeRepo.Collection.AsQueryable(),
                    recipient => recipient.Id,
                    subscribe => subscribe.SubscribeId,
                    (recipient, subscribes) => new { recipient, subscribes })
                    .Select(x => new
                    {
                        x.recipient.Id,
                        x.recipient.FirstName,
                        x.recipient.MiddleName,
                        x.recipient.LastName,
                        Count = x.subscribes.Count()
                    });

            if (request.SortAscendingOnSubscribeDonation)
            {
                entities = entities.OrderBy(x => x.Count);
            }
            else
            {
                entities = entities.OrderByDescending(x => x.Count);
            }


            var recipients = await entities
                            .ProjectToType<GetRecipientQueryDto>()
                            .ToListAsync(cancellationToken);

            return BaseResponse<List<GetRecipientQueryDto>>.Success(recipients);
        }
    }
}
