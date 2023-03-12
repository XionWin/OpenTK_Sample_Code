using Common;
using Extension;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Drawing;

namespace MultipleObjects.Objects
{
    internal class RenderObject : IRenderObject
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

            _texture = new Texture(TextureUnit.Texture0, TextureMinFilter.Nearest).With(x => x.Load("Resources/Sprite.png"));
        }

        public PointF Location { get; set; } = new PointF(100, 100);
        public SizeF Size { get; set; } = new SizeF(38 * 2, 45 * 2);

        public RectangleF Coordinate { get; set; } = new RectangleF(0, 0, 38, 45);
        public SizeF TextureSize { get; set; } = new SizeF(256, 256);

        public void OnRenderFrame(Shader shader)
        {
            // Bind the VAO
            GL.BindVertexArray(_vao);

            // Change vertices data
            _vertices = new IVertex2[]
            {
                new TextureVertex2(new Vector2(0, 0), new Vector2(this.Coordinate.X / this.TextureSize.Width, this.Coordinate.Y / this.TextureSize.Height)),
                new TextureVertex2(new Vector2(this.Size.Width, 0), new Vector2(this.Coordinate.X / this.TextureSize.Width + this.Coordinate.Width/this.TextureSize.Width, this.Coordinate.Y / this.TextureSize.Height)),
                new TextureVertex2(new Vector2(this.Size.Width,  this.Size.Height), new Vector2(this.Coordinate.X / this.TextureSize.Width + this.Coordinate.Width/this.TextureSize.Width, this.Coordinate.Y / this.TextureSize.Height + this.Coordinate.Height/this.TextureSize.Height)),
                new TextureVertex2(new Vector2(0, this.Size.Height), new Vector2(this.Coordinate.X / this.TextureSize.Width, this.Coordinate.Y / this.TextureSize.Height + this.Coordinate.Height/this.TextureSize.Height)),
            };
            // bind vbo and set data for vbo
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            var vertices = _vertices.GetRaw();
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            var transform = Matrix4.Identity;
            transform = transform * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(DateTime.Now.Ticks / 100000 % 360 + this.Location.X + this.Location.Y));
            Matrix4.CreateTranslation(this.Location.X, this.Location.Y, 0f, out var t);
            transform = transform * t;
            shader.SetMatrix4("aTransform", transform);


            shader.SetVector2("aCenter", new Vector2(this.Size.Width / 2f, this.Size.Height / 2f));

            // Active texture
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
