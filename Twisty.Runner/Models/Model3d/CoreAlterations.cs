using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twisty.Engine.Geometry.Rotations;
using Twisty.Engine.Structure.Analysis;

namespace Twisty.Runner.Models.Model3d
{
	/// <summary>
	/// Class describing an ensemble of objects alterations information around a Rotation Core.
	/// </summary>
	public class CoreAlterations
	{
		private readonly IReadOnlyDictionary<string, IEnumerable<Rotation>> m_Rotations;
		private readonly IReadOnlyDictionary<string, AlterationType> m_Alterations;

		public CoreAlterations(IReadOnlyDictionary<string, IReadOnlyList<SimpleRotation3d>> rotations,
			IReadOnlyDictionary<string, AlterationType> alterations = null)
		{
			if (rotations is null)
				m_Rotations = new Dictionary<string, IEnumerable<Rotation>>();
			else
				m_Rotations = rotations
					.Select(kvp => new Tuple<string, IEnumerable<Rotation>>(kvp.Key, kvp.Value.Select(r => new Rotation(r))))
					.ToDictionary((t) => t.Item1, (t) => t.Item2);

			m_Alterations = alterations ?? new Dictionary<string, AlterationType>();
		}

		/// <summary>
		/// Gets the rotation applied on a specified block id.
		/// </summary>
		/// <param name="blockId">Id of the rotated block.</param>
		/// <returns>Ordered collection of rotation to apply to the rotated block.</returns>
		public IEnumerable<Rotation> GetRotations(string blockId)
			=> m_Rotations.ContainsKey(blockId) ? m_Rotations[blockId] : Array.Empty<Rotation>();

		/// <summary>
		/// Gets the alterations status for a specified block id.
		/// </summary>
		/// <param name="blockId">Id of the altered block.</param>
		/// <returns>ALteration status of the block.</returns>
		public AlterationType GetAlteration(string blockId)
			=> m_Alterations.ContainsKey(blockId) ? m_Alterations[blockId] : AlterationType.None;
	}
}
