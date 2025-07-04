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
			this.Categories = [];
		}

		/// <summary>
		/// Algorythm command to execute.
		/// </summary>
		public string Command { get; }

		/// <summary>
		/// Gets the categories to which the elgorythm belongs.
		/// </summary>
		public List<string> Categories { get; }
	}
}
