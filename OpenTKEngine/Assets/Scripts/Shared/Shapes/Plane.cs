using OpenTKEngine.Entities;
using OpenTK.Mathematics;
using OpenTKEngine.Entities.Components;
using static OpenTKEngine.Models.Constants;
using OpenTKEngine.Models;
using OpenTKEngine.Services;
using OpenTKEngine.Enums;

namespace OpenTKEngine.Assets.Scripts.Shared.Shapes
{
    public class Plane : Entity
    {
        private Shader _shader;
        public Vector3 _position;
        public Quaternion? _rotation;
        public Vector2? _scale;

        public Plane(Shader shader,Vector3 position, Quaternion? rotation = null, Vector2? scale = null, Layer layer = Layer.None)
        {
            _shader = shader;
            _position = position;
            _rotation = rotation;
            _scale = scale;
            Layer = layer;

            Transform = new Transform(position, rotation, scale != null ? new Vector3(scale.Value.X, 0.01f, scale.Value.Y) : new Vector3(1.0f, 0.01f, 1.0f));
            AddComponent(new ModelComponent(_shader, new Models.Shapes3D.Models.Model($"{AssetRoutes.Models}/plane.dae"), new List<Texture>()
            {
                Texture.LoadFromFile($"{AssetRoutes.Textures}/planegray.png")
            }));
            AddComponent(new StaticRigidBodyComponent());
        }
    }
}
