using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twisty.Engine.Operations;
using Twisty.Engine.Operations.Rubiks;
using Twisty.Engine.Operations.Skewb;
using Xunit;

namespace Twisty.Engine.Tests.Operations.Rubiks
{
	[Trait("Category", "Operations")]
	public class SkewbOperationsParserTest
	{
		#region Test Methods

		[Theory]
		// White values.
		[InlineData("", 0)]
		[InlineData(" ", 0)]
		[InlineData("   ", 0)]
		[InlineData("\t", 0)]
		[InlineData(" \t ", 0)]
		// Multiple operations
		[InlineData("RRR", 3)]
		[InlineData("R R R U", 4)]
		[InlineData("RL'", 2)]
		[InlineData("RLUB'", 4)]
		public void SkewbOperationsParser_ParseCommand_ExpectedCount(string command, int count)
		{
			// 1. Prepare
			SkewbOperationsParser p = new SkewbOperationsParser();

			// 2. Execute
			var r = p.Parse(command);

			// 3. Verify
			Assert.NotNull(r);
			Assert.Equal(count, r.Count());
		}

		[Theory]
		[InlineData("R")]
		[InlineData("L")]
		[InlineData("U")]
		[InlineData("B")]
		public void SkewbOperationsParser_ParseSingleOperationCheckOrientation_BeExpected(string command)
		{
			// 1. Prepare
			SkewbOperationsParser p = new SkewbOperationsParser();

			// 2. Execute
			var parsed = p.Parse(command);
			var parsedReverse = p.Parse(command + "'");

			// 3. Verify
			Assert.NotNull(parsed);
			Assert.NotNull(parsedReverse);
			Assert.Single(parsed);
			Assert.Single(parsedReverse);
			Assert.True(parsed.Cast<SkewbOperation>().FirstOrDefault().IsClockwise);
			Assert.False(parsedReverse.Cast<SkewbOperation>().FirstOrDefault().IsClockwise);
		}

		[Theory]
		[InlineData("ABCD", 0)]
		[InlineData(" ABCD", 1)]
		[InlineData("RR'Y", 3)]
		public void SkewbOperationsParser_ParseInvalidCommand_ThrowOperationParsingException(string command, int badIndex)
		{
			// 1. Prepare
			SkewbOperationsParser p = new SkewbOperationsParser();

			// 2. Execute
			Action a = () => p.Parse(command);

			// 3. Verify
			var e = Assert.Throws<OperationParsingException>(a);
			Assert.Equal(command, e.Command);
			Assert.Equal(badIndex, e.Index);
		}

		#endregion Test Methods
	}
}
