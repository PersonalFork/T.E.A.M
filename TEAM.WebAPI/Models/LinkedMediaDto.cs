using NEC.TEAM.WebAPI.Models.Base;
using Newtonsoft.Json;

namespace NEC.TEAM.WebAPI.Models
{
    public enum Direction
    {
        To,
        From,
        Bi
    }

    public class LinkedMediaDto : Dto
    {
        #region Properties.

        private Direction _direction;
        [JsonProperty("direction")]
        public Direction Direction
        {
            get { return _direction; }
            set
            {
                _direction = value;
                RaisePropertyChanged(nameof(this.Direction));

            }
        }



        private string _relation;
        [JsonProperty("relation")]
        public string Relation
        {
            get { return _relation; }
            set { _relation = value; }
        }


        private MediaDto _sourceMedia;
        [JsonProperty("sourceMedia")]
        public MediaDto SourceMedia
        {
            get { return _sourceMedia; }
            set
            {
                _sourceMedia = value;
                RaisePropertyChanged(nameof(this.SourceMedia));
            }
        }

        private MediaDto _linkedMedia;
        [JsonProperty("linkedMedia")]
        public MediaDto LinkedMedia
        {
            get { return _linkedMedia; }
            set
            {
                _linkedMedia = value;
                RaisePropertyChanged(nameof(this.LinkedMedia));
            }
        }

        #endregion

        public LinkedMediaDto()
        {

        }

        public LinkedMediaDto(string relation, MediaDto sourceMedia, MediaDto linkedMedia)
        {
            this.Relation = relation;
            this.SourceMedia = sourceMedia;
            this.LinkedMedia = linkedMedia;
        }
    }
}