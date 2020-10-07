using Azure.Cosmos;
using Microsoft.VisualBasic.FileIO;
using ShellProgressBar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TaleLearnCode.ChChChChanges.Common;

namespace TaleLearnCode.ChChChChanges.DenormalizeData
{
	class Program
	{
		static async Task Main(string[] args)
		{
			WelcomeUser();
			await AddDataToCosmosAsync();
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

		private static async Task AddDataToCosmosAsync()
		{

			var presentations = GetPresentations();

			Console.WriteLine();
			Console.WriteLine("Press any key to start the writing data to Cosmos...");
			Console.ReadKey();
			Console.WriteLine();
			Console.WriteLine();
			using var progressBar = new ProgressBar(presentations.Count, "Connecting to the database");

			CosmosClient client = new CosmosClient(Settings.CosmosConnectionString);
			CosmosDatabase database = client.GetDatabase(Settings.ShindigManagerDatabaseName);
			CosmosContainer container = database.GetContainer(Settings.ShindigsContainerName);

			int counter = 0;

			foreach (var presentation in presentations)
			{
				counter++;
				await container.CreateItemAsync(presentation);
				progressBar.Tick($"Writing presentation {counter} of {presentations.Count}");
			}

		}

		private static List<Presentation> GetPresentations()
		{

			Console.WriteLine();
			Console.WriteLine("Which event should be imported: ");
			Console.WriteLine("\t[CPL19] Code PaLOUsa 2019");
			Console.WriteLine("\t[CPL20] Code PaLOUsa 2020");
			var eventId = Console.ReadLine();

			var presentations = new Dictionary<string, Presentation>();

			using var parser = new TextFieldParser($@"D:\Repros\TaleLearnCode\Presentations\Ch-Ch-Ch-Changes\Data\{eventId}.csv");
			parser.TextFieldType = FieldType.Delimited;
			parser.SetDelimiters(",");
			while (!parser.EndOfData)
			{
				string[] fields = parser.ReadFields();
				if (presentations.ContainsKey(fields[PresentationFields.Id]))
				{
					if (!presentations[fields[PresentationFields.Id]].Speakers.Contains(fields[PresentationFields.Speaker]))
						presentations[fields[PresentationFields.Id]].Speakers.Add(fields[PresentationFields.Speaker]);
					if (!presentations[fields[PresentationFields.Id]].Topics.Contains(fields[PresentationFields.Topic]))
						presentations[fields[PresentationFields.Id]].Topics.Add(fields[PresentationFields.Topic]);
					if (!presentations[fields[PresentationFields.Id]].Tags.Contains(fields[PresentationFields.Tag]))
						presentations[fields[PresentationFields.Id]].Tags.Add(fields[PresentationFields.Tag]);
				}
				else
					presentations.Add(fields[PresentationFields.Id], new Presentation(fields));
			}

			return presentations.Values.ToList();

		}

	}

}