using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BenchmarkDotNet.Attributes;

namespace CodeMania.Core.Benchmarks
{
	class Program
	{
		static void Main(string[] args)
		{
			var benchmarks = GetBenchmarkTypes();

			start:
			Console.WriteLine("Available benchmarks:");

			PrintBenchmarksMenu(benchmarks);

			Console.WriteLine("\r\nTo exit type \"quit\" or \"q\"");

			Console.WriteLine("Type benchmark number and press ENTER:");

			var readLine = Console.ReadLine();

			var input = readLine?.Trim()?.ToLowerInvariant() ?? string.Empty;

			if (input == "quit" || input == "q") return;

			if (!benchmarks.TryGetValue(input, out var benchmarkType))
			{
				goto start;
			}

			BenchmarkRunner.Run(benchmarkType);

			if (AskYesNo("Run another benchmark?"))
			{
				Console.WriteLine();
				goto start;
			}

			Console.WriteLine("Press ENTER to exit.");
			Console.ReadLine();
		}

		private static bool AskYesNo(string question)
		{
			start:

			Console.Write(question);
			Console.WriteLine(" (y/n): ");

			var input = char.ToLowerInvariant((char) Console.Read());
			if (input == 'y') return true;
			if (input == 'n') return false;

			goto start;
		}

		private static Dictionary<string, Type> GetBenchmarkTypes() =>
			Assembly.GetExecutingAssembly().GetTypes()
				.Where(t => t.IsClass && !t.IsAbstract && t.GetMethods().Any(x => x.IsDefined(typeof(BenchmarkAttribute), true)))
				.Select((t, i) => (index: i + 1, type: t))
				.ToDictionary(x => x.index.ToString(), x => x.type);

		private static void PrintBenchmarksMenu(IReadOnlyDictionary<string, Type> benchmarks)
		{
			var padding = benchmarks.Count.ToString();

			var itemFormat = "{0," + padding + "}. {1}";

			foreach (var pair in benchmarks)
			{
				Console.WriteLine(itemFormat, pair.Key, pair.Value.Name + $" ({pair.Value.Namespace})");
			}
		}
	}
}
