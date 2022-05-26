using System.Collections;
using Microsoft.Playwright;

namespace Beng.Specta.Compta.FunctionalTests;

public sealed class BrowsersTestData : IEnumerable<object[]>
{
    private bool UseAllBrowserType { get; }
    
    public BrowsersTestData()
    {
    }
    
    public BrowsersTestData(bool useAllBrowserType)
    { 
        UseAllBrowserType = useAllBrowserType;
    }
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { BrowserType.Chromium, null! };

        if (UseAllBrowserType || !OperatingSystem.IsWindows())
        {
            yield return new object[] { BrowserType.Chromium, "chrome" };
        }

        if (UseAllBrowserType || (!OperatingSystem.IsLinux() && !OperatingSystem.IsMacOS()))
        {
            yield return new object[] { BrowserType.Chromium, "msedge" };
        }

        yield return new object[] { BrowserType.Firefox, null! };

        if (UseAllBrowserType || OperatingSystem.IsMacOS())
        {
            yield return new object[] { BrowserType.Webkit, null! };
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}