using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Primes
{
	class PrimeGeneratorUtility
	{
		public static List<long> GetPrimesSimple(int n)
		{
			List<long> list = new List<long>();
			list.Add(2);
			long checkNum = 3;
			while (checkNum < n)
			{
				bool isPrime = true;
				foreach (long prime in list)
				{
					if ((checkNum % prime) == 0)
					{
						isPrime = false;
						break;
					}
				}
				if (isPrime)
				{
					list.Add(checkNum);
				}
				checkNum += 2;
			}
			return list;
		}

		public static List<long> GetPrimesWindowed(int n, int windowSize)
		{
			int windowOffset = windowSize;
			// TODO: Creating the first window is kind of an edge case so I'm using the simple generator for it.
			// It'd be better to handle the edge case
			List<long> primes = GetPrimesSimple(windowSize);
            bool []window = new bool[windowSize];
			while (windowOffset < n)
			{
				Array.Clear(window, 0, windowSize);
				foreach (long prime in primes)
				{
					long startIdx = (windowOffset / prime) * prime;
					if (startIdx < windowOffset)
					{
						startIdx += prime;
					}
					startIdx -= windowOffset;
					Debug.Assert(startIdx >= 0);
					while (startIdx < windowSize)
					{
						window[startIdx] = true;
						startIdx += prime;
					}
				}

				int count = Math.Min( n - windowOffset, windowSize);
				for (int i = 0; i < windowSize; i++)
				{
					if (!window[i])
					{
						primes.Add(windowOffset + i);
					}
				}
				windowOffset += windowSize;
			}
			
			return primes;
		}
	}
}