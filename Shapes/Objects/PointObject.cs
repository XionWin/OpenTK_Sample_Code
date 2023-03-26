using Common;
using Extension;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Drawing;
using System.Linq;

namespace Shapes.Objects
{
    internal class PointObject : ColorObject
    {
        private IVertex2[]? _vertices = null;
        public override IVertex2[]? Vertices => this._vertices;


        public int Size { get; set; } = 1;

        public override Point Center => this.Location;

        public PointObject(Point location, Vector3 color) : base(location, color) { }


        public override void OnLoad(Shader shader)
        {
            var step = this.Size * 8;
            // Change vertices data
            var points = Enumerable.Range(0, step).Select(x => new PointF((float)Math.Cos(Math.PI * 2 / step *  x) * Size / 2 + Location.X, (float)Math.Sin(Math.PI * 2 / step * x) * Size / 2 + Location.Y));
            this._vertices = points.Select(x => new ColorVertex2(new Vector2(x.X, x.Y), this.Color)).Cast<IVertex2>().ToArray();

            base.OnLoad(shader);

            shader.EnableAttribs(ColorVertex2.AttribLocations);
        }

        public override void SetParameters(Shader shader)
        {
            base.SetParameters(shader);
        }


        public override void OnRenderFrame(Shader shader)
        {
            var step = this.Size * 8;
            var points = Enumerable.Range(0, step).Select(x => new PointF((float)Math.Cos(Math.PI * 2 / step * x) * Size / 2 + Location.X, (float)Math.Sin(Math.PI * 2 / step * x) * Size / 2 + Location.Y));
            this._vertices = points.Select(x => new ColorVertex2(new Vector2(x.X, x.Y), this.Color)).Cast<IVertex2>().ToArray();
            this.SetVBO();

            base.OnRenderFrame(shader);

            GL.DrawArrays(PrimitiveType.TriangleFan, 0, this.Vertices!.Length);

        }

    }
}
