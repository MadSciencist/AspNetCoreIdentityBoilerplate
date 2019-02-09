using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityBoilerplate.Models
{
    public class AppRole : IdentityRole<int>
    {
        public AppRole(string name) : base(name)
        {
        }
    }
}
