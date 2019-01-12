using NEC.TEAM.WebAPI.Models.Base;
using Newtonsoft.Json;

namespace NEC.TEAM.WebAPI.Models
{

    public class MediaDto : Dto
    {
        private string _caseId;
        [JsonProperty("caseId")]
        public string CaseId
        {
            get { return _caseId; }
            set
            {
                _caseId = value;
                RaisePropertyChanged(nameof(this.CaseId));
            }
        }

        private string _contentType;
        [JsonProperty("contentType")]
        public string ContentType
        {
            get { return _contentType; }
            set
            {
                _contentType = value;
                RaisePropertyChanged(nameof(this.ContentType));
            }
        }


        private string _contentId;
        [JsonProperty("contentId")]
        public string ContentId
        {
            get { return _contentId; }
            set
            {
                _contentId = value;
                RaisePropertyChanged(nameof(this.ContentId));
            }
        }

        private string _name;
        [JsonProperty("name")]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged(nameof(this.Name));
            }
        }

        private string _url;
        [JsonProperty("url")]
        public string URL
        {
            get { return _url; }
            set
            {
                _url = value;
                RaisePropertyChanged(nameof(this.URL));
            }
        }



    }
}