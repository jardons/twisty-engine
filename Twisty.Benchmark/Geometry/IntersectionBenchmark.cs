using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twisty.Engine.Geometry;

namespace Twisty.Benchmark.Geometry;

public class IntersectionBenchmark
{
	private ParametricLine[] m_IntersectedLines;
	private Plane[] m_CuttingPlanes;
	private ParametricLine[] m_CuttingLines;

	[GlobalSetup]
	public void Setup()
	{
		const int COUNT = 5000;
		m_IntersectedLines = new ParametricLine[COUNT];
		m_CuttingPlanes = new Plane[COUNT];
		m_CuttingLines = new ParametricLine[COUNT];

		Random r = new Random();

		for (int i = 0; i < COUNT; ++i)
		{
			m_IntersectedLines[i] = new ParametricLine(r.NextDouble(), r.NextDouble(), r.NextDouble(), r.NextDouble(), r.NextDouble(), r.NextDouble());
			m_CuttingPlanes[i] = new Plane(r.NextDouble(), r.NextDouble(), r.NextDouble(), r.NextDouble());
			m_CuttingLines[i] = new ParametricLine(m_CuttingPlanes[i].GetIntersection(m_IntersectedLines[i]), new(r.NextDouble(), r.NextDouble(), r.NextDouble()));
		}
	}


	[Benchmark]
	public Cartesian3dCoordinate PlaneIntersection()
	{
		// Prepare

		// Execute
		Cartesian3dCoordinate v = Cartesian3dCoordinate.Zero;
		for (int i = 0; i < m_IntersectedLines.Length; i++)
			v = m_CuttingPlanes[i].GetIntersection(m_IntersectedLines[i]);

		return v;
	}

	[Benchmark]
	public Cartesian3dCoordinate LineIntersection()
	{
		// Prepare

		// Execute
		Cartesian3dCoordinate v = Cartesian3dCoordinate.Zero;
		for (int i = 0; i < m_IntersectedLines.Length; i++)
			v = m_CuttingLines[i].GetIntersection(m_IntersectedLines[i]);

		return v;
	}
}
