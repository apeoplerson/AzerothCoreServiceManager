

using System.Threading.Tasks;

namespace AzerothCoreManager.Service
{
    public interface IProcessManager
    {
        Task StartAuthServer();
        Task StopAuthServer();
        bool IsAuthServerRunning();
        int GetAuthServerPid();

        Task StartWorldServer();
        Task StopWorldServer();
        bool IsWorldServerRunning();
        int GetWorldServerPid();
    }
}

