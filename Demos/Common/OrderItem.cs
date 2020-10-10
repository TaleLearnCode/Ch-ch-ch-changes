using System.Text.Json.Serialization;

namespace TaleLearnCode.ChChChChanges.Common
{

	public class OrderItem : Product
	{


		[JsonPropertyName("quantity")]
		public int Quantity { get; set; }

		public OrderItem() : base() { }

		public OrderItem(Product product, int quantity)
		{
			Id = product.Id;
			ProductName = product.ProductName;
			UnitPrice = product.UnitPrice;
			Quantity = quantity;
		}

	}

}