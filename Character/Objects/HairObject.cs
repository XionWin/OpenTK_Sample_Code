using Common;
using Extension;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Drawing;

namespace Character.Objects
{
    internal class HairObject : IRenderObject
    {
        private IVertex2[] _vertices = new IVertex2[0];

        private uint[] _indices =
        {
            0, 1, 3,
            1, 2, 3
        };

        private int _vao;

        private int _vbo;

        private int _ebo;


        public PointF Location { get; set; }
        public SizeF Size { get; set; }

        public RectangleF Coordinate { get; set; }
        public Texture? Texture { get; set; }


        private int[] _actionTable = new int[]
        {
            7, 7, 7, 7,
            8, 8, 8, 8,
            9, 9, 9, 9,
            6, 6, 6, 6,
            13, 13, 13, 13,
            6
        };


        public long Tick { get; set; }
        public int Action { get; set; }
        public float Light { get; set; }

        private Vector3 _color;
        public HairObject(SizeF size, RectangleF coordinate, Texture? texture)
        {
            this.Size = size;
            this.Coordinate = coordinate;
            this.Texture = texture;

            var random = new Random();
            var light = random.NextSingle();
            _color = new Vector3(random.NextSingle(), random.NextSingle(), random.NextSingle());
        }


        public void OnLoad(Shader shader)
        {
            // Change vertices data
            _vertices = new IVertex2[]
            {
                new ColorTextureVertex2(new Vector2(0, 0), _color, new Vector2(this.Coordinate.X / this.Texture!.Size.X, this.Coordinate.Y / this.Texture!.Size.Y)),
                new ColorTextureVertex2(new Vector2(this.Size.Width, 0), _color, new Vector2(this.Coordinate.X / this.Texture!.Size.X + this.Coordinate.Width/this.Texture!.Size.X, this.Coordinate.Y / this.Texture!.Size.Y)),
                new ColorTextureVertex2(new Vector2(this.Size.Width,  this.Size.Height), _color, new Vector2(this.Coordinate.X / this.Texture!.Size.X + this.Coordinate.Width/this.Texture!.Size.X, this.Coordinate.Y / this.Texture!.Size.Y + this.Coordinate.Height/this.Texture!.Size.Y)),
                new ColorTextureVertex2(new Vector2(0, this.Size.Height), _color, new Vector2(this.Coordinate.X / this.Texture!.Size.X, this.Coordinate.Y / this.Texture!.Size.Y + this.Coordinate.Height/this.Texture!.Size.Y)),
            };


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

            shader.EnableAttribs(ColorTextureVertex2.AttribLocations);

        }

        public void OnRenderFrame(Shader shader)
        {
            // Bind the VAO
            GL.BindVertexArray(_vao);

            var transform = Matrix4.Identity;
            //transform = transform * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(DateTime.Now.Ticks / 100000 % 360));
            Matrix4.CreateTranslation(this.Location.X, this.Location.Y, 0f, out var t);
            transform = transform * t;
            shader.UniformMatrix4("aTransform", transform);


            shader.Uniform2("aCenter", new Vector2(this.Size.Width / 2f, this.Size.Height / 2f));

            // Active texture
            shader.Uniform1("aTexture", 3);

            var indexTick = this.Tick % (_actionTable[this.Action]);
            shader.Uniform2("aTexOffset", new Vector2(1f / 13f * indexTick, 1f / 21f * this.Action));

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
