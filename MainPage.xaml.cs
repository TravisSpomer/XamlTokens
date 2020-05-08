using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

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

		/*
			Adds a rounded rectangle child visual to a UIElement with a specified color, and X and Y corner
			radii.  The radii will be constrained so that there are no corners "sharper" than a circle, as in the
			CSS border-radius property.  Corner radii of float.PositiveInfinity will produce a "pill" shape with
			two circles on the short ends, connected by a rectangle.

			<Rectangle x:Name="SpecialRect" Margin="50" Grid.Row="1" Grid.ColumnSpan="2" />
			AddPillVisuals(SpecialRect, Colors.Red, float.PositiveInfinity, float.PositiveInfinity);
		*/
		private void AddPillVisuals(UIElement element, Color color, float cornerX, float cornerY)
		{
			var hostVisual = ElementCompositionPreview.GetElementVisual(element);
			var compositor = Window.Current.Compositor;

			ExpressionAnimation bindWidthAnimation = compositor.CreateExpressionAnimation("hostVisual.Size.X");
			bindWidthAnimation.SetReferenceParameter("hostVisual", hostVisual);
			ExpressionAnimation bindHeightAnimation = compositor.CreateExpressionAnimation("hostVisual.Size.Y");
			bindHeightAnimation.SetReferenceParameter("hostVisual", hostVisual);
			ExpressionAnimation bindCornerXAnimation = compositor.CreateExpressionAnimation("min(cornerX, min(hostVisual.Size.X, hostVisual.Size.Y)) / 2");
			bindCornerXAnimation.SetReferenceParameter("hostVisual", hostVisual);
			bindCornerXAnimation.SetScalarParameter("cornerX", cornerX);
			ExpressionAnimation bindCornerYAnimation = compositor.CreateExpressionAnimation("min(cornerY, min(hostVisual.Size.X, hostVisual.Size.Y)) / 2");
			bindCornerYAnimation.SetReferenceParameter("hostVisual", hostVisual);
			bindCornerYAnimation.SetScalarParameter("cornerY", cornerY);

			var geometry = compositor.CreateRoundedRectangleGeometry();
			geometry.StartAnimation("CornerRadius.X", bindCornerXAnimation);
			geometry.StartAnimation("CornerRadius.Y", bindCornerYAnimation);
			geometry.StartAnimation("Size.X", bindWidthAnimation);
			geometry.StartAnimation("Size.Y", bindHeightAnimation);

			var spriteShape = compositor.CreateSpriteShape(geometry);
			spriteShape.FillBrush = compositor.CreateColorBrush(color);

			var shapeVisual = compositor.CreateShapeVisual();
			shapeVisual.Shapes.Add(spriteShape);
			shapeVisual.StartAnimation("Size.X", bindWidthAnimation);
			shapeVisual.StartAnimation("Size.Y", bindHeightAnimation);

			ElementCompositionPreview.SetElementChildVisual(element, shapeVisual);
		}
	}
}
