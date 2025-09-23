using ApiDocBot.Data;
using ApiDocBot.DTO.DiagnosticMLMechanicalArmDTO;
using ApiDocBot.ML_MechanicalARM;
using ApiDocBot.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiDocBot.Controllers
{
    [ApiController]
    [Route("/api[controller]")]
    public class DiagnosticMLMechanicalArmController:ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly MLServiceMechanicalArm _mlServiceMechanicalArm;
        public DiagnosticMLMechanicalArmController(AppDbContext context, MLServiceMechanicalArm mlServiceMechanicalArm)
        {
            _context = context;
            _mlServiceMechanicalArm = mlServiceMechanicalArm;
        }

        //GET: api/diagnosticmlmechanicalarm
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiagnosticMLMechanicalArmReadDTO>>> GetDiagnosticMLMechanicalArm()
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

        //POST: api/predict-mechanicaarm
        //[HttpPost]
        //public async Task<ActionResult<DiagnosticMLMechanicalArmCreateDTO>> CreateDiagnosticMLMechanicalArm(DiagnosticMLMechanicalArmCreateDTO diagnosticDTO)
        //{
        //    var diagnostic = new DiagnosticMLMechanicalArmModel
        //    {
        //        diagnosticMlFree_patientProfileFreeId = diagnosticDTO.DiagnosticMLMechanicalArmPatientProfileFreeId,
        //        diagnosticMlFree_riskLevel = diagnosticDTO.DiagnosticMLMechanicalArmRiskLevel,
        //        diagnosticMlFree_recommendations = diagnosticDTO.DiagnosticMLMechanicalArmRecomendation,
        //        diagnosticMlFree_needUrgentPsychologist = diagnosticDTO.DiagnosticMLMechanicalArmNeedUrgenPsychologist,
        //        diagnosticMlFree_createdAt = DateTime.Now,
        //    };
        //    _context.diagnosticMl_mechanicalArm.Add(diagnostic);
        //    await _context.SaveChangesAsync();

        //    var result = new DiagnosticMLMechanicalArmReadDTO
        //    {
        //        DiagnosticMLMechanicalPatientProfileId = diagnostic.diagnosticMlFree_patientProfileFreeId,
        //        DiagnosticMLMechanicalRiskLevel = diagnostic.diagnosticMlFree_riskLevel,
        //        DiagnosticMLMechanicalRecomendations = diagnostic.diagnosticMlFree_recommendations,
        //        DiagnosticMLMechanicalNeedUrgentPsychologist = diagnostic.diagnosticMlFree_needUrgentPsychologist,
        //        DiagnosticMLMechanicalCreateAt = diagnostic.diagnosticMlFree_createdAt
        //    };

        //    return CreatedAtAction(nameof(GetDiagnosticMLMechanicalArm), new {id=diagnostic.diagnosticMlFree_id}, result);
        //}

        // POST: api/diagnosticmlmechanicalarm
        [HttpPost]
        public async Task<ActionResult<DiagnosticMLMechanicalArmReadDTO>> CreateDiagnosticMLMechanicalArm([FromBody] PatientDataMechanicalArm inputData)
        {
            if (inputData == null)
                return BadRequest("Los datos del paciente son requeridos.");

            // 1. Predecir riesgo con el modelo ML
            var riskLevel = _mlServiceMechanicalArm.PredictRiskMechanicalArm(inputData);

            // 2. Recomendaciones según el nivel de riesgo
            var recommendations = riskLevel switch
            {
                "Alto" =>
                    "Se detecta un alto nivel de riesgo en el desempeño de la estación Brazo Mecánico. " +
                    "Es recomendable una evaluación profesional inmediata para reforzar las habilidades cognitivas y motoras. " +
                    "Se sugiere apoyo personalizado y ejercicios guiados en el entorno escolar y familiar.",

                "Moderado" =>
                    "Se observa un nivel de riesgo moderado. Se recomienda continuar con las actividades de práctica " +
                    "en coordinación óculo-manual, planificación y tolerancia a la frustración. " +
                    "El acompañamiento cercano de docentes y familiares puede mejorar el desempeño.",

                "Bajo" =>
                    "El nivel de riesgo es bajo. El paciente muestra un desempeño adecuado en la estación. " +
                    "Se aconseja mantener la práctica constante, reforzar rutinas saludables y fomentar actividades que estimulen la perseverancia.",

                "No" =>
                    "No se detectan señales de riesgo significativas en la estación Brazo Mecánico. " +
                    "Se recomienda continuar con el mismo ritmo de actividades y monitorear periódicamente el progreso.",

                _ =>
                    "No se pudo determinar una recomendación específica debido a un resultado no reconocido. " +
                    "Por favor, intente nuevamente o consulte con un especialista."
            };

            // 3. Definir si requiere atención urgente
            var needUrgent = riskLevel == "Alto";

            // 4. Guardar en la tabla
            var diagnostic = new DiagnosticMLMechanicalArmModel
            {
                diagnosticMlFree_patientProfileFreeId = inputData.PatientAge > 0 ? 1 : 0, // ⚠️ Aquí deberías mapear el perfil real del paciente
                diagnosticMlFree_riskLevel = riskLevel,
                diagnosticMlFree_recommendations = recommendations,
                diagnosticMlFree_needUrgentPsychologist = needUrgent,
                diagnosticMlFree_createdAt = DateTime.Now
            };

            _context.diagnosticMl_mechanicalArm.Add(diagnostic);
            await _context.SaveChangesAsync();

            // 5. DTO de salida
            var result = new DiagnosticMLMechanicalArmReadDTO
            {
                DiagnosticMLMechanicalId = diagnostic.diagnosticMlFree_id,
                DiagnosticMLMechanicalPatientProfileId = diagnostic.diagnosticMlFree_patientProfileFreeId,
                DiagnosticMLMechanicalRiskLevel = diagnostic.diagnosticMlFree_riskLevel,
                DiagnosticMLMechanicalRecomendations = diagnostic.diagnosticMlFree_recommendations,
                DiagnosticMLMechanicalNeedUrgentPsychologist = diagnostic.diagnosticMlFree_needUrgentPsychologist,
                DiagnosticMLMechanicalCreateAt = diagnostic.diagnosticMlFree_createdAt
            };

            return CreatedAtAction(nameof(GetDiagnosticMLMechanicalArm), new { id = diagnostic.diagnosticMlFree_id }, result);
        }
    }
}
