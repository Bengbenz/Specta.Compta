namespace Beng.Specta.Compta.SharedKernel
{
    public abstract class BasePOId<TId> : ValueObject
    {
        public TId Value { get; private set; }

        protected BasePOId(TId value)
        {
            Value = value;
        }
    }
}
