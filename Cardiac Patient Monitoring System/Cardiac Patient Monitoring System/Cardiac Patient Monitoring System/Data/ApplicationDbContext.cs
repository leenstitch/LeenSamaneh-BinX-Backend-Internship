// This DbContext connects the application to the database.
// It defines the project entities and configures their relationships,
// foreign keys, delete behaviors, and unique constraints.

using System.Collections.Generic;
using System.Reflection.Emit;
using Cardiac_Patient_Monitoring_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cardiac_Patient_Monitoring_System.Data
{
    public class ApplicationDbContext :
        IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<EmergencyContact> EmergencyContacts { get; set; }

        public DbSet<Diagnosis> Diagnoses { get; set; }

        public DbSet<VitalSign> VitalSigns { get; set; }

        public DbSet<Medication> Medications { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        // Configures entity relationships, foreign keys,
        // delete behaviors, and database constraints.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configures the one-to-one relationship between
            // Patient and ApplicationUser.
            modelBuilder.Entity<Patient>()
              .HasOne(p => p.User)
              .WithOne(u => u.Patient)
              .HasForeignKey<Patient>(p => p.UserId)
              .OnDelete(DeleteBehavior.Restrict);

            // Configures the one-to-many relationship between
            // Patient and EmergencyContact.
            modelBuilder.Entity<EmergencyContact>()
             .HasOne(ec => ec.Patient)
            .WithMany(p => p.EmergencyContacts)
            .HasForeignKey(ec => ec.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

            // Configures the one-to-many relationship between
            // Patient and Diagnosis.
            modelBuilder.Entity<Diagnosis>()
              .HasOne(d => d.Patient)
              .WithMany(p => p.Diagnoses)
              .HasForeignKey(d => d.PatientId)
              .OnDelete(DeleteBehavior.Restrict);

            // Configures the one-to-many relationship between
            // Patient and VitalSign.
            modelBuilder.Entity<VitalSign>()
              .HasOne(v => v.Patient)
              .WithMany(p => p.VitalSigns)
              .HasForeignKey(v => v.PatientId)
              .OnDelete(DeleteBehavior.Restrict);

            // Configures the one-to-many relationship between
            // Patient and Medication.
            modelBuilder.Entity<Medication>()
              .HasOne(m => m.Patient)
              .WithMany(p => p.Medications)
              .HasForeignKey(m => m.PatientId)
              .OnDelete(DeleteBehavior.Restrict);

            // Configures the one-to-many relationship between
            // Patient and Appointment.
            modelBuilder.Entity<Appointment>()
              .HasOne(a => a.Patient)
              .WithMany(p => p.Appointments)
              .HasForeignKey(a => a.PatientId)
              .OnDelete(DeleteBehavior.Restrict);

            // Configures the one-to-many relationship between
            // ApplicationUser and RefreshToken.
            modelBuilder.Entity<RefreshToken>()
              .HasOne(rt => rt.User)
              .WithMany(u => u.RefreshTokens)
              .HasForeignKey(rt => rt.UserId)
              .OnDelete(DeleteBehavior.Cascade);

            // Ensures that each patient's NationalId is unique.
            modelBuilder.Entity<Patient>()
             .HasIndex(p => p.NationalId)
             .IsUnique();
        }
    }
}