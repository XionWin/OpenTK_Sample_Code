using Common;
using Extension;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using System.Numerics;

namespace MultipleObjects.Objects
{
    internal class SceneObject : IRenderObject
    {
        private static int IX = 1;
        private IVertex2[] _vertices = new IVertex2[0];

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

            //var vertices = _vertices.GetRaw();
            //GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // bind ebo and set data for ebo
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            shader.EnableAttribs(TextureVertex2.AttribLocations);

            _texture = new Texture(TextureUnit.Texture1, TextureMinFilter.Nearest).With(x => x.Load("Resources/Bg.png"));
        }

        public PointF Location { get; set; } = new PointF(0, 0);
        public SizeF Size { get; set; } = new SizeF(288 * 4, 200 * 4);

        public RectangleF Coordinate { get; set; } = new RectangleF(0, 0, 288, 200);
        public SizeF TextureSize { get; set; } = new SizeF(288, 200);

        public void OnRenderFrame(Shader shader)
        {
            // Bind the VAO
            GL.BindVertexArray(_vao);

            // Change vertices data
            _vertices = new IVertex2[]
            {
                new TextureVertex2(new Vector2(this.Location.X + this.Size.Width, this.Location.Y), new Vector2(this.Coordinate.X / this.TextureSize.Width + this.Coordinate.Width/this.TextureSize.Width, this.Coordinate.Y / this.TextureSize.Height)),
                new TextureVertex2(new Vector2(this.Location.X + this.Size.Width, this.Location.Y + this.Size.Height), new Vector2(this.Coordinate.X / this.TextureSize.Width + this.Coordinate.Width/this.TextureSize.Width, this.Coordinate.Y / this.TextureSize.Height + this.Coordinate.Height/this.TextureSize.Height)),
                new TextureVertex2(new Vector2(this.Location.X, this.Location.Y + this.Size.Height), new Vector2(this.Coordinate.X / this.TextureSize.Width, this.Coordinate.Y / this.TextureSize.Height + this.Coordinate.Height/this.TextureSize.Height)),
                new TextureVertex2(new Vector2(this.Location.X, this.Location.Y), new Vector2(this.Coordinate.X / this.TextureSize.Width, this.Coordinate.Y / this.TextureSize.Height)),
            };
            // bind vbo and set data for vbo
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            var vertices = _vertices.GetRaw();
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // Active texture
            shader.SetInt("aTexture", 1);

            // Enable Alpha
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.DstColor, BlendingFactor.Zero);

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
