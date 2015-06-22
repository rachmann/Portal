 using DapperExtensions;
 using Portal.Identity.DapperExtensions;
 using Microsoft.AspNet.Identity;

namespace Portal.Identity.Models
{
    public class PortalRole : IRole<int>
    {
        // Id is key by default
        // [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
