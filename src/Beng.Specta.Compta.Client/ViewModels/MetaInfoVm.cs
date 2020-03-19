namespace Beng.Specta.Compta.Client.ViewModels
{
    public class MetaInfoVm
    {
        public MetaInfoVm(string url, string iconClass = "")
        {
            Url = url;
            IconClass = iconClass;
        }

        public string Url { get; set; }
        public string IconClass { get; set; }
        
    }
}