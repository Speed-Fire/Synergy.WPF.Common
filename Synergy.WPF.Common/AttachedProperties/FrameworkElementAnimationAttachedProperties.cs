using Synergy.WPF.Common.Animations;
using System.Collections.Generic;
using System;
using System.Windows;
using System.Linq;
using System.Threading.Tasks;

namespace Synergy.WPF.Common.AttachedProperties
{
	/// <summary>
	/// A base class to run an animation method when the boolean is set to true
	/// and to reverse animation when set to false.
	/// </summary>
	/// <typeparam name="Parent"></typeparam>
	public abstract class AnimateBaseProperty<Parent> : BaseAttachedProperty<Parent, bool>
		where Parent : BaseAttachedProperty<Parent, bool>, new()
	{
		#region Properties

		/// <summary>
		/// True if this is the very first time the value has been updated
		/// Used to make sure we run the logic at least once during first load
		/// </summary>
		protected Dictionary<WeakReference, bool> mAlreadyLoaded = new();

		/// <summary>
		/// The most recent value used if we get a value changed before we do the first load
		/// </summary>
		protected Dictionary<WeakReference, bool> mFirstLoadValue = new();

		#endregion

		public override void OnValueUpdated(DependencyObject sender, object value)
		{
			// Get the framework element
			if (sender is not FrameworkElement element)
				return;

			// Try and get the already loaded reference
			var alreadyLoadedReference = mAlreadyLoaded.FirstOrDefault(f => f.Key.Target == sender);

			// Try and get the first load reference
			var firstLoadReference = mFirstLoadValue.FirstOrDefault(f => f.Key.Target == sender);

			// Don't run if property is not changed, except of first start
			if ((bool)sender.GetValue(ValueProperty) == (bool)value && alreadyLoadedReference.Key != null)
				return;

			// On first load...
			if (alreadyLoadedReference.Key == null)
			{
				// Create weak reference
				var weakReference = new WeakReference(sender);

				// Flag that we are in first load but have not finished it
				mAlreadyLoaded[weakReference] = false;

				// Start off hidden before we decide how to animate
				//element.Visibility = Visibility.Hidden;

				// Create a single self-unhookable event
				// for the elements Loaded event
				RoutedEventHandler onLoaded = null;
				onLoaded = async (sender, e) =>
				{
					// Unhook ourselves
					element.Loaded -= onLoaded;

					// Slight delay after load is needed for some elements to get laid out
					// and their width/heights correctly calculated
					await Task.Delay(5);

					// Refresh the first load value in case it changed
					// since the 5ms delay
					firstLoadReference = mFirstLoadValue.FirstOrDefault(f => f.Key.Target == sender);

					// Do desired animation
					DoAnimation(element, firstLoadReference.Key != null ? firstLoadReference.Value : (bool)value, true);

					// Flag that we have finished first load
					mAlreadyLoaded[weakReference] = true;
				};

				// Hook into the Loaded event of the element
				element.Loaded += onLoaded;
			}
			// If we have started a first load but not fired the animation yet, update the property
			else if (alreadyLoadedReference.Value == false)
				mFirstLoadValue[new WeakReference(sender)] = (bool)value;
			else
				// Do desired animation
				DoAnimation(element, (bool)value, false);
		}

		/// <summary>
		/// The animation method that is fired when the value changes.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="value">The new value.</param>
		protected virtual void DoAnimation(FrameworkElement element, bool value, bool firstLoad) { }
	}

	/// <summary>
	/// Animate a framework element through events.
	/// </summary>
	public class AnimateFireEventsProperty : AnimateBaseProperty<AnimateFireEventsProperty>
	{
		#region Routed events

		#region Animating

		// Register a custom routed event using the bubble routing strategy.
		public static readonly RoutedEvent AnimatingEvent = EventManager.RegisterRoutedEvent(
			"Animating", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AnimateFireEventsProperty));

		// Provide an add handler accessor method for the Animating event.
		public static void AddAnimatingHandler(DependencyObject dependencyObject, RoutedEventHandler handler)
		{
			if (dependencyObject is not UIElement uiElement)
				return;

			uiElement.AddHandler(AnimatingEvent, handler);
		}

		// Provide a remove handler accessor method for the Animating event.
		public static void RemoveAnimatingHandler(DependencyObject dependencyObject, RoutedEventHandler handler)
		{
			if (dependencyObject is not UIElement uiElement)
				return;

			uiElement.RemoveHandler(AnimatingEvent, handler);
		}

		#endregion

		#region AnimatingBack

		// Register a custom routed event using the bubble routing strategy.
		public static readonly RoutedEvent AnimatingBackEvent = EventManager.RegisterRoutedEvent(
			"AnimatingBack", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AnimateFireEventsProperty));

		// Provide an add handler accessor method for the AnimatingBack event.
		public static void AddAnimatingBackHandler(DependencyObject dependencyObject, RoutedEventHandler handler)
		{
			if (dependencyObject is not UIElement uiElement)
				return;

			uiElement.AddHandler(AnimatingBackEvent, handler);
		}

		// Provide a remove handler accessor method for the AnimatingBack event.
		public static void RemoveAnimatingBackHandler(DependencyObject dependencyObject, RoutedEventHandler handler)
		{
			if (dependencyObject is not UIElement uiElement)
				return;

			uiElement.RemoveHandler(AnimatingBackEvent, handler);
		}

		#endregion

		#endregion

		protected override void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
		{
			if (value)
				element.RaiseEvent(new RoutedEventArgs(AnimatingEvent, firstLoad));
			else
				element.RaiseEvent(new RoutedEventArgs(AnimatingBackEvent, firstLoad));
		}
	}

	/// <summary>
	/// Fades in an image once the source changes
	/// </summary>
	public class FadeInImageOnLoadProperty : AnimateBaseProperty<FadeInImageOnLoadProperty>
	{
		public override void OnValueUpdated(DependencyObject sender, object value)
		{
			// Make sure we have an image
			if (sender is not System.Windows.Controls.Image image)
				return;

			// If we want to animate in...
			if ((bool)value)
				// Listen for target change
				image.TargetUpdated += Image_TargetUpdatedAsync;
			// Otherwise
			else
				// Make sure we unhooked
				image.TargetUpdated -= Image_TargetUpdatedAsync;
		}

		private async void Image_TargetUpdatedAsync(object sender, System.Windows.Data.DataTransferEventArgs e)
		{
			// Fade in image
			await (sender as System.Windows.Controls.Image).FadeInAsync(false);
		}
	}

	#region Sliding animations

	/// <summary>
	/// Animate a framework element sliding it in from the left on show
	/// and sliding out to the left on hide.
	/// </summary>
	public class AnimateSlideInFromLeftProperty
		: AnimateBaseProperty<AnimateSlideInFromLeftProperty>
	{
		protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
		{
			if (value)
				// Animate in
				await element.SlideAndFadeInAsync(AnimationSlideInDirection.Left, firstLoad, firstLoad ? 0 : 0.3f, fading: false);
			else
				// Animate out
				await element.SlideAndFadeOutAsync(AnimationSlideInDirection.Left, firstLoad, firstLoad ? 0 : 0.3f, fading: false);
		}
	}

	/// <summary>
	/// Animate a framework element sliding it in from the left on show
	/// and sliding out to the left on hide.
	/// </summary>
	public class AnimateSlideAndFadeInFromLeftProperty
		: AnimateBaseProperty<AnimateSlideAndFadeInFromLeftProperty>
	{
		protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
		{
			if (value)
				// Animate in
				await element.SlideAndFadeInAsync(AnimationSlideInDirection.Left, firstLoad, firstLoad ? 0 : 0.3f);
			else
				// Animate out
				await element.SlideAndFadeOutAsync(AnimationSlideInDirection.Left, firstLoad, firstLoad ? 0 : 0.3f);
		}
	}

	/// <summary>
	/// Animate a framework element sliding it in from the right on show
	/// and sliding out to the right on hide.
	/// </summary>
	public class AnimateSlideInFromRightProperty
		: AnimateBaseProperty<AnimateSlideInFromRightProperty>
	{
		protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
		{
			if (value)
				// Animate in
				await element.SlideAndFadeInAsync(AnimationSlideInDirection.Right, firstLoad, firstLoad ? 0 : 0.3f, fading: false);
			else
				// Animate out
				await element.SlideAndFadeOutAsync(AnimationSlideInDirection.Right, firstLoad, firstLoad ? 0 : 0.3f, fading: false);
		}
	}

	/// <summary>
	/// Animate a framework element sliding it in from the right on show
	/// and sliding out to the right on hide.
	/// </summary>
	public class AnimateSlideAndFadeInFromRightProperty
		: AnimateBaseProperty<AnimateSlideAndFadeInFromRightProperty>
	{
		protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
		{
			if (value)
				// Animate in
				await element.SlideAndFadeInAsync(AnimationSlideInDirection.Right, firstLoad, firstLoad ? 0 : 0.3f);
			else
				// Animate out
				await element.SlideAndFadeOutAsync(AnimationSlideInDirection.Right, firstLoad, firstLoad ? 0 : 0.3f);
		}
	}

	/// <summary>
	/// Animate a framework element sliding it in from the top on show
	/// and sliding out to the top on hide.
	/// </summary>
	public class AnimateSlideInFromTopProperty
		: AnimateBaseProperty<AnimateSlideInFromTopProperty>
	{
		protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
		{
			if (value)
				// Animate in
				await element.SlideAndFadeInAsync(AnimationSlideInDirection.Top, firstLoad, firstLoad ? 0 : 0.3f, fading: false);
			else
				// Animate out
				await element.SlideAndFadeOutAsync(AnimationSlideInDirection.Top, firstLoad, firstLoad ? 0 : 0.3f, fading: false);
		}
	}

	/// <summary>
	/// Animate a framework element sliding it in from the top on show
	/// and sliding out to the top on hide.
	/// </summary>
	public class AnimateSlideAndFadeInFromTopProperty
		: AnimateBaseProperty<AnimateSlideAndFadeInFromTopProperty>
	{
		protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
		{
			if (value)
				// Animate in
				await element.SlideAndFadeInAsync(AnimationSlideInDirection.Top, firstLoad, firstLoad ? 0 : 0.3f);
			else
				// Animate out
				await element.SlideAndFadeOutAsync(AnimationSlideInDirection.Top, firstLoad, firstLoad ? 0 : 0.3f);
		}
	}

	/// <summary>
	/// Animate a framework element sliding it in from the bottom on show
	/// and sliding out to the bottom on hide.
	/// </summary>
	public class AnimateSlideInFromBottomProperty
		: AnimateBaseProperty<AnimateSlideInFromBottomProperty>
	{
		protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
		{
			if (value)
				// Animate in
				await element.SlideAndFadeInAsync(AnimationSlideInDirection.Bottom, firstLoad, firstLoad ? 0 : 0.3f, fading: false);
			else
				// Animate out
				await element.SlideAndFadeOutAsync(AnimationSlideInDirection.Bottom, firstLoad, firstLoad ? 0 : 0.3f, fading: false);
		}
	}

	/// <summary>
	/// Animate a framework element sliding it in from the bottom on show
	/// and sliding out to the bottom on hide.
	/// </summary>
	public class AnimateSlideAndFadeInFromBottomProperty
		: AnimateBaseProperty<AnimateSlideAndFadeInFromBottomProperty>
	{
		protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
		{
			if (value)
				// Animate in
				await element.SlideAndFadeInAsync(AnimationSlideInDirection.Bottom, firstLoad, firstLoad ? 0 : 0.3f);
			else
				// Animate out
				await element.SlideAndFadeOutAsync(AnimationSlideInDirection.Bottom, firstLoad, firstLoad ? 0 : 0.3f);
		}
	}

	/// <summary>
	/// Animates a framework element sliding up from the bottom on load
	/// if the value is true
	/// </summary>
	public class AnimateSlideInFromBottomOnLoadProperty : AnimateBaseProperty<AnimateSlideInFromBottomOnLoadProperty>
	{
		protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
		{
			// Animate in
			await element.SlideAndFadeInAsync(AnimationSlideInDirection.Bottom, !value, !value ? 0 : 0.3f);
		}
	}

	/// <summary>
	/// Animates a framework element fading in on show
	/// and fading out on hide
	/// </summary>
	public class AnimateFadeInProperty : AnimateBaseProperty<AnimateFadeInProperty>
	{
		protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
		{
			if (value)
				// Animate in
				await element.FadeInAsync(firstLoad, firstLoad ? 0 : 0.3f);
			else
				// Animate out
				await element.FadeOutAsync(firstLoad ? 0 : 0.3f);
		}
	}

	/// <summary>
	/// Animates a framework element sliding it from right to left and repeating forever
	/// </summary>
	public class AnimateMarqueeProperty : AnimateBaseProperty<AnimateMarqueeProperty>
	{
		protected override void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
		{
			// Animate in
			element.MarqueeAsync(firstLoad ? 0 : 3f);
		}
	}

	#endregion
}
