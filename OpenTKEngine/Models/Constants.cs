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
            public const string BaseStaticColorVertexShader = "../../../Shaders/staticcolor.vert";
            public const string BaseStaticColorFragmentShader = "../../../Shaders/staticcolor.frag";
        }
        public static class FontRoutes
        {
            public const string Corbel = "../../../Assets/Fonts/CORBEL.TTF";
        }
        public static class AssetRoutes
        {
            public const string Textures = "../../../Assets/Textures";
        }
        public static class ShaderConstants
        {
            public const string TextureShader = "TextureShader";
            public const string LightShader = "LightShader";
            public const string StaticColorShader = "StaticColorShader";
            public const string TextShader = "TextShader";
        }
        public static class ColorVectors
        {
            public static Vector3 Black = Vector3.Zero;
            public static Vector3 White = Vector3.One;
        }
    }
}
