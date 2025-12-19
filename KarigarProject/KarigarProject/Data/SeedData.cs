using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Karigar.Models;

namespace Karigar.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

                // Create roles if they don't exist
                string[] roles = { "Admin", "Customer", "ServiceProvider" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                // Create admin user
                var adminEmail = "admin@karigar.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    adminUser = new IdentityUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(adminUser, "Admin@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");

                        // Create admin profile
                        context.UserProfiles.Add(new UserProfile
                        {
                            UserId = adminUser.Id,
                            FullName = "Administrator",
                            Email = adminEmail,
                            PhoneNumber = "+91 9876543000"
                        });
                        await context.SaveChangesAsync();
                    }
                }

                // Create sample customer
                var customerEmail = "customer@karigar.com";
                var customerUser = await userManager.FindByEmailAsync(customerEmail);
                if (customerUser == null)
                {
                    customerUser = new IdentityUser
                    {
                        UserName = customerEmail,
                        Email = customerEmail,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(customerUser, "Customer@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(customerUser, "Customer");

                        // Create customer profile
                        context.UserProfiles.Add(new UserProfile
                        {
                            UserId = customerUser.Id,
                            FullName = "Rahul Sharma",
                            Email = customerEmail,
                            PhoneNumber = "+91 9876543200",
                            Address = "123 Main Street",
                            City = "Mumbai",
                            State = "Maharashtra",
                            Pincode = "400001"
                        });
                        await context.SaveChangesAsync();
                    }
                }

                // Create sample service provider
                var providerEmail = "provider@karigar.com";
                var providerUser = await userManager.FindByEmailAsync(providerEmail);
                if (providerUser == null)
                {
                    providerUser = new IdentityUser
                    {
                        UserName = providerEmail,
                        Email = providerEmail,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(providerUser, "Provider@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(providerUser, "ServiceProvider");

                        // Create provider profile
                        context.UserProfiles.Add(new UserProfile
                        {
                            UserId = providerUser.Id,
                            FullName = "AquaFix Plumbers",
                            Email = providerEmail,
                            PhoneNumber = "+91 9876543210",
                            Address = "456 Service Lane",
                            City = "Mumbai",
                            State = "Maharashtra",
                            Pincode = "400002",
                            Skills = "Plumbing, Pipe Repair, Leakage Fixing",
                            Experience = "10+ years",
                            Certifications = "Certified Plumber, ISO Certified"
                        });
                        await context.SaveChangesAsync();
                    }
                }

                // Now seed services AFTER users are created
                if (!context.Services.Any())
                {
                    var services = new List<Service>
                    {
                        new Service
                        {
                            Title = "Professional Plumbing Services",
                            Description = "Expert plumbing solutions for homes and offices including pipe repair, leakage fixing, and installation of fixtures.",
                            Category = "Plumbing",
                            Price = 1500,
                            Location = "Mumbai",
                            ProviderId = providerUser?.Id,
                            ProviderName = "AquaFix Plumbers",
                            ProviderPhone = "+91 9876543210",
                            Rating = 4.5,
                            ReviewCount = 25,
                            Availability = "Mon-Sun, 8AM-8PM",
                            CreatedAt = DateTime.UtcNow.AddDays(-30)
                        },
                        new Service
                        {
                            Title = "Electrical Repair & Installation",
                            Description = "Certified electrician for all electrical needs including wiring, switchboard repair, and appliance installation.",
                            Category = "Electrical",
                            Price = 1200,
                            Location = "Delhi",
                            ProviderId = providerUser?.Id,
                            ProviderName = "Spark Electricians",
                            ProviderPhone = "+91 9876543211",
                            Rating = 4.2,
                            ReviewCount = 18,
                            Availability = "Mon-Sat, 9AM-7PM",
                            CreatedAt = DateTime.UtcNow.AddDays(-25)
                        },
                        new Service
                        {
                            Title = "Home Cleaning & Deep Cleaning",
                            Description = "Professional cleaning services for homes and offices with trained staff and eco-friendly products.",
                            Category = "Cleaning",
                            Price = 2000,
                            Location = "Bangalore",
                            ProviderId = providerUser?.Id,
                            ProviderName = "CleanMaster Services",
                            ProviderPhone = "+91 9876543212",
                            Rating = 4.7,
                            ReviewCount = 42,
                            Availability = "Mon-Sun, 7AM-9PM",
                            CreatedAt = DateTime.UtcNow.AddDays(-20)
                        },
                        new Service
                        {
                            Title = "Math & Science Tutoring",
                            Description = "Experienced tutor for CBSE/ICSE syllabus from classes 6-12 with flexible timing options.",
                            Category = "Tutoring",
                            Price = 500,
                            Location = "Kolkata",
                            ProviderName = "EduCare Tutors",
                            ProviderPhone = "+91 9876543213",
                            Rating = 4.8,
                            ReviewCount = 31,
                            Availability = "Mon-Sat, 3PM-8PM",
                            CreatedAt = DateTime.UtcNow.AddDays(-15)
                        },
                        new Service
                        {
                            Title = "AC Repair & Service",
                            Description = "Expert AC technicians for all brands including installation, gas filling, and annual maintenance contracts.",
                            Category = "AC Repair",
                            Price = 800,
                            Location = "Hyderabad",
                            ProviderName = "CoolBreeze AC Services",
                            ProviderPhone = "+91 9876543214",
                            Rating = 4.3,
                            ReviewCount = 56,
                            Availability = "Mon-Sun, 8AM-10PM",
                            CreatedAt = DateTime.UtcNow.AddDays(-10)
                        }
                    };

                    context.Services.AddRange(services);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}