using Common;
using MultipleObjects.Objects;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;

namespace Transformation;
public class Window : GLWindow
{
    public Window() : base("Transformation", 288 * 4, 200 * 4)
    { }

    private IEnumerable<IRenderObject> _renderObjects = new IRenderObject[]
    {
        
        //Bg
        new SceneObject(),

        //tree 0
        new RenderObject()
        {
            Location = new PointF(100, 735 - 45 * 4),
            Size = new SizeF(38 * 4, 45 * 4),
            Coordinate = new RectangleF(0, 0, 38, 45)
        },
        // tree 1
        new RenderObject()
        {
            Location = new PointF(100 + 38 * 4 + 120, 735 - 45 * 4),
            Size = new SizeF(42 * 4, 45 * 4),
            Coordinate = new RectangleF(38, 0, 42, 45)
        },
        // bush 0
        new RenderObject()
        {
            Location = new PointF(100 + 38 * 4 + 120 + 42 * 4 + 50, 735 - 12 * 4),
            Size = new SizeF(16 * 4, 12 * 4),
            Coordinate = new RectangleF(38 + 42, 33, 16, 12)
        },

        // bush 1
        new RenderObject()
        {
            Location = new PointF(100 + 38 * 4 + 120 + 42 * 4 + 50 + 16 * 4 + 30, 735 - 12 * 4),
            Size = new SizeF(16 * 4, 12 * 4),
            Coordinate = new RectangleF(38 + 42, 33, 16, 12)
        },

        // whiteboard 0
        new RenderObject()
        {
            Location = new PointF(100 + 38 * 4 + 30, 735 - 15 * 4),
            Size = new SizeF(16 * 4, 15 * 4),
            Coordinate = new RectangleF(38 + 42, 18, 16, 15)
        },

        // ground
        new RenderObject()
        {
            Location = new PointF(0, 555 + 45 * 4),
            Size = new SizeF(45 * 4, 16 * 4),
            Coordinate = new RectangleF(0, 45, 45, 16)
        },
        new RenderObject()
        {
            Location = new PointF(0 + 45 * 4, 555 + 45 * 4),
            Size = new SizeF(16 * 4, 16 * 4),
            Coordinate = new RectangleF(45, 45, 16, 16)
        },
        new RenderObject()
        {
            Location = new PointF(0 + 45 * 4 + 16 * 4, 555 + 45 * 4),
            Size = new SizeF(16 * 4, 16 * 4),
            Coordinate = new RectangleF(45, 45, 16, 16)
        },
        new RenderObject()
        {
            Location = new PointF(0 + 45 * 4 + 16 * 4 * 2, 555 + 45 * 4),
            Size = new SizeF(16 * 4, 16 * 4),
            Coordinate = new RectangleF(45, 45, 16, 16)
        },
        new RenderObject()
        {
            Location = new PointF(0 + 45 * 4 + 16 * 4 * 3, 555 + 45 * 4),
            Size = new SizeF(16 * 4, 16 * 4),
            Coordinate = new RectangleF(45, 45, 16, 16)
        },
        new RenderObject()
        {
            Location = new PointF(0 + 45 * 4 + 16 * 4 * 4, 555 + 45 * 4),
            Size = new SizeF(16 * 4, 16 * 4),
            Coordinate = new RectangleF(45, 45, 16, 16)
        },
        new RenderObject()
        {
            Location = new PointF(0 + 45 * 4 + 16 * 4 * 5, 555 + 45 * 4),
            Size = new SizeF(16 * 4, 16 * 4),
            Coordinate = new RectangleF(45, 45, 16, 16)
        },
        new RenderObject()
        {
            Location = new PointF(0 + 45 * 4 + 16 * 4 * 6, 555 + 45 * 4),
            Size = new SizeF(16 * 4, 16 * 4),
            Coordinate = new RectangleF(45, 45, 16, 16)
        },
        new RenderObject()
        {
            Location = new PointF(0 + 45 * 4 + 16 * 4 * 7, 555 + 45 * 4),
            Size = new SizeF(16 * 4, 16 * 4),
            Coordinate = new RectangleF(45, 45, 16, 16)
        },
        new RenderObject()
        {
            Location = new PointF(0 + 45 * 4 + 16 * 4 * 8, 555 + 45 * 4),
            Size = new SizeF(16 * 4, 16 * 4),
            Coordinate = new RectangleF(45, 45, 16, 16)
        },
        new RenderObject()
        {
            Location = new PointF(0 + 45 * 4 + 16 * 4 * 9, 555 + 45 * 4),
            Size = new SizeF(16 * 4, 16 * 4),
            Coordinate = new RectangleF(45, 45, 16, 16)
        },
        new RenderObject()
        {
            Location = new PointF(0 + 45 * 4 + 16 * 4 * 10, 555 + 45 * 4),
            Size = new SizeF(16 * 4, 16 * 4),
            Coordinate = new RectangleF(45, 45, 16, 16)
        },
        new RenderObject()
        {
            Location = new PointF(0 + 45 * 4 + 16 * 4 * 11, 555 + 45 * 4),
            Size = new SizeF(16 * 4, 16 * 4),
            Coordinate = new RectangleF(45, 45, 16, 16)
        },
        new RenderObject()
        {
            Location = new PointF(0 + 45 * 4 + 16 * 4 * 12, 555 + 45 * 4),
            Size = new SizeF(16 * 4, 16 * 4),
            Coordinate = new RectangleF(45, 45, 16, 16)
        },
        new RenderObject()
        {
            Location = new PointF(0 + 45 * 4 + 16 * 4 * 13, 555 + 45 * 4),
            Size = new SizeF(44 * 4, 16 * 4),
            Coordinate = new RectangleF(45 + 16, 45, 44, 16)
        }
    };

    private int _uniformViewPort;

    protected override void OnLoad()
    {
        base.OnLoad();


        GL.ClearColor(Color.FromArgb(96, 80, 128));

        foreach (var renderObject in _renderObjects)
        {
            renderObject.OnLoad(this.Shader);
        }

        this._uniformViewPort = GL.GetUniformLocation(this.Shader.ProgramHandle, "aViewport");
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        foreach (var renderObject in _renderObjects)
        {
            renderObject.OnRenderFrame(this.Shader);
        }

        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        if (this.KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        // When the window gets resized, we have to call GL.Viewport to resize OpenGL's viewport to match the new size.
        // If we don't, the NDC will no longer be correct.
        GL.Viewport(0, 0, Size.X, Size.Y);
        GL.Uniform3(this._uniformViewPort, this.Size.X, this.Size.Y, 1.0f);
    }

    protected override void OnUnload()
    {
        GL.UseProgram(Shader.ProgramHandle);
        // Unbind all the resources by binding the targets to 0/null.
        foreach (var renderObject in _renderObjects)
        {
            renderObject.OnUnload();
        }

        GL.DeleteProgram(Shader.ProgramHandle);

        base.OnUnload();
    }
}




