using ApiDocBot.Data;
using ApiDocBot.DTO.PatientIndicatorEvaluationDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace ApiDocBot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientIndicatorEvaluationController:ControllerBase
    {
        private readonly AppDbContext _context;
        public PatientIndicatorEvaluationController(AppDbContext context)
        {
            _context = context;
        }

        //Get: api/patientindicatorevaluation   
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientIndicatorEvaluationReadDTO>>> GetPatientIndicatorEvaluation()
        {
            var indicatorEvaluation = await _context.patient_indicator_evaluation.ToListAsync();

            var indicatorEvaluationDTO = indicatorEvaluation.Select(f => new PatientIndicatorEvaluationReadDTO
            {
                PatientIndicatorEvaluationId = f.patientIndicatorEvaluation_id,
                PatientIndicatorEvaluationPatientProfileFreeId = f.patientIndicatorEvaluation_patientProfileFreeId,
                PatientIndicatorEvaluationIndicatorCatalogId = f.patientIndicatorEvaluation_indicatorCatalogId,
                PatientIndicatorEvaluationScore = f.patientIndicatorEvaluation_score,
                PatientIndicatorEvaluationObservation = f.patientIndicatorEvaluation_observation,
                PatientIndicatorEvaluationDate = f.patientIndicatorEvaluation_date
            });
            return Ok(indicatorEvaluationDTO);
        }
    }
}
