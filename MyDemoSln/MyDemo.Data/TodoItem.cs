using System;
using Newtonsoft.Json;

namespace MyDemo.Data
{
    public class TodoItem : EntityBase
    {
        [JsonProperty(PropertyName = "Text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "Complete")]
        public bool Complete { get; set; }
    }
}

