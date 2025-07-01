namespace ApiDocBot.DTOs.PsychologistProfileDTOs
{
    public class PsychologistProfileCreateDTOs
    {
        public int PsychologistProfileId { get; set; }
        public string PsychologistProfileLicenseNumber { get; set; }=string.Empty;
        public string PsychologistProfileSpeciality { get; set; } = string.Empty;
        public int PsychologistProfileYearsEsperience { get; set; }
        public string PsychologistProfileWorkInstitution { get; set; } = string.Empty;
        public string PsychologistProfileContactPhone { get; set; } = string.Empty;
        public string PsychologistProfileBiography { get; set; } = string.Empty;
        public DateTime PsychologistProfileCreateAt { get; set; }
        public DateTime PsychologistProfileUpdateAt { get; set; }
    }
}
