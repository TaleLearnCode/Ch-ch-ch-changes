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
	public static class DenormalizeData
	{

		private static readonly CosmosContainer _container;

		static DenormalizeData()
		{
			_container = new CosmosClient(Environment.GetEnvironmentVariable("CosmosConnectionString"))
				.GetDatabase(Environment.GetEnvironmentVariable("ShindigMangerDatabaseName"))
				.GetContainer(Environment.GetEnvironmentVariable("PresentationsContainerName"));
		}

		[FunctionName("DenormalizeData")]
		public static async Task RunAsync([CosmosDBTrigger(
			databaseName: "shindigs", // shindigManager
			collectionName: "metadata",
			ConnectionStringSetting = "CosmosConnectionString",
			LeaseCollectionName = "leases",
			LeaseCollectionPrefix = "Denormalize",
			CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> documents, ILogger log)
		{
			foreach (var document in documents)
			{
				try
				{
					await ProcessChangeAsync(document, log);
				}
				catch (Exception ex)
				{
					log.LogError(ex.Message);
				}
			}
		}

		private static async Task ProcessChangeAsync(Document document, ILogger log)
		{
			var item = JsonSerializer.Deserialize<Metadata>(document.ToString());
			switch (item.Type)
			{
				case "topic":
					await UpdateTopicAsync(item.Id, item.Name, log);
					break;
				case "tag":
					await UpdateTagAsync(item.Id, item.Name, log);
					break;
				case "speaker":
					var speaker = JsonSerializer.Deserialize<Speaker>(document.ToString());
					await UpdateSpeakerAsync(speaker.Id, speaker.FirstName, speaker.LastName, log);
					break;
			}

		}

		private static async Task UpdateTopicAsync(string topicId, string topicName, ILogger log)
		{
			var presentationsToUpdate = await GetPresentationsToUpdateAsync("topics", topicId, log);
			if (presentationsToUpdate.Any())
			{
				foreach (var id in presentationsToUpdate)
				{
					var sql = $"SELECT * from presentations WHERE presentations.id = '{id}'";
					QueryDefinition queryDefinition = new QueryDefinition(sql);
					await foreach (var presentation in _container.GetItemQueryIterator<Presentation>(queryDefinition))
					{
						var topic = ((IEnumerable<Metadata>)presentation.Topics).First(t => t.Id == topicId);
						topic.Name = topicName;
						await _container.ReplaceItemAsync(presentation, presentation.Id);
						log.LogInformation($"Updated a topic on {presentation.Title}");
					}
				}
			}
		}

		private static async Task UpdateTagAsync(string tagId, string topicName, ILogger log)
		{
			var presentationsToUpdate = await GetPresentationsToUpdateAsync("tags", tagId, log);
			if (presentationsToUpdate.Any())
			{
				foreach (var id in presentationsToUpdate)
				{
					var sql = $"SELECT * from presentations WHERE presentations.id = '{id}'";
					QueryDefinition queryDefinition = new QueryDefinition(sql);
					await foreach (var presentation in _container.GetItemQueryIterator<Presentation>(queryDefinition))
					{
						try
						{
							var tag = ((IEnumerable<Metadata>)presentation.Tags).First(t => t.Id == tagId);
							tag.Name = topicName;
							await _container.ReplaceItemAsync(presentation, presentation.Id);
							log.LogInformation($"Updated a tag on {presentation.Title}");
						}
						catch (Exception ex)
						{
							log.LogError(ex.Message);
						}
					}
				}
			}
		}

		private static async Task UpdateSpeakerAsync(string speakerId, string firstName, string lastName, ILogger log)
		{
			var presentationsToUpdate = await GetPresentationsToUpdateAsync("speakers", speakerId, log);
			if (presentationsToUpdate.Any())
			{
				foreach (var id in presentationsToUpdate)
				{
					var sql = $"SELECT * from presentations WHERE presentations.id = '{id}'";
					QueryDefinition queryDefinition = new QueryDefinition(sql);
					await foreach (var presentation in _container.GetItemQueryIterator<Presentation>(queryDefinition))
					{
						try
						{
							var speaker = ((IEnumerable<Speaker>)presentation.Speakers).First(t => t.Id == speakerId);
							speaker.FirstName = firstName;
							speaker.LastName = lastName;
							await _container.ReplaceItemAsync(presentation, presentation.Id);
							log.LogInformation($"Updated a speaker on {presentation.Title}");
						}
						catch (Exception ex)
						{
							log.LogError(ex.Message);
						}
					}
				}
			}
		}

		private static async Task<List<string>> GetPresentationsToUpdateAsync(string type, string metadataId, ILogger log)
		{
			var metadataToUpdate = new List<string>();
			try
			{
				var sql = $"SELECT presentations.id FROM presentations JOIN {type} IN presentations.{type} WHERE {type}.id = '{metadataId}'";
				QueryDefinition queryDefinition = new QueryDefinition(sql);
				await foreach (QueryId queryId in _container.GetItemQueryIterator<QueryId>(queryDefinition))
					metadataToUpdate.Add(queryId.Id);
			}
			catch (Exception ex)
			{
				log.LogError(ex.Message);
			}
			return metadataToUpdate;
		}


	}
}
