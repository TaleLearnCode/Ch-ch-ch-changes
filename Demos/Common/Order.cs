using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TaleLearnCode.ChChChChanges.Common
{

	public class Order
	{

		[JsonPropertyName("id")]
		public string Id { get; set; } = Guid.NewGuid().ToString();

		[JsonPropertyName("userName")]
		public string UserName { get; set; }

		[JsonPropertyName("emailAddress")]
		public string EmailAddress { get; set; }

		[JsonPropertyName("shippingAddress")]
		public PostalAddress ShippingAddress { get; set; }

		[JsonPropertyName("orderItems")]
		public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

		[JsonPropertyName("orderDateTime")]
		public DateTime? OrderDateTime { get; set; }

		[JsonPropertyName("fulfillmentDateTime")]
		public DateTime? FulfillmentDateTime { get; set; }

		[JsonPropertyName("inventoryManagementDateTime")]
		public DateTime? InventoryManagementDateTime { get; set; }

		[JsonPropertyName("orderConfirmedDateTime")]
		public DateTime? OrderConfirmedDateTime { get; set; }

	}

}