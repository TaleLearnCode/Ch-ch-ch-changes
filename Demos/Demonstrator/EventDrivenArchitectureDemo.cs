using Azure.Cosmos;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaleLearnCode.ChChChChanges.Common;

namespace TaleLearnCode.ChChChChanges.Demonstrator
{

	public class EventDrivenArchitectureDemo
	{

		private readonly CosmosContainer _container;
		private List<FakeName> _fakeNames = new List<FakeName>();
		private List<Product> _products = new List<Product>();

		private EventDrivenArchitectureDemo(CosmosClient cosmosClient)
		{
			_container = cosmosClient
				.GetDatabase(Settings.OrderManagementDatabaseName)
				.GetContainer(Settings.OrdersContainerName);
			_fakeNames = GetFakeNames();
			_products = GetProducts();
		}

		public static async Task ExecuteAsync(CosmosClient cosmosClient)
		{
			await new EventDrivenArchitectureDemo(cosmosClient).ExecuteAsync();
		}

		private async Task ExecuteAsync()
		{
			WelcomeUser();
			do
			{
				while (!Console.KeyAvailable)
				{
					var order = GetOrder();
					await _container.CreateItemAsync(order);
					Console.WriteLine($"Saved order #{order.Id}");
					await Task.Delay(500);
				}
			} while (Console.ReadKey(true).Key != ConsoleKey.Escape);
		}

		private static void WelcomeUser()
		{
			Console.Clear();
			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(@"___________________      _____   ");
			Console.WriteLine(@"\_   _____/\______ \    /  _  \  ");
			Console.WriteLine(@" |    __)_  |    |  \  /  /_\  \ ");
			Console.WriteLine(@" |        \ |    `   \/    |    \");
			Console.WriteLine(@"/_______  //_______  /\____|__  /");
			Console.WriteLine(@"        \/         \/         \/ ");
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine();
		}

		private List<FakeName> GetFakeNames()
		{

			var returnValue = new List<FakeName>();

			Console.WriteLine("Generating fake names for the demo...");

			using var parser = new TextFieldParser($"{Settings.DataFolderPath}FakeNames.csv");
			parser.TextFieldType = FieldType.Delimited;
			parser.SetDelimiters(",");
			while (!parser.EndOfData)
			{
				string[] fields = parser.ReadFields();
				returnValue.Add(new FakeName()
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

			return returnValue;
		}

		private List<Product> GetProducts()
		{
			var returnValue = new List<Product>();
			Console.WriteLine("Generating fake products for the demo...");
			returnValue.Add(new Product("10267", "Creator Gingerbread House", 99.99m));
			returnValue.Add(new Product("10255", "Creator Assembly Square", 279.99m));
			returnValue.Add(new Product("10261", "Creator Roller Coaster", 379.99m));
			returnValue.Add(new Product("10256", "Crator Taj Mahal", 369.99m));
			returnValue.Add(new Product("75192", "Millennium Falcon", 799.99m));
			returnValue.Add(new Product("71040", "The Disney Castle", 349.99m));
			returnValue.Add(new Product("75290", "Mos Eisley Cantina", 349.99m));
			returnValue.Add(new Product("76161", "1989 Batwing", 199.99m));
			returnValue.Add(new Product("21323", "Ideas Grand Piano", 349.99m));
			returnValue.Add(new Product("10270", "Creator Bookshop", 179.99m));
			returnValue.Add(new Product("10264", "Creator Corner Garage", 199.99m));
			returnValue.Add(new Product("10275", "Elf Club House", 9.99m));
			returnValue.Add(new Product("10273", "Creator Haunted House", 249.99m));
			returnValue.Add(new Product("10262", "Creator James Bond™ Aston Martin DB5", 149.99m));
			returnValue.Add(new Product("75288", "AT-AT", 159.99m));
			returnValue.Add(new Product("75271", "Luke Skywalker's Landspeeder™", 29.99m));
			returnValue.Add(new Product("75292", "The Razor Crest", 129.99m));
			returnValue.Add(new Product("75275", "A-wing Starfighter™", 199.99m));
			returnValue.Add(new Product("21317", "Ideas Steamboat Willie", 89.99m));
			returnValue.Add(new Product("21316", "Ideas The Flintstones", 59.99m));
			returnValue.Add(new Product("10260", "Creator Downtown Diner", 169.99m));
			returnValue.Add(new Product("10255", "Creator Assembly Square", 279.99m));
			returnValue.Add(new Product("21034", "Architecture London", 39.99m));
			returnValue.Add(new Product("21047", "Architecture Las Vegas", 39.99m));
			returnValue.Add(new Product("21043", "Architecture San Francisco", 49.99m));
			returnValue.Add(new Product("21051", "Architecture Tokyo", 59.99m));
			returnValue.Add(new Product("21028", "Architecture New York City", 59.99m));
			returnValue.Add(new Product("21052", "Architecture Dubai", 59.99m));
			returnValue.Add(new Product("21045", "Architecture Trafalgar Square", 79.99m));
			returnValue.Add(new Product("21042", "Architecture Statue of Liberty", 119.99m));
			returnValue.Add(new Product("21046", "Architecture Empire State Building", 129.99m));
			returnValue.Add(new Product("21054", "Architecture The White House", 99.99m));

			return returnValue;
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
				OrderDateTime = DateTime.UtcNow
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