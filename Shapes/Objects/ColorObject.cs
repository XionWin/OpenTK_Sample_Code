using Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shapes.Objects
{
    internal abstract class ColorObject : RenderObject
    {
        public Vector3 Color { get; set; }


        public ColorObject(Point location, Vector3 color)
        {
            this.Location = location;
            this.Color = color;
        }

        public override void OnLoad(Shader shader) => base.OnLoad(shader);

        public override void SetParameters(Shader shader)
        {
            base.SetParameters(shader);
            // Active color mode
            shader.Uniform1("aMode", 0);
        }

        public override void OnRenderFrame(Shader shader) => base.OnRenderFrame(shader);

    }
}
