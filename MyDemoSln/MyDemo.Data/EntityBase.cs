using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MyDemo.Data
{
    public abstract class EntityBase: INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsDirty { get; set; }

        protected virtual void OnPropertyChanged([CallerMemberName] String propName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propName));
            }
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        //Only raise event if value is changed.
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }
                
            storage = value;
            OnPropertyChanged(propertyName);
            return true;

        }

        #endregion

        #region AzureMobileServicesRequiredFields

        private bool _deleted;

        //[JsonProperty (PropertyName = "_id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "__version")]
        [Timestamp]
        public byte[] Version { get; set; }
        // Version

        [JsonProperty(PropertyName = "_createdAt")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTimeOffset CreatedAt { get; set; }
        // CreatedAt

        [JsonProperty(PropertyName = "_updateAt")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? UpdatedAt { get; set; }
        // UpdatedAt
        public bool Deleted
        {
            get { return _deleted; }
            set { SetProperty(ref _deleted, value); }
        }
        // Deleted

        #endregion

        #region BaseProperties

        [JsonProperty(PropertyName = "CreatedBy")]
        public string CreatedBy { get; set; }

        [JsonProperty(PropertyName = "UpdatedBy")]
        public string UpdatedBy { get; set; }


        [JsonProperty(PropertyName = "UpdatedOnDeviceToken")]
        public string UpdatedOnDeviceToken { get; set; }

        #endregion
    }
}

