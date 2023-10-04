﻿using System;
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
using Twisty.Runner.Models;
using Twisty.Runner.ViewModels;

namespace Twisty.Runner.Views
{
	/// <summary>
	/// Control allowing to use algorithms libraries.
	/// </summary>
	public partial class AlgorithmLibrary : UserControl
	{
		#region Binding Properties

		private static readonly DependencyProperty CoreProperty =
			DependencyProperty.Register("Core",
				typeof(RotationCoreObject),
				typeof(AlgorithmLibrary),
				new FrameworkPropertyMetadata(
					null,
					FrameworkPropertyMetadataOptions.AffectsRender,
					(DependencyObject defectImageControl, DependencyPropertyChangedEventArgs eventArgs) =>
						// Update binded data to ViewModel.
						((AlgorithmLibrary)defectImageControl).DataContext.Core = eventArgs.NewValue as RotationCoreObject
				));

		#endregion Binding Properties

		public AlgorithmLibrary()
		{
			InitializeComponent();
		}

		public new AlgorithmLibraryViewModel DataContext
		{
			get => (AlgorithmLibraryViewModel)base.DataContext;
			set => base.DataContext = value;
		}

		public RotationCoreObject Core
		{
			get => DataContext.Core;
			set => DataContext.Core = value;
		}
	}
}