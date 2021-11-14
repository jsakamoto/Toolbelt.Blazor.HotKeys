using NUnit.Framework;
using OpenQA.Selenium.Chrome;

namespace Toolbelt.Blazor.HotKeys.E2ETest;

[SetUpFixture]
public class TestContext
{
    public static TestContext Instance { get; private set; } = null!;

    private readonly IReadOnlyDictionary<HostingModel, SampleSite> SampleSites = new Dictionary<HostingModel, SampleSite> {
            { HostingModel.Wasm31, new SampleSite(5011, "Client31") },
            { HostingModel.Wasm50, new SampleSite(5012, "Client", "net5.0") },
            { HostingModel.Wasm60, new SampleSite(5013, "Client", "net6.0") },
            { HostingModel.Server31, new SampleSite(5014, "Server", "netcoreapp3.1") },
            { HostingModel.Server50, new SampleSite(5015, "Server", "net5.0") },
            { HostingModel.Server60, new SampleSite(5016, "Server", "net6.0") },
        };

    private ChromeDriver? _WebDriver;

    public ChromeDriver WebDriver
    {
        get
        {
            if (this._WebDriver == null)
            {
                this._WebDriver = new ChromeDriver();
            }
            return this._WebDriver;
        }
    }

    public TestContext()
    {
    }

    public void StartHost(HostingModel hostingModel)
    {
        this.SampleSites[hostingModel].Start();
    }

    public string GetHostUrl(HostingModel hostingModel)
    {
        return this.SampleSites[hostingModel].GetUrl();
    }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        Instance = this;
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        Parallel.ForEach(this.SampleSites.Values, sampleSite => sampleSite.Stop());
        this._WebDriver?.Quit();
    }
}
