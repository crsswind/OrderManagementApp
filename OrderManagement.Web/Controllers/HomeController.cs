using Microsoft.AspNetCore.Mvc;
using OrderManagement.Shared.Services;

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

    #endregion
}