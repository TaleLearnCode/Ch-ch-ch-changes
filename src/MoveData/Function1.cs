using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace MoveData
{
	public static class Function1
	{
		[FunctionName("Function1")]
		public static void Run([CosmosDBTrigger(
			databaseName: "bca",
			collectionName: "ef",
			ConnectionStringSetting = "CosmosConnectionString",
			LeaseCollectionName = "leases",
			CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input, ILogger log)
		{
			if (input != null && input.Count > 0)
			{
				log.LogInformation("Twitch Id " + input[0].Id);
				log.LogInformation($"Stream Actual Name: {input[0].GetPropertyValue<string>("firstName")} {input[0].GetPropertyValue<string>("lastName")}");
			}
		}
	}
}
