using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyComicListApi.Models
{
    public class ApplicationUser
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ComicShop { get; set; }
        public string ListId { get; set; }
    }
}
