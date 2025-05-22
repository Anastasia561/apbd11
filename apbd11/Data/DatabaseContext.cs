using apbd11.Model;
using Microsoft.EntityFrameworkCore;

namespace apbd11.Data;

public class DatabaseContext : DbContext
{
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    protected DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Doctor>().HasData(new List<Doctor>()
        {
            new Doctor() { IdDoctor = 1, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@gmail.com" },
            new Doctor() { IdDoctor = 2, FirstName = "John", LastName = "Doe", Email = "john.doe@gmail.com" }
        });

        modelBuilder.Entity<Patient>().HasData(new List<Patient>()
        {
            new Patient()
                { IdPatient = 1, FirstName = "Ann", LastName = "Smith", Birthdate = DateOnly.Parse("1990-04-03") },
            new Patient()
                { IdPatient = 2, FirstName = "Jane", LastName = "Grand", Birthdate = DateOnly.Parse("1997-04-01") }
        });

        modelBuilder.Entity<Prescription>().HasData(new List<Prescription>()
        {
            new Prescription()
            {
                IdPrescription = 1, Date = DateOnly.Parse("2025-04-03"), DueDate = DateOnly.Parse("2026-04-03"),
                IdDoctor = 1, IdPatient = 1
            },
            new Prescription()
            {
                IdPrescription = 2, Date = DateOnly.Parse("2020-04-03"), DueDate = DateOnly.Parse("2020-04-21"),
                IdDoctor = 1, IdPatient = 2
            }
        });

        modelBuilder.Entity<Medicament>().HasData(new List<Medicament>()
        {
            new Medicament()
                { IdMedicament = 1, Name = "Med1", Description = "Description1", Type = "Pill" },
            new Medicament()
                { IdMedicament = 2, Name = "Med2", Description = "Description2", Type = "Mixture" }
        });

        modelBuilder.Entity<PrescriptionMedicament>().HasData(new List<PrescriptionMedicament>()
        {
            new PrescriptionMedicament()
                { IdMedicament = 1, IdPrescription = 1, Dose = 2, Details = "details1" },
            new PrescriptionMedicament()
                { IdMedicament = 2, IdPrescription = 2, Dose = null, Details = "details2" }
        });
    }
}