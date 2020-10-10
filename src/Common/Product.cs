using System.Text.Json.Serialization;

namespace TaleLearnCode.ChChChChanges.Common
{

	public class Product
	{

		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("productName")]
		public string ProductName { get; set; }

		[JsonPropertyName("unitPrice")]
		public decimal UnitPrice { get; set; }

		public Product() { }

		public Product(string id, string productName, decimal unitPrice)
		{
			Id = id;
			ProductName = productName;
			UnitPrice = unitPrice;
		}

	}

}