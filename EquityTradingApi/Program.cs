using Microsoft.EntityFrameworkCore;
using EquityTradingApi.AppDbContext;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<EquityDbContext>(options =>
                              options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
