using OpenTKEngine.Models.Shapes3D;
using OpenTKEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System.Reflection.Metadata;

namespace OpenTKEngine.Entities.Components
{
    public abstract class ThreeDimensionalRenderedComponent : Component
    {
        protected readonly Shader _shader;
        protected List<Texture> _textures = new List<Texture>();
        public ThreeDimensionalRenderedComponent(Shader shader, List<Texture>? textures = null)
        {
            _shader = shader;
            _textures = textures ?? new List<Texture>();
        }
        public override void Init()
        {
            base.Init();

            BindAndBuffer();
        }
        public override void Draw()
        {
            base.Draw();

            for (int i = 0; i < _textures.Count; i++)
            {
                _textures[i].Use(TextureUnit.Texture0 + i);
            }

            DrawComp();

            for (int i = 0; i < _textures.Count; i++) // reset textures
            {
                _textures[i].Use(TextureUnit.Texture0 + i);
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }
        }
        public override void Update()
        {
            base.Update();
        }

        public abstract void BindAndBuffer();
        public abstract void DrawComp();
    }
}
