using DTO = TEAM.Business.Dto.Base.Dto;

namespace TEAM.Business.Dto
{
    public class UserRegistrationDto : DTO
    {
        private UserLoginDto _loginInfo;
        public UserLoginDto LoginInfo
        {
            get => _loginInfo;
            set
            {
                _loginInfo = value;
                RaisePropertyChanged(nameof(LoginInfo));
            }
        }

        private UserInfoDto _userInfo;
        public UserInfoDto UserInfo
        {
            get => _userInfo;
            set
            {
                _userInfo = value;
                RaisePropertyChanged(nameof(UserInfo));
            }
        }
    }
}