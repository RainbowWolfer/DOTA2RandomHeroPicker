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
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace DOTA2RandomHeroPicker.Views {
	public sealed partial class HeroDataView: UserControl {
		private Hero hero;

		private bool updating;
		private bool enableEdit;

		public Hero Hero {
			get => hero;
			set {
				hero = value;
				UpdateHeroInfo();
			}
		}

		public bool EnableEdit {
			get => enableEdit;
			set {
				enableEdit = value;
				AttackTypeBox.IsEnabled = value;
				HeroRoleBox.IsEnabled = value;
			}
		}

		public HeroDataView() {
			this.InitializeComponent();
			EnableEdit = false;
		}

		private void UpdateHeroInfo() {
			if(Hero == null) {
				return;
			}
			updating = true;
			HeroImage.Source = new BitmapImage(new Uri(this.BaseUri, $"/HeroImages/{Hero.Name}.png"));
			HeroName.Text = Hero.GetFixedHeroName();

			HeroTypeImage.Source = new BitmapImage(new Uri(this.BaseUri, Hero.HeroType switch {
				HeroType.Strength => "/Icons/Strength_attribute_symbol.png",
				HeroType.Agility => "/Icons/Agility_attribute_symbol.png",
				HeroType.Intelligence => "/Icons/Intelligence_attribute_symbol.png",
				_ => "",
			}));

			AttackTypeBox.SelectedIndex = Hero.AttackType switch {
				AttackType.Ranged => 0,
				AttackType.Melee => 1,
				_ => 1,
			};

			HeroRoleBox.SelectedIndex = Hero.HeroRole switch {
				HeroRole.Core => 0,
				HeroRole.Support => 1,
				HeroRole.Both => 2,
				_ => 2,
			};

			switch(Hero.HeroDifficulty) {
				case HeroDifficulty.One:
					DifficultyOneToggle.IsChecked = true;
					DifficultyTwoToggle.IsChecked = false;
					DifficultyThreeToggle.IsChecked = false;
					break;
				case HeroDifficulty.Two:
					DifficultyOneToggle.IsChecked = true;
					DifficultyTwoToggle.IsChecked = true;
					DifficultyThreeToggle.IsChecked = false;
					break;
				case HeroDifficulty.Three:
					DifficultyOneToggle.IsChecked = true;
					DifficultyTwoToggle.IsChecked = true;
					DifficultyThreeToggle.IsChecked = true;
					break;
			}

			updating = false;
		}

		private async void DifficultyOneToggle_Click(object sender, RoutedEventArgs e) {
			if(updating || !EnableEdit) {
				return;
			}
			await UpdateDifficulty(HeroDifficulty.One);
		}

		private async void DifficultyTwoToggle_Click(object sender, RoutedEventArgs e) {
			if(updating || !EnableEdit) {
				return;
			}
			await UpdateDifficulty(HeroDifficulty.Two);
		}

		private async void DifficultyThreeToggle_Click(object sender, RoutedEventArgs e) {
			if(updating || !EnableEdit) {
				return;
			}
			await UpdateDifficulty(HeroDifficulty.Three);
		}

		private async Task UpdateDifficulty(HeroDifficulty difficulty) {
			DifficultyOneToggle.IsEnabled = false;
			DifficultyTwoToggle.IsEnabled = false;
			DifficultyThreeToggle.IsEnabled = false;

			Hero.HeroDifficulty = difficulty;

			UpdateHeroInfo();

			await LocalData.SaveHeroes();
			DifficultyOneToggle.IsEnabled = true;
			DifficultyTwoToggle.IsEnabled = true;
			DifficultyThreeToggle.IsEnabled = true;
		}

		private async void AttackTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			if(updating || !EnableEdit) {
				return;
			}

			AttackTypeBox.IsEnabled = false;

			if(AttackTypeBox.SelectedIndex == 0) {
				Hero.AttackType = AttackType.Ranged;
			} else {
				Hero.AttackType = AttackType.Melee;
			}

			UpdateHeroInfo();

			await LocalData.SaveHeroes();
			AttackTypeBox.IsEnabled = true;
		}

		private async void HeroRoleBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			if(updating || !EnableEdit) {
				return;
			}

			AttackTypeBox.IsEnabled = false;

			if(HeroRoleBox.SelectedIndex == 0) {
				Hero.HeroRole = HeroRole.Core;
			} else if(HeroRoleBox.SelectedIndex == 1) {
				Hero.HeroRole = HeroRole.Support;
			} else {
				Hero.HeroRole = HeroRole.Both;
			}

			UpdateHeroInfo();

			await LocalData.SaveHeroes();
			AttackTypeBox.IsEnabled = true;
		}

	}
}
