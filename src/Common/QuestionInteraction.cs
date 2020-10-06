using ShellProgressBar;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TaleLearnCode.ChChChChanges.Common
{
	public class QuestionInteraction
	{

		[JsonPropertyName("id")]
		public string Id { get; set; } = Guid.NewGuid().ToString();

		[JsonPropertyName("interactionDateTime")]
		public DateTime InteractionDateTime { get; set; }

		[JsonPropertyName("school")]
		public string School { get; set; }

		[JsonPropertyName("writeDateTime")]
		public string WriteDateTime { get; set; }

		public QuestionInteraction() { }

		public QuestionInteraction(string csvInput)
		{
			String[] input = csvInput.Split(',');
			InteractionDateTime = Convert.ToDateTime(input[0]);
			School = input[1];
		}

		public static List<QuestionInteraction> GetListOfInteractions(string sourceFilePath)
		{
			var returnValue = new List<QuestionInteraction>();


			int counter = 0;
			string line;

			using var progressBar = new ProgressBar(428818, ProgressBarMessage(counter));

			System.IO.StreamReader file = new System.IO.StreamReader(sourceFilePath);
			while ((line = file.ReadLine()) != null)
			{
				returnValue.Add(new QuestionInteraction(line));
				counter++;
				progressBar.Tick(ProgressBarMessage(counter));
			}
			file.Close();
			Console.WriteLine("There were {0} interactions imported.", counter);

			return returnValue;

		}

		private static string ProgressBarMessage(int counter)
		{
			return $"Importing {counter:n0} interaction of 428,818";
		}

	}

}