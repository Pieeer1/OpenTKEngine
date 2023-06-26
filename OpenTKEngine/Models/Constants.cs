namespace OpenTKEngine.Models
{
    public static class Constants
    {
        public static class ShaderRoutes
        {
            public const string BaseVertexShader = "../../../Shaders/shader.vert";
            public const string BaseFragmentShader = "../../../Shaders/shader.frag";
            public const string BaseLightingShader = "../../../Shaders/lighting.frag";
        }
        public static class AssetRoutes
        {
            public const string Textures = "../../../Assets/Textures";
        }
        public static class ShaderConstants
        {
            public const string TextureShader = "TextureShader";
            public const string LightShader = "LightShader";
            public const string ColorTexture = "ColorShader";//not yet implemented
        }
    }
}
