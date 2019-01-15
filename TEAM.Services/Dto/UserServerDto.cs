using Newtonsoft.Json;

namespace TEAM.Business.Dto
{
    public class UserServerDto : Base.Dto
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

        private int _tfsId;
        [JsonProperty("tfsId")]
        public int TfsId
        {
            get => _tfsId;
            set
            {
                _tfsId = value;
                RaisePropertyChanged(nameof(TfsId));
            }
        }

        private string _serverName;
        [JsonProperty("serverName")]
        public string ServerName
        {
            get => _serverName;
            set
            {
                _serverName = value;
                RaisePropertyChanged(nameof(ServerName));
            }
        }

        private string _url;
        [JsonProperty("serverUrl")]
        public string ServerUrl
        {
            get => _url;
            set
            {
                _url = value;
                RaisePropertyChanged(nameof(ServerUrl));
            }
        }


    }
}
