using System;
using System.IO;

namespace Tunny.Util
{
    public static class TunnyVariables
    {
        public static string TunnyEnvPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".tunny_env");
        public static string OptimizeSettingsPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".tunny_env", "settings.json");
    }
}
