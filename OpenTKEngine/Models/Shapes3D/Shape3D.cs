using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTKEngine.Entities;
using OpenTKEngine.Entities.Components;

namespace OpenTKEngine.Models.Shapes3D
{
    public abstract class Shape3D : RenderableObject
    {
        public abstract float[] _vertices { get; }
        private protected void ArrayBuffer(Shader shader)
        {
            int vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

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
        private protected void ElementArrayBuffer(Shader shader, uint[] indices)
        {
            int vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            int elementArrayBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementArrayBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);


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
        private protected void DrawShape(Shader shader, TransformComponent transform, Action glDraw)
        {
            GL.BindVertexArray(VAO);

            CameraComponent camera = EntityComponentManager.Instance.GetEntitiesWithType<CameraComponent>().FirstOrDefault()?.GetComponent<CameraComponent>() ?? throw new NullReferenceException("No Camera In Scene");

            shader.SetMatrix4("view", camera.GetViewMatrix());
            shader.SetMatrix4("projection", camera.GetProjectionMatrix());

            Matrix4 model = Matrix4.CreateTranslation(transform.Position);
            model = model * Matrix4.CreateFromAxisAngle(transform.Rotation.Axis, transform.Rotation.Angle) * Matrix4.CreateScale(transform.Scale);
            shader.SetMatrix4("model", model);

            glDraw.Invoke();
        }
    }
}
