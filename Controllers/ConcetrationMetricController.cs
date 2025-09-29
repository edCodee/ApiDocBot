using ApiDocBot.Data;
using ApiDocBot.DTO.ConcetrationMetricModelDTO;
using ApiDocBot.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiDocBot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConcetrationMetricController:ControllerBase
    {
        private readonly AppDbContext _context;

        public ConcetrationMetricController(AppDbContext context)
        {
            _context = context;
        }

        //Get: api/concetrationmetric
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConcetrationMetricReadDTO>>> GetConcentrarionMetrics()
        {
            var concetration = await _context.concentration_metrics.ToListAsync();

            var concetrationDTO = concetration.Select(f => new ConcetrationMetricReadDTO
            {
                ConcetrationMetricId = f.concentrationMetrics_id,
                ConcetrationMetricPatientProfileFreeId = f.concentrationMetrics_patientProfileFreeId,
                ConcetrationMetricDurationMs = f.concentrationMetrics_durationMs,
                ConcetrationMetricPercentMoving = f.concentrationMetrics_percentMoving,
                ConcetrationMetricAvgMovement = f.concentrationMetrics_avgMovement,
                ConcetrationMetricCreateAt = f.concentrationMetrics_createdAt
            });
            return Ok(concetrationDTO);
        }

        //Post: api/concetrationmetric
        [HttpPost]
        public async Task<ActionResult<ConcetrationMetricCreateDTO>> CreateConcetrationMetric(ConcetrationMetricCreateDTO concetrationDTO)
        {
            var concetration = new ConcetrationMetricModel
            {
                concentrationMetrics_patientProfileFreeId = concetrationDTO.ConcetrationMetricPatientProfileFreeId,
                concentrationMetrics_durationMs = concetrationDTO.ConcetrationMetricDurationMs,
                concentrationMetrics_percentMoving = concetrationDTO.ConcetrationMetricPercentMoving,
                concentrationMetrics_avgMovement = concetrationDTO.ConcetrationMetricAvgMovement,
                concentrationMetrics_createdAt = DateTime.Now
            };

            _context.concentration_metrics.Add(concetration);
            await _context.SaveChangesAsync();

            var result = new ConcetrationMetricReadDTO
            {
                ConcetrationMetricId = concetration.concentrationMetrics_id,
                ConcetrationMetricPatientProfileFreeId = concetration.concentrationMetrics_patientProfileFreeId,
                ConcetrationMetricDurationMs = concetration.concentrationMetrics_durationMs,
                ConcetrationMetricPercentMoving = concetration.concentrationMetrics_percentMoving,
                ConcetrationMetricAvgMovement = concetration.concentrationMetrics_avgMovement,
                ConcetrationMetricCreateAt = concetration.concentrationMetrics_createdAt
            };

            return CreatedAtAction(nameof(GetConcentrarionMetrics), new {id=concetration.concentrationMetrics_id}, result);
        }

        [Authorize]
        [HttpPost("create-free")]
        public async Task<IActionResult> CreateConcentrationMetricFree(ConcetrationMetricCreateDTO concetrationDTO)
        {
            // 1. Sacar la cédula del token
            var cedula = User.FindFirst("cedula")?.Value;
            if (string.IsNullOrEmpty(cedula))
                return Unauthorized("No se encontró la cédula del usuario en el token.");

            // 2. Buscar el usuario
            var user = await _context.user.FirstOrDefaultAsync(u => u.user_ide == cedula);
            if (user == null)
                return NotFound($"No existe un usuario con cédula {cedula}.");

            // 3. Buscar el perfil del paciente
            var profile = await _context.patient_profile_free
                .FirstOrDefaultAsync(p => p.patientProfileFree_userSerial == user.user_serial);
            if (profile == null)
                return NotFound("Perfil de paciente no encontrado.");

            // 4. Crear el objeto de concentración
            var concetration = new ConcetrationMetricModel
            {
                concentrationMetrics_patientProfileFreeId = profile.patientProfileFree_id,
                concentrationMetrics_durationMs = concetrationDTO.ConcetrationMetricDurationMs,
                concentrationMetrics_percentMoving = concetrationDTO.ConcetrationMetricPercentMoving,
                concentrationMetrics_avgMovement = concetrationDTO.ConcetrationMetricAvgMovement,
                concentrationMetrics_createdAt = DateTime.Now
            };

            _context.concentration_metrics.Add(concetration);
            await _context.SaveChangesAsync();

            // 5. Preparar DTO de respuesta
            var result = new ConcetrationMetricReadDTO
            {
                ConcetrationMetricId = concetration.concentrationMetrics_id,
                ConcetrationMetricPatientProfileFreeId = concetration.concentrationMetrics_patientProfileFreeId,
                ConcetrationMetricDurationMs = concetration.concentrationMetrics_durationMs,
                ConcetrationMetricPercentMoving = concetration.concentrationMetrics_percentMoving,
                ConcetrationMetricAvgMovement = concetration.concentrationMetrics_avgMovement,
                ConcetrationMetricCreateAt = concetration.concentrationMetrics_createdAt
            };

            return CreatedAtAction(nameof(GetConcentrarionMetrics),
                new { id = concetration.concentrationMetrics_id }, result);
        }

        // GET: api/concetrationmetric/by-profile/{profileId}
        [HttpGet("by-profile/{profileId}")]
        public async Task<ActionResult<IEnumerable<ConcetrationMetricReadDTO>>> GetConcentrationMetricsByProfile(int profileId)
        {
            // 1. Filtrar por el patientProfileFreeId
            var concetration = await _context.concentration_metrics
                .Where(f => f.concentrationMetrics_patientProfileFreeId == profileId)
                .ToListAsync();

            // 2. Si no hay registros, devolver 404
            if (concetration == null || concetration.Count == 0)
                return NotFound($"No existen métricas de concentración para el perfil {profileId}.");

            // 3. Mapear a DTO
            var concetrationDTO = concetration.Select(f => new ConcetrationMetricReadDTO
            {
                ConcetrationMetricId = f.concentrationMetrics_id,
                ConcetrationMetricPatientProfileFreeId = f.concentrationMetrics_patientProfileFreeId,
                ConcetrationMetricDurationMs = f.concentrationMetrics_durationMs,
                ConcetrationMetricPercentMoving = f.concentrationMetrics_percentMoving,
                ConcetrationMetricAvgMovement = f.concentrationMetrics_avgMovement,
                ConcetrationMetricCreateAt = f.concentrationMetrics_createdAt
            });

            return Ok(concetrationDTO);
        }
    }
}
