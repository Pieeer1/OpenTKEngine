using OpenTK.Mathematics;
using OpenTKEngine.Entities;
using OpenTKEngine.Entities.Components;
using OpenTKEngine.Models;
using OpenTKEngine.Models.Shapes3D;
using static OpenTKEngine.Models.Constants;

namespace OpenTKEngine.Assets.Scripts.Shared.Entities
{
    public class Player : Entity
    {
        private Shader _shader;

        public Player(Shader shader)
        {
            Transform = new Transform(new Vector3(0.0f, 2.0f, 0.0f));

            _shader = shader;

            PlayerComponent playerComponent = AddComponent(new PlayerComponent(_shader));
            playerComponent.ActiveMovementPreset = Enums.MovementPresets.Player;
            Entity childBlock = AddChildEntity(new Entity());

            Transform comp = new Transform(new Vector3(0.0f, -1.0f, 0.0f), new Quaternion(0.0f, 0.0f, 0.0f), new Vector3(0.25f, 0.25f, 0.25f));
            childBlock.Transform = comp;
            childBlock.SyncedTransforms = Enums.SyncedTransforms.Position | Enums.SyncedTransforms.Rotation;
            childBlock.AddComponent(new ShapeComponent(_shader, new Cube(), textures: new List<Texture>()
            {
                Texture.LoadFromFile($"{AssetRoutes.Textures}/container2.png"),
                Texture.LoadFromFile($"{AssetRoutes.Textures}/container2_specular.png"),
            }));
        }
    }
}
