﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twisty.Engine.Structure.Analysis
{
	/// <summary>
	/// Enum describing the alteration on a block.
	/// </summary>
	[Flags]
	public enum AlterationType
	{
		/// <summary>
		/// No alteration have been found.
		/// </summary>
		None = 0,
		/// <summary>
		/// Block is no in the expected position.
		/// </summary>
		Position = 1,
		/// <summary>
		/// Orientation of the block is not the expected one.
		/// </summary>
		Orientation = 2
	}

	/// <summary>
	/// Analysis tools providing alteration status on blocks.
	/// </summary>
	public class ResolutionAnalyzer
	{
		private readonly IBlocksStructure m_Blocks;

		/// <summary>
		/// Create a new ResolutionAnalyzer for the provided block structure.
		/// </summary>
		/// <param name="blocks">Structure of block for which analyze will be provided.</param>
		public ResolutionAnalyzer(IBlocksStructure blocks)
			=> m_Blocks = blocks;

		/// <summary>
		/// Gets alteration state for the block provided as argument.
		/// </summary>
		/// <param name="b">Block for which alteration status will be evaluated.</param>
		/// <returns>The alteration status of the provided block.</returns>
		public IReadOnlyDictionary<string, AlterationType> GetAlterations()
		{
			return m_Blocks.Blocks.ToDictionary(
				b => b.Id,
				b => this.GetBlockAlterations(b)
			);
		}

		/// <summary>
		/// Gets alteration state for the block provided as argument.
		/// </summary>
		/// <param name="b">Block for which alteration status will be evaluated.</param>
		/// <returns>The alteration status of the provided block.</returns>
		public AlterationType GetBlockAlterations(in Block b)
		{
			var position = b.Position;
			var originalBlock = m_Blocks.GetBlockForInitialPosition(position);

			return b.GetAlteration(originalBlock);
		}
	}
}
