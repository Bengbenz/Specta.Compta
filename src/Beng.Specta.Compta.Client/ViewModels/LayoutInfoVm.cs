namespace Beng.Specta.Compta.Client.ViewModels
{
    public class LayoutInfoVm
    {
        public bool IsMinimized { get; set; }
        public int MobileWidth { get; set; } = 767;

        public void ToggleMinimized()
        {
            IsMinimized = !IsMinimized;
        }
    }
}