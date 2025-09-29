using ApiDocBot.Data;
using ApiDocBot.DTO.DiagnosticMlFreeDTO;
using ApiDocBot.DTO.DiagnosticMLMechanicalArmDTO;
using ApiDocBot.ML_MechanicalARM;
using ApiDocBot.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiDocBot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiagnosticMLMechanicalArmController:ControllerBase
    {
        private readonly ILogger<DiagnosticMLMechanicalArmController> _logger;
        private readonly MLServiceMechanicalArm _mlService;
        private readonly AppDbContext _context;

        public DiagnosticMLMechanicalArmController(ILogger<DiagnosticMLMechanicalArmController> logger, MLServiceMechanicalArm mlService, AppDbContext context)
        {
            _logger = logger;
            _mlService = mlService;
            _context = context;
        }

        //Get: api/diagnosticcontroller
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiagnosticMLMechanicalArmReadDTO>>> GetDiagnosticMechanicalArm()
        {
            var diagnostic = await _context.diagnosticMl_mechanicalArm.ToListAsync();

            var diagnosticDTO = diagnostic.Select(f => new DiagnosticMLMechanicalArmReadDTO
            {
                DiagnosticMLMechanicalId = f.diagnosticMlFree_id,
                DiagnosticMLMechanicalPatientProfileId = f.diagnosticMlFree_patientProfileFreeId,
                DiagnosticMLMechanicalRiskLevel = f.diagnosticMlFree_riskLevel,
                DiagnosticMLMechanicalRecomendations = f.diagnosticMlFree_recommendations,
                DiagnosticMLMechanicalNeedUrgentPsychologist = f.diagnosticMlFree_needUrgentPsychologist,
                DiagnosticMLMechanicalCreateAt = f.diagnosticMlFree_createdAt
            });
            return Ok(diagnosticDTO);
        }

        //Post: api/Diagnosticmechanicalarm
        //[HttpPost]
        //public async Task<ActionResult<DiagnosticMLMechanicalArmCreateDTO>> CreateDiagnosticMechanicalArm(DiagnosticMLMechanicalArmCreateDTO diagnosticDTO)
        //{
        //    var diagnostic = new DiagnosticMLMechanicalArmModel
        //    {
        //        diagnosticMlFree_patientProfileFreeId = diagnosticDTO.DiagnosticMLMechanicalArmPatientProfileFreeId,
        //        diagnosticMlFree_riskLevel = diagnosticDTO.DiagnosticMLMechanicalArmRiskLevel,
        //        diagnosticMlFree_recommendations = diagnosticDTO.DiagnosticMLMechanicalArmRecomendation,
        //        diagnosticMlFree_needUrgentPsychologist = diagnosticDTO.DiagnosticMLMechanicalArmNeedUrgenPsychologist,
        //        diagnosticMlFree_createdAt = diagnosticDTO.DiagnosticMLMechanicalArmCreateAt
        //    };
        //    _context.diagnosticMl_mechanicalArm.Add(diagnostic);
        //    await _context.SaveChangesAsync();

        //    var result = new DiagnosticMLMechanicalArmReadDTO
        //    {
        //        DiagnosticMLMechanicalId = diagnostic.diagnosticMlFree_id,
        //        DiagnosticMLMechanicalPatientProfileId = diagnostic.diagnosticMlFree_patientProfileFreeId,
        //        DiagnosticMLMechanicalRiskLevel = diagnostic.diagnosticMlFree_riskLevel,
        //        DiagnosticMLMechanicalRecomendations = diagnostic.diagnosticMlFree_recommendations,
        //        DiagnosticMLMechanicalNeedUrgentPsychologist = diagnostic.diagnosticMlFree_needUrgentPsychologist,
        //        DiagnosticMLMechanicalCreateAt = diagnostic.diagnosticMlFree_createdAt
        //    };

        //    return CreatedAtAction(nameof(GetDiagnosticMechanicalArm), new { id = diagnostic.diagnosticMlFree_id }, result);
        //}

        [HttpPost]
        public async Task<ActionResult<DiagnosticMLMechanicalArmCreateDTO>> CreateDiagnosticMechanicalArm(DiagnosticMLMechanicalArmCreateDTO diagnosticDTO)
        {
            // 1. Validar que exista el perfil del paciente
            var profileExists = await _context.patient_profile_free
                .AnyAsync(p => p.patientProfileFree_id == diagnosticDTO.DiagnosticMLMechanicalArmPatientProfileFreeId);

            if (!profileExists)
                return BadRequest($"El PatientProfileId {diagnosticDTO.DiagnosticMLMechanicalArmPatientProfileFreeId} no existe en la base de datos.");

            // 2. Crear el diagnóstico
            var diagnostic = new DiagnosticMLMechanicalArmModel
            {
                diagnosticMlFree_patientProfileFreeId = diagnosticDTO.DiagnosticMLMechanicalArmPatientProfileFreeId,
                diagnosticMlFree_riskLevel = diagnosticDTO.DiagnosticMLMechanicalArmRiskLevel,
                diagnosticMlFree_recommendations = diagnosticDTO.DiagnosticMLMechanicalArmRecomendation,
                diagnosticMlFree_needUrgentPsychologist = diagnosticDTO.DiagnosticMLMechanicalArmNeedUrgenPsychologist,
                diagnosticMlFree_createdAt = diagnosticDTO.DiagnosticMLMechanicalArmCreateAt
            };

            _context.diagnosticMl_mechanicalArm.Add(diagnostic);
            await _context.SaveChangesAsync();

            // 3. Mapear a DTO de respuesta
            var result = new DiagnosticMLMechanicalArmReadDTO
            {
                DiagnosticMLMechanicalId = diagnostic.diagnosticMlFree_id,
                DiagnosticMLMechanicalPatientProfileId = diagnostic.diagnosticMlFree_patientProfileFreeId,
                DiagnosticMLMechanicalRiskLevel = diagnostic.diagnosticMlFree_riskLevel,
                DiagnosticMLMechanicalRecomendations = diagnostic.diagnosticMlFree_recommendations,
                DiagnosticMLMechanicalNeedUrgentPsychologist = diagnostic.diagnosticMlFree_needUrgentPsychologist,
                DiagnosticMLMechanicalCreateAt = diagnostic.diagnosticMlFree_createdAt
            };

            return CreatedAtAction(nameof(GetDiagnosticMechanicalArm), new { id = diagnostic.diagnosticMlFree_id }, result);
        }


        // POST: api/DiagnosticMLMechanicalArm/predict
        [HttpPost("predict")]
        public async Task<ActionResult<DiagnosticMLMechanicalArmReadDTO>> PredictAndCreateDiagnosticMechanicalArm([FromBody] PatientDataMechanicalArm patientData, [FromQuery] int patientProfileId)
        {
            if (patientData == null)
                return BadRequest("Debe enviar los datos del paciente.");

            // 1. Ejecutar predicción usando ML
            var riskLevel = _mlService.PredictRisk(patientData);

            // 2. Generar recomendaciones según nivel de riesgo
            var recommendations = riskLevel switch
            {
                "Alto" =>
                    "Se detecta un alto nivel de riesgo. Es fundamental programar una cita con un psicólogo lo antes posible. " +
                    "Se recomienda reforzar la tolerancia a la frustración y mantener un acompañamiento cercano durante las actividades.",

                "Moderado" =>
                    "Se detecta un nivel moderado de riesgo. Se recomienda trabajar en la planificación y perseverancia con actividades guiadas. " +
                    "El seguimiento constante permitirá prevenir mayores dificultades.",

                "Bajo" =>
                    "El nivel de riesgo es bajo. Se sugiere continuar con las prácticas actuales, fomentando la precisión motriz fina " +
                    "y la coordinación óculo-manual a través de ejercicios recreativos.",

                _ =>
                    "No se pudo determinar una recomendación específica. Revise los datos ingresados e intente nuevamente."
            };

            // 3. Determinar si necesita psicólogo urgente
            var needUrgent = riskLevel == "Alto";

            // 4. Crear entidad para guardar en BD
            var diagnostic = new DiagnosticMLMechanicalArmModel
            {
                diagnosticMlFree_patientProfileFreeId = patientProfileId,
                diagnosticMlFree_riskLevel = riskLevel,
                diagnosticMlFree_recommendations = recommendations,
                diagnosticMlFree_needUrgentPsychologist = needUrgent,
                diagnosticMlFree_createdAt = DateTime.Now
            };

            _context.diagnosticMl_mechanicalArm.Add(diagnostic);
            await _context.SaveChangesAsync();

            // 5. Devolver resultado al frontend
            var result = new DiagnosticMLMechanicalArmReadDTO
            {
                DiagnosticMLMechanicalId = diagnostic.diagnosticMlFree_id,
                DiagnosticMLMechanicalPatientProfileId = diagnostic.diagnosticMlFree_patientProfileFreeId,
                DiagnosticMLMechanicalRiskLevel = diagnostic.diagnosticMlFree_riskLevel,
                DiagnosticMLMechanicalRecomendations = diagnostic.diagnosticMlFree_recommendations,
                DiagnosticMLMechanicalNeedUrgentPsychologist = diagnostic.diagnosticMlFree_needUrgentPsychologist,
                DiagnosticMLMechanicalCreateAt = diagnostic.diagnosticMlFree_createdAt
            };

            return CreatedAtAction(nameof(GetDiagnosticMechanicalArm), new { id = diagnostic.diagnosticMlFree_id }, result);
        }


        // GET: api/diagnosticcontroller/by-profile/{profileId}
        [HttpGet("by-profile/{profileId}")]
        public async Task<ActionResult<IEnumerable<DiagnosticMLMechanicalArmReadDTO>>> GetDiagnosticMechanicalArmByProfile(int profileId)
        {
            // 1. Filtrar por el PatientProfileFreeId
            var diagnostics = await _context.diagnosticMl_mechanicalArm
                .Where(d => d.diagnosticMlFree_patientProfileFreeId == profileId)
                .ToListAsync();

            // 2. Si no hay registros, devolver 404
            if (diagnostics == null || diagnostics.Count == 0)
                return NotFound($"No existen diagnósticos para el perfil {profileId}.");

            // 3. Mapear a DTO
            var diagnosticDTOs = diagnostics.Select(f => new DiagnosticMLMechanicalArmReadDTO
            {
                DiagnosticMLMechanicalId = f.diagnosticMlFree_id,
                DiagnosticMLMechanicalPatientProfileId = f.diagnosticMlFree_patientProfileFreeId,
                DiagnosticMLMechanicalRiskLevel = f.diagnosticMlFree_riskLevel,
                DiagnosticMLMechanicalRecomendations = f.diagnosticMlFree_recommendations,
                DiagnosticMLMechanicalNeedUrgentPsychologist = f.diagnosticMlFree_needUrgentPsychologist,
                DiagnosticMLMechanicalCreateAt = f.diagnosticMlFree_createdAt
            });

            return Ok(diagnosticDTOs);
        }



    }
}
