using System;
using System.Data.Entity;
using TEAM.DAL.Repositories.Base;
using TEAM.Entity;

namespace TEAM.DAL.Repositories
{
    public class WeekInfoRepository : Repository<WeekInfo>
    {
        public WeekInfoRepository() : base(DbHelper.GetDatabase)
        {
        }

        public WeekInfo GetCurrentWeekInfo()
        {
            // For MySql the below query needs to be executed in the database.
            //CREATE FUNCTION `TruncateTime`(dateValue DateTime) RETURNS date
            //    return Date(dateValue)

            WeekInfo currentWeek = FindLocal(
                    x => DbFunctions.TruncateTime(x.StartDate) <= DateTime.Today
                    && DbFunctions.TruncateTime(x.EndDate) >= DateTime.Today
                    );
            return currentWeek;
        }
    }
}
