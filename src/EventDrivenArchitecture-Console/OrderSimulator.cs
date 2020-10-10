using Azure.Cosmos;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaleLearnCode.ChChChChanges.Common;

namespace TaleLearnCode.ChChChChanges.EventDrivenArchitecture
{
	public class OrderSimulator
	{

		private List<FakeName> _fakeNames = new List<FakeName>();
		private List<Product> _products = new List<Product>();

		public OrderSimulator()
		{
			GetFakeNames();
			GetProducts();
		}

		public async Task SimulateOrdersAsync()
		{
			using CosmosClient client = new CosmosClient(Settings.CosmosConnectionString);
			CosmosDatabase database = client.GetDatabase(Settings.OrderManagementDatabaseName);
			CosmosContainer container = database.GetContainer(Settings.OrdersContainerName);

			while (true)
			{
				var order = GetOrder();
				await container.CreateItemAsync(order);
				Console.WriteLine($"Saved order #{order.Id}");
				await Task.Delay(500);
			}
		}

		private void GetFakeNames()
		{

			Console.WriteLine("Generating fake names for the demo...");

			using var parser = new TextFieldParser($@"C:\Users\chadg\Downloads\FakeNameGenerator.com_100eedb1\FakeNames.csv");
			parser.TextFieldType = FieldType.Delimited;
			parser.SetDelimiters(",");
			while (!parser.EndOfData)
			{
				string[] fields = parser.ReadFields();
				_fakeNames.Add(new FakeName()
				{
					FirstName = fields[FakeNameFields.GivenName],
					LastName = fields[FakeNameFields.Surname],
					StreetAddress = fields[FakeNameFields.StreetAddress],
					City = fields[FakeNameFields.City],
					State = fields[FakeNameFields.State],
					PostalCode = fields[FakeNameFields.ZipCode],
					Country = fields[FakeNameFields.Country],
					EmailAddress = fields[FakeNameFields.EmailAddress],
					UserName = fields[FakeNameFields.UserName]
				});
			}

		}

		private void GetProducts()
		{
			Console.WriteLine("Generating fake products for the demo...");
			_products.Add(new Product("75291", "Lego Death Start Final Duel", 99.99m));
			_products.Add(new Product("75293", "Resistance I-TS Transport", 99.99m));
			_products.Add(new Product("75286", "General Grievous's Starfighter", 79.99m));
			_products.Add(new Product("75318", "The Child", 79.99m));
			_products.Add(new Product("75278", "D-O", 69.99m));
			_products.Add(new Product("75284", "Knights of Ren™ Transport Ship", 69.99m));
			_products.Add(new Product("75277", "Boba Fett™ Helmet", 59.99m));
			_products.Add(new Product("75276", "Stormtrooper™ Helmet", 59.99m));
			_products.Add(new Product("75292", "The Razor Crest", 129.99m));
			_products.Add(new Product("21046", "Empire State Building", 129.99m));
		}

		private Order GetOrder()
		{
			var rand = new Random();

			var customer = _fakeNames[rand.Next(0, _fakeNames.Count)];
			var order = new Order()
			{
				UserName = customer.UserName,
				EmailAddress = customer.EmailAddress,
				ShippingAddress = new PostalAddress(customer.FirstName, customer.LastName, customer.StreetAddress, customer.State, customer.PostalCode, customer.Country),
			};

			var numberOfItems = rand.Next(1, 10);
			for (int i = 1; i <= numberOfItems; i++)
			{
				var product = _products[rand.Next(0, _products.Count)];
				order.OrderItems.Add(new OrderItem(product, rand.Next(1, 3)));
			}

			return order;

		}

	}

}