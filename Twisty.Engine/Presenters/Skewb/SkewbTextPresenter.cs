using System;
using System.Collections.Generic;
using System.Linq;
using Twisty.Engine.Geometry;
using Twisty.Engine.Structure;
using Twisty.Engine.Structure.Skewb;

namespace Twisty.Engine.Presenters.Skewb
{
	/// <summary>
	/// Presenter preparing view object used to display a Skewb cube in the console.
	/// </summary>
	public class SkewbTextPresenter
	{
		/// <summary>
		/// Create a new SkewbTextPresenter that will present the provided cube to the Console.
		/// </summary>
		/// <param name="cube">Cube that will be displayed in the Console.</param>
		public SkewbTextPresenter(SkewbCube cube)
		{
			this.Cube = cube;
		}

		/// <summary>
		/// Gets the Cube that will be presented by this instance.
		/// </summary>
		public SkewbCube Cube { get; }

		/// <summary>
		/// Gets the information allowing to display a Skewb cube face on the console.
		/// </summary>
		/// <param name="faceId">Id of the Skewb's face that will be displayed.</param>
		/// <returns>A SkewbFaceTextView object containing the color of the faces to display in the console for the requested face.</returns>
		public SkewbFaceTextView GetFaceAsText(string faceId)
		{
			CoreFace face = this.Cube.GetFace(faceId);
			var blocks = this.Cube.GetBlocksForFace(faceId);

			var sortedBlocks = blocks.OfType<IPositionnedByCartesian3dVector>().ToList();
			sortedBlocks.Sort(new PlanePositionPointComparer(face.Coordinates));

			var faces = sortedBlocks.OfType<Block>().Select(b => b.GetBlockFace(face.Coordinates.Normal)).ToList();

			SkewbFaceTextView facesView = new SkewbFaceTextView
			{
				TopLeftColor = faces[0].Id,
				TopRightColor = faces[1].Id,
				CenterColor = faces[2].Id,
				BottomLeftColor = faces[3].Id,
				BottomRightColor = faces[4].Id
			};

			return facesView;
		}
	}
}
