namespace Cardiac_Patient_Monitoring_System.DTO_S.VitalSignDto_s
{
    public class VitalSignSummaryDto
    {
        public int ReadingCount { get; set; }

        // Heart Rate
        public double? AverageHeartRate { get; set; }

        public int? MinimumHeartRate { get; set; }
        public DateTime? MinimumHeartRateDate { get; set; }

        public int? MaximumHeartRate { get; set; }
        public DateTime? MaximumHeartRateDate { get; set; }


        // Systolic Blood Pressure
        public double? AverageSystolicPressure { get; set; }

        public int? MinimumSystolicPressure { get; set; }
        public DateTime? MinimumSystolicPressureDate { get; set; }

        public int? MaximumSystolicPressure { get; set; }
        public DateTime? MaximumSystolicPressureDate { get; set; }


        // Diastolic Blood Pressure
        public double? AverageDiastolicPressure { get; set; }

        public int? MinimumDiastolicPressure { get; set; }
        public DateTime? MinimumDiastolicPressureDate { get; set; }

        public int? MaximumDiastolicPressure { get; set; }
        public DateTime? MaximumDiastolicPressureDate { get; set; }


        // Oxygen Saturation
        public double? AverageOxygenSaturation { get; set; }

        public decimal? MinimumOxygenSaturation { get; set; }
        public DateTime? MinimumOxygenSaturationDate { get; set; }

        public decimal? MaximumOxygenSaturation { get; set; }
        public DateTime? MaximumOxygenSaturationDate { get; set; }


        // Temperature
        public double? AverageTemperature { get; set; }

        public decimal? MinimumTemperature { get; set; }
        public DateTime? MinimumTemperatureDate { get; set; }

        public decimal? MaximumTemperature { get; set; }
        public DateTime? MaximumTemperatureDate { get; set; }


        // Abnormal readings
        public int AbnormalReadingCount { get; set; }
        
    }
}