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
		/// Get a Block for a specific face of the twisty puzzle based on the current blocks positions.
		/// </summary>
		/// <param name="o">Orientation of the face of the puzzle.</param>
		/// <returns>The list of blocks currently visible on the requested face.</returns>
		/// <remarks>Logis is only valid for standard forms. Any shapeshifting form would need to improve this logic.</remarks>
		public IEnumerable<Block> GetBlockForFace(SphericalVector o)
		{
			if (o == null)
				throw new ArgumentNullException("Orientation is mandatory", nameof(o));

			return m_Blocks.Where(b => b.GetBlockFace(o) != null);
		}
	}
}