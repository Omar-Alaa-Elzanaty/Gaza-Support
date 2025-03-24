using Gaza_Support.Domains.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interface
{
    public interface IRoleRepo:IBaseRepo<Role>
    {
        Task<Role?> FindOneByAsync(string id);
    }
}
