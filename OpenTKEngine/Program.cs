using Newtonsoft.Json;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTKEngine.Engine;
using OpenTKEngine.Models.Configurations;
using OpenTKEngine.Services;
using System.Runtime.InteropServices;
using static OpenTKEngine.Models.Constants;
namespace OpenTKEngine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Startup.Start();

            SettingsConfiguration defaultSettings = GetSettings();

            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings()
            {
                WindowState = WindowState.Normal,
                Size = DataManipulationService.ParseResolution(defaultSettings.Resolution),
                Title = "OpenTKEngine",
                Flags = ContextFlags.ForwardCompatible,
                NumberOfSamples = 4
            };

            GameWindowSettings gameWindowSettings = GameWindowSettings.Default;
            gameWindowSettings.RenderFrequency = 60.0f; // max fps
            gameWindowSettings.UpdateFrequency = 60.0f; // max ups

            using (Window window = new Window(gameWindowSettings, nativeWindowSettings, defaultSettings.IsFullScreen))
            {
                window.Run();
            }
        }


        private static SettingsConfiguration GetSettings()
        {
            if (!File.Exists(AssetRoutes.UserSettings) || string.IsNullOrWhiteSpace(File.ReadAllText(AssetRoutes.UserSettings)))
            {
                if (!Directory.Exists("Config"))
                {
                    Directory.CreateDirectory("Config");
                }
                using (var @ref = File.Create(AssetRoutes.UserSettings)){ }
                File.WriteAllText(AssetRoutes.UserSettings, JsonConvert.SerializeObject(GenerateDefaultSettingsObject, new JsonSerializerSettings()
                { 
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }));
            }

            try
            {
                SettingsConfiguration config = JsonConvert.DeserializeObject<SettingsConfiguration>(File.ReadAllText(AssetRoutes.UserSettings)) ?? GenerateDefaultSettingsObject;
                return config;
            }
            catch
            {
                return GenerateDefaultSettingsObject;
            }
        }
        private static SettingsConfiguration GenerateDefaultSettingsObject => new SettingsConfiguration()
        {
            IsFullScreen = false,
            Resolution = DataManipulationService.ParseResolution(new Vector2i(800, 600)),
        };

    }
    public static class Startup
    {
        public static void Start()
        { 
            var handle = GetConsoleWindow();
            ShowWindow(handle, 0);
        }

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    }


}