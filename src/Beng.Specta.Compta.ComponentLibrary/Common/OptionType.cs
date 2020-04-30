using System;

namespace Beng.Specta.Compta.ComponentLibrary.Common
{
    public class OptionType
    {
        public string Label { get; set; }
        public string Value { get; set; }
        public Action OnSelectedItemHandler { get; set; }
    }
}