using Twisty.Engine.Geometry;

namespace Twisty.Engine.Structure
{
	/// <summary>
	/// Class representing one face for a block.
	/// </summary>
	public class BlockFace
	{
		/// <summary>
		/// Create a new objet representing a block face.
		/// </summary>
		/// <param name="identifier">Identifier of the BlockFaces that should match the expected face.</param>
		/// <param name="p"></param>
		public BlockFace(string identifier, SphericalVector p)
		{
			this.Position = p;
			this.Id = identifier;
		}

		/// <summary>
		/// Create a new objet representing a block face.
		/// </summary>
		/// <param name="identifier">Identifier of the BlockFaces that should match the expected face.</param>
		/// <param name="p"></param>
		public BlockFace(string identifier, Cartesian3dCoordinate p)
		{
			this.Position = CoordinateConverter.ConvertToSpherical(p);
			this.Id = identifier;
		}

		/// <summary>
		/// Indicate the position of the face in the block.
		/// Position is relative to the Block Center.
		/// </summary>
		/// <remarks>distance from the block is not specified as we stay in a conceptual level.</remarks>
		public SphericalVector Position { get; private set; }

		/// <summary>
		/// Textual identifier used to identify BlockFaces among them and should match the correct face of the owner.
		/// </summary>
		public string Id { get; }

		public void MoveAround(SphericalVector axis, double theta) => this.Position = this.Position.RotateAround(axis, theta);
	}
}