using Azure.Cosmos;
using Microsoft.VisualBasic.FileIO;
using ShellProgressBar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaleLearnCode.ChChChChanges.Common;

namespace TaleLearnCode.ChChChChanges.Demonstrator
{

	public class DenormalizeDemo
	{

		private readonly CosmosDatabase _database;

		private DenormalizeDemo(CosmosClient cosmosClient)
		{
			_database = cosmosClient.GetDatabase(Settings.ShindigManagerDatabaseName);
		}

		public static async Task ExecuteAsync(CosmosClient cosmosClient)
		{
			await new DenormalizeDemo(cosmosClient).ExecuteAsync();
		}

		private async Task ExecuteAsync()
		{
			var execute = true;
			while (execute)
			{
				WelcomeUser();
				Console.WriteLine("Select an option:");
				Console.WriteLine("\tUpload [P]resentations");
				Console.WriteLine("\tUpload [M]etedata");
				Console.WriteLine("\tE[x]it");
				var cki = Console.ReadKey();
				if (cki.Key == ConsoleKey.P) await AddPresentationsToCosmosAsync();
				if (cki.Key == ConsoleKey.M) await AddMetadataToCosmosAsync();
				if (cki.Key == ConsoleKey.X) execute = false;
			}
		}

		private static void WelcomeUser()
		{
			Console.Clear();
			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(@"________                                            .__  .__               ");
			Console.WriteLine(@"\______ \   ____   ____   ___________  _____ _____  |  | |__|_______ ____  ");
			Console.WriteLine(@" |    |  \_/ __ \ /    \ /  _ \_  __ \/     \\__  \ |  | |  \___   // __ \ ");
			Console.WriteLine(@" |    `   \  ___/|   |  (  <_> )  | \/  Y Y  \/ __ \|  |_|  |/    /\  ___/ ");
			Console.WriteLine(@"/_______  /\___  >___|  /\____/|__|  |__|_|  (____  /____/__/_____ \\___  >");
			Console.WriteLine(@"        \/     \/     \/                   \/     \/              \/    \/ "); Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.White;
		}

		private (List<Metadata> Metadata, List<Speaker> Speakers) GetMetadata()
		{

			var metadata = new List<Metadata>();
			var speakers = new List<Speaker>();

			using var parser = new TextFieldParser($"{Settings.DataFolderPath}Metadata.csv");
			parser.TextFieldType = FieldType.Delimited;
			parser.SetDelimiters(",");
			while (!parser.EndOfData)
			{
				string[] fields = parser.ReadFields();
				if (fields[MetadataFields.Type] != "speaker")
					metadata.Add(
						new Metadata(
							fields[MetadataFields.Type],
							fields[MetadataFields.Id],
							fields[MetadataFields.Name]));
				else
					speakers.Add(
						new Speaker(
							fields[MetadataFields.Id],
							fields[MetadataFields.FirstName],
							fields[MetadataFields.LastName]));
			}

			return (metadata, speakers);

		}

		private async Task AddMetadataToCosmosAsync()
		{

			Console.WriteLine();
			Console.WriteLine("Retrieving the metadata...");
			var metadata = GetMetadata();

			Console.WriteLine();
			using var progressBar = new ProgressBar(metadata.Metadata.Count + metadata.Speakers.Count, "Connecting to the Cosmos DB container");
			int counter = 0;

			CosmosContainer container = _database.GetContainer(Settings.MetadataContainerName);

			foreach (var item in metadata.Metadata)
			{
				counter++;
				await container.CreateItemAsync(item);
				progressBar.Tick($"Writing metadata item {counter} of {metadata.Metadata.Count + metadata.Speakers.Count}");
			}

			foreach (var speaker in metadata.Speakers)
			{
				counter++;
				await container.CreateItemAsync(speaker);
				progressBar.Tick($"Writing metadata item {counter} of {metadata.Metadata.Count + metadata.Speakers.Count}");
			}

		}

		private async Task AddPresentationsToCosmosAsync()
		{

			Console.WriteLine();
			Console.WriteLine("Retrieving the presentations...");
			var presentations = GetAllPresentations();

			Console.WriteLine();
			using var progressBar = new ProgressBar(presentations.Count, "Connecting to the Cosmos DB container");
			int counter = 0;

			CosmosContainer container = _database.GetContainer(Settings.PresentationsContainerName);

			foreach (var presentation in presentations)
			{
				counter++;
				await container.CreateItemAsync(presentation);
				progressBar.Tick($"Writing presentation {counter} of {presentations.Count}");
			}

		}

		private List<Presentation> GetAllPresentations()
		{
			var presentations = new List<Presentation>();
			presentations.AddRange(GetEventPresentations("CPL19"));
			presentations.AddRange(GetEventPresentations("CPL20"));
			return presentations;
		}

		private List<Presentation> GetEventPresentations(string eventId)
		{

			var presentations = new Dictionary<string, Presentation>();

			using var parser = new TextFieldParser($"{Settings.DataFolderPath}{eventId}.csv");
			parser.TextFieldType = FieldType.Delimited;
			parser.SetDelimiters(",");
			while (!parser.EndOfData)
			{
				string[] fields = parser.ReadFields();

				if (presentations.ContainsKey(fields[PresentationFields.Id]))
				{

					if (presentations[fields[PresentationFields.Id]].Speakers.FindIndex(x => x.Id == fields[PresentationFields.SpeakerId]) == -1)
						presentations[fields[PresentationFields.Id]].Speakers.Add(
							new Speaker(
								fields[PresentationFields.SpeakerId],
								fields[PresentationFields.SpeakerFirstName],
								fields[PresentationFields.SpeakerLastName]));

					if (presentations[fields[PresentationFields.Id]].Topics.FindIndex(x => x.Id == fields[PresentationFields.TopicId]) == -1)
						presentations[fields[PresentationFields.Id]].Topics.Add(
							Metadata.TopicFactory(fields[PresentationFields.TopicId],
							fields[PresentationFields.TopicName]));

					if (presentations[fields[PresentationFields.Id]].Tags.FindIndex(x => x.Id == fields[PresentationFields.TagId]) == -1)
						presentations[fields[PresentationFields.Id]].Tags.Add(
							Metadata.TagFactory(fields[PresentationFields.TagId],
							fields[PresentationFields.TagName]));

				}
				else
					presentations.Add(fields[PresentationFields.Id], new Presentation(fields));
			}

			return presentations.Values.ToList();

		}

	}

}