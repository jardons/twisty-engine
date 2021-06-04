using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twisty.Engine.Algorythms
{
	public class Algorythm
	{
		public Algorythm(string command)
		{
			this.Command = command;
		}

		public string Command { get; }
	}
}
