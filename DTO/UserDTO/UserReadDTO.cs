namespace ApiDocBot.DTOs.UserDTOs
{
    public class UserReadDTO
    {
        public int UserSerial { get; set; }
        public string? UserPhoto { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserFirstName { get; set; } = string.Empty;
        public string? UserMiddleName { get; set; }
        public string UserLastName { get; set; } = string.Empty;
        public string? UserSecondLastName { get; set; }
        public DateTime UserBirthDate { get; set; }
        public string UserUsername { get; set; } = string.Empty;
        public string? UserEmail { get; set; }
    }
}
