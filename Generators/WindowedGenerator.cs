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

		// don't allow access to this list while the generator is running
		public List<long> Primes { get { return mPrimes; } }

		// TODO: Could Skip even numbers
		private void GenFunction()
		{
			bool[] window = new bool[mWindowsSize];
			long maximumPrimeinArraySquared = -1;
			long windowOffset = mWindowsSize;
			// Use the windowed method to calculate a starting point
			// TODO: 10 is arbitrary would be better calculate an appropriate window inside the function
			// Could also consider using a variable sized window that grows as the windowOffset increases
			mPrimes = PrimeGeneratorUtility.GetPrimesWindowed(mWindowsSize, mWindowsSize/10);
			mPrimeCount = mPrimes.Count;
            while (!mShouldExit)
			{
				int maxPrime = (int)Math.Sqrt(windowOffset + mWindowsSize)+1;
                Array.Clear(window, 0, mWindowsSize);
				foreach (long prime in mPrimes)
				{
					// don't check primes that have no chance of affecting this window
					if (prime > maxPrime)
						break;
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
						if (maximumPrimeinArraySquared == -1)
						{
							try
							{
								mPrimes.Add(mLastPrime);
							}
							catch (OutOfMemoryException exception)
							{
								// if we can add anymore, our algorithm won't work after this number squared
								// but we can keep going
								maximumPrimeinArraySquared = mLastPrime * mLastPrime;

							}
						}
						else if (mLastPrime >= maximumPrimeinArraySquared)
						{
							return;
						}
						mPrimeCount++;
					}
				}
				windowOffset += mWindowsSize;
			}
		}

		// TODO: Put in checks so you can't call begin twice before end
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

		private long mPrimeCount;
		private long mLastPrime;
		private List<long> mPrimes = new List<long>();
		private Thread mThread;
		private volatile bool mShouldExit = false;
		int mWindowsSize;
	}
}