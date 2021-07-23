using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twisty.Engine.Algorithms
{
	public class Algorithm
	{
		public Algorithm(string command)
		{
			this.Command = command;
		}

		public string Command { get; }
	}
}
