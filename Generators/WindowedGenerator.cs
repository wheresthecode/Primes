using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Primes
{
	internal class PrimeGeneratorWindowed : IPrimeGenerator
	{
		public PrimeGeneratorWindowed(int windowSize)
		{
			mWindowsSize = windowSize;
		}

		public long LastPrime { get { return mLastPrime; } }

		public long PrimesFound { get { return mPrimeCount; } }

		public List<long> Primes { get { return mPrimes; } }

		private void GenFunction()
		{
			bool[] window = new bool[mWindowsSize];
			long windowOffset = mWindowsSize;
			mPrimes = PrimeGeneratorUtility.GetPrimes(mWindowsSize);
			while (!mShouldExit)
			{
				Array.Clear(window, 0, mWindowsSize);
				foreach (long prime in mPrimes)
				{
					long startIdx = (windowOffset / prime) * prime;
					if (startIdx < windowOffset)
					{
						startIdx += prime;
					}
					startIdx -= windowOffset;
					Debug.Assert(startIdx >= 0);
					while (startIdx < mWindowsSize)
					{
						window[startIdx] = true;
						startIdx += prime;
					}
				}

				for (int i = 0; i < mWindowsSize; i++)
				{
					if (!window[i])
					{
						mLastPrime = windowOffset + i;
						mPrimes.Add(mLastPrime);
						mPrimeCount++;
					}
				}
				windowOffset += mWindowsSize;
			}
		}

		public void Begin()
		{
			mThread = new Thread(new ThreadStart(GenFunction));
			mThread.Start();
		}

		public void End()
		{
			mShouldExit = true;
			mThread.Join();
		}

		private long mPrimeCount;
		private long mLastPrime;
		private List<long> mPrimes = new List<long>();
		private Thread mThread;
		private volatile bool mShouldExit = false;
		int mWindowsSize;
	}
}