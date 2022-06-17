using OrderManagement.Shared.Models;

namespace OrderManagement.Shared.Contracts;

public interface IOrderService
{
    Task<IList<ProductSale>> GetTopFiveSoldProducts();

    Task SetStock(string productNo, int stock);
}