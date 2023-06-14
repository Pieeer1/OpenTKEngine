using OpenTK.Compute.OpenCL;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTKEngine.Entities;
using OpenTKEngine.Entities.Components;
using OpenTKEngine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKEngine.Models.Shapes3D
{
    public class Sphere : Shape3D
    {
        public override float[] _vertices { get => CreateSphere(); }
        public uint[] _indices { get => CreateSphereIndices(_vertices); }
        public override int VAO { get; set; }

        private const int xSegments = 64;
        private const int ySegments = 64;

        public override void BindAndBuffer(Shader shader)
        {
            int vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            int elementArrayBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementArrayBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);


            var positionLocation = shader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

            var normalLocation = shader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation);
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

            var texCoordLocation = shader.GetAttribLocation("aTexCoords");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
        }

        private float[] CreateSphere()
        {
            List<Vector3> positions = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> textures = new List<Vector2>();

            for (int x = 0; x <= xSegments; ++x)
            {
                for (int y = 0; y <= ySegments; ++y)
                {
                    float xSegment = (float)x / (float)xSegments;
                    float ySegment = (float)y / (float)ySegments;
                    float xPos = (float)(MathHelper.Cos(xSegment * 2.0f * MathHelper.Pi) * MathHelper.Sin(ySegment * MathHelper.Pi));
                    float yPos = (float)MathHelper.Cos(ySegment * MathHelper.Pi);
                    float zPos = (float)(MathHelper.Sin(xSegment * 2.0f * MathHelper.Pi) * MathHelper.Sin(ySegment * MathHelper.Pi));

                    positions.Add(new Vector3(xPos, yPos, zPos));
                    normals.Add(new Vector3(xPos, yPos, zPos));
                    textures.Add(new Vector2(xSegment, ySegment));
                }
            }

            return DataManipulationService.GetTextureShadedArrayFromVectors(positions, normals, textures).Get1DFrom2D().ToArray();
        }
        private uint[] CreateSphereIndices(float[] vertices)
        {
            List<uint> indices = new List<uint>();
            for (int y = 0; y < ySegments; y++)
            {
                if (y % 2 == 0)
                {
                    for (uint x = 0; x <= xSegments; ++x)
                    {
                        indices.Add((uint)(y * (xSegments + 1) + x));
                        indices.Add((uint)((y + 1) * (xSegments + 1) + x));
                    }
                }
                else
                {
                    for (int x = xSegments; x >= 0; --x)
                    {
                        indices.Add((uint)((y + 1) * (xSegments + 1) + x));
                        indices.Add((uint)(y * (xSegments + 1) + x));
                    }
                }
            }

            return indices.ToArray();
        }

        public override void Draw(Shader shader, TransformComponent transform)
        {
            GL.BindVertexArray(VAO);

            CameraComponent camera = EntityComponentManager.Instance.GetEntitiesWithType<CameraComponent>().FirstOrDefault()?.GetComponent<CameraComponent>() ?? throw new NullReferenceException("No Camera In Scene");

            shader.SetMatrix4("view", camera.GetViewMatrix());
            shader.SetMatrix4("projection", camera.GetProjectionMatrix());

            Matrix4 model = Matrix4.CreateTranslation(transform.Position);
            Matrix4.CreateFromQuaternion(transform.Rotation, out Matrix4 rotationModel);
            model = model * rotationModel * Matrix4.CreateScale(transform.Scale);
            shader.SetMatrix4("model", model);

            GL.DrawElements(PrimitiveType.TriangleStrip, _indices.Count(), DrawElementsType.UnsignedInt, 0);
        }
    }
}
