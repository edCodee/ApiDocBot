namespace ApiDocBot.DTO.AnswerFreeDTO
{
    public class AnswerFreeReadDTO
    {
        public int AnswerFreeId { get; set; }
        public int AnswerFreePatientProfileFreeId { get; set; }
        public int AnswerFreeQuestionFreeId { get; set; }
        public string AnswerFreeAnswer { get; set; } = string.Empty;
        public DateTime AnswerFreeDate { get; set; }
    }
}
