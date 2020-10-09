using System.Text.Json.Serialization;

namespace TaleLearnCode.ChChChChanges.Common
{

	public class CartItem
	{

		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("productName")]
		public string ProductName { get; set; }

		[JsonPropertyName("unitPrice")]
		public decimal UnitPrice { get; set; }

		[JsonPropertyName("quantity")]
		public decimal Quantity { get; set; }
	}

}