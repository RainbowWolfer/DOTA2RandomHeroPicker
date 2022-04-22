using DOTA2RandomHeroPicker.Data;
using DOTA2RandomHeroPicker.Services;
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

namespace DOTA2RandomHeroPicker.Pages {
	public sealed partial class HomePage: Page {
		private int Amount { get; set; } = 1;

		public HomePage() {
			this.InitializeComponent();
			this.NavigationCacheMode = NavigationCacheMode.Required;

			StrengthFilter.IsChecked = LocalData.Filter.StrengthEnabled;
			StrengthFilter.Weight = LocalData.Filter.StrengthWeight;
			AgilityFilter.IsChecked = LocalData.Filter.AgilityEnabled;
			AgilityFilter.Weight = LocalData.Filter.AgilityWeight;
			IntelligenceFilter.IsChecked = LocalData.Filter.IntelligenceEnabled;
			IntelligenceFilter.Weight = LocalData.Filter.IntelligenceWeight;
			OneFilter.IsChecked = LocalData.Filter.DifficultyOneEnabled;
			OneFilter.Weight = LocalData.Filter.DifficultyOneWeight;
			TwoFilter.IsChecked = LocalData.Filter.DifficultyTwoEnabled;
			TwoFilter.Weight = LocalData.Filter.DifficultyTwoWeight;
			ThreeFilter.IsChecked = LocalData.Filter.DifficultyThreeEnabled;
			ThreeFilter.Weight = LocalData.Filter.DifficultyThreeWeight;
			CoreFilter.IsChecked = LocalData.Filter.CoreEnabled;
			CoreFilter.Weight = LocalData.Filter.CoreWeight;
			SupportFilter.IsChecked = LocalData.Filter.SupportEnabled;
			SupportFilter.Weight = LocalData.Filter.SupportWeight;
			RangedFilter.IsChecked = LocalData.Filter.RangedEnabled;
			RangedFilter.Weight = LocalData.Filter.RangedWeight;
			MeleeFilter.IsChecked = LocalData.Filter.MeleeEnabled;
			MeleeFilter.Weight = LocalData.Filter.MeleeWeight;
			AmountSlider.Value = LocalData.Filter.Amount;
			AmountText.Text = $"{LocalData.Filter.Amount}";
			Amount = LocalData.Filter.Amount;
		}

		private void SplitViewToggleButton_Click(object sender, RoutedEventArgs e) {
			MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
		}

		private async void RandomButton_Click(object sender, RoutedEventArgs e) {
			bool? result = Random();
			if(result == false) {
				await RandomNoResultDialog.ShowAsync();
				return;
			} else if(result == true) {
				DisplayPanel.Visibility = Visibility.Visible;
				RandomButton.IsEnabled = false;
				RefreshButton.IsEnabled = false;
				RandomPanelDisappearStoryboard.Begin();
				DisplayPanelAppearStoryboard.Begin();
			}
		}

		private async void RefreshButton_Click(object sender, RoutedEventArgs e) {
			bool? result = Random();
			if(result == false) {
				await RandomNoResultDialog.ShowAsync();
				return;
			} else if(result == true) {
				DisplayPanel.Visibility = Visibility.Visible;
				RefreshButton.IsEnabled = false;
				DisplayPanelAppearStoryboard.Begin();
			}
		}

		private Hero lastHero = null;
		private bool? Random() {
			if(!StrengthFilter.IsValid && !AgilityFilter.IsValid && !IntelligenceFilter.IsValid) {
				MissingCheckTeachingTip.Target = HeroTypePanel;
				MissingCheckTeachingTip.IsOpen = true;
				return null;
			} else if(!OneFilter.IsValid && !TwoFilter.IsValid && !ThreeFilter.IsValid) {
				MissingCheckTeachingTip.Target = DifficultyPanel;
				MissingCheckTeachingTip.IsOpen = true;
				return null;
			} else if(!CoreFilter.IsValid && !SupportFilter.IsValid) {
				MissingCheckTeachingTip.Target = HeroRolePanel;
				MissingCheckTeachingTip.IsOpen = true;
				return null;
			} else if(!RangedFilter.IsValid && !MeleeFilter.IsValid) {
				MissingCheckTeachingTip.Target = AttackTypePanel;
				MissingCheckTeachingTip.IsOpen = true;
				return null;
			}
			List<Hero> list = new();
			for(int i = 0; i < 1; i++) {
				list.Add(RandomCalculator.Calculate2(lastHero?.Name));
			}
			Hero item = list.FirstOrDefault();
			lastHero = item;
			if(item == null) {
				return false;
			}
			string name = item.GetFixedHeroName();
			HeroImage.ImageSource = new BitmapImage(new Uri(this.BaseUri, $"/HeroImages/{item.Name}.png"));
			string typePath = item.HeroType switch {
				HeroType.Strength => "/Icons/Strength_attribute_symbol.png",
				HeroType.Agility => "/Icons/Agility_attribute_symbol.png",
				HeroType.Intelligence => "/Icons/Intelligence_attribute_symbol.png",
				_ => "",
			};
			HeroTypeImage.Source = new BitmapImage(new Uri(this.BaseUri, typePath));
			HeroNameText.Text = name;
			return true;
		}

		private void AmountSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e) {
			if(AmountText == null) {
				return;
			}
			Amount = (int)AmountSlider.Value;
			AmountText.Text = $"{Amount}";
		}

		private async void AmountSlider_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e) {
			LocalData.Filter.Amount = Amount;
			await LocalData.SaveFilters();
		}

		private async void StrengthFilter_OnEnabledChanged(bool enabled) {
			LocalData.Filter.StrengthEnabled = enabled;
			await LocalData.SaveFilters();
		}

		private async void StrengthFilter_OnWeightChanged(int value) {
			await LocalData.SaveFilters();
		}

		private async void AgilityFilter_OnEnabledChanged(bool enabled) {
			LocalData.Filter.AgilityEnabled = enabled;
			await LocalData.SaveFilters();
		}

		private async void AgilityFilter_OnWeightChanged(int value) {
			await LocalData.SaveFilters();
		}

		private async void IntelligenceFilter_OnEnabledChanged(bool enabled) {
			LocalData.Filter.IntelligenceEnabled = enabled;
			await LocalData.SaveFilters();
		}

		private async void IntelligenceFilter_OnWeightChanged(int value) {
			await LocalData.SaveFilters();
		}

		private async void OneFilter_OnEnabledChanged(bool enabled) {
			LocalData.Filter.DifficultyOneEnabled = enabled;
			await LocalData.SaveFilters();
		}

		private async void OneFilter_OnWeightChanged(int value) {
			await LocalData.SaveFilters();
		}

		private async void TwoFilter_OnEnabledChanged(bool enabled) {
			LocalData.Filter.DifficultyTwoEnabled = enabled;
			await LocalData.SaveFilters();
		}

		private async void TwoFilter_OnWeightChanged(int value) {
			await LocalData.SaveFilters();
		}

		private async void ThreeFilter_OnEnabledChanged(bool enabled) {
			LocalData.Filter.DifficultyThreeEnabled = enabled;
			await LocalData.SaveFilters();
		}

		private async void ThreeFilter_OnWeightChanged(int value) {
			await LocalData.SaveFilters();
		}

		private async void RangedFilter_OnEnabledChanged(bool enabled) {
			LocalData.Filter.RangedEnabled = enabled;
			await LocalData.SaveFilters();
		}

		private async void RangedFilter_OnWeightChanged(int value) {
			await LocalData.SaveFilters();
		}

		private async void MeleeFilter_OnEnabledChanged(bool enabled) {
			LocalData.Filter.MeleeEnabled = enabled;
			await LocalData.SaveFilters();
		}

		private async void MeleeFilter_OnWeightChanged(int value) {
			await LocalData.SaveFilters();
		}

		private void StrengthFilter_OnWeightLiveUpdated(int value) {
			LocalData.Filter.StrengthWeight = value;
		}

		private void AgilityFilter_OnWeightLiveUpdated(int value) {
			LocalData.Filter.AgilityWeight = value;
		}

		private void IntelligenceFilter_OnWeightLiveUpdated(int value) {
			LocalData.Filter.IntelligenceWeight = value;
		}

		private void OneFilter_OnWeightLiveUpdated(int value) {
			LocalData.Filter.DifficultyOneWeight = value;
		}

		private void TwoFilter_OnWeightLiveUpdated(int value) {
			LocalData.Filter.DifficultyTwoWeight = value;
		}

		private void ThreeFilter_OnWeightLiveUpdated(int value) {
			LocalData.Filter.DifficultyThreeWeight = value;
		}

		private void RangedFilter_OnWeightLiveUpdated(int value) {
			LocalData.Filter.RangedWeight = value;
		}

		private void MeleeFilter_OnWeightLiveUpdated(int value) {
			LocalData.Filter.MeleeWeight = value;
		}

		private void RandomPanelDisappearStoryboard_Completed(object sender, object e) {
			RandomPanel.Visibility = Visibility.Collapsed;
		}

		private void DisplayPanelAppearStoryboard_Completed(object sender, object e) {
			RefreshButton.IsEnabled = true;
		}

		private void TestButton_Click(object sender, RoutedEventArgs e) {
			RandomCalculator.DebugSamplesAttributes();
		}

		private async void CoreFilter_OnEnabledChanged(bool enable) {
			LocalData.Filter.CoreEnabled = enable;
			await LocalData.SaveFilters();
		}

		private async void CoreFilter_OnWeightChanged(int value) {
			await LocalData.SaveFilters();
		}

		private void CoreFilter_OnWeightLiveUpdated(int value) {
			LocalData.Filter.CoreWeight = value;
		}

		private async void SupportFilter_OnEnabledChanged(bool enable) {
			LocalData.Filter.SupportEnabled = enable;
			await LocalData.SaveFilters();
		}

		private async void SupportFilter_OnWeightChanged(int value) {
			await LocalData.SaveFilters();
		}

		private void SupportFilter_OnWeightLiveUpdated(int value) {
			LocalData.Filter.SupportWeight = value;
		}

		private void Test2Button_Click(object sender, RoutedEventArgs e) {
			RandomCalculator.DebugSamplesHeroes();
		}
	}
}
