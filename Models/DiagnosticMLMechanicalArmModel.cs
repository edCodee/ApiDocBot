using System.ComponentModel.DataAnnotations;

namespace ApiDocBot.Models
{
    public class DiagnosticMLMechanicalArmModel
    {
        [Key]
        public int diagnosticMlFree_id { get; set; }
        public int diagnosticMlFree_patientProfileFreeId { get; set; }
        public string diagnosticMlFree_riskLevel { get; set; } = string.Empty;
        public string diagnosticMlFree_recommendations { get; set; } = string.Empty;
        public bool diagnosticMlFree_needUrgentPsychologist { get; set; }
        public DateTime diagnosticMlFree_createdAt { get; set; }

        public DiagnosticMLMechanicalArmModel()
        {

        }

        public DiagnosticMLMechanicalArmModel(int diagnosticMlFree_id, int diagnosticMlFree_patientProfileFreeId, string diagnosticMlFree_riskLevel, string diagnosticMlFree_recommendations, bool diagnosticMlFree_needUrgentPsychologist, DateTime diagnosticMlFree_createdAt)
        {
            this.diagnosticMlFree_id = diagnosticMlFree_id;
            this.diagnosticMlFree_patientProfileFreeId = diagnosticMlFree_patientProfileFreeId;
            this.diagnosticMlFree_riskLevel = diagnosticMlFree_riskLevel;
            this.diagnosticMlFree_recommendations = diagnosticMlFree_recommendations;
            this.diagnosticMlFree_needUrgentPsychologist = diagnosticMlFree_needUrgentPsychologist;
            this.diagnosticMlFree_createdAt = diagnosticMlFree_createdAt;
        }
    }
}
