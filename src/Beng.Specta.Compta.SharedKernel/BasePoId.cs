namespace Beng.Specta.Compta.SharedKernel;

public abstract class BasePoId<TId> : ValueObject
{
    public TId Value { get; private set; }

    protected BasePoId(TId value)
    {
        Value = value;
    }
}