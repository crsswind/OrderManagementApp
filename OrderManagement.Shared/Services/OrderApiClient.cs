using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OrderManagement.Shared.Contracts;
using OrderManagement.Shared.Models;

namespace OrderManagement.Shared.Services;

public class OrderApiClient : IOrderApiClient
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    #region Fields

    private readonly string _apiKey;

    #endregion

    #region Constructors

    public OrderApiClient(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
        _apiKey = configuration["ApiKey"];
    }

    #endregion

    #region IOrderApiClient Implementation

    public void SetStock(string productNo, int stock)
    {
        var client = _httpClientFactory.CreateClient("ChannelEngine");
        var uri = new Uri($"{_configuration["UpdateProductUrl"]}/{productNo}?apikey={_apiKey}");
        var body = new
        {
            value = $"{stock}",
            path = "Stock",
            op = "replace"
        };

        var content = JsonConvert.SerializeObject(body);
        var buffer = Encoding.UTF8.GetBytes(content);
        var byteContent = new ByteArrayContent(buffer);

        client.PatchAsync(uri, byteContent);
    }

    public async Task<IList<Order>> GetAllInProgressOrders()
    {
        var client = _httpClientFactory.CreateClient("ChannelEngine");
        var uri = new Uri($"{_configuration["OrderUrl"]}&apikey={_apiKey}");
        var response = await client.GetAsync(uri);
        response.EnsureSuccessStatusCode();
        var responseBody = response.Content.ReadAsStringAsync().Result;
        var orders = JsonConvert.DeserializeObject<OrderCollection>(responseBody);

        return orders.Content;
    }

    public async Task<IDictionary<string, Product>> GetAllProducts(IEnumerable<string> productNoList)
    {
        var productNoListString = string.Join('&', productNoList.Select(x => $"merchantProductNoList={x}"));
        var client = _httpClientFactory.CreateClient("ChannelEngine");
        var url = new Uri($"{_configuration["ProductUrl"]}&apikey={_apiKey}&{productNoListString}");
        var responseBody = await client.GetAsync(url).Result.Content.ReadAsStringAsync();
        var products = JsonConvert.DeserializeObject<ProductCollection>(responseBody).Content.ToDictionary(x => x.MerchantProductNo);

        return products;
    }

    #endregion
}