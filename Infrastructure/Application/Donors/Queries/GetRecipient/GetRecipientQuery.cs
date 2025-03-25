using DataAccess.Interface;
using Gaza_Support.Domains.Contstants;
using Gaza_Support.Domains.Dtos.ResponseDtos;
using Infrastructure.Extensions;
using Mapster;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Application.Donors.Queries.GetRecipient
{
    public class GetRecipientQuery : IRequest<PaginatedResponse<GetRecipientQueryDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public bool SortAscendingOnSubscribeDonation { get; set; }
    }

    internal class GetRecipientQueryHandler : IRequestHandler<GetRecipientQuery, PaginatedResponse<GetRecipientQueryDto>>
    {
        private readonly IunitOfWork _unitOfWork;

        public GetRecipientQueryHandler(IunitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResponse<GetRecipientQueryDto>> Handle(GetRecipientQuery query, CancellationToken cancellationToken)
        {
            var roleId = await _unitOfWork.RoleRepo.Collection
                        .Find(x => x.Name == Roles.Recipient)
                        .Project(x => x.Id)
                        .FirstOrDefaultAsync(cancellationToken);

            if (roleId == null)
            {
                return PaginatedResponse<GetRecipientQueryDto>.Failure("Role not found.");
            }

            var entities = _unitOfWork.RecipientRepo.Collection.AsQueryable()
                .Where(x => x.RoleId == roleId && x.IsVerified)
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

            if (query.SortAscendingOnSubscribeDonation)
            {
                entities = entities.OrderBy(x => x.Count);
            }
            else
            {
                entities = entities.OrderByDescending(x => x.Count);
            }


            var recipients = await entities
                            .ProjectToType<GetRecipientQueryDto>()
                            .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);

            return recipients;
        }
    }
}
