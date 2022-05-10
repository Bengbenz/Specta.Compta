namespace Beng.Specta.Compta.FunctionalTests;

/// <summary>
/// A class representing the options to use with <see cref="FunctionalTestingWebApplicationFactory"/>.
/// </summary>
public class BrowserFixtureOptions
{
    /// <summary>
    /// Gets or sets the browser type.
    /// </summary>
    public string BrowserType { get; set; }

    /// <summary>
    /// Gets or sets the optional browser channel.
    /// </summary>
    public string BrowserChannel { get; set; }

    /// <summary>
    /// Gets or sets the optional build number.
    /// </summary>
    public string Build { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to capture traces.
    /// </summary>
    /// <remarks>
    /// Take care to only enable traces in publicly accessible
    /// repositories if your tests do not use sensitive information,
    /// such as user credentials, in interactions with the browser.
    /// Otherwise your traces might leak secrets to unauthorized third parties.
    /// </remarks>
    public bool ShouldCaptureTrace { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to capture video.
    /// </summary>
    public bool ShouldCaptureVideo { get; set; }

    /// <summary>
    /// Gets or sets the optional operating system name.
    /// </summary>
    public string OperatingSystem { get; set; }

    /// <summary>
    /// Gets or sets the optional operating system version.
    /// </summary>
    public string OperatingSystemVersion { get; set; }

    /// <summary>
    /// Gets or sets the optional Playwright version number.
    /// </summary>
    public string PlaywrightVersion { get; set; }

    /// <summary>
    /// Gets or sets the optional project name.
    /// </summary>
    public string ProjectName { get; set; }

    /// <summary>
    /// Gets or sets the optional test name.
    /// </summary>
    public string TestName { get; set; }

    /// <summary>
    /// Gets or sets the headless param
    /// </summary>
    public bool Headless { get; set; }
    
    /// <summary>
    /// Gets or sets the Slow Motion param
    /// </summary>
    public int SlowMo { get; set; }
}