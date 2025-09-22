using System.Security.Principal;

namespace ApiDocBot.DTO.DiagnosticMLMechanicalArmDTO
{
    public class DiagnosticMLMechanicalArmReadDTO
    {
        public int DiagnosticMLMechanicalId { get; set; }
        public int DiagnosticMLMechanicalPatientProfileId { get; set; }
        public string DiagnosticMLMechanicalRiskLevel { get; set; } = string.Empty;
        public string DiagnosticMLMechanicalRecomendations { get; set; } = string.Empty;
        public bool DiagnosticMLMechanicalNeedUrgentPsychologist { get; set; }
        public DateTime DiagnosticMLMechanicalCreateAt { get; set; }
    }
}
