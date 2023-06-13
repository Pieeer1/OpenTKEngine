﻿using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKEngine.Models;

namespace OpenTKEngine.Entities.Components
{
    public class CameraComponent : Component
    {
        private Vector3 _front = -Vector3.UnitZ;
        private Vector3 _up = Vector3.UnitY;
        private Vector3 _right = Vector3.UnitX;

        private Shader _shader;

        private float _pitch;
        private float _yaw = -MathHelper.PiOver2;
        private float _fov = MathHelper.PiOver2;

        public TransformComponent Transform = null!;
        public float AspectRatio { private get; set; }

        public CameraComponent(Shader shader, float aspectRatio)
        {
            _shader = shader;
            AspectRatio = aspectRatio;
        }

        public Vector3 Front => _front;
        public Vector3 Up => _up;
        public Vector3 Right => _right;

        public float Pitch
        {
            get => MathHelper.RadiansToDegrees(_pitch);
            set
            {
                var angle = MathHelper.Clamp(value, -89f, 89f);
                _pitch = MathHelper.DegreesToRadians(angle);
                UpdateVectors();
            }
        }
        public float Yaw
        {
            get => MathHelper.RadiansToDegrees(_yaw);
            set
            {
                _yaw = MathHelper.DegreesToRadians(value);
                UpdateVectors();
            }
        }
        public float Fov
        {
            get => MathHelper.RadiansToDegrees(_fov);
            set
            {
                var angle = MathHelper.Clamp(value, 1f, 90f);
                _fov = MathHelper.DegreesToRadians(angle);
            }
        }
        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Transform.Position, Transform.Position + _front, _up);
        }
        public Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(_fov, AspectRatio, 0.01f, 100f);
        }
        private void UpdateVectors()
        {
            _front.X = MathF.Cos(_pitch) * MathF.Cos(_yaw);
            _front.Y = MathF.Sin(_pitch);
            _front.Z = MathF.Cos(_pitch) * MathF.Sin(_yaw);

            _front = Vector3.Normalize(_front);

            _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
            _up = Vector3.Normalize(Vector3.Cross(_right, _front));
        }

        public override void Init()
        {
            base.Init();

            Transform = Entity.AddComponent(new TransformComponent(Vector3.UnitZ * 3));
        }
        public override void Draw()
        {
            base.Draw();

            _shader.SetMatrix4("view", GetViewMatrix());
            _shader.SetMatrix4("projection", GetProjectionMatrix());

            _shader.SetVector3("viewPos", Transform.Position);

        }
        public override void Update()
        {
            base.Update();
        }

        public override void UpdateInput(FrameEventArgs e, KeyboardState input, MouseState mouse, ref bool firstMove, ref Vector2 lastPos)
        {
            const float cameraSpeed = 1.5f;
            const float sensitivity = 0.2f;

            if (input.IsKeyDown(Keys.W))
            {
                Transform.Position += Front * cameraSpeed * (float)e.Time; // Forward
            }
            if (input.IsKeyDown(Keys.S))
            {
                Transform.Position -= Front * cameraSpeed * (float)e.Time; // Backwards
            }
            if (input.IsKeyDown(Keys.A))
            {
                Transform.Position -= Right * cameraSpeed * (float)e.Time; // Left
            }
            if (input.IsKeyDown(Keys.D))
            {
                Transform.Position += Right * cameraSpeed * (float)e.Time; // Right
            }
            if (input.IsKeyDown(Keys.Space))
            {
                Transform.Position += Up * cameraSpeed * (float)e.Time; // Up
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                Transform.Position -= Up * cameraSpeed * (float)e.Time; // Down
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

                Yaw += deltaX * sensitivity;
                Pitch -= deltaY * sensitivity;
            }
        }
    }
}
