using DTO = TEAM.Business.Dto.Base.Dto;

namespace TEAM.Business.Dto
{
    public class UserLoginDto : DTO
    {
        private int _employeeId;
        public int EmployeeId
        {
            get => _employeeId;
            set
            {
                _employeeId = value;
                RaisePropertyChanged(nameof(EmployeeId));
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                RaisePropertyChanged(nameof(Password));
            }
        }
    }
}
