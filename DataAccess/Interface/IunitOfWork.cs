namespace DataAccess.Interface
{
    public interface IunitOfWork
    {
        IDonationRepo DonationRepo { get; }
        IDonorRepo DonorRepo { get; }
        IRecipientRepo RecipientRepo { get; }
        IUserRepo UserRepo { get; }
        IRoleRepo RoleRepo { get; }
        ISubscribeRepo SubscribeRepo { get; }
    }
}
