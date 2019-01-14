using Newtonsoft.Json;
using DTO = TEAM.Business.Dto.Base.Dto;

namespace TEAM.Business.Dto
{
    public class UserSessionDto : DTO
    {
        [JsonProperty("sessionId")]
        public string SessionId { get; set; }

        [JsonProperty("user")]
        public UserInfoDto User { get; set; }
    }
}