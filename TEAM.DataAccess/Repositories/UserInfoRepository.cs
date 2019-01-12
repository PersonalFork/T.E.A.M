using TEAM.DAL.Repositories.Base;
using TEAM.Entity;

namespace TEAM.DAL.Repositories
{
    public class UserInfoRepository : Repository<UserInfo>
    {
        public UserInfoRepository() : base(DbHelper.GetDatabase)
        {
        }
    }
}
