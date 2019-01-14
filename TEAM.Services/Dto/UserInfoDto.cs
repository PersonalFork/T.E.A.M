using Newtonsoft.Json;

using DTO = TEAM.Business.Dto.Base.Dto;

namespace TEAM.Business.Dto
{
    public class UserInfoDto : DTO
    {
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

        private string _email;
        [JsonProperty("email")]
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
        [JsonProperty("firstName")]
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
        [JsonProperty("lastName")]
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
        [JsonProperty("gender")]
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
