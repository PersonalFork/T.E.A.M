using Newtonsoft.Json;
using DTO = TEAM.Business.Dto.Base.Dto;
namespace TEAM.Business.Dto
{
    public class WorkItemDto : DTO
    {
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

        private string _path;
        [JsonProperty("path")]
        public string Path
        {
            get => _path;
            set
            {
                _path = value;
                RaisePropertyChanged(nameof(Path));
            }
        }

        private string _iteration;
        [JsonProperty("sprint")]
        public string Iteration
        {
            get => _iteration;
            set
            {
                _iteration = value;
                RaisePropertyChanged(nameof(Iteration));
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

        private double _completedHours;
        [JsonProperty("completedHours")]
        public double CompletedHours
        {
            get => _completedHours;
            set
            {
                _completedHours = value;
                RaisePropertyChanged(nameof(CompletedHours));
            }
        }

        private double _remainingHours;
        [JsonProperty("remainingHours")]
        public double RemainingHours
        {
            get => _remainingHours;
            set
            {
                _remainingHours = value;
                RaisePropertyChanged(nameof(RemainingHours));
            }
        }

        private string _year;
        [JsonProperty("year")]
        public string Year
        {
            get => _year;
            set
            {
                _year = value;
                RaisePropertyChanged(nameof(Year));
            }
        }

        private string _month;
        [JsonProperty("month")]
        public string Month
        {
            get => _month;
            set
            {
                _month = value;
                RaisePropertyChanged(nameof(Month));
            }
        }

        private int _weekNumber;
        [JsonProperty("week")]
        public int WeekNumber
        {
            get => _weekNumber;
            set
            {
                _weekNumber = value;
                RaisePropertyChanged(nameof(WeekNumber));
            }
        }
    }
}