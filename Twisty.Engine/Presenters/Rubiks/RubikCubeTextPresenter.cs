using System;
using System.Collections.Generic;
using System.Linq;
using Twisty.Engine.Geometry;
using Twisty.Engine.Structure;
using Twisty.Engine.Structure.Rubiks;

namespace Twisty.Engine.Presenters.Rubiks
{
	public interface IRubikFaceTextView
	{
		/// <summary>
		/// Get shte specified line to display in the console.
		/// </summary>
		/// <param name="i">Index of the line to display.</param>
		/// <returns>Line to display for the requested index of the view.</returns>
		string GetLine(int i);

		string Color { get; }

		IEnumerable<string> Lines { get; }
	}

	public class RubikFaceTextView
	{
		private IRubikFaceTextView[,] m_BlocksFaces;

		public RubikFaceTextView(int n)
		{
			this.N = n;
			m_BlocksFaces = new IRubikFaceTextView[n, n];
		}

		public IRubikFaceTextView this[int x, int y]
		{
			get { return m_BlocksFaces[x, y]; }
			internal set { m_BlocksFaces[x, y] = value; }
		}

		public int N { get; }
	}

	public class RubikCubeTextPresenter
	{
		private class RubikCubeBlockFaceTextView : IRubikFaceTextView
		{
			#region Private Members

			private List<string> m_Lines;
			private int m_Size;

			#endregion Private Members

			public RubikCubeBlockFaceTextView(int size, string color)
			{
				m_Lines = new List<string>(size / 2);
				m_Size = size;
				this.Color = color;
			}

			public string Color { get; }

			public void Append(string line)
			{
				if (line == null)
					throw new ArgumentNullException(nameof(line));

				if (line.Length != m_Size)
					throw new ArgumentException("Length of the line does not matche the face size.", nameof(line));

				if (m_Lines.Count >= m_Size / 2)
					throw new InvalidOperationException("Maximal number of line has been reached.");

				m_Lines.Add(line);
			}

			#region IFaceTextView Members

			/// <summary>
			/// Get shte specified line to display in the console.
			/// </summary>
			/// <param name="i">Index of the line to display.</param>
			/// <returns>Line to display for the requested index of the view.</returns>
			public string GetLine(int i) => m_Lines[i];

			public IEnumerable<string> Lines => m_Lines;

			#endregion IFaceTextView Members
		}

		private class RubikCubeFacePlaceOlder : IRubikFaceTextView
		{
			private readonly string m_PlaceHolder;

			public RubikCubeFacePlaceOlder(int size)
			{
				m_PlaceHolder = string.Empty.PadRight(size);
			}

			#region IFaceTextView Members

			public string Color => null;

			/// <summary>
			/// Get shte specified line to display in the console.
			/// </summary>
			/// <param name="i">Index of the line to display.</param>
			/// <returns>Line to display for the requested index of the view.</returns>
			public string GetLine(int i) => m_PlaceHolder;

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

		public RubikFaceTextView GetFaceAsText(string faceId)
		{
			// TODO review this code by sorting blocks to their correct position.
			var axis = this.Cube.GetAxis(faceId);
			var blocks = this.Cube.GetBlocksForFace(axis.Vector);

			RubikFaceTextView faces = new RubikFaceTextView(this.Cube.N);
			int i = 0;
			int j = 0;
			
			var first = CoordinateConverter.ConvertToCartesian(blocks.FirstOrDefault().Position);
			Plane p = new Plane(CoordinateConverter.ConvertToCartesian(axis.Vector), first);

			var sortedBlocks = blocks.OfType<IPositionnedBySphericalVector>().ToList();
			sortedBlocks.Sort(new PlanePositionPointComparer(p));

			foreach (var block in sortedBlocks.OfType<Block>())
			{
				var face = block.GetBlockFace(axis.Vector);

				RubikCubeBlockFaceTextView result = new RubikCubeBlockFaceTextView(RowsPerBlockFace, face.Id);
				//result.Append("****");
				//result.Append("****");
				result.Append(block.Id);
				result.Append("****");

				faces[j, i++] = result;
				if (i >= 2)
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
		/// <returns></returns>
		public RubikFaceTextView GetFacePlaceHolder()
		{
			RubikFaceTextView faces = new RubikFaceTextView(this.Cube.N);
			RubikCubeFacePlaceOlder result = new RubikCubeFacePlaceOlder(this.RowsPerBlockFace);
			
			for (int i=0;i< this.Cube.N;++i)
				for (int j = 0; j < this.Cube.N; ++j)
					faces[i, j] = result;

			return faces;
		}
	}
}