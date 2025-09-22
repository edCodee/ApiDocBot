using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ApiDocBot.Models
{
    public class PatientIndicatorEvaluationModel
    {
        [Key]
        public int patientIndicatorEvaluation_id { get; set; }
        public int patientIndicatorEvaluation_patientProfileFreeId { get; set; }
        public int patientIndicatorEvaluation_indicatorCatalogId { get; set; }
        public int patientIndicatorEvaluation_score { get; set; }
        public string patientIndicatorEvaluation_observation { get; set; }=string.Empty;
        public DateTime patientIndicatorEvaluation_date { get; set; }

        public PatientIndicatorEvaluationModel()
        {

        }

        public PatientIndicatorEvaluationModel(int patientIndicatorEvaluation_id, int patientIndicatorEvaluation_patientProfileFreeId, int patientIndicatorEvaluation_indicatorCatalogId, int patientIndicatorEvaluation_score, string patientIndicatorEvaluation_observation, DateTime patientIndicatorEvaluation_date)
        {
            this.patientIndicatorEvaluation_id = patientIndicatorEvaluation_id;
            this.patientIndicatorEvaluation_patientProfileFreeId = patientIndicatorEvaluation_patientProfileFreeId;
            this.patientIndicatorEvaluation_indicatorCatalogId = patientIndicatorEvaluation_indicatorCatalogId;
            this.patientIndicatorEvaluation_score = patientIndicatorEvaluation_score;
            this.patientIndicatorEvaluation_observation = patientIndicatorEvaluation_observation;
            this.patientIndicatorEvaluation_date = patientIndicatorEvaluation_date;
        }
    }
}
