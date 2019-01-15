using System.Collections.Generic;
using Tapioca.HATEOAS;

namespace PersonApi.Data.VO
{
    public class UserVO : ISupportsHyperMedia
    {

        public long? Id { get; set; }
        public string Login { get; set; }
        public string AccessKey { get; set; }

        public List<HyperMediaLink> Links { get; set; } = new List<HyperMediaLink>();
    }
}