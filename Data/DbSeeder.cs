using gift_of_the_givers.Models;
using Microsoft.AspNetCore.Identity;

namespace gift_of_the_givers.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider sp)
    {
        var db = sp.GetRequiredService<ApplicationDbContext>();
        object value = await db.Database.EnsureCreatedAsync();
        var roles = sp.GetRequiredService<RoleManager<IdentityRole>>();
        var users = sp.GetRequiredService<UserManager<IdentityUser>>();

        foreach (var r in new[] { "Employee", "Donor" })
            if (!await roles.RoleExistsAsync(r)) await roles.CreateAsync(new IdentityRole(r));

        async Task Ensure(string email, string role)
        {
            if (await users.FindByEmailAsync(email) is null)
            {
                var u = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
                await users.CreateAsync(u, "Gotg#2026!");
                await users.AddToRoleAsync(u, role);
            }
        }
        await Ensure("employee@giftofthegivers.org", "Employee");
        await Ensure("donor@giftofthegivers.org", "Donor");

        if (!db.Incidents.Any())
        {
            var kzn = new Incident { IncidentName = "KwaZulu-Natal Floods", IncidentType = "Flood", Location = "Durban, KZN", StartDate = new DateTime(2026, 2, 14), Status = "Active" };
            var fire = new Incident { IncidentName = "Khayelitsha Settlement Fire", IncidentType = "Fire", Location = "Cape Town, WC", StartDate = new DateTime(2026, 5, 2), Status = "Monitoring" };
            var drt = new Incident { IncidentName = "Eastern Cape Drought Relief", IncidentType = "Drought", Location = "Gqeberha, EC", StartDate = new DateTime(2025, 11, 20), Status = "Resolved" };
            db.Incidents.AddRange(kzn, fire, drt);
            var h1 = new ReliefCentre { CentreName = "Durban Civic Centre Hub", Location = "Durban, KZN", Capacity = 1200, Incident = kzn };
            var h2 = new ReliefCentre { CentreName = "Khayelitsha Distribution Point", Location = "Cape Town, WC", Capacity = 450, Incident = fire };
            db.ReliefCentres.AddRange(h1, h2);
            db.Resources.AddRange(
                new Resource { ResourceType = "Bottled Water (L)", Quantity = 8500, ReliefCentre = h1 },
                new Resource { ResourceType = "Meal Packs", Quantity = 4200, ReliefCentre = h1 },
                new Resource { ResourceType = "Blankets", Quantity = 1500, ReliefCentre = h2 });
            var v1 = new Volunteer { FirstName = "Naledi", LastName = "Khumalo", Email = "naledi@example.org", Phone = "082 555 0134", SkillSet = "First Aid, Logistics", Availability = "Weekends" };
            var v2 = new Volunteer { FirstName = "Sipho", LastName = "Dlamini", Email = "sipho@example.org", Phone = "083 555 0177", SkillSet = "Driving, Translation", Availability = "On-call" };
            db.Volunteers.AddRange(v1, v2);
            db.VolunteerAssignments.AddRange(
                new VolunteerAssignment { Volunteer = v1, Incident = kzn, RoleAssigned = "Medic Assistant" },
                new VolunteerAssignment { Volunteer = v2, Incident = fire, RoleAssigned = "Logistics Driver" });
            await db.SaveChangesAsync();
        }
    }
}