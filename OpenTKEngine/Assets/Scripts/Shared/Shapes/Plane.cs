using OpenTKEngine.Entities;
using OpenTK.Mathematics;
using OpenTKEngine.Entities.Components;
using static OpenTKEngine.Models.Constants;
using OpenTKEngine.Models;

namespace OpenTKEngine.Assets.Scripts.Shared.Shapes
{
    public class Plane
    {
        private Shader _shader;
        private Entity _entity;
        public Vector3 _position;
        public Quaternion? _rotation;
        public Vector2? _scale;

        public Plane(Shader shader, Entity entity, Vector3 position, Quaternion? rotation = null, Vector2? scale = null)
        {
            _shader = shader;
            _entity = entity;
            _position = position;
            _rotation = rotation;
            _scale = scale;

            var test = Quaternion.Identity;

            Vector3 initialOrientation = new Vector3(0.0f, 0.0f, 1.0f); // default orientation
            Vector3 targetOrientation = new Vector3(0.0f, 1.0f, 0.0f);

            Quaternion rotationQuaternion = Quaternion.FromAxisAngle(Vector3.Cross(initialOrientation, targetOrientation), MathHelper.DegreesToRadians(90f));

            _entity.AddComponent(new ModelComponent(_shader, new Models.Shapes3D.Models.Model($"{AssetRoutes.Models}/plane.dae"), position, rotation ?? rotationQuaternion, scale != null ? new Vector3(scale.Value.X, scale.Value.Y, 0.01f) : new Vector3(1.0f, 1.0f, 0.01f), new List<Texture>()
            {
                Texture.LoadFromFile($"{AssetRoutes.Textures}/planegray.png")
            }));
        }
    }
}
