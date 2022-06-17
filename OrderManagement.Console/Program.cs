using ConsoleTables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Shared.Contracts;
using OrderManagement.Shared.Services;

namespace OrderManagement.Console;

internal class Program
{
    #region Fields

    private static IOrderService _orderService;
    private const int StockValue = 25;

    #endregion

    #region Public Methods

    public static async Task Main()
    {
        var services = new ServiceCollection();
        SetupConfiguration(services);
        _orderService = GetOrderService(services);
        if (_orderService == null)
        {
            System.Console.WriteLine("Api services are not available!");
            return;
        }

        System.Console.WriteLine("Press D to display top 5 products");
        System.Console.WriteLine("Press S to set the stock of a product");

        switch (System.Console.ReadKey(true).Key)
        {
            case ConsoleKey.D:
                await DisplayProducts();
                break;
            case ConsoleKey.S:
                await SetStock(StockValue);
                break;
            default:
                return;
        }
    }

    #endregion

    #region Private Methods

    private static IOrderService GetOrderService(ServiceCollection services)
    {
        return services
            .BuildServiceProvider()
            .GetService<IOrderService>();
    }

    private static async Task DisplayProducts()
    {
        var productSales = await _orderService.GetTopFiveSoldProducts();
        var table = new ConsoleTable("#", "Product Name", "Product Number", "Gtin", "Quantity");
        var i = 1;
        foreach (var productSale in productSales)
        {
            table.AddRow(i++, productSale.Name, productSale.ProductNo, productSale.Gtin, productSale.Quantity);
        }

        table.Options.NumberAlignment = Alignment.Left;
        table.Write();
        System.Console.WriteLine();
    }

    private static async Task SetStock(int stock)
    {
        var productSales = await _orderService.GetTopFiveSoldProducts();
        if (productSales.Count == 0)
        {
            System.Console.WriteLine("There isn't any product in the list.");
            return;
        }
        await DisplayProducts();
        System.Console.WriteLine("Please select the index of a product to set its stock amount:");

        var key = System.Console.ReadLine();
        if (key != null)
        {
            if (!int.TryParse(key, out var index))
            {
                System.Console.WriteLine("Invalid input!");
                return;
            }
            if (index < 1 || index > productSales.Count)
            {
                System.Console.WriteLine("Index out of range!");
                return;
            }

            var product = productSales[index - 1];
            await _orderService.SetStock(product.ProductNo, stock);
            System.Console.WriteLine($"The stock of {product.Name} product has been set to {stock}");
        }
    }

    private static void SetupConfiguration(ServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        ConfigureServices(services, configuration);
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IOrderService, OrderService>();
        services.AddTransient<IOrderApiClient, OrderApiClient>();
        services.AddSingleton(configuration);
        services.AddHttpClient("namedType", c =>
        {
            c.BaseAddress = new Uri("https://api-dev.channelengine.net/api/v2");
            double timeOut = 10;
            if (configuration.GetChildren().Any(c => c.Key == "HttpRequestTimeOutSecond"))
            {
                timeOut = double.Parse(configuration["HttpRequestTimeOutSecond"]);
            }
            c.Timeout = TimeSpan.FromSeconds(timeOut);
        });
    }

    #endregion
}