using System;

namespace Twisty.Bash.Controllers
{
	/// <summary>
	/// Attributes used to activate a route on a method.
	/// </summary>
	public class ConsoleRouteAttribute : Attribute
	{
		/// <summary>
		/// Create the ConsoleRouteAttribute that will link a route to the method.
		/// </summary>
		/// <param name="route">Route that will allow to call the method.</param>
		public ConsoleRouteAttribute(string route)
		{
			this.Route = route;
		}

		/// <summary>
		/// Route command for the method.
		/// </summary>
		public string Route { get; }
	}
}