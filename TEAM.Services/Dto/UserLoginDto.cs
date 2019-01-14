using Newtonsoft.Json;
using DTO = TEAM.Business.Dto.Base.Dto;

namespace TEAM.Business.Dto
{
    public class UserLoginDto : DTO
    {
        private string _userId;
        [JsonProperty("userid")]
        public string UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                RaisePropertyChanged(nameof(UserId));
            }
        }

        private string _password;
        [JsonProperty("password")]
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
