using System.Collections.Generic;

namespace Primes
{
	class PrimeGeneratorUtility
	{
		public static List<long> GetPrimes(int n)
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
	}
}