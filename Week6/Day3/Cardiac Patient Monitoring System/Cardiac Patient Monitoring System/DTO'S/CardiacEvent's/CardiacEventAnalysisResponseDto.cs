using Cardiac_Patient_Monitoring_System.DTO_S.CardiacEvent_s;
using Cardiac_Patient_Monitoring_System.DTO_S.HospitalizationDto_s;
using Cardiac_Patient_Monitoring_System.DTO_S.LabResultDto_s;
using Cardiac_Patient_Monitoring_System.DTO_S.MedicalProcedureDto_s;
using Cardiac_Patient_Monitoring_System.DTO_S.MedicationDto_s;
using Cardiac_Patient_Monitoring_System.DTO_S.VitalSignDto_s;
using Cardiac_Patient_Monitoring_System.DTOs;

namespace Cardiac_Patient_Monitoring_System.DTO_S.CardiacEvent_s
{
    public class CardiacEventAnalysisResponseDto
    {
        public CardiacEventResponseDto CardiacEvent { get; set; }
            = null!;

        public DateTime AnalysisFrom { get; set; }

        public DateTime AnalysisTo { get; set; }

        public VitalSignSummaryDto VitalSummary { get; set; }
            = null!;

        public IEnumerable<CardiacEventVitalDto> VitalSigns { get; set; }
            = [];

        public IEnumerable<LabResultResponseDto> LabResults { get; set; }
            = [];

        public IEnumerable<MedicationResponseDto> Medications { get; set; }
            = [];

        public IEnumerable<DiagnosisResponseDto> Diagnoses { get; set; }
            = [];

        public IEnumerable<CardiacEventResponseDto> PreviousCardiacEvents { get; set; }
            = [];

        public IEnumerable<HospitalizationResponseDto> Hospitalizations { get; set; }
            = [];

        public IEnumerable<MedicalProcedureResponseDto> MedicalProcedures { get; set; }
            = [];
    }
}