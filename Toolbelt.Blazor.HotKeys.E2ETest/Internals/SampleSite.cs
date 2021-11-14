using System.Diagnostics;
using System.Text;

namespace Toolbelt.Blazor.HotKeys.E2ETest;

public class SampleSite
{
    private readonly int ListenPort;

    private readonly string ProjectName;
    private readonly string? TargetFramework;
    private Process? dotnetCLI;

    private readonly ManualResetEventSlim ListeningWaiter = new ManualResetEventSlim(initialState: false);

    public SampleSite(int listenPort, string projectName, string? targetFramework = null)
    {
        this.ListenPort = listenPort;
        this.ProjectName = projectName;
        this.TargetFramework = targetFramework;
    }

    public string GetUrl() => $"http://localhost:{this.ListenPort}";

    public void Start()
    {
        if (this.dotnetCLI != null) return;

        var workDir = AppDomain.CurrentDomain.BaseDirectory;
        while (workDir != null && !Directory.GetDirectories(workDir, "SampleSites").Any())
            workDir = Path.GetDirectoryName(workDir);

        var projectNameParts = this.ProjectName.Split('/');
        var projectFolder = projectNameParts[0];
        var projectName = projectNameParts.Length > 1 ? projectNameParts[1] : "";
        workDir = Path.Combine(workDir ?? ".", "SampleSites", projectFolder);

        var args = new StringBuilder();
        args.Append($"run --urls {this.GetUrl()}");
        if (this.TargetFramework != null) args.Append($" -f {this.TargetFramework}");
        if (!string.IsNullOrEmpty(projectName)) args.Append($" -p {projectName}.csproj");

        this.dotnetCLI = new Process
        {
            EnableRaisingEvents = true,
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = args.ToString(),
                UseShellExecute = false,
                CreateNoWindow = true,
                ErrorDialog = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WorkingDirectory = workDir
            },
        };
        this.dotnetCLI.OutputDataReceived += this.Process_OutputDataReceived;

        this.dotnetCLI.Start();
        this.dotnetCLI.BeginOutputReadLine();
        this.dotnetCLI.BeginErrorReadLine();

        var timedOut = !this.ListeningWaiter.Wait(millisecondsTimeout: 10000);
        if (timedOut) throw new TimeoutException("\"dotnet run\" did not respond \"Now listening on: http://\".");
        Thread.Sleep(200);
    }

    private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (e.Data?.Contains("Now listening on: http://") == true) this.ListeningWaiter.Set();
    }

    public void Stop()
    {
        if (this.dotnetCLI != null)
        {
            //dotnetCLI.OutputDataReceived -= Process_OutputDataReceived;
            if (!this.dotnetCLI.HasExited) this.dotnetCLI.Kill();
            this.dotnetCLI.Dispose();
        }
        this.ListeningWaiter.Dispose();
    }
}
