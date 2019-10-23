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
		public class Test : IEquatable<Test>
		{
			public readonly int Value;

			public Test(int value)
			{
				Value = value;
			}

			public bool Equals(Test other)
			{
				if (ReferenceEquals(null, other))
				{
					return false;
				}

				if (ReferenceEquals(this, other))
				{
					return true;
				}

				return Value == other.Value;
			}

			public override bool Equals(object obj)
			{
				if (ReferenceEquals(null, obj))
				{
					return false;
				}

				if (ReferenceEquals(this, obj))
				{
					return true;
				}

				if (obj.GetType() != this.GetType())
				{
					return false;
				}

				return Equals((Test) obj);
			}

			public override int GetHashCode()
			{
				return Value;
			}

			public static bool operator ==(Test left, Test right)
			{
				return Equals(left, right);
			}

			public static bool operator !=(Test left, Test right)
			{
				return !Equals(left, right);
			}
		}

		static void Main(string[] args)
		{
			////var arr1 = new Test[] { new Test(1), new Test(2) };
			////var arr2 = new Test[] { new Test(1), new Test(2) };
			////ReadOnlySpan<Test> s1 = arr1;
			////ReadOnlySpan<Test> s2 = arr2;
			////Console.WriteLine(s1.SequenceEqual(s2));

			//ReadOnlySpan<byte> bytes = new byte[] {1, 2, 3, 4, 5};
			//ReadOnlySpan<int> ints = MemoryMarshal.Cast<byte, int>(bytes);

			//Console.WriteLine("Press ENTER to start");
			//Console.ReadLine();
			//var equalityComparerEqualsTests = new Benchmarks.EqualityComparer_EqualsTests();
			//equalityComparerEqualsTests.ObjectStructureComparer();
			////bool cancelled = false;
			////new Thread(() =>
			////{
			////	while (!cancelled)
			////	{
			////		equalityComparerEqualsTests.ObjectStructureComparer();
			////	}
			////}).Start();
			//Console.WriteLine("Press ENTER to exit");
			//Console.ReadLine();
			////cancelled = true;
			//return;

			var benchmarks = GetBenchmarkTypes();

			start:
			Console.WriteLine("Available benchmarks:");

			PrintBenchmarksMenu(benchmarks);

			Console.WriteLine("\r\nTo exit type \"quit\" or \"q\"");

			Console.WriteLine("Type benchmark number and press ENTER:");

			var input = ReadNonEmptyLine();

			if (input == "quit" || input == "q") return;

			if (!benchmarks.TryGetValue(input, out var benchmarkType))
			{
				goto start;
			}

			BenchmarkRunner.Run(benchmarkType);

			if (AskYesNo("\r\nRun another benchmark?"))
			{
				Console.WriteLine();
				goto start;
			}

			Console.WriteLine("Press ENTER to exit.");
			Console.ReadLine();
		}

		private static string ReadNonEmptyLine()
		{
			string input = string.Empty;
			while (string.IsNullOrEmpty(input))
			{
				input = Console.ReadLine()?.Trim()?.ToLowerInvariant() ?? string.Empty;
			}
			return input;
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
			var padding = Math.Max(benchmarks.Count.ToString().Length, 3);

			var itemFormat = "{0," + padding + "}. {1}";

			foreach (var pair in benchmarks)
			{
				Console.WriteLine(itemFormat, pair.Key, pair.Value.Name + $" ({pair.Value.Namespace})");
			}
		}
	}
}
