using ApiDocBot.Data;
using ApiDocBot.DTO.PatientIndicatorEvaluationDTO;
using ApiDocBot.Models;
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

        // POST: api/patientindicatorevaluation
        [HttpPost]
        public async Task<ActionResult<PatientIndicatorEvaluationReadDTO>> CreatePatientIndicatorEvaluation(
    [FromBody] PatientIndicatorEvaluationCreateDTO dto)
        {
            var entity = new PatientIndicatorEvaluationModel
            {
                patientIndicatorEvaluation_patientProfileFreeId = dto.PatientIndicatorEvaluationPatientProfileFreeId,
                patientIndicatorEvaluation_indicatorCatalogId = dto.PatientIndicatorEvaluationIndicatorCatalogId,
                patientIndicatorEvaluation_score = dto.PatientIndicatorEvaluationScore,
                patientIndicatorEvaluation_observation = dto.PatientIndicatorEvaluationObservation ?? string.Empty,
                patientIndicatorEvaluation_date = DateTime.Now
            };

            _context.patient_indicator_evaluation.Add(entity);
            await _context.SaveChangesAsync();

            var readDto = new PatientIndicatorEvaluationReadDTO
            {
                PatientIndicatorEvaluationId = entity.patientIndicatorEvaluation_id,
                PatientIndicatorEvaluationPatientProfileFreeId = entity.patientIndicatorEvaluation_patientProfileFreeId,
                PatientIndicatorEvaluationIndicatorCatalogId = entity.patientIndicatorEvaluation_indicatorCatalogId,
                PatientIndicatorEvaluationScore = entity.patientIndicatorEvaluation_score,
                PatientIndicatorEvaluationObservation = entity.patientIndicatorEvaluation_observation,
                PatientIndicatorEvaluationDate = entity.patientIndicatorEvaluation_date
            };

            return CreatedAtAction(nameof(GetPatientIndicatorEvaluation), new { id = entity.patientIndicatorEvaluation_id }, readDto);
        }
    }
}
