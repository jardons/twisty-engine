using System;
using System.Collections.Generic;
using System.Text;
using Twisty.Engine.Geometry;

namespace Twisty.Engine.Structure.Rubiks
{
	/// <summary>
	/// Class describing a standard corner in a rubiks cube.
	/// A Corner is always showing 3 faces to the outside, with an angle of 90 degrees between them.
	/// </summary>
    public class RubikCornerBlock : Block
    {
		private string m_Id;

		/// <summary>
		/// Create a standard Rubik Corner.
		/// </summary>
		/// <param name="initialPosition">Initial position vector of the block in the cube.</param>
		/// <param name="face1">First visible face of the block.</param>
		/// <param name="face2">Second visible face of the block.</param>
		/// <param name="face3">Third visible face of the block.</param>
		public RubikCornerBlock(SphericalVector initialPosition, BlockFace face1, BlockFace face2, BlockFace face3)
			: base(new BlockFace[] { face1, face2, face3 })
        {
            base.Position = initialPosition;

			// Each corner block is unique in the cube and be identified by the combination of his 3 faces.
			m_Id = $"C{face1.Id}{face2.Id}{face3.Id}";
		}

		#region Block Members

		/// <summary>
		/// Gets the unique ID of the block.
		/// </summary>
		public override string Id => m_Id;

		#endregion Block Members
	}
}