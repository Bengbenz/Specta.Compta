using System;
using System.Threading.Tasks;

namespace Beng.Specta.Compta.Client.Services
{
    public interface IHttpApiClientRequestBuilder
    {
        Task GetAsync(object data = null);
        
        Task PostAsync<T>(T data = default);

        Task PostAsync();

        IHttpApiClientRequestBuilder CreateRequest(string link);

        IHttpApiClientRequestBuilder OnOK<T>(Action<T> action);

        IHttpApiClientRequestBuilder OnOK(Func<Task> func);

        IHttpApiClientRequestBuilder OnOK(Action action);

        IHttpApiClientRequestBuilder OnOK(string successMessage = null, string navigateTo = null);

        IHttpApiClientRequestBuilder OnBadRequest(Func<Task> func);

        IHttpApiClientRequestBuilder OnBadRequest<T>(Action<T> action);

        IHttpApiClientRequestBuilder OnBadRequest(Action action);

        IHttpApiClientRequestBuilder OnNotFound<T>(Action<T> action);

        IHttpApiClientRequestBuilder OnNotFound(Action action);
    }
}