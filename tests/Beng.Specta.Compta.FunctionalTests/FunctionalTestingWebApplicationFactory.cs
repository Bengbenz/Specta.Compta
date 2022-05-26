using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using Beng.Specta.Compta.IntegrationTests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Playwright;
using Xunit.Abstractions;

namespace Beng.Specta.Compta.FunctionalTests;

/// <inheritdoc />
public sealed class FunctionalTestingWebApplicationFactory : IntegrationTestingWebApplicationFactory
{
    private ITestOutputHelper? _outputHelper;
    private ITestOutputHelper OutputHelper => _outputHelper ?? throw new InvalidOperationException($"The {nameof(OutputHelper)} is not setted. call {nameof(WithTestOutputHelper)} before.");

    private BrowserFixtureOptions _options = new()
    {
        Headless = true,
        SlowMo = 100,
        ShouldCaptureTrace = true,
        TimeoutAsMilliseconds = 300000
    };

    public FunctionalTestingWebApplicationFactory()
    {
    }
    
    public override async Task InitializeAsync()
    {
        await TryInstallBrowsers();
        await base.InitializeAsync();
    }
    
    protected override IHost CreateHost(IHostBuilder builder)
    {
        // need to create a plain host that we can return.
        var dummyHost = builder.Build();
    
        // configure and start the actual host.
        builder.ConfigureWebHost(webHostBuilder => webHostBuilder.UseKestrel());
        base.CreateHost(builder);
        
        return dummyHost;
    }
    
    public FunctionalTestingWebApplicationFactory WithTestOutputHelper(
        ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper ?? throw new ArgumentException(nameof(outputHelper));
        return this;
    }
    
    public FunctionalTestingWebApplicationFactory WithFixtureOptions(
        BrowserFixtureOptions fixtureOptions)
    {
        _options = fixtureOptions ?? throw new ArgumentException(nameof(fixtureOptions));
        return this;
    }
    
    public FunctionalTestingWebApplicationFactory WithFixtureOptions(
        string browserType,
        string? browserChannel)
    {
        _options.BrowserType = browserType;
        _options.BrowserChannel = browserChannel;
        _options.Headless = !(browserChannel is not null && browserChannel.Equals("msedge"));
        
        return this;
    }

    public async Task WithPageAsync(
        Func<IPage, Task> action,
        [CallerMemberName] string? testName = null)
    {
        if (!string.IsNullOrWhiteSpace(_options.TestName))
        {
            testName = _options.TestName;
        }
        
        // Create a new browser of the specified type
        using IPlaywright playwright = await Playwright.CreateAsync();

        await using IBrowser browser = await CreateBrowserAsync(playwright);
        // Create a new context for the test
        BrowserNewContextOptions contextOptions = CreateContextOptions();

        await using IBrowserContext context = await browser.NewContextAsync(contextOptions);
        context.SetDefaultTimeout(_options.TimeoutAsMilliseconds);

        // Enable generating a trace, if enabled, to use with https://trace.playwright.dev
        if (_options.ShouldCaptureTrace)
        {
            await context.Tracing.StartAsync(new TracingStartOptions
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true,
                Title = testName
            });
        }

        // Create a new page to use for the test
        IPage page = await context.NewPageAsync();

        // Capture output from the browser to the test logs
        page.Console += (_, e) => OutputHelper.WriteLine(e.Text);
        page.PageError += (_, e) => OutputHelper.WriteLine(e);

        try
        {
            // Run the test, passing the page to it
            await action(page);
        }
        catch (Exception)
        {
            // Try and capture a screenshot at the point the test failed
            await TryCaptureScreenshotAsync(page, testName!);

            throw;
        }
        finally
        {
            if (_options.ShouldCaptureTrace)
            {
                string traceName = GenerateFileName(testName!, ".zip");
                string path = Path.Combine("traces", traceName);

                await context.Tracing.StopAsync(new TracingStopOptions
                {
                    Path = path
                });
            }

            await TryCaptureVideoAsync(page, testName!);
        }
    }

    private static Task TryInstallBrowsers()
    {
        // Try to install browsers
        var exitCode = Program.Main(new[] {"install"});
        if (exitCode != 0)
        {
            throw new InvalidOperationException($"Playwright exited with code {exitCode}.");
        }

        return Task.CompletedTask;
    }

    private BrowserNewContextOptions CreateContextOptions()
    {
        var options = new BrowserNewContextOptions
        {
            Locale = "en-GB",
            TimezoneId = "Europe/Paris",
            IgnoreHTTPSErrors = true,
            
        };

        if (_options.ShouldCaptureVideo)
        {
            options.RecordVideoDir = Path.GetTempPath();
        }

        return options;
    }

    private async Task<IBrowser> CreateBrowserAsync(IPlaywright playwright)
    {
        var options = new BrowserTypeLaunchOptions
        {
            Channel = _options.BrowserChannel,
            Headless = _options.Headless,
            SlowMo = _options.SlowMo
        };

        if (Debugger.IsAttached)
        {
            options.Devtools = true;
            options.Headless = false;
            options.SlowMo = 100;
        }

        IBrowserType browserType = playwright[_options.BrowserType];
        return await browserType.LaunchAsync(options);
    }
    
    private string GenerateFileName(string testName, string extension)
    {
        var browserType = _options.BrowserType;

        if (!string.IsNullOrEmpty(_options.BrowserChannel))
        {
            browserType += "_" + _options.BrowserChannel;
        }

        string os =
            OperatingSystem.IsLinux() ? "linux" :
            OperatingSystem.IsMacOS() ? "macos" :
            OperatingSystem.IsWindows() ? "windows" :
            "other";

        string utcNow = DateTimeOffset.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture);
        return $"{testName}_{browserType}_{os}_{utcNow}{extension}";
    }
    
    private async Task TryCaptureScreenshotAsync(
        IPage page,
        string testName)
    {
        try
        {
            // Generate a unique name for the screenshot
            string fileName = GenerateFileName(testName, ".png");
            string path = Path.Combine("screenshots", fileName);

            await page.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = path
            });

            OutputHelper.WriteLine($"Screenshot saved to {path}.");
        }
        catch (Exception ex)
        {
            OutputHelper.WriteLine("Failed to capture screenshot: " + ex);
        }
    }

    private async Task TryCaptureVideoAsync(
        IPage page,
        string testName)
    {
        if (!_options.ShouldCaptureVideo || page.Video is null)
        {
            return;
        }

        try
        {
            string fileName = GenerateFileName(testName, ".webm");
            string path = Path.Combine("videos", fileName);

            await page.CloseAsync();
            await page.Video!.SaveAsAsync(path);

            OutputHelper.WriteLine($"Video saved to {path}.");
        }
        catch (Exception ex)
        {
            OutputHelper.WriteLine("Failed to capture video: " + ex);
        }
    }
}