using Newtonsoft.Json;
using TEAM.Entity;
using DTO = TEAM.Business.Dto.Base.Dto;

namespace TEAM.Business.Dto
{
    public class WorkItemDto : DTO
    {
        #region Public Properties.

        private int _id;
        [JsonProperty("id")]
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                RaisePropertyChanged(nameof(Id));
            }
        }

        private string _userId;
        [JsonProperty("userId")]
        public string UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                RaisePropertyChanged(nameof(UserId));
            }
        }

        private WorkItemState _state;
        [JsonProperty("state")]
        public WorkItemState State
        {
            get => _state;
            set
            {
                _state = value;
                RaisePropertyChanged(nameof(State));
            }
        }

        private int _taskId;
        [JsonProperty("taskId")]
        public int TaskId
        {
            get => _taskId;
            set
            {
                _taskId = value;
                RaisePropertyChanged(nameof(TaskId));
            }
        }

        private int _serverId;
        [JsonProperty("serverId")]
        public int ServerId
        {
            get => _serverId;
            set
            {
                _serverId = value;
                RaisePropertyChanged(nameof(ServerId));
            }
        }

        private int _weekId;
        [JsonProperty("weekId")]
        public int WeekId
        {
            get => _weekId;
            set
            {
                _weekId = value;
                RaisePropertyChanged(nameof(WeekId));
            }
        }

        private string _title;
        [JsonProperty("title")]
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged(nameof(Title));
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                RaisePropertyChanged(nameof(Description));
            }
        }

        private string _status;
        [JsonProperty("status")]
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                RaisePropertyChanged(nameof(Status));
            }
        }

        private string _assignedTo;
        [JsonProperty("assignedTo")]
        public string AssignedTo
        {
            get => _assignedTo;
            set
            {
                _assignedTo = value;
                RaisePropertyChanged(nameof(AssignedTo));
            }
        }

        private string _sprint;
        [JsonProperty("sprint")]
        public string Sprint
        {
            get => _sprint;
            set
            {
                _sprint = value;
                RaisePropertyChanged(nameof(Sprint));
            }
        }

        private string _project;
        [JsonProperty("project")]
        public string Project
        {
            get => _project;
            set
            {
                _project = value;
                RaisePropertyChanged(nameof(Project));
            }
        }

        private string _startDate;
        [JsonProperty("startDate")]
        public string StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                RaisePropertyChanged(nameof(StartDate));
            }
        }

        private string _endDate;
        [JsonProperty("endDate")]
        public string EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                RaisePropertyChanged(nameof(EndDate));
            }
        }

        private string _eta;
        [JsonProperty("eta")]
        public string ETA
        {
            get => _eta;
            set
            {
                _eta = value;
                RaisePropertyChanged(nameof(ETA));
            }
        }

        private double _estimatedHours;
        [JsonProperty("estimatedHours")]
        public double EstimatedHours
        {
            get => _estimatedHours;
            set
            {
                _estimatedHours = value;
                RaisePropertyChanged(nameof(EstimatedHours));
            }
        }

        private double _weekHours;
        [JsonProperty("weekHours")]
        public double WeekHours
        {
            get => _weekHours;
            set
            {
                _weekHours = value;
                RaisePropertyChanged(nameof(WeekHours));
            }
        }

        private int _totalHours;
        [JsonProperty("totalHours")]
        public int TotalHours
        {
            get => _totalHours;
            set
            {
                _totalHours = value;
                RaisePropertyChanged(nameof(TotalHours));
            }
        }

        private string _comments;
        public string Comments
        {
            get => _comments;
            set
            {
                _comments = value;
                RaisePropertyChanged(nameof(Comments));
            }
        }

        #endregion

        #region Public Method Declarations.

        public bool Equals(WorkItemDto dto, out bool isUpdated)
        {
            isUpdated = false;
            if (dto == null)
            {
                return false;
            }

            if (dto.TaskId != TaskId)
            {
                return false;
            }

            if (dto.Title != Title)
            {
                isUpdated = true;
                return false;
            }

            if (dto.Sprint != Sprint)
            {
                isUpdated = true;
                return false;
            }

            if (dto.Description != Description)
            {
                isUpdated = true;
                return false;
            }

            if (dto.Project != Project)
            {
                isUpdated = true;
                return false;
            }
            return false;
        }

        #endregion
    }
}