using Microsoft.AspNetCore.Identity;

namespace CMS.Entities
{
    public class User : IdentityUser
    {
        public string Provider { get; set; } = null!;
    }
}
