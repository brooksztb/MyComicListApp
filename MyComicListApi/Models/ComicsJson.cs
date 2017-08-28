using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyComicListApi.Models;
using Newtonsoft.Json;

namespace MyComicListApi.Models
{
    public class ComicsJson
    {
        [JsonProperty("comics")]
        public List<Comic> Comic { get; set; }
    }
}
