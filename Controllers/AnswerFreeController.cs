using ApiDocBot.Data;
using ApiDocBot.DTO.AnswerFreeDTO;
using ApiDocBot.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace ApiDocBot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnswerFreeController:ControllerBase
    {
        private readonly AppDbContext _context;
        public AnswerFreeController(AppDbContext context)
        {
            _context = context;
        }

        //GET: api/answerfree
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnswerFreeReadDTO>>> GetAnswerFree()
        {
            var answer = await _context.answer_free.ToListAsync();

            var answerDTO = answer.Select(f => new AnswerFreeReadDTO
            {
                AnswerFreeId = f.answerFree_id,
                AnswerFreePatientProfileFreeId = f.answerFree_patientProfileFreeId,
                AnswerFreeQuestionFreeId = f.answerFree_questionFreeId,
                AnswerFreeAnswer = f.answerFree_answer,
                AnswerFreeDate = f.answerFree_date
            });
            return Ok(answerDTO);
        }

        //POST: api/answerfree
        [HttpPost]
        public async Task<ActionResult<AnswerFreeCreateDTO>> CreateAnswerFree(AnswerFreeCreateDTO answerDTO)
        {
            var answer = new AnswerFreeModel
            {
                //answerFree_patientProfileFreeId = answerDTO.AnswerFreePatientProfileFreeId,
                answerFree_questionFreeId = answerDTO.AnswerFreeQuestionFreeId,
                answerFree_answer = answerDTO.AnswerFreeAnswer,
                answerFree_date = DateTime.Now
            };
            _context.answer_free.Add(answer);
            await _context.SaveChangesAsync();

            var result = new AnswerFreeReadDTO
            {
                AnswerFreeId = answer.answerFree_id,
                AnswerFreePatientProfileFreeId = answer.answerFree_patientProfileFreeId,
                AnswerFreeQuestionFreeId = answer.answerFree_questionFreeId,
                AnswerFreeAnswer = answer.answerFree_answer,
                AnswerFreeDate = answer.answerFree_date
            };
            return CreatedAtAction(nameof(GetAnswerFree), new { id=answer.answerFree_id }, result);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<AnswerFreeReadDTO>> CreateAnswerFreeAuthorized(AnswerFreeCreateDTO answerDTO)
        {
            // obtener la cedula desde el token
            var cedula = User.FindFirst("cedula")?.Value;
            if (string.IsNullOrEmpty(cedula))
                return Unauthorized("No se encontró la cédula en el token.");

            // buscar el user
            var user = await _context.user.FirstOrDefaultAsync(u => u.user_ide == cedula);
            if (user == null)
                return NotFound($"No existe un usuario con cédula {cedula}");

            // obtener el patientProfileFree_id para ese user
            var patientProfile = await _context.patient_profile_free
                .FirstOrDefaultAsync(p => p.patientProfileFree_userSerial == user.user_serial);

            if (patientProfile == null)
                return NotFound($"No existe un perfil de paciente asociado al usuario con cédula {cedula}");

            // crear la respuesta
            var answer = new AnswerFreeModel
            {
                answerFree_patientProfileFreeId = patientProfile.patientProfileFree_id, // automático
                answerFree_questionFreeId = answerDTO.AnswerFreeQuestionFreeId, // desde el cliente
                answerFree_answer = answerDTO.AnswerFreeAnswer,
                answerFree_date = DateTime.Now
            };

            _context.answer_free.Add(answer);
            await _context.SaveChangesAsync();

            var result = new AnswerFreeReadDTO
            {
                AnswerFreeId = answer.answerFree_id,
                AnswerFreePatientProfileFreeId = answer.answerFree_patientProfileFreeId,
                AnswerFreeQuestionFreeId = answer.answerFree_questionFreeId,
                AnswerFreeAnswer = answer.answerFree_answer,
                AnswerFreeDate = answer.answerFree_date
            };

            return CreatedAtAction(nameof(GetAnswerFree), new { id = answer.answerFree_id }, result);
        }

        [Authorize]
        [HttpGet("my-answers")]
        public async Task<ActionResult<IEnumerable<AnswerFreeReadDTO>>> GetMyAnswers()
        {
            // sacar la cedula del token
            var cedula = User.FindFirst("cedula")?.Value;
            if (string.IsNullOrEmpty(cedula))
                return Unauthorized("No se encontró la cédula en el token.");

            // buscar el usuario
            var user = await _context.user.FirstOrDefaultAsync(u => u.user_ide == cedula);
            if (user == null)
                return NotFound($"No existe un usuario con cédula {cedula}");

            // buscar el patientProfileFree
            var patientProfile = await _context.patient_profile_free
                .FirstOrDefaultAsync(p => p.patientProfileFree_userSerial == user.user_serial);

            if (patientProfile == null)
                return NotFound($"No existe un perfil de paciente asociado al usuario con cédula {cedula}");

            // buscar respuestas
            var answers = await _context.answer_free
                .Where(a => a.answerFree_patientProfileFreeId == patientProfile.patientProfileFree_id)
                .OrderBy(a => a.answerFree_questionFreeId)
                .ToListAsync();

            // si tiene 20 respuestas significa encuesta completa
            if (answers.Count >= 20)
            {
                var result = answers.Select(a => new AnswerFreeReadDTO
                {
                    AnswerFreeId = a.answerFree_id,
                    AnswerFreePatientProfileFreeId = a.answerFree_patientProfileFreeId,
                    AnswerFreeQuestionFreeId = a.answerFree_questionFreeId,
                    AnswerFreeAnswer = a.answerFree_answer,
                    AnswerFreeDate = a.answerFree_date
                });

                return Ok(result);
            }

            // si no tiene 20 respuestas, devolver vacío
            return Ok(new List<AnswerFreeReadDTO>());
        }




    }
}
