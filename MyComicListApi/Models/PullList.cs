using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyComicListApi.Models
{
    public class PullList
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public List<Comic> Comics { get; set; }
    }
}
