// This service handles the business logic for patient vital signs.
// It provides operations for retrieving, creating, filtering, updating,
// deleting, and comparing vital-sign records.

using Cardiac_Patient_Monitoring_System.DTO_S.Paginat;
using Cardiac_Patient_Monitoring_System.DTO_S.VitalSignDto_s;
using Cardiac_Patient_Monitoring_System.Interfaces;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;

namespace Cardiac_Patient_Monitoring_System.Services
{
    public class VitalSignService : IVitalSignService
    {
        private readonly IVitalSignRepository _repository;

        public VitalSignService(
            IVitalSignRepository repository)
        {
            _repository = repository;
        }

        // Returns a vital-sign record by its ID.
        public async Task<VitalSignResponseDto?>
            GetByIdAsync(int id)
        {
            var vitalSign =
                await _repository.GetByIdAsync(id);

            if (vitalSign == null)
                return null;

            return new VitalSignResponseDto
            {
                VitalSignId =
                    vitalSign.VitalSignId,

                PatientId =
                    vitalSign.PatientId,

                RecordedByDoctorName =
                    vitalSign.RecordedByDoctorName,

                HeartRate =
                    vitalSign.HeartRate,

                SystolicPressure =
                    vitalSign.SystolicPressure,

                DiastolicPressure =
                    vitalSign.DiastolicPressure,

                OxygenSaturation =
                    vitalSign.OxygenSaturation,

                Temperature =
                    vitalSign.Temperature,

                MeasuredAt =
                    vitalSign.MeasuredAt,

                CreatedAt =
                    vitalSign.CreatedAt,

                Notes =
                    vitalSign.Notes
            };
        }

        // Returns vital-sign records belonging to a specific patient.
        public async Task<IEnumerable<VitalSignResponseDto>>
            GetByPatientIdAsync(int patientId)
        {
            var vitalSigns =
                await _repository.GetByPatientIdAsync(
                    patientId);

            return vitalSigns.Select(vitalSign =>
                new VitalSignResponseDto
                {
                    VitalSignId =
                        vitalSign.VitalSignId,

                    PatientId =
                        vitalSign.PatientId,

                    RecordedByDoctorName =
                        vitalSign.RecordedByDoctorName,

                    HeartRate =
                        vitalSign.HeartRate,

                    SystolicPressure =
                        vitalSign.SystolicPressure,

                    DiastolicPressure =
                        vitalSign.DiastolicPressure,

                    OxygenSaturation =
                        vitalSign.OxygenSaturation,

                    Temperature =
                        vitalSign.Temperature,

                    MeasuredAt =
                        vitalSign.MeasuredAt,

                    CreatedAt =
                        vitalSign.CreatedAt,

                    Notes =
                        vitalSign.Notes
                });
        }

        // Returns all vital-sign records.
        public async Task<PaginatedResponseDto<VitalSignResponseDto>>
    GetAllAsync(VitalSignQueryDto query)
        {
            // Validation
            if (query.Page < 1)
                query.Page = 1;

            if (query.PageSize < 1)
                query.PageSize = 10;

            if (query.PageSize > 100)
                query.PageSize = 100;

            // Get data from repository
            var (data, totalCount) =
                await _repository.GetAllAsync(query);

            // Map Entity -> DTO
            var responseData = data.Select(v => new VitalSignResponseDto
            {
                VitalSignId = v.VitalSignId,
                PatientId = v.PatientId,
                RecordedByDoctorName = v.RecordedByDoctorName,
                HeartRate = v.HeartRate,
                SystolicPressure = v.SystolicPressure,
                DiastolicPressure = v.DiastolicPressure,
                OxygenSaturation = v.OxygenSaturation,
                Temperature = v.Temperature,
                MeasuredAt = v.MeasuredAt,
                CreatedAt = v.CreatedAt,
                Notes = v.Notes
            });

            // Calculate total pages
            var totalPages =
                (int)Math.Ceiling(
                    (double)totalCount / query.PageSize);

            return new PaginatedResponseDto<VitalSignResponseDto>
            {
                Data = responseData,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };
        }
        // this for week 5
        //public async Task<IEnumerable<VitalSignResponseDto>>
        //    GetAllAsync()
        //{
        //    var vitalSigns =
        //        await _repository.GetAllAsync();

        //    return vitalSigns.Select(vitalSign =>
        //        new VitalSignResponseDto
        //        {
        //            VitalSignId =
        //                vitalSign.VitalSignId,

        //            PatientId =
        //                vitalSign.PatientId,

        //            RecordedByDoctorName =
        //                vitalSign.RecordedByDoctorName,

        //            HeartRate =
        //                vitalSign.HeartRate,

        //            SystolicPressure =
        //                vitalSign.SystolicPressure,

        //            DiastolicPressure =
        //                vitalSign.DiastolicPressure,

        //            OxygenSaturation =
        //                vitalSign.OxygenSaturation,

        //            Temperature =
        //                vitalSign.Temperature,

        //            MeasuredAt =
        //                vitalSign.MeasuredAt,

        //            CreatedAt =
        //                vitalSign.CreatedAt,

        //            Notes =
        //                vitalSign.Notes
        //        });
        //}

        // Creates a vital-sign record for the patient linked
        // to the authenticated user.
        public async Task<VitalSignResponseDto?> CreateAsync(
            int userId,
            CreateVitalSignDto dto)
        {
            var patientId =
                await _repository.GetPatientIdByUserIdAsync(userId);

            if (!patientId.HasValue)
                return null;

            var vitalSign = new VitalSign
            {
                PatientId = patientId.Value,

                RecordedByDoctorName =
                    dto.RecordedByDoctorName,

                HeartRate =
                    dto.HeartRate ?? 0,

                SystolicPressure =
                    dto.SystolicPressure ?? 0,

                DiastolicPressure =
                    dto.DiastolicPressure ?? 0,

                OxygenSaturation =
                    dto.OxygenSaturation ?? 0,

                Temperature =
                    dto.Temperature ?? 0,

                MeasuredAt =
                    dto.MeasuredAt ?? DateTime.UtcNow,

                CreatedAt =
                    DateTime.UtcNow,

                Notes =
                    dto.Notes
            };

            var createdVitalSign =
                await _repository.AddAsync(
                    vitalSign);

            return new VitalSignResponseDto
            {
                VitalSignId =
                    createdVitalSign.VitalSignId,

                PatientId =
                    createdVitalSign.PatientId,

                RecordedByDoctorName =
                    createdVitalSign.RecordedByDoctorName,

                HeartRate =
                    createdVitalSign.HeartRate,

                SystolicPressure =
                    createdVitalSign.SystolicPressure,

                DiastolicPressure =
                    createdVitalSign.DiastolicPressure,

                OxygenSaturation =
                    createdVitalSign.OxygenSaturation,

                Temperature =
                    createdVitalSign.Temperature,

                MeasuredAt =
                    createdVitalSign.MeasuredAt,

                CreatedAt =
                    createdVitalSign.CreatedAt,

                Notes =
                    createdVitalSign.Notes
            };
        }

        // Returns vital-sign records belonging to the authenticated patient.
        public async Task<IEnumerable<VitalSignResponseDto>>
            GetMyVitalSignsAsync(int userId)
        {
            var vitalSigns =
                await _repository.GetByUserIdAsync(userId);

            return vitalSigns.Select(vitalSign =>
                new VitalSignResponseDto
                {
                    VitalSignId =
                        vitalSign.VitalSignId,

                    PatientId =
                        vitalSign.PatientId,

                    RecordedByDoctorName =
                        vitalSign.RecordedByDoctorName,

                    HeartRate =
                        vitalSign.HeartRate,

                    SystolicPressure =
                        vitalSign.SystolicPressure,

                    DiastolicPressure =
                        vitalSign.DiastolicPressure,

                    OxygenSaturation =
                        vitalSign.OxygenSaturation,

                    Temperature =
                        vitalSign.Temperature,

                    MeasuredAt =
                        vitalSign.MeasuredAt,

                    CreatedAt =
                        vitalSign.CreatedAt,

                    Notes =
                        vitalSign.Notes
                });
        }

        // Filters vital-sign records using patient information.
        public async Task<IEnumerable<VitalSignResponseDto>>
            FilterAsync(VitalSignFilterDto filter)
        {
            var vitalSigns =
                await _repository.FilterAsync(
                    filter.PatientName,
                    filter.Age,
                    filter.Gender,
                    filter.NationalId);

            return vitalSigns.Select(vitalSign =>
                new VitalSignResponseDto
                {
                    VitalSignId =
                        vitalSign.VitalSignId,

                    PatientId =
                        vitalSign.PatientId,

                    RecordedByDoctorName =
                        vitalSign.RecordedByDoctorName,

                    HeartRate =
                        vitalSign.HeartRate,

                    SystolicPressure =
                        vitalSign.SystolicPressure,

                    DiastolicPressure =
                        vitalSign.DiastolicPressure,

                    OxygenSaturation =
                        vitalSign.OxygenSaturation,

                    Temperature =
                        vitalSign.Temperature,

                    MeasuredAt =
                        vitalSign.MeasuredAt,

                    CreatedAt =
                        vitalSign.CreatedAt,

                    Notes =
                        vitalSign.Notes
                });
        }

        // Updates an existing vital-sign record.
        public async Task<bool> UpdateAsync(
            int id,
            UpdateVitalSignDto dto)
        {
            var vitalSign =
                await _repository.GetByIdAsync(id);

            if (vitalSign == null)
                return false;

            if (dto.HeartRate.HasValue)
            {
                vitalSign.HeartRate =
                    dto.HeartRate.Value;
            }

            if (dto.SystolicPressure.HasValue)
            {
                vitalSign.SystolicPressure =
                    dto.SystolicPressure.Value;
            }

            if (dto.DiastolicPressure.HasValue)
            {
                vitalSign.DiastolicPressure =
                    dto.DiastolicPressure.Value;
            }

            if (dto.OxygenSaturation.HasValue)
            {
                vitalSign.OxygenSaturation =
                    dto.OxygenSaturation.Value;
            }

            if (dto.Temperature.HasValue)
            {
                vitalSign.Temperature =
                    dto.Temperature.Value;
            }

            if (dto.MeasuredAt.HasValue)
            {
                vitalSign.MeasuredAt =
                    dto.MeasuredAt.Value;
            }

            if (dto.Notes != null)
            {
                vitalSign.Notes =
                    dto.Notes;
            }

            if (dto.RecordedByDoctorName != null)
            {
                vitalSign.RecordedByDoctorName =
                    dto.RecordedByDoctorName;
            }

            await _repository.UpdateAsync(
                vitalSign);

            return true;
        }

        // Deletes an existing vital-sign record.
        public async Task<bool> DeleteAsync(int id)
        {
            var vitalSign =
                await _repository.GetByIdAsync(id);

            if (vitalSign == null)
                return false;

            await _repository.DeleteAsync(
                vitalSign);

            return true;
        }

        // Compares the patient's latest two vital-sign records
        // and determines the change and status for each measurement.
        public async Task<VitalSignComparisonDto?>
            CompareLatestTwoAsync(int userId)
        {
            var vitalSigns =
                await _repository.GetLatestTwoByUserIdAsync(userId);

            if (vitalSigns.Count < 2)
                return null;

            var latest =
                vitalSigns[0];

            var previous =
                vitalSigns[1];

            return new VitalSignComparisonDto
            {
                Previous = new VitalSignValuesDto
                {
                    HeartRate =
                        previous.HeartRate,

                    SystolicPressure =
                        previous.SystolicPressure,

                    DiastolicPressure =
                        previous.DiastolicPressure,

                    OxygenSaturation =
                        previous.OxygenSaturation,

                    Temperature =
                        previous.Temperature,

                    MeasuredAt =
                        previous.MeasuredAt
                },

                Latest = new VitalSignValuesDto
                {
                    HeartRate =
                        latest.HeartRate,

                    SystolicPressure =
                        latest.SystolicPressure,

                    DiastolicPressure =
                        latest.DiastolicPressure,

                    OxygenSaturation =
                        latest.OxygenSaturation,

                    Temperature =
                        latest.Temperature,

                    MeasuredAt =
                        latest.MeasuredAt
                },

                Comparison = new VitalSignComparisonValuesDto
                {
                    HeartRate =
                        CompareLowerIsBetter(
                            previous.HeartRate,
                            latest.HeartRate),

                    SystolicPressure =
                        CompareLowerIsBetter(
                            previous.SystolicPressure,
                            latest.SystolicPressure),

                    DiastolicPressure =
                        CompareLowerIsBetter(
                            previous.DiastolicPressure,
                            latest.DiastolicPressure),

                    OxygenSaturation =
                        CompareHigherIsBetter(
                            previous.OxygenSaturation,
                            latest.OxygenSaturation),

                    Temperature =
                        CompareTemperature(
                            previous.Temperature,
                            latest.Temperature)
                }
            };
        }

        // Compares measurements where a higher value is considered better.
        private static VitalSignMetricComparisonDto
            CompareHigherIsBetter(
                decimal previous,
                decimal latest)
        {
            var change =
                latest - previous;

            string status;

            if (latest > previous)
                status = "Improved";
            else if (latest < previous)
                status = "Worsened";
            else
                status = "No Change";

            return new VitalSignMetricComparisonDto
            {
                Change =
                    change,

                Status =
                    status
            };
        }

        // Compares temperature based on its distance from
        // the defined normal temperature range.
        private static VitalSignMetricComparisonDto
            CompareTemperature(
                decimal previous,
                decimal latest)
        {
            const decimal normalMin = 36.5m;
            const decimal normalMax = 37.5m;

            var previousDistance =
                previous < normalMin
                    ? normalMin - previous
                    : previous > normalMax
                        ? previous - normalMax
                        : 0;

            var latestDistance =
                latest < normalMin
                    ? normalMin - latest
                    : latest > normalMax
                        ? latest - normalMax
                        : 0;

            var change =
                latest - previous;

            string status;

            if (latestDistance < previousDistance)
                status = "Improved";
            else if (latestDistance > previousDistance)
                status = "Worsened";
            else
                status = "No Change";

            return new VitalSignMetricComparisonDto
            {
                Change =
                    change,

                Status =
                    status
            };
        }

        // Compares measurements where a lower value is considered better.
        private static VitalSignMetricComparisonDto
            CompareLowerIsBetter(
                decimal previous,
                decimal latest)
        {
            var change =
                latest - previous;

            string status;

            if (latest < previous)
                status = "Improved";
            else if (latest > previous)
                status = "Worsened";
            else
                status = "No Change";

            return new VitalSignMetricComparisonDto
            {
                Change =
                    change,

                Status =
                    status
            };
        }

        // Compares vital-sign records from two selected dates.
        public async Task<VitalSignDateComparisonDto?>
            CompareByDatesAsync(
                int userId,
                DateTime firstDate,
                DateTime secondDate)
        {
            var firstVitalSign =
                await _repository.GetLatestByUserIdAndDateAsync(
                    userId,
                    firstDate);

            var secondVitalSign =
                await _repository.GetLatestByUserIdAndDateAsync(
                    userId,
                    secondDate);

            if (firstVitalSign == null ||
                secondVitalSign == null)
            {
                return null;
            }

            return new VitalSignDateComparisonDto
            {
                FirstDate = new VitalSignValuesDto
                {
                    HeartRate =
                        firstVitalSign.HeartRate,

                    SystolicPressure =
                        firstVitalSign.SystolicPressure,

                    DiastolicPressure =
                        firstVitalSign.DiastolicPressure,

                    OxygenSaturation =
                        firstVitalSign.OxygenSaturation,

                    Temperature =
                        firstVitalSign.Temperature,

                    MeasuredAt =
                        firstVitalSign.MeasuredAt
                },

                SecondDate = new VitalSignValuesDto
                {
                    HeartRate =
                        secondVitalSign.HeartRate,

                    SystolicPressure =
                        secondVitalSign.SystolicPressure,

                    DiastolicPressure =
                        secondVitalSign.DiastolicPressure,

                    OxygenSaturation =
                        secondVitalSign.OxygenSaturation,

                    Temperature =
                        secondVitalSign.Temperature,

                    MeasuredAt =
                        secondVitalSign.MeasuredAt
                },

                Comparison = new VitalSignComparisonValuesDto
                {
                    HeartRate =
                        CompareLowerIsBetter(
                            firstVitalSign.HeartRate,
                            secondVitalSign.HeartRate),

                    SystolicPressure =
                        CompareLowerIsBetter(
                            firstVitalSign.SystolicPressure,
                            secondVitalSign.SystolicPressure),

                    DiastolicPressure =
                        CompareLowerIsBetter(
                            firstVitalSign.DiastolicPressure,
                            secondVitalSign.DiastolicPressure),

                    OxygenSaturation =
                        CompareHigherIsBetter(
                            firstVitalSign.OxygenSaturation,
                            secondVitalSign.OxygenSaturation),

                    Temperature =
                        CompareTemperature(
                            firstVitalSign.Temperature,
                            secondVitalSign.Temperature)
                }
            };
        }
    }
}