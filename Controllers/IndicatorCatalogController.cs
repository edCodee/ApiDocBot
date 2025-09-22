using ApiDocBot.Data;
using ApiDocBot.DTO.IndicatorCatalogDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiDocBot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IndicatorCatalogController:ControllerBase
    {
        private readonly AppDbContext _context;
        public IndicatorCatalogController(AppDbContext context)
        {
            _context = context;
        }

        //GET:api/indicatorController
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IndicatorCatalogRead>>> GetIndicatorCatalog()
        {
            var indicator = await _context.indicator_catalog.ToListAsync();

            var indicatorDTO = indicator.Select(f => new IndicatorCatalogRead
            {
                IndicatorCatalogId = f.indicatorcatalog_id,
                IndicatorCatalogName = f.indicatorcatalog_name,
                IndicatorCatalogDescription = f.indicatorcatalog_description
            });
            return Ok(indicatorDTO);
        }

    }
}
