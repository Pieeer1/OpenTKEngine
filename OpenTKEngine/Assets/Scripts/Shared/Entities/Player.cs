using OpenTK.Mathematics;
using OpenTKEngine.Entities;
using OpenTKEngine.Entities.Components;
using OpenTKEngine.Models;

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

        }
    }
}
