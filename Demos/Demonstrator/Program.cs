using Azure.Cosmos;
using System;
using System.Linq;
using System.Threading.Tasks;
using TaleLearnCode.ChChChChanges.Common;
using TaleLearnCode.ChChChChanges.Demonstrator;

namespace Demonstrator
{
	class Program
	{

		static async Task Main(string[] args)
		{

			CosmosClient cosmosClient = new CosmosClient(Settings.CosmosConnectionString);

			var exit = false;
			while (!exit)
			{
				switch (ProvideDemoOptions())
				{
					case DemoOption.ArchivingData:
						await ArchivalDemo.ExecuteAsync(cosmosClient);
						break;
					case DemoOption.DenormalizingData:
						await DenormalizeDemo.ExecuteAsync(cosmosClient);
						break;
					case DemoOption.EventDrivenArchitecture:
						await EventDrivenArchitectureDemo.ExecuteAsync(cosmosClient);
						break;
					case DemoOption.Exit:
						exit = true;
						break;
					case DemoOption.RealTimeReporting:
						Console.WriteLine();
						Console.WriteLine();
						Console.WriteLine("This demo is not ready yet.  Press any key to continue...");
						Console.Beep();
						Console.ReadKey();
						break;
					case DemoOption.ReplicatingContainers:
						await DenormalizeDemo.AddPresentationsToCosmosAsync(cosmosClient);
						break;
				}
			}


		}

		private static DemoOption ProvideDemoOptions()
		{
			int returnValue = -1;

			int minDemoOptionValue = (int)Enum.GetValues(typeof(DemoOption)).Cast<DemoOption>().First();
			int maxDemoOptionValue = (int)Enum.GetValues(typeof(DemoOption)).Cast<DemoOption>().Last();
			while (returnValue < minDemoOptionValue || returnValue > maxDemoOptionValue)
			{
				Console.Clear();
				Console.WriteLine();
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine(@"_________ .__                                   ___________               .___");
				Console.WriteLine(@"\_   ___ \|  |__ _____    ____    ____   ____   \_   _____/___   ____   __| _/");
				Console.WriteLine(@"/    \  \/|  |  \\__  \  /    \  / ___\_/ __ \   |    __)/ __ \_/ __ \ / __ | ");
				Console.WriteLine(@"\     \___|   Y  \/ __ \|   |  \/ /_/  >  ___/   |     \\  ___/\  ___// /_/ | ");
				Console.WriteLine(@" \______  /___|  (____  /___|  /\___  / \___  >  \___  / \___  >\___  >____ | ");
				Console.WriteLine(@"        \/     \/     \/     \//_____/      \/       \/      \/     \/     \/ "); Console.ForegroundColor = ConsoleColor.White;
				Console.WriteLine();
				Console.WriteLine();
				Console.WriteLine("Choose the demo to run:");
				Console.WriteLine("\t [1]  Archiving Data");
				Console.WriteLine("\t [2]  Replicating Containers");
				Console.WriteLine("\t [3]  Denormalizing Data");
				Console.WriteLine("\t [4]  Event-Driven Architecture");
				Console.WriteLine("\t [5]  Real-Time Reporting");
				Console.WriteLine("\t[ESC] Exit demo");
				var keyPress = Console.ReadKey(true);
				switch (keyPress.Key)
				{
					case ConsoleKey.D1:
					case ConsoleKey.NumPad1:
						returnValue = (int)DemoOption.ArchivingData;
						break;
					case ConsoleKey.D2:
					case ConsoleKey.NumPad2:
						returnValue = (int)DemoOption.ReplicatingContainers;
						break;
					case ConsoleKey.D3:
					case ConsoleKey.NumPad3:
						returnValue = (int)DemoOption.DenormalizingData;
						break;
					case ConsoleKey.D4:
					case ConsoleKey.NumPad4:
						returnValue = (int)DemoOption.EventDrivenArchitecture;
						break;
					case ConsoleKey.D5:
					case ConsoleKey.NumPad5:
						returnValue = (int)DemoOption.RealTimeReporting;
						break;
					case ConsoleKey.Escape:
						returnValue = (int)DemoOption.Exit;
						break;
				}
			}

			return (DemoOption)returnValue;

		}

	}

}