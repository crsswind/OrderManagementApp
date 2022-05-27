using OrderManagement.Shared.Models;

namespace OrderManagement.Shared.Services;

public class OrderService : IOrderService
{
    #region Fields

    private readonly IOrderApiClient _orderApiClient;

    #endregion

    #region Constructors

    public OrderService(IOrderApiClient orderApiClient)
    {
        _orderApiClient = orderApiClient;
    }

    #endregion

    #region IOrderService Implementation

    public async Task<IList<ProductSale>> GetTopFiveSoldProducts()
    {
        var topFiveList = (await _orderApiClient.GetAllInProgressOrders())
            .SelectMany(x => x.Lines)
            .GroupBy(l => l.MerchantProductNo, (key, g) => new ProductSale { ProductNo = key, Quantity = g.Sum(x => x.Quantity) })
            .OrderByDescending(x => x.Quantity)
            .Take(5)
            .ToList();

        var productNoList = topFiveList.Select(x => x.ProductNo);
        var products = await _orderApiClient.GetAllProducts(productNoList);

        foreach (var productSale in topFiveList)
        {
            productSale.Name = products[productSale.ProductNo].Name;
            productSale.Gtin = products[productSale.ProductNo].Ean;
        }

        return topFiveList;
    }

    public void SetStock(string productNo, int stock)
    {
        _orderApiClient.SetStock(productNo, stock);
    }

    #endregion
}