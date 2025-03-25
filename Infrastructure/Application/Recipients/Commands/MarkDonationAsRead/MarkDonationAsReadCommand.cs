using DataAccess.Interface;
using Gaza_Support.Domains.Dtos.ResponseDtos;
using Gaza_Support.Domains.Models;
using MediatR;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Application.Recipients.Commands.MarkDonationAsRead
{
    public class MarkDonationAsReadCommand:IRequest<BaseResponse<string>>
    {
        public string DonotionId { get; set; }

    }

    internal class MarkDonationAsReadCommandHandler : IRequestHandler<MarkDonationAsReadCommand, BaseResponse<string>>
    {
        private readonly IunitOfWork _unitOfWork;

        public async Task<BaseResponse<string>> Handle(MarkDonationAsReadCommand request, CancellationToken cancellationToken)
        {
            if(!await _unitOfWork.DonationRepo.Collection
                .Find(x=>x.Id==request.DonotionId)
                .AnyAsync(cancellationToken))
            {
                return BaseResponse<string>.Failure("Donotion not found.", HttpStatusCode.NotFound);
            }
            var updateDefinition = Builders<Donation>.Update.Set(x => x.IsRead, true);

            await _unitOfWork.DonationRepo.Collection
                .UpdateOneAsync(x => x.Id == request.DonotionId, updateDefinition, cancellationToken: cancellationToken);

            return BaseResponse<string>.Success();
        }
    }
}
