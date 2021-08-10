using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twisty.Engine.Algorithms;

namespace Twisty.Runner.Models
{
	public class AlgorithmEntry
	{
		private Algorithm m_Algorithm;

		internal AlgorithmEntry(Algorithm a)
		{
			m_Algorithm = a;
		}

		public string Command { get => m_Algorithm.Command; }
	}
}
