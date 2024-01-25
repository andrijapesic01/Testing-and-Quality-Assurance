namespace Models;
public class Stock
{
    [Key]
    public int ID { get; set; }

    [MaxLength(8)]
    public required string Symbol { get; set; }

    [MaxLength(100)]
    public required string Company { get; set; }

    public required string LogoURL { get; set; }

    public double CurrentPrice { get; set; }

    public double PriceChange { get; set; }

    public double PercentChange { get; set; }
}