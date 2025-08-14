


namespace AzerothCoreManager.Service
{
    public class AzerothCorePaths
    {
        public string AuthServerPath { get; set; } = "C:\\azerothcore\\bin\\authserver.exe";
        public string WorldServerPath { get; set; } = "C:\\azerothcore\\bin\\worldserver.exe";
        public string LogsDirectory { get; set; } = "C:\\azerothcore\\logs";
        public string ConfigsDirectory { get; set; } = "C:\\azerothcore\\etc";
    }

    public class SecuritySettings
    {
        public string ApiToken { get; set; } = "changeme123";
        public bool AutoRestartOnCrash { get; set; } = true;
    }
}


