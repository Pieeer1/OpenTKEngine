using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTKEngine.Entities.Components;
using OpenTKEngine.Scenes;
using System.Reflection.Metadata;

namespace OpenTKEngine.Models.Skybox
{
    public class Skybox : RenderableObject
    {
        private float[] skyboxVertices = new float[]{        
                -1.0f,  1.0f, -1.0f,
                -1.0f, -1.0f, -1.0f,
                 1.0f, -1.0f, -1.0f,
                 1.0f, -1.0f, -1.0f,
                 1.0f,  1.0f, -1.0f,
                -1.0f,  1.0f, -1.0f,

                -1.0f, -1.0f,  1.0f,
                -1.0f, -1.0f, -1.0f,
                -1.0f,  1.0f, -1.0f,
                -1.0f,  1.0f, -1.0f,
                -1.0f,  1.0f,  1.0f,
                -1.0f, -1.0f,  1.0f,

                 1.0f, -1.0f, -1.0f,
                 1.0f, -1.0f,  1.0f,
                 1.0f,  1.0f,  1.0f,
                 1.0f,  1.0f,  1.0f,
                 1.0f,  1.0f, -1.0f,
                 1.0f, -1.0f, -1.0f,

                -1.0f, -1.0f,  1.0f,
                -1.0f,  1.0f,  1.0f,
                 1.0f,  1.0f,  1.0f,
                 1.0f,  1.0f,  1.0f,
                 1.0f, -1.0f,  1.0f,
                -1.0f, -1.0f,  1.0f,

                -1.0f,  1.0f, -1.0f,
                 1.0f,  1.0f, -1.0f,
                 1.0f,  1.0f,  1.0f,
                 1.0f,  1.0f,  1.0f,
                -1.0f,  1.0f,  1.0f,
                -1.0f,  1.0f, -1.0f,

                -1.0f, -1.0f, -1.0f,
                -1.0f, -1.0f,  1.0f,
                 1.0f, -1.0f, -1.0f,
                 1.0f, -1.0f, -1.0f,
                -1.0f, -1.0f,  1.0f,
                 1.0f, -1.0f,  1.0f
            };

        private Texture _cubeMapTexture = null!;
        private IEnumerable<string> _faces;

        public Skybox(IEnumerable<string> faces)
        {
            _faces = faces;
        }

        public override void BindAndBuffer(Shader shader)
        {
            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, skyboxVertices.Length * sizeof(float), skyboxVertices, BufferUsageHint.StaticDraw);

            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            var positionLocation = shader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);


            _cubeMapTexture = Texture.LoadCubemap(_faces);
        }

        public override void Draw(Shader shader, TransformComponent transform)
        {
            GL.DepthMask(false);
            GL.DepthFunc(DepthFunction.Lequal);

            shader.Use();

            GL.BindVertexArray(VAO);

            _cubeMapTexture.Use(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.TextureCubeMap, _cubeMapTexture.Handle);

            CameraComponent camera = SceneManager.Instance.ActiveScene.EntityComponentManager.GetEntitiesWithType<CameraComponent>().FirstOrDefault()?.GetComponent<CameraComponent>() ?? throw new NullReferenceException("No Camera In Scene");

            Matrix4 matWithoutFinal = new Matrix4(new Matrix3(camera.GetViewMatrix()));

            shader.SetMatrix4("view", false, matWithoutFinal);
            shader.SetMatrix4("projection", false, camera.GetProjectionMatrix());

            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            GL.DepthMask(true);


            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.BindVertexArray(0);

            GL.DepthFunc(DepthFunction.Less);
        }
    }
}
