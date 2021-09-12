using System;
using System.Collections.Generic;
using System.Text;

namespace TaleLearnCode.ChChChChanges.Functions
{
	public static class Settings
	{
		public static string CosmosConnectionString => "AccountEndpoint=https://cosmos-changefeed-eus2.documents.azure.com:443/;AccountKey=uKC2Tv6E4tsm2icnd2L7udcSCKCeIA2J33h7c1ZPN6EoYUyVGFzZ1FsyGrXVhFXz8BVzXUF4LX8RKQAO57aFdg==;";
		public static string MoveDataDatabasebaseName => "moveData";
		public static string ArchivalContainerName => "stchangeseus2";
		public static string DataFolderPath => @"C:\Repos\TaleLearnCode\Presentations\Ch-ch-ch-changes\Demos\Data\";

		public static string ShindigManagerDatabaseName => "";
		public static string PresentationsByTagContainerName => "";
	}
}