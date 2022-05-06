using System;

namespace Beng.Specta.Compta.Client.State
{
    public class SpinnerState
	{
		public bool IsVisible { get; private set; }

		public event Action OnChange;

		public void ToggleSpinner()
		{
			IsVisible = !IsVisible;
			NotifyStateChanged();
		}

		private void NotifyStateChanged() => OnChange?.Invoke();
    }
}