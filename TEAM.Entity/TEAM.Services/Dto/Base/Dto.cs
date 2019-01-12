using System;
using System.ComponentModel;

namespace TEAM.Business.Dto.Base
{
    public abstract class Dto : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Unique Identifier for DTP.
        /// </summary>
        public string UId { get; private set; }

        public Dto()
        {
            Guid guid = Guid.NewGuid();
            UId = guid.ToString();
        }

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
