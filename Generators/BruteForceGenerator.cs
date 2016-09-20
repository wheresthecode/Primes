using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Primes
{
	internal class BruteForceGenerator : IPrimeGenerator
	{
		public long PrimesFound { get { return mPrimeCount; } }
		public long LastPrime { get { return mLastPrime; } }

		public List<long> Primes { get { return mPrimes; } }

		public void Begin()
		{
			mShouldExit = false;
			mThread = new Thread(new ThreadStart(GenFunction));
			mThread.Start();
		}

		public void End()
		{
			mShouldExit = true;
			mThread.Join();
		}

		private void GenFunction()
		{
			mPrimes = new List<long>();
			mPrimes.Add(2);
			mPrimeCount = 0;
			mLastPrime = 2;
			long checkNum = 3;
			while (!mShouldExit)
			{
				bool isPrime = true;
				foreach (long prime in mPrimes)
				{
					if ((checkNum % prime) == 0)
					{
						isPrime = false;
						break;
					}
				}
				if (isPrime)
				{
					mPrimeCount = mPrimeCount + 1;
					mLastPrime = checkNum;
					mPrimes.Add(checkNum);
				}
				checkNum += 2;
			}
		}

		private long mPrimeCount;
		private long mLastPrime;
		private List<long> mPrimes = new List<long>();
		private Thread mThread;
		private volatile bool mShouldExit = false;
	}
}