using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TaleLearnCode.ChChChChanges.Common;

namespace TaleLearnCode.ChChChChanges.Functions
{
	public static class ArchiveData
	{

		private static readonly CloudBlobContainer _blobContainer;

		static ArchiveData()
		{

			var storageAccount = new CloudStorageAccount(
				new StorageCredentials(
					Environment.GetEnvironmentVariable("StorageAccountName"),
					Environment.GetEnvironmentVariable("StorageAccountKey")),
				useHttps: true);

			var blobClient = storageAccount.CreateCloudBlobClient();

			_blobContainer = blobClient.GetContainerReference(Environment.GetEnvironmentVariable("BlobContainerName"));

		}


		[FunctionName("ArchiveData")]
		public static async Task RunAsync([CosmosDBTrigger(
			databaseName: "moveData",
			collectionName: "archival",
			ConnectionStringSetting = "CosmosConnectionString",
			LeaseCollectionName = "leases",
			CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> documents, ILogger log)
		{
			if (documents != null && documents.Count > 0)
			{
				foreach (var document in documents)
				{
					try
					{
						await ArchiveDocument(document, log);
					}
					catch (Exception ex)
					{
						log.LogError($"Error archiving document id {document.Id}: {ex.Message}");
					}
				}
			}
		}

		private static async Task ArchiveDocument(Document document, ILogger log)
		{
			var questionInteraction = JsonSerializer.Deserialize<QuestionInteraction>(document.ToString());
			var blobName = $"{questionInteraction.School}-{questionInteraction.Id}";
			var blob = _blobContainer.GetBlockBlobReference(blobName);
			var bytes = Encoding.ASCII.GetBytes(document.ToString());
			await blob.UploadFromByteArrayAsync(bytes, 0, bytes.Length);
			log.LogInformation($"Archived '{blobName}' to blob storage");
		}

	}

}
