using ApiDocBot.Data;
using ApiDocBot.DTO.PatientProfileFreeDTO;
using ApiDocBot.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ApiDocBot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientProfileFreeController:ControllerBase
    {
        private readonly AppDbContext _context;
        public PatientProfileFreeController(AppDbContext context)   
        {
            _context = context;
        }

        // GET: api/patientprofilefreelist
        [HttpGet("patientprofilefreelist")]
        public async Task<ActionResult<IEnumerable<PatientProfileFreeListReadDTO>>> GetPatientProfileList()
        {
            var patientList = await (
                from u in _context.user
                join p in _context.patient_profile_free
                    on u.user_serial equals p.patientProfileFree_userSerial
                select new PatientProfileFreeListReadDTO
                {
                    UserFirstName = u.user_firstName,
                    UserLastName = u.user_lastName,
                    PatientProfileFreeFirstName = p.patientProfileFree_firstName,
                    PatientProfileFreeLastName = p.patientProfileFree_lastName
                }
            ).ToListAsync();

            return Ok(patientList);
        }

        //GET:api/patientprofilefree
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientProfileFreeReadDTO>>> GetPatientProfileFree()
        {
            var patient = await _context.patient_profile_free.ToListAsync();

            var patientDTO = patient.Select(f => new PatientProfileFreeReadDTO
            {
                PatientProfileFreeId = f.patientProfileFree_id,
                PatientProfileFreeUserSerial = f.patientProfileFree_userSerial,
                PatientProfileFreeFirstName = f.patientProfileFree_firstName,
                PatientProfileFreeLastName = f.patientProfileFree_lastName,
                PatientProfileFreeGender = f.patientProfileFree_gender,
                PatientProfileFreeBirthDate = f.patientProfileFree_birthDate,
                PatientProfileFreeCreateAt = f.patientProfileFree_createdAt
            });
            return Ok(patientDTO);
        }

        //POST:api/patienteprofilefree
        [HttpPost]
        public async Task<ActionResult<PatientProfileFreeCreateDTO>> CreatePatientProfileFree(PatientProfileFreeCreateDTO patientDTO)
        {
            var patient = new PatientProfileFreeModel
            {
                //patientProfileFree_userSerial = patientDTO.PatientProfileFreeUserSerial,
                patientProfileFree_firstName = patientDTO.PatientProfileFreeFirstName,
                patientProfileFree_lastName = patientDTO.PatientProfileFreeLastName,
                patientProfileFree_gender = patientDTO.PatientProfileFreeGender,
                patientProfileFree_birthDate = patientDTO.PatientProfileFreeBirthDate,
                patientProfileFree_createdAt = DateTime.Now
            };
            _context.patient_profile_free.Add(patient);
            await _context.SaveChangesAsync();

            var result = new PatientProfileFreeReadDTO
            {
                PatientProfileFreeId = patient.patientProfileFree_id,
                PatientProfileFreeUserSerial = patient.patientProfileFree_userSerial,
                PatientProfileFreeFirstName = patient.patientProfileFree_firstName,
                PatientProfileFreeLastName = patient.patientProfileFree_lastName,
                PatientProfileFreeGender = patient.patientProfileFree_gender,
                PatientProfileFreeBirthDate = patient.patientProfileFree_birthDate,
                PatientProfileFreeCreateAt = patient.patientProfileFree_createdAt
            };

            return CreatedAtAction(nameof(GetPatientProfileFree), new {id=patient.patientProfileFree_userSerial}, result);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<PatientProfileFreeReadDTO>> CreatePatientProfileFreeAuthorized(PatientProfileFreeCreateDTO patientDTO)
        {
            // extraer la cedula del token
            var cedula = User.FindFirst("cedula")?.Value;
            if (string.IsNullOrEmpty(cedula))
                return Unauthorized("No se encontró la cédula del usuario en el token.");

            // buscar el usuario por cedula
            var user = await _context.user.FirstOrDefaultAsync(u => u.user_ide == cedula);
            if (user == null)
                return NotFound($"No se encontró el usuario con cédula {cedula}.");

            // crear el perfil con el serial automáticamente
            var patient = new PatientProfileFreeModel
            {
                patientProfileFree_userSerial = user.user_serial, // se extrae del token y la tabla de usuarios
                patientProfileFree_firstName = patientDTO.PatientProfileFreeFirstName,
                patientProfileFree_lastName = patientDTO.PatientProfileFreeLastName,
                patientProfileFree_gender = patientDTO.PatientProfileFreeGender,
                patientProfileFree_birthDate = patientDTO.PatientProfileFreeBirthDate,
                patientProfileFree_createdAt = DateTime.Now
            };

            _context.patient_profile_free.Add(patient);
            await _context.SaveChangesAsync();

            var result = new PatientProfileFreeReadDTO
            {
                PatientProfileFreeId = patient.patientProfileFree_id,
                PatientProfileFreeUserSerial = patient.patientProfileFree_userSerial,
                PatientProfileFreeFirstName = patient.patientProfileFree_firstName,
                PatientProfileFreeLastName = patient.patientProfileFree_lastName,
                PatientProfileFreeGender = patient.patientProfileFree_gender,
                PatientProfileFreeBirthDate = patient.patientProfileFree_birthDate,
                PatientProfileFreeCreateAt = patient.patientProfileFree_createdAt
            };

            return CreatedAtAction(nameof(GetPatientProfileFree), new { id = patient.patientProfileFree_userSerial }, result);
        }

        [Authorize]
        [HttpGet("my-profiles")]
        public async Task<ActionResult<IEnumerable<PatientProfileFreeReadDTO>>> GetMyPatientProfiles()
        {
            // 1. Obtener la cédula desde el token
            var cedula = User.FindFirst("cedula")?.Value;
            if (string.IsNullOrEmpty(cedula))
                return Unauthorized("No se encontró la cédula del usuario en el token.");

            // 2. Buscar el usuario
            var user = await _context.user.FirstOrDefaultAsync(u => u.user_ide == cedula);
            if (user == null)
                return NotFound($"No se encontró el usuario con cédula {cedula}.");

            // 3. Traer los perfiles asociados al user_serial
            var patientProfiles = await _context.patient_profile_free
                .Where(p => p.patientProfileFree_userSerial == user.user_serial)
                .ToListAsync();

            // 4. Mapear a DTO
            var result = patientProfiles.Select(f => new PatientProfileFreeReadDTO
            {
                PatientProfileFreeId = f.patientProfileFree_id,
                PatientProfileFreeUserSerial = f.patientProfileFree_userSerial,
                PatientProfileFreeFirstName = f.patientProfileFree_firstName,
                PatientProfileFreeLastName = f.patientProfileFree_lastName,
                PatientProfileFreeGender = f.patientProfileFree_gender,
                PatientProfileFreeBirthDate = f.patientProfileFree_birthDate,
                PatientProfileFreeCreateAt = f.patientProfileFree_createdAt
            }).ToList();

            return Ok(result);
        }





    }
}
