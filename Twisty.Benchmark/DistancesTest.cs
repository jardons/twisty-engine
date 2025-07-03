using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twisty.Engine.Geometry;

namespace Twisty.Benchmark
{
	[MemoryDiagnoser]
	public class DistancesTest
	{
		private List<Cartesian3dCoordinate> m_Tests3;
		private List<Cartesian2dCoordinate> m_Tests2;

		[GlobalSetup]
		public void Setup()
		{
			m_Tests3 = new();
			m_Tests2 = new();

			Random r = new Random();
			r.Next(1000);

			for (int i = 0; i < 50000; ++i)
			{
				m_Tests3.Add(new Cartesian3dCoordinate (1.0d / r.Next(1000), 1.0d / r.Next(1000), 1.0d / r.Next(1000)));
				m_Tests2.Add(new Cartesian2dCoordinate(1.0d / r.Next(1000), 1.0d / r.Next(1000)));
			}
		}

		[Benchmark]
		public double Distance3()
		{
			double v = 0;
			for (int i = 0; i < m_Tests3.Count - 1; i++)
				v += m_Tests3[i].GetDistanceTo(m_Tests3[i+1]);
			return v;
		}

		[Benchmark]
		public double ManhattanDistance3()
		{
			double v = 0;
			for (int i = 0; i < m_Tests3.Count - 1; i++)
				v += m_Tests3[i].GetManhattanDistanceTo(m_Tests3[i + 1]);
			return v;
		}

		[Benchmark]
		public double Distance2()
		{
			double v = 0;
			for (int i = 0; i < m_Tests2.Count - 1; i++)
				v += m_Tests2[i].GetDistanceTo(m_Tests2[i + 1]);
			return v;
		}

		[Benchmark]
		public double ManhattanDistance2()
		{
			double v = 0;
			for (int i = 0; i < m_Tests2.Count - 1; i++)
				v += m_Tests2[i].GetManhattanDistanceTo(m_Tests2[i + 1]);
			return v;
		}
	}
}