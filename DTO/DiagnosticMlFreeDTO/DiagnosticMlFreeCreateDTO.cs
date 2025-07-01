namespace ApiDocBot.DTO.DiagnosticMlFreeDTO
{
    public class DiagnosticMlFreeCreateDTO
    {
        public int DiagnosticMlFreePatientProfileFreeId { get; set; }
        public string DiagnosticMlFreeRiskLevel { get; set; } = string.Empty;
        public string DiagnosticMlFreeRecommendations { get; set; } = string.Empty;
        public bool DiagnosticMlFreeNeedUrgentPsychologist { get; set; }
        public DateTime DiagnosticMlFreeCreatedAt { get; set; }
    }
}
