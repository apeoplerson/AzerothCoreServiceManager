



using System.Diagnostics;

namespace AzerothCoreManager.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IProcessManager _processManager;

        public Worker(ILogger<Worker> logger, IProcessManager processManager)
        {
            _logger = logger;
            _processManager = processManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Monitor processes and handle crashes
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (_processManager.IsAuthServerRunning())
                    {
                        var authPid = _processManager.GetAuthServerPid();
                        if (authPid > 0)
                        {
                            try
                            {
                                using (var process = Process.GetProcessById(authPid))
                                {
                                    if (process.HasExited)
                                    {
                                        _logger.LogWarning("Auth server crashed with exit code: {ExitCode}", process.ExitCode);
                                        // Implement auto-restart logic here if needed
                                    }
                                }
                            }
                            catch (ArgumentException)
                            {
                                // Process no longer exists, consider it crashed
                                _logger.LogWarning("Auth server process no longer exists");
                            }
                        }
                    }

                    if (_processManager.IsWorldServerRunning())
                    {
                        var worldPid = _processManager.GetWorldServerPid();
                        if (worldPid > 0)
                        {
                            try
                            {
                                using (var process = Process.GetProcessById(worldPid))
                                {
                                    if (process.HasExited)
                                    {
                                        _logger.LogWarning("World server crashed with exit code: {ExitCode}", process.ExitCode);
                                        // Implement auto-restart logic here if needed
                                    }
                                }
                            }
                            catch (ArgumentException)
                            {
                                // Process no longer exists, consider it crashed
                                _logger.LogWarning("World server process no longer exists");
                            }
                        }
                    }

                    await Task.Delay(5000, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in background monitoring loop");
                    await Task.Delay(10000, stoppingToken); // Backoff on error
                }
            }
        }
    }
}


