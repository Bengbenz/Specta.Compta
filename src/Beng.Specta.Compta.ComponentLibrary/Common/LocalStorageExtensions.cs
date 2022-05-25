using Microsoft.JSInterop;

namespace Beng.Specta.Compta.ComponentLibrary.Common;

public static class LocalStorageExtensions
{
    public static ValueTask<T> GetAsync<T>(this IJSRuntime jsRuntime, string key)
        => jsRuntime.InvokeAsync<T>("blazorLocalStorage.get", key);

    public static ValueTask SetAsync(this IJSRuntime jsRuntime, string key, object value)
        => jsRuntime.InvokeVoidAsync("blazorLocalStorage.set", key, value);

    public static ValueTask DeleteAsync(this IJSRuntime jsRuntime, string key)
        => jsRuntime.InvokeVoidAsync("blazorLocalStorage.delete", key);
}