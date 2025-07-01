using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;

namespace ApiDocBot.Models
{
    public class PatientProfileFreeModel
    {
        [Key]
        public int patientProfileFree_id { get; set; }
        public int patientProfileFree_userSerial { get; set; }
        public string patientProfileFree_firstName { get; set; } = string.Empty;
        public string patientProfileFree_lastName { get; set; } = string.Empty;
        public string patientProfileFree_gender { get; set; } = string.Empty;
        public DateTime patientProfileFree_birthDate { get; set; }
        public DateTime patientProfileFree_createdAt { get; set; }
        public PatientProfileFreeModel() { }

        public PatientProfileFreeModel(int patientProfileFree_id, int patientProfileFree_userSerial, string patientProfileFree_firstName, string patientProfileFree_lastName, string patientProfileFree_gender, DateTime patientProfileFree_birthDate, DateTime patientProfileFree_createdAt)
        {
            this.patientProfileFree_id = patientProfileFree_id;
            this.patientProfileFree_userSerial = patientProfileFree_userSerial;
            this.patientProfileFree_firstName = patientProfileFree_firstName;
            this.patientProfileFree_lastName = patientProfileFree_lastName;
            this.patientProfileFree_gender = patientProfileFree_gender;
            this.patientProfileFree_birthDate = patientProfileFree_birthDate;
            this.patientProfileFree_createdAt = patientProfileFree_createdAt;
        }
    }
}
