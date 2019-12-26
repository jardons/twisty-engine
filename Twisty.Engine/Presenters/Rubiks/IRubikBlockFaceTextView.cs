using System;
using System.Collections.Generic;
using System.Text;

namespace Twisty.Engine.Presenters.Rubiks
{
	public interface IRubikBlockFaceTextView
	{
		/// <summary>
		/// Get shte specified line to display in the console.
		/// </summary>
		/// <param name="i">Index of the line to display.</param>
		/// <returns>Line to display for the requested index of the view.</returns>
		string GetLine(int i);

		/// <summary>
		/// Gets the id of the color used to display this face in the console.
		/// </summary>
		string Color { get; }

		/// <summary>
		/// Gets the collection of lines to display in the console for this Block face.
		/// </summary>
		IEnumerable<string> Lines { get; }
	}
}