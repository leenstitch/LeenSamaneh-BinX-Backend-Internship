namespace Cardiac_Patient_Monitoring_System.DTO_S.AdminDashboardDto_s
{
    public class AdminDashboardOverviewDto
    {
        public int TotalPatients { get; set; }

        public int TotalVitalSigns { get; set; }

        public int TotalMedications { get; set; }

        public int TotalDiagnoses { get; set; }

        public int TotalAppointments { get; set; }

        public int ScheduledAppointments { get; set; }

        public int CompletedAppointments { get; set; }

        public int CancelledAppointments { get; set; }

        public int AppointmentsToday { get; set; }

        public int PatientsNeedingAttention { get; set; }
    }
}