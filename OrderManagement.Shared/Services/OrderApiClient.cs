using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OrderManagement.Shared.Models;

namespace OrderManagement.Shared.Services;

public class OrderApiClient : IOrderApiClient
{
    #region Fields

    private static readonly HttpClient Client = new();
    private readonly string _apiKey;
    private const string OrderUrl = "https://api-dev.channelengine.net/api/v2/orders?statuses=in_progress";
    private const string UpdateProductUrl = "https://api-dev.channelengine.net/api/v2/products";
    private const string ProductUrl = "https://api-dev.channelengine.net/api/v2/products?";

    #endregion

    #region Constructors

    public OrderApiClient(IConfiguration configuration)
    {
        _apiKey = configuration["ApiKey"];
    }

    #endregion

    #region IOrderApiClient Implementation

    public void SetStock(string productNo, int stock)
    {
        var uri = $"{UpdateProductUrl}/{productNo}?apikey={_apiKey}";
        var body = new
        {
            value = $"{stock}",
            path = "Stock",
            op = "replace"
        };

        var content = JsonConvert.SerializeObject(body);
        var buffer = Encoding.UTF8.GetBytes(content);
        var byteContent = new ByteArrayContent(buffer);

        Client.PatchAsync(uri, byteContent);
    }

    public async Task<IList<Order>> GetAllInProgressOrders()
    {
        var uri = $"{OrderUrl}&apikey={_apiKey}";
        var response = await Client.GetAsync(uri);
        response.EnsureSuccessStatusCode();
        var responseBody = response.Content.ReadAsStringAsync().Result;
        var orders = JsonConvert.DeserializeObject<OrderCollection>(responseBody);

        return orders.Content;
    }

    public async Task<IDictionary<string, Product>> GetAllProducts(IEnumerable<string> productNoList)
    {
        var productNoListString = string.Join('&', productNoList.Select(x => $"merchantProductNoList={x}"));
        var url = $"{ProductUrl}&apikey={_apiKey}&{productNoListString}";
        var responseBody = await Client.GetAsync(url).Result.Content.ReadAsStringAsync();
        var products = JsonConvert.DeserializeObject<ProductCollection>(responseBody).Content.ToDictionary(x => x.MerchantProductNo);

        return products;
    } 
    
    #endregion
}