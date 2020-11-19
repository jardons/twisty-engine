using System;
using Twisty.Bash.Controllers;

namespace Twisty.Bash
{
	public class Program
	{
		static void Main(string[] args)
		{
			string input = null;
			while (input != "/exit")
			{
				Console.WriteLine("Choose your twisty puzzle:");
				Console.WriteLine("1) 2X2X2 Rubik's Cube");
				Console.WriteLine("2) 3X3X3 Rubik's Cube");
				Console.WriteLine("3) Skewb Cube");

				input = Console.ReadLine();
				switch (input)
				{
					case "1":
						RubikController r2 = new RubikController(2);
						r2.Start();
						break;
					case "2":
						RubikController r3 = new RubikController(3);
						r3.Start();
						break;
					case "3":
						SkewbController skewb = new SkewbController();
						skewb.Start();
						break;
				}
			}
		}
	}
}