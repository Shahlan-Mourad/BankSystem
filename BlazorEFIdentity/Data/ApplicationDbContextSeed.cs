using BlazorEFIdentity.Data;
using Microsoft.AspNetCore.Identity;

public class ApplicationDbContextSeed
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        // Skapa roller om de inte finns
        var roles = new[] { "Admin", "User", "Manager" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Skapa administratörsanvändaren om den inte finns
        string adminEmail = "admin@admin.se";
        string adminPassword = "Abc123!";

        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail
            };
            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        
    }
}

