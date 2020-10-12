using Azure.Cosmos;
using ShellProgressBar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaleLearnCode.ChChChChanges.Common;

namespace TaleLearnCode.ChChChChanges.Demonstrator
{

	public class ArchivalDemo
	{

		private readonly CosmosContainer _container;

		private ArchivalDemo(CosmosClient cosmosClient)
		{
			_container = cosmosClient
				.GetDatabase(Settings.MoveDataDatabasebaseName)
				.GetContainer(Settings.ArchivalContainerName);
		}

		public static async Task ExecuteAsync(CosmosClient cosmosClient)
		{
			await new ArchivalDemo(cosmosClient).ExecuteAsync();
		}

		private async Task ExecuteAsync()
		{
			WelcomeUser();
			await AddDataToCosmosAsync(GetQuestionInteractions());
		}

		private void WelcomeUser()
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

		private List<QuestionInteraction> GetQuestionInteractions()
		{
			Console.WriteLine();
			Console.WriteLine();
			return QuestionInteraction.GetListOfInteractions($"{Settings.DataFolderPath}QuestionInteractions.csv");
		}

		private async Task AddDataToCosmosAsync(List<QuestionInteraction> questionInteractions)
		{

			Console.WriteLine();
			Console.WriteLine("Press any key to start the writing data to Cosmos...");
			Console.ReadKey();

			Console.WriteLine();
			Console.WriteLine();
			using var progressBar = new ProgressBar(questionInteractions.Count, "Connecting to database");

			int index = 0;
			do
			{
				while (!Console.KeyAvailable && index < questionInteractions.Count)
				{
					progressBar.Tick($"Writing interaction {index + 1:n0} of {questionInteractions.Count:n0}");
					await _container.CreateItemAsync(questionInteractions[index]);
					var delay = questionInteractions[index + 1].InteractionDateTime.Subtract(questionInteractions[index].InteractionDateTime);
					await Task.Delay(Convert.ToInt32(delay.TotalMilliseconds));
					index++;
				}
				if (index >= questionInteractions.Count - 1) break;
			} while (Console.ReadKey(true).Key != ConsoleKey.Escape);

			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine($"Finished writing data to the {Settings.ArchivalContainerName} container");

		}

	}

}