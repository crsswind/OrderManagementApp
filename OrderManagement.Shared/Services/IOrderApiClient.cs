using OrderManagement.Shared.Models;

namespace OrderManagement.Shared.Services;

public interface IOrderApiClient
{
    void SetStock(string productNo, int stock);
    Task<IList<Order>> GetAllInProgressOrders();
    Task<IDictionary<string, Product>> GetAllProducts(IEnumerable<string> productNoList);
}