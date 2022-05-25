namespace Beng.Specta.Compta.Core.Objects.Identities;

[AttributeUsage(AttributeTargets.Field)]
public sealed class LinkedToModuleAttribute : Attribute
{
    public PaidForModule PaidForModule { get; private set; }

    public LinkedToModuleAttribute(PaidForModule paidForModule)
    {
        PaidForModule = paidForModule;
    }
}