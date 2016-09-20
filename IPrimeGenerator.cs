using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primes
{
	internal interface IPrimeGenerator
	{
		long PrimesFound { get; }
		long LastPrime { get; }

		List<long> Primes { get; }

		void Begin();

		void End();
	}
}
