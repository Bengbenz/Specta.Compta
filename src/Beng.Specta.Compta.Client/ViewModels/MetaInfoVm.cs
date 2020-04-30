using System;

namespace Beng.Specta.Compta.Client.ViewModels
{
    public class MetaInfoVm
    {
        public MetaInfoVm(string link, string iconClass = "")
        {
            Url = new Uri(link, UriKind.Relative);
            IconClass = iconClass;
        }

        public Uri Url { get; set; }
        public string IconClass { get; set; }
        
    }
}