﻿namespace ApiDocBot.DTO.PatientProfileFreeDTO
{
    public class PatientProfileFreeCreateDTO
    {
        //public int PatientProfileFreeUserSerial { get; set; }
        public string PatientProfileFreeFirstName { get; set; } = string.Empty;
        public string PatientProfileFreeLastName { get; set; } = string.Empty;
        public string PatientProfileFreeGender { get; set; } = string.Empty;
        public DateTime PatientProfileFreeBirthDate { get; set; }
        //public DateTime PatientProfileFreeCreateAt { get; set; }= DateTime.UtcNow;
    }
}
