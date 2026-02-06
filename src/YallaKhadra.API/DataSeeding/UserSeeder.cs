using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using YallaKhadra.Core.Entities;
using YallaKhadra.Core.Entities.IdentityEntities;

namespace YallaKhadra.API.DataSeeding {
    public static class UserSeeder {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager) {
            int usersInDb = await userManager.Users.CountAsync();
            if (usersInDb > 0)
                return;

            string usersJson = await File.ReadAllTextAsync("DataSeeding/Users.json");
            List<UserSeedData>? seedData = JsonSerializer.Deserialize<List<UserSeedData>>(usersJson);

            if (seedData is null || seedData.Count == 0)
                return;

            foreach (var data in seedData) {
                var domainUser = new DomainUser {
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    Email = data.Email,
                    PhoneNumber = data.PhoneNumber,
                    Address = data.Address,
                    CreatedAt = DateTime.UtcNow
                };

                var applicationUser = new ApplicationUser {
                    UserName = data.Email,
                    Email = data.Email,
                    PhoneNumber = data.PhoneNumber,
                    EmailConfirmed = true,
                    DomainUser = domainUser
                };

                var result = await userManager.CreateAsync(applicationUser, data.Password ?? "P@ssw0rd");

                if (result.Succeeded) {
                    await userManager.AddToRoleAsync(applicationUser, data.Role ?? "User");
                }
            }
        }

        private class UserSeedData {
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string PhoneNumber { get; set; } = string.Empty;
            public string? Address { get; set; }
            public string? Password { get; set; }
            public string? Role { get; set; }
        }
    }
}
