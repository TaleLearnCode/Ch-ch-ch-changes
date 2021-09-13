using System;
using System.Collections.Generic;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BasicChangeFeedDemo_5
{
    public static class Function1
    {
        [Function("Function1")]
        public static void Run([CosmosDBTrigger(
            databaseName: "ToDoList",
            collectionName: "Items",
            ConnectionStringSetting = "CosmosConnectionString",
            LeaseCollectionName = "leases")] IReadOnlyList<MyDocument> input, FunctionContext context)
        {
            var logger = context.GetLogger("Function1");
            if (input != null && input.Count > 0)
            {
                logger.LogWarning("Documents modified: " + input.Count);
                logger.LogWarning("First document Id: " + input[0].Id);
            }
        }
    }

    public class MyDocument
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public int Number { get; set; }

        public bool Boolean { get; set; }
    }
}
