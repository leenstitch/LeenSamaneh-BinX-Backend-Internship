using Cardiac_Patient_Monitoring_System.DTO_S.MedicalTimelineItemDto_s;
using Cardiac_Patient_Monitoring_System.Interfaces;
using Cardiac_Patient_Monitoring_System.Repositories;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;

namespace Cardiac_Patient_Monitoring_System.Services
{
    public class MedicalTimelineService : IMedicalTimelineService
    {
        private readonly IAllergyRepository _allergyRepository;
        private readonly IFamilyMedicalHistoryRepository _familyHistoryRepository;
        private readonly IVitalSignRepository _vitalSignRepository;
        private readonly IMedicationRepository _medicationRepository;
        private readonly IDiagnosisRepository _diagnosisRepository;
        private readonly ILabResultRepository _labResultRepository;
        private readonly IHospitalizationRepository _hospitalizationRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public MedicalTimelineService(
            IAllergyRepository allergyRepository,
            IFamilyMedicalHistoryRepository familyHistoryRepository,
            IVitalSignRepository vitalSignRepository,
            IMedicationRepository medicationRepository,
            IDiagnosisRepository diagnosisRepository,
            ILabResultRepository labResultRepository,
            IHospitalizationRepository hospitalizationRepository,
            IAppointmentRepository appointmentRepository)
        {
            _allergyRepository = allergyRepository;
            _familyHistoryRepository = familyHistoryRepository;
            _vitalSignRepository = vitalSignRepository;
            _medicationRepository = medicationRepository;
            _diagnosisRepository = diagnosisRepository;
            _labResultRepository = labResultRepository;
            _hospitalizationRepository = hospitalizationRepository;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<MedicalTimelineResponseDto>
            GetPatientMedicalTimelineAsync(
                int patientId,
                int page,
                int pageSize)
        {
            // Validate pagination values

            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }


            // Get medical data

            var allergies =
                await _allergyRepository
                    .GetByPatientIdAsync(patientId);

            var familyHistory =
                await _familyHistoryRepository
                    .GetByPatientIdAsync(patientId);

            var vitalSigns =
                await _vitalSignRepository
                    .GetByPatientIdAsync(patientId);

            var medications =
                await _medicationRepository
                    .GetByPatientIdAsync(patientId);

            var diagnoses =
                await _diagnosisRepository
                    .GetByPatientIdAsync(patientId);

            //var labResults =
            //    await _labResultRepository
            //        .GetByPatientIdAsync(patientId);

            //var hospitalizations =
            //    await _hospitalizationRepository
            //        .GetByPatientIdAsync(patientId);

            var appointments =
                await _appointmentRepository
                    .GetByPatientIdAsync(patientId);


            // Create unified timeline

            var timeline =
                new List<MedicalTimelineItemDto>();


            // Allergies

            timeline.AddRange(
                allergies.Select(a =>
                    new MedicalTimelineItemDto
                    {
                        EventType = "Allergy",

                        RecordId =
                            a.AllergyId,

                        Date =
                            a.CreatedAt,

                        Title =
                            a.Name,

                        Description =
                            a.Reaction
                    }));


            // Family Medical History

            timeline.AddRange(
                familyHistory.Select(f =>
                    new MedicalTimelineItemDto
                    {
                        EventType =
                            "Family Medical History",

                        RecordId =
                            f.FamilyHistoryId,

                        Date =
                            f.CreatedAt,

                        Title =
                            f.Condition,

                        Description =
                            $"Relationship: {f.Relationship}"
                    }));


            // Vital Signs

            timeline.AddRange(
                vitalSigns.Select(v =>
                    new MedicalTimelineItemDto
                    {
                        EventType =
                            "Vital Sign",

                        RecordId =
                            v.VitalSignId,

                        Date =
                            v.MeasuredAt,

                        Title =
                            "Vital Signs",

                        Description =
                            $"Heart Rate: {v.HeartRate}, " +
                            $"BP: {v.SystolicPressure}/" +
                            $"{v.DiastolicPressure}, " +
                            $"Oxygen: {v.OxygenSaturation}%"
                    }));


            // Medications

            timeline.AddRange(
                medications.Select(m =>
                    new MedicalTimelineItemDto
                    {
                        EventType =
                            "Medication",

                        RecordId =
                            m.MedicationId,

                        Date =
                            m.StartDate,

                        Title =
                            m.Name,

                        Description =
                            $"{m.Dosage} - {m.Frequency}"
                    }));


            // Diagnoses

            timeline.AddRange(
                diagnoses.Select(d =>
                    new MedicalTimelineItemDto
                    {
                        EventType =
                            "Diagnosis",

                        RecordId =
                            d.DiagnosisId,

                        Date =
                            d.DiagnosedAt,

                        Title =
                            d.DiagnosisName,

                        Description =
                            d.Notes
                    }));


            //// Lab Results

            //timeline.AddRange(
            //    labResults.Select(l =>
            //        new MedicalTimelineItemDto
            //        {
            //            EventType =
            //                "Lab Result",

            //            RecordId =
            //                l.LabResultId,

            //            Date =
            //                l.TestDate,

            //            Title =
            //                l.TestName,

            //            Description =
            //                $"{l.Result} {l.Unit}"
            //        }));


            //// Hospitalizations

            //timeline.AddRange(
            //    hospitalizations.Select(h =>
            //        new MedicalTimelineItemDto
            //        {
            //            EventType =
            //                "Hospitalization",

            //            RecordId =
            //                h.HospitalizationId,

            //            Date =
            //                h.AdmissionDate,

            //            Title =
            //                h.HospitalName,

            //            Description =
            //                h.Reason
            //        }));


            // Appointments

            timeline.AddRange(
                appointments.Select(a =>
                    new MedicalTimelineItemDto
                    {
                        EventType =
                            "Appointment",

                        RecordId =
                            a.AppointmentId,

                        Date =
                            a.AppointmentDate,

                        Title =
                            a.Reason,

                        Description =
                            a.Notes
                    }));


            // Sort newest → oldest

            timeline = timeline
                .OrderByDescending(x => x.Date)
                .ToList();


            // Total records before pagination

            var totalCount =
                timeline.Count;


            // Pagination

            var totalPages =
                (int)Math.Ceiling(
                    totalCount / (double)pageSize);


            var items =
                timeline
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();


            // Return response DTO

            return new MedicalTimelineResponseDto
            {
                PatientId =
                    patientId,

                Page =
                    page,

                PageSize =
                    pageSize,

                TotalCount =
                    totalCount,

                TotalPages =
                    totalPages,

                Items =
                    items
            };
        }
    }
}