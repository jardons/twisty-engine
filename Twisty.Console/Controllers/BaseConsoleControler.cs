using System;
using System.Collections.Generic;
using System.Reflection;
using Twisty.Engine.Geometry;
using Twisty.Engine.Operations;
using Twisty.Engine.Operations.Rubiks;
using Twisty.Engine.Structure;

namespace Twisty.Bash.Controllers
{
	public abstract class BaseConsoleControler<T>
		where T : RotationCore
	{
		private Dictionary<string, MethodInfo> m_Routes;
		private IOperationsParser<T> m_Parser;

		protected BaseConsoleControler(T rotationCore, IOperationsParser<T> parser)
		{
			m_Parser = parser;
			m_Routes = new Dictionary<string, MethodInfo>();
			Core = rotationCore;

			foreach (var m in this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
			{
				foreach (var attr in m.GetCustomAttributes<ConsoleRouteAttribute>())
				{
					m_Routes.Add(attr.Route, m);
				}
			}
		}

		public T Core { get; }

		public void Start()
		{
			Render();

			while (true)
			{
				string line = Console.ReadLine().Trim();
				if (line.StartsWith("/"))
				{
					string[] command = line.Substring(1).Split(' ', StringSplitOptions.RemoveEmptyEntries);

					if (command[0] == "/exit" || command[0] == "/quit")
						return;

					if (m_Routes.ContainsKey(command[0]))
					{
						MethodInfo info = m_Routes[command[0]];

						var p = info.GetParameters();

						object[] args = new object[p.Length];
						for (int i = 0; i < p.Length; ++i)
							args[i] = command[i + 1];
						info.Invoke(this, args);
						continue;
					}
				}

				try
				{
					var operations = m_Parser.Parse(line);

					foreach (IOperation<T> operation in operations)
						operation.ExecuteOn(Core);

					Render();
				}
				catch (OperationParsingException e)
				{
					Console.WriteLine("Invalid Command: " + e.Command);
					Console.WriteLine(string.Empty.PadLeft(17 + e.Index, '-') + '^');
				}
			}
		}

		#region Abstract Members

		protected abstract void Render();

		#endregion Abstract Members

		#region Protected Members

		protected string FormatCoordinates(SphericalVector v) => $"({v.Phi:0.00}, {v.Theta:0.00})";

		protected string FormatCoordinates(Cartesian3dCoordinate cc) => $"({cc.X:0.00}, {cc.Y:0.00}, {cc.Z:0.00})";

		protected string FormatCoordinates(Cartesian2dCoordinate cc) => $"({cc.X:0.00}, {cc.Y:0.00})";

		#endregion Protected Members
	}
}
