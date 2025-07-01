using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;

namespace ApiDocBot.DTOs.UserRoleDTOs
{
    public class UserRolReadDTO
    {
        public int UserRoleUserSerial { get; set; }
        public int UserRoleRoleSerial { get; set; }
        public DateTime UserRoleAssignedAt { get; set; }
    }
}
