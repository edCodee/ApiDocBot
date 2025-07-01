using ApiDocBot.Data;
using ApiDocBot.DTO.QuestionFreeDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiDocBot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionFreeController:ControllerBase
    {
        private readonly AppDbContext _context;
        public QuestionFreeController(AppDbContext context)
        {
            _context = context;
        }

        //GET: api/questionfree
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionFreeReadDTO>>> GetQuestionFree()
        {
            var question = await _context.question_free.ToListAsync();

            var questionDTO = question.Select(f => new QuestionFreeReadDTO
            {
                QuestionFreeId = f.questionFree_id,
                QuestionFreeText = f.questionFree_text,
                QuestionFreeType = f.questionFree_type
            });

            return Ok(questionDTO);
        }

        
    }
}
