using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
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
		private IOperationsParser<T> m_Parser;

		protected BaseConsoleController(T rotationCore, IOperationsParser<T> parser)
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

		public void Start()
		{
			Render();

			while (true)
			{
				string line = ReadLine();
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

		protected abstract void Render();

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
			}
		}
	}
}