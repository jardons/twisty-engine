using System;
using System.Collections.Generic;
using System.Text;

namespace Twisty.Engine.Presenters.Skewb
{
	/// <summary>
	/// Object proposing the colors of the differents part of a skewb face to display in the Console.
	/// </summary>
	public class SkewbFaceTextView
	{
		/// <summary>
		/// Gets the id of the color used to display the center of face in the console.
		/// </summary>
		public string CenterColor { get; set;  }

		/// <summary>
		/// Gets the id of the color used to display the top left corner of face in the console.
		/// </summary>
		public string TopLeftColor { get; set; }

		/// <summary>
		/// Gets the id of the color used to display the top right corner of face in the console.
		/// </summary>
		public string TopRightColor { get; set; }

		/// <summary>
		/// Gets the id of the color used to display the bottom left corner of face in the console.
		/// </summary>
		public string BottomLeftColor { get; set; }

		/// <summary>
		/// Gets the id of the color used to display the bottom right corner of face in the console.
		/// </summary>
		public string BottomRightColor { get; set; }
	}
}
