using Toolbelt.Diagnostics;
using static Toolbelt.Diagnostics.XProcess;

namespace Toolbelt.Blazor.HotKeys.E2ETest;

public class SampleSite
{
    private readonly int ListenPort;

    private readonly string ProjectSubFolder;

    private readonly string? TargetFramework;

    private XProcess? dotnetCLI;

    public SampleSite(int listenPort, string projectSubFolder, string? targetFramework = null)
    {
        this.ListenPort = listenPort;
        this.ProjectSubFolder = projectSubFolder;
        this.TargetFramework = targetFramework;
    }

    public string GetUrl() => $"http://localhost:{this.ListenPort}";

    internal string GetUrl(string subPath) => this.GetUrl() + "/" + subPath.TrimStart('/');

    public async ValueTask<SampleSite> StartAsync()
    {
        if (this.dotnetCLI != null) return this;

        var solutionDir = FileIO.FindContainerDirToAncestor("*.sln");
        var sampleSiteDir = Path.Combine(solutionDir, "SampleSites");
        var projDir = Path.Combine(sampleSiteDir, this.ProjectSubFolder);

        var frameworkOption = string.IsNullOrEmpty(this.TargetFramework) ? "" : "-f " + this.TargetFramework;
        this.dotnetCLI = Start("dotnet", $"run --urls {this.GetUrl()} {frameworkOption}", projDir);

        var success = await this.dotnetCLI.WaitForOutputAsync(output => output.Contains(this.GetUrl()), millsecondsTimeout: 15000);
        if (!success)
        {
            try { this.dotnetCLI.Dispose(); } catch { }
            var output = this.dotnetCLI.Output;
            this.dotnetCLI = null;
            throw new TimeoutException($"\"dotnet run\" did not respond \"Now listening on: {this.GetUrl()}\".\r\n" + output);
        }

        Thread.Sleep(200);
        return this;
    }

    public void Stop()
    {
        this.dotnetCLI?.Dispose();
    }
}
