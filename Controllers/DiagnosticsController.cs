using ApiDocBot.Data;
using ApiDocBot.DTO.DiagnosticMlFreeDTO;
using ApiDocBot.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace ApiDocBot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiagnosticsController : ControllerBase
    {
        private readonly ILogger<DiagnosticsController> _logger;
        private readonly MLService _mlService;
        private readonly AppDbContext _context;

        public DiagnosticsController(
            ILogger<DiagnosticsController> logger,
            MLService mlService,
            AppDbContext context)
        {
            _logger = logger;
            _mlService = mlService;
            _context = context;
        }


        //Get: api/diagnosticcontroller
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiagnosticMlFreeReadDTO>>> GetDiagnostic()
        {
            var diagnostic = await _context.diagnostic_ml_free.ToListAsync();

            var diagnosticDTO = diagnostic.Select(f => new DiagnosticMlFreeReadDTO
            {
                DiagnosticMlFreeId = f.diagnosticMlFree_id,
                DiagnosticMlFreePatientProfileFreeId = f.diagnosticMlFree_patientProfileFreeId,
                DiagnosticMlFreeRiskLevel = f.diagnosticMlFree_riskLevel,
                DiagnosticMlFreeRecommendations = f.diagnosticMlFree_recommendations,
                DiagnosticMlFreeNeedUrgentPsychologist = f.diagnosticMlFree_needUrgentPsychologist,
                DiagnosticMlFreeCreatedAt = f.diagnosticMlFree_createdAt
            });
            return Ok(diagnosticDTO);
        }

        //Post:api/diagnostic
        [HttpPost]
        public async Task<ActionResult<DiagnosticMlFreeCreateDTO>> CreateDiagnosticFree(DiagnosticMlFreeCreateDTO diagnosticDTO)
        {
            var diagnostic = new DiagnosticMlFreeModel
            {
                diagnosticMlFree_patientProfileFreeId = diagnosticDTO.DiagnosticMlFreePatientProfileFreeId,
                diagnosticMlFree_riskLevel = diagnosticDTO.DiagnosticMlFreeRiskLevel,
                diagnosticMlFree_recommendations = diagnosticDTO.DiagnosticMlFreeRecommendations,
                diagnosticMlFree_needUrgentPsychologist = diagnosticDTO.DiagnosticMlFreeNeedUrgentPsychologist,
                diagnosticMlFree_createdAt = DateTime.Now
            };

            _context.diagnostic_ml_free.Add(diagnostic);
            await _context.SaveChangesAsync();

            var result = new DiagnosticMlFreeReadDTO
            {
                DiagnosticMlFreeId = diagnostic.diagnosticMlFree_id,
                DiagnosticMlFreePatientProfileFreeId = diagnostic.diagnosticMlFree_patientProfileFreeId,
                DiagnosticMlFreeRiskLevel = diagnostic.diagnosticMlFree_riskLevel,
                DiagnosticMlFreeRecommendations = diagnostic.diagnosticMlFree_recommendations,
                DiagnosticMlFreeNeedUrgentPsychologist = diagnostic.diagnosticMlFree_needUrgentPsychologist,
                DiagnosticMlFreeCreatedAt = diagnostic.diagnosticMlFree_createdAt
            };

            return CreatedAtAction(nameof(GetDiagnostic), new {id=diagnostic.diagnosticMlFree_id}, result);

        }


        [Authorize]
        [HttpPost("predict-free")]
        public async Task<IActionResult> PredictAndCreateDiagnosticFree()
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

            // 4. Buscar las 20 respuestas que ya guardó el usuario
            var answers = await _context.answer_free
                .Where(a => a.answerFree_patientProfileFreeId == profile.patientProfileFree_id)
                .OrderBy(a => a.answerFree_questionFreeId)
                .ToListAsync();

            if (answers.Count < 20)
                return BadRequest("El paciente aún no ha respondido las 20 preguntas.");

            // 5. Crear el objeto PatientData para el modelo ML
            var patientData = new PatientData
            {
                Gender = profile.patientProfileFree_gender,
                PatientAge = DateTime.Now.Year - profile.patientProfileFree_birthDate.Year,
                Answer1 = answers[0].answerFree_answer == "Yes" ? "1" : "0",
                Answer2 = answers[1].answerFree_answer == "Yes" ? "1" : "0",
                Answer3 = answers[2].answerFree_answer == "Yes" ? "1" : "0",
                Answer4 = answers[3].answerFree_answer == "Yes" ? "1" : "0",
                Answer5 = answers[4].answerFree_answer == "Yes" ? "1" : "0",
                Answer6 = answers[5].answerFree_answer == "Yes" ? "1" : "0",
                Answer7 = answers[6].answerFree_answer == "Yes" ? "1" : "0",
                Answer8 = answers[7].answerFree_answer == "Yes" ? "1" : "0",
                Answer9 = answers[8].answerFree_answer == "Yes" ? "1" : "0",
                Answer10 = answers[9].answerFree_answer == "Yes" ? "1" : "0",
                // Las demás respuestas las dejamos igual:
                Answer11 = answers[10].answerFree_answer,
                Answer12 = answers[11].answerFree_answer,
                Answer13 = answers[12].answerFree_answer,
                Answer14 = answers[13].answerFree_answer,
                Answer15 = answers[14].answerFree_answer,
                Answer16 = answers[15].answerFree_answer,
                Answer17 = answers[16].answerFree_answer,
                Answer18 = answers[17].answerFree_answer,
                Answer19 = answers[18].answerFree_answer,
                Answer20 = answers[19].answerFree_answer
            };


            // 6. Predecir el nivel de riesgo
            var riskLevel = _mlService.PredictRisk(patientData);

            // 7. Armar recomendación según nivel
            var recommendations = riskLevel switch
            {
                "Alto" =>
                    "Se detecta un alto nivel de riesgo. Es fundamental programar una cita con un psicólogo lo antes posible. " +
                    "Se recomienda evitar el uso excesivo de pantallas, establecer rutinas saludables y reforzar el acompañamiento familiar. " +
                    "En caso de observar signos de ansiedad, aislamiento o cambios de comportamiento, acudir inmediatamente a un profesional de salud mental.",

                "Medio" =>
                    "Se ha detectado un nivel de riesgo moderado. Es recomendable realizar un seguimiento en el hogar mediante observación de rutinas, " +
                    "acompañamiento emocional y comunicación activa. Establezca límites sanos en el uso de pantallas y fomente actividades al aire libre o sociales. " +
                    "Si los síntomas persisten o aumentan, considere una evaluación psicológica preventiva.",

                "Bajo" =>
                    "El nivel de riesgo es bajo. No se detectan señales preocupantes en este momento. " +
                    "Se recomienda mantener los buenos hábitos actuales, fomentar actividades recreativas, y continuar monitoreando periódicamente el bienestar emocional. " +
                    "Es importante mantener una comunicación abierta en el entorno familiar o escolar.",

                _ =>
                    "No se pudo determinar una recomendación específica debido a un resultado no reconocido. Por favor, intente realizar nuevamente el diagnóstico o consulte con un profesional."
            };



            // 8. Saber si necesita psicólogo urgente
            var needUrgent = riskLevel == "Alto";

            // 9. Guardar en la tabla diagnostic_ml_free
            var diagnostic = new DiagnosticMlFreeModel
            {
                diagnosticMlFree_patientProfileFreeId = profile.patientProfileFree_id,
                diagnosticMlFree_riskLevel = riskLevel,
                diagnosticMlFree_recommendations = recommendations,
                diagnosticMlFree_needUrgentPsychologist = needUrgent,
                diagnosticMlFree_createdAt = DateTime.Now
            };
            _context.diagnostic_ml_free.Add(diagnostic);
            await _context.SaveChangesAsync();

            // 10. Devolver al frontend el resultado
            return Ok(new
            {
                riskLevel,
                recommendations,
                needUrgentPsychologist = needUrgent
            });
        }


        [Authorize]
        [HttpGet("diagnostics")]
        public async Task<ActionResult<IEnumerable<DiagnosticMlFreeReadDTO>>> GetMyDiagnostics()
        {
            // 1. Obtener la cédula del token
            var cedula = User.FindFirst("cedula")?.Value;
            if (string.IsNullOrEmpty(cedula))
                return Unauthorized("No se encontró la cédula en el token.");

            // 2. Buscar el usuario
            var user = await _context.user.FirstOrDefaultAsync(u => u.user_ide == cedula);
            if (user == null)
                return NotFound($"No existe un usuario con cédula {cedula}.");

            // 3. Obtener el perfil del paciente
            var profile = await _context.patient_profile_free
                .FirstOrDefaultAsync(p => p.patientProfileFree_userSerial == user.user_serial);
            if (profile == null)
                return NotFound("No existe un perfil de paciente asociado a este usuario.");

            // 4. Obtener los diagnósticos
            var diagnostics = await _context.diagnostic_ml_free
                .Where(d => d.diagnosticMlFree_patientProfileFreeId == profile.patientProfileFree_id)
                .OrderByDescending(d => d.diagnosticMlFree_createdAt)
                .ToListAsync();

            // 5. Convertir a DTO
            var diagnosticsDTO = diagnostics.Select(d => new DiagnosticMlFreeReadDTO
            {
                DiagnosticMlFreeId = d.diagnosticMlFree_id,
                DiagnosticMlFreePatientProfileFreeId = d.diagnosticMlFree_patientProfileFreeId,
                DiagnosticMlFreeRiskLevel = d.diagnosticMlFree_riskLevel,
                DiagnosticMlFreeRecommendations = d.diagnosticMlFree_recommendations,
                DiagnosticMlFreeNeedUrgentPsychologist = d.diagnosticMlFree_needUrgentPsychologist,
                DiagnosticMlFreeCreatedAt = d.diagnosticMlFree_createdAt
            });

            return Ok(diagnosticsDTO);
        }



    }

}
