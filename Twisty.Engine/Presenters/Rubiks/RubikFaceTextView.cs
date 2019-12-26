using System;
using System.Collections.Generic;
using System.Text;

namespace Twisty.Engine.Presenters.Rubiks
{
	/// <summary>
	/// Class providing all the information needed to display a Rubiks' cube face in the Console.
	/// </summary>
	public class RubikFaceTextView
	{
		private IRubikBlockFaceTextView[,] m_BlocksFaces;

		/// <summary>
		/// Create a new RubikFaceTextView for a Rubiks' cube of size N.
		/// </summary>
		/// <param name="n">Size of the cube that will be represented when displaying the content of the RubikFaceTextView.</param>
		public RubikFaceTextView(int n)
		{
			this.N = n;
			m_BlocksFaces = new IRubikBlockFaceTextView[n, n];
		}

		/// <summary>
		/// Get the IRubikBlockFaceTextView for a specified position in the Rubiks' cube face.
		/// </summary>
		/// <param name="x">X coordinate of the seeked block.</param>
		/// <param name="y">Y coordinate of the seeked block.</param>
		/// <returns>Block at the specified coordinates.</returns>
		public IRubikBlockFaceTextView this[int x, int y]
		{
			get { return m_BlocksFaces[x, y]; }
			internal set { m_BlocksFaces[x, y] = value; }
		}

		/// <summary>
		/// Gets the displayed Rubiks' cube size.
		/// </summary>
		public int N { get; }
	}
}
