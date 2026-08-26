using Cardiac_Patient_Monitoring_System.DTO_S.CardiacEvent_s;
using Cardiac_Patient_Monitoring_System.DTO_S.HospitalizationDto_s;
using Cardiac_Patient_Monitoring_System.DTO_S.LabResultDto_s;
using Cardiac_Patient_Monitoring_System.DTO_S.MedicalProcedureDto_s;
using Cardiac_Patient_Monitoring_System.DTO_S.MedicationDto_s;
using Cardiac_Patient_Monitoring_System.DTO_S.VitalSignDto_s;
using Cardiac_Patient_Monitoring_System.DTOs;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;
using Cardiac_Patient_Monitoring_System.Services.Interfaces;

namespace Cardiac_Patient_Monitoring_System.Services
{
    public class CardiacEventAnalysisService
        : ICardiacEventAnalysisService
    {
        private readonly ICardiacEventRepository
            _cardiacEventRepository;

        private readonly IVitalSignRepository
            _vitalSignRepository;

        private readonly IMedicationRepository
            _medicationRepository;

        private readonly IDiagnosisRepository
            _diagnosisRepository;

        private readonly ILabResultRepository
            _labResultRepository;

        private readonly IHospitalizationRepository
            _hospitalizationRepository;

        private readonly IMedicalProcedureRepository
            _medicalProcedureRepository;


        public CardiacEventAnalysisService(
            ICardiacEventRepository cardiacEventRepository,
            IVitalSignRepository vitalSignRepository,
            IMedicationRepository medicationRepository,
            IDiagnosisRepository diagnosisRepository,
            ILabResultRepository labResultRepository,
            IHospitalizationRepository hospitalizationRepository,
            IMedicalProcedureRepository medicalProcedureRepository)
        {
            _cardiacEventRepository =
                cardiacEventRepository;

            _vitalSignRepository =
                vitalSignRepository;

            _medicationRepository =
                medicationRepository;

            _diagnosisRepository =
                diagnosisRepository;

            _labResultRepository =
                labResultRepository;

            _hospitalizationRepository =
                hospitalizationRepository;

            _medicalProcedureRepository =
                medicalProcedureRepository;
        }


        // ============================================================
        // Analyze Cardiac Event
        // ============================================================

        public async Task<CardiacEventAnalysisResponseDto?>
            AnalyzeEventAsync(
                int userId,
                int cardiacEventId,
                int daysBefore)
        {
            if (daysBefore <= 0)
            {
                throw new ArgumentException(
                    "DaysBefore must be greater than zero.");
            }


            // --------------------------------------------------------
            // 1. Get the patient connected to the authenticated user
            // --------------------------------------------------------

            var patientId =
                await _vitalSignRepository
                    .GetPatientIdByUserIdAsync(userId);

            if (patientId == null)
            {
                return null;
            }


            // --------------------------------------------------------
            // 2. Get the cardiac event
            // --------------------------------------------------------

            var cardiacEvent =
                await _cardiacEventRepository
                    .GetByIdAsync(cardiacEventId);

            if (cardiacEvent == null)
            {
                return null;
            }


            // --------------------------------------------------------
            // 3. Security:
            //    Make sure the event belongs to this patient
            // --------------------------------------------------------

            if (cardiacEvent.PatientId != patientId.Value)
            {
                return null;
            }


            // --------------------------------------------------------
            // 4. Calculate analysis period
            // --------------------------------------------------------

            var eventDate =
                cardiacEvent.EventDate;

            var startDate =
                eventDate.AddDays(-daysBefore);


            // --------------------------------------------------------
            // 5. Get Vital Signs
            // --------------------------------------------------------

            var vitalResult =
                await _vitalSignRepository
                    .GetByPatientAndDateRangeAsync(
                        patientId.Value,
                        startDate,
                        eventDate,
                        new CardiacEventVitalQueryDto
                        {
                            PageNumber = 1,

                            // We need all readings for
                            // calculating the summary.
                            PageSize = int.MaxValue,

                            SortBy = "MeasuredAt",

                            SortDescending = false
                        });

            var vitalSigns =
                vitalResult.Data.ToList();


            // --------------------------------------------------------
            // 6. Get Medications active during the period
            // --------------------------------------------------------

            var medications =
                await _medicationRepository
                    .GetHistoricalMedicationsAsync(
                        patientId.Value,
                        startDate,
                        eventDate);


            // --------------------------------------------------------
            // 7. Get Diagnoses recorded before the event
            // --------------------------------------------------------

            var diagnoses =
                await _diagnosisRepository
                    .GetRecordedBeforeEventAsync(
                        patientId.Value,
                        eventDate);


            // --------------------------------------------------------
            // 8. Get Lab Results during the period
            // --------------------------------------------------------

            var labResults =
                await _labResultRepository
                    .GetByPatientAndDateRangeAsync(
                        patientId.Value,
                        startDate,
                        eventDate);


            // --------------------------------------------------------
            // 9. Get Hospitalizations overlapping the period
            // --------------------------------------------------------

            var hospitalizations =
                await _hospitalizationRepository
                    .GetOverlappingPeriodAsync(
                        patientId.Value,
                        startDate,
                        eventDate);


            // --------------------------------------------------------
            // 10. Get Medical Procedures during the period
            // --------------------------------------------------------

            var procedures =
                await _medicalProcedureRepository
                    .GetByPatientAndDateRangeAsync(
                        patientId.Value,
                        startDate,
                        eventDate);


            // --------------------------------------------------------
            // 11. Get previous cardiac events
            // --------------------------------------------------------

            var previousEvents =
                await _cardiacEventRepository
                    .GetPreviousEventsAsync(
                        patientId.Value,
                        startDate,
                        eventDate);


            // --------------------------------------------------------
            // 12. Build Vital Signs Summary
            // --------------------------------------------------------

            var vitalSummary =
                BuildVitalSummary(vitalSigns);


            // --------------------------------------------------------
            // 13. Convert Vital Signs to DTOs
            // --------------------------------------------------------

            var vitalDtos =
                vitalSigns.Select(v =>
                    new CardiacEventVitalDto
                    {
                        VitalSignId =
                            v.VitalSignId,

                        MeasuredAt =
                            v.MeasuredAt,

                        HeartRate =
                            v.HeartRate,

                        SystolicPressure =
                            v.SystolicPressure,

                        DiastolicPressure =
                            v.DiastolicPressure,

                        OxygenSaturation =
                            v.OxygenSaturation,

                        Temperature =
                            v.Temperature,

                        Notes =
                            v.Notes
                    });


            // --------------------------------------------------------
            // 14. Build final response
            // --------------------------------------------------------

            return new CardiacEventAnalysisResponseDto
            {
                CardiacEvent =
                    new CardiacEventResponseDto
                    {
                        CardiacEventId =
                            cardiacEvent.CardiacEventId,

                        PatientId =
                            cardiacEvent.PatientId,

                        DoctorId =
                            cardiacEvent.DoctorId,

                        EventType =
                            cardiacEvent.EventType,

                        EventDate =
                            cardiacEvent.EventDate,

                        Description =
                            cardiacEvent.Description,

                        Location =
                            cardiacEvent.Location,

                        Outcome =
                            cardiacEvent.Outcome,

                        Notes =
                            cardiacEvent.Notes
                    },


                AnalysisFrom =
                    startDate,


                AnalysisTo =
                    eventDate,


                VitalSummary =
                    vitalSummary,


                VitalSigns =
                    vitalDtos,


                LabResults =
                    labResults.Select(x =>
                        new LabResultResponseDto
                        {
                            LabResultId =
                                x.LabResultId,

                            PatientId =
                                x.PatientId,

                            TestName =
                                x.TestName,

                            Result =
                                x.Result,

                            Unit =
                                x.Unit,

                            ReferenceRange =
                                x.ReferenceRange,

                            TestDate =
                                x.TestDate,

                            LaboratoryName =
                                x.LaboratoryName,

                            Notes =
                                x.Notes
                        }),


                Medications =
                    medications.Select(x =>
                        new MedicationResponseDto
                        {
                            MedicationId =
                                x.MedicationId,

                            PatientId =
                                x.PatientId,

                            PrescribedByDoctorName =
                                x.PrescribedByDoctorName,

                            PrescribedBySpecialization =
                                x.PrescribedBySpecialization,

                            Name =
                                x.Name,

                            Dosage =
                                x.Dosage,

                            Frequency =
                                x.Frequency,

                            StartDate =
                                x.StartDate,

                            EndDate =
                                x.EndDate,

                            Notes =
                                x.Notes,

                            CreatedAt =
                                x.CreatedAt,

                            UpdatedAt =
                                x.UpdatedAt
                        }),


                Diagnoses =
                    diagnoses.Select(x =>
                        new DiagnosisResponseDto
                        {
                            DiagnosisId =
                                x.DiagnosisId,

                            PatientId =
                                x.PatientId,

                            DiagnosedByName =
                                x.DiagnosedByName,

                            DiagnosedBySpecialization =
                                x.DiagnosedBySpecialization,

                            DiagnosisName =
                                x.DiagnosisName,

                            DiagnosedAt =
                                x.DiagnosedAt,

                            Notes =
                                x.Notes,

                            CreatedAt =
                                x.CreatedAt,

                            UpdatedAt =
                                x.UpdatedAt
                        }),


                Hospitalizations =
                    hospitalizations.Select(x =>
                        new HospitalizationResponseDto
                        {
                            HospitalizationId =
                                x.HospitalizationId,

                            PatientId =
                                x.PatientId,

                            HospitalName =
                                x.HospitalName,

                            AdmissionDate =
                                x.AdmissionDate,

                            DischargeDate =
                                x.DischargeDate,

                            Reason =
                                x.Reason,

                            Diagnosis =
                                x.Diagnosis,

                            Notes =
                                x.Notes
                        }),


                MedicalProcedures =
                    procedures.Select(x =>
                        new MedicalProcedureResponseDto
                        {
                            ProcedureId =
                                x.ProcedureId,

                            PatientId =
                                x.PatientId,

                            ProcedureName =
                                x.ProcedureName,

                            ProcedureDate =
                                x.ProcedureDate,

                            HospitalName =
                                x.HospitalName,

                            Reason =
                                x.Reason,

                            Outcome =
                                x.Outcome,

                            Notes =
                                x.Notes
                        }),


                PreviousCardiacEvents =
                    previousEvents.Select(x =>
                        new CardiacEventResponseDto
                        {
                            CardiacEventId =
                                x.CardiacEventId,

                            PatientId =
                                x.PatientId,

                            DoctorId =
                                x.DoctorId,

                            EventType =
                                x.EventType,

                            EventDate =
                                x.EventDate,

                            Description =
                                x.Description,

                            Location =
                                x.Location,

                            Outcome =
                                x.Outcome,

                            Notes =
                                x.Notes
                        })
            };
        }


        // ============================================================
        // Get filtered / sorted / paginated vital signs
        // ============================================================

        public async Task<
            (IEnumerable<CardiacEventVitalDto> Data,
             int TotalCount)>
            GetEventVitalsAsync(
                int userId,
                int cardiacEventId,
                DateTime startDate,
                DateTime endDate,
                CardiacEventVitalQueryDto query)
        {
            // Get patient belonging to authenticated user
            var patientId =
                await _vitalSignRepository
                    .GetPatientIdByUserIdAsync(userId);

            if (patientId == null)
            {
                return (Enumerable.Empty<CardiacEventVitalDto>(), 0);
            }


            // Get cardiac event
            var cardiacEvent =
                await _cardiacEventRepository
                    .GetByIdAsync(cardiacEventId);

            if (cardiacEvent == null ||
                cardiacEvent.PatientId != patientId.Value)
            {
                return (
                    Enumerable.Empty<CardiacEventVitalDto>(),
                    0);
            }


            // Get filtered / sorted / paginated data
            var result =
                await _vitalSignRepository
                    .GetByPatientAndDateRangeAsync(
                        patientId.Value,
                        startDate,
                        endDate,
                        query);


            // Convert to DTO
            var data =
                result.Data.Select(v =>
                    new CardiacEventVitalDto
                    {
                        VitalSignId =
                            v.VitalSignId,

                        MeasuredAt =
                            v.MeasuredAt,

                        HeartRate =
                            v.HeartRate,

                        SystolicPressure =
                            v.SystolicPressure,

                        DiastolicPressure =
                            v.DiastolicPressure,

                        OxygenSaturation =
                            v.OxygenSaturation,

                        Temperature =
                            v.Temperature,

                        Notes =
                            v.Notes
                    });


            return (
                data,
                result.TotalCount);
        }


        // ============================================================
        // Vital Signs Summary
        // ============================================================

        private VitalSignSummaryDto BuildVitalSummary(
            IEnumerable<VitalSign> vitalSigns)
        {
            var list =
                vitalSigns.ToList();


            if (!list.Any())
            {
                return new VitalSignSummaryDto
                {
                    ReadingCount = 0,

                    AbnormalReadingCount = 0
                };
            }


            return new VitalSignSummaryDto
            {
                ReadingCount =
                    list.Count,


                AverageHeartRate =
                    list.Average(
                        x => x.HeartRate),


                MinimumHeartRate =
                    list.Min(
                        x => x.HeartRate),


                MaximumHeartRate =
                    list.Max(
                        x => x.HeartRate),


                AverageSystolicPressure =
                    list.Average(
                        x => x.SystolicPressure),


                MinimumSystolicPressure =
                    list.Min(
                        x => x.SystolicPressure),


                MaximumSystolicPressure =
                    list.Max(
                        x => x.SystolicPressure),


                AverageDiastolicPressure =
                    list.Average(
                        x => x.DiastolicPressure),


                MinimumDiastolicPressure =
                    list.Min(
                        x => x.DiastolicPressure),


                MaximumDiastolicPressure =
                    list.Max(
                        x => x.DiastolicPressure),


                AverageOxygenSaturation =
                    (double)list.Average(
                        x => x.OxygenSaturation),


                MinimumOxygenSaturation =
                    list.Min(
                        x => x.OxygenSaturation),


                MaximumOxygenSaturation =
                    list.Max(
                        x => x.OxygenSaturation),


                AbnormalReadingCount =
                    list.Count(
                        IsAbnormalVital)
            };
        }


        // ============================================================
        // Determine whether a vital sign is abnormal
        // ============================================================

        private bool IsAbnormalVital(
            VitalSign vital)
        {
            return
                vital.HeartRate < 60 ||
                vital.HeartRate > 100 ||

                vital.SystolicPressure < 90 ||
                vital.SystolicPressure > 140 ||

                vital.DiastolicPressure < 60 ||
                vital.DiastolicPressure > 90 ||

                vital.OxygenSaturation < 95;
        }
    }
}