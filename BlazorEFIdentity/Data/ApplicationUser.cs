using BlazorEFIdentity.Models;
using Microsoft.AspNetCore.Identity;

namespace BlazorEFIdentity.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string? Handle { get; set; }
        public string? SocialSecurityNr { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public List<Account> Accounts { get; set; }
    }
}
