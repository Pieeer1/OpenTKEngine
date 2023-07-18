using OpenTK.Mathematics;

namespace OpenTKEngine.Models
{
    public static class Constants
    {
        public static class ShaderRoutes
        {
            public const string BaseVertexShader = "../../../Shaders/shader.vert";
            public const string BaseFragmentShader = "../../../Shaders/shader.frag";
            public const string BaseLightingShader = "../../../Shaders/lighting.frag";
            public const string BaseTextVertexShader = "../../../Shaders/textshader.vert";
            public const string BaseTextFragmentShader = "../../../Shaders/textshader.frag";
            public const string SkyboxVertexShader = "../../../Shaders/skyboxshader.vert";
            public const string SkyboxFragmentShader = "../../../Shaders/skyboxshader.frag";
        }
        public static class FontRoutes
        {
            public const string Corbel = "../../../Assets/Fonts/CORBEL.TTF";
            public const string Arial = "../../../Assets/Fonts/ARIAL.TTF";
        }
        public static class AssetRoutes
        {
            public const string Textures = "../../../Assets/Textures";
            public const string Models = "../../../Assets/Models";
            public const string UserSettings = "Config/settings.json";
        }
        public static class ShaderConstants
        {
            public const string TextureShader = "TextureShader";
            public const string LightShader = "LightShader";
            public const string TextShader = "TextShader";
            public const string SkyboxShader = "SkyboxShader";
        }
        public static class ColorVectors
        {
            public static Vector3 Black = Vector3.Zero;
            public static Vector3 White = Vector3.One;
        }
        public static class LimitedLists
        {
            public static List<string> Resolutions = new List<string>()
            {
                "3840x2160",
                "3440x1440",
                "2560x1600",
                "2560x1600",
                "2560x1440",
                "2560x1080",
                "2560x1080",
                "1920x1440",
                "1920x1200",
                "1920x1080",
                "1680x1050",
                "1600x900",
                "1600x1200",
                "1600x1024",
                "1440x900",
                "1366x768",
                "1280x1024",
                "1280x800",
                "1280x720",
                "1024x768"
            };
        }
        public static class Styles
        {
            public static Dictionary<string, Vector4> ColorDictionary = new Dictionary<string, Vector4>()
            {
                { "BackgroundColor", new Vector4(0.2f, 0.2f, 0.2f, 1.0f) },
                { "DefaultText", new Vector4(0.8f, 0.8f, 0.8f, 1.0f) },
                { "SpecialText", new Vector4(0.0f, 0.6f, 0.8f, 1.0f) },
                { "ButtonColor", new Vector4(0.7f, 0.2f, 0.2f, 1.0f) },
                { "BorderColor", new Vector4(0.3f, 0.3f, 0.3f, 1.0f) },
                { "AccentColor1", new Vector4(0.5f, 0.2f, 0.3f, 1.0f) },
                { "AccentColor2", new Vector4(0.4f, 0.5f, 0.2f, 1.0f) },
                { "AccentColor3", new Vector4(0.7f, 0.5f, 0.1f, 1.0f) },
                { "AccentColor4", new Vector4(0.1f, 0.3f, 0.4f, 1.0f) },
                { "AccentColor5", new Vector4(0.6f, 0.3f, 0.1f, 1.0f) }
            };
        }
    }
}
