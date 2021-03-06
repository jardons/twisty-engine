﻿using System;
using System.Collections.Generic;
using System.Text;
using Twisty.Engine.Geometry;

namespace Twisty.Engine.Structure
{
	/// <summary>
	/// Class describing a center block in the cube.
	/// Center are the only place that never change of position and stay in their position on the rotation axis.
	/// </summary>
	public class CenterBlock : Block
	{
		/// <summary>
		/// Create a standard Cube Corner.
		/// </summary>
		/// <param name="initialPosition">Initial position vector of the block in the cube.</param>
		/// <param name="face">Visible face of the block.</param>
		public CenterBlock(Cartesian3dCoordinate initialPosition, BlockFace face)
			: base(face)
		{
			base.Position = initialPosition;

			// Each corner block is unique in the cube and be identified by his face.
			Id = $"CF_{face.Id}";
		}

		#region Block Members

		/// <summary>
		/// Gets the unique ID of the block.
		/// </summary>
		public override string Id { get; }

		#endregion Block Members
	}
}
