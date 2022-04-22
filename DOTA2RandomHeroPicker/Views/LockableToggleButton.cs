using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOTA2RandomHeroPicker.Views {
	public class LockableToggleButton: ToggleButton {
		public static readonly DependencyProperty LockToggleProperty = DependencyProperty.Register("LockToggle", typeof(bool), typeof(LockableToggleButton), new PropertyMetadata(false));

		protected override void OnToggle() {
			if(!LockToggle) {
				base.OnToggle();
			}
		}

		public bool LockToggle {
			get => (bool)GetValue(LockToggleProperty);
			set {
				SetValue(LockToggleProperty, value);
			}
		}

	}
}
