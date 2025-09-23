using ApiDocBot.Data;
using ApiDocBot.DTO.DiagnosticMlFreeDTO;
using ApiDocBot.DTO.DiagnosticMLMechanicalArmDTO;
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
    }
}
