using System;
using System.Collections.Generic;
using System.Text;

namespace TaleLearnCode.ChChChChanges.Functions
{
	public static class Settings
	{

		// General Settings
		public static string CosmosConnectionString => "AccountEndpoint=https://cosmos-changefeed-eus2.documents.azure.com:443/;AccountKey=uKC2Tv6E4tsm2icnd2L7udcSCKCeIA2J33h7c1ZPN6EoYUyVGFzZ1FsyGrXVhFXz8BVzXUF4LX8RKQAO57aFdg==;";
		public static string DataFolderPath => @"C:\Repos\TaleLearnCode\Presentations\Ch-ch-ch-changes\Demos\Data\";

		// Archive Data
		public static string MoveDataDatabasebaseName => "moveData";
		public static string ArchivalContainerName => "archival";

		// Denormalize Data
		public static string ShindigManagerDatabaseName => "shindigs";
		public static string MetadataContainerName => "metadata";
		public static string PresentationsContainerName => "presentations";



		public static string OrderManagementDatabaseName => "";
		public static string OrdersContainerName => "";

	}

}