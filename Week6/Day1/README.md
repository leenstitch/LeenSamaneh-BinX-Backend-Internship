# 1. Sprint Goal

Design and implement the complete database schema for the Cardiac Patient Monitoring System, apply EF Core migrations, 
and deliver the core patient health-record API operations with proper business logic.

------------------------------------------------------------------------------------------------------------------------------------------------
## 2. Full Domain Entities

The Cardiac Patient Monitoring System is designed around the following entities:

### Authentication
- ApplicationUser
- RefreshToken


### Patient Management
- Patient
- Doctor
- EmergencyContact
- Insurance

### Medical Records
- VitalSign
- Diagnosis
- Medication
- Allergy
- MedicalProcedure
- CardiacEvent

### Medical History
- Hospitalization
- FamilyMedicalHistory
- LabResult

### Emergency & Support
- EmergencyMedicalInformation
- Reminder

### Scheduling
- Appointment
------------------------------------------------------------------------------------------------------------------------------------------------

### 3. Complete Database Schema & Normalization

The Cardiac Patient Monitoring System database was designed using normalization principles to reduce data redundancy, maintain data integrity, and establish clear relationships between entities.

The complete database schema includes the following main entities:

- Patients
- AspNetUsers
- RefreshTokens
- Doctors
- EmergencyContacts
- Insurance
- Diagnoses
- VitalSigns
- Medications
- Appointments
- Allergies
- FamilyMedicalHistory
- LabResults
- MedicalProcedures
- Hospitalizations
- EmergencyMedicalInformation
- Reminders
- CardiacEvents

The schema separates different types of patient information into dedicated tables and uses primary keys and foreign keys 
to establish relationships between related entities.

Examples of normalization applied:

- Patient information is stored separately from authentication data in`Patients and AspNetUsers.
- Medical records such as diagnoses, medications, vital signs, allergies, and lab results are stored in separate tables
   instead of being duplicated inside the Patients table.
- Doctors are stored separately and referenced through DoctorId where applicable.
- Repeated patient-related records use a one-to-many relationship with PatientId as a foreign key.
- EmergencyMedicalInformation has a one-to-one relationship with Patients.
- PatientId and DoctorId foreign keys maintain referential integrity between related entities.
- Unique constraints are applied to fields such as Patients.UserId and Patients.NationalId where required.

This structure helps keep the database organized, reduces unnecessary duplication, and makes the system easier to maintain and extend.

------------------------------------------------------------------------------------------------------------------------------------------------

### 4. Finalized Entity Relationship Diagram (ERD)

The complete database schema was finalized and documented using an Entity Relationship Diagram (ERD).

The ERD represents:

- All entities required for the Cardiac Patient Monitoring System.
- Primary keys (PK) and foreign keys (FK).
- One-to-one and one-to-many relationships.
- Relationships between patients and their medical records.
- Relationships between patients and their doctors.
- Authentication relationships between AspNetUsers, Patients, and RefreshTokens.
- Referential integrity between related entities.

The ERD was designed and reviewed to ensure that it matches the planned database schema and follows the normalization principles applied during schema design.

<img width="1893" height="802" alt="image" src="https://github.com/user-attachments/assets/63a6fb8f-7ca2-45ad-b7df-ab7af2136986" />
<img width="1889" height="586" alt="image" src="https://github.com/user-attachments/assets/b0d4bfb0-728b-43fc-9df9-d7aa42076c73" />

------------------------------------------------------------------------------------------------------------------------------------------------

##### 5. Sprint 1 Backlog

| ID    |      Task                                                       | Estimated Effort | Status  |
--------|-----------------------------------------------------------------|------------------|---------| 
| S1-01 | Finalize complete database schema                               | 1 hr             | Done    |
| S1-02 | Create and finalize ERD                                         | 1 hr             | Done    |
| S1-03 | Implement full EF Core entity model                             | 4 hrs            | Pending |
| S1-04 | Configure relationships and constraints using Fluent API        | 3 hrs            | Pending |
| S1-05 | Add initial seed data                                           | 1 hr             | Pending |
| S1-06 | Generate, review, and apply EF Core migration                   | .5 hr            | Pending |
| S1-07 | Implement paginated VitalSigns GET endpoint                     | 4 hrs            | Pending |
| S1-08 | Add filtering and sorting to VitalSigns                         | 3 hrs            | Pending |
| S1-09 | Implement DTO projection for VitalSigns                         | 4 hrs            | Pending |
| S1-10 | Implement VitalSign business logic and abnormal-value detection | 5 hrs            | Pending |
| S1-11 | Wrap multi-step operations in a database transaction            | 4 hrs            | Pending |
| S1-12 | Test Sprint 1 features using Postman                            | 4 hrs            | Pending |
| S1-13 | Prepare and submit GitHub Pull Request                          | 2 hrs            | Pending |



