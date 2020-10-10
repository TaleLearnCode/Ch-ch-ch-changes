using Azure.Cosmos;
using ShellProgressBar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaleLearnCode.ChChChChanges.Common;

namespace TaleLearnCode.ChChChChanges.ArchiveData
{
	class Program
	{
		static async Task Main(string[] args)
		{
			WelcomeUser();
			await AddDataToCosmosAsync(GetQuestionInteractions());

			Console.Beep();
			Console.WriteLine("Done");
		}

		private static void WelcomeUser()
		{
			Console.Clear();
			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(@"   _____                .__    .__              .__   ");
			Console.WriteLine(@"  /  _  \_______   ____ |  |__ |__|__  _______  |  |  ");
			Console.WriteLine(@" /  /_\  \_  __ \_/ ___\|  |  \|  \  \/ /\__  \ |  |  ");
			Console.WriteLine(@"/    |    \  | \/\  \___|   Y  \  |\   /  / __ \|  |__");
			Console.WriteLine(@"\____|__  /__|    \___  >___|  /__| \_/  (____  /____/");
			Console.WriteLine(@"        \/            \/     \/               \/      ");
			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.White;
		}

		private static List<QuestionInteraction> GetQuestionInteractions()
		{
			Console.WriteLine();
			Console.WriteLine();
			return QuestionInteraction.GetListOfInteractions(@"D:\Repros\TaleLearnCode\Presentations\Ch-Ch-Ch-Changes\Data\QuestionInteractions.csv");
		}

		private static async Task AddDataToCosmosAsync(List<QuestionInteraction> questionInteractions)
		{

			Console.WriteLine();
			Console.WriteLine("Press any key to start the writing data to Cosmos...");
			Console.ReadKey();

			Console.WriteLine();
			Console.WriteLine();
			using var progressBar = new ProgressBar(questionInteractions.Count, "Connecting to database");

			CosmosClient client = new CosmosClient(Settings.CosmosConnectionString);
			CosmosDatabase database = client.GetDatabase(Settings.MoveDataDatabasebaseName);
			CosmosContainer container = database.GetContainer(Settings.ArchivalContainerName);

			for (int i = 0; i < questionInteractions.Count; i++)
			{
				progressBar.Tick($"Writing interaction {i + 1:n0} of {questionInteractions.Count:n0}");
				await container.CreateItemAsync(questionInteractions[i]);
				var delay = questionInteractions[i + 1].InteractionDateTime.Subtract(questionInteractions[i].InteractionDateTime);
				await Task.Delay(Convert.ToInt32(delay.TotalMilliseconds));
			}

		}

	}

}