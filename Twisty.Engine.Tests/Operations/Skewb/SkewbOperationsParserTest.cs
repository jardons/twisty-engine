using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twisty.Engine.Operations;
using Twisty.Engine.Operations.Rubiks;
using Twisty.Engine.Operations.Skewb;
using Twisty.Engine.Tests.Assertions;
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
			GeometryAssert.AngleEqual(Math.PI * 2.0 / 3.0, parsed.Cast<AxisOperation>().First().Theta);
			GeometryAssert.AngleEqual(-Math.PI * 2.0 / 3.0, parsedReverse.Cast<AxisOperation>().First().Theta);
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
			void a() => p.Parse(command);

			// 3. Verify
			var e = Assert.Throws<OperationParsingException>(a);
			Assert.Equal(command, e.Command);
			Assert.Equal(badIndex, e.Index);
		}

		[Theory]
		// Unchanged
		[InlineData("R", "R")]
		[InlineData("RL'", "RL'")]
		// Cases correction
		[InlineData("r", "R")]
		[InlineData("lu'", "LU'")]
		public void SkewbOperationsParser_TryClean_ReturnTrueAndCleaned(string command, string expected)
		{
			// 1. Prepare
			SkewbOperationsParser p = new SkewbOperationsParser();

			// 2. Execute
			bool b = p.TryClean(command, out string cleaned);

			// 3. Verify
			Assert.True(b);
			Assert.Equal(expected, cleaned);
		}

		[Theory]
		[InlineData("this is a test")]
		[InlineData("?")]
		[InlineData("FGH")]
		public void SkewbOperationsParser_TryCleanInvalid_ReturnFalse(string command)
		{
			// 1. Prepare
			SkewbOperationsParser p = new SkewbOperationsParser();

			// 2. Execute
			bool b = p.TryClean(command, out string cleaned);

			// 3. Verify
			Assert.False(b);
			Assert.Equal(command, cleaned);
		}

		#endregion Test Methods
	}
}
