namespace Beng.Specta.Compta.ComponentLibrary.Common;

public class OptionType
{
    public string Label { get; set; } = null!;
    public string Value { get; set; } = null!;
    public Action? OnSelectedItemHandler { get; set; }
}