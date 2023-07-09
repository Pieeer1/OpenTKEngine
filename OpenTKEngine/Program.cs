using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTKEngine.Engine;

namespace OpenTKEngine
{
    public class Program
    {
        public static void Main(string[] args)
        {
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
}