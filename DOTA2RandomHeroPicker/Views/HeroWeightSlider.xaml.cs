using DOTA2RandomHeroPicker.Data;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace DOTA2RandomHeroPicker.Views {
	public sealed partial class HeroWeightSlider: UserControl {
		private string heroImagePath;

		public string HeroName { get; set; }

		public string FixedHeroName => HeroName.Replace("-", " ").Replace("_", " ");

		public int Weight { get; set; }

		public string HeroImagePath {
			get => heroImagePath;
			set {
				heroImagePath = value;
				if(string.IsNullOrWhiteSpace(value)) {
					HeroImage.Source = null;
				} else {
					HeroImage.Source = new BitmapImage(new Uri(this.BaseUri, value));
				}
			}
		}

		public bool IsChecked { get; set; }

		public HeroWeightSlider() {
			this.InitializeComponent();
		}

		private void WeightSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e) {
			int value = (int)WeightSlider.Value;
			WeightText.Text = $"{value}%";
			if(value <= 0) {
				HeroImage.Opacity = 0.5;
			} else {
				HeroImage.Opacity = 1;
			}
		}

		private async void HeroEnableToggle_Click(object sender, RoutedEventArgs e) {
			bool enable = HeroEnableToggle.IsChecked.Value;
			WeightSlider.IsEnabled = enable;
			if(enable) {
				WeightText.Text = $"{(int)WeightSlider.Value}%";
				HeroImage.Opacity = 1;
			} else {
				WeightText.Text = $"None";
				HeroImage.Opacity = 0.5;
			}
			LocalData.Heroes.Find(h => h.Name == HeroName).IsEnabled = enable;
			await LocalData.SaveHeroes();
		}

		private async void WeightSlider_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e) {
			LocalData.Heroes.Find(h => h.Name == HeroName).Weight = (int)WeightSlider.Value;
			await LocalData.SaveHeroes();
		}
	}
}
