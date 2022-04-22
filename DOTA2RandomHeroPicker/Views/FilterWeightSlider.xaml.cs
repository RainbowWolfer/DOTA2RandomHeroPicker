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
	public sealed partial class FilterWeightSlider: UserControl {
		public event Action<bool> OnEnabledChanged;
		public event Action<int> OnWeightChanged;
		public event Action<int> OnWeightLiveUpdated;

		private string iconPath;
		private int weight;
		private bool isChecked;

		public string Title { get; set; }
		public bool IsChecked {
			get => isChecked;
			set {
				isChecked = value;
				WeightSlider.IsEnabled = value;
			}
		}
		public int Weight {
			get => weight;
			set {
				weight = value;
				SliderToolTip.Text = EnabledCheckBox.IsChecked.Value ? $"{(int)WeightSlider.Value}%" : "None";
			}
		}

		public bool IsValid => EnabledCheckBox.IsChecked.Value && (int)WeightSlider.Value > 0;

		public string IconPath {
			get => iconPath;
			set {
				iconPath = value;
				if(string.IsNullOrWhiteSpace(value)) {
					IconImage.Visibility = Visibility.Collapsed;
					IconImage.Source = null;
				} else {
					IconImage.Visibility = Visibility.Visible;
					IconImage.Source = new BitmapImage(new Uri(this.BaseUri, value));
				}
			}
		}

		public FilterWeightSlider() {
			this.InitializeComponent();
		}

		private void WeightSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e) {
			int value = (int)e.NewValue;
			SliderToolTip.Text = $"{value}%";
			OnWeightLiveUpdated?.Invoke(value);
		}

		private void EnabledCheckBox_Click(object sender, RoutedEventArgs e) {
			bool enable = EnabledCheckBox.IsChecked.Value;
			WeightSlider.IsEnabled = enable;
			if(enable) {
				SliderToolTip.Text = $"{(int)WeightSlider.Value}%";
			} else {
				SliderToolTip.Text = $"None";
			}
			OnEnabledChanged?.Invoke(enable);
		}

		private void WeightSlider_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e) {
			OnWeightChanged?.Invoke((int)WeightSlider.Value);
		}
	}
}
