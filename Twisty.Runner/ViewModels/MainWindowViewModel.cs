﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Twisty.Runner.Models.Model3d;
using Twisty.Runner.Services;
using Twisty.Runner.Models;
using System.Windows.Input;
using Twisty.Runner.Wpf;

namespace Twisty.Runner.ViewModels
{
	public class MainWindowViewModel : INotifyPropertyChanged
	{
		// Services
		private readonly IRotationCoreService m_RotationCoreService;

		// Internal models
		private RotationCoreObject m_Core;
		private CoreAlterations m_VisualAlterations;
		private string m_MaterializerId;

		public MainWindowViewModel(IRotationCoreService rotationCoreService)
		{
			m_RotationCoreService = rotationCoreService;
			this.LoadCore("Rubik[3]");

			this.SelectCore = new RelayCommand(
				(p) => this.LoadCore((string)p)
			);

			this.SelectMaterializer = new RelayCommand(
				(p) =>
				{
					this.MaterializerId = (string)p;
					this.LoadCore(this.Core.Id);
				}
			);
		}

		/// <summary>
		/// Gets the RotationCoreObject used in this window.
		/// </summary>
		public RotationCoreObject Core
		{
			get => m_Core;
			set
			{
				m_Core = value;
				if (PropertyChanged is not null)
					this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(Core)));
			}
		}

		public CoreAlterations Alterations
		{
			get => m_VisualAlterations;
			set
			{
				m_VisualAlterations = value;
				if (PropertyChanged is not null)
					this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(Alterations)));
			}
		}

		public string MaterializerId
		{
			get => m_MaterializerId ?? "stickerless";
			set
			{
				m_MaterializerId = value;
				if (PropertyChanged is not null)
					this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(MaterializerId)));
			}
		}

		public ICommand SelectCore { get; }

		public ICommand SelectMaterializer { get; }

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion INotifyPropertyChanged Members

		private void LoadCore(string id)
		{
			this.Core = m_RotationCoreService.CreateNewCore(id, MaterializerId, () => RefreshAlterations());
			this.RefreshAlterations();
		}

		private void RefreshAlterations()
		{
			this.Alterations = this.m_RotationCoreService.CalculateAlterations(this.Core);
		}
	}
}
