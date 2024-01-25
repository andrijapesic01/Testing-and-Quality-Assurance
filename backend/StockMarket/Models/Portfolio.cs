namespace Models;

public class Portfolio
{
    [Key]
    public int ID { get; set; }

    public required string OwnerName { get; set; }

    public required string BankName { get; set; }

    public double BankBalance { get; set; }

    public int RiskTolerance { get; set; }
    
    public required string InvestmentStrategy { get; set; }
    
    [JsonIgnore]
    public List<BoughtStock> BoughtStocks { get; set; } = new List<BoughtStock>();
}
