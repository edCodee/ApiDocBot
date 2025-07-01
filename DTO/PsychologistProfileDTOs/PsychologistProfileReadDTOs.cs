namespace ApiDocBot.DTOs.PsychologistProfileDTOs
{
    public class PsychologistProfileReadDTOs
    {
        public int PsychologistProfileId { get; set; }
        public string PsychologistProfileLicenseNumber { get; set; }
        public string PsychologistProfileSpeciality { get; set; }
        public int PsychologistProfileYearsEsperience { get; set; }
        public string PsychologistProfileWorkInstitution { get; set; }
        public string PsychologistProfileContactPhone { get; set; }
        public string PsychologistProfileBiography { get; set; }
        public DateTime PsychologistProfileCreateAt { get; set; }
        public DateTime PsychologistProfileUpdateAt { get; set; }
    }
}
