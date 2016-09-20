using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace Primes
{
	class PrimeGenerator
	{
		public long PrimesFound { get { return mPrimeCount; } }
		public long MostRecentyPrime { get { return mLastPrime; } }

		public List<int> GetPrimeList() { return new List<int>(); }

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

		private void GenFunction()
		{
			mPrimes = new List<long>();
			mPrimes.Add(2);
			mPrimeCount = 0;
			mLastPrime = 2;
			long checkNum = 3;
			while(!mShouldExit)
			{
				bool isPrime = true;
				foreach (long prime in mPrimes )
				{
					if( (checkNum % prime) == 0 )
					{
						isPrime = false;
						break;
					}
				}
				if( isPrime )
				{
					mPrimeCount = mPrimeCount + 1;
					mLastPrime = checkNum;
                    mPrimes.Add(checkNum);
					mPrimeCount = 0;
                }
				checkNum+=2;
			}
		}

		private long mPrimeCount;
		private long mLastPrime;
		private List<long> mPrimes = new List<long>();
		private Thread mThread;
		private volatile bool mShouldExit = false;
	}

	class Program
	{
		static string mLastConsoleOutput;
		static Stopwatch mStopwatch;
		static int mRunDuration = 60;
		static PrimeGenerator mGenerator = new PrimeGenerator();

		static int GetRemainingTime()
		{
			return Math.Max(0, ((mRunDuration*1000) - (int)mStopwatch.ElapsedMilliseconds) );
		}

		static void UpdateOutput()
		{
			
			StringBuilder builder = new StringBuilder();
			builder.AppendFormat("Remaining Time: {0}\n", GetRemainingTime()/1000);
			builder.AppendFormat("Primes Found: {0}\n", mGenerator.PrimesFound);
			builder.AppendFormat("Last Prime Found: {0}", mGenerator.MostRecentyPrime);

			string thisString = builder.ToString();
            if (mLastConsoleOutput != thisString)
			{
				Console.Clear();
				Console.WriteLine(thisString);
				mLastConsoleOutput = thisString;
			}
		}

		static void Main(string[] args)
		{
			mGenerator = new PrimeGenerator();
			mStopwatch = new Stopwatch();
			mStopwatch.Start();
			mGenerator.Begin();

			while (GetRemainingTime() > 0)
			{
				Thread.Sleep(33);
				UpdateOutput();
			}
			mGenerator.End();

			UpdateOutput();
		}
	}
}
