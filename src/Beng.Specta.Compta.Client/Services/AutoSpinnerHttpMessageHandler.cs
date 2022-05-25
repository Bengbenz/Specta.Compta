using Beng.Specta.Compta.Client.State;

namespace Beng.Specta.Compta.Client.Services;

public class AutoSpinnerHttpMessageHandler : DelegatingHandler
{
    private readonly SpinnerState _spinnerState;

    public AutoSpinnerHttpMessageHandler(SpinnerState spinnerState)
    {
        _spinnerState  = spinnerState;
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