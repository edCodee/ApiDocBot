using ApiDocBot.Data;
using ApiDocBot.DTO.DiagnosticMLMechanicalArmDTO;
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
        public DiagnosticMLMechanicalArmController(AppDbContext context)
        {
            _context = context;
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
        [HttpPost]
        public async Task<ActionResult<DiagnosticMLMechanicalArmCreateDTO>> CreateDiagnosticMLMechanicalArm(DiagnosticMLMechanicalArmCreateDTO diagnosticDTO)
        {
            var diagnostic = new DiagnosticMLMechanicalArmModel
            {
                diagnosticMlFree_patientProfileFreeId = diagnosticDTO.DiagnosticMLMechanicalArmPatientProfileFreeId,
                diagnosticMlFree_riskLevel = diagnosticDTO.DiagnosticMLMechanicalArmRiskLevel,
                diagnosticMlFree_recommendations = diagnosticDTO.DiagnosticMLMechanicalArmRecomendation,
                diagnosticMlFree_needUrgentPsychologist = diagnosticDTO.DiagnosticMLMechanicalArmNeedUrgenPsychologist,
                diagnosticMlFree_createdAt = DateTime.Now,
            };
            _context.diagnosticMl_mechanicalArm.Add(diagnostic);
            await _context.SaveChangesAsync();

            var result = new DiagnosticMLMechanicalArmReadDTO
            {
                DiagnosticMLMechanicalPatientProfileId = diagnostic.diagnosticMlFree_patientProfileFreeId,
                DiagnosticMLMechanicalRiskLevel = diagnostic.diagnosticMlFree_riskLevel,
                DiagnosticMLMechanicalRecomendations = diagnostic.diagnosticMlFree_recommendations,
                DiagnosticMLMechanicalNeedUrgentPsychologist = diagnostic.diagnosticMlFree_needUrgentPsychologist,
                DiagnosticMLMechanicalCreateAt = diagnostic.diagnosticMlFree_createdAt
            };

            return CreatedAtAction(nameof(GetDiagnosticMLMechanicalArm), new {id=diagnostic.diagnosticMlFree_id}, result);
        }
    }
}
