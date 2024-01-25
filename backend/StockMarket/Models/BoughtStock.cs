namespace Models;

public class BoughtStock
{
    [Key]
    public int ID { get; set; }

    public required Stock Stock { get; set; }

    public double BuyingPrice { get; set; }

    public int Quantity { get; set; }

}