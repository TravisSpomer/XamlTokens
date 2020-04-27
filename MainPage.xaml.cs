using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace XamlTokens
{
	public sealed partial class MainPage : Page
	{
		public MainPage()
		{
			InitializeComponent();

			SharedShadow.Receivers.Add(ShadowBackground);

			var window = Window.Current;
			window.SetTitleBar(TitleBar);
			var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
			coreTitleBar.ExtendViewIntoTitleBar = true;
			var titleBar = ApplicationView.GetForCurrentView().TitleBar;
			titleBar.BackgroundColor = Colors.Black;
			titleBar.ForegroundColor = Colors.White;
			titleBar.ButtonBackgroundColor = Colors.Transparent;
			titleBar.ButtonForegroundColor = Colors.White;
			titleBar.ButtonHoverBackgroundColor = Color.FromArgb(96, 255, 255, 255);
			titleBar.ButtonHoverForegroundColor = Colors.White;
			titleBar.ButtonPressedBackgroundColor = Color.FromArgb(64, 0, 0, 0);
			titleBar.ButtonPressedForegroundColor = Colors.White;
			titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
			titleBar.ButtonInactiveForegroundColor = Colors.White;

			window.Activated += Window_Activated;
		}

		private void Window_Activated(object sender, WindowActivatedEventArgs e)
		{
			switch (e.WindowActivationState)
			{
				case CoreWindowActivationState.CodeActivated:
				case CoreWindowActivationState.PointerActivated:
					TitleBar.Background = App.Current.Resources["GlobalColorAccent100"] as Brush;
					break;
				case CoreWindowActivationState.Deactivated:
					TitleBar.Background = App.Current.Resources["GlobalColorNeutral120"] as Brush;
					break;
			}
		}
	}
}
