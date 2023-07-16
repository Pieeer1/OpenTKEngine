using BepuPhysics;
using BepuUtilities;
using BepuPhysics.Collidables;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKEngine.Attributes;
using OpenTKEngine.Enums;
using OpenTKEngine.Models;
using OpenTKEngine.Services;
using OpenTKEngine.Models.Physics;
using System.Security.Cryptography;

namespace OpenTKEngine.Entities.Components
{
    public class PlayerComponent : Component
    {
        private TransformComponent _transform = null!;
        private CameraComponent _camera = null!;
        private SpotLightComponent _flashlight = null!;
        private BoxRigidComponent _boxRigidComponent = null!;
        private BasicHitHandler _hitHandler;
        private BodyReference _bodyReference;
        private float _moveSpeed = 50.0f;

        private bool isGrounded { 
            get
            {
                if (_hitHandler.HasHit)
                {
                    return true;
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
            _boxRigidComponent = Entity.AddComponent(new BoxRigidComponent(new BepuPhysics.Collidables.Box(1.0f, 2.0f, 1.0f), 3.0f));

            _flashlight = Entity.AddComponent(new SpotLightComponent(_shader, _transform.Position));


            _hitHandler = new BasicHitHandler() { };
            _bodyReference = PhysicsService.Instance.Simulation.Bodies[_boxRigidComponent._handle];

        }

        public override void Draw()
        {
            base.Draw();
        }
        public override void Update()
        {
            base.Update();

            PhysicsService.Instance.Simulation.RayCast(DataManipulationService.OpenTKVectorToSystemVector(_transform.Position), new System.Numerics.Vector3(0.0f, -1.0f, 0.0f), 100.0f, ref _hitHandler, 0);

        }
        public override void UpdateInput(FrameEventArgs e, KeyboardState input, MouseState mouse, ref bool firstMove, ref Vector2 lastPos)
        {
            base.UpdateInput(e, input, mouse, ref firstMove, ref lastPos);
            if ((ActiveInputFlags & InputFlags.Player) == 0) { return; } //TODO KEEP PHYSICS WORKING IN BACKGROUND REGARDLESS
            float cameraSpeed = 1.5f;
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
                _bodyReference.Awake = true;


                Matrix4 viewMatrix = _camera.GetViewMatrix();

                Vector3 forward = new Vector3(viewMatrix.M31, viewMatrix.M32, viewMatrix.M33);

                Vector3 right = new Vector3(viewMatrix.M11, viewMatrix.M12, viewMatrix.M13);

                Vector3 moveDirection = (forward * GetVerticalInput(input)) + (right * GetHorizontalInput(input));
                moveDirection.Y = 0f;
                if (moveDirection.X != 0 && moveDirection.Z != 0)
                {
                    _bodyReference.ApplyImpulse(DataManipulationService.OpenTKVectorToSystemVector(moveDirection.Normalized() * _moveSpeed), DataManipulationService.OpenTKVectorToSystemVector(new Vector3(0.0f, 0.0f, 0.0f)));
                }
                if (isGrounded)
                {
                    _bodyReference.Velocity.Linear = new System.Numerics.Vector3(_bodyReference.Velocity.Linear.X * 0.95f * (float)TimeService.Instance.DeltaTime/0.05f, _bodyReference.Velocity.Linear.Y, _bodyReference.Velocity.Linear.Z * 0.95f * (float)TimeService.Instance.DeltaTime/0.05f);
                }

                Vector3 flatVel = new Vector3(_bodyReference.Velocity.Linear.X, 0.0f, _bodyReference.Velocity.Linear.Z);
                Vector3 flatAng = new Vector3(_bodyReference.Velocity.Angular.X, 0.0f, _bodyReference.Velocity.Angular.Z);
                if (flatVel.Length > _moveSpeed)
                {
                    Vector3 limitedVel = flatVel.Normalized() * _moveSpeed;
                    _bodyReference.Velocity.Linear.X = limitedVel.X;
                    _bodyReference.Velocity.Linear.Z = limitedVel.Z;
                }
                if (flatAng.Length > _moveSpeed)
                {
                    Vector3 limitedAng = flatAng.Normalized() * _moveSpeed;
                    _bodyReference.Velocity.Angular.X = limitedAng.X;
                    _bodyReference.Velocity.Angular.Z = limitedAng.Z;
                }

                if (input.IsKeyDown(Keys.LeftShift)) // sprint
                {
                    _moveSpeed = 100.0f;
                }
                else
                {
                    _moveSpeed = 50.0f;
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
                _bodyReference.ApplyImpulse(new System.Numerics.Vector3(0.0f, 15.0f, 0.0f), DataManipulationService.OpenTKVectorToSystemVector(new Vector3(0.0f, 0.0f, 0.0f)));
            }
        }
        public float GetVerticalInput(KeyboardState input)
        {
            float verticalInput = 0f;

            if (input.IsKeyDown(Keys.W))
            {
                verticalInput = -1f;
            }
            if (input.IsKeyReleased(Keys.W))
            {
                verticalInput = 0.0f;
            }

            if (input.IsKeyDown(Keys.S))
            {
                verticalInput = 1f;
            }
            if (input.IsKeyReleased(Keys.S))
            {
                verticalInput = 0.0f;
            }
            return verticalInput;
        }

        public float GetHorizontalInput(KeyboardState input)
        {
            float horizontalInput = 0f;

            if (input.IsKeyDown(Keys.D))
            {
                horizontalInput = 1f;
            }
            if (input.IsKeyReleased(Keys.D))
            {
                horizontalInput = 0.0f;
            }
            if (input.IsKeyDown(Keys.A))
            {
                horizontalInput = -1f;
            }
            if (input.IsKeyReleased(Keys.A))
            {
                horizontalInput = 0.0f;
            }
            return horizontalInput;
        }

    }
}
