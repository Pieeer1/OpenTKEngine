using OpenTK.Mathematics;
using OpenTKEngine.Entities;
using OpenTKEngine.Entities.Components;
using OpenTKEngine.Models;
using OpenTKEngine.Models.Shapes3D;
using static OpenTKEngine.Models.Constants;

namespace OpenTKEngine.Assets.Scripts.Shared.Entities
{
    public class Player
    {
        private Shader _shader;
        private Entity _entity;

        public Player(Shader shader, Entity entity)
        {
            _shader = shader;
            _entity = entity;

            _entity.AddComponent(new PlayerComponent(_shader, new Vector3(0.0f, 2.0f, 0.0f)));
            _entity.GetComponent<PlayerComponent>().ActiveMovementPreset = Enums.MovementPresets.Player;
            Entity childBlock = _entity.AddChildEntity(new Entity());
            TransformComponent comp = new TransformComponent(new Vector3(0.0f, -1.0f, 0.0f), new Quaternion(0.0f, 0.0f, 0.0f), new Vector3(0.25f, 0.25f, 0.25f));
            comp.SyncedTransforms = Enums.SyncedTransforms.Position | Enums.SyncedTransforms.Rotation;
            childBlock.AddComponent(new ShapeComponent(_shader, new Cube(), comp, textures: new List<Texture>()
            {
                Texture.LoadFromFile($"{AssetRoutes.Textures}/container2.png"),
                Texture.LoadFromFile($"{AssetRoutes.Textures}/container2_specular.png"),
            }));
        }
    }
}
