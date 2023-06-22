using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKEngine.Models;

namespace OpenTKEngine.Entities.Components
{
    public class PlayerComponent : Component
    {
        private TransformComponent _transform = null!;
        private CameraComponent _camera = null!;
        private SpotLightComponent _flashlight = null!;

        private readonly Vector3? _startingLocation;
        private readonly Shader _shader;
        private readonly float _aspectRatio;
        public PlayerComponent(Shader shader, float aspectRatio, Vector3? startingLocation = null)
        {
            _shader = shader;
            _aspectRatio = aspectRatio;
            _startingLocation = startingLocation;
        }

        public override void Init()
        {
            _transform = Entity.AddComponent<TransformComponent>(new TransformComponent(_startingLocation));
            _camera = Entity.AddComponent<CameraComponent>(new CameraComponent(_shader, _aspectRatio));
            _flashlight = Entity.AddComponent<SpotLightComponent>(new SpotLightComponent(_shader, _transform.Position));
        }

        public override void Draw()
        {
            base.Draw();
        }
        public override void UpdateInput(FrameEventArgs e, KeyboardState input, MouseState mouse, ref bool firstMove, ref Vector2 lastPos)
        {
            const float cameraSpeed = 1.5f;
            const float sensitivity = 0.2f;

            if (input.IsKeyDown(Keys.W))
            {
                _transform.Position += _camera.Front * cameraSpeed * (float)e.Time; // Forward
            }
            if (input.IsKeyDown(Keys.S))
            {
                _transform.Position -= _camera.Front * cameraSpeed * (float)e.Time; // Backwards
            }
            if (input.IsKeyDown(Keys.A))
            {
                _transform.Position -= _camera.Right * cameraSpeed * (float)e.Time; // Left
            }
            if (input.IsKeyDown(Keys.D))
            {
                _transform.Position += _camera.Right * cameraSpeed * (float)e.Time; // Right
            }
            if (input.IsKeyDown(Keys.Space))
            {
                _transform.Position += _camera.Up * cameraSpeed * (float)e.Time; // Up
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                _transform.Position -= _camera.Up * cameraSpeed * (float)e.Time; // Down
            }

            if (firstMove)
            {
                lastPos = new Vector2(mouse.X, mouse.Y);
                firstMove = false;
            }
            else
            {
                var deltaX = mouse.X - lastPos.X;
                var deltaY = mouse.Y - lastPos.Y;
                lastPos = new Vector2(mouse.X, mouse.Y);

                _camera.Yaw += deltaX * sensitivity;
                _camera.Pitch -= deltaY * sensitivity;
            }
        }
    }
}
