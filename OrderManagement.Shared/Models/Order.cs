namespace OrderManagement.Shared.Models;

public class Order
{
	public List<OrderItem> Lines { get; set; }
}

public class OrderCollection
{
	public IList<Order> Content { get; set; }
}