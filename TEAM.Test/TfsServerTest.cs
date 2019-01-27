using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;

using TEAM.Business;
using TEAM.Business.Base;
using TEAM.DAL.Repositories;
using TEAM.Entity;

namespace TEAM.Test
{
    [TestClass]
    public class TfsServerTest
    {
        private readonly ITeamWorkItemService _teamWorkItemService;

        public TfsServerTest()
        {
            _teamWorkItemService = new TfsTeamWorkItemService();
        }
        // Assumes that in current culture week starts on Sunday
        public int GetWeekOfMonth(DateTimeOffset time)
        {
            DateTimeOffset first = new DateTimeOffset(time.Year, time.Month, 1, 0, 0, 0, time.Offset);
            int firstSunday = (7 - (int)first.DayOfWeek) % 7 + 1;
            int weekOfMonth = 1 + (time.Day + 7 - firstSunday) / 7;

            return weekOfMonth;
        }

        [TestMethod]
        public void GetWeekOfMonth()
        {
            DateTime today = DateTime.Today.AddDays(28);
            int weekOfMonth = GetWeekOfMonth(today);
        }

        [TestMethod]
        public void PopulateWeek()
        {
            DateTime startDate = DateTime.Today.Date.AddDays(-14);
            DateTime endDate = startDate.AddDays(6);
            using (WeekInfoRepository repository = new WeekInfoRepository())
            {
                for (int i = 0; i < 200; i++)
                {
                    WeekInfo weekInfo = new WeekInfo
                    {
                        StartDate = startDate,
                        EndDate = endDate
                    };
                    string label = string.Empty;
                    label = startDate.ToString("MMM") + "-Week " + GetWeekOfMonth(startDate);

                    if (startDate.Month != endDate.Month)
                    {
                        label += " / " + endDate.ToString("MMM") + "-Week 1";
                    }
                    weekInfo.Label = label;
                    //string label = startDate.
                    repository.Insert(weekInfo);
                    startDate = startDate.AddDays(7);
                    endDate = endDate.AddDays(7);
                }
            }
        }

        #region Get Work Items.
        [TestMethod]
        public void GetValidItemById()
        {
            IWorkItemSyncService service = new WorkItemSyncService(_teamWorkItemService);
            Business.Dto.WorkItemDto obj = service.GetWorkItemById(200052, 1, "1111111");
            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void GetUserIncompleteItems()
        {
            IWorkItemSyncService service = new WorkItemSyncService(_teamWorkItemService);
            List<Business.Dto.WorkItemDto> obj = service.GetUserIncompleteSyncedTasks(1, "1111111");
            Assert.IsTrue(obj.Count > 0);
        }

        [TestMethod]
        public void GetInvalidItemById()
        {
            IWorkItemSyncService service = new WorkItemSyncService(_teamWorkItemService);
            Business.Dto.WorkItemDto obj = service.GetWorkItemById(1913745, 5, "1111111");
            Assert.IsNull(obj);
        }
        #endregion

        [TestMethod]
        public void AddTfsServer()
        {
            TfsTeamWorkItemService service = new TfsTeamWorkItemService();
            int id = service.AddTeamServer("TFS OLD", Settings.tfsUrl, 8080);
            Assert.IsTrue(id != 0);
        }

        [TestMethod]
        public void GetCurrentWeekTasks()
        {
            IWorkItemSyncService userWorkItemManagementService = new WorkItemSyncService(_teamWorkItemService);
            List<Business.Dto.WorkItemDto> currentWeekTasks = userWorkItemManagementService.GetUserCurrentWeekSyncedTasks("1111111");
            Assert.IsTrue(currentWeekTasks.Count > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ReAddTfsServer()
        {
            TfsTeamWorkItemService service = new TfsTeamWorkItemService();
            int id = service.AddTeamServer("TFS OLD", Settings.tfsUrl, 8080);
            Assert.IsTrue(id != 0);
        }

        [TestMethod]
        public void RemoveTfsServer()
        {
            TfsTeamWorkItemService service = new TfsTeamWorkItemService();
            int code = service.DeleteTeamServer(2);
            Assert.IsTrue(code != 0);
        }
    }
}
