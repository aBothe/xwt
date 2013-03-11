﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xwt.Backends;
using System.Windows;
using SWC = System.Windows.Controls;

namespace Xwt.WPFBackend
{
	class CanvasBackend
		: WidgetBackend, ICanvasBackend
	{
		#region ICanvasBackend Members

		public CanvasBackend ()
		{
			Canvas = new ExCanvas ();
			Canvas.RenderAction = Render;
		}

		private ExCanvas Canvas
		{
			get { return (ExCanvas) Widget; }
			set { Widget = value; }
		}

		private ICanvasEventSink CanvasEventSink
		{
			get { return (ICanvasEventSink) EventSink; }
		}

		private void Render (System.Windows.Media.DrawingContext dc)
		{
			CanvasEventSink.OnDraw (new Xwt.WPFBackend.DrawingContext (Widget, dc), new Rectangle (0, 0, Widget.ActualWidth, Widget.ActualHeight));
		}

		public void QueueDraw ()
		{
			Canvas.InvalidateVisual ();
		}

		public void QueueDraw (Rectangle rect)
		{
			Canvas.InvalidateVisual ();
		}

		public void AddChild (IWidgetBackend widget, Rectangle bounds)
		{
			UIElement element = widget.NativeWidget as UIElement;
			if (element == null)
				throw new ArgumentException ();

			if (!Canvas.Children.Contains (element))
				Canvas.Children.Add (element);

			SetChildBounds (widget, bounds);
		}

		public void SetChildBounds (IWidgetBackend widget, Rectangle bounds)
		{
			FrameworkElement element = widget.NativeWidget as FrameworkElement;
			if (element == null)
				throw new ArgumentException ();

			double hratio = HeightPixelRatio;
			double wratio = WidthPixelRatio;

			SWC.Canvas.SetTop (element, bounds.Top * hratio);
			SWC.Canvas.SetLeft (element, bounds.Left * wratio);

			// We substract the widget margin here because the size we are assigning is the actual size, not including the WPF marings
			var h = bounds.Height - ((WidgetBackend)widget).Frontend.Margin.VerticalSpacing;
			var w = bounds.Width - ((WidgetBackend)widget).Frontend.Margin.HorizontalSpacing;

			element.Height = (h > 0) ? h * hratio : 0;
			element.Width = (w > 0) ? w * wratio : 0;

			((FrameworkElement)widget.NativeWidget).UpdateLayout ();
		}

		public void RemoveChild (IWidgetBackend widget)
		{
			UIElement element = widget.NativeWidget as UIElement;
			if (element == null)
				throw new ArgumentException ();

			Canvas.Children.Remove (element);
		}

		#endregion
	}
}
