using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CodeMania.Core;
using NUnit.Framework;

namespace CodeMania.UnitTests
{
	[TestFixture]
	[Parallelizable(ParallelScope.All)]
	public class EnumMapTests
	{
		[Flags]
		public enum Flags : int
		{
			None = 0,
			One = 1,
			Two = 2,
			Four = 4,
			Eight = 8,
			Sixteen = 16,
			All = One | Two | Four | Eight | Sixteen
		}

		public enum NonFlags : long
		{
			None = 0,
			One = 1,
			Two = 2,
			Four = 4,
			Eight = 8,
			Sixteen = 16,
			All = One | Two | Four | Eight | Sixteen
		}

		[TestCase(Flags.None, "None")]
		[TestCase(Flags.One, "One")]
		[TestCase(Flags.Two, "Two")]
		[TestCase(Flags.Four, "Four")]
		[TestCase(Flags.Eight, "Eight")]
		[TestCase(Flags.Sixteen, "Sixteen")]
		[TestCase(Flags.All, "All")]
		[TestCase(Flags.Eight | Flags.Four, "Four, Eight")]
		[TestCase(Flags.One | Flags.Four, "One, Four")]
		[TestCase(Flags.Two| Flags.Four, "Two, Four")]
		[TestCase(Flags.Two | Flags.Eight | Flags.Four, "Two, Four, Eight")]
		[TestCase(Flags.Two | Flags.Eight | Flags.Four | Flags.None, "Two, Four, Eight")]
		[TestCase(Flags.All | Flags.Eight | Flags.Four | Flags.None, "All")]
		[TestCase(Flags.One | Flags.Two | Flags.Eight | Flags.Four | Flags.Sixteen, "All")]
		public void Flags_ToString(Flags flags, string expected)
		{
			Assert.AreEqual(expected, EnumMap<Flags>.Instance.ToString(flags));
		}

		[TestCase("None", Flags.None)]
		[TestCase("One", Flags.One)]
		[TestCase("Two", Flags.Two)]
		[TestCase("Four", Flags.Four)]
		[TestCase("Eight", Flags.Eight)]
		[TestCase("Sixteen", Flags.Sixteen)]
		[TestCase("All", Flags.All)]
		[TestCase("Four, Eight", Flags.Eight | Flags.Four)]
		[TestCase("Two, Eight", Flags.Two| Flags.Eight)]
		[TestCase("Two, Four", Flags.Two | Flags.Four)]
		[TestCase("None", Flags.None)]
		[TestCase("Two, Four", Flags.Two | Flags.Four | Flags.None)]
		[TestCase("Two,                            Four", Flags.Two | Flags.Four | Flags.None)]
		[TestCase("Two, Four, Eight", Flags.Two | Flags.Four | Flags.Eight)]
		[TestCase("Two,		Four,					Eight", Flags.Two | Flags.Four | Flags.Eight)]
		[TestCase("One, Two, Four, Eight", Flags.One | Flags.Two | Flags.Four | Flags.Eight)]
		[TestCase(" None", Flags.None)]
		[TestCase("  One", Flags.One)]
		[TestCase("   Two", Flags.Two)]
		[TestCase("		Four", Flags.Four)]
		[TestCase("Eight ", Flags.Eight)]
		[TestCase("Sixteen  ", Flags.Sixteen)]
		[TestCase(" All ", Flags.All)]
		[TestCase(" Four,   Eight   ", Flags.Eight | Flags.Four)]
		[TestCase("    Two    , Eight       ", Flags.Two | Flags.Eight)]
		[TestCase("		Two,    Four   ", Flags.Two | Flags.Four)]
		[TestCase("			None		 ", Flags.None)]
		[TestCase("		Two		,		   Four ", Flags.Two | Flags.Four | Flags.None)]
		[TestCase(" Two,                            Four", Flags.Two | Flags.Four | Flags.None)]
		[TestCase("  Two	, Four	, Eight	", Flags.Two | Flags.Four | Flags.Eight)]
		[TestCase("		Two,		Four,					Eight", Flags.Two | Flags.Four | Flags.Eight)]
		[TestCase("		One,		Two,	Four,	Eight", Flags.One | Flags.Two | Flags.Four | Flags.Eight)]
		public void Flags_Parse(string value, Flags expected)
		{
			var actual = EnumMap<Flags>.Instance.Parse(value);

			Assert.IsTrue(EqualityComparer<Flags>.Default.Equals(expected, actual));
		}

		[TestCase(true, "None", Flags.None)]
		[TestCase(true, "One", Flags.One)]
		[TestCase(true, "Two", Flags.Two)]
		[TestCase(true, "Four", Flags.Four)]
		[TestCase(true, "Eight", Flags.Eight)]
		[TestCase(true, "Sixteen", Flags.Sixteen)]
		[TestCase(true, "All", Flags.All)]
		[TestCase(true, "Four, Eight", Flags.Eight | Flags.Four)]
		[TestCase(true, "Two, Eight", Flags.Two | Flags.Eight)]
		[TestCase(true, "Two, Four", Flags.Two | Flags.Four)]
		[TestCase(true, "None", Flags.None)]
		[TestCase(true, "Two, Four", Flags.Two | Flags.Four | Flags.None)]
		[TestCase(true, "Two, Four, Eight", Flags.Two | Flags.Four | Flags.Eight)]
		[TestCase(true, "One, Two, Four, Eight", Flags.One | Flags.Two | Flags.Four | Flags.Eight)]
		[TestCase(false, "NoneS", Flags.None)]
		[TestCase(false, "OneS", Flags.One)]
		[TestCase(false, "Twos", Flags.Two)]
		[TestCase(false, "Fours", Flags.Four)]
		[TestCase(false, "Eights", Flags.Eight)]
		[TestCase(false, "Sixteens", Flags.Sixteen)]
		[TestCase(false, "+All", Flags.All)]
		[TestCase(false, "Four, Eight+", Flags.Eight | Flags.Four)]
		[TestCase(false, "Two_, _Eight", Flags.Two | Flags.Eight)]
		[TestCase(false, "Two, Four_", Flags.Two | Flags.Four)]
		[TestCase(false, "None1", Flags.None)]
		[TestCase(false, "Two, Four, Fake", Flags.Two | Flags.Four | Flags.None)]
		[TestCase(false, "Two, Four, Eight, Two_", Flags.Two | Flags.Four | Flags.Eight)]
		[TestCase(false, "One, Two, Four, Eight, One-", Flags.One | Flags.Two | Flags.Four | Flags.Eight)]
		public void Flags_TryParse(bool successful, string value, Flags expected)
		{
			var isParsed = EnumMap<Flags>.Instance.TryParse(value, out var enumResult);

			Assert.AreEqual(successful, isParsed);

			if (isParsed)
				Assert.IsTrue(EqualityComparer<Flags>.Default.Equals(expected, enumResult));
		}

		[TestCase(true, "none", Flags.None)]
		[TestCase(true, "ONE", Flags.One)]
		[TestCase(true, "TwO", Flags.Two)]
		[TestCase(true, "FouR", Flags.Four)]
		[TestCase(true, "eight", Flags.Eight)]
		[TestCase(true, "SIXTEEN", Flags.Sixteen)]
		[TestCase(true, "All", Flags.All)]
		[TestCase(true, "Four, EIGHT", Flags.Eight | Flags.Four)]
		[TestCase(true, "two, Eight", Flags.Two | Flags.Eight)]
		[TestCase(true, "TWO, four", Flags.Two | Flags.Four)]
		[TestCase(true, "NOne", Flags.None)]
		[TestCase(true, "two, four", Flags.Two | Flags.Four | Flags.None)]
		[TestCase(true, "TWO, FOUR, eight", Flags.Two | Flags.Four | Flags.Eight)]
		[TestCase(true, "one, two, four, eight", Flags.One | Flags.Two | Flags.Four | Flags.Eight)]
		[TestCase(false, "noneS", Flags.None)]
		[TestCase(false, "oneS", Flags.One)]
		[TestCase(false, "TWOS", Flags.Two)]
		[TestCase(false, "FourS", Flags.Four)]
		[TestCase(false, "eights", Flags.Eight)]
		[TestCase(false, "sixteens", Flags.Sixteen)]
		[TestCase(false, "+All", Flags.All)]
		[TestCase(false, "four, eight+", Flags.Eight | Flags.Four)]
		[TestCase(false, "two_, _Eight", Flags.Two | Flags.Eight)]
		[TestCase(false, "Two, Four_", Flags.Two | Flags.Four)]
		[TestCase(false, "None1", Flags.None)]
		[TestCase(false, "Two, Four, Fake", Flags.Two | Flags.Four | Flags.None)]
		[TestCase(false, "Two, Four, Eight, Two_", Flags.Two | Flags.Four | Flags.Eight)]
		[TestCase(false, "One, Two, Four, Eight, One-", Flags.One | Flags.Two | Flags.Four | Flags.Eight)]
		[TestCase(false, "1four, eight+             , four", Flags.Eight | Flags.Four)]
		public void Flags_TryParse_IgnoreCase(bool successful, string value, Flags expected)
		{
			var isParsed = EnumMap<Flags>.Instance.TryParse(value, out var enumResult, ignoreCase: true);

			Assert.AreEqual(successful, isParsed);

			if (isParsed)
				Assert.IsTrue(EqualityComparer<Flags>.Default.Equals(expected, enumResult));
		}

		[TestCase("none", Flags.None)]
		[TestCase("ONE", Flags.One)]
		[TestCase("tWo", Flags.Two)]
		[TestCase("FOUR", Flags.Four)]
		[TestCase("eighT", Flags.Eight)]
		[TestCase("sixteen", Flags.Sixteen)]
		[TestCase("ALL", Flags.All)]
		[TestCase("FOUR, eight", Flags.Eight | Flags.Four)]
		[TestCase("two, EIGHT", Flags.Two | Flags.Eight)]
		[TestCase("two, four", Flags.Two | Flags.Four)]
		[TestCase("none", Flags.None)]
		[TestCase("TWO, FOUR", Flags.Two | Flags.Four | Flags.None)]
		[TestCase("TWO, FOUR, Eight", Flags.Two | Flags.Four | Flags.Eight)]
		[TestCase("ONE, two, four, EIGHT", Flags.One | Flags.Two | Flags.Four | Flags.Eight)]
		public void Flags_Parse_IgnoreCase(string value, Flags expected)
		{
			var actual = EnumMap<Flags>.Instance.Parse(value, ignoreCase: true);

			Assert.IsTrue(EqualityComparer<Flags>.Default.Equals(expected, actual));
		}

		[TestCase(NonFlags.None, "None")]
		[TestCase(NonFlags.One, "One")]
		[TestCase(NonFlags.Two, "Two")]
		[TestCase(NonFlags.Four, "Four")]
		[TestCase(NonFlags.Eight, "Eight")]
		[TestCase(NonFlags.Sixteen, "Sixteen")]
		[TestCase(NonFlags.All, "All")]
		public void NonNonFlags_ToString(NonFlags flags, string expected)
		{
			Assert.AreEqual(expected, EnumMap<NonFlags>.Instance.ToString(flags));
		}

		[TestCase("None", NonFlags.None)]
		[TestCase("One", NonFlags.One)]
		[TestCase("Two", NonFlags.Two)]
		[TestCase("Four", NonFlags.Four)]
		[TestCase("Eight", NonFlags.Eight)]
		[TestCase("Sixteen", NonFlags.Sixteen)]
		[TestCase("All", NonFlags.All)]
		public void NonFlags_Parse(string value, NonFlags expected)
		{
			var actual = EnumMap<NonFlags>.Instance.Parse(value);

			Assert.IsTrue(EqualityComparer<NonFlags>.Default.Equals(expected, actual));
		}

		[TestCase("nOnE", NonFlags.None)]
		[TestCase("ONE", NonFlags.One)]
		[TestCase("two", NonFlags.Two)]
		[TestCase("FouR", NonFlags.Four)]
		[TestCase("four", NonFlags.Four)]
		[TestCase("FOUR", NonFlags.Four)]
		[TestCase("eight", NonFlags.Eight)]
		[TestCase("EIGHT", NonFlags.Eight)]
		[TestCase("sixteen", NonFlags.Sixteen)]
		[TestCase("SIXTEEN", NonFlags.Sixteen)]
		[TestCase("Sixteen", NonFlags.Sixteen)]
		[TestCase("ALL", NonFlags.All)]
		[TestCase("all", NonFlags.All)]
		[TestCase("All", NonFlags.All)]
		[TestCase("AlL", NonFlags.All)]
		public void NonFlags_Parse_IgnoreCase(string value, NonFlags expected)
		{
			var actual = EnumMap<NonFlags>.Instance.Parse(value, ignoreCase: true);

			Assert.IsTrue(EqualityComparer<NonFlags>.Default.Equals(expected, actual));
		}

		[Test]
		public void MultiThread_Test_Check()
		{
			var exceptMethod = nameof(MultiThread_Test_Check);

			// get public test methods and their test cases
			var methods = this.GetType().GetMethods()
				.Where(methodInfo => methodInfo.Name != exceptMethod)
				.Select(methodInfo => (methodInfo, methodInfo.GetCustomAttributes<TestCaseAttribute>().ToList()))
				.ToList();

			var resetEvent = new ManualResetEventSlim(false);
			ConcurrentDictionary<string, int> callStatistics = new ConcurrentDictionary<string, int>();

			// run test about 10000 milliseconds
			var cancellationTokenSource = new CancellationTokenSource(10000);

			try
			{
				foreach (var tuple in methods)
				{
					Task.Factory.StartNew(() =>
					{
						while (!cancellationTokenSource.IsCancellationRequested)
						{
							foreach (var testCase in tuple.Item2)
							{
								ThreadPool.QueueUserWorkItem(obj =>
								{
									var callInfo = ((MethodInfo method, TestCaseAttribute testCase)) obj;

									callInfo.method.Invoke(this, callInfo.testCase.Arguments);

									callStatistics.AddOrUpdate(
										callInfo.method.ToString(),
										_ => 0,
										(_, old) => Interlocked.Increment(ref old));
								}, (tuple.Item1, testCase));
							}
						}
					},
					// force create new thread:
					TaskCreationOptions.LongRunning);
				}

				resetEvent.Wait(cancellationTokenSource.Token);
			}
			catch (OperationCanceledException)
			{
				// ignored
			}

			Console.WriteLine("Test call statistics:");
			Console.WriteLine($"{"Count",12}\tMethod");
			foreach (var pair in callStatistics)
			{
				Console.WriteLine($"{pair.Value,12}\t{pair.Key}");
			}
		}
	}
}