using BulletSharp;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKEngine.Attributes;
using OpenTKEngine.Enums;
using OpenTKEngine.Models;
using OpenTKEngine.Models.Physics;
using OpenTKEngine.Services;

namespace OpenTKEngine.Entities.Components
{
    public class PlayerComponent : Component
    {
        private TransformComponent _transform = null!;
        private RigidBodyComponent _rigidBody = null!;
        private CameraComponent _camera = null!;
        private SpotLightComponent _flashlight = null!;
        private bool isGrounded { 
            get
            {
                foreach (var rb in LayerManager.Instance.GetEntitiesAtLayer(Layer.Ground).Select(x => x.GetComponent<RigidBodyComponent>()))
                {
                    if (rb.RigidBody.CheckCollideWith(_rigidBody.RigidBody)) // straight up not fucking working
                    {
                        return true;
                    }
                }
                return false;
            } 
        }

        [MenuDisable]
        public bool IsControlEnabled { get; set; } = true;

        private readonly Vector3? _startingLocation;
        private readonly Shader _shader;

        public MovementPresets ActiveMovementPreset { get; set; } = MovementPresets.Player;
        public PlayerComponent(Shader shader, Vector3? startingLocation = null)
        {
            _shader = shader;
            _startingLocation = startingLocation;
        }

        public override void Init()
        {
            _transform = Entity.AddComponent(new TransformComponent(_startingLocation));
            _camera = Entity.AddComponent(new CameraComponent(_shader));

            CollisionShape collisionShape = new BoxShape(1.0f, 1.0f, 1.0f);
            float mass = 1.0f;

            System.Numerics.Vector3 localInertia = collisionShape.CalculateLocalInertia(mass);

            RigidBodyConstructionInfo rbInfo = new RigidBodyConstructionInfo(mass, new OpenTKMotionState(_transform), collisionShape, localInertia)
            {
                Restitution = 0.0f,
                Friction = 0.01f,
            };
            RigidBody rb= new RigidBody(rbInfo);

            _rigidBody = Entity.AddComponent(new RigidBodyComponent(rb, CollisionFlags.CharacterObject));

            _flashlight = Entity.AddComponent(new SpotLightComponent(_shader, _transform.Position));
        }

        public override void Draw()
        {
            base.Draw();
        }
        public override void UpdateInput(FrameEventArgs e, KeyboardState input, MouseState mouse, ref bool firstMove, ref Vector2 lastPos)
        {
            base.UpdateInput(e, input, mouse, ref firstMove, ref lastPos);
            if ((ActiveInputFlags & InputFlags.Player) == 0) { return; }
            const float cameraSpeed = 1.5f;
            const float sensitivity = 0.2f;
            if (ActiveMovementPreset == MovementPresets.Spectator)
            {
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
            }
            else if (ActiveMovementPreset == MovementPresets.Player)
            {
                if (input.IsKeyDown(Keys.W))
                {
                    _transform.Position = new Vector3(_transform.Position.X + _camera.Front.X * cameraSpeed * (float)e.Time, _transform.Position.Y, _transform.Position.Z + _camera.Front.Z * cameraSpeed * (float)e.Time); // Forward
                }
                if (input.IsKeyDown(Keys.S))
                {
                    _transform.Position = new Vector3(_transform.Position.X - _camera.Front.X * cameraSpeed * (float)e.Time, _transform.Position.Y, _transform.Position.Z - _camera.Front.Z * cameraSpeed * (float)e.Time); // Backwards
                }
                if (input.IsKeyDown(Keys.A))
                {
                    _transform.Position = new Vector3(_transform.Position.X - _camera.Right.X * cameraSpeed * (float)e.Time, _transform.Position.Y, _transform.Position.Z - _camera.Right.Z * cameraSpeed * (float)e.Time); // Left
                }
                if (input.IsKeyDown(Keys.D))
                {
                    _transform.Position = new Vector3(_transform.Position.X + _camera.Right.X * cameraSpeed * (float)e.Time, _transform.Position.Y, _transform.Position.Z + _camera.Right.Z * cameraSpeed * (float)e.Time);// Right
                }
                if (input.IsKeyPressed(Keys.Space))
                {
                    Jump();
                }
            }

            if (input.IsKeyPressed(Keys.F))
            {
                _flashlight.ToggleLight();
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

        private void Jump()
        {
            if (isGrounded)
            {
                _rigidBody.RigidBody!.ApplyImpulse(new System.Numerics.Vector3(0.0f, 3.0f, 0.0f), DataManipulationService.OpenTKVectorToBulletVector(_transform.Position));
            }
        }
    }
}
