using DataAccess.Interface;
using Gaza_Support.API.Settings;
using Microsoft.Extensions.Options;
using System.Collections;

namespace DataAccess.Repo
{
    public class UnitOfWork : IunitOfWork
    {
        private Hashtable _repositories;
        private readonly MongoDbConfig _dbConfig;

        public UnitOfWork(
            IDonationRepo donationRepo,
            IUserRepo userRepo,
            IOptions<MongoDbConfig> options,
            IDonorRepo donorRepo,
            IRecipientRepo recipientRepo,
            IRoleRepo roleRepo,
            ISubscribeRepo subscribeRepo)
        {
            DonationRepo = donationRepo;
            DonorRepo = donorRepo;
            RecipientRepo = recipientRepo;
            UserRepo = userRepo;
            RoleRepo = roleRepo;
            SubscribeRepo = subscribeRepo;
            _repositories = [];
            _dbConfig = options.Value;
        }

        public IDonationRepo DonationRepo { get; }
        public IUserRepo UserRepo { get; }

        public IDonorRepo DonorRepo { get; }

        public IRecipientRepo RecipientRepo { get; }

        public IRoleRepo RoleRepo { get; }

        public ISubscribeRepo SubscribeRepo { get; }
    }
}
