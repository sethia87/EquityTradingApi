using EquityTradingApi.AppDbContext;
using EquityTradingApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EquityTradingApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PositionsController(EquityDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Position>>> GetPositions()
    {
        return await context.Positions.ToListAsync();
    }
}
