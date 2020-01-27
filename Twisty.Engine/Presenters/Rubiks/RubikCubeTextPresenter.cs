using System;
using System.Collections.Generic;
using System.Linq;
using Twisty.Engine.Geometry;
using Twisty.Engine.Structure;
using Twisty.Engine.Structure.Rubiks;

namespace Twisty.Engine.Presenters.Rubiks
{
	/// <summary>
	/// Class providing presentation operations of the Rubiks' cube in the Console.
	/// </summary>
	public class RubikCubeTextPresenter
	{
		#region Inner Classes

		/// <summary>
		/// Class describing a Rubiks' cube face.
		/// </summary>
		private class RubikCubeBlockFaceTextView : IRubikBlockFaceTextView
		{
			#region Private Members

			private List<string> m_Lines;
			private int m_Width;

			#endregion Private Members

			/// <summary>
			/// Craete a new RubikCubeBlockFaceTextView.
			/// </summary>
			/// <param name="width">Width of the block face in char count.</param>
			/// <param name="color">Color of the space represented by this object.</param>
			public RubikCubeBlockFaceTextView(int width, string color)
			{
				m_Lines = new List<string>(width / 2);
				m_Width = width;
				this.Color = color;
			}

			/// <summary>
			/// Append a line to the RubikCubeBlockFaceTextView during his creation process.
			/// </summary>
			/// <param name="line">Line to add to the RubikCubeBlockFaceTextView object.</param>
			public void Append(string line)
			{
				if (line == null)
					throw new ArgumentNullException(nameof(line));

				if (line.Length != m_Width)
					throw new ArgumentException("Length of the line does not matche the face size.", nameof(line));

				if (m_Lines.Count >= m_Width / 2)
					throw new InvalidOperationException("Maximal number of line has been reached.");

				m_Lines.Add(line);
			}

			#region IFaceTextView Members

			/// <summary>
			/// Gets the id of the color used to display this face in the console.
			/// </summary>
			public string Color { get; }

			/// <summary>
			/// Get shte specified line to display in the console.
			/// </summary>
			/// <param name="i">Index of the line to display.</param>
			/// <returns>Line to display for the requested index of the view.</returns>
			public string GetLine(int i) => m_Lines[i];

			/// <summary>
			/// Gets the collection of lines to display in the console for this Block face.
			/// </summary>
			public IEnumerable<string> Lines => m_Lines;

			#endregion IFaceTextView Members
		}

		/// <summary>
		/// Class describing a Place Holder used to fill the space when a Rubiks' cube face doesn't exist.
		/// </summary>
		private class RubikCubeFacePlaceHolder : IRubikBlockFaceTextView
		{
			private readonly string m_PlaceHolder;

			/// <summary>
			/// Craete a new RubikCubeFacePlaceHolder.
			/// </summary>
			/// <param name="width">Width of the block face in char count.</param>
			public RubikCubeFacePlaceHolder(int width)
			{
				m_PlaceHolder = string.Empty.PadRight(width);
			}

			#region IFaceTextView Members

			/// <summary>
			/// Gets the id of the color used to display this face in the console.
			/// </summary>
			public string Color => null;

			/// <summary>
			/// Get shte specified line to display in the console.
			/// </summary>
			/// <param name="i">Index of the line to display.</param>
			/// <returns>Line to display for the requested index of the view.</returns>
			public string GetLine(int i) => m_PlaceHolder;

			/// <summary>
			/// Gets the collection of lines to display in the console for this Block face.
			/// </summary>
			public IEnumerable<string> Lines
			{
				get
				{
					for (int i = 0; i < m_PlaceHolder.Length / 2; ++i)
						yield return m_PlaceHolder;
				}
			}

			#endregion IFaceTextView Members
		}

		#endregion Inner Classes

		/// <summary>
		/// Create a new RubikCubeTextPresenter that will present the provided cube to the Console.
		/// </summary>
		/// <param name="cube">Cube that will be displayed in the Console.</param>
		public RubikCubeTextPresenter(RubikCube cube)
		{
			this.Cube = cube;
		}

		/// <summary>
		/// Gets the Cube that will be presented by this instance.
		/// </summary>
		public RubikCube Cube { get; }

		/// <summary>
		/// Gets the number of lines displayed for each block face.
		/// </summary>
		public int LinesPerBlockFace => this.RowsPerBlockFace / 2;

		/// <summary>
		/// Gets the number of rows displayed for each block face.
		/// </summary>
		public int RowsPerBlockFace => 4;

		/// <summary>
		/// Gets the information allowing to display a rubiks' cube face on the console.
		/// </summary>
		/// <param name="faceId">Id of the Rubiks' face that will be displayed.</param>
		/// <returns>A RubikFaceTextView object containing the lines to display in the console for the requested face.</returns>
		public RubikFaceTextView GetFaceAsText(string faceId)
		{
			var axis = this.Cube.GetAxis(faceId);
			var blocks = this.Cube.GetBlocksForFace(axis.Vector);

			RubikFaceTextView faces = new RubikFaceTextView(this.Cube.N);
			int i = 0;
			int j = 0;

			var first = blocks.FirstOrDefault().Position;
			Plane p = new Plane(axis.Vector, first);

			var sortedBlocks = blocks.OfType<IPositionnedByCartesian3dVector>().ToList();
			sortedBlocks.Sort(new PlanePositionPointComparer(p));

			foreach (var block in sortedBlocks.OfType<Block>())
			{
				var face = block.GetBlockFace(axis.Vector);

				RubikCubeBlockFaceTextView result = new RubikCubeBlockFaceTextView(RowsPerBlockFace, face.Id);
				result.Append("****");
				result.Append("****");

				faces[j, i++] = result;
				if (i >= this.Cube.N)
				{
					i = 0;
					++j;
				}
			}

			return faces;
		}

		/// <summary>
		/// Gets the PlaceHolder for a single face.
		/// </summary>
		/// <returns>A RubikFaceTextView object used to replace a Rubiks' cube face when their is no face to display in a grid.</returns>
		public RubikFaceTextView GetFacePlaceHolder()
		{
			RubikFaceTextView faces = new RubikFaceTextView(this.Cube.N);
			RubikCubeFacePlaceHolder result = new RubikCubeFacePlaceHolder(this.RowsPerBlockFace);

			for (int i = 0; i < this.Cube.N; ++i)
				for (int j = 0; j < this.Cube.N; ++j)
					faces[i, j] = result;

			return faces;
		}
	}
}