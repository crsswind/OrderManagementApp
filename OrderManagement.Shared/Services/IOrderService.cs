using OrderManagement.Shared.Models;

namespace OrderManagement.Shared.Services;

public interface IOrderService
{
    Task<IList<ProductSale>> GetTopFiveSoldProducts();

    void SetStock(string productNo, int stock);
}