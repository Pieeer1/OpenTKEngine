using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTKEngine.Attributes;
using OpenTKEngine.Entities.Components;
using System.Runtime.CompilerServices;

namespace OpenTKEngine.Models.UI
{
    public class Canvas : RenderableObject
    {
        private Shader _shader;
        public Vector2 ScreenSize { get; set; }
        public double ElapsedTime { get; set; }
        private int _texture;
        private int _vboSize = 10000;
        private int _eboSize = 2000;
        public Canvas(Shader shader)
        {
            _shader = shader;
            IntPtr context = ImGui.CreateContext();
            ImGui.SetCurrentContext(context);
            ImGuiIOPtr io = ImGui.GetIO();
            io.Fonts.AddFontDefault();
            io.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;
            BindAndBuffer(_shader);

            SetPerFrameImGuiData(1f / 60f);

            ImGui.NewFrame();    
        }

        public override void BindAndBuffer(Shader shader)
        {
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);
            
            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, _vboSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);

            EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO??0);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _eboSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);

            CreateFontTexture();

            int stride = Unsafe.SizeOf<ImDrawVert>();
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, stride, 0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, stride, 8);
            GL.VertexAttribPointer(2, 4, VertexAttribPointerType.UnsignedByte, true, stride, 16);

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);

            GL.BindVertexArray(0);

        }
        private void SetPerFrameImGuiData(double time)
        {
            ImGui.GetIO().DisplaySize = new System.Numerics.Vector2(ScreenSize.X, ScreenSize.Y);
            ImGui.GetIO().DisplayFramebufferScale = System.Numerics.Vector2.One;
            ImGui.GetIO().DeltaTime = (float)ElapsedTime;
        }
        public override void Draw(Shader shader, TransformComponent transform)
        {
            ImGui.Render();

            SetPerFrameImGuiData(ElapsedTime);

            ImGui.NewFrame();

            ImGui.ShowDemoWindow();

            ImGui.Render();


            ImDrawDataPtr drawDataPtr = ImGui.GetDrawData();

            if (drawDataPtr.CmdListsCount == 0)
            {
                return;
            }

            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);

            for (int i = 0; i < drawDataPtr.CmdListsCount; i++)
            {
                ImDrawListPtr drawListPtr = drawDataPtr.CmdListsRange[i];

                int vertexSize = drawListPtr.VtxBuffer.Size * Unsafe.SizeOf<ImDrawVert>();
                if (vertexSize > _vboSize)
                {
                    int newSize = (int)Math.Max(_vboSize * 1.5f, vertexSize);

                    GL.BufferData(BufferTarget.ArrayBuffer, newSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);
                    _vboSize = newSize;
                }

                int indexSize = drawListPtr.IdxBuffer.Size * sizeof(ushort);
                if (indexSize > _eboSize)
                {
                    int newSize = (int)Math.Max(_eboSize * 1.5f, indexSize);

                    GL.BufferData(BufferTarget.ElementArrayBuffer, newSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);
                    _eboSize = newSize;
                }
            }
            ImGuiIOPtr io = ImGui.GetIO();
            Matrix4 mvp = Matrix4.CreateOrthographicOffCenter(
                0.0f,
                io.DisplaySize.X,
                io.DisplaySize.Y,
                0.0f,
                -1.0f,
                1.0f);

            _shader.Use();
            _shader.SetMatrix4("projection", false, mvp);

            GL.Uniform1(_shader.Handle, 0);

            GL.BindVertexArray(VAO);
            drawDataPtr.ScaleClipRects(io.DisplayFramebufferScale);

            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.ScissorTest);
            GL.BlendEquation(BlendEquationMode.FuncAdd);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.DepthTest);

            for (int n = 0; n < drawDataPtr.CmdListsCount; n++)
            {
                ImDrawListPtr drawListPtr = drawDataPtr.CmdListsRange[n];

                GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, drawListPtr.VtxBuffer.Size * Unsafe.SizeOf<ImDrawVert>(), drawListPtr.VtxBuffer.Data);

                GL.BufferSubData(BufferTarget.ElementArrayBuffer, IntPtr.Zero, drawListPtr.IdxBuffer.Size * sizeof(ushort), drawListPtr.IdxBuffer.Data);

                for (int cmd_i = 0; cmd_i < drawListPtr.CmdBuffer.Size; cmd_i++)
                {
                    ImDrawCmdPtr pcmd = drawListPtr.CmdBuffer[cmd_i];    
                    
                    GL.ActiveTexture(TextureUnit.Texture0);
                    GL.BindTexture(TextureTarget.Texture2D, (int)pcmd.TextureId);

                    var clip = pcmd.ClipRect;
                    GL.Scissor((int)clip.X, (int)ScreenSize.Y - (int)clip.W, (int)(clip.Z - clip.X), (int)(clip.W - clip.Y));

                    if ((io.BackendFlags & ImGuiBackendFlags.RendererHasVtxOffset) != 0)
                    {
                        GL.DrawElementsBaseVertex(PrimitiveType.Triangles, (int)pcmd.ElemCount, DrawElementsType.UnsignedShort, (IntPtr)(pcmd.IdxOffset * sizeof(ushort)), unchecked((int)pcmd.VtxOffset));
                    }
                    else
                    {
                        GL.DrawElements(BeginMode.Triangles, (int)pcmd.ElemCount, DrawElementsType.UnsignedShort, (int)pcmd.IdxOffset * sizeof(ushort));
                    }
                    
                }
            }

            GL.Disable(EnableCap.ScissorTest);
            GL.Enable(EnableCap.DepthTest);

        }


        private void CreateFontTexture()
        {
            ImGuiIOPtr io = ImGui.GetIO();
            io.Fonts.GetTexDataAsRGBA32(out IntPtr pixels, out int width, out int height, out int bytesPerPixel);

            int mips = (int)Math.Floor(Math.Log(Math.Max(width, height), 2));

            int prevActiveTexture = GL.GetInteger(GetPName.ActiveTexture);
            GL.ActiveTexture(TextureUnit.Texture0);
            int prevTexture2D = GL.GetInteger(GetPName.TextureBinding2D);

            _texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _texture);
            GL.TexStorage2D(TextureTarget2d.Texture2D, mips, SizedInternalFormat.Rgba8, width, height);

            GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, width, height, PixelFormat.Bgra, PixelType.UnsignedByte, pixels);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, mips - 1);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);

            // Restore state
            GL.BindTexture(TextureTarget.Texture2D, prevTexture2D);
            GL.ActiveTexture((TextureUnit)prevActiveTexture);

            io.Fonts.SetTexID((IntPtr)_texture);

            io.Fonts.ClearTexData();
        }
    }
}
