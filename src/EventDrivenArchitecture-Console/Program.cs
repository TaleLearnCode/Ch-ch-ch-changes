using Microsoft.VisualBasic.FileIO;
using ShellProgressBar;
using System;
using System.Collections.Generic;
using TaleLearnCode.ChChChChanges.Common;

namespace TaleLearnCode.ChChChChanges.EventDrivenArchitecture
{
	class Program
	{
		static void Main(string[] args)
		{
			WelcomeUser();
			GetFakeNames();
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
		}

		private static List<FakeName> GetFakeNames()
		{
			var fakeNames = new List<FakeName>();

			using var progressBar = new ProgressBar(100000, "Getting the fake names to work with...");

			using var parser = new TextFieldParser($@"C:\Users\chadg\Downloads\FakeNameGenerator.com_100eedb1\FakeNames.csv");
			parser.TextFieldType = FieldType.Delimited;
			parser.SetDelimiters(",");
			while (!parser.EndOfData)
			{
				string[] fields = parser.ReadFields();
				fakeNames.Add(new FakeName()
				{
					FirstName = fields[FakeNameFields.GivenName],
					LastName = fields[FakeNameFields.Surname],
					StreetAddress = fields[FakeNameFields.StreetAddress],
					City = fields[FakeNameFields.City],
					State = fields[FakeNameFields.State],
					PostalCode = fields[FakeNameFields.ZipCode],
					Country = fields[FakeNameFields.Country],
					EmailAddress = fields[FakeNameFields.EmailAddress],
					UserName = fields[FakeNameFields.UserName]
				});
				progressBar.Tick();
			}

			return fakeNames;
		}

		private static List<Cart> GetProducts()
		{
			var products = new List<Cart>();


			return products;
		}


	}






}
