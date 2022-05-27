namespace OrderManagement.Shared.Models;

public class OrderItem
{
    public string MerchantProductNo { get; set; }
    public string Gtin { get; set; }
    public int Quantity { get; set; }
}