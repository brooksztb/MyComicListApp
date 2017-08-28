using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyComicListApi
{
    public class PullListDbSettings
    {
        public string DatabaseId { get; set; }
        public string CollectionId { get; set; }
        public string ApplicationUserCollectionId { get; set; }
        public string EndpointUrl { get; set; }
        public string AuthorizationKey { get; set; }
    }
}
