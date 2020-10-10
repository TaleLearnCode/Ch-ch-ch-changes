using System;
using System.Threading.Tasks;

namespace TaleLearnCode.ChChChChanges.EventDrivenArchitecture
{
	class Program
	{
		static async Task Main(string[] args)
		{
			WelcomeUser();
			var orderSimulator = new OrderSimulator();
			Console.WriteLine("Press any key to start the simulator...");
			Console.ReadKey();
			await orderSimulator.SimulateOrdersAsync();
		}

		private static void WelcomeUser()
		{
			Console.Clear();
			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(@"___________________      _____   ");
			Console.WriteLine(@"\_   _____/\______ \    /  _  \  ");
			Console.WriteLine(@" |    __)_  |    |  \  /  /_\  \ ");
			Console.WriteLine(@" |        \ |    `   \/    |    \");
			Console.WriteLine(@"/_______  //_______  /\____|__  /");
			Console.WriteLine(@"        \/         \/         \/ ");
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine();
		}

	}

}