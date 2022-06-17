using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Shared.Contracts;
using OrderManagement.Web.Models;

namespace OrderManagement.Web.Controllers;

public class HomeController : Controller
{
    #region Fields

    private readonly IOrderService _orderService;

    #endregion

    #region Constructors

    public HomeController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    #endregion

    #region Actions

    public async Task<IActionResult> Index()
    {
        var products = await _orderService.GetTopFiveSoldProducts();

        return View(products);
    }

    [HttpPost]
    public IActionResult SetStock(string productNo, int stock)
    {
       _orderService.SetStock(productNo, stock);

        return Ok();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    #endregion
}