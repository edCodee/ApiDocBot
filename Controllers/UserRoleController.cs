using Microsoft.AspNetCore.Mvc;
using ApiDocBot.Data;
using ApiDocBot.DTOs.UserDTOs;
using ApiDocBot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;
using ApiDocBot.DTOs.UserRoleDTOs;

namespace ApiDocBot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserRoleController:ControllerBase
    {
        private readonly AppDbContext _context;

        public UserRoleController(AppDbContext context)
        {
            _context = context;
        }

        //GET: api/userrole
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserRolReadDTO>>> GetUserRole()
        {
            var userRoles=await _context.user_role.ToListAsync();

            var userRoleDTOs = userRoles.Select(f => new UserRolReadDTO
            {
                UserRoleUserSerial = f.userRole_userSerial,
                UserRoleRoleSerial = f.userRole_userSerial,
                UserRoleAssignedAt = f.assigned_at ?? DateTime.MinValue
            });
            return Ok(userRoleDTOs);
        }

        //POST: api/userrole
        [HttpPost]
        public async Task<ActionResult<UserRolReadDTO>> CreateUserRole([FromBody] UserRoleCreateDTO userRoleDTOs)
        {
            // Validación opcional: evitar duplicados
            var exists = await _context.user_role.FindAsync(userRoleDTOs.UserRoleUserSerial, userRoleDTOs.UserRoleRoleSerial);
            if (exists != null)
            {
                return Conflict("Este rol ya está asignado al usuario.");
            }

            var userRole = new UserRoleModel
            {
                userRole_userSerial = userRoleDTOs.UserRoleUserSerial,
                userRole_roleSerial = userRoleDTOs.UserRoleRoleSerial,
                assigned_at = userRoleDTOs.UserRolesAssinedAt
            };

            _context.user_role.Add(userRole);
            await _context.SaveChangesAsync();

            var result = new UserRolReadDTO
            {
                UserRoleUserSerial = userRole.userRole_userSerial,
                UserRoleRoleSerial = userRole.userRole_roleSerial,
                UserRoleAssignedAt = userRole.assigned_at ?? DateTime.MinValue
            };

            // Como no hay ID único, devolvemos los dos seriales como identificación
            return CreatedAtAction(nameof(GetUserRole), new
            {
                userSerial = userRole.userRole_userSerial,
                roleSerial = userRole.userRole_roleSerial
            }, result);
        }

    }
}
