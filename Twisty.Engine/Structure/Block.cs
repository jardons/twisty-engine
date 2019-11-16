using System;
using System.Collections.Generic;
using System.Linq;
using Twisty.Engine.Geometry;

namespace Twisty.Engine.Structure
{
	public class Block
	{
		private List<BlockFace> m_Faces;

		/// <summary>
		/// Create a new block proposing a single BlockFace.
		/// </summary>
		/// <param name="face">only available BlockFace.</param>
		public Block(BlockFace face)
		{
			if (face == null)
				throw new ArgumentNullException("A block need at least one visible BlockFace", nameof(face));

			m_Faces = new List<BlockFace>();
			m_Faces.Add(face);
		}

		public Block(IEnumerable<BlockFace> faces)
		{
			if (faces == null)
				throw new ArgumentNullException("A block need at least one visible BlockFace", nameof(faces));

			m_Faces = new List<BlockFace>(faces);
			if (m_Faces.Count == 0)
				throw new ArgumentException("A block need at least one visible BlockFace", nameof(faces));
		}

		#region Public Properties

		/// <summary>
		/// Position is stored using the direction relative to the Form center.
		/// </summary>
		public SphericalVector Position { get; set; }

		#endregion Public Properties

		#region Public Methods

		public void RotateAround(SphericalVector axis, double theta)
		{
			if (axis == null)
				throw new ArgumentNullException("Orientation is mandatory", nameof(axis));

			foreach (BlockFace face in m_Faces)
				face.MoveAround(axis, theta);
		}

		public BlockFace GetBlockFace(SphericalVector o)
		{
			if (o == null)
				throw new ArgumentNullException("Orientation is mandatory", nameof(o));

			return m_Faces.FirstOrDefault(f => f.Position == o);
		}

		#endregion Public Methods
	}
}