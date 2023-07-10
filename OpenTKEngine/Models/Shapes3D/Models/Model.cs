using OpenTKEngine.Entities.Components;
using Assimp;

namespace OpenTKEngine.Models.Shapes3D.Models
{
    public class Model
    {
        private readonly string _path;
        private readonly List<Mesh> _meshes = new List<Mesh>();
        private Scene _scene = null!;
        public Model(string path)
        {
            _path = path;
        }

        public void Init(Shader shader)
        {
            LoadModel();
            foreach (Mesh mesh in _meshes)
            {
                mesh.BindAndBuffer(shader);
            }
        }
        public void Draw(Shader shader, TransformComponent transform)
        {
            foreach (Mesh mesh in _meshes)
            {
                mesh.Draw(shader, transform);
            }
        }

        private void LoadModel() 
        {
            AssimpContext assimpContext = new AssimpContext();
            //_scene = assimpContext.ImportFileFromStream(DataManipulationService.GenerateStreamFromString(Encoding.UTF8.GetString(File.ReadAllBytes(_path))));
            _scene = assimpContext.ImportFile(_path, PostProcessSteps.Triangulate | PostProcessSteps.FlipUVs);

            //foreach (Assimp.Material material in _scene.Materials)
            //{
            //    TextureSlot[] textureSlots = material.GetAllMaterialTextures();
            //    foreach (TextureSlot textureSlot in textureSlots)
            //    {
            //        string texturePath = textureSlot.FilePath;
            //        Texture texture = Texture.LoadFromFile(texturePath);
            //    }
            //}

            foreach (Assimp.Mesh mesh in _scene.Meshes)
            {
                List<float> vertices = new List<float>();
                for (int i = 0; i < mesh.VertexCount; i++)
                {
                    //positions
                    vertices.Add(mesh.Vertices[i].X);
                    vertices.Add(mesh.Vertices[i].Y);
                    vertices.Add(mesh.Vertices[i].Z);

                    vertices.Add(mesh.Normals[i].X);
                    vertices.Add(mesh.Normals[i].Y);
                    vertices.Add(mesh.Normals[i].Z);
                    //textures
                    vertices.Add(mesh.TextureCoordinateChannels[0][i].X);
                    vertices.Add(mesh.TextureCoordinateChannels[0][i].Y); //MAY COME BACK HERE FOR TEXTURE ISSUES. DOES NOT SEEM CORRECT
                }
                List<uint> indices = new List<uint>();
                foreach (Assimp.Face face in mesh.Faces)
                {
                    foreach (uint index in face.Indices)
                    {
                        indices.Add(index);
                    }
                }
                _meshes.Add(new Mesh(vertices.ToArray(), indices.ToArray()));
            }
        }
    }
}
