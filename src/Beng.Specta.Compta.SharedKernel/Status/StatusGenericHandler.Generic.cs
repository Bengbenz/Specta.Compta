using Beng.Specta.Compta.SharedKernel.Interfaces;

namespace Beng.Specta.Compta.SharedKernel.Status;

/// <summary>
/// This contains the error hanlding part of the GenericBizRunner
/// </summary>
public class StatusGenericHandler<T> : StatusGenericHandler, IStatusGeneric<T>
{
    private T? _result;

    /// <summary>
    /// This is the returned result
    /// </summary>
    public T? Result => IsValid ? _result : default;

    /// <summary>
    /// This sets the result to be returned
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public StatusGenericHandler<T> SetResult(T result)
    {
        _result = result;
        return this;
    }
}