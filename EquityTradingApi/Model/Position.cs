using System.ComponentModel.DataAnnotations;

namespace EquityTradingApi.Model;

public class Position
{
    [Key]
    public string SecurityCode { get; set; }
    public int Quantity { get; set; }
}
