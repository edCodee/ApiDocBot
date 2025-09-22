namespace ApiDocBot.DTO.PatientIndicatorEvaluationDTO
{
    public class PatientIndicatorEvaluationCreateDTO
    {
        public int PatientIndicatorEvaluationPatientProfileFreeId { get; set; }
        public int PatientIndicatorEvaluationIndicatorCatalogId { get; set; }
        public int PatientIndicatorEvaluationScore { get; set; }
        public string PatientIndicatorEvaluationObservation { get; set; }=string.Empty;

    }
}
