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
    public class HttpWebApiClient
    {
        private string _uri;
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigationManager;
        private readonly IJSRuntime _jsRuntime;
        
        private Func<HttpResponseMessage, Task> _onBadRequest;
        private Func<HttpResponseMessage, Task> _onNotFound;
        private Func<HttpResponseMessage, Task> _onOk;

        // private readonly IBrowserCookieService _browserCookieService;
        // private readonly IMessageService _messageService;

        public HttpWebApiClient(
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
                case HttpStatusCode.Created:
                {
                    if (_onOk != null)
                    {
                        await _onOk(response);
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

        public HttpWebApiClient CreateRequest(string link)
        {
            _uri = link ?? throw new ArgumentNullException(nameof(link));
            return this;
        }
        
        public HttpWebApiClient OnOK<T>(Action<T> action)
        {
            _onOk = async (HttpResponseMessage r) =>
            {
                T response = await JsonSerializer.DeserializeAsync<T>(await r.Content.ReadAsStreamAsync());
                action(response);
            };
            return this;
        }

        public HttpWebApiClient OnOK(Func<Task> func)
        {
            _onOk = async (HttpResponseMessage r) =>
            {
                await func();
            };
            return this;
        }

        public HttpWebApiClient OnOK(Action action)
        {
            _onOk = (HttpResponseMessage r) =>
            {
                action();
                return Task.CompletedTask;
            };
            return this;
        }

        public HttpWebApiClient OnOK(string successMessage = null, string navigateTo = null)
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

        public HttpWebApiClient OnBadRequest(Func<Task> func)
        {
            _onBadRequest = async (r) =>
            {
                await func();
            };
            return this;
        }

        public HttpWebApiClient OnBadRequest<T>(Action<T> action)
        {
            _onBadRequest = async (r) =>
            {
                T response = await JsonSerializer.DeserializeAsync<T>(await r.Content.ReadAsStreamAsync());
                action(response);
            };
            return this;
        }

        public HttpWebApiClient OnBadRequest(Action action)
        {
            _onBadRequest = (r) =>
            {
                action();
                return Task.CompletedTask;
            };
            return this;
        }

         public HttpWebApiClient OnNotFound<T>(Action<T> action)
        {
            _onNotFound = async (r) =>
            {
                T response = await JsonSerializer.DeserializeAsync<T>(await r.Content.ReadAsStreamAsync());
                action(response);
            };
            return this;
        }

        public HttpWebApiClient OnNotFound(Action action)
        {
            _onNotFound= (r) =>
            {
                action();
                return Task.CompletedTask;
            };
            return this;
        }
    }
}