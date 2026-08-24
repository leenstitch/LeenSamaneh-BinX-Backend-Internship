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
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Allergy> Allergies { get; set; }
        public DbSet<CardiacEvent> CardiacEvents { get; set; }
        public DbSet<EmergencyMedicalInformation> EmergencyMedicalInformation { get; set; }
        public DbSet<FamilyMedicalHistory> FamilyMedicalHistories { get; set; }
        public DbSet<Hospitalization> Hospitalizations { get; set; }
        public DbSet<Insurance> Insurances { get; set; }
        public DbSet<LabResult> LabResults { get; set; }
        public DbSet<MedicalProcedure> MedicalProcedures { get; set; }
        public DbSet<Reminder>Reminders { get; set; }
        public DbSet<ReminderType> ReminderTypes { get; set; }
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

            
            // Patient - Doctor (1:N)
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Patient)
                .WithMany(p => p.Doctors)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Doctor - Diagnosis (1:N)
            modelBuilder.Entity<Diagnosis>()
                .HasOne(d => d.Doctor)
                .WithMany(d => d.Diagnoses)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.SetNull);

            // Doctor - Medication (1:N)
            modelBuilder.Entity<Medication>()
                .HasOne(m => m.Doctor)
                .WithMany(d => d.Medications)
                .HasForeignKey(m => m.DoctorId)
                .OnDelete(DeleteBehavior.SetNull);

            // Doctor - Appointment (1:N)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.SetNull);

            // Patient - Allergy (1:N)
            modelBuilder.Entity<Allergy>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Allergies)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            
            // Patient - FamilyMedicalHistory (1:N)
            modelBuilder.Entity<FamilyMedicalHistory>()
                .HasOne(f => f.Patient)
                .WithMany(p => p.FamilyMedicalHistories)
                .HasForeignKey(f => f.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

           
            // Patient - LabResult (1:N)
            modelBuilder.Entity<LabResult>()
                .HasOne(l => l.Patient)
                .WithMany(p => p.LabResults)
                .HasForeignKey(l => l.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

           
            // Patient - MedicalProcedure (1:N)
            modelBuilder.Entity<MedicalProcedure>()
                .HasOne(mp => mp.Patient)
                .WithMany(p => p.MedicalProcedures)
                .HasForeignKey(mp => mp.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            
            // Doctor - MedicalProcedure (1:N)
            modelBuilder.Entity<MedicalProcedure>()
                .HasOne(mp => mp.Doctor)
                .WithMany(d => d.MedicalProcedures)
                .HasForeignKey(mp => mp.DoctorId)
                .OnDelete(DeleteBehavior.SetNull);

           
            // Patient - Hospitalization (1:N)
            modelBuilder.Entity<Hospitalization>()
                .HasOne(h => h.Patient)
                .WithMany(p => p.Hospitalizations)
                .HasForeignKey(h => h.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            
            // Patient - EmergencyMedicalInformation (1:1)
            modelBuilder.Entity<EmergencyMedicalInformation>()
                .HasOne(e => e.Patient)
                .WithOne(p => p.EmergencyMedicalInformation)
                .HasForeignKey<EmergencyMedicalInformation>(
                    e => e.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            
            // Patient - Reminder (1:N)
            modelBuilder.Entity<Reminder>()
                .HasOne(r => r.Patient)
                .WithMany(p => p.Reminders)
                .HasForeignKey(r => r.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

           
            // Patient - CardiacEvent (1:N)
            modelBuilder.Entity<CardiacEvent>()
                .HasOne(c => c.Patient)
                .WithMany(p => p.CardiacEvents)
                .HasForeignKey(c => c.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

          
            // Doctor - CardiacEvent (1:N)
            modelBuilder.Entity<CardiacEvent>()
                .HasOne(c => c.Doctor)
                .WithMany(d => d.CardiacEvents)
                .HasForeignKey(c => c.DoctorId)
                .OnDelete(DeleteBehavior.SetNull);

            
            // Patient - Insurance (1:N)
            modelBuilder.Entity<Insurance>()
                .HasOne(i => i.Patient)
                .WithMany(p => p.Insurances)
                .HasForeignKey(i => i.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // RemiderType - Reminders (1:N)
            modelBuilder.Entity<Reminder>()
               .HasOne(r => r.ReminderType)
               .WithMany(rt => rt.Reminders)
               .HasForeignKey(r => r.ReminderTypeId)
               .OnDelete(DeleteBehavior.Restrict);

            // Ensures that each patient's NationalId is unique.
            modelBuilder.Entity<Patient>()
             .HasIndex(p => p.NationalId)
             .IsUnique();

            modelBuilder.Entity<EmergencyMedicalInformation>()
             .HasIndex(e => e.PatientId)
             .IsUnique();

            // Seed Reminder Types
            modelBuilder.Entity<ReminderType>().HasData(
                new ReminderType
                {
                    ReminderTypeId = 1,
                    Name = "Medication",
                    Description = "Reminder for taking medication"
                },
                new ReminderType
                {
                    ReminderTypeId = 2,
                    Name = "Appointment",
                    Description = "Reminder for a doctor appointment"
                },
                new ReminderType
                {
                    ReminderTypeId = 3,
                    Name = "Vital Sign Check",
                    Description = "Reminder to measure vital signs"
                },
                new ReminderType
                {
                    ReminderTypeId = 4,
                    Name = "Doctor Follow-up",
                    Description = "Reminder for a doctor follow-up"
                },
                new ReminderType
                {
                    ReminderTypeId = 5,
                    Name = "Medical Test",
                    Description = "Reminder for a medical test"
                },
                new ReminderType
                {
                    ReminderTypeId = 6,
                    Name = "General",
                    Description = "General health reminder"
                }
            );
        }
    }
}