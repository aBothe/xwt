using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xwt;

namespace Samples
{
	public class KeyEvents : VBox
	{
		const string defaultText = "Press a key!";
		Label label = new Label(defaultText);

		public KeyEvents()
		{
			PackStart(label, BoxMode.FillAndExpand);
		}

		protected override void OnKeyPressed(KeyEventArgs args)
		{
			label.Text = args.Key.ToString();
			base.OnKeyPressed(args);
		}

		protected override void OnKeyReleased(KeyEventArgs args)
		{
			label.Text = defaultText;
			base.OnKeyReleased(args);
		}
	}
}
