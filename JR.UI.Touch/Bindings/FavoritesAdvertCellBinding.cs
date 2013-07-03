using System;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Bindings.Target;

namespace JR.UI.Touch.Bindings
{
	public class FavoritesAdvertCellBinding
		: MvxTargetBinding
	{
		private AdvertCell2 Cell {
			get {
				return Target as AdvertCell2;
			}
		}
		private bool _currentValue;

		public FavoritesAdvertCellBinding(AdvertCell2 cell)
			: base(cell)
		{
			cell.PublicFavoritesButtonPressed += HandlePublicFavoritesButtonPressed;
		}

		protected override void Dispose(bool isDisposing)
		{
			if (isDisposing)
			{
				var cell = Cell;
				if (cell != null)
					cell.PublicFavoritesButtonPressed -= HandlePublicFavoritesButtonPressed;
			}

			base.Dispose(isDisposing);
		}

		void HandlePublicFavoritesButtonPressed(object sender, EventArgs e)
		{
			_currentValue = !_currentValue;
			FireValueChanged(_currentValue);
		}

		public override Type TargetType
		{
			get { return typeof(bool); }
		}

		public override MvxBindingMode DefaultMode
		{
			get { return MvxBindingMode.TwoWay; }
		}

		public override void SetValue(object value)
		{
			var cell = Cell;
			if (cell == null)
				return;

			_currentValue = (bool)value;
			var button = cell.PublicFavoritesButton;
			FavoritesButtonBinding.SetButtonBackground(button, _currentValue);
		}
	}
}