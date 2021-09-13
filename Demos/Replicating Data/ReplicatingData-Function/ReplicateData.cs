using Azure.Cosmos;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TaleLearnCode.ChChChChanges.Common;

namespace TaleLearnCode.ChChChChanges.Functions
{
	public static class ReplicateData
	{

		private static readonly CosmosContainer _container;

		static ReplicateData()
		{
			_container = new CosmosClient(Environment.GetEnvironmentVariable("CosmosConnectionString"))
				.GetDatabase(Environment.GetEnvironmentVariable("DatabaseName"))
				.GetContainer(Environment.GetEnvironmentVariable("PresentationsByTagContainerName"));
		}

		[FunctionName("ReplicateData")]
		public static async Task RunAsync([CosmosDBTrigger(
			databaseName: "replicateData",
			collectionName: "presentations",
			ConnectionStringSetting = "CosmosConnectionString",
			LeaseCollectionName = "leases",
			LeaseCollectionPrefix = "ReplicateData",
			CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> documents, ILogger log)
		{

			if (documents != null && documents.Count > 0)
			{
				foreach (var document in documents)
				{
					try
					{

						// Build the list of PresenationByTag objects
						var presentation = JsonSerializer.Deserialize<Presentation>(document.ToString());
						var presentationsByTag = new List<PresentationByTag>();
						if (presentation.Tags.Any())
							foreach (var tag in presentation.Tags)
								presentationsByTag.Add(new PresentationByTag(presentation, tag));


						if (document.TimeToLive == null)
						{

							if (presentationsByTag.Any())
							{
								// Upsert present tags
								var counter = 0;
								foreach (PresentationByTag presentationByTag in presentationsByTag)
								{
									await _container.UpsertItemAsync(presentationByTag);
									counter++;
								}
								log.LogInformation($"Upserted {counter} documents into the PresentationsByTag container");

								// Handle deleted tags
								var sql = $"SELECT * FROM p WHERE p.presentationId = '{presentation.Id}'";
								QueryDefinition queryDefinition = new QueryDefinition(sql);
								await foreach (PresentationByTag presentationByTag in _container.GetItemQueryIterator<PresentationByTag>(queryDefinition))
								{
									if (presentation.Tags.FindIndex(x => x.Id == presentationByTag.TagId) == -1)
									{
										await _container.DeleteItemAsync<PresentationByTag>(presentationByTag.Id, new Azure.Cosmos.PartitionKey(presentationByTag.TagId));
										log.LogInformation($"Deleted PresentationById item {presentationByTag.Id} because the tag has been removed from the presentation.");
									}
								}
							}
						}
						else
						{
							// Handle presentation deletions
							var counter = 0;
							foreach (var presentationByTag in presentationsByTag)
							{
								await _container.DeleteItemAsync<PresentationByTag>(presentationByTag.Id, new Azure.Cosmos.PartitionKey(presentationByTag.TagId));
								counter++;
							}
							log.LogInformation($"Deleted {counter} documents from the PresentationsByTag container");
						}
					}
					catch (Exception ex)
					{
						log.LogError($"Error processing change for presentation id {document.Id}: {ex.Message}");
					}
				}
			}
		}

	}
}
