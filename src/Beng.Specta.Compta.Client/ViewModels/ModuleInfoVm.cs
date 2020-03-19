using System.Collections.Generic;

namespace Beng.Specta.Compta.Client.ViewModels
{
    public class ModuleInfoVm
    {
        public ModuleInfoVm(string id, string displayName, MetaInfoVm meta)
        {
            Id = id;
            DisplayName = displayName;
            Meta = meta;
        }

        public string Id { get; set; }
        public string DisplayName { get; set; }
        public MetaInfoVm Meta { get; set; }
        public IReadOnlyCollection<ModuleInfoVm> Children { get; set; } = new List<ModuleInfoVm>();
    }
}