# Week 6 — Sprint 1 Summary

## 1. Sprint Goal

Design and implement the complete database schema for the **Cardiac Patient Monitoring System**, apply EF Core migrations, and deliver the core patient health-record API operations with proper business logic, validation, testing, and documentation.

---

## 2. Database & Entity Implementation

Completed the database foundation for the Cardiac Patient Monitoring System.

### Completed Work

* Finalized the complete database schema.
* Created and finalized the ERD.
* Implemented the EF Core entity models.
* Configured entity relationships and constraints using Fluent API.
* Added initial seed data.
* Generated, reviewed, and applied EF Core migrations.
* Verified relationships, foreign keys, and database integrity.
* Applied normalization principles to reduce data redundancy and maintain data consistency.

### Main Entities

* ApplicationUser
* RefreshToken
* Patient
* Doctor
* EmergencyContact
* Insurance
* VitalSign
* Diagnosis
* Medication
* Allergy
* MedicalProcedure
* CardiacEvent
* Hospitalization
* FamilyMedicalHistory
* LabResult
* EmergencyMedicalInformation
* Reminder
* Appointment

---

## 3. Vital Signs API

Implemented and improved the Vital Signs API with pagination, filtering, sorting, DTO mapping, and business logic.

### Completed Features

* Implemented paginated Vital Signs GET endpoint.
* Added `Page` and `PageSize` parameters.
* Added `TotalCount` and `TotalPages`.
* Implemented filtering by patient name and gender.
* Implemented sorting by measurement date.
* Created reusable `PaginatedResponseDto<T>`.
* Created `VitalSignQueryDto`.
* Updated the Repository to use `Skip()` and `Take()` for pagination.
* Updated the Service to validate pagination parameters.
* Added DTO mapping.
* Added abnormal-value detection.
* Added validation for invalid query values such as invalid gender.
* Verified different pagination, filtering, and sorting scenarios using Postman.

---

## 4. Cardiac Event Management & Analysis

Implemented cardiac event management and historical medical analysis.

### Completed Features

* Created cardiac events.
* Linked cardiac events to the authenticated patient.
* Validated patient ownership of cardiac events.
* Retrieved cardiac event information.
* Implemented configurable historical analysis using a selected number of days before the event.

The analysis includes:

* Vital signs
* Medications
* Diagnoses
* Laboratory results
* Hospitalizations
* Medical procedures

Vital-sign analysis also calculates abnormal readings and summarizes the patient's measurements before the cardiac event.

---

## 5. Medical History

Implemented historical medical-record retrieval required for cardiac event analysis.

### Completed Features

* Historical medications.
* Previous diagnoses.
* Laboratory results.
* Hospitalizations.
* Medical procedures.

---

## 6. Repository & Service Layer

Reviewed and improved the Repository and Service layers for:

* Vital Signs
* Cardiac Events
* Diagnoses
* Medications
* Laboratory Results
* Hospitalizations
* Medical Procedures

---

## 7. Database Transaction

Implemented a database transaction for patient account creation.

The registration process creates and links:

* Application User
* Patient Profile

Both operations are handled within the same transaction.

If any operation fails, the transaction is rolled back to prevent incomplete or inconsistent data from being stored.

---

## 8. Testing & API Verification

Completed testing and verification of the implemented Sprint 1 features.

### Postman

* Tested the implemented API endpoints.
* Verified successful responses.
* Tested pagination.
* Tested filtering and sorting.
* Tested cardiac event functionality.
* Verified patient-specific data access.
* Verified error cases and invalid input handling.
<img width="980" height="626" alt="image" src="https://github.com/user-attachments/assets/bb3f2a06-4bbd-49a1-b07f-77567a015c86" />


---

## 9. Code Documentation

Improved code readability and maintainability by adding clear  comments throughout the Repository and Service layers.

---

## 10. Sprint 1 Backlog

All Sprint 1 tasks were completed and reviewed.

| ID    | Task                                                            | Estimated Effort | Status |
| ----- | --------------------------------------------------------------- | ---------------: | :----: |
| S1-01 | Finalize complete database schema                               |             1 hr | ✅ Done |
| S1-02 | Create and finalize ERD                                         |             1 hr | ✅ Done |
| S1-03 | Implement full EF Core entity model                             |            4 hrs | ✅ Done |
| S1-04 | Configure relationships and constraints using Fluent API        |            3 hrs | ✅ Done |
| S1-05 | Add initial seed data                                           |             1 hr | ✅ Done |
| S1-06 | Generate, review, and apply EF Core migration                   |           0.5 hr | ✅ Done |
| S1-07 | Implement paginated VitalSigns GET endpoint                     |            4 hrs | ✅ Done |
| S1-08 | Add filtering and sorting to VitalSigns                         |            3 hrs | ✅ Done |
| S1-09 | Implement DTO projection for VitalSigns                         |            4 hrs | ✅ Done |
| S1-10 | Implement VitalSign business logic and abnormal-value detection |            5 hrs | ✅ Done |
| S1-11 | Wrap multi-step operations in a database transaction            |            4 hrs | ✅ Done |
| S1-12 | Test Sprint 1 features using Postman                            |            4 hrs | ✅ Done |
| S1-13 | Prepare and submit GitHub Pull Request                          |            2 hrs | ✅ Done |

---

## 11. Sprint Review & Close-Out

Completed the Sprint 1 close-out activities.

* Demoed the Sprint 1 API using Postman.
* Verified the completed backlog tasks against the Sprint acceptance criteria.
* Confirmed that all Sprint 1 tasks were completed.
* Reviewed the implemented features and API behavior.
* Confirmed that there were no unresolved code review comments.
* Prepared the project for GitHub submission.
* Completed the Sprint 1 Retrospective.

---

# Sprint 1 Retrospective

## What Went Well

* Completed the Sprint 1 backend features successfully.
* Implemented database transactions for patient account creation.
* Completed Vital Signs functionality, including filtering, sorting, pagination, comparison, and abnormal-value detection.
* Implemented cardiac event management and historical medical data analysis.
* Added unit and integration tests for the implemented features.
* Tested the API using Postman and verified the main endpoints.

## What Could Be Improved

* Some features required more debugging and testing than initially expected.
* Testing should be planned alongside feature implementation rather than mainly after completing the feature.

## Concrete Action for Sprint 2

* Write the happy-path and error-case tests alongside each new feature before considering the feature complete.
---

## 13. Sprint 1 Completion

**Sprint 1 was successfully completed.**

All backlog tasks from **S1-01 to S1-13** were completed, reviewed, tested, and prepared for submission.

The Cardiac Patient Monitoring System now has a complete database foundation, core patient health-record APIs, Vital Signs business logic, cardiac event analysis, transaction-based patient registration, testing, Postman verification, and Sprint documentation.






