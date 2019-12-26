using System;
using Twisty.Bash.Controllers;
using Twisty.Engine.Geometry;

namespace Twisty.Bash
{
	public class Program
	{
		static void Main(string[] args)
		{
			RubikController c = new RubikController();

			c.Start();
			
			Console.ReadLine();
		}
	}
}