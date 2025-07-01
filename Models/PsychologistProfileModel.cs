using System.ComponentModel.DataAnnotations;

namespace ApiDocBot.Models
{
    public class PsychologistProfileModel
    {
        [Key]
        public int psychologistProfile_id { get; set; }
        public string psychologistProfile_licenseNumber { get; set; }
        public string psychologistProfile_speciality { get; set; }
        public int psychologistProfile_yearsEsperience { get; set; }
        public string psychologistProfile_workInstitution { get; set; }
        public string psychologistProfile_contactPhone { get; set; }
        public string psychologistProfile_biography { get; set; }
        public DateTime psychologistProfile_createAt { get; set; }
        public DateTime psychologistProfile_updateAt { get; set; }

        public PsychologistProfileModel() { }

        public PsychologistProfileModel(int psychologistProfile_id, string psychologistProfile_licenseNumber, string psychologistProfile_speciality, int psychologistProfile_yearsEsperience, string psychologistProfile_workInstitution, string psychologistProfile_contactPhone, string psychologistProfile_biography, DateTime psychologistProfile_createAt, DateTime psychologistProfile_updateAt)
        {
            this.psychologistProfile_id = psychologistProfile_id;
            this.psychologistProfile_licenseNumber = psychologistProfile_licenseNumber;
            this.psychologistProfile_speciality = psychologistProfile_speciality;
            this.psychologistProfile_yearsEsperience = psychologistProfile_yearsEsperience;
            this.psychologistProfile_workInstitution = psychologistProfile_workInstitution;
            this.psychologistProfile_contactPhone = psychologistProfile_contactPhone;
            this.psychologistProfile_biography = psychologistProfile_biography;
            this.psychologistProfile_createAt = psychologistProfile_createAt;
            this.psychologistProfile_updateAt = psychologistProfile_updateAt;
        }
    }
}
