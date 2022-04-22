using DOTA2RandomHeroPicker.Data;
using DOTA2RandomHeroPicker.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace DOTA2RandomHeroPicker {
	public sealed partial class MainWindow: Window {
		public MainWindow() {
			this.InitializeComponent();
			ExtendsContentIntoTitleBar = true;
			SetTitleBar(AppTitleBar);
		}

		private void MainNavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args) {
			if(args.InvokedItemContainer is NavigationViewItem item && item.IsSelected) {
				return;
			}
			Type type = null;
			if(args.IsSettingsInvoked) {
				type = typeof(SettingsPage);
			} else if(HomeItem == args.InvokedItemContainer as NavigationViewItem) {
				type = typeof(HomePage);
			} else if(WeightItem == args.InvokedItemContainer as NavigationViewItem) {
				type = typeof(WeightPage);
			}
			if(type == null) {
				return;
			}
			MainFrame.Navigate(type, null, new EntranceNavigationTransitionInfo());
		}

		private void MainFrame_Navigated(object sender, NavigationEventArgs e) {
			MainNavigationView.IsBackEnabled = MainFrame.CanGoBack;
		}

		private void MainNavigationView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args) {
			if(!MainFrame.CanGoBack) {
				return;
			}
			MainFrame.GoBack();

			Type type = MainFrame.Content.GetType();
			if(type == typeof(HomePage)) {
				MainNavigationView.SelectedItem = HomeItem;
			} else if(type == typeof(WeightPage)) {
				MainNavigationView.SelectedItem = WeightItem;
			} else if(type == typeof(SettingsPage)) {
				MainNavigationView.SelectedItem = MainNavigationView.SettingsItem;
			}
		}

		private async void Grid_Loaded(object sender, RoutedEventArgs e) {
			MainNavigationView.IsEnabled = false;
			MainFrame.Navigate(typeof(InitializingPage), null, new EntranceNavigationTransitionInfo());
			await LocalData.Initialize();
			MainFrame.Navigate(typeof(HomePage), null, new EntranceNavigationTransitionInfo());
			MainFrame.BackStack.Clear();
			MainNavigationView.IsEnabled = true;
			MainNavigationView.SelectedItem = HomeItem;
			MainNavigationView.IsBackEnabled = false;
		}
	}
}
