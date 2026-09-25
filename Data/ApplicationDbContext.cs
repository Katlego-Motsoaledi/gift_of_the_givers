using gift_of_the_givers.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace gift_of_the_givers.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> o) : base(o) { }
    public DbSet<Incident> Incidents => Set<Incident>();
    public DbSet<ReliefCentre> ReliefCentres => Set<ReliefCentre>();
    public DbSet<Resource> Resources => Set<Resource>();
    public DbSet<Volunteer> Volunteers => Set<Volunteer>();
    public DbSet<VolunteerAssignment> VolunteerAssignments => Set<VolunteerAssignment>();
    public DbSet<Donation> Donations => Set<Donation>();
    public DbSet<ReliefUpdate> ReliefUpdates => Set<ReliefUpdate>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);
        b.Entity<ReliefCentre>().HasOne(r => r.Incident).WithMany(i => i.ReliefCentres).HasForeignKey(r => r.IncidentID);
        b.Entity<Resource>().HasOne(r => r.ReliefCentre).WithMany(c => c.Resources).HasForeignKey(r => r.ReliefCentreID);
        b.Entity<VolunteerAssignment>().HasOne(a => a.Volunteer).WithMany().HasForeignKey(a => a.VolunteerID);
        b.Entity<VolunteerAssignment>().HasOne(a => a.Incident).WithMany().HasForeignKey(a => a.IncidentID);
        b.Entity<Donation>().Property(d => d.Amount).HasPrecision(18, 2);
    }


}