using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CalalogAPI.Models
{
    public class Book
    {
        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "isbn")]
        public string ISBN
        {
            get;
            set;
        }
       
    }
}