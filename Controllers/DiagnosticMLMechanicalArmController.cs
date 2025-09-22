using ApiDocBot.Data;
using ApiDocBot.DTO.DiagnosticMLMechanicalArmDTO;
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
    }
}
