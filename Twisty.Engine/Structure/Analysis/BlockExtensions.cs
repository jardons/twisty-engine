using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twisty.Engine.Structure.Analysis;

/// <summary>
/// Extensions method providing analysis tools on the Block class.
/// </summary>
internal static class BlockExtensions
{
	/// <summary>
	/// Calculate the alteration level needed to reach the difference between 2 blocks.
	/// </summary>
	/// <param name="source">Source block for the comparison.</param>
	/// <param name="target">Target block for the comparison.</param>
	/// <returns>AlterationType indicating if the target block is the result of a permutation, rotation or didn't suffer any alteration.</returns>
	public static AlterationType GetAlteration(this Block source, in Block target)
	{
		if (source.Definition.Faces.Count != target.Definition.Faces.Count)
			return AlterationType.Position;

		AlterationType finalResult = AlterationType.None;

		foreach (var face in target.Definition.Faces)
		{
			var originalFace = source.GetBlockFace(face.Id);
			if (originalFace is null)
				return AlterationType.Position;

			// One face is enough to declare an orientation difference.
			if (finalResult == AlterationType.Orientation)
				continue;

			if (!target.Orientation.Rotate(face.Position).IsSameVector(originalFace.Position))
				// Indicate the orientation issue, but we still need to confirm it's not a position difference.
				finalResult = AlterationType.Orientation;
		}

		return finalResult;
	}
}
