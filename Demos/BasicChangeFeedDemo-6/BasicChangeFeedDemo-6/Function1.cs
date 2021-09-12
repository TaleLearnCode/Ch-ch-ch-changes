using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace BasicChangeFeedDemo_6
{
	public static class Function1
	{
		[FunctionName("Function1")]
		public static void Run([CosmosDBTrigger(
						databaseName: "ToDoList",
						collectionName: "Items",
						ConnectionStringSetting = "CosmosConnectionString",
						LeaseCollectionName = "leases")]IReadOnlyList<Document> input, ILogger log)
		{
			if (input != null && input.Count > 0)
			{
				log.LogWarning("Documents modified " + input.Count);
				log.LogWarning("First document Id " + input[0].Id);
			}
		}
	}
}