using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Twisty.Runner.Models.Model3d;
using Twisty.Runner.ViewModels;

namespace Twisty.Runner.Views
{
	/// <summary>
	/// Interaction logic for StandardView.xaml
	/// </summary>
	public partial class RotationCoreStandardView : UserControl
	{
		#region Binding Properties

		private static readonly DependencyProperty ObjectStructuresProperty =
			DependencyProperty.Register("ObjectStructures",
				typeof(IEnumerable<Core3dObject>),
				typeof(RotationCoreStandardView),
				new FrameworkPropertyMetadata(
					Array.Empty<Core3dObject>(),
					FrameworkPropertyMetadataOptions.AffectsRender,
					(DependencyObject defectImageControl, DependencyPropertyChangedEventArgs eventArgs) =>
						((RotationCoreStandardView)defectImageControl).ObjectStructures = eventArgs.NewValue as IEnumerable<Core3dObject>
				));

		private static readonly DependencyProperty ObjectRotationsProperty =
			DependencyProperty.Register("ObjectRotations",
				typeof(CoreRotations),
				typeof(RotationCoreStandardView),
				new FrameworkPropertyMetadata(
					null,
					FrameworkPropertyMetadataOptions.AffectsRender,
					(DependencyObject defectImageControl, DependencyPropertyChangedEventArgs eventArgs) =>
						((RotationCoreStandardView)defectImageControl).ObjectRotations = eventArgs.NewValue as CoreRotations
				));

		#endregion Binding Properties

		private Dictionary<string, ModelVisual3D> m_3dObjects;

		public RotationCoreStandardView()
		{
			m_3dObjects = new Dictionary<string, ModelVisual3D>();

			InitializeComponent();

			this.DataContext.CameraPosition = new Point3D(6.0, -3.0, 3.0);

			CreateCanvasContent();
		}

		#region Public Properties

		public new RotationCoreStandardViewModel DataContext
			=> (RotationCoreStandardViewModel)base.DataContext;

		public IEnumerable<Core3dObject> ObjectStructures
		{
			get => this.GetValue(ObjectStructuresProperty) as IEnumerable<Core3dObject>
				?? Array.Empty<Core3dObject>();
			set
			{
				this.SetValue(ObjectStructuresProperty, value);
				this.CreateCanvasContent();
			}
		}

		public CoreRotations ObjectRotations
		{
			get => this.GetValue(ObjectRotationsProperty) as CoreRotations;
			set
			{
				this.SetValue(ObjectRotationsProperty, value);
				this.RefreshRotations();
			}
		}

		public string CurrentCamera
		{
			get => this.DataContext.ActiveCameraId;
			set { this.DataContext.ActiveCameraId = value; }
		}

		#endregion Public Properties

		#region Private Methods

		private void CreateCanvasContent()
		{
			// Remove Previous instance of the core.
			foreach (var o in m_3dObjects.Values)
				AnimatedCore.Children.Remove(o);

			m_3dObjects.Clear();

			// Create new instance.
			foreach (Core3dObject o in this.ObjectStructures)
			{
				var v = CreateVisuals(o);

				m_3dObjects.Add(o.Id, v);
				AnimatedCore.Children.Add(v);
			}
		}

		private ModelVisual3D CreateVisuals(Core3dObject o)
		{
			Model3DGroup group = new Model3DGroup();

			foreach (var part in o.Parts)
			{
				// Create visual object.
				GeometryModel3D model = new GeometryModel3D
				{
					Geometry = GetMesh(part.Points),
					Material = GetBrush(part.FrontColor),
					BackMaterial = GetBrush(part.BackColor)
				};

				group.Children.Add(model);
			}

			return new ModelVisual3D { Content = group };
		}

		/// <summary>
		/// Create the Mesh for a single block face.
		/// </summary>
		/// <param name="points"></param>
		/// <returns></returns>
		private MeshGeometry3D GetMesh(IReadOnlyList<Point3D> points)
		{
			// Mesh will be created by suming triangle sharing a common vertice in the center of the surface.
			MeshGeometry3D geo = new MeshGeometry3D();
			geo.Positions.Add(points[0]);
			geo.Positions.Add(points[1]);
			geo.Positions.Add(points[2]);

			geo.TriangleIndices.Add(0);
			geo.TriangleIndices.Add(1);
			geo.TriangleIndices.Add(2);

			for (int i = 3; i < points.Count; ++i)
			{
				// Update positions.
				geo.Positions.Add(points[i]);

				geo.TriangleIndices.Add(0);         // First Point
				geo.TriangleIndices.Add(i - 1);     // Previous
				geo.TriangleIndices.Add(i);         // Current
			}

			return geo;
		}

		private DiffuseMaterial GetBrush(Color c)
			=> new DiffuseMaterial { Brush = new SolidColorBrush(c) };

		/// <summary>
		/// Refresh the display state of the Core object.
		/// </summary>
		private void RefreshRotations()
		{
			foreach (var key in this.m_3dObjects.Keys)
			{
				m_3dObjects[key].Transform = GetTransform(this.ObjectRotations.GetRotations(key));
			}
		}

		private Transform3D GetTransform(IEnumerable<Models.Model3d.Rotation> rotations)
		{
			Transform3DGroup group = new Transform3DGroup();

			// Use Euler angles to provide correct orientation on every blocks.
			foreach (var rotation in rotations)
			{
				group.Children.Add(new RotateTransform3D
				{
					Rotation = new AxisAngleRotation3D
					{
						Axis = rotation.Axis,
						Angle = rotation.Angle
					}
				});
			}

			return group;
		}

		#endregion Private Methods
	}
}
