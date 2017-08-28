using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyComicListApi.Models
{
    public class Comic
    {
        [JsonProperty("publisher")]
        public string Publisher { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("price")]
        public string Price { get; set; }
        [JsonProperty("creators")]
        public string Creators { get; set; }
        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }
        [JsonProperty("diamond_id")]
        public string DiamondId { get; set; }
    }
}
