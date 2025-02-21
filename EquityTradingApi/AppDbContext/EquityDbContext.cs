using Microsoft.EntityFrameworkCore;

using EquityTradingApi.Model;

namespace EquityTradingApi.AppDbContext
{
    public class EquityDbContext(DbContextOptions<EquityDbContext> options) : DbContext(options)
    {
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Position> Positions { get; set; }
    }
}