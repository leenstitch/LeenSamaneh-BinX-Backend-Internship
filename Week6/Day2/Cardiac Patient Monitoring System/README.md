# Day 2 — Building the EF Core Data Model & Migrations

## Overview

Day 2 focused on implementing the Cardiac Patient Monitoring System database schema using Entity Framework Core.

The full domain model was implemented as C# entity classes, relationships were configured using the Fluent API,
initial reference data was seeded, and an EF Core migration was generated and reviewed.

---

## Tasks Completed

### 1. Implemented the EF Core Entity Model

Created the entity classes required for the complete Cardiac Patient Monitoring System domain, including:

- Doctor
- Insurance
- Allergy
- MedicalProcedure
- CardiacEvent
- Hospitalization
- FamilyMedicalHistory
- LabResult
- EmergencyMedicalInformation
- Reminder
- ReminderType

Navigation properties and foreign keys were added to represent the relationships between the entities.

---

### 2. Configured Relationships Using Fluent API

Configured the entity relationships explicitly in OnModelCreating.

The configuration includes:

- Patient-to-medical-record relationships.
- Doctor-to-medical-record relationships.
- One-to-many relationships between patients and their records.
- One-to-one relationship between Patient and EmergencyMedicalInformation.
- Relationships between Reminder and ReminderType.
- Foreign key constraints.
- Required and optional relationships.
- Explicit delete behaviors using:
  - DeleteBehavior.Restrict
  - DeleteBehavior.SetNull
  - DeleteBehavior.Cascade where appropriate.

Explicit relationship configuration was used to avoid unexpected cascade delete behavior and maintain database integrity.

---

### 3. Added Seed Data

Added initial reference data for ReminderType.

The seeded reminder types include:

- Medication
- Appointment
- Vital Sign Check
- Doctor Follow-up
- Medical Test
- General
<img width="852" height="556" alt="image" src="https://github.com/user-attachments/assets/eb7190c8-e094-40fe-a89a-0829214ec214" />

The seed data was configured using EF Core HasData.

---

### 4. Generated EF Core Migration

Generated the initial database migration:
"Week6InitialSchema"
