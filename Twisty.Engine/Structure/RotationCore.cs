using System;
using System.Collections.Generic;
using System.Linq;
using Twisty.Engine.Geometry;

namespace Twisty.Engine.Structure
{
	/// <summary>
	/// Class describing the object that will represent the central rotation point around which blocks will rotate.
	/// </summary>
	public abstract class RotationCore
	{
		private List<Block> m_Blocks;

		/// <summary>
		/// Create a new RotationCore object
		/// </summary>
		/// <param name="blocks"></param>
		protected RotationCore(IEnumerable<Block> blocks)
		{
			m_Blocks = new List<Block>(blocks);
		}

		/// <summary>
		/// Get the Blocks for a specific face of the twisty puzzle based on the current blocks positions.
		/// </summary>
		/// <param name="v">Orientation of the face of the puzzle.</param>
		/// <returns>The list of blocks currently visible on the requested face.</returns>
		/// <remarks>Logic is only valid for standard forms. Any shapeshifting form would need to improve this logic.</remarks>
		public IEnumerable<Block> GetBlocksForFace(SphericalVector v)
		{
			if (v == null)
				throw new ArgumentNullException("Orientation is mandatory", nameof(v));

			return m_Blocks.Where(b => b.GetBlockFace(v) != null);
		}
	}
}