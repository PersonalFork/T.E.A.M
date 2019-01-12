using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace NEC.TEAM.WebAPI.Models.Base
{
    public class Dto : INotifyPropertyChanged
    {
        private static Int64 _uId = 1001;

        #region Properties

        private Int64 _serialVersionUID;
        [JsonProperty("uId")]
        public Int64 SerialVersionUID
        {
            get { return _serialVersionUID; }
            set { _serialVersionUID = value; }
        }

        #endregion

        #region Constructor

        protected Dto()
        {
            this._serialVersionUID = _uId++;
        }

        #endregion

        #region INotifyPropertyChanged Implementation.

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        #endregion
    }
}