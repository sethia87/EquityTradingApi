using EquityTradingApi.AppDbContext;
using EquityTradingApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EquityTradingApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionsController(EquityDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetPositions()
    {
        return await context.Transactions.ToListAsync();
    }

    [HttpPost]
    public async Task<IActionResult> AddTransaction([FromBody] Transaction transaction)
    {
        if (transaction == null)
        {
            return BadRequest();
        }

        var existingTransaction = context.Transactions
            .Where(t => t.TradeID == transaction.TradeID && t.Version == transaction.Version)
            .FirstOrDefault();

        if (existingTransaction == null)
        {
            context.Transactions.Add(transaction);
            await context.SaveChangesAsync();

        }
        UpdatePositions(transaction);
        return Ok();

    }

    private void UpdatePositions(Transaction transaction)
    {
        var position = context.Positions.FirstOrDefault(p => p.SecurityCode == transaction.SecurityCode);
        if (transaction.Action == "INSERT")
        {
            if (position == null)
            {
                position = new Position { SecurityCode = transaction.SecurityCode, Quantity = 0 };
                context.Positions.Add(position);
                context.SaveChanges();
            }

            int quantityChange = transaction.BuySell == "Buy" ? transaction.Quantity : -transaction.Quantity;
            position.Quantity += quantityChange;
            context.Positions.Update(position);
        }
        else if (transaction.Action == "UPDATE")
        {
            if (position == null)
            {
                position = new Position { SecurityCode = transaction.SecurityCode, Quantity = 0 };
                context.Positions.Add(position);
                context.SaveChanges();
            }

            int quantityChange = transaction.BuySell == "Buy" ? transaction.Quantity : -transaction.Quantity;
            position.Quantity = quantityChange;
            context.Positions.Update(position);
        }
        else if (transaction.Action == "CANCEL")
        {
            if (position != null)
            {
                if (transaction.Version != 1)
                    position.Quantity = 0;
                else
                {
                    int quantityChange = transaction.BuySell == "Buy" ? -transaction.Quantity : transaction.Quantity;
                    position.Quantity += quantityChange;
                }
                context.Positions.Update(position);
            }
        }

        context.SaveChanges();
    }
}
