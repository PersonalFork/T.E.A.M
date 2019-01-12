using NEC.TEAM.WebAPI.Models.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NEC.TEAM.WebAPI.Models
{
    public class UserDto : Dto
    {
        private string _fName;
        [JsonProperty("firstName")]
        public string FirstName
        {
            get { return _fName; }
            set
            {
                _fName = value;
                RaisePropertyChanged(nameof(this.FirstName));
            }
        }

        private string _lName;
        [JsonProperty("lastName")]
        public string LastName
        {
            get { return _fName; }
            set
            {
                _fName = value;
                RaisePropertyChanged(nameof(this.LastName));
            }
        }

        private string _tfsId;
        [JsonProperty("tfsUserid")]
        public string TfsId
        {
            get { return _tfsId; }
            set
            {
                _tfsId = value;
                RaisePropertyChanged(nameof(this.TfsId));
            }
        }

        private string _tfsPassword;
        [JsonProperty("tfsPassword")]
        public string TfsPassword
        {
            get { return _tfsPassword; }
            set
            {
                _tfsPassword = value;
                RaisePropertyChanged(nameof(this.TfsPassword));
            }
        }

        private string _emailId;
        [JsonProperty("emailId")]
        public string EmailId
        {
            get { return _emailId; }
            set
            {
                _emailId = value;
                RaisePropertyChanged(nameof(this.EmailId));
            }
        }


    }
}