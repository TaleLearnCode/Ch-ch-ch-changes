# Ch-ch-ch-changes: Tracing Changes in Azure Cosmos DB

0. [Setup](#setup)
1. [Basic Change Feed Demo](#basic-change-feed-demo)
2. Archiving Data
3. Denormalizing Data
4. Replicating Containers

---

## Setup

#### Create Cosmos DB Account
Ensure that a Cosmos DB account is created using the Core (SQL) API

#### Prepare for the Basic Change Feed Demo
1. Ensure that the 'Items' container is created via the "Quick Start" blade
2. Create the demo To Do items using the data below

~~~
{
    "title": "Basic Change Feed Demo",
    "sortOrder": 1,
    "assignedTo": "Chad Green",
    "status": "Not Started",
    "partitionKey": "EventName"
}

{
    "title": "Archiving Data",
    "sortOrder": 2,
    "assignedTo": "Chad Green",
    "status": "Not Started",
    "partitionKey": "EventName"
}

{
    "title": "Denormalizing Data",
    "sortOrder": 3,
    "assignedTo": "Chad Green",
    "status": "Not Started",
    "partitionKey": "EventName"
}

{
    "title": "Replicating Containers",
    "sortOrder": 4,
    "assignedTo": "Chad Green",
    "status": "Not Started",
    "partitionKey": "EventName"
}

{
    "title": "Event-Driven Architecture",
    "sortOrder": 5,
    "assignedTo": "Chad Green",
    "status": "Not Started",
    "partitionKey": "EventName"
}

{
    "title": "Real-Time Reporting",
    "sortOrder": 6,
    "assignedTo": "Chad Green",
    "status": "Not Started",
    "partitionKey": "EventName"
}

~~~

---

## Basic Change Feed Demo

1. Navigate to Cosmos database
2. Open the 'Data Explorer' blade
3. Click on the ToDoList database
4. Show off the to do items in the database
5. Open a SQL Query tab
6. Execute: ~SELECT c.title, c.status  FROM c WHERE c.partitionKey = 'KCDC'~
7. In Visual Studio, create an Azure Functions project using .NET Core 3.1 adding a Cosmos triggered function
8. Add the 'CosmosConnectionString' element to the local.settings
9. Talk through the generated code.
10. While talking through the generated code, add ~CreateLeaseCollectionIfNotExists = true~
11. Change the LogInformation to LogWarning (so it shows up better during the demo)
11. Start the Azure Function project
12. From the Azure Portal, make changes and show how it gets detected by the change feed
13. Show how deletes are not detected by the change feed