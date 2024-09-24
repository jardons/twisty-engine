using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twisty.Benchmark
{
	public class QuickTest
	{
		private const float PRECISION = 0.0000001f;

		public class Test1
		{
			public int Value { get; set; }
		}

		public class Test2 : Test1
		{

			public int GetMyValue() => Value;
		}

		private List<Test1> m_Test1;
		private List<Test1> m_Test2;

		[GlobalSetup]
		public void Setup()
		{
			m_Test1 = new();
			m_Test2 = new();

			for (int i = 0; i < 50000; ++i)
			{
				m_Test1.Add(new Test1 { Value = i });
				m_Test2.Add(new Test2 { Value = i });
			}
		}


		[Benchmark]
		public int RunTest1()
		{
			// Prepare

			// Execute
			int v = 0;
			for (int i = 0; i < m_Test1.Count; i++)
				v += m_Test1[i].Value;
			return v;
		}

		[Benchmark]
		public int RunTest2()
		{
			// Prepare


			// Execute
			int v = 0;
			for (int i = 0; i < m_Test2.Count; i++)
				v += m_Test2[i].Value;
			return v;
		}

	}
}