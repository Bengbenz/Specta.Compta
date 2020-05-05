using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Beng.Specta.Compta.Client.State;

namespace Beng.Specta.Compta.Client
{
    public class DisplaySpinnerAutomaticallyHttpMessageHandler : DelegatingHandler
    {
        private readonly SpinnerState _spinnerState;

        public DisplaySpinnerAutomaticallyHttpMessageHandler(SpinnerState spinnerService)
        {
            _spinnerState  = spinnerService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            _spinnerState.ToggleSpinner(); // Show
            var response = await base.SendAsync(request, cancellationToken);
            _spinnerState.ToggleSpinner(); // Hide
            return response;            
        }
    }
}