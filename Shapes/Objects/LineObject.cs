using Common;
using Extension;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Drawing;

namespace Shapes.Objects
{
    internal class LineObject : ColorObject
    {
        private IVertex2[]? _vertices = null;
        public override IVertex2[]? Vertices => this._vertices;

        public Point Start { get; set; }
        public Point End { get; set; }

        public override Point Center => this.Location;


        public LineObject(Point start, Point end, Vector3 color) : base(new Point((start.X - end.X) / 2, (start.Y - end.Y) / 2), color)
        {
            this.Start = start;
            this.End = end;
        }


        public override void OnLoad(Shader shader)
        {
            // Change vertices data

            _vertices = new IVertex2[]
            {
                new ColorVertex2(new Vector2(Start.X, Start.Y), this.Color),
                new ColorVertex2(new Vector2(End.X, End.Y), this.Color),
            };
            base.OnLoad(shader);
            shader.EnableAttribs(ColorVertex2.AttribLocations);
        }

        public override void SetParameters(Shader shader)
        {
            base.SetParameters(shader);
            GL.LineWidth(1);
        }


        public override void OnRenderFrame(Shader shader)
        {
            _vertices = new IVertex2[]
            {
                new ColorVertex2(new Vector2(Start.X, Start.Y), this.Color),
                new ColorVertex2(new Vector2(End.X, End.Y), this.Color),
            };

            this.SetVBO();
            base.OnRenderFrame(shader);
            GL.DrawArrays(PrimitiveType.Lines, 0, this._vertices!.Length);

        }
    }
}
