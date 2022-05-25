namespace Beng.Specta.Compta.SharedKernel;

public abstract class BaseDomainEvent
{
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}