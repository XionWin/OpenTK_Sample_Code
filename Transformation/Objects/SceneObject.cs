using Common;
using Extension;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using OpenTK.Mathematics;

namespace MultipleObjects.Objects
{
    internal class SceneObject : IRenderObject
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

        public SceneObject(SizeF size, RectangleF coordinate, Texture? texture)
        {
            this.Size = size;
            this.Coordinate = coordinate;
            this.Texture = texture;
        }

        public void OnLoad(Shader shader)
        {
            _vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();
            _ebo = GL.GenBuffer();

            GL.BindVertexArray(_vao); 

            // Change vertices data
            _vertices = new IVertex2[]
            {
                new ColorTextureVertex2(new Vector2(0, 0), new Vector3(1.0f, 1.0f, 1.0f), new Vector2(this.Coordinate.X / this.Texture!.Size.X, this.Coordinate.Y / this.Texture!.Size.Y)),
                new ColorTextureVertex2(new Vector2(this.Size.Width, 0), new Vector3(1.0f, 1.0f, 1.0f), new Vector2(this.Coordinate.X / this.Texture!.Size.X + this.Coordinate.Width/this.Texture!.Size.X, this.Coordinate.Y / this.Texture!.Size.Y)),
                new ColorTextureVertex2(new Vector2(this.Size.Width,  this.Size.Height), new Vector3(1.0f, 1.0f, 1.0f), new Vector2(this.Coordinate.X / this.Texture!.Size.X + this.Coordinate.Width/this.Texture!.Size.X, this.Coordinate.Y / this.Texture!.Size.Y + this.Coordinate.Height/this.Texture!.Size.Y)),
                new ColorTextureVertex2(new Vector2(0, this.Size.Height), new Vector3(1.0f, 1.0f, 1.0f), new Vector2(this.Coordinate.X / this.Texture!.Size.X, this.Coordinate.Y / this.Texture!.Size.Y + this.Coordinate.Height/this.Texture!.Size.Y)),
            };
            // bind vbo and set data for vbo
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            var vertices = _vertices.GetRaw();
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);



            //var vertices = _vertices.GetRaw();
            //GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

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
            shader.UniformMatrix4("aTransform", transform);


            shader.Uniform2("aCenter", new Vector2(this.Size.Width / 2f, this.Size.Height / 2f));

            // Active texture
            shader.Uniform1("aTexture", 0);

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
