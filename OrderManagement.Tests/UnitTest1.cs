using System.Collections.Generic;
using FakeItEasy;
using OrderManagement.Shared.Models;
using OrderManagement.Shared.Services;
using Xunit;

namespace OrderManagement.Tests;

public class UnitTest1
{
    [Fact]
    public void Sort_And_Sum_OrderResponse_Test()
    {
        var dummyApiClient = A.Fake<IOrderApiClient>();
        var orderResponseList = InitOrderResponseList();

        A.CallTo(() => dummyApiClient.GetAllInProgressOrders()).Returns(orderResponseList);

        var productDict = InitProducts();

        A.CallTo(() => dummyApiClient.GetAllProducts(A<IEnumerable<string>>.That
            .IsSameSequenceAs("5", "4", "7", "8", "2"))).Returns(productDict);

        var orderService = new OrderService(dummyApiClient);
        var topFiveSoldProducts = orderService.GetTopFiveSoldProducts().Result;

        Assert.Equal(5, topFiveSoldProducts.Count);

        Assert.Equal("5", topFiveSoldProducts[0].ProductNo);
        Assert.Equal("sweater", topFiveSoldProducts[0].Name);
        Assert.Equal("A", topFiveSoldProducts[0].Gtin);
        Assert.Equal(22, topFiveSoldProducts[0].Quantity);

        Assert.Equal("4", topFiveSoldProducts[1].ProductNo);
        Assert.Equal("coat", topFiveSoldProducts[1].Name);
        Assert.Equal("U", topFiveSoldProducts[1].Gtin);
        Assert.Equal(21, topFiveSoldProducts[1].Quantity);

        Assert.Equal("7", topFiveSoldProducts[2].ProductNo);
        Assert.Equal("jacket", topFiveSoldProducts[2].Name);
        Assert.Equal("E", topFiveSoldProducts[2].Gtin);
        Assert.Equal(20, topFiveSoldProducts[2].Quantity);

        Assert.Equal("8", topFiveSoldProducts[3].ProductNo);
        Assert.Equal("T-shirt", topFiveSoldProducts[3].Name);
        Assert.Equal("K", topFiveSoldProducts[3].Gtin);
        Assert.Equal(8, topFiveSoldProducts[3].Quantity);

        Assert.Equal("2", topFiveSoldProducts[4].ProductNo);
        Assert.Equal("shorts", topFiveSoldProducts[4].Name);
        Assert.Equal("F", topFiveSoldProducts[4].Gtin);
        Assert.Equal(8, topFiveSoldProducts[3].Quantity);
    }

    private static Dictionary<string, Product> InitProducts()
    {
        var productDict = new Dictionary<string, Product>
        {
            {
                "5", new Product
                {
                    MerchantProductNo = "5",
                    Name = "sweater",
                    Ean = "A"
                }
            },
            {
                "4", new Product
                {
                    MerchantProductNo = "4",
                    Name = "coat",
                    Ean = "U"
                }
            },
            {
                "7", new Product
                {
                    MerchantProductNo = "7",
                    Name = "jacket",
                    Ean = "E"
                }
            },
            {
                "8", new Product
                {
                    MerchantProductNo = "8",
                    Name = "T-shirt",
                    Ean = "K"
                }
            },
            {
                "2", new Product
                {
                    MerchantProductNo = "2",
                    Name = "shorts",
                    Ean = "F"
                }
            }
        };
        return productDict;
    }

    private static List<Order> InitOrderResponseList()
    {
        var orderResponseList = new List<Order>
        {
            new()
            {
                Lines = new List<OrderItem>
                {
                    new()
                    {
                        Gtin = "X",
                        MerchantProductNo = "1",
                        Quantity = 5
                    },
                    new()
                    {
                        Gtin = "F",
                        MerchantProductNo = "2",
                        Quantity = 7
                    },
                    new()
                    {
                        Gtin = "V",
                        MerchantProductNo = "3",
                        Quantity = 2
                    },
                    new()
                    {
                        Gtin = "U",
                        MerchantProductNo = "4",
                        Quantity = 1
                    },
                    new()
                    {
                        Gtin = "U",
                        MerchantProductNo = "4",
                        Quantity = 6
                    },
                    new()
                    {
                        Gtin = "A",
                        MerchantProductNo = "5",
                        Quantity = 4
                    },
                    new()
                    {
                        Gtin = "W",
                        MerchantProductNo = "6",
                        Quantity = 2
                    }
                }
            },
            new()
            {
                Lines = new List<OrderItem>
                {
                    new()
                    {
                        Gtin = "U",
                        MerchantProductNo = "4",
                        Quantity = 1
                    },
                    new()
                    {
                        Gtin = "U",
                        MerchantProductNo = "4",
                        Quantity = 6
                    },
                    new()
                    {
                        Gtin = "A",
                        MerchantProductNo = "5",
                        Quantity = 4
                    },
                    new()
                    {
                        Gtin = "W",
                        MerchantProductNo = "6",
                        Quantity = 2
                    },
                    new()
                    {
                        Gtin = "E",
                        MerchantProductNo = "7",
                        Quantity = 3
                    },
                    new()
                    {
                        Gtin = "E",
                        MerchantProductNo = "7",
                        Quantity = 7
                    },
                    new()
                    {
                        Gtin = "K",
                        MerchantProductNo = "8",
                        Quantity = 8
                    }
                }
            },
            new()
            {
                Lines = new List<OrderItem>
                {
                    new()
                    {
                        Gtin = "U",
                        MerchantProductNo = "4",
                        Quantity = 1
                    },
                    new()
                    {
                        Gtin = "U",
                        MerchantProductNo = "4",
                        Quantity = 6
                    },
                    new()
                    {
                        Gtin = "A",
                        MerchantProductNo = "5",
                        Quantity = 4
                    },
                    new()
                    {
                        Gtin = "A",
                        MerchantProductNo = "5",
                        Quantity = 2
                    },
                    new()
                    {
                        Gtin = "E",
                        MerchantProductNo = "7",
                        Quantity = 3
                    },
                    new()
                    {
                        Gtin = "E",
                        MerchantProductNo = "7",
                        Quantity = 7
                    },
                    new()
                    {
                        Gtin = "A",
                        MerchantProductNo = "5",
                        Quantity = 8
                    }
                }
            }
        };
        return orderResponseList;
    }
}