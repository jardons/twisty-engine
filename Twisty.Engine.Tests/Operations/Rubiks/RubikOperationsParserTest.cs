using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twisty.Engine.Operations;
using Twisty.Engine.Operations.Rubiks;
using Twisty.Engine.Tests.Assertions;
using Xunit;

namespace Twisty.Engine.Tests.Operations.Rubiks
{
	[Trait("Category", "Operations")]
	public class RubikOperationsParserTest
	{
		#region Test Data

		//(string command)
		public static readonly TheoryData<string> Operations = new TheoryData<string>()
		{
			{"R"},
			{"L"},
			{"F"},
			{"U"},
			{"B"},
			{"D"},
		};

		//(string command, int count, int cleanedCount)
		public static readonly TheoryData<string, int, int> InputStrings = new TheoryData<string, int, int>()
		{
			// White values.
			{"", 0, 0},
			{" ", 0, 0},
			{"   ", 0, 0},
			{"\t", 0, 0},
			{" \t ", 0, 0},
			// Single operations.
			{"R", 1, 1},
			{"L", 1, 1},
			{"F", 1, 1},
			{"U", 1, 1},
			{"B", 1, 1},
			{"D", 1, 1},
			{"R'", 1, 1},
			{"L'", 1, 1},
			{"F'", 1, 1},
			{"U'", 1, 1},
			{"B'", 1, 1},
			{"D'", 1, 1},
			// Multiple operations
			{"RRR", 3, 3},
			{"R R R F", 4, 4},
			{"RL'", 2, 2},
			{"RLFUBD'", 6, 6},
		};

		#endregion Test Data

		#region Test Methods

		[Theory]
		[MemberData(nameof(RubikOperationsParserTest.InputStrings), MemberType = typeof(RubikOperationsParserTest))]
		public void RubikOperationsParser_ParseCommand_ExpectedCount(string command, int count, int cleanedCount)
		{
			// 1. Prepare
			RubikOperationsParser p = new RubikOperationsParser();

			// 2. Execute
			var r = p.Parse(command);

			// 3. Verify
			Assert.Equal(count, r.Count());
		}

		[Theory]
		[MemberData(nameof(RubikOperationsParserTest.Operations), MemberType = typeof(RubikOperationsParserTest))]
		public void RubikOperationsParser_ParseSingleOperationCheckOrientation_BeExpected(string command)
		{
			// 1. Prepare
			RubikOperationsParser p = new RubikOperationsParser();

			// 2. Execute
			var r1 = p.Parse(command).Cast<LayerOperation>().First();
			var r2 = p.Parse(command + "'").Cast<LayerOperation>().First();

			// 3. Verify
			Assert.Equal(Math.PI / 2.0, r1.Theta, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(-Math.PI / 2.0, r2.Theta, GeometryAssert.PRECISION_DOUBLE);
		}

		[Theory]
		[InlineData("ABCD", 0)]
		[InlineData(" ABCD", 1)]
		[InlineData("RR'Y", 3)]
		public void RubikOperationsParser_ParseInvalidCommand_ThrowOperationParsingException(string command, int badIndex)
		{
			// 1. Prepare
			RubikOperationsParser p = new RubikOperationsParser();

			// 2. Execute
			void a() => p.Parse(command);

			// 3. Verify
			var e = Assert.Throws<OperationParsingException>(a);
			Assert.Equal(command, e.Command);
			Assert.Equal(badIndex, e.Index);
		}

		[Theory]
		[InlineData("R", "R")]
		[InlineData("RL'", "RL'")]
		public void RubikOperationsParser_TryClean_ReturnTrueAndCleaned(string command, string expected)
		{
			// 1. Prepare
			RubikOperationsParser p = new RubikOperationsParser();

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
		public void RubikOperationsParser_TryCleanInvalid_ReturnFalse(string command)
		{
			// 1. Prepare
			RubikOperationsParser p = new RubikOperationsParser();

			// 2. Execute
			bool b = p.TryClean(command, out string cleaned);

			// 3. Verify
			Assert.False(b);
			Assert.Equal(command, cleaned);
		}

		#endregion Test Methods
	}
}
