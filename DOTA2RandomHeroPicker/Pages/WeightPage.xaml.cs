using DOTA2RandomHeroPicker.Data;
using DOTA2RandomHeroPicker.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace DOTA2RandomHeroPicker.Pages {
	public sealed partial class WeightPage: Page {
		public ObservableCollection<GroupTagList> TagLists { get; } = new();

		public WeightPage() {
			this.InitializeComponent();
			this.NavigationCacheMode = NavigationCacheMode.Required;

			Initialize();
		}

		private void Initialize() {
			TagLists.Clear();
			AddHeroType("Strength", HeroType.Strength);
			AddHeroType("Agility", HeroType.Agility);
			AddHeroType("Intelligence", HeroType.Intelligence);
		}

		private void AddHeroType(string title, HeroType type) {
			TagLists.Add(new GroupTagList(title,
				LocalData.Heroes.Where(h => h.HeroType == type).Select(h => new HeroWeightSlider() {
					HeroName = h.Name,
					HeroImagePath = $"/HeroImages/{h.Name}.png",
					IsChecked = h.IsEnabled,
					Weight = h.Weight,
				}).ToList())
			);
		}

		private async void ResetButton_Click(object sender, RoutedEventArgs e) {
			ResetButton.IsEnabled = false;
			if(await ResetComfirmDialog.ShowAsync() == ContentDialogResult.Primary) {
				LocalData.ResetWeight();
				Initialize();
				await LocalData.SaveHeroes();
			}
			ResetButton.IsEnabled = true;
		}
	}
	public class GroupTagList: ObservableCollection<HeroWeightSlider> {
		public string Key { get; set; }
		public GroupTagList(string key) : base() {
			this.Key = key;
		}
		public GroupTagList(string key, List<HeroWeightSlider> content) : base() {
			this.Key = key;
			content.ForEach(s => this.Add(s));
		}
	}
}
