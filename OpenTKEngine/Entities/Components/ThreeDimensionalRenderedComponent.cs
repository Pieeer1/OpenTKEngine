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
        protected readonly Vector3 _position;
        protected readonly AxisAngle? _rotation;
        protected readonly Vector3? _scale;
        protected TransformComponent? _transform;
        protected List<Texture> _textures = new List<Texture>();
        public ThreeDimensionalRenderedComponent(Shader shader, Vector3 position, AxisAngle? rotation = null, Vector3? scale = null, List<Texture>? textures = null)
        {
            _shader = shader;
            _position = position;
            _rotation = rotation;
            _scale = scale;
            _textures = textures ?? new List<Texture>();
        }
        public ThreeDimensionalRenderedComponent(Shader shader, TransformComponent transform, List<Texture>? textures = null)
        {
            _shader = shader;
            _transform = transform;
            _textures = textures ?? new List<Texture>();
        }
        public override void Init()
        {
            base.Init();

            BindAndBuffer();

            _transform = Entity.AddComponent(_transform ?? new TransformComponent(_position, _rotation, _scale));
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
