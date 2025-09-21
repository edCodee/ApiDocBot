namespace ApiDocBot.DTO.PatientProfileFreeDTO
{
    public class PatientProfileFreeDiagnosis
    {
        public string UserFirstName { get; set; } = string.Empty;
        public string UserLastName { get; set; } = string.Empty;
        public int PatientProfileFreeId { get; set; }
        public string PatientProfileFreeFirstName { get; set; } = string.Empty;
        public string PatientProfileFreeLastName { get; set; } = string.Empty;
        public string PatientProfileFreeGender { get; set; } = string.Empty;
        public DateTime? PatientProfileFreeBirthDate { get; set; }
        public string DiagnosticMlFreeRiskLevel { get; set; } = string.Empty;
        public string DiagnosticMlFreeRecommendations { get; set; } = string.Empty;
        public bool DiagnosticMlFreeNeedUrgentPsychologist { get; set; }

    }
}
