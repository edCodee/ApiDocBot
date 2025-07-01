using ApiDocBot.Data;
using ApiDocBot.DTO.UserDTO;
using ApiDocBot.DTOs.RoleDTO;
using ApiDocBot.DTOs.UserDTOs;
using ApiDocBot.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ApiDocBot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Hash con SHA256
        /// </summary>
        private static byte[] HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        [HttpGet("roles")]
        public async Task<ActionResult<IEnumerable<RoleReadDTO>>> GetRoles()
        {
            var roles = await _context.role
                .Select(r => new RoleReadDTO
                {
                    RoleSerial = r.role_serial,
                    RoleName = r.role_name
                })
                .ToListAsync();

            return Ok(roles);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadDTO>>> GetUsers()
        {
            var users = await _context.user
                .Select(u => new UserReadDTO
                {
                    UserSerial = u.user_serial,
                    UserPhoto = u.user_photo != null ? Convert.ToBase64String(u.user_photo) : null,
                    UserId = u.user_ide,
                    UserFirstName = u.user_firstName,
                    UserMiddleName = u.user_middleName,
                    UserLastName = u.user_lastName,
                    UserSecondLastName = u.user_secondLastname,
                    UserBirthDate = u.user_birthDate,
                    UserUsername = u.user_userName,
                    UserEmail = u.user_email
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<UserReadDTO>> CreateUser(UserCreateDTO dto)
        {
            var user = new UserModel
            {
                //user_photo = string.IsNullOrWhiteSpace(dto.UserPhoto) ? null : Convert.FromBase64String(dto.UserPhoto),
                user_ide = dto.UserId,
                user_firstName = dto.UserFirstName,
                user_middleName = string.IsNullOrWhiteSpace(dto.UserMiddleName) ? null : dto.UserMiddleName,
                user_lastName = dto.UserLastName,
                user_secondLastname = string.IsNullOrWhiteSpace(dto.UserSecondLastName) ? null : dto.UserSecondLastName,
                user_birthDate = dto.UserBirthDate.Date,
                user_userName = dto.UserUsername,
                user_email = string.IsNullOrWhiteSpace(dto.UserEmail) ? null : dto.UserEmail,
                user_password = HashPassword(dto.UserPassword)
            };

            _context.user.Add(user);
            await _context.SaveChangesAsync();

            // Asignar rol por defecto (paciente)
            var userRole = new UserRoleModel
            {
                userRole_userSerial = user.user_serial,
                userRole_roleSerial = 2, // rol 2 por defecto
                assigned_at = DateTime.Now
            };
            _context.user_role.Add(userRole);
            await _context.SaveChangesAsync();

            var result = new UserReadDTO
            {
                UserSerial = user.user_serial,
                UserPhoto = user.user_photo != null ? Convert.ToBase64String(user.user_photo) : null,
                UserId = user.user_ide,
                UserFirstName = user.user_firstName,
                UserMiddleName = user.user_middleName,
                UserLastName = user.user_lastName,
                UserSecondLastName = user.user_secondLastname,
                UserBirthDate = user.user_birthDate,
                UserUsername = user.user_userName,
                UserEmail = user.user_email
            };

            return CreatedAtAction(nameof(GetUsers), new { id = user.user_serial }, result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.UserId) || string.IsNullOrWhiteSpace(request.UserPassword))
                return BadRequest("Cédula y contraseña requeridas.");

            try
            {
                var user = await _context.user
                    .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(u => u.user_ide == request.UserId);

                if (user == null)
                    return Unauthorized("Usuario no encontrado.");

                var inputPasswordHashed = HashPassword(request.UserPassword);

                if (!inputPasswordHashed.SequenceEqual(user.user_password))
                    return Unauthorized("Contraseña incorrecta.");

                var key = Encoding.ASCII.GetBytes("pSKD93kdFJls00dkfJ2kfjLSKdj38DKSlskfjd94sldkfj"); // ⚠️ mover a appsettings

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.user_userName),
                    new Claim("cedula", user.user_ide),
                    new Claim("user_serial", user.user_serial.ToString()),
                    new Claim("nombre", user.user_firstName),
                    new Claim("apellido", user.user_lastName),
                    new Claim("roles", string.Join(",", user.UserRoles.Select(r => r.Role!.role_name)))
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = new JwtSecurityTokenHandler().WriteToken(
                    new JwtSecurityTokenHandler().CreateToken(tokenDescriptor)
                );

                return Ok(new
                {
                    token,
                    user_serial = user.user_serial,
                    cedula = user.user_ide,
                    userName = user.user_userName,
                    nombre = user.user_firstName,
                    segundoNombre = user.user_middleName,
                    apellido = user.user_lastName,
                    segundoApellido = user.user_secondLastname,
                    email = user.user_email,
                    roles = user.UserRoles.Select(r => new
                    {
                        roleSerial = r.Role!.role_serial,
                        roleName = r.Role.role_name
                    })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetUserCount()
        {
            var totalUsers = await _context.user.CountAsync();
            return Ok(totalUsers);
        }


        [HttpGet("latest-diagnostics")]
        public async Task<ActionResult<IEnumerable<DiagnosticSummaryDTO>>> GetLatestDiagnostics()
        {
            var data = await (from u in _context.user
                              join p in _context.patient_profile_free
                                  on u.user_serial equals p.patientProfileFree_userSerial
                              join d in _context.diagnostic_ml_free
                                  on p.patientProfileFree_id equals d.diagnosticMlFree_patientProfileFreeId
                              orderby d.diagnosticMlFree_id descending
                              select new DiagnosticSummaryDTO
                              {
                                  PatientFirstName = p.patientProfileFree_firstName,
                                  PatientLastName = p.patientProfileFree_lastName,
                                  RiskLevel = d.diagnosticMlFree_riskLevel
                              })
                              .Take(10)
                              .ToListAsync();

            return Ok(data);
        }





    }
}
