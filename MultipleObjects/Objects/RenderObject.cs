using Common;
using OpenTK.Graphics.OpenGL4;
using System.Numerics;

namespace MultipleObjects.Objects
{
    internal class RenderObject : IRenderObject
    {
        private readonly IVertex2[] _vertices = new IVertex2[]
        {
            new TextureVertex2(new Vector2(320f, 0f), new Vector2(0.5f, 0f)),
            new TextureVertex2(new Vector2(320f, 320f), new Vector2(0.5f, 0.5f)),
            new TextureVertex2(new Vector2(0f, 320f), new Vector2(0f, 0.5f)),
            new TextureVertex2(new Vector2(0f, 0f), new Vector2(0f, 0f)), 
        };

        private uint[] _indices =
        {
            0, 1, 3,
            1, 2, 3
        };

        private int _vao;

        private int _vbo;

        private int _ebo;

        private Texture? _texture;
        public void OnLoad(Shader shader)
        {
            _vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();
            _ebo = GL.GenBuffer();

            GL.BindVertexArray(_vao);

            // bind vbo and set data for vbo
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            var vertices = _vertices.GetRaw();
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // bind ebo and set data for ebo
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            shader.EnableAttribs(TextureVertex2.AttribLocations);

            _texture = Texture.Load("Resources/Item1.png", TextureUnit.Texture0);
        }

        public void OnRenderFrame(Shader shader)
        {
            // Bind the VAO
            GL.BindVertexArray(_vao);
            shader.SetInt("aTexture", 0);

            // Enable Alpha
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        public void OnUnload()
        {
            // Unbind all the resources by binding the targets to 0/null.
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BindVertexArray(_vao);

            // Delete all the resources.
            GL.DeleteBuffer(_vbo);
            GL.DeleteVertexArray(_vao);
        }
    }
}
