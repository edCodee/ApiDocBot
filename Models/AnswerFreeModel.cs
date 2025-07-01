using System.ComponentModel.DataAnnotations;

namespace ApiDocBot.Models
{
    public class AnswerFreeModel
    {
        [Key]
        public int answerFree_id { get; set; }
        public int answerFree_patientProfileFreeId { get; set; }
        public int answerFree_questionFreeId { get; set; }
        public string answerFree_answer { get; set; } = string.Empty;
        public DateTime answerFree_date { get; set; }

        public AnswerFreeModel() { }

        public AnswerFreeModel(int answerFree_id, int answerFree_patientProfileFreeId, int answerFree_questionFreeId, string answerFree_answer, DateTime answerFree_date)
        {
            this.answerFree_id = answerFree_id;
            this.answerFree_patientProfileFreeId = answerFree_patientProfileFreeId;
            this.answerFree_questionFreeId = answerFree_questionFreeId;
            this.answerFree_answer = answerFree_answer;
            this.answerFree_date = answerFree_date;
        }
    }
}
