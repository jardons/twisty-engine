using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twisty.Engine.Algorithms;
using Twisty.Engine.Algorithms.Storage;
using Twisty.Runner.Models;

namespace Twisty.Runner.Services
{
	public class AlgorithmsStorageService
	{
		public AlgorithmsStorageService()
		{
		}

		public IList<AlgorithmEntry> ReadAlgorithms(string coreTypeId)
		{
			JsonAlgorithmsStore s = CreateStore();
			return s.Read(coreTypeId).Algorythms.Select(a => new AlgorithmEntry(a)).ToList();
		}

		// TODO : Save operations
		/*public void WriteAlgorithms(string coreTypeId, IEnumerable<AlgorithmEntry> algorithms)
		{
			JsonAlgorithmsStore s = CreateStore();
			s.Write(coreTypeId, lib);
		}*/

		private JsonAlgorithmsStore CreateStore()
		{
			// TODO : create settings.
			return new(@".\Algorithms");
		}
	}
}
