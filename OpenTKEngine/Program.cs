using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTKEngine.Engine;
using System.Runtime.InteropServices;

namespace OpenTKEngine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Startup.Start();

            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(800, 600),
                Title = "OpenTKEngine",
                Flags = ContextFlags.ForwardCompatible,
            };

            GameWindowSettings gameWindowSettings = GameWindowSettings.Default;
            gameWindowSettings.RenderFrequency = 60.0f; // max fps
            gameWindowSettings.UpdateFrequency = 60.0f; // max ups

            using (Window window = new Window(gameWindowSettings, nativeWindowSettings))
            {
                window.Run();
            }
        }
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