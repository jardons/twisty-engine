using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twisty.Engine.Algorithms
{
	public class AlgorithmsLibrary
	{
		public AlgorithmsLibrary()
		{
			Algorythms = new List<Algorithm>();
		}

		public IList<Algorithm> Algorythms { get; }
	}
}
