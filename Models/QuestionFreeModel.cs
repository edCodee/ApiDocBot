using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;

namespace ApiDocBot.Models
{
    public class QuestionFreeModel
    {
        [Key]
        public int questionFree_id { get; set; }
        public string questionFree_text { get; set; } = string.Empty;
        public string questionFree_type { get; set; } = string.Empty;

        public QuestionFreeModel() { }

        public QuestionFreeModel(int questionFree_id, string questionFree_text, string questionFree_type)
        {
            this.questionFree_id = questionFree_id;
            this.questionFree_text = questionFree_text;
            this.questionFree_type = questionFree_type;
        }
    }
}
