using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LibraryApp.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class Role : IdentityRole
    {
        public Role() : base(){}
        public Role(string roleName) : base(roleName){}
    }
}