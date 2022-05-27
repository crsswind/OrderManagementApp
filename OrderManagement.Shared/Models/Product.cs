namespace OrderManagement.Shared.Models;
public class Product
{
    public string Name { get; set; }
    public string Ean { get; set; }
    public string MerchantProductNo { get; set; }
}

public class ProductCollection
{
    public IList<Product> Content { get; set; }
}