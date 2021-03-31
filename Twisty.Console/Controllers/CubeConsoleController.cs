using System;
using System.Collections.Generic;
using System.Text;
using Twisty.Engine.Operations;
using Twisty.Engine.Structure;

namespace Twisty.Bash.Controllers
{
	/// <summary>
	/// Controller class providing general methods common to various cube controllers.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class CubeConsoleController<T> : BaseConsoleController<T>
		where T : CubicRotationCore
	{
		protected CubeConsoleController(T rotationCore, IOperationsParser parser)
			: base(rotationCore, parser)
		{
		}

		/// <summary>
		/// Sets the Color used to print in the console.
		/// </summary>
		/// <param name="id">Id of the color that will be used to print in the console.</param>
		protected void SetConsolColor(string id)
		{
			switch (id)
			{
				case CubicRotationCore.ID_FACE_UP:
					Console.ForegroundColor = ConsoleColor.Yellow;
					break;
				case CubicRotationCore.ID_FACE_LEFT:
					Console.ForegroundColor = ConsoleColor.DarkYellow;
					break;
				case CubicRotationCore.ID_FACE_RIGHT:
					Console.ForegroundColor = ConsoleColor.Red;
					break;
				case CubicRotationCore.ID_FACE_FRONT:
					Console.ForegroundColor = ConsoleColor.Blue;
					break;
				case CubicRotationCore.ID_FACE_BACK:
					Console.ForegroundColor = ConsoleColor.Green;
					break;
				case CubicRotationCore.ID_FACE_DOWN:
				default:
					Console.ForegroundColor = ConsoleColor.White;
					break;
			}
		}
	}
}
