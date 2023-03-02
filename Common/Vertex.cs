using Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public struct Vertex2
    {
        public Vector2 Position { get; init; }
        public Vector2 Coordinate { get; init; }

        private float[]? raw = null;
        public float[] Raw => this.GetRaw();

        public Vertex2(Vector2 position, Vector2 coordinate)
        {
            this.Position = position;
            this.Coordinate = coordinate;
        }
    }

    public static class Vertex2Extension
    {
        public static float[] GetRaw(this Vertex2 vertex) => new[] { vertex.Position.X, vertex.Position.Y, vertex.Coordinate.X, vertex.Coordinate.Y };


        public static float[] GetRaw(this IEnumerable<Vertex2> vertices) => vertices.SelectMany(x => x.Raw).ToArray();
    }
}
