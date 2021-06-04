using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twisty.Engine.Algorythms
{
	public class AlgorythmsLibrary
	{
		public AlgorythmsLibrary()
		{
			Algorythms = new List<Algorythm>();
		}

		public IList<Algorythm> Algorythms { get; }
	}
}
