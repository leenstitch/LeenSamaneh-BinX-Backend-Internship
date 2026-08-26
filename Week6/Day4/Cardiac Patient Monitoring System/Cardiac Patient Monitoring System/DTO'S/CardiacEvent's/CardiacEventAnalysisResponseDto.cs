using Cardiac_Patient_Monitoring_System.DTO_S.DiagnosisDto_s;
using Cardiac_Patient_Monitoring_System.DTO_S.HospitalizationDto_s;
using Cardiac_Patient_Monitoring_System.DTO_S.LabResultDto_s;
using Cardiac_Patient_Monitoring_System.DTO_S.MedicalProcedureDto_s;
using Cardiac_Patient_Monitoring_System.DTO_S.MedicationDto_s;
using Cardiac_Patient_Monitoring_System.DTO_S.VitalSignDto_s;
using Cardiac_Patient_Monitoring_System.DTOs;

namespace Cardiac_Patient_Monitoring_System.DTO_S.CardiacEventAnalysisDto_s
{
    public class CardiacEventAnalysisResponseDto
    {
        // Cardiac event information
        public int CardiacEventId { get; set; }

        public int PatientId { get; set; }

        public string EventType { get; set; } = string.Empty;

        public DateTime EventDate { get; set; }

        public string? Description { get; set; }

        public string? Location { get; set; }

        public string? Outcome { get; set; }


        // Analysis period
        public int DaysBeforeEvent { get; set; }

        public DateTime AnalysisStartDate { get; set; }

        public DateTime AnalysisEndDate { get; set; }


        // Vital signs summary
        public VitalSignSummaryDto VitalSigns { get; set; } = new();


        // Medical history
        public IEnumerable<LabResultResponseDto> LabResults { get; set; }
            = new List<LabResultResponseDto>();

        public IEnumerable<MedicationResponseDto> Medications { get; set; }
            = new List<MedicationResponseDto>();

        public IEnumerable<DiagnosisResponseDto> Diagnoses { get; set; }
            = new List<DiagnosisResponseDto>();

        public IEnumerable<MedicalProcedureResponseDto> MedicalProcedures { get; set; }
            = new List<MedicalProcedureResponseDto>();

        public IEnumerable<HospitalizationResponseDto> Hospitalizations { get; set; }
            = new List<HospitalizationResponseDto>();
    }
}