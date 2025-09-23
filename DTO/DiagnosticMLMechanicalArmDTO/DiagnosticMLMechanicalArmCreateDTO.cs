using System.Reflection.Metadata.Ecma335;

namespace ApiDocBot.DTO.DiagnosticMLMechanicalArmDTO
{
    public class DiagnosticMLMechanicalArmCreateDTO
    {
        public int DiagnosticMLMechanicalArmId { get; set; }
        public int DiagnosticMLMechanicalArmPatientProfileFreeId { get; set; }
        public string DiagnosticMLMechanicalArmRiskLevel { get; set; } = string.Empty;
        public string DiagnosticMLMechanicalArmRecomendation { get; set; } = string.Empty;
        public bool DiagnosticMLMechanicalArmNeedUrgenPsychologist { get; set; }
        public DateTime DiagnosticMLMechanicalArmCreateAt { get; set; }
    }
}
