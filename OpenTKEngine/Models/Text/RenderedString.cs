using FreeTypeSharp.Native;
using static FreeTypeSharp.Native.FT;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTKEngine.Entities.Components;
using static OpenTKEngine.Models.Constants;
using System.Reflection.Metadata;
using OpenTKEngine.Services;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.Mime.MediaTypeNames;
using OpenTKEngine.Entities;

namespace OpenTKEngine.Models.Text
{
    public class RenderedString : RenderableObject
    {

        private string _text;
        private float _x;
        private float _y;
        private float _scale;
        private Vector3 _color;
        private int _handle;
        private Dictionary<char, RChar> _characters = new Dictionary<char, RChar>();

        public RenderedString(string text, float x, float y, float scale, Vector3 color)
        {
            _text = text;
            _x = x;
            _y = y;
            _scale = scale;
            _color = color;
        }
        public override void BindAndBuffer(Shader shader)
        {

            Matrix4 projection = Matrix4.CreateOrthographicOffCenter(0.0f, 800.0f, 0.0f, 600.0f, 1.0f, -1.0f);

            GL.UniformMatrix4(GL.GetUniformLocation(shader.Handle, "projection"), false, ref projection);  

            unsafe
            {
                IntPtr library;
                FT_Error error;
                FT_FaceRec* face_ptr;

                error = FT_Init_FreeType(out library);

                error = FT_New_Face(library, FontRoutes.Corbel /*TODO: MAKE DYNAMIC*/, 0, out nint face);

                face_ptr = (FT_FaceRec*)face;

                FT_Set_Pixel_Sizes(face, 0, 48);

                GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
                for (uint c = 0; c < 128 /*basic ascii*/; c++)
                {
                    error = FT_Load_Char(face, (char)c, FT_LOAD_RENDER);
                    if (error != FT_Error.FT_Err_Ok)
                    {
                        throw new Exception("error");
                    }

                    _handle = GL.GenTexture();
                    GL.BindTexture(TextureTarget.Texture2D, _handle);
                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.R8, (int)face_ptr->glyph->bitmap.width, (int)face_ptr->glyph->bitmap.rows, 0, PixelFormat.Red, PixelType.UnsignedByte, face_ptr->glyph->bitmap.buffer);

                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Linear);

                    RChar rChar = new RChar((uint)_handle, new Vector2(face_ptr->glyph->bitmap.width, face_ptr->glyph->bitmap.rows), new Vector2(face_ptr->glyph->bitmap_left, face_ptr->glyph->bitmap_top), (uint)face_ptr->glyph->advance.x);
                    _characters.Add((char)c, rChar);
                }
                GL.BindTexture(TextureTarget.Texture2D, 0);
                FT_Done_Face(face);
                FT_Done_FreeType(library);
            }

            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * 6 * 4, 0, BufferUsageHint.DynamicDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        public override void Draw(Shader shader, TransformComponent transform)
        {
            RenderText(shader, _text, _x, _y, _scale, _color);
        }
        private void RenderText(Shader shader, string text, float x, float y, float scale, Vector3 color)
        {
            shader.Use();

            Matrix4 projection = Matrix4.CreateOrthographicOffCenter(0.0f, 800.0f, 0.0f, 600.0f, 1.0f, -1.0f);

            GL.UniformMatrix4(GL.GetUniformLocation(shader.Handle, "projection"), false, ref projection);

            GL.Uniform3(GL.GetUniformLocation(shader.Handle, "textColor"), color.X, color.Y, color.Z);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindVertexArray(VAO);

            for (int i = 0; i < text.Length; i++)
            {
                RChar rChar = _characters[(char)text[i]];
                float xPos = x + rChar.Bearing.X * scale;
                float yPos = y - (rChar.Size.Y - rChar.Bearing.Y) * scale;
                float w = rChar.Size.X * scale;
                float h = rChar.Size.Y * scale;
                float[,] vertices = new float[,]
                {
                    { xPos, yPos + h, 0.0f, 0.0f },
                    { xPos, yPos, 0.0f, 1.0f },
                    { xPos + w, yPos, 1.0f, 1.0f },

                    { xPos, yPos + h, 0.0f, 0.0f },
                    { xPos + w, yPos, 1.0f, 1.0f },
                    { xPos + w, yPos + h, 1.0f, 0.0f }
                };
                GL.BindTexture(TextureTarget.Texture2D, rChar.TextureId);
                GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
                GL.BufferSubData(BufferTarget.ArrayBuffer, 0, sizeof(float) * 6 * 4, vertices);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

                GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

                x += (rChar.Advance >> 6) * scale;
            }

            GL.BindVertexArray(0);
            GL.BindTexture(TextureTarget.Texture2D, 0);

        }

        private class RChar
        {
            public uint TextureId { get; private set; }
            public Vector2 Size { get; private set; }
            public Vector2 Bearing { get; private set; }
            public uint Advance { get; private set; }

            public RChar(uint textureId, Vector2 size, Vector2 bearing, uint advance)
            {
                TextureId = textureId;
                Size = size;
                Bearing = bearing;
                Advance = advance;
            }
        }
    }
}
