using System.Linq;
using LibraryApp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryApp.Data
{
    public class DbSeed
    {
        public static async void Seed(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();      
                if (context.Users.Any()) 
                    return;

                string[] roles = new string[] { "Admin", "Librarian", "Reader"};
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<Role>>();
                foreach (string role in roles)
                {
                    await roleManager.CreateAsync(new Role(role));
                }

                var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
                
                var admin = new User()
                {
                    Email = "Admin01@admin.com",
                    UserName = "Admin01@admin.com",
                    Name = "Admin",
                    Surname = "Admin",
                    PersonalNumber = "000000-00000",
                    RFID = 0
                };
                
                
                var result = await userManager.CreateAsync(admin,"Admin01@admin.com");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }

                await context.SaveChangesAsync();
            }
        }
    }
}