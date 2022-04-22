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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace DOTA2RandomHeroPicker.Pages {
	public sealed partial class SettingsPage: Page {
		public SettingsPage() {
			this.InitializeComponent();
			this.NavigationCacheMode = NavigationCacheMode.Required;
			Update();
		}

		private void Update() {
			RefreshButton.IsEnabled = false;
			HeroesGridView.Items.Clear();
			foreach(Hero item in LocalData.Heroes) {
				HeroesGridView.Items.Add(new HeroDataView() {
					Hero = item,
				});
			}
			RefreshButton.IsEnabled = true;
		}

		private async void RefreshButton_Click(object sender, RoutedEventArgs e) {
			RefreshButton.IsEnabled = false;
			EditToggle.IsChecked = false;
			UpdateItemsEdit();
			if(await ResetComfirmDialog.ShowAsync() == ContentDialogResult.Primary) {
				LocalData.ResetHeroes();
				Update();
			}
			RefreshButton.IsEnabled = true;
		}

		private void EditToggle_Click(object sender, RoutedEventArgs e) {
			UpdateItemsEdit();
		}

		private void UpdateItemsEdit() {
			foreach(HeroDataView item in HeroesGridView.Items) {
				item.EnableEdit = EditToggle.IsChecked.Value;
			}
		}
	}
}
