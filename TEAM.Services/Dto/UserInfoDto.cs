using DTO = TEAM.Business.Dto.Base.Dto;

namespace TEAM.Business.Dto
{
    public class UserInfoDto : DTO
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

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                RaisePropertyChanged(nameof(Email));
            }
        }

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                RaisePropertyChanged(nameof(FirstName));
            }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                RaisePropertyChanged(nameof(LastName));
            }
        }

        private string _gender;
        public string Gender
        {
            get => _gender;
            set
            {
                _gender = value;
                RaisePropertyChanged(nameof(Gender));
            }
        }
    }
}
