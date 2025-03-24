using DataAccess.Interface;
using Gaza_Support.Domains.Contstants;
using Gaza_Support.Domains.Models;
using Infrastructure.IServices;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gaza_Support.API.Seeding
{
    public class DataSeeding
    {
        public static async Task Invoke(IServiceProvider service)
        {
            var unitOfWork = service.GetRequiredService<IunitOfWork>();
            var authServices = service.GetRequiredService<IAuthService>();

            if (await unitOfWork.RoleRepo.Collection.CountDocumentsAsync(x => true) == 0)
            {
                var roles = new List<Role>
                {
                    new Role
                    {
                        Name = Roles.Admin,
                        Id = ObjectId.GenerateNewId().ToString()
                    },
                    new Role
                    {
                        Name = Roles.Recipient,
                        Id = ObjectId.GenerateNewId().ToString()
                    },
                    new Role
                    {
                        Name = Roles.Donor,
                        Id = ObjectId.GenerateNewId().ToString()
                    }
                };

                await unitOfWork.RoleRepo.Collection.InsertManyAsync(roles);
            }

            if (await unitOfWork.UserRepo.Collection.CountDocumentsAsync(x => true) == 0)
            {

                var roleId = await unitOfWork.RoleRepo.Collection
                            .Find(x => x.Name == Roles.Admin)
                            .Project(x => x.Id)
                            .FirstAsync();

                var user = new User()
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    Email = "admin@gmail.com",
                    RoleId = roleId
                };

                authServices.CreatePasswordHash(user, "123@Abc");

                await unitOfWork.UserRepo.AddAsync(user);
            }

        }
    }
}
