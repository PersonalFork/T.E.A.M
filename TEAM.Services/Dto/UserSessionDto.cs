using DTO = TEAM.Business.Dto.Base.Dto;

namespace TEAM.Business.Dto
{
    public class UserSessionDto : DTO
    {
        public string SessionId { get; set; }
        public UserInfoDto User { get; set; }
    }
}