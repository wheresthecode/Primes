using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Primes
{
	internal class Program
	{
		private static string mLastConsoleOutput;
		private const int RunDuration = 60;

		private static int GetRemainingTime(int duration, Stopwatch stopwatch)
		{
			return Math.Max(0, ((duration * 1000) - (int)stopwatch.ElapsedMilliseconds));
		}

		private static void UpdateOutput(IPrimeGenerator generator, int timeRemaining)
		{
			StringBuilder builder = new StringBuilder();
			builder.AppendFormat("Remaining Time: {0}\n", timeRemaining / 1000);
			builder.AppendFormat("Primes Found: {0}\n", generator.PrimesFound);
			builder.AppendFormat("Last Prime Found: {0}", generator.LastPrime);

			string thisString = builder.ToString();
			if (mLastConsoleOutput != thisString)
			{
				Console.Clear();
				Console.WriteLine(thisString);
				mLastConsoleOutput = thisString;
			}
		}

		private static void TestGenerator(IPrimeGenerator generator, int testSize)
		{
			List<long> correctPrimes = PrimeGeneratorUtility.GetPrimes(testSize);

			generator.Begin();
			while (generator.LastPrime < testSize)
			{
				Thread.Sleep(33);
			}
			generator.End();

			Debug.Assert(correctPrimes.Count <= generator.Primes.Count);
			for (int i = 0; i < correctPrimes.Count; i++)
			{
				Debug.Assert(correctPrimes[i] == generator.Primes[i]);
			}
		}

		private static void Main(string[] args)
		{
			//TestGenerator(new PrimeGenerator(), 100 * 1024);
			//TestGenerator(new PrimeGenerator2(), 100 * 1024);

			IPrimeGenerator generator = new PrimeGeneratorWindowed(10 * 1024);
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			generator.Begin();

			int duration = 60;
			while (GetRemainingTime(duration, stopwatch) > 0)
			{

				Thread.Sleep(33);
				UpdateOutput(generator, GetRemainingTime(duration, stopwatch));
			}
			generator.End();

			UpdateOutput(generator, GetRemainingTime(duration, stopwatch));
		}
	}
}