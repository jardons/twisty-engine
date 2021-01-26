using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Twisty.Engine.Geometry;
using Twisty.Engine.Operations;
using Twisty.Engine.Operations.Rubiks;
using Twisty.Engine.Structure;

namespace Twisty.Bash.Controllers
{
	/// <summary>
	/// Controller objet that will handle interaction between the consol and the Twisty Engine Cores.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class BaseConsoleController<T>
		where T : RotationCore
	{
		private Dictionary<string, MethodInfo> m_Routes;
		private IOperationsParser m_Parser;

		protected BaseConsoleController(T rotationCore, IOperationsParser parser)
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

		/// <summary>
		/// Gets the Rotation Core controlled by this Controller.
		/// </summary>
		public T Core { get; }

		/// <summary>
		/// Start the Execution of the Console Control process.
		/// </summary>
		/// <remarks>Method will only terminate once the user enter the '/exit' or '/quit' command.</remarks>
		public void Start()
		{
			Render();

			while (true)
			{
				string line = ReadLine();

				// Functionnal operation start with a '/', otherwise, we are on simple manipulation operation.
				if (line.StartsWith("/"))
				{
					string[] command = line.Substring(1).Split(' ', StringSplitOptions.RemoveEmptyEntries);

					if (command[0] == "exit" || command[0] == "quit")
						return;

					if (m_Routes.ContainsKey(command[0]))
					{
						MethodInfo info = m_Routes[command[0]];

						var p = info.GetParameters();
						if (p.Length > command.Length - 1)
						{
							string c = "/" + string.Join(' ', command);
							WriteError("Missing arguments", c, c.Length + 1);
							continue;
						}

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

					foreach (IOperation operation in operations)
						operation.ExecuteOn(Core);

					Render();
				}
				catch (OperationParsingException e)
				{
					WriteError("Invalid Command", e.Command, e.Index);
				}
			}
		}

		#region Abstract Members

		protected abstract void Render();

		#endregion Abstract Members

		#region Protected Members

		protected string FormatCoordinates(Plane p) => $"({FormatCoordinates(p.Normal)}, {p.D:0.00})";

		protected string FormatCoordinates(SphericalVector v) => $"({v.Phi:0.00}, {v.Theta:0.00})";

		protected string FormatCoordinates(Cartesian3dCoordinate cc) => $"({cc.X:0.00}, {cc.Y:0.00}, {cc.Z:0.00})";

		protected string FormatCoordinates(Cartesian2dCoordinate cc) => $"({cc.X:0.00}, {cc.Y:0.00})";

		#endregion Protected Members

		/// <summary>
		/// Write Error message relative to an invalid command.
		/// </summary>
		/// <param name="title">Title describing the cause of the error.</param>
		/// <param name="command">Command which caused the error.</param>
		/// <param name="errorIndex">Index of the error causes in the command.</param>
		private void WriteError(string title, string command, int errorIndex)
		{
			Console.WriteLine($"{title}: {command}");
			Console.WriteLine(string.Empty.PadLeft(title.Length + 2 + errorIndex, '-') + '^');
		}

		/// <summary>
		/// Read a line in the Console.
		/// </summary>
		/// <returns></returns>
		private string ReadLine()
		{
			StringBuilder b = new StringBuilder();

			while (true)
			{
				var key = Console.ReadKey(true);

				// Keep accepted chars.
				if (Char.IsLetterOrDigit(key.KeyChar)
					|| key.KeyChar == '\''
					|| key.KeyChar == '/'
					|| key.KeyChar == '-'
					|| key.KeyChar == ' ')
				{
					Console.Write(key.KeyChar);
					b.Append(key.KeyChar);
					continue;
				}

				// Handle backspace.
				if (key.KeyChar == '\b')
				{
					// Backspace doesn't erase previous char in console if we don't rewrite a space over it.
					Console.Write("\b \b");
					b.Remove(b.Length - 1, 1);
					continue;
				}

				// Detect the end of line.
				if (key.KeyChar == '\r')
				{
					Console.WriteLine();
					return b.ToString();
				}

				// Handle auto-complete
				if (key.KeyChar == '\t' && b[0] == '/')
				{
					string route = b.ToString(1, b.Length - 1);
					b.Clear();
					b.Append("/");
					b.Append(AutoCompleteRoute(route));

					Console.SetCursorPosition(0, Console.CursorTop);
					Console.Write(b.ToString());
				}
			}
		}

		/// <summary>
		/// Auto complete the string builder command from the availables routes.
		/// </summary>
		/// <param name="route">Route to auto complete.</param>
		/// <returns>Completed route with all common char to all possible route. A space follow the route if it has been fully completed.</returns>
		private string AutoCompleteRoute(string route)
		{
			var routes = m_Routes.Keys.Where(s => s.StartsWith(route));

			// No Routes
			if (routes.Count() == 0)
				return route;

			// If only one possibility, we can validate this route.
			if (routes.Count() == 1)
				return routes.FirstOrDefault() + " ";

			StringBuilder r = new StringBuilder(route);

			// As long as the next char is common to all routes, add it.
			routes = routes.Select(s => s.Substring(route.Length));
			while (routes.All(s => s.Length > 0 && s[0] == routes.First()[0]))
			{
				r.Append(routes.First()[0]);
				routes = routes.Select(s => s.Substring(1));
			}

			return r.ToString();
		}
	}
}