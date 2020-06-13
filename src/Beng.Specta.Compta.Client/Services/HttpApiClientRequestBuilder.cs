using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using Beng.Specta.Compta.ComponentLibrary.Common;

namespace Beng.Specta.Compta.Client.Services
{
    public class HttpApiClientRequestBuilder : IHttpApiClientRequestBuilder
    {
        private string _uri;
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigationManager;
        private readonly IJSRuntime _jsRuntime;
        
        private Func<HttpResponseMessage, Task> _onBadRequest;
        private Func<HttpResponseMessage, Task> _onNotFound;
        private Func<HttpResponseMessage, Task> _onOK;

        // private readonly IBrowserCookieService _browserCookieService;
        // private readonly IMessageService _messageService;

        public HttpApiClientRequestBuilder(
            HttpClient httpClient,
            NavigationManager navigationManager,
            IJSRuntime jsRuntime)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _navigationManager = navigationManager ?? throw new ArgumentNullException(nameof(navigationManager));
            _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
        }

        private void RedirectToLocal(string navigateTo)
        {
            if (!string.IsNullOrEmpty(navigateTo))
            {
                _navigationManager.NavigateTo(navigateTo);
            }
            else
            {
                _navigationManager.NavigateTo("/");
            }
        }

        private async Task ExecuteHttpQuery(Func<Task<HttpResponseMessage>> httpCall)
        {
            var response = await httpCall();

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                {
                    if (_onOK != null)
                    {
                        await _onOK(response);
                    }
                    break;
                }
                case HttpStatusCode.BadRequest:
                {
                    if (_onBadRequest != null)
                    {
                        await _onBadRequest(response);
                    }
                    break;
                }
                case HttpStatusCode.NotFound:
                {
                    if (_onNotFound != null)
                    {
                        await _onNotFound(response);
                    }
                    break;
                }
                case HttpStatusCode.InternalServerError:
                {
                    await _jsRuntime.LaunchToast("error", "A server error occured, sorry");
                    break;
                }
                default:
                {
                    // case HttpStatusCode.Unauthorized:
                    // case HttpStatusCode.Forbidden:
                    // other case , we do nothing, I'll add this case as needed
                    break;
                }
            }
        }

        public async Task GetAsync(object data = null)
        {
            if (data != null)
            {
                _uri += $"/{data}";
            }
            await ExecuteHttpQuery(async () => await _httpClient.GetAsync(_uri));
        }

        public async Task PostAsync<T>(T data = default)
        {
            // await SetCaptchaToken(data);
            await ExecuteHttpQuery(async () => await _httpClient.PostAsJsonAsync(_uri, data));
        }

        public async Task PostAsync()
        {
            // await SetCaptchaToken(data);
            await ExecuteHttpQuery(async () => await _httpClient.PostAsync(_uri, null));
        }

        public IHttpApiClientRequestBuilder CreateRequest(string link)
        {
            _uri = link ?? throw new ArgumentNullException(nameof(link));
            return this;
        }
        
        public IHttpApiClientRequestBuilder OnOK<T>(Action<T> action)
        {
            _onOK = async (HttpResponseMessage r) =>
            {
                T response = await JsonSerializer.DeserializeAsync<T>(await r.Content.ReadAsStreamAsync());
                action(response);
            };
            return this;
        }

        public IHttpApiClientRequestBuilder OnOK(Func<Task> func)
        {
            _onOK = async (HttpResponseMessage r) =>
            {
                await func();
            };
            return this;
        }

        public IHttpApiClientRequestBuilder OnOK(Action action)
        {
            _onOK = (HttpResponseMessage r) =>
            {
                action();
                return Task.CompletedTask;
            };
            return this;
        }

        public IHttpApiClientRequestBuilder OnOK(string successMessage = null, string navigateTo = null)
        {
            OnOK(() =>
            {
                if (!string.IsNullOrEmpty(successMessage))
                {
                    _jsRuntime.LaunchToast("success", successMessage);
                }

                RedirectToLocal(navigateTo);
            });
            return this;
        }

        public IHttpApiClientRequestBuilder OnBadRequest(Func<Task> func)
        {
            _onBadRequest = async (HttpResponseMessage r) =>
            {
                await func();
            };
            return this;
        }

        public IHttpApiClientRequestBuilder OnBadRequest<T>(Action<T> action)
        {
            _onBadRequest = async (HttpResponseMessage r) =>
            {
                T response = await JsonSerializer.DeserializeAsync<T>(await r.Content.ReadAsStreamAsync());
                action(response);
            };
            return this;
        }

        public IHttpApiClientRequestBuilder OnBadRequest(Action action)
        {
            _onBadRequest = (HttpResponseMessage r) =>
            {
                action();
                return Task.CompletedTask;
            };
            return this;
        }

         public IHttpApiClientRequestBuilder OnNotFound<T>(Action<T> action)
        {
            _onNotFound = async (HttpResponseMessage r) =>
            {
                T response = await JsonSerializer.DeserializeAsync<T>(await r.Content.ReadAsStreamAsync());
                action(response);
            };
            return this;
        }

        public IHttpApiClientRequestBuilder OnNotFound(Action action)
        {
            _onNotFound= (HttpResponseMessage r) =>
            {
                action();
                return Task.CompletedTask;
            };
            return this;
        }
    }
}