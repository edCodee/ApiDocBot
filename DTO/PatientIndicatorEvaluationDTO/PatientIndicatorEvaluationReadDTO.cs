namespace ApiDocBot.DTO.PatientIndicatorEvaluationDTO
{
    public class PatientIndicatorEvaluationReadDTO
    {
        public int PatientIndicatorEvaluationId { get; set; }
        public int PatientIndicatorEvaluationPatientProfileFreeId { get; set; }
        public int PatientIndicatorEvaluationIndicatorCatalogId { get; set; }
        public int PatientIndicatorEvaluationScore { get; set; }
        public string PatientIndicatorEvaluationObservation { get; set; } = string.Empty;
        public DateTime PatientIndicatorEvaluationDate { get; set; }
    }
}
