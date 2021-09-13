# Ch-ch-ch-changes: Tracing Changes in Azure Cosmos DB

0. [Basic Change Feed Demo](#basic-change-feed-demo)
1. [Archiving Data](#archiving-data)
2. [Denormalizing Data](#denormalizing-data)
3. [Replicating Containers](#replicating-containers)
4. Event-Driven Architecture

All of these demos assume that a Cosmos DB account has been created using the Core (SQL) API.

---

## Basic Change Feed Demo

#### Prep-Work

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

#### Demo Steps

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

---

## Archiving Data

#### Prep-Work

1. Prepare the Azure Storage Blob container to accept the archived data
* Ensure that that the container is empty

2. Prepare the Azure Cosmos DB account to accept the data
* Ensure that there is a ~moveData~ database with an empty ~archival~ container

3. Add the following settings to the local.settings.json file within the ArchiveData-Function project:
* CosmosConnectionString
* StorageAccountName
* StorageAccountKey
* BlobContainerName

4. Ensure the following settings are present in the ignored Setting class in the Demonstrator project:
* MoveDataDatabasebaseName
* ArchivalContainerName
* DataFolderPath

#### Demo Steps

1. Talk about how we are going to stimulate the questions interaction scenario
2. Talk through the code within ArchiveData function
3. Navigate to the *archival* container in the *moveData* container to show that it is empty
4. Navigate to the *data-archival* container in the Azure Storage account to show that it is empty
5. Ensure that the ArchiveData-Function and Demonstrator projects are set to start
6. Run the demonstrator project and start the Archive Data demo and talk through what's happening

---

## Denormalizing Data

#### Prep-Work

1. Create an Azure Cosmos DB database labeled *denormalizeData*

2. Create a container within the *denormalizeData* database labeled *metadata* with a partition key named *type*

3. Create a container within the *denormalizeData* database labeled *presentations* with a partition key named *eventId*

4. Validate the following properties within the *Demonstrator* Settings class:
* DataFolderPath
* PresentationsContainerName
* MetadataContainerName
* DenormalizeDataDatabaseName

4. Run the *Demonstrator* project and go to the Denormalization demo and perform the following tasks:
* Upload Presentations
* Upload Metadata

5. Validate the following settings within the *DenomralizationData-Function* local.setings.json:
* CosmosConnectionString
* DatabaseName
* MetadataContainerName
* PresentationsContainerName

#### Demo Steps
1. Talk about how we are going to stimulate changes happening within denormalized data
2. Talk through the code within the DenormalizeData function
3. Run the DenormalizationData-Function project
4. Perform the following query against the *presentations* container:

~~~
SELECT p.title, topics.name
  FROM presentations p
  JOIN topics IN p.topics
 WHERE topics.id = 'Topic-5'
~~~

5. Go to the *metadata* container, find Topic-5, change 'Soft Skills' to 'People Skills'
6. Show how the Azure Function was triggered
7. Rerun the query above against the *presentations* container and show how 'Soft Skills' has been changed back to 'People Skills'

---

## Replicating Containers

#### Prep-Work

1. Create an Azure Cosmos DB database labeled *replicateData*

2. Create a container within the *replicateData* database labeled *presentations* with a partition key named *eventId*

3. Create a container within the *replicateData* database labeled *presentationsByTag* with a partition key named *tagId*

4. Validate the following properties within the *Demonstrator* Settings class:
* DataFolderPath
* PresentationsContainerName
* ReplicateDataDatabaseName

5. Validate the following settings within the *ReplicatingData-Function* local.setings.json:
* CosmosConnectionString
* DatabaseName
* PresentationsByTagContainerName

#### Demo Steps
1. Talk about how we are going to stimulate replicating data to different partitions to improve search
2. In the Azure Portal, show how both the *Presentations* and *PresentationsByTag* containers are empty
3. Talk through the code within the *ReplicatingData* function
4. Ensure that the ReplicateData-Function and Demonstrator projects are set to start
5. Start the solution projects
6. Start the *Replicating Containers* demo from the *Demonstrator* project
7. Show how the records have been added to the *PresentationsByTag* container

---

## Event-Driven Architecture

#### Prep-Work

1. Create an Azure Cosmos DB database labeled *eventDrivenArchitecture*

2. Create a container within the *eventDrivenArchitecture* database labeled *orders* with a partition key named *userName*

3. Validate the following properties within the *Demonstrator* Settings class:
* DataFolderPath
* EventDrivenArchitectureDatabaseName
* OrdersContainerName

5. Validate the following settings within the *EventDrivenArchitecture-Function-Fulfillment* local.setings.json:
* CosmosConnectionString
* DatabaseName
* OrdersContainerName

6. Validate the following settings within the *EventDrivenArchitecture-Function-Inventory* local.setings.json:
* CosmosConnectionString
* DatabaseName
* OrdersContainerName

7. Validate the following settings within the *EventDrivenArchitecture-Function-Notification* local.setings.json:
* CosmosConnectionString
* DatabaseName
* OrdersContainerName

#### Demo Steps
1. Talk about how we are going to stimulate an event-driven architecture demonstrating an order processing system
3. Talk through the code within the *EventDrivenArchitecture-Function-Fulfillment* function
4. Ensure that the *EventDrivenArchitecture-Function-Fulfillment*, *EventDrivenArchitecture-Function-Inventory*, *EventDrivenArchitecture-Function-Notification* and *Demonstrator* projects are set to start
5. Start the solution projects
6. Start the *Event-Driven Architecture* demo from the *Demonstrator* project
7. Show how all the data has been updated within the *orders* container