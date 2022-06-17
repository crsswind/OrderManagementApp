using OrderManagement.Shared.Contracts;
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

    public async Task SetStock(string productNo, int stock)
    {
        if (string.IsNullOrEmpty(productNo))
        {
            throw new Exception("Product number is not specified.");
        }

        if (stock < 0)
        {
            throw new Exception("Stock amount cannot be a negative number.");
        }

        var products = await GetTopFiveSoldProducts();
        var product = products.FirstOrDefault(p => p.ProductNo == productNo);
        if (product == null)
        {
            throw new Exception("Product not found.");
        }

        _orderApiClient.SetStock(productNo, stock);
    }

    #endregion
}