using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primes
{
	// TODO: Consider making a base generator class. It looks like the generators share functionality
	internal interface IPrimeGenerator
	{
		long PrimesFound { get; }
		long LastPrime { get; }

		// TODO: Not thread safe. call after End is called
		List<long> Primes { get; }

		void Begin();

		void End();
	}
}
