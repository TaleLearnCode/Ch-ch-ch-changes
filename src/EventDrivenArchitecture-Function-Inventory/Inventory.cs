using Azure.Cosmos;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using TaleLearnCode.ChChChChanges.Common;

namespace TaleLearnCode.ChChChChanges.Functions
{
	public static class Inventory
	{

		private static readonly CosmosClient _client;
		private static readonly CosmosDatabase _database;
		private static readonly CosmosContainer _container;

		static Inventory()
		{
			_client = new CosmosClient(Settings.CosmosConnectionString);
			_database = _client.GetDatabase(Settings.OrderManagementDatabaseName);
			_container = _database.GetContainer(Settings.OrdersContainerName);
		}

		[FunctionName("Inventory")]
		public static async System.Threading.Tasks.Task RunAsync([CosmosDBTrigger(
			databaseName: "orderManagement",
			collectionName: "orders",
			ConnectionStringSetting = "CosmosConnectionString",
			LeaseCollectionName = "leases",
			LeaseCollectionPrefix = "Inventory",
			CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> documents, ILogger log)
		{

			var exceptions = new List<Exception>();

			if (documents != null && documents.Count > 0)
			{

				foreach (var document in documents)
				{
					try
					{
						// Do the necessary inventory work here
						var order = JsonSerializer.Deserialize<Order>(document.ToString());
						if (order.InventoryManagementDateTime is null)
						{
							log.LogInformation($"Starting Inventory of order #{order.Id}");
							order.InventoryManagementDateTime = DateTime.UtcNow;
							await _container.UpsertItemAsync(order);
						}
					}
					catch (Exception ex)
					{
						// We need to keep processing the rest of the batch - capture this exception and continue.
						// Also, consider capturing details of the message that failed processing so it can be processed again later.
						exceptions.Add(ex);
					}

				}
			}

			// Once processing of the batch is complete, if any messages in the batch failed processing throw an exception so that there is a record of the failure.
			if (exceptions.Count > 1)
				throw new AggregateException(exceptions);

			if (exceptions.Count == 1)
				throw exceptions.Single();

		}

	}

}