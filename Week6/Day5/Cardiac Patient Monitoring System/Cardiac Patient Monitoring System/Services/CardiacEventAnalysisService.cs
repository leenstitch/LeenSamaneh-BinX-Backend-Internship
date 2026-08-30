using Cardiac_Patient_Monitoring_System.DTO_S.CardiacEvent_s;
using Cardiac_Patient_Monitoring_System.DTO_S.CardiacEventAnalysisDto_s;
using Cardiac_Patient_Monitoring_System.DTO_S.DiagnosisDto_s;
using Cardiac_Patient_Monitoring_System.DTO_S.HospitalizationDto_s;
using Cardiac_Patient_Monitoring_System.DTO_S.LabResultDto_s;
using Cardiac_Patient_Monitoring_System.DTO_S.MedicalProcedureDto_s;
using Cardiac_Patient_Monitoring_System.DTO_S.MedicationDto_s;
using Cardiac_Patient_Monitoring_System.DTO_S.VitalSignDto_s;
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


            var patientId =
                await _vitalSignRepository
                    .GetPatientIdByUserIdAsync(userId);

            if (patientId == null)
            {
                return null;
            }


            var cardiacEvent =
                await _cardiacEventRepository
                    .GetByIdAsync(cardiacEventId);

            if (cardiacEvent == null)
            {
                return null;
            }


            if (cardiacEvent.PatientId != patientId.Value)
            {
                return null;
            }
            var eventDate =
                cardiacEvent.EventDate;

            var startDate =
                eventDate.AddDays(-daysBefore);



            var vitalSigns =
                await _vitalSignRepository
                    .GetByPatientAndDateRangeAsync(
                        patientId.Value,
                        startDate,
                        eventDate);


            var medications =
                await _medicationRepository
                    .GetHistoricalMedicationsAsync(
                        patientId.Value,
                        startDate,
                        eventDate);



            var diagnoses =
                await _diagnosisRepository
                    .GetRecordedBeforeEventAsync(
                        patientId.Value,
                        eventDate);



            var labResults =
    await _labResultRepository
        .GetForCardiacEventAnalysisAsync(
            patientId.Value,
            startDate,
            eventDate);


       

            var hospitalizations =
                await _hospitalizationRepository
                    .GetOverlappingPeriodAsync(
                        patientId.Value,
                        startDate,
                        eventDate);


  

            var procedures =
                await _medicalProcedureRepository
                    .GetByPatientAndDateRangeAsync(
                        patientId.Value,
                        startDate,
                        eventDate);



            var vitalSummary =
                BuildVitalSummary(vitalSigns);


     

            return new CardiacEventAnalysisResponseDto
            {
             

                CardiacEventId =
                    cardiacEvent.CardiacEventId,

                PatientId =
                    cardiacEvent.PatientId,

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


              

                DaysBeforeEvent =
                    daysBefore,

                AnalysisStartDate =
                    startDate,

                AnalysisEndDate =
                    eventDate,


             

                VitalSigns =
                    vitalSummary,



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


                // ----------------------------------------------------
                // Medications
                // ----------------------------------------------------

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


                // ----------------------------------------------------
                // Diagnoses
                // ----------------------------------------------------

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


                // ----------------------------------------------------
                // Hospitalizations
                // ----------------------------------------------------

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


                // ----------------------------------------------------
                // Medical Procedures
                // ----------------------------------------------------

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
                        })
            };
        }


        // ============================================================
        // Build Vital Signs Summary
        // ============================================================

        private VitalSignSummaryDto BuildVitalSummary(
            IEnumerable<VitalSign> vitalSigns)
        {
            var list =
                vitalSigns.ToList();


            // --------------------------------------------------------
            // No readings
            // --------------------------------------------------------

            if (!list.Any())
            {
                return new VitalSignSummaryDto
                {
                    ReadingCount = 0,
                    AbnormalReadingCount = 0
                };
            }


            // --------------------------------------------------------
            // Minimum and Maximum Heart Rate
            // --------------------------------------------------------

            var minimumHeartRate =
                list.OrderBy(x => x.HeartRate).First();

            var maximumHeartRate =
                list.OrderByDescending(x => x.HeartRate).First();


            // --------------------------------------------------------
            // Minimum and Maximum Systolic Pressure
            // --------------------------------------------------------

            var minimumSystolic =
                list.OrderBy(x => x.SystolicPressure).First();

            var maximumSystolic =
                list.OrderByDescending(
                    x => x.SystolicPressure).First();


            // --------------------------------------------------------
            // Minimum and Maximum Diastolic Pressure
            // --------------------------------------------------------

            var minimumDiastolic =
                list.OrderBy(x => x.DiastolicPressure).First();

            var maximumDiastolic =
                list.OrderByDescending(
                    x => x.DiastolicPressure).First();


            // --------------------------------------------------------
            // Minimum and Maximum Oxygen Saturation
            // --------------------------------------------------------

            var minimumOxygen =
                list.OrderBy(x => x.OxygenSaturation).First();

            var maximumOxygen =
                list.OrderByDescending(
                    x => x.OxygenSaturation).First();


            // --------------------------------------------------------
            // Minimum and Maximum Temperature
            // --------------------------------------------------------

            var minimumTemperature =
                list.OrderBy(x => x.Temperature).First();

            var maximumTemperature =
                list.OrderByDescending(
                    x => x.Temperature).First();


            // --------------------------------------------------------
            // Build summary
            // --------------------------------------------------------

            return new VitalSignSummaryDto
            {
                ReadingCount =
                    list.Count,


                // ----------------------------------------------------
                // Heart Rate
                // ----------------------------------------------------

                AverageHeartRate =
                    list.Average(x => x.HeartRate),

                MinimumHeartRate =
                    minimumHeartRate.HeartRate,

                MinimumHeartRateDate =
                    minimumHeartRate.MeasuredAt,

                MaximumHeartRate =
                    maximumHeartRate.HeartRate,

                MaximumHeartRateDate =
                    maximumHeartRate.MeasuredAt,


                // ----------------------------------------------------
                // Systolic Pressure
                // ----------------------------------------------------

                AverageSystolicPressure =
                    list.Average(x => x.SystolicPressure),

                MinimumSystolicPressure =
                    minimumSystolic.SystolicPressure,

                MinimumSystolicPressureDate =
                    minimumSystolic.MeasuredAt,

                MaximumSystolicPressure =
                    maximumSystolic.SystolicPressure,

                MaximumSystolicPressureDate =
                    maximumSystolic.MeasuredAt,


                // ----------------------------------------------------
                // Diastolic Pressure
                // ----------------------------------------------------

                AverageDiastolicPressure =
                    list.Average(x => x.DiastolicPressure),

                MinimumDiastolicPressure =
                    minimumDiastolic.DiastolicPressure,

                MinimumDiastolicPressureDate =
                    minimumDiastolic.MeasuredAt,

                MaximumDiastolicPressure =
                    maximumDiastolic.DiastolicPressure,

                MaximumDiastolicPressureDate =
                    maximumDiastolic.MeasuredAt,


                // ----------------------------------------------------
                // Oxygen Saturation
                // ----------------------------------------------------

                AverageOxygenSaturation =
                    (double)list.Average(
                        x => x.OxygenSaturation),

                MinimumOxygenSaturation =
                    minimumOxygen.OxygenSaturation,

                MinimumOxygenSaturationDate =
                    minimumOxygen.MeasuredAt,

                MaximumOxygenSaturation =
                    maximumOxygen.OxygenSaturation,

                MaximumOxygenSaturationDate =
                    maximumOxygen.MeasuredAt,


                // ----------------------------------------------------
                // Temperature
                // ----------------------------------------------------

                AverageTemperature =
                    (double)list.Average(
                        x => x.Temperature),

                MinimumTemperature =
                    minimumTemperature.Temperature,

                MinimumTemperatureDate =
                    minimumTemperature.MeasuredAt,

                MaximumTemperature =
                    maximumTemperature.Temperature,

                MaximumTemperatureDate =
                    maximumTemperature.MeasuredAt,


                // ----------------------------------------------------
                // Abnormal Readings
                // ----------------------------------------------------

                AbnormalReadingCount =
                    list.Count(IsAbnormalVital)
            };
        }



        // Checks whether any vital-sign measurement
        // is outside the defined normal range.

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

                vital.OxygenSaturation < 95 ||

                vital.Temperature < 36.5m ||
                vital.Temperature > 37.5m;
        }


        // Creates a new cardiac event for the authenticated patient.
        public async Task<CardiacEventResponseDto?>
      CreateAsync(
          int userId,
          CreateCardiacEventDto dto)
        {
            var patientId =
                await _cardiacEventRepository
                    .GetPatientIdByUserIdAsync(userId);

            if (!patientId.HasValue)
            {
                return null;
            }

            var cardiacEvent = new CardiacEvent
            {
                PatientId =
                    patientId.Value,

                DoctorId =
                    dto.DoctorId,

                EventType =
                    dto.EventType,

                EventDate =
                    dto.EventDate,

                Description =
                    dto.Description,

                Location =
                    dto.Location,

                Outcome =
                    dto.Outcome,

                Notes =
                    dto.Notes,

                CreatedAt =
                    DateTime.UtcNow,

                UpdatedAt =
                    DateTime.UtcNow
            };

            var createdEvent =
                await _cardiacEventRepository
                    .AddAsync(cardiacEvent);

            return new CardiacEventResponseDto
            {
                CardiacEventId =
                    createdEvent.CardiacEventId,

                PatientId =
                    createdEvent.PatientId,

                DoctorId =
                    createdEvent.DoctorId,

                EventType =
                    createdEvent.EventType,

                EventDate =
                    createdEvent.EventDate,

                Description =
                    createdEvent.Description,

                Location =
                    createdEvent.Location,

                Outcome =
                    createdEvent.Outcome,

                Notes =
                    createdEvent.Notes
            };
        }


        // Retrieves the latest vital-sign record recorded
        // before the specified cardiac event.
    ////////    public async Task<VitalSign?>
    ////////GetLatestVitalBeforeEventAsync(
    ////////    int userId,
    ////////    int cardiacEventId)
    ////////    {
    ////////        var patientId =
    ////////            await _cardiacEventRepository
    ////////                .GetPatientIdByUserIdAsync(userId);

    ////////        if (!patientId.HasValue)
    ////////        {
    ////////            return null;
    ////////        }

    ////////        var cardiacEvent =
    ////////            await _cardiacEventRepository
    ////////                .GetByIdAsync(cardiacEventId);

    ////////        if (cardiacEvent == null)
    ////////        {
    ////////            return null;
    ////////        }

    ////////        if (cardiacEvent.PatientId != patientId.Value)
    ////////        {
    ////////            return null;
    ////////        }

    ////////        return await _vitalSignRepository
    ////////            .GetLatestBeforeDateAsync(
    ////////                patientId.Value,
    ////////                cardiacEvent.EventDate);
    ////////    }

    }
    }
