using System.ComponentModel.DataAnnotations;

namespace gift_of_the_givers.Models;

public class Incident
{
    public int IncidentID { get; set; }
    [Required, StringLength(150)] public string IncidentName { get; set; } = "";
    [Required, StringLength(100)] public string IncidentType { get; set; } = "";
    [Required, StringLength(255)] public string Location { get; set; } = "";
    public DateTime StartDate { get; set; }
    [Required, StringLength(50)] public string Status { get; set; } = "Active";
    public List<ReliefCentre> ReliefCentres { get; set; } = new();
}
public class ReliefCentre
{
    public int ReliefCentreID { get; set; }
    [Required, StringLength(150)] public string CentreName { get; set; } = "";
    [Required, StringLength(255)] public string Location { get; set; } = "";
    public int Capacity { get; set; }
    public int IncidentID { get; set; }
    public Incident Incident { get; set; } = null!;
    public List<Resource> Resources { get; set; } = new();
}
public class Resource
{
    public int ResourceID { get; set; }
    [Required, StringLength(100)] public string ResourceType { get; set; } = "";
    public int Quantity { get; set; }
    public int ReliefCentreID { get; set; }
    public ReliefCentre ReliefCentre { get; set; } = null!;
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}
public class Volunteer
{
    public int VolunteerID { get; set; }
    [Required, StringLength(100)] public string FirstName { get; set; } = "";
    [Required, StringLength(100)] public string LastName { get; set; } = "";
    [Required, EmailAddress, StringLength(150)] public string Email { get; set; } = "";
    [Required, Phone, StringLength(20)] public string Phone { get; set; } = "";
    [Required, StringLength(255)] public string SkillSet { get; set; } = "";
    [Required, StringLength(100)] public string Availability { get; set; } = "";
    public DateTime RegisteredDate { get; set; } = DateTime.UtcNow;
    [StringLength(50)] public string Status { get; set; } = "Available";
}
public class VolunteerAssignment
{
    [Key]
    public int AssignmentID { get; set; }
    public int VolunteerID { get; set; }
    public Volunteer Volunteer { get; set; } = null!;
    public int IncidentID { get; set; }
    public Incident Incident { get; set; } = null!;
    [Required, StringLength(100)] public string RoleAssigned { get; set; } = "";
    public DateTime AssignmentDate { get; set; } = DateTime.UtcNow;
}
public class Donation
{
    public int DonationID { get; set; }
    [StringLength(100)] public string DonorName { get; set; } = "Anonymous";
    public bool IsAnonymous { get; set; }
    [Range(1, 100000)] public decimal Amount { get; set; }
    [StringLength(3)] public string Currency { get; set; } = "ZAR";
    public bool IsRecurring { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string CertificateNumber => $"GOTG-18A-{DonationID:D5}";
}
public class ReliefUpdate
{
    [Key]
    public int UpdateID { get; set; }
    public int IncidentID { get; set; }
    public Incident Incident { get; set; } = null!;
    [Required, StringLength(500)] public string Message { get; set; } = "";
    public string PostedBy { get; set; } = "";
    public DateTime PostedAt { get; set; } = DateTime.UtcNow;
}