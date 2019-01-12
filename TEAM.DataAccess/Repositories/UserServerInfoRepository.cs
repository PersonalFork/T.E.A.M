using System;
using System.Linq;
using System.Linq.Expressions;
using TEAM.DAL.Repositories.Base;
using TEAM.Entity;

namespace TEAM.DAL.Repositories
{
    public class UserServerInfoRepository : Repository<UserServerInfo>
    {
        public UserServerInfoRepository() : base(DbHelper.GetDatabase)
        {
        }
    }
}
