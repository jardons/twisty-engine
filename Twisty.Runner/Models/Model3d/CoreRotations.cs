using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twisty.Engine.Geometry.Rotations;

namespace Twisty.Runner.Models.Model3d
{
	/// <summary>
	/// Class describing an ensemble of objects rotations around a Rotation Core.
	/// </summary>
	public class CoreRotations
	{
		private IDictionary<string, IEnumerable<Rotation>> m_Rotations;

		public CoreRotations(IReadOnlyDictionary<string, IReadOnlyList<SimpleRotation3d>> rotations)
		{
			if (rotations == null)
				m_Rotations = new Dictionary<string, IEnumerable<Rotation>>();
			else
				m_Rotations = rotations
					.Select(kvp => new Tuple<string, IEnumerable<Rotation>>(kvp.Key, kvp.Value.Select(r => new Rotation(r))))
					.ToDictionary((t) => t.Item1, (t) => t.Item2);
		}

		/// <summary>
		/// Gets the rotation applied on a specified block id.
		/// </summary>
		/// <param name="blockId">Id of the rotated block.</param>
		/// <returns>Ordered collection of rotation to apply to the rotated block.</returns>
		public IEnumerable<Rotation> GetRotations(string blockId)
			=> m_Rotations.ContainsKey(blockId) ? m_Rotations[blockId] : Array.Empty<Rotation>();
	}
}
