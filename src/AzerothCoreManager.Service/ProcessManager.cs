




using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AzerothCoreManager.Service
{
    public class ProcessManager : IProcessManager
    {
        private readonly ILogger<ProcessManager> _logger;
        private readonly AzerothCorePaths _paths;
        private readonly SecuritySettings _securitySettings;
        private Process? _authServerProcess;
        private Process? _worldServerProcess;

        public ProcessManager(ILogger<ProcessManager> logger, IOptions<AzerothCorePaths> paths, IOptions<SecuritySettings> securitySettings)
        {
            _logger = logger;
            _paths = paths.Value;
            _securitySettings = securitySettings.Value;
        }

        public async Task StartAuthServer()
        {
            if (IsAuthServerRunning())
            {
                _logger.LogInformation("Auth server is already running");
                return;
            }

            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = _paths.AuthServerPath,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    WorkingDirectory = Path.GetDirectoryName(_paths.AuthServerPath) ?? string.Empty
                };

                _authServerProcess = new Process { StartInfo = startInfo };
                _authServerProcess.OutputDataReceived += (sender, args) => LogProcessOutput("auth", args.Data);
                _authServerProcess.ErrorDataReceived += (sender, args) => LogProcessOutput("auth", args.Data);

                _authServerProcess.Start();
                _authServerProcess.BeginOutputReadLine();
                _authServerProcess.BeginErrorReadLine();

                _logger.LogInformation("Started Auth server with PID: {Pid}", _authServerProcess.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to start Auth server");
                _authServerProcess = null;
            }
        }

        public async Task StopAuthServer()
        {
            if (!_authServerProcess?.HasExited ?? true)
            {
                try
                {
                    // Try graceful shutdown first
                    NativeMethods.GenerateConsoleCtrlEvent(0, (uint)_authServerProcess.Id);

                    await Task.Delay(2000);

                    if (!_authServerProcess.HasExited)
                    {
                        _logger.LogInformation("Auth server didn't respond to CTRL+C, forcing termination");
                        _authServerProcess.Kill();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to stop Auth server gracefully, attempting force kill");
                    try
                    {
                        _authServerProcess?.Kill();
                    }
                    catch { }
                }

                _authServerProcess.WaitForExit();
                _logger.LogInformation("Auth server stopped with exit code: {ExitCode}", _authServerProcess.ExitCode);
            }
        }

        public bool IsAuthServerRunning() => _authServerProcess?.HasExited == false;

        public int GetAuthServerPid() => IsAuthServerRunning() ? _authServerProcess!.Id : 0;

        public async Task StartWorldServer()
        {
            if (IsWorldServerRunning())
            {
                _logger.LogInformation("World server is already running");
                return;
            }

            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = _paths.WorldServerPath,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    WorkingDirectory = Path.GetDirectoryName(_paths.WorldServerPath) ?? string.Empty
                };

                _worldServerProcess = new Process { StartInfo = startInfo };
                _worldServerProcess.OutputDataReceived += (sender, args) => LogProcessOutput("world", args.Data);
                _worldServerProcess.ErrorDataReceived += (sender, args) => LogProcessOutput("world", args.Data);

                _worldServerProcess.Start();
                _worldServerProcess.BeginOutputReadLine();
                _worldServerProcess.BeginErrorReadLine();

                _logger.LogInformation("Started World server with PID: {Pid}", _worldServerProcess.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to start World server");
                _worldServerProcess = null;
            }
        }

        public async Task StopWorldServer()
        {
            if (!_worldServerProcess?.HasExited ?? true)
            {
                try
                {
                    // Try graceful shutdown first
                    NativeMethods.GenerateConsoleCtrlEvent(0, (uint)_worldServerProcess.Id);

                    await Task.Delay(2000);

                    if (!_worldServerProcess.HasExited)
                    {
                        _logger.LogInformation("World server didn't respond to CTRL+C, forcing termination");
                        _worldServerProcess.Kill();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to stop World server gracefully, attempting force kill");
                    try
                    {
                        _worldServerProcess?.Kill();
                    }
                    catch { }
                }

                _worldServerProcess.WaitForExit();
                _logger.LogInformation("World server stopped with exit code: {ExitCode}", _worldServerProcess.ExitCode);
            }
        }

        public bool IsWorldServerRunning() => _worldServerProcess?.HasExited == false;

        public int GetWorldServerPid() => IsWorldServerRunning() ? _worldServerProcess!.Id : 0;

        private void LogProcessOutput(string serverType, string? line)
        {
            if (string.IsNullOrEmpty(line)) return;

            // Log to file and console
            var logFilePath = Path.Combine(_paths.LogsDirectory, $"{serverType}server.log");
            File.AppendAllText(logFilePath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {line}{Environment.NewLine}");
        }
    }

    internal static class NativeMethods
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern bool GenerateConsoleCtrlEvent(uint dwCtrlEvent, uint dwProcessGroupId);
    }
}




